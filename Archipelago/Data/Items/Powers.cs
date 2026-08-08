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

        internal static bool ProgressiveWater = false;

        internal static void UnlockProgressiveWater() { 
            if (WaterLevel == WaterState.Speedy) return;
            WaterLevel = (WaterState)((int)WaterLevel + 1);
        }

        internal static void UnlockCleanWater() => WaterLevel = WaterState.Clean;

        internal static void UnlockSticky() { HasSticky = true; }
        internal static void UnlockBouncy() { HasBouncy = true; }

        internal static void UnlockProgressive() {
            if (HasBouncy) return;

            switch(WaterLevel) {
                case WaterState.None: WaterLevel = ProgressiveWater ? WaterState.Polluted : WaterState.Clean; return;
                case WaterState.Polluted: WaterLevel = WaterState.Clean; return;
                case WaterState.Clean: 
                    if (ProgressiveWater) WaterLevel = WaterState.Speedy;
                    else if (HasSticky) HasBouncy = true;
                    else HasSticky = true;
                    return;
                case WaterState.Speedy:
                    if (HasSticky) HasBouncy = true;
                    else HasSticky = true;
                    return;
            }
        }
    }
}