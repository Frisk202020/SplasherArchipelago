using Archipelago.MultiClient.Net.Models;

namespace SplasherArchipelago.Network.Items {
    public static class ItemManager {
        private static Item[] orderedItems = {
            new Victory(), new Splasher(),
            new Powers.Water(), new Powers.Stickink(), new Powers.Bouncink()
        };

        public static void Collect(ItemInfo item) {
            var id = item.ItemId - Util.BaseId;
            if (id >= 0 && id < orderedItems.Length) orderedItems[id].Collect(item); // need to substract game's base id
        }
    }
}