using Archipelago.MultiClient.Net.Models;

namespace Archipelago.Network.Items.Traps {
    internal class MadGun : Item {
        public string Name() => $"Mad Gun";
        public void Collect(ItemInfo _item) => Data.TrapController.SetRandomAlwaysAction();
    }
}