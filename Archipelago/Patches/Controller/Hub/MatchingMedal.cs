using HarmonyLib;

namespace SplasherArchipelago.Patches.Controller.Hub {
    /**
     * Get the medal actual matching with locations (ignoring actual PB).
     * This is useful when a speedrun location is checked by another player finishing their game.
     */

    [HarmonyPatch(typeof(LevelMetaData), "GetMatchingMedal")]
    public static class MatchingMedal {
        public static void Postfix(LevelMetaData __instance, ref Medal __result) {
            var medal = Data.Locations.Speedrun.GetMatchingMedal(__instance.LevelName);
            if (medal is null) return;

            if ((int)medal.Value > (int)__result)
                __result = medal.Value;
        }
    }
}
