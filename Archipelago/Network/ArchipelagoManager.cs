using Archipelago.MultiClient.Net;
using SplasherArchipelago.Data.Locations;
using System;
using System.Collections.Generic;

namespace SplasherArchipelago.Network {
    static class ArchipelagoManager {
        private static readonly Version version = new Version(0, 6, 7);
        private static Helpers.FailableSession session;

        private static Dictionary<string, object> slotData;

        internal static bool SaveLoaded { get; private set; } = false;
        internal static void FinalizeSaveLoading() {
            SaveLoaded = true;

            if ((long)slotData["include_keys"] == 0) {
                Data.Items.LevelKeys.UnlockAll();
            } else {
                Data.Items.LevelKeys.UnlockFirst();
            }

            RestoreCheckedLocations();
            Items.ItemManager.CollectPending();
        }

        internal static bool Start() {
            try {
                if (session is null) {
                    if (Shared.Config is null) {
                        Shared.Parse();
                        if (Shared.Config is null) return false;
                    }

                    if (Shared.Config.ShowLevelTitle.Value) {
                        Data.Items.LevelKeys.ShowName = true;    
                    }

                    var targetAddress = new Helpers.Address { domain = Shared.Config.Address.Value, port = (int)Shared.Config.Port.Value };
                    var session = ArchipelagoSessionFactory.CreateSession(Shared.Config.Proxy.Value ? $"ws://localhost:8080" : targetAddress.ToString());

                    session.Items.ItemReceived += (recvItemHelper) => {
                        if (SaveLoaded) {
                            Items.ItemManager.Collect(recvItemHelper.DequeueItem());
                        } else {
                            Items.ItemManager.Enqueue(recvItemHelper.DequeueItem());
                        }
                    };

                    session.Locations.CheckedLocationsUpdated += (recvLocHelper) => {
                        foreach(var loc in recvLocHelper) {
                            Restore(loc);
                        }
                    };

                    ArchipelagoManager.session = new Helpers.FailableSession(
                        session, Shared.Config.Slot.Value, version,
                        Shared.Config.Proxy.Value ? targetAddress : null
                    );
                }
                if (!session.FirstConnection()) return false;
                Util.Log("Archipelago Loaded !");

                slotData = ApplyOptions();
                Data.SaveData.Init();

                return true;
            } catch (Exception e) {
                Util.Error($"Failed to initialize Archipelago : {e.Message}");
                return false;
            }
        }

        private static Dictionary<string, object> ApplyOptions() {
            return session.ApplyOptions();
        }

        private static void Restore(long locId) {
            int id = (int)(locId - Util.BaseId);
            if (id < 0) return;

            var type = LocationExtensions.FindRange(id);
            switch (type) {
                case LocationType.Water: Powers.RestoreWater(); break;
                case LocationType.Stickink: Powers.RestoreStickink(); break;
                case LocationType.Bouncink: Powers.RestoreBouncink(); break;
                case LocationType.Splasher: Splashers.Restore(id); break;
                case LocationType.Clear: Clears.Restore(id); break;
                case LocationType.Bronze: Speedrun.Restore(LocationType.Bronze, id); break;
                case LocationType.Silver: Speedrun.Restore(LocationType.Silver, id); break;
                case LocationType.Gold: Speedrun.Restore(LocationType.Gold, id); break;
                case LocationType.Platinum: Speedrun.Restore(LocationType.Platinum, id); break;
            }
        }

        public static void RestoreCheckedLocations() {
            session.Execute((session) => {
                foreach (var loc in session.Locations.AllLocationsChecked) {
                    Restore(loc);
                }
            });
        }

        internal static void Check(LocationType loc, long id) {
            session.Execute(session => session.Locations.CompleteLocationChecks(Util.BaseId + (int)loc + id));
        }

        internal static void SendDeathLink() {
            session.SendDeathLink();
        }

        internal static void Victory() {
            session.Execute(session => session.SetGoalAchieved());
        }
    }
}