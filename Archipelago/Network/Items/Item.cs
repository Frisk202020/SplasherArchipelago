using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Models;

namespace Archipelago.Network.Items {
    interface Item {
        void Collect(ItemInfo item);
        string Name();
        bool SaveCollect();
        ItemFlags GetClassification();
    }
}