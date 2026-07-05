using Archipelago.MultiClient.Net.Models;
using System.Collections.Generic;

namespace SplasherArchipelago.Network.Items {
    class Key : Item {
        private readonly uint id;
        private readonly string levelName;
        private readonly bool speedrun;
        private Key(uint id, string lvl, bool speedrun) {
            this.id = id;
            levelName = lvl;
            this.speedrun = speedrun;
        }

        public void Collect(ItemInfo _item) {
            Data.Items.LevelKeys.Unlock((int)id, speedrun);
        }

        public string Name() => $"{levelName} {(speedrun ? "- Time Attack" : "")}: Entrance Key";
        
        internal static void AddAll(List<Item> items, bool speedrun) {
            for (uint i = 1; i < Util.Levels.Length; i++) {
                items.Add(new Key(i - 1, Util.Levels[i], speedrun));
            }
        }
    }
}
