using System.Collections.Generic;
using Archipelago.Helpers;
using Archipelago.MultiClient.Net.Models;

namespace Archipelago.Network.Items.Checkpoints {
    class Singular : Level {
        readonly int id;

        private Singular(int lvl, int id) : base(lvl) {
            this.id = id;
        }

        public override string Name() => Language.Get(CATEGORY, "Checkpoint", new[] { level, (id + 1).ToString() });
        public override void Collect(ItemInfo _info) => Data.Items.CheckpointItem.Collect(scene, id);

        internal static new void AddAll(List<Item> items) {
            for (var i = 0; i < Util.Scenes.Length; i++) {
                for (var j = 0; j < Data.CheckpointTable.IdRange(Util.Scenes[i]); j++) {
                    items.Add(new Singular(i, j));
                }
            }
        }
    }
}