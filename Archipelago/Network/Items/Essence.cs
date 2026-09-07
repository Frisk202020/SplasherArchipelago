using Archipelago.Helpers;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Models;

namespace Archipelago.Network.Items {
    class Essence : Item {
        private readonly int amount;
        private const int MIN = 50;
        private const int MAX = 700;  

        public Essence(int amount) { 
            this.amount = amount;
        }

        public override string Name() => Language.Get(CATEGORY, $"Essence{amount}");
        public override void Collect(ItemInfo info) {
            var data = GameData.Instance.CollectableData;
            if (data.StarFillCount == MIN) return;

            data.StarFillCount -= amount;
            if (data.StarFillCount < MIN) data.StarFillCount = MIN;
            else if (data.StarFillCount > MAX) data.StarFillCount = MAX;
        }
        public override bool SaveCollect() => false;
        public override ItemFlags GetClassification() => ItemFlags.NeverExclude;
    }
}
