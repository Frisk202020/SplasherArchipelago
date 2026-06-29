

using HarmonyLib;

/**
 * Prevent DataStore.OnAutosave_Load defined in vanilla to run on custom saves
 * We whitelist vanilla files
 */
namespace SplasherManager.Patches {
    [HarmonyPatch(typeof(GameData), "DataStore_OnAutosaveLoad")]
    public static class OnSaveLoad {
        
        public static bool Prefix(string savename) {
            return savename != SplasherArchipelago.Shared.SaveFileExtension();
        }
    }
}
