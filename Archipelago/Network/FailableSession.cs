using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Exceptions;
using System;
using System.Collections.Generic;
using System.Threading;

namespace SplasherArchipelago.Network {
    class FailableSession {
        private const uint AUTO_RETRY_COUNT = 2;
        private const int AUTO_RETRY_DELAY = 5000;

        private readonly ArchipelagoSession session;
        private readonly string player;
        private readonly Version version;

        // Stores failed checks if the game is later reconnected
        // If the game is closed, locations are lost and need to be checked again
        private readonly Queue<Action<ArchipelagoSession>> pendingEvents = new Queue<Action<ArchipelagoSession>>();

        public FailableSession(ArchipelagoSession session, string player, Version version) {
            this.session = session;
            this.player = player;
            this.version = version;
        }

        public LoginResult Connect() {
            return session.TryConnectAndLogin(
                game: Util.Game,
                name: player,
                itemsHandlingFlags: ItemsHandlingFlags.AllItems,
                version: version,
                requestSlotData: false
            );
        }

        public void Execute(Action<ArchipelagoSession> callback) {
            var success = false;
            for (uint i = 0; i < AUTO_RETRY_COUNT; i++) {
                try {
                    callback(session);
                    success = true;
                    break;
                } catch (ArchipelagoSocketClosedException) {
                   var result = session.TryConnectAndLogin(
                        game: Util.Game,
                        name: player,
                        itemsHandlingFlags: ItemsHandlingFlags.AllItems,
                        version: version,
                        requestSlotData: false
                    );

                    if (result is LoginSuccessful) {
                        callback(session);
                        return;
                    }

                    if (i < AUTO_RETRY_COUNT - 1) {
                        Console.WriteLine("Failed to reconnect to server, attempting again in a moment...");
                        Thread.Sleep(AUTO_RETRY_DELAY);
                    }
                } catch {
                    pendingEvents.Enqueue(callback);
                    Console.WriteLine("Failed to reconnect to server, try to reconnect manually...");
                    return;
                }
            }

            if (!success) {
                Console.WriteLine("Failed to reconnect to server, try to reconnect manually...");
                return;
            }

            while (pendingEvents.Count > 0) {
                var pending = pendingEvents.Dequeue();
                try {
                    pending(session);
                } catch {
                    pendingEvents.Enqueue(pending);
                    Console.WriteLine("Failed to check pending locations, try to reconnect manually...");
                    return;
                }
            }
        }
    }
}
