using Archipelago.Helpers;
using Archipelago.MultiClient.Net.Models;

namespace Archipelago.Network.Items.Powers {
    class Bouncink : Power {
        public override string Name() => Language.Get(CATEGORY, "Bouncink");

        public override void Collect(ItemInfo _item) {
            Data.Items.Powers.UnlockBouncy();
            base.Collect(_item);
        }
    }
}