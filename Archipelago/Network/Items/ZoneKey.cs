using Archipelago.MultiClient.Net.Models;
using System.Collections.Generic;

namespace SplasherArchipelago.Network.Items {
    internal class ZoneKey : Item {
        private readonly string name;
        private readonly uint[] keys;
        private readonly bool speedrun;

        private class KeyData {
            public string name;
            public uint[] keys;
        }

        private static readonly KeyData[] data = new KeyData[] {
            new KeyData { name = "Reception Hub", keys = new uint[] { 0, 1, 5 } },
            new KeyData { name = "Water Pool", keys = new uint[] { 3, 4, 7, 11 } }, 
            new KeyData { name = "Ray Man Paradise", keys = new uint[] { 6, 10, 16, 18 } },
            new KeyData { name = "Toxink Hell", keys = new uint[] { 14, 17, 19 } },
            new KeyData { name = "Inkorp Outskirts", keys = new uint[] { 9, 12, 15 } },
            new KeyData { name = "Fun Park", keys = new uint[] { 2, 8, 13 } },
            new KeyData { name = "Docteur's Office", keys = new uint[] { 20 } }
        };

        private ZoneKey(string name, uint[] keys, bool speedrun) {
            this.name = name;
            this.keys = keys;
            this.speedrun = speedrun;
        }

        public void Collect(ItemInfo _item) {
            foreach (var key in keys) {
                Data.Items.LevelKeys.Unlock((int)key, speedrun);
            }
        }

        public string Name() => $"{name} : Keys";
        
        internal static void AddAll(List<Item> items, bool speedrun) {
            foreach(var x in data) {
                items.Add(new ZoneKey(x.name, x.keys, speedrun));
            }
        }
    }
}
