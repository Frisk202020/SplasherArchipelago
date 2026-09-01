using Archipelago.Helpers;
using Archipelago.MultiClient.Net.Models;

namespace Archipelago.Network.Items.Traps {
    internal class Feet : Trap {
        public override string Name() => Language.Get(CATEGORY, "Feet State");
        public override void Collect(ItemInfo _item) => Data.TrapController.SetRandomFeetState();
    }
}