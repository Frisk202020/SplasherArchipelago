using Archipelago.MultiClient.Net.Models;
using System.Collections.Generic;

namespace Archipelago.Network.Items {
    static class ItemManager {
        private static readonly HashSet<long> collectedLocationIds = new HashSet<long>();
        private static readonly Queue<ItemInfo> pending = new Queue<ItemInfo>();

        private static List<Item> OrderedItems() {
            var items = new List<Item> {
                new Freedom(), new Splasher(), new Powers.Progressive(), new Powers.ProgressiveWater(),
                new Powers.Water(), new Powers.Stickink(), new Powers.Bouncink(),
                new Fillers.Nothing(), new Fillers.Nothing(), new Fillers.Nothing(),
                new Traps.PaintSwap(), new Traps.MadGun(), new Traps.Feet()
            };

            foreach (int n in new int[] { 
                1, 2, 5, 10, 15, 20, 25, 30, 40, 50,
                -1, -2, -3, -5, -10, -15, -20, -25 
            }) {
                items.Add(new Essence(n));
            }

            Key.AddAll(items, false);
            ZoneKey.AddAll(items, false);
            Key.AddAll(items, true);
            ZoneKey.AddAll(items, true);

            return items;
        }
        private static readonly List<Item> orderedItems = OrderedItems();

        internal static void Enqueue(ItemInfo item) {
            pending.Enqueue(item);
        }

        internal static void CollectPending() {
            while (pending.Count > 0) {
                Collect(pending.Dequeue());
            }
        }

        internal static void Collect(ItemInfo item) {
            // -1 is for cheat console
            if (item.LocationId > -1) {
                if (collectedLocationIds.Contains(item.LocationId)) return;
                collectedLocationIds.Add(item.LocationId);
            }

            var id = item.ItemId - Util.BaseId;
            if (id >= 0 && id < orderedItems.Count) {
                var splasherItem = orderedItems[(int)id];

                splasherItem.Collect(item);
            } 
        }
    }
}