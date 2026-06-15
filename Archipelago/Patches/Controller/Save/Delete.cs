using HarmonyLib;
using TSKGames.Save;

namespace SplasherArchipelago.Patches.Controller.Save {
    [HarmonyPatch(typeof(DataStore), "DeleteAutoSave")]
    public static class Delete {
        public static bool Prefix(ref string AutoSaveFilename) {
            AutoSaveFilename = Util.SaveFile();
            return true;
        }
    }
}
