using HarmonyLib;

/**
 * Mark all powers as unlocked. Actual unlocks are then managed by button patches (@see Input).
 * This is easier to proceed that way because powers are very much enforced to be progressive in the game's code.
 */

namespace Archipelago.Patches.Controller.Hub {
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