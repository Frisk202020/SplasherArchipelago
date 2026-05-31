using HarmonyLib;

namespace SplasherArchipelago.Patches.Controller.Hub {
    [HarmonyPatch(typeof(GameData), "GetGameRescuedSplashersCount")]
    public class SplasherCount {
        public static bool Prefix(ref int __result) {
            __result = Data.Items.Splashers.Count;
            return false;
        }
    }
}