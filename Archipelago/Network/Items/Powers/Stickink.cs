using Archipelago.MultiClient.Net.Models;

namespace Archipelago.Network.Items.Powers {
    class Stickink : Power,Item {
        public void Collect(ItemInfo _item) {
            Data.Items.Powers.UnlockSticky();
        }

        public override string Name() => $"Sticky Paint {base.Name()}";
    }
}