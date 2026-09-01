using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Models;

namespace Archipelago.Network.Items {
    class Freedom : Item {
        internal Freedom() { }

        public override string Name() => "Liberté";
        public override bool SaveCollect() => false;

        public override void Collect(ItemInfo _item) {
            Data.Items.Freedom.Free();
        }

        public override ItemFlags GetClassification() => ItemFlags.Advancement;
    }
}