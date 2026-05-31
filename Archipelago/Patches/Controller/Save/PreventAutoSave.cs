using HarmonyLib;

namespace SplasherArchipelago.Patches.Controller.Save {
    [HarmonyPatch(typeof(TSKGames.Save.DataStore), "AutoSaveSilently")]
    public static class PreventAutoSave {
        public static bool Prefix() { return false; }
    }
}
