using Archipelago.MultiClient.Net.Models;

namespace Archipelago.Network.Items.Traps {
    internal abstract class Trap : Item {
        public bool SaveCollect() => true;
        abstract public string Name();
        abstract public void Collect(ItemInfo _item);
    }
}