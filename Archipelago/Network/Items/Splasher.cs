using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Models;

namespace Archipelago.Network.Items {
    class Splasher : Item {
        public string Name() => "Splasher";
        public void Collect(ItemInfo item) => Data.Items.Splashers.Add();
        public bool SaveCollect() => false;
        public ItemFlags GetClassification() => ItemFlags.Advancement;
    }
}