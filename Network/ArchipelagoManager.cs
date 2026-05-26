using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.Enums;
using SplasherArchipelago.Data.Locations;
using System;

namespace SplasherArchipelago.Network {
    static class ArchipelagoManager {
        private static string domain = "localhost";
        private static int port = 38281;
        private static string player = "Frisk";
        private static Version version = new Version(0, 6, 7);
        public static int? Slot = null;

        private static ArchipelagoSession session;

        internal static LoginResult Init() {
            session = ArchipelagoSessionFactory.CreateSession(domain, port);

            session.Items.ItemReceived += (recvItemHelper) => {
                Items.ItemManager.Collect(recvItemHelper.DequeueItem());
            };

            return session.TryConnectAndLogin(
                Util.Game, player, ItemsHandlingFlags.AllItems,
                version, null, null, null, true
            );
        }

        internal static void ReceiveAllItems() {
            if (session is null) return;

            foreach (var item in session.Items.AllItemsReceived) {
                Items.ItemManager.Collect(item);
            }
        }

        internal static void RestoreCheckedLocations() {
            if (session is null) return;

            foreach(var loc in session.Locations.AllLocationsChecked) {
                int id = (int)(loc - Util.BaseId);
                if (id < 0) continue;

                var type = LocationExtensions.FindRange(id);

                Console.WriteLine(type);
                Console.WriteLine(id);
                Console.WriteLine(id - (int)type);

                switch (type) {
                    case LocationType.Water: Powers.CheckWater(); break;
                    case LocationType.Stickink: Powers.CheckStickink(); break;
                    case LocationType.Bouncink: Powers.CheckBouncink(); break;
                    case LocationType.Splasher: Splashers.Check(id - (int)LocationType.Splasher); break;
                    case LocationType.Clear: LocationOnEachLevel.Clears.Check(id - (int)LocationType.Clear); break;
                    case LocationType.Bronze: LocationOnEachLevel.Bronzes.Check(id - (int)LocationType.Clear); break;
                    case LocationType.Silver: LocationOnEachLevel.Silvers.Check(id - (int)LocationType.Clear); break;
                    case LocationType.Gold: LocationOnEachLevel.Golds.Check(id - (int)LocationType.Clear); break;
                }
            }
        }

        internal static void Check(LocationType loc, long id) {
            session.Locations.CompleteLocationChecks(Util.BaseId + (int)loc + id);
        }
    }
}