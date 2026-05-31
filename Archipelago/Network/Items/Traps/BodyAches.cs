using Archipelago.MultiClient.Net.Models;

namespace SplasherArchipelago.Network.Items.Traps {
    class BodyAches : Item {
        public void Collect(ItemInfo _item) {
            //! TODO : patch jump to implement a delay
        }

        public string Name() => "Body Aches";

        public bool CollectOnStart() { return true; }
    }
}
