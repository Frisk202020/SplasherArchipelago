using HarmonyLib;
using TSKGames.Save;

namespace SplasherArchipelago.Patches.Controller.Save {
    [HarmonyPatch(typeof(DataStore), "LoadAutoSave")]
    public static class Load {
        public static bool Prefix(ref string AutoSaveFilename) {
            AutoSaveFilename = Util.SaveFile();
            return true;
        }
    }
}