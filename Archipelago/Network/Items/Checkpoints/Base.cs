using Archipelago.MultiClient.Net.Enums;

namespace Archipelago.Network.Items.Checkpoints {
    abstract class Base : Item {
        public override ItemFlags GetClassification() => Data.Items.CheckpointItem.seedOption == 2 
            ? ItemFlags.Advancement : ItemFlags.NeverExclude;

        public override bool SaveCollect() => false;
    }
}