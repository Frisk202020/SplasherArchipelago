using Archipelago.MultiClient.Net.Models;

namespace Archipelago.Network.Items.Powers {
    class Progressive : Power, Item {
        public override string Name() => $"Progressive Power {base.Name()}";

        public void Collect(ItemInfo _item) {
            Data.Items.Powers.UnlockProgressive();
        }
    }
}