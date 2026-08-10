using HarmonyLib;

/**
 * Capture a splasher death to punish the player if in hero mode.
 */

namespace Archipelago.Patches.Controller.Death {
    [HarmonyPatch(typeof(Splasher), "Die")]
    public static class SplasherDie {
        public static bool Prefix(Splasher __instance) {
            if (__instance.isDocteur) return false;

            Data.Death.ReportSplasherDeath();
            return true;
        }
    }
}
