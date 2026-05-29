using HarmonyLib;

namespace SplasherArchipelago.Patches.Controller {
    [HarmonyPatch(typeof(TSKGames.Save.DataStore), "Save")]
    public static class PreventSave {
        public static bool Prefix() { return false; }
    }
}
