using HarmonyLib;

namespace SplasherArchipelago.Patches.Setup {
    [HarmonyPatch(typeof(Hub), "IsFirstLevelFinished", MethodType.Getter)]
    public static class SkipTutorial {
        public static bool Prefix(ref bool __result) {
            __result = true;
            return false;
        }
    }
}
