using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.Enums;
using System;

namespace SplasherArchipelago.Network {
    public static class ArchipelagoManager {
        private static string domain = "localhost";
        private static int port = 38281;
        private static string player = "Frisk";
        private static Version version = new Version(0, 6, 7);
        public static int? Slot = null;

        private static ArchipelagoSession session;

        public static LoginResult Init() {
            session = ArchipelagoSessionFactory.CreateSession(domain, port);
            session.Items.ItemReceived += (recvItemHelper) => {
                Items.ItemManager.Collect(recvItemHelper.DequeueItem());
            };

            return session.TryConnectAndLogin(
                Util.Game, player, ItemsHandlingFlags.AllItems,
                version, null, null, null, true
            );
        }

        public static void ReceiveAllItems() {
            if (session is null) return;

            foreach (var item in session.Items.AllItemsReceived) {
                Items.ItemManager.Collect(item);
            }
        }
    }
}