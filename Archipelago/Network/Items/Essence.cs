using Archipelago.Helpers;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Models;

namespace Archipelago.Network.Items {
    class Essence : Item {
        private readonly int amount;

        public Essence(int amount) { 
            this.amount = amount;
        }

        public override string Name() => Language.Get(CATEGORY, $"Essence{amount}");
        public override void Collect(ItemInfo info) {
            var data = GameData.Instance.CollectableData;
            if (data.StarFillCount == 1) return;

            data.StarFillCount -= amount;
            if (data.StarFillCount < 1) data.StarFillCount = 1;
            else if (data.StarFillCount > 700) data.StarFillCount = 700;
        }
        public override bool SaveCollect() => false;
        public override ItemFlags GetClassification() => ItemFlags.NeverExclude;
    }
}
