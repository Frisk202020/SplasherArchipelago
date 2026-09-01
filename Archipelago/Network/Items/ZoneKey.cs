using Archipelago.Helpers;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Models;
using System.Collections.Generic;

namespace Archipelago.Network.Items {
    internal class ZoneKey : Item {
        private readonly string name;
        private readonly HashSet<uint> keys;
        private readonly bool speedrun;

        private class KeyData {
            public string name;
            public HashSet<uint> keys;
        }

        private static readonly KeyData[] data = new KeyData[] {
            new KeyData { name = "Reception Hub", keys = new HashSet<uint> { 0, 1, 5 } },
            new KeyData { name = "Water Pool", keys = new HashSet<uint> { 3, 4, 7, 11 } }, 
            new KeyData { name = "Ray Man Paradise", keys = new HashSet<uint> { 6, 10, 16, 18 } },
            new KeyData { name = "Toxink Hell", keys = new HashSet<uint> { 14, 17, 19 } },
            new KeyData { name = "Inkorp Outskirts", keys = new HashSet<uint> { 9, 12, 15 } },
            new KeyData { name = "Fun Park", keys = new HashSet<uint> { 2, 8, 13 } },
            new KeyData { name = "Docteur's Office", keys = new HashSet<uint> { 20 } }
        };

        internal static string FindZone(int trueId) {
            uint id = (uint)trueId - 1;
            foreach(var zoneData in data) {
                if (zoneData.keys.Contains(id)) return zoneData.name;
            }

            return "";
        }

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

        public override string Name() => Language.Get(CATEGORY, $"Zone{(speedrun ? "Time" : "")}", new[] { name });
        public override bool SaveCollect() => false;
        
        internal static void AddAll(List<Item> items, bool speedrun) {
            foreach(var x in data) {
                items.Add(new ZoneKey(x.name, x.keys, speedrun));
            }
        }

        public override ItemFlags GetClassification() => ItemFlags.Advancement;
    }
}
