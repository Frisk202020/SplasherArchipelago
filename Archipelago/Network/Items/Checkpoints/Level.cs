using System.Collections.Generic;
using Archipelago.Helpers;
using Archipelago.MultiClient.Net.Models;

namespace Archipelago.Network.Items.Checkpoints {
    class Level : Base {
        protected readonly string level;
        protected readonly string scene;
        protected Level(int lvl) {
            scene = Util.Scenes[lvl];
            level = Util.Levels[lvl];
        }

        public override string Name() => Language.Get(CATEGORY, "CheckpointLevel", level);
        public override void Collect(ItemInfo _info) => Data.Items.CheckpointItem.CollectLevel(scene);

        internal static void AddAll(List<Item> items) {
            for (var i = 0; i < Util.Scenes.Length; i++) {
                items.Add(new Level(i));
            }
        }
    }
}