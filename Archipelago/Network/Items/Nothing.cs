using Archipelago.MultiClient.Net.Models;

namespace Archipelago.Network.Items {
    class Nothing : Item {
        public string Name() => "Nothing";
        public bool SaveCollect() => false;
        public void Collect(ItemInfo _item) {}
        public Classification GetClassification() => Classification.Filler;
    }
}
