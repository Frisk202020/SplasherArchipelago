using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Models;

namespace Archipelago.Network.Items {
    class Nothing : Item {
        private string name;
        public Nothing(string _name) => name = _name;

        public string Name() => name;
        public bool SaveCollect() => false;
        public void Collect(ItemInfo _item) {}
        public ItemFlags GetClassification() => ItemFlags.None;
    }
}
