using Archipelago.MultiClient.Net.Models;

namespace Archipelago.Network.Items.Powers {
    internal class ProgressiveWater : Power {
        public override void Collect(ItemInfo info) {
            Data.Items.Powers.UnlockProgressiveWater();
            base.Collect(info);
        }
        public override string Name() => "Progressive Water";
    }
}