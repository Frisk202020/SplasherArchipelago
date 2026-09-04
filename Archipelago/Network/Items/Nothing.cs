using Archipelago.Helpers;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Models;

namespace Archipelago.Network.Items {
    class Nothing : Item {
        private string name;
        public Nothing(string _name) => name = Language.Get(CATEGORY, _name);

        public override string Name() => name;
        public override bool SaveCollect() => false;
        public override void Collect(ItemInfo _item) {}
        public override ItemFlags GetClassification() => ItemFlags.None;
    }
}
