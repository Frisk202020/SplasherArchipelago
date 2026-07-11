using HarmonyLib;

/**
 * Skip the intro cutscene to rather spawn in the Hub on a new save.
 */

namespace Archipelago.Patches.Controller.Hub {
    [HarmonyPatch(typeof(global::Hub), "IsFirstLevelFinished", MethodType.Getter)]
    public static class SkipTutorial {
        public static bool Prefix(ref bool __result) {
            __result = true;
            return false;
        }
    }
}
