using HarmonyLib;
using TSKGames.Save;

/**
 * Use a save dedicated to the current Archipelago seed instead of the default one.
 */

namespace SplasherArchipelago.Patches.Controller.Save {
    [HarmonyPatch(typeof(DataStore), "AutoSaveSilently")]
    public static class Save {
        public static bool Prefix(ref string AutoSaveFilename) {
            AutoSaveFilename = Util.SaveFile();
            return true;
        }
    }
}