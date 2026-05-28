using HarmonyLib;

namespace SplasherArchipelago.Patches.Setup {
    [HarmonyPatch(typeof(GameData), "SetInstance")]
    public static class PostLoading {
        public static void Postfix() {
            Network.InternalArchipelagoManager.RestoreCheckedLocations();
        }
    }
}
