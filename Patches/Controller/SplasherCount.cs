using HarmonyLib;

namespace SplasherArchipelago.Patches.Controller {
    [HarmonyPatch(typeof(GameData), "GetGameRescuedSplashersCount")]
    public class SplasherCount {
        public static bool Prefix(ref int __result) {
            __result = Data.Splashers.Count;
            return false;
        }
    }
}