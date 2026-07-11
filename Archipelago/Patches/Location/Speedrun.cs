using HarmonyLib;

/**
 * Detect a speedrun location.
 */

namespace Archipelago.Patches.Location {
    [HarmonyPatch(typeof(UIScorePanel), "Refresh")]
    public static class Speedrun {
        public static bool Prefix(ref bool showWorld) {
            showWorld = false;
            return true;
        }

        public static void Postfix(LevelMetaData lmd, Medal __result) {
            Data.Locations.Speedrun.Check(__result, lmd.LevelName);
        }
    }
}
