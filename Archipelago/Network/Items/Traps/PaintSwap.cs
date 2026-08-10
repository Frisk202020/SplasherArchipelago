using Archipelago.MultiClient.Net.Models;

namespace Archipelago.Network.Items.Traps {
    class PaintSwap : Item {
        public void Collect(ItemInfo _item) => Data.TrapController.SetRandomMapping();
        public string Name() => "Paint Swap";
    }
}
