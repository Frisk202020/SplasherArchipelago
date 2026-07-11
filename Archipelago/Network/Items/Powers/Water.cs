using Archipelago.MultiClient.Net.Models;

namespace Archipelago.Network.Items.Powers {
    class Water : Power,Item {
        public override string Name() => $"Water {base.Name()}";
        public void Collect(ItemInfo _item) {
            Data.Items.Powers.UnlockWater();
        }
    }
}