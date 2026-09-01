using System.Collections.Generic;
using Archipelago.Helpers;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Models;

namespace Archipelago.Network.Items {
    internal class CheckpointLevel : Item {
        protected readonly string level;
        protected readonly string scene;
        protected CheckpointLevel(int lvl) {
            scene = Util.Scenes[lvl];
            level = Util.Levels[lvl];
        }

        public override ItemFlags GetClassification() => Data.Items.CheckpointItem.seedOption == 2 
            ? ItemFlags.Advancement : ItemFlags.NeverExclude;

        public override bool SaveCollect() => false;
        public override string Name() => Language.Get(CATEGORY, "CheckpointLevel", new[] { level });
        public override void Collect(ItemInfo _info) {
            for (var i = 0; i < Data.CheckpointTable.IdRange(scene); i++) {
                Data.Items.CheckpointItem.Collect(scene, i);
            }
        }

        internal static void AddAll(List<Item> items) {
            for (var i = 0; i < Util.Scenes.Length; i++) {
                items.Add(new CheckpointLevel(i));
            }
        }
    }
}