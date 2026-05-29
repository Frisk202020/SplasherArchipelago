using HarmonyLib;

namespace SplasherArchipelago.Patches.Location {
    [HarmonyPatch(typeof(OutroScene), "Start")]
    public static class Release {
        public static bool Prefix() {
            Data.Items.Freedom.Free();
            return true;
        }
    }
}
