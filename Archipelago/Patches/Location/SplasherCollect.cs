using HarmonyLib;

/**
 * Detect a splasher location.
 */

namespace SplasherArchipelago.Patches.Location {
    [HarmonyPatch(typeof(Splasher), "OnCheckpoint")]
    public static class SplasherCollect {
        public static bool Prefix(Splasher __instance, int ___index) {
            if (!__instance.Rescued) return true;

            var name = GameData.Instance.CurrentLevelMetaData.LevelName;
            Data.Locations.Splashers.Check(name, ___index);
            return true;
        }
    }
}
