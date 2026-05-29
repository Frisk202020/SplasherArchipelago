using Archipelago.MultiClient.Net.Models;

namespace SplasherArchipelago.Network.Items {
    interface Item {
        void Collect(ItemInfo item);
        string Name();
    }
}