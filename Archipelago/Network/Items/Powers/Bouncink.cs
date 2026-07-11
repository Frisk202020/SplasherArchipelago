using Archipelago.MultiClient.Net.Models;

namespace Archipelago.Network.Items.Powers {
    class Bouncink : Power,Item {
        public override string Name() => $"Bouncy Paint {base.Name()}";

        public void Collect(ItemInfo _item) {
            Data.Items.Powers.UnlockBouncy();
        }
    }
}