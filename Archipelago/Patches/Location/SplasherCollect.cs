using HarmonyLib;

/**
 * Detect a splasher location.
 */

namespace Archipelago.Patches.Location {
    [HarmonyPatch(typeof(GameActor), "NoReset", MethodType.Setter)]
    public static class SplasherCollect {
        public static bool Prefix(GameActor __instance, bool value) {
            if (!(__instance is Splasher)) return true;

            var instance = (Splasher)__instance;
            if (!value || !instance.Rescued) return true;

            Data.Locations.Splashers.Check(instance);
            return true;
        }
    }
}
