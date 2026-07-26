namespace Archipelago.Data.Items {
    static class Powers {
        internal static bool HasWater { get; private set; } = false;
        internal static bool HasSticky { get; private set; } = false;
        internal static bool HasBouncy { get; private set; } = false;
        internal static bool HasSpeed { get; private set; } = false;

        internal static void UnlockProgressiveWater() { 
            if (HasWater) HasSpeed = true;
            else HasWater = true;
        }
        internal static void UnlockSticky() { HasSticky = true; }
        internal static void UnlockBouncy() { HasBouncy = true; }

        internal static void UnlockProgressive() {
            if (!HasWater) {
                HasWater = true;
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