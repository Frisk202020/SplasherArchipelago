namespace SplasherArchipelago.Data {
    public static class Powers {
        public static bool HasWater { get; private set; } = false;
        public static bool HasSticky { get; private set; } = false;
        public static bool HasBouncy { get; private set; } = false;

        public static void UnlockWater() { HasWater = true;  }
        public static void UnlockSticky() { HasSticky = true; }
        public static void UnlockBouncy() { HasBouncy = true; }
    }
}