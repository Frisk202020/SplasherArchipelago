using System.Collections.Generic;
using Archipelago.Helpers;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Models;

namespace Archipelago.Network.Items {
    class Checkpoint : Item {
        readonly string level;
        readonly string scene;
        readonly int id;

        private Checkpoint(int lvl, int id) {
            scene = Util.Scenes[lvl];
            level = Util.Levels[lvl];
            this.id = id;
        }

        public override ItemFlags GetClassification() => Data.Items.CheckpointItem.seedOption == 2 ? ItemFlags.Advancement : ItemFlags.NeverExclude;
        public override string Name() => Language.Get(CATEGORY, "Checkpoint", new[] { level, (id + 1).ToString() });
        public override bool SaveCollect() => false;
        public override void Collect(ItemInfo _info) => Data.Items.CheckpointItem.Collect(scene, id);

        internal static void AddAll(List<Item> items) {
            for (var i = 0; i < Util.Scenes.Length; i++) {
                for (var j = 0; j < Data.CheckpointTable.IdRange(Util.Scenes[i]); j++) {
                    items.Add(new Checkpoint(i, j));
                }
            }
        }
    }
}