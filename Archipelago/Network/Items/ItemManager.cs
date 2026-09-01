using Archipelago.MultiClient.Net.Models;
using System.Collections.Generic;

namespace Archipelago.Network.Items {
    static class ItemManager {
        private class EssenceInfo {
            public readonly int amount;
            public readonly string name;
            public EssenceInfo(int _amount, string _name) {
                amount = _amount; name = _name;
            } 
        }

        private static readonly HashSet<long> collectedLocationIds = new HashSet<long>();
        private static readonly Queue<ItemInfo> pending = new Queue<ItemInfo>();
        internal static void AddCollected(List<long> locIds) => collectedLocationIds.UnionWith(locIds);

        private static List<Item> OrderedItems() {
            var items = new List<Item> {
                new Freedom(), new Splasher(), new Powers.Progressive(), new Powers.ProgressiveWater(),
                new Powers.Water(), new Powers.Stickink(), new Powers.Bouncink(),
                new Nothing("Job"), new Nothing("Autograph"), new Nothing("Ticket"),
                new Traps.PaintSwap(), new Traps.MadGun(), new Traps.Feet()
            };

            foreach (var i in new[] {
                1, 2, 5, 10, 15, 20, 25, 30, 40, 50,
                -1, -2, -3, -5, -10, -15, -20, -25
            }) {
                items.Add(new Essence(i));
            }

            Key.AddAll(items, false);
            ZoneKey.AddAll(items, false);
            Key.AddAll(items, true);
            ZoneKey.AddAll(items, true);

            Checkpoint.AddAll(items);
            CheckpointLevel.AddAll(items);

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

                bool save = false;
                if (!Data.SaveData.WeakCollectedItems.Contains(item.LocationId)) {
                    Data.SaveData.WeakCollectedItems.Add(item.LocationId);
                    Data.UI.Tracker.AddItemReceived(
                        splasherItem, 
                        item.Flags,
                        ArchipelagoManager.IsPlayerSelf(item.Player.Slot) ?  null : item.Player.Name
                    );
                    save = true;
                }
                if (splasherItem.SaveCollect()) {
                    Data.SaveData.CollectedItems.Add(item.LocationId);
                    save = true;
                }

                if (save) Data.SaveData.Save();
            } 
        }
    }
}