using Archipelago.Helpers;
using Archipelago.MultiClient.Net.Models;

namespace Archipelago.Network.Items.Powers {
    class Stickink : Power {
        public override void Collect(ItemInfo _item) {
            Data.Items.Powers.UnlockSticky();
            base.Collect(_item);
        }

        public override string Name() => Language.Get(CATEGORY, "Stickink");
    }
}