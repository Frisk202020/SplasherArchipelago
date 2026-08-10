using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.BounceFeatures.DeathLink;
using Archipelago.MultiClient.Net.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Archipelago.Network.Helpers {
    class FailableSession {
        private const int TIMEOUT_MS = 5000;

        private readonly ArchipelagoSession session;
        private readonly string player;
        private readonly string password;
        private readonly Version version;
        private readonly string uuid = Guid.NewGuid().ToString();

        // Stores failed checks if the game is later reconnected
        // If the game is closed, locations are lost and need to be checked again
        private readonly Queue<Action<ArchipelagoSession>> pendingEvents = new Queue<Action<ArchipelagoSession>>();
        private readonly Address proxyTarget;
        private readonly BackgroundThread connexionThread;
        private bool firstConnectionDone = false;
        internal bool Connected { get; private set; } = false;
        private int slot;

        private BackgroundThread deathLinkThread;

        public FailableSession(ArchipelagoSession session, string player, string password, Version version, Address proxyTarget) {
            this.session = session;
            this.player = player;
            this.version = version;
            this.proxyTarget = proxyTarget;
            this.password = string.IsNullOrEmpty(password) ? null : password;

            session.Socket.SocketOpened += () => {
                Core.Static.Log("Connected to Archipelago !");
                Connected = true;
                if (!firstConnectionDone) firstConnectionDone = true;
            };
            session.Socket.ErrorReceived += (exception, message) => {
                Core.Static.Error($"Internal Error: {message}\n{exception}");
            };
            session.Socket.SocketClosed += (reason) =>{
                Core.Static.Warn($"Connexion closed{(string.IsNullOrEmpty(reason) ? "." : $": {reason}")}");
                Connected = false;
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
                uuid: uuid,
                password: password
            );

            if (res is LoginFailure error) {
                string msg = $"Failed to connect to server\n";
                foreach (string err in error.Errors) {
                    msg += $"{err}\n";
                }

                Core.Static.Error(msg);
                return false;
            }

            slot = session.ConnectionInfo.Slot;
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

        internal Queue<HintInfo> GetPendingHints() {
            var hints = session.DataStorage.GetHints();
            if (hints is null) return new Queue<HintInfo>();

            return new Queue<HintInfo>(
                hints.Where(hint => !hint.Found && hint.FindingPlayer == slot).Select(hint => {
                    var receiver = session.Players.AllPlayers.FirstOrDefault(x => x.Slot == hint.ReceivingPlayer);
                    
                    return new HintInfo(
                        session.Locations.GetLocationNameFromId(hint.LocationId),  
                        receiver?.Name, 
                        receiver is null ? null : session.Items.GetItemName(hint.ItemId, receiver.Game),
                        hint.ReceivingPlayer == slot
                    );
                })
            );
        }

        // returns slot data to apply options that needs save loaded later
        internal Dictionary<string, object> ApplyOptions(Core.Tools.Config conf) {
            var data = session.DataStorage.GetSlotData();
            Util.Seed = (string)data["seed"];

            Data.Items.Splashers.Goal = (int)(long)data["splashers_goal"];
            Data.Items.Powers.ProgressiveWater = (long)data["progressive_water"] == 1;
            Data.Locations.Speedrun.SetHighestMedal((Medal)(long)data["include_medals"]);

            var deathLink = conf.DeathLinkOverride.Value > 0 
                ? conf.DeathLinkOverride.Value
                : (long)data["death_link"];
            if (deathLink > 0) ApplyDeathLink((uint)deathLink);
            
            bool? heroOverride = null;
            switch (conf.HeroModeOverride.Value) {
                case "true": heroOverride = true; break;
                case "false": heroOverride = false; break;
                case "":
                case "null": break;
                default: Core.Static.Warn($"Unrecognized Hero Override option : {conf.HeroModeOverride.Value}"); break;
            }

            if (
                heroOverride == true ||
                (heroOverride == null && (long)data["hero_mode"] == 1)
            ) {
                Data.Death.SetHero();
            }

            Data.PendingKeyUnlock.Mode = (Data.PendingKeyUnlock.KeyMode)(long)data["include_keys"];
            if (Data.PendingKeyUnlock.Mode > 0 && (long)data["include_speedrun_keys"] == 1) {
                Data.SaveData.EnableTimeAttackDoors = true;
                Data.PendingKeyUnlock.SpeedrunKeys = true;
            }

            return data;
        }

        private void ApplyDeathLink(uint trigger) {
            Data.Death.DeathLinkAmnesty = trigger;

            var deathLinkService = session.CreateDeathLinkService();
            deathLinkService.EnableDeathLink();
            deathLinkService.OnDeathLinkReceived += Data.Death.ReceiveDeathLink;

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
