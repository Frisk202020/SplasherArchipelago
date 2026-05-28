namespace SplasherArchipelago.Data.Locations {
    static class Powers {
        internal static bool checkedWater { get; private set; } = false;
        internal static bool checkedStickink { get; private set; } = false;
        internal static bool checkedBouncink { get; private set; } = false;

        internal static void CheckWater() {
            if (checkedWater) return;

            Network.InternalArchipelagoManager.Check(LocationType.Water, 0);
            checkedWater = true; 
        }
        internal static void CheckStickink() {
            if (checkedStickink) return;

            Network.InternalArchipelagoManager.Check(LocationType.Stickink, 0);
            checkedStickink = true; 
        }
        internal static void CheckBouncink() {
            if (checkedBouncink) return;

            Network.InternalArchipelagoManager.Check(LocationType.Bouncink, 0);
            checkedBouncink = true; 
        }

        internal static void RestoreWater() { checkedWater = true; }
        internal static void RestoreStickink() { checkedStickink = true; }
        internal static void RestoreBouncink() { checkedBouncink = true; }
    }
}
