using Archipelago.MultiClient.Net.Models;

namespace Archipelago.Network.Items.Powers {
    class Progressive : Power {
        public override string Name() => $"Progressive Power {base.Name()}";

        public override void Collect(ItemInfo _item) {
            Data.Items.Powers.UnlockProgressive();
            base.Collect(_item);
        }
    }
}