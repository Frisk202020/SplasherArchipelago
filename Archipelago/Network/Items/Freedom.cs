using Archipelago.MultiClient.Net.Models;

namespace SplasherArchipelago.Network.Items {
    class Freedom : Item {
        internal Freedom() { }

        public string Name() => "Freedom";

        public void Collect(ItemInfo _item) {
            Data.Items.Freedom.Free();
        }
    }
}