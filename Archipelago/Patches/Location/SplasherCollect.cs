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

            var index = (int)AccessTools.DeclaredField(typeof(Splasher), "index").GetValue(instance);
            var name = GameData.Instance.CurrentLevelMetaData.LevelName;
            Data.Locations.Splashers.Check(name, index);
            return true;
        }
    }
}
