using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.BounceFeatures.DeathLink;
using Archipelago.MultiClient.Net.Enums;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Archipelago.Network.Helpers {
    class FailableSession {
        private const int TIMEOUT_MS = 5000;

        private readonly ArchipelagoSession session;
        private readonly string player;
        private readonly Version version;
        private readonly string uuid = Guid.NewGuid().ToString();

        // Stores failed checks if the game is later reconnected
        // If the game is closed, locations are lost and need to be checked again
        private readonly Queue<Action<ArchipelagoSession>> pendingEvents = new Queue<Action<ArchipelagoSession>>();
        private readonly Address proxyTarget;
        private readonly BackgroundThread connexionThread;
        private bool firstConnectionDone = false;

        private BackgroundThread deathLinkThread;

        public FailableSession(ArchipelagoSession session, string player, Version version, Address proxyTarget) {
            this.session = session;
            this.player = player;
            this.version = version;
            this.proxyTarget = proxyTarget;

            session.Socket.SocketOpened += () => {
                Core.Static.Log("Connected to Archipelago !");
                if (!firstConnectionDone) firstConnectionDone = true;
            };
            session.Socket.ErrorReceived += (exception, message) => {
                Core.Static.Error($"Internal Error: {message}\n{exception}");
            };
            session.Socket.SocketClosed += (reason) =>{
                Core.Static.Warn($"Connexion closed{(string.IsNullOrEmpty(reason) ? "." : $": {reason}")}");
                if (firstConnectionDone) connexionThread.Execute();
            };

            connexionThread = new BackgroundThread("Connexion Worker", () => {
                while (true) {
                    if (Connect()) {
                        return;
                    }

                    Thread.Sleep(TIMEOUT_MS);
                }
            });
        }

        public bool FirstConnection() {
            return Connect(true);
        }

        private bool Connect(bool requestSlotData = false) {
            if (proxyTarget != null && !ProxyManager.Init(proxyTarget)) return false;

            Core.Static.Log($"Trying to connect to Archipelago Server...");
            var res = session.TryConnectAndLogin(
                game: Core.Static.Game,
                name: player,
                itemsHandlingFlags: ItemsHandlingFlags.AllItems,
                version: version,
                requestSlotData: requestSlotData,
                uuid: uuid
            );

            if (res is LoginFailure error) {
                string msg = $"Failed to connect to server\n";
                foreach (string err in error.Errors) {
                    msg += $"{err}\n";
                }

                Core.Static.Error(msg);
                return false;
            }

            while (pendingEvents.Count > 0) {
                var pending = pendingEvents.Dequeue();
                try {
                    pending(session);
                }
                catch {
                    pendingEvents.Enqueue(pending);
                    return false;
                }
            }

            return true;
        }

        // returns slot data to apply options that needs save loaded later
        public Dictionary<string, object> ApplyOptions() {
            var data = session.DataStorage.GetSlotData();

            Util.Seed = (string)data["seed"];

            Data.Items.Splashers.Goal = (int)(long)data["splashers_goal"];
            Data.Locations.Speedrun.SetHighestMedal((Medal)(long)data["include_medals"]);

            ApplyDeathLink((Options.DeathLink)(long)data["death_link"]);

            if ((long)data["hero_mode"] == 1) {
                Data.DeathLink.SetHero();
            }

            Data.PendingKeyUnlock.Mode = (Data.PendingKeyUnlock.KeyMode)(long)data["include_keys"];
            if (Data.PendingKeyUnlock.Mode > 0 && (long)data["include_speedrun_keys"] == 1) {
                Data.SaveData.EnableTimeAttackDoors = true;
                Data.PendingKeyUnlock.SpeedrunKeys = true;
            }

            return data;
        }

        private void ApplyDeathLink(Options.DeathLink option) {
            switch (option) {
                case Options.DeathLink.Normal: Data.DeathLink.Trigger = 4; break;
                case Options.DeathLink.Insane: Data.DeathLink.Trigger = 2; break;
                case Options.DeathLink.Legend: Data.DeathLink.Trigger = 0; break;
                default: return;
            }

            var deathLinkService = session.CreateDeathLinkService();
            deathLinkService.EnableDeathLink();
            deathLinkService.OnDeathLinkReceived += Data.DeathLink.ReceiveDeathLink;

            deathLinkThread = new BackgroundThread("DeathLink Worker", () => deathLinkService.SendDeathLink(new DeathLink(player)));
        }

        public void Execute(Action<ArchipelagoSession> callback) {
            try {
                callback(session);
            } catch {
                pendingEvents.Enqueue(callback);
                return;
            }
        }

        internal void SendDeathLink() {
            if (deathLinkThread is null) return;

            deathLinkThread.Execute();
        }
    }
}
