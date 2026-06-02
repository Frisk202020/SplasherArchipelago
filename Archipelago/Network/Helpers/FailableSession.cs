using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Exceptions;
using System;
using System.Collections.Generic;

namespace SplasherArchipelago.Network.Helpers {
    class FailableSession {
        private const uint AUTO_RETRY_COUNT = 2;

        private readonly ArchipelagoSession session;
        private readonly string player;
        private readonly Version version;
        private readonly string uuid = Guid.NewGuid().ToString();

        // Stores failed checks if the game is later reconnected
        // If the game is closed, locations are lost and need to be checked again
        private readonly Queue<Action<ArchipelagoSession>> pendingEvents = new Queue<Action<ArchipelagoSession>>();
        private readonly Address proxyTarget;

        public FailableSession(ArchipelagoSession session, string player, Version version, Address proxyTarget) {
            this.session = session;
            this.player = player;
            this.version = version;
            this.proxyTarget = proxyTarget;

        }

        public LoginResult Connect(bool requestSlotData = false) {
            if (proxyTarget != null && !ProxyManager.Init(proxyTarget)) return null;

            Util.Log($"Trying to connect to Archipelago Server");
            return session.TryConnectAndLogin(
                game: Util.Game,
                name: player,
                itemsHandlingFlags: ItemsHandlingFlags.AllItems,
                version: version,
                requestSlotData: requestSlotData,
                uuid: uuid
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
                    var result = Connect();

                    if (result is LoginSuccessful) {
                        callback(session);
                        return;
                    }

                    if (i < AUTO_RETRY_COUNT - 1) {
                        Util.Warn("Failed to reconnect to server, attempting again in a moment...");
                    }
                } catch {
                    pendingEvents.Enqueue(callback);
                    Util.Error("Failed to reconnect to server, try to reconnect manually...");
                    return;
                }
            }

            if (!success) {
                Util.Error("Failed to reconnect to server, try to reconnect manually...");
                return;
            }

            while (pendingEvents.Count > 0) {
                var pending = pendingEvents.Dequeue();
                try {
                    pending(session);
                } catch {
                    pendingEvents.Enqueue(pending);
                    Util.Error("Failed to check pending locations, try to reconnect manually...");
                    return;
                }
            }
        }
    }
}
