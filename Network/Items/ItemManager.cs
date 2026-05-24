using Archipelago.MultiClient.Net.Models;
using System.Collections.Generic;

namespace SplasherArchipelago.Network.Items {
    public static class ItemManager {
        private static List<Item> OrderedItems() {
            var items = new List<Item> {
                new Victory(), new Splasher(),
                new Powers.Water(), new Powers.Stickink(), new Powers.Bouncink(),
                new Fillers.JobPromotion(),
                new Traps.PaintSwap(), new Traps.BodyAches(), // new Traps.Antiwater(), 
            };

            foreach (uint n in new uint[] { 1, 10, 25, 50 }) {
                items.Add(new Essence(n));
            }
            Key.AddAll(items);

            return items;
        }
        private static List<Item> orderedItems = OrderedItems();

        public static void Collect(ItemInfo item) {
            var id = item.ItemId - Util.BaseId;
            if (id >= 0 && id < orderedItems.Count) orderedItems[(int)id].Collect(item); // need to substract game's base id
        }
    }
}