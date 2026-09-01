using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Models;

namespace Archipelago.Network.Items {
    class Splasher : Item {
        public override string Name() => "Splasher";
        public override void Collect(ItemInfo item) => Data.Items.Splashers.Add();
        public override bool SaveCollect() => false;
        public override ItemFlags GetClassification() => ItemFlags.Advancement;
    }
}