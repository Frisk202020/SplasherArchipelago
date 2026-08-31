using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Models;

namespace Archipelago.Network.Items.Powers {
    abstract class Power : Item {
        public virtual string Name() => "Gun Unlock";
        public virtual void Collect(ItemInfo item) {
            Patches.UI.Backpack.Update();
        }
        public bool SaveCollect() => false;
        public ItemFlags GetClassification() => ItemFlags.Advancement;
    }
}