using Archipelago.MultiClient.Net;
using SplasherArchipelago.Data.Locations;
using System;
using System.IO;

namespace SplasherArchipelago.Network {
    static class ArchipelagoManager {
        private static readonly Version version = new Version(0, 6, 7);
        private static Helpers.FailableSession session;

        internal static bool Start() {
            try {
                if (session is null) {
                    if (Shared.Config is null) {
                        Shared.Parse();
                        if (Shared.Config is null) return false;
                    }

                    if (Shared.Config.ShowLevelTitle.Value) {
                        Patches.Controller.Hub.Door.ShowName = true;    
                    }

                    var targetAddress = new Helpers.Address { domain = Shared.Config.Address.Value, port = (int)Shared.Config.Port.Value };
                    var session = ArchipelagoSessionFactory.CreateSession(Shared.Config.Proxy.Value ? $"ws://localhost:8080" : targetAddress.ToString());

                    session.Items.ItemReceived += (recvItemHelper) => {
                        Items.ItemManager.Collect(recvItemHelper.DequeueItem());
                    };

                    ArchipelagoManager.session = new Helpers.FailableSession(
                        session, Shared.Config.Slot.Value, version,
                        Shared.Config.Proxy.Value ? targetAddress : null
                    );
                }
                if (!session.FirstConnection()) return false;
                Util.Log("Archipelago Loaded !");

                ApplyOptions();
                RestoreCheckedLocations();
                return true;
            } catch (Exception e) {
                Util.Error($"Failed to initialize Archipelago : {e.Message}");
                return false;
            }
        }

        private static void ApplyOptions() {
            session.ApplyOptions();
        }

        public static void RestoreCheckedLocations() {
            session.Execute((session) => {
                foreach (var loc in session.Locations.AllLocationsChecked) {
                    int id = (int)(loc - Util.BaseId);
                    if (id < 0) continue;

                    var type = LocationExtensions.FindRange(id);
                    switch (type) {
                        case LocationType.Water: Powers.RestoreWater(); break;
                        case LocationType.Stickink: Powers.RestoreStickink(); break;
                        case LocationType.Bouncink: Powers.RestoreBouncink(); break;
                        case LocationType.Splasher: Splashers.Check(id - (int)LocationType.Splasher); break;
                        case LocationType.Clear: LocationOnEachLevel.Clears.Check(id - (int)LocationType.Clear); break;
                        case LocationType.Bronze: LocationOnEachLevel.Bronzes.Check(id - (int)LocationType.Bronze); break;
                        case LocationType.Silver: LocationOnEachLevel.Silvers.Check(id - (int)LocationType.Silver); break;
                        case LocationType.Gold: LocationOnEachLevel.Golds.Check(id - (int)LocationType.Gold); break;
                        case LocationType.Platinum: LocationOnEachLevel.Platinums.Check(id - (int)LocationType.Platinum); break;
                    }
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