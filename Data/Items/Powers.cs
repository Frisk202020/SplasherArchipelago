namespace SplasherArchipelago.Data.Items {
    static class Powers {
        internal static bool HasWater { get; private set; } = false;
        internal static bool HasSticky { get; private set; } = false;
        internal static bool HasBouncy { get; private set; } = false;

        internal static void UnlockWater() { HasWater = true;  }
        internal static void UnlockSticky() { HasSticky = true; }
        internal static void UnlockBouncy() { HasBouncy = true; }
    }
}