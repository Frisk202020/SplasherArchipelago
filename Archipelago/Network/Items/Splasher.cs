using Archipelago.MultiClient.Net.Models;
using System.Collections.Generic;

namespace Archipelago.Network.Items {
    class Splasher : Item {
        private readonly HashSet<long> collected = new HashSet<long>();

        internal Splasher() { }

        public string Name() => "Splasher";

        public void Collect(ItemInfo item) {
            if (item.LocationId != -1 && collected.Contains(item.LocationId)) return;

            Data.Items.Splashers.Add();
            collected.Add(item.LocationId);            
        }
    }
}