using Archipelago.MultiClient.Net.Models;

namespace Archipelago.Network.Items.Powers {
    abstract class Power {
        public virtual string Name() => "Unlock";
        public virtual void Collect(ItemInfo item) {
            Patches.UI.Backpack.Update();
        }
    }
}