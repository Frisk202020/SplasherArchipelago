using Archipelago.MultiClient.Net.Models;
using System;
using System.Collections.Generic;

namespace SplasherArchipelago.Network.Items {
    static class ItemManager {
        private static readonly HashSet<long> collectedLocationIds = new HashSet<long>(); 

        private static List<Item> OrderedItems() {
            var items = new List<Item> {
                new Freedom(), new Splasher(), new Powers.Progressive(),
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
        private static readonly List<Item> orderedItems = OrderedItems();

        internal static void Collect(ItemInfo item, bool isStartup=false) {
            if (collectedLocationIds.Contains(item.LocationId)) return;
            collectedLocationIds.Add(item.LocationId);

            var id = item.ItemId - Util.BaseId;
            if (id >= 0 && id < orderedItems.Count) {
                var splasherItem = orderedItems[(int)id];
                Console.WriteLine($"{splasherItem.Name()}, startup={isStartup}, collect={splasherItem.CollectOnStart()}");
                if (isStartup && !splasherItem.CollectOnStart()) return;

                splasherItem.Collect(item);
            } 
        }
    }
}