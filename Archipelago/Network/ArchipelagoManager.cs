using Archipelago.MultiClient.Net;
using SplasherArchipelago.Data.Locations;
using SplasherArchipelago.Network.Options;
using System;
using System.IO;

namespace SplasherArchipelago.Network {
    static class ArchipelagoManager {
        private static string domain = "archipelago.gg";
        private static int port = 0;
        private static string player = "Splasher";
        private static Version version = new Version(0, 6, 7);
        public static int? Slot = null;

        private static FailableSession session;

        internal static bool Start() {
            if (session is null) {
                try {
                    Parse();
                } catch (Exception ex) {
                    Console.WriteLine($"Parse Error : check your connection file. Error is the following : {ex.Message}");
                    return false;
                }

                Console.WriteLine($"Attempting to connect to : {domain}:{port} as {player}");
                var session = ArchipelagoSessionFactory.CreateSession(domain, port);

                session.Items.ItemReceived += (recvItemHelper) => {
                    Items.ItemManager.Collect(recvItemHelper.DequeueItem());
                };

                ArchipelagoManager.session = new FailableSession(session, player, version);
            }

            var connectResult = session.Connect();

            if (connectResult is LoginFailure error) {
                string msg = $"Failed to connect to server\n";
                foreach (string err in error.Errors)
                {
                    msg += $"{err}\n";
                }

                Console.WriteLine(msg);
                return false;
            }

            var success = (LoginSuccessful)connectResult;
            Slot = success.Slot;

            Data.Items.LevelKeys.UnlockAll();
            Console.WriteLine("Archipelago Loaded !");

            ApplyOptions();
            RestoreCheckedLocations();
            return true;
        }

        // naive implementation : need to integrate to game's UI
        private static void Parse() {
            var sr = new StreamReader("connection.md");
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
                    }
                }

                line = sr.ReadLine();
            }
        }

        private static void ApplyOptions() {
            session.Execute((session) => {
                var data = session.DataStorage.GetSlotData<ArchipelagoOptions>();
                data.Apply();
            });
        }

        private static void RestoreCheckedLocations() {
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

        internal static void Victory() {
            session.Execute(session => session.SetGoalAchieved());
        }
    }
}