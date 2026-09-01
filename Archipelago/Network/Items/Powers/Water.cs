using Archipelago.Helpers;
using Archipelago.MultiClient.Net.Models;

namespace Archipelago.Network.Items.Powers {
    class Water : Power {
        public override string Name() => Language.Get(CATEGORY, "Water");
        public override void Collect(ItemInfo _item) {
            Data.Items.Powers.UnlockCleanWater();
            base.Collect(_item);
        }
    }
}