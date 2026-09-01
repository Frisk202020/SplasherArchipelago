using Archipelago.Helpers;
using Archipelago.MultiClient.Net.Models;

namespace Archipelago.Network.Items.Traps {
    internal class MadGun : Trap {
        public override string Name() => Language.Get(CATEGORY, "Mad Gun");
        public override void Collect(ItemInfo _item) => Data.TrapController.SetRandomAlwaysAction();
    }
}