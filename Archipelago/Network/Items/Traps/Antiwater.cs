using Archipelago.MultiClient.Net.Models;

namespace SplasherArchipelago.Network.Items.Traps {
    class Antiwater : Item {
        public void Collect(ItemInfo _item) {
            //! TODO : see if possible to implement Antiwater power (see game files) 
            //   => replace water by poison for a ramdom duration
        }

        public string Name() => "Antiwater";

        public bool CollectOnStart() { return false; }
    }
}
