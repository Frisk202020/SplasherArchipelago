using Archipelago.MultiClient.Net.Models;

namespace Archipelago.Network.Items.Traps {
    internal class Feet : Trap {
        public override string Name() => $"Feet State";
        public override void Collect(ItemInfo _item) => Data.TrapController.SetRandomFeetState();
    }
}