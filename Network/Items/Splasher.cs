using Archipelago.MultiClient.Net.Models;
using System.Collections.Generic;

namespace SplasherArchipelago.Network.Items {
    class Splasher : Item {
        private HashSet<long> collected = new HashSet<long>();

        internal Splasher() { }

        public string Name() => "Splasher";

        public void Collect(ItemInfo item) {
            if (collected.Contains(item.LocationId)) return;

            Data.Items.Splashers.Add();
            collected.Add(item.LocationId);
        }
    }
}