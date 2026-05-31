using Archipelago.MultiClient.Net.Models;

namespace SplasherArchipelago.Network.Items.Fillers {
    abstract class Filler {
        public void Collect(ItemInfo _item) { }

        public bool CollectOnStart() { return false; }
    }
}
