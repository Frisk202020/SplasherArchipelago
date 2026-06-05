using Archipelago.MultiClient.Net;
using SplasherArchipelago.Data.Locations;
using System;
using System.IO;

namespace SplasherArchipelago.Network {
    static class ArchipelagoManager {
        private static string domain = "archipelago.gg";
        private static int port = 0;
        private static string player = "Splasher";
        private static bool proxy = false;

        private static readonly Version version = new Version(0, 6, 7);
        public static int? Slot = null;

        private static Helpers.FailableSession session;

        internal static bool Start() {
            if (session is null) {
                try {
                    Parse();
                } catch (Exception ex) {
                    Util.Error($"Failed to parse connection info : Check your connection file.\nDetails : {ex.Message}");
                    return false;
                }

                var targetAddress = new Helpers.Address { domain = domain, port = port };
                var session = ArchipelagoSessionFactory.CreateSession(proxy ? $"ws://localhost:8080" : targetAddress.ToString());

                session.Items.ItemReceived += (recvItemHelper) => {
                    Items.ItemManager.Collect(recvItemHelper.DequeueItem());
                };

                ArchipelagoManager.session = new Helpers.FailableSession(session, player, version, proxy ? targetAddress : null);
            }

            var connectResult = session.Connect(true);

            if (connectResult is LoginFailure error) {
                string msg = $"Failed to connect to server\n";
                foreach (string err in error.Errors)
                {
                    msg += $"{err}\n";
                }

                Util.Error(msg);
                return false;
            }

            var success = (LoginSuccessful)connectResult;
            Slot = success.Slot;

            Data.Items.LevelKeys.UnlockAll();
            Util.Log("Archipelago Loaded !");

            ApplyOptions();
            RestoreCheckedLocations();
            return true;
        }

        // naive implementation : need to integrate to game's UI
        private static void Parse() {
            var sr = new StreamReader("connection.yaml");
            var line = sr.ReadLine();
            while (line != null) {
                var lineArr = line.Split(':');

                if (lineArr.Length > 1) {
                    var name = lineArr[0].Trim(' ');
                    var value = lineArr[1].Trim(' ');

                    switch(name) {
                        case "address":
                            domain = value; break;
                        case "port":
                            port = int.Parse(value); break;
                        case "slot":
                            player = value; break;
                        case "proxy":
                            proxy = value == "true"; break;
                    }
                }

                line = sr.ReadLine();
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
                        case LocationType.Bronze: LocationOnEachLevel.Bronzes.Check(id - (int)LocationType.Clear); break;
                        case LocationType.Silver: LocationOnEachLevel.Silvers.Check(id - (int)LocationType.Clear); break;
                        case LocationType.Gold: LocationOnEachLevel.Golds.Check(id - (int)LocationType.Clear); break;
                    }
                }
            });
        }

        internal static void Reconnect() {
            session.Connect();
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