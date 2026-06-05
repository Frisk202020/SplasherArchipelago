using HarmonyLib;

namespace SplasherArchipelago.Patches.Controller.Death {
    [HarmonyPatch(typeof(Splasher), "Die")]
    public static class SplasherDie {
        public static bool Prefix(Splasher __instance) {
            if (__instance.isDocteur) return false;

            Data.DeathLink.ReportSplasherDeath();
            return true;
        }
    }
}
