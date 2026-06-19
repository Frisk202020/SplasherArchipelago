using Archipelago.MultiClient.Net.Models;
using System;

namespace SplasherArchipelago.Network.Items {
    class Essence : Item {
        private readonly uint ammount;

        public Essence(uint ammount) { 
            this.ammount = ammount;
        }

        public string Name() => $"Essence ({ammount})";
        public void Collect(ItemInfo info) {
            Data.Items.Essence.Add(ammount);
        }
    }
}
