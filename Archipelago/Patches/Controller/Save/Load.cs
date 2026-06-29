using HarmonyLib;
using TSKGames.Save;

/**
 * Use a save dedicated to the current Archipelago seed instead of the default one.
 */

namespace SplasherArchipelago.Patches.Controller.Save {
    [HarmonyPatch(typeof(DataStore), "LoadAutoSave")]
    public static class Load {
        public static bool Prefix(ref string AutoSaveFilename) {
            if (AutoSaveFilename != Shared.VANILLA_FILE) return true;

            AutoSaveFilename = Shared.SaveFile();
            return true;
        }
    }
}