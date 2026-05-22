using Archipelago.MultiClient.Net.Models;

namespace SplasherArchipelago.Network.Items.Powers {
    class Bouncink : Power,Item {
        public Bouncink() { }

        public override string Name() => $"Bouncy Paint {base.Name()}";

        public void Collect(ItemInfo _item) {
            Data.Powers.UnlockBouncy();
        }
    }
}