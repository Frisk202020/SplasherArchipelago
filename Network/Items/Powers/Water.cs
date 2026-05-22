using Archipelago.MultiClient.Net.Models;

namespace SplasherArchipelago.Network.Items.Powers {
    class Water : Power,Item {
        public override string Name() => $"Water {base.Name()}";
        public void Collect(ItemInfo _item) {
            Data.Powers.UnlockWater();
        }

        internal Water() { }
    }
}