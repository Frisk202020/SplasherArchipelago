using Archipelago.MultiClient.Net.Models;

namespace Archipelago.Network.Items {
    interface Item {
        void Collect(ItemInfo item);
        string Name();
        bool SaveCollect();
        Classification GetClassification();
    }
}