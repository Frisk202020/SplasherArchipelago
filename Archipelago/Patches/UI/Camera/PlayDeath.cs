using HarmonyLib;

namespace Archipelago.Patches.UI.Camera {
    [HarmonyPatch(typeof(PlayerCamera), "PlayDeathEffect")]
    public static class PlayDeath {
        public static bool Prefix(PlayerCamera __instance) {
            if (Data.Poison.Poisoned()) {
                Data.UI.Camera.UpdateCurves(__instance, .5f, true);
                __instance.PlayEffect("Poisoned", 0);
                return false;
            }

            return true;
        }
    }
}