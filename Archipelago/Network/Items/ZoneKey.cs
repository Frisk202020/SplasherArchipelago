using Archipelago.Helpers;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Models;
using System.Collections.Generic;

namespace Archipelago.Network.Items {
    internal class ZoneKey : Item {
        private readonly string name;
        private readonly HashSet<uint> keys;
        private readonly bool speedrun;

        private ZoneKey(string name, HashSet<uint> keys, bool speedrun) {
            this.name = name;
            this.keys = keys;
            this.speedrun = speedrun;
        }

        public override void Collect(ItemInfo _item) {
            foreach (var key in keys) {
                Data.Items.LevelKeys.Unlock((int)key, speedrun);
            }
        }

        public override string Name() => Language.Get(CATEGORY, $"Zone{(speedrun ? "Time" : "")}", name);
        public override bool SaveCollect() => false;
        
        internal static void AddAll(List<Item> items, bool speedrun) {
            foreach(var x in Data.Items.Zone.ZoneForLevel) {
                items.Add(new ZoneKey(x.name, x.keys, speedrun));
            }
        }

        public override ItemFlags GetClassification() => ItemFlags.Advancement;
    }
}
