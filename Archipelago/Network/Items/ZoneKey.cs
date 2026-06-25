using Archipelago.MultiClient.Net.Models;
using System.Collections.Generic;

namespace SplasherArchipelago.Network.Items {
    internal class ZoneKey : Item {
        private readonly string name;
        private readonly uint[] keys;

        private static readonly ZoneKey[] data = new ZoneKey[] {
            new ZoneKey ("Reception Hub", new uint[] { 0, 1, 5 }),
            new ZoneKey ("Water Pool", new uint[] { 3, 4, 7, 11 }), 
            new ZoneKey ("Ray Man Paradise", new uint[] { 6, 10, 16, 18 } ),
            new ZoneKey ("Toxink Hell", new uint[] { 14, 17, 19 } ),
            new ZoneKey ("Inkorp Outskirts", new uint[] { 9, 12, 15 }),
            new ZoneKey ("Fun Park", new uint[] { 2, 8, 13 }),
            new ZoneKey ("Docteur's Office", new uint[] { 20 } )
        };

        private ZoneKey(string name, uint[] keys) {
            this.name = name;
            this.keys = keys;
        }

        public void Collect(ItemInfo _item) {
            foreach (var key in keys) {
                Data.Items.LevelKeys.Unlock((int)key);
            }
        }

        public string Name() => $"{name} : Keys";
        
        internal static void AddAll(List<Item> items) {
            foreach(var x in data) {
                items.Add(x);
            }
        }
    }
}
