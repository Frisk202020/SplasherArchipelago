namespace SplasherArchipelago.Data.Locations {
    static class Powers {
        internal static bool checkedWater { get; private set; } = false;
        internal static bool checkedStickink { get; private set; } = false;
        internal static bool checkedBouncink { get; private set; } = false;

        internal static void CheckWater() { checkedWater = true;  }
        internal static void CheckStickink() { checkedStickink = true; }
        internal static void CheckBouncink() { checkedBouncink = true; }
    }
}
