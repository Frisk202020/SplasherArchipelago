using System.Collections.Generic;
using Archipelago.Helpers;
using Archipelago.MultiClient.Net.Models;

namespace Archipelago.Network.Items.Checkpoints {
    class Zone : Base {
        private readonly Data.Items.Zone.ZoneData zone;
        private Zone(int id) => zone = Data.Items.Zone.ZoneForLevel[id];

        public override string Name() => Language.Get(CATEGORY, "CheckpointLevel", new[] { zone.name });
        public override void Collect(ItemInfo _item) {
            foreach (var i in zone.keys) {
                for (var j = 0; j < Data.CheckpointTable.IdRange(Util.Scenes[i]); j++) {
                    Data.Items.CheckpointItem.Collect(Util.Scenes[i+1], j);
                }
            }
        }

        internal static void AddAll(List<Item> items) {
            for (var i = 0; i < Data.Items.Zone.ZoneForLevel.Length; i++) {
                items.Add(new Zone(i));
            }
        }
    }
}