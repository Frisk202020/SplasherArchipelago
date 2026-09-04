using System.Collections.Generic;
using Archipelago.Helpers;
using Archipelago.MultiClient.Net.Models;

namespace Archipelago.Network.Items.Checkpoints {
    class Zone : Base {
        private readonly Data.Items.Zone.ZoneData zone;
        private Zone(int id) => zone = Data.Items.Zone.ZoneForLevel[id];

        public override string Name() => Language.Get(CATEGORY, "CheckpointLevel", zone.name);
        public override void Collect(ItemInfo _item) {
            foreach (var i in zone.keys) {
                Data.Items.CheckpointItem.CollectLevel(Util.Scenes[i]);
            }
        }

        internal static void AddAll(List<Item> items) {
            for (var i = 0; i < Data.Items.Zone.ZoneForLevel.Length; i++) {
                items.Add(new Zone(i));
            }
        }
    }
}