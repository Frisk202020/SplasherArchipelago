using Archipelago.MultiClient.Net.Models;

namespace SplasherArchipelago.Network.Items.Powers {
    class Stickink : Power,Item {
        public void Collect(ItemInfo _item) {
            Data.Powers.UnlockSticky();
        }

        public override string Name() => $"Sticky Paint {base.Name()}";

        internal Stickink() { }
    }
}