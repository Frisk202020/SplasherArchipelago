using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Models;

namespace Archipelago.Network.Items {
    class Essence : Item {
        private readonly int amount;
        private readonly string name;

        public Essence(int amount, string name) { 
            this.amount = amount;
            this.name = name;
        }

        public string Name() => name;
        public void Collect(ItemInfo info) {
            var data = GameData.Instance.CollectableData;
            if (data.StarFillCount == 1) return;

            data.StarFillCount -= amount;
            if (data.StarFillCount < 1) data.StarFillCount = 1;
            else if (data.StarFillCount > 700) data.StarFillCount = 700;
        }
        public bool SaveCollect() => false;
        public ItemFlags GetClassification() => ItemFlags.NeverExclude;
    }
}
