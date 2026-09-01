using Archipelago.Helpers;
using Archipelago.MultiClient.Net.Models;

namespace Archipelago.Network.Items.Powers {
    class Progressive : Power {
        public override string Name() => Language.Get(CATEGORY, "Progressive");

        public override void Collect(ItemInfo _item) {
            Data.Items.Powers.UnlockProgressive();
            base.Collect(_item);
        }
    }
}