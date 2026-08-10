using Archipelago.MultiClient.Net.Models;

namespace Archipelago.Network.Items.Traps {
    internal class Feet : Item {
        public string Name() => $"Feet State";
        public void Collect(ItemInfo _item) => Data.TrapController.SetRandomFeetState();
    }
}