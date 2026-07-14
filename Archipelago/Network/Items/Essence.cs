using Archipelago.MultiClient.Net.Models;
using System;
using UnityEngine;

namespace Archipelago.Network.Items {
    class Essence : Item {
        private readonly int ammount;

        public Essence(int ammount) { 
            this.ammount = ammount;
        }

        public string Name() => $"Essence ({ammount})";
        public void Collect(ItemInfo info) {
            var data = GameData.Instance.CollectableData;
            if (data.StarFillCount == 1) return;

            data.StarFillCount -= ammount;
            data.StarFillCount = Math.Max(data.StarFillCount, 1);
        }
    }
}
