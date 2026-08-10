using HarmonyLib;

namespace Archipelago.Patches.UI.Camera {
    [HarmonyPatch(typeof(PlayerCamera), "PlayArenaAddEffect")]
    public static class PlayArena {
        public static bool Prefix(PlayerCamera __instance) {
            if (Data.TrapController.FeetState == PaintType.AntiWater) {
                __instance.PlayEffect("ArenaInfected", 1);
                return false;
            }

            Data.UI.Camera.ResetCurves(__instance);
            return true;
        }
    }
}