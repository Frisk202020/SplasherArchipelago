using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Models;

namespace Archipelago.Network.Items.Traps {
    internal abstract class Trap : Item {
        public override bool SaveCollect() => true;
        public override ItemFlags GetClassification() => ItemFlags.Trap;
    }
}