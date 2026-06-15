using HarmonyLib;
using TSKGames.Save;
using System;

namespace SplasherArchipelago.Patches.Controller.Save {
    [HarmonyPatch(
        declaringType: typeof(DataStore), 
        methodName: "AutoSaveExist", 
        argumentTypes: new Type[] {typeof(string), typeof(DataStore.StringSlotSaveDelegate)}
    )]
    public static class Exists {
        public static bool Prefix(ref string AutoSaveFilename) {
            AutoSaveFilename = Util.SaveFile();
            return true;
        }
    }
}
