using HarmonyLib;
using TSKGames.Save;

/**
 * Use a save dedicated to the current Archipelago seed instead of the default one.
 */

namespace Archipelago.Patches.Controller.Save {
    [HarmonyPatch(typeof(DataStore), "LoadAutoSave")]
    public static class Load {
        public static bool Prefix(ref string AutoSaveFilename) {
            if (AutoSaveFilename != Core.Static.VanillaSave) return true;

            AutoSaveFilename = Util.SaveFile();
            return true;
        }
    }
}