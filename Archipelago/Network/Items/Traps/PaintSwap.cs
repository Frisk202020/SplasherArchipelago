using Archipelago.MultiClient.Net.Models;

namespace Archipelago.Network.Items.Traps {
    class PaintSwap : Trap {
        public override void Collect(ItemInfo _item) => Data.TrapController.SetRandomMapping();
        public override string Name() => "Paint Swap";
    }
}
