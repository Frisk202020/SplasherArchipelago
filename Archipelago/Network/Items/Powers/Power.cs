using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Models;

namespace Archipelago.Network.Items.Powers {
    abstract class Power : Item {
        public override void Collect(ItemInfo item) {
            Patches.UI.Backpack.Update();
        }
        public override bool SaveCollect() => false;
        public override ItemFlags GetClassification() => ItemFlags.Advancement;
    }
}