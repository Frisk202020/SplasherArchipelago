using HarmonyLib;
using TSKGames.Save;

/**
 * Use a save dedicated to the current Archipelago seed instead of the default one.
 */

namespace Archipelago.Patches.Controller.Save {
    [HarmonyPatch(typeof(DataStore), "DeleteAutoSave")]
    public static class Delete {
        public static bool Prefix(ref string AutoSaveFilename) {
            AutoSaveFilename = Util.SaveFile();
            return true;
        }
    }
}
