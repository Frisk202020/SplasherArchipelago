using Archipelago.MultiClient.Net.Models;
using System.Collections.Generic;

namespace SplasherArchipelago.Network.Items {
    class Key : Item {
        private readonly uint id;
        private readonly string levelName;
        private Key(uint id, string lvl) {
            this.id = id;
            levelName = lvl;
        }

        public void Collect(ItemInfo _item) {
            Data.Items.LevelKeys.Unlock((int)id);
        }

        public string Name() => $"{levelName} : Entrance Key";
        
        internal static void AddAll(List<Item> items) {
            for (uint i = 1; i < Util.Levels.Length; i++) {
                items.Add(new Key(i - 1, Util.Levels[i]));
            }
        }
    }
}
