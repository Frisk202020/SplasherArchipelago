namespace Archipelago.Data {
    internal class PendingKeyUnlock {
        internal enum KeyMode {
            None,
            Zone,
            Level
        }

        internal static KeyMode Mode = KeyMode.None;
        internal static bool SpeedrunKeys { private get; set; } = false;

        internal static string KeyItemName(LocalizedString lvlName, bool isSpeedrun) {
            var isZone = Mode == KeyMode.Zone;
            var itemName = isZone
                ? Items.Zone.FindZone((uint)Helpers.LevelByName.Id(lvlName))
                : lvlName.GetString();

            if (SpeedrunKeys && isSpeedrun) itemName += " Time Attack";
            return itemName + (isZone ? " - Zone Keys" : " - Entrance Key");        
        }

        internal readonly int id;
        internal readonly bool isSpeedrun;

        internal PendingKeyUnlock(int id, bool isSpeedrun) {
            this.id = id;
            this.isSpeedrun = isSpeedrun;
        }
    }
}
