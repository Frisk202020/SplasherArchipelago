using HarmonyLib;

namespace SplasherArchipelago.Patches.Setup {
    [HarmonyPatch(typeof(GameData), "InitializePlayerData")]
    static class PreventLoadSave {  
        public static bool Prefix(ref bool forceNew) {
            forceNew = true;
            return true;
        }
    }
}
