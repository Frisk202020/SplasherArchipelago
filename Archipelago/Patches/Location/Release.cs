using HarmonyLib;

/**
 * Detect the game's ending (early, because the level actually clears after credits so we can't rely on ExitLevel).
 */

namespace Archipelago.Patches.Location {
    [HarmonyPatch(typeof(OutroScene), "Start")]
    public static class Release {
        public static bool Prefix() {
            Data.Items.Freedom.Free();

            return true;
        }
    }
}
