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
                new Nothing(), new Nothing(), new Nothing(),
                new Traps.PaintSwap(), new Traps.MadGun(), new Traps.Feet()
            };

            foreach (EssenceInfo x in new List<EssenceInfo> {
                new EssenceInfo(1, "Essence drop"),
                new EssenceInfo(2, "Essence drops"),
                new EssenceInfo(5, "Broken essence flask"),
                new EssenceInfo(10, "Full essence flask"),
                new EssenceInfo(15, "Dry essence barrel"),
                new EssenceInfo(20, "Essence barrel"),
                new EssenceInfo(25, "Overflowing essence barrel"),
                new EssenceInfo(30, "Goombase essence tank"),
                new EssenceInfo(40, "Secretaire essence tank"),
                new EssenceInfo(50, "Docteur's essence storage"),
                new EssenceInfo(-1, "Minor essence leak"),
                new EssenceInfo(-2, "Small essence leak"),
                new EssenceInfo(-3, "Noticeable essence leak"),
                new EssenceInfo(-5, "Severe essence leak"),
                new EssenceInfo(-10, "Essence container crack"),
                new EssenceInfo(-15, "Forgiving essence fee"),
                new EssenceInfo(-20, "Severe essence fee"),
                new EssenceInfo(-25, "Le Docteur's essence tax")
            }) {
                items.Add(new Essence(x.amount, x.name));
            }

            Key.AddAll(items, false);
            ZoneKey.AddAll(items, false);
            Key.AddAll(items, true);
            ZoneKey.AddAll(items, true);

            Checkpoint.AddAll(items);

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