namespace Archipelago.Data.Items {
    internal enum WaterState {
        None,
        Polluted,
        Clean,
        Speedy
    }

    static class Powers {
        internal static WaterState WaterLevel { get; private set; } = WaterState.None;
        internal static bool HasWater => WaterLevel != WaterState.None;
        internal static bool HasSticky { get; private set; } = false;
        internal static bool HasBouncy { get; private set; } = false;

        internal static void UnlockProgressiveWater() { 
            if (WaterLevel == WaterState.Speedy) return;
            WaterLevel = (WaterState)((int)WaterLevel + 1);
        }

        internal static void UnlockCleanWater() => WaterLevel = WaterState.Clean;

        internal static void UnlockSticky() { HasSticky = true; }
        internal static void UnlockBouncy() { HasBouncy = true; }

        internal static void UnlockProgressive() {
            if (!HasWater) {
                WaterLevel = WaterState.Clean;
                return;
            }

            if (!HasSticky) {
                HasSticky = true;
                return;
            }

            HasBouncy = true;
        }
    }
}