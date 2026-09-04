using HarmonyLib;

namespace Archipelago.Patches.Location {
    [HarmonyPatch(typeof(Splasher), "Collect")]
    public static class InstantSplasherCollect {
        internal static bool Active = false;

        public static void Postfix(Splasher __instance) {
            if (!Active || !__instance.Rescued) return;
            Data.Locations.Splashers.Check(__instance);
        }
    }
}