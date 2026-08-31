using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Models;

namespace Archipelago.Network.Items {
    class Freedom : Item {
        internal Freedom() { }

        public string Name() => "Freedom";
        public bool SaveCollect() => false;

        public void Collect(ItemInfo _item) {
            Data.Items.Freedom.Free();
        }

        public ItemFlags GetClassification() => ItemFlags.Advancement;
    }
}