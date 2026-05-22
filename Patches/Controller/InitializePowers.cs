using HarmonyLib;

namespace SplasherArchipelago.Patches.Controller {
    [HarmonyPatch(typeof(SauceMachine), "InitializePowers")]
    public static class InitializePowers {
        public static bool Prefix(SauceMachine __instance) {
            __instance.water = true;
            __instance.stickyPaint = true;
            __instance.bouncyPaint = true;
            return false;
        }
    }
}