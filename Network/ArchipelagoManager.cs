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

        public static LoginResult init() {
            var session = ArchipelagoSessionFactory.CreateSession(domain, port);
            session.Items.ItemReceived += (recvItemHelper) => {
                var item = recvItemHelper.PeekItem();
                if (item.Player.Slot == Slot || item.Player.Name == player) {
                    Items.ItemManager.Collect(item);
                }
            };

            return session.TryConnectAndLogin(
                Util.Game, player, ItemsHandlingFlags.AllItems,
                version, null, null, null, true
            );
        }
    }
}