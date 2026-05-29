using HarmonyLib;

namespace SplasherArchipelago.Patches.Controller {
    [HarmonyPatch(typeof(TSKGames.Save.DataStore), "AutoSaveSilently")]
    public static class PreventAutoSave {
        public static bool Prefix() { return false; }
    }
}
