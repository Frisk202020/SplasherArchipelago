using Archipelago.MultiClient.Net.Models;

namespace Archipelago.Network.Items.Powers {
    class Bouncink : Power {
        public override string Name() => $"Bouncy Paint {base.Name()}";

        public override void Collect(ItemInfo _item) {
            Data.Items.Powers.UnlockBouncy();
            base.Collect(_item);
        }
    }
}