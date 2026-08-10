using HarmonyLib;

namespace Archipelago.Patches.UI.Camera {
    [HarmonyPatch(typeof(PlayerCamera), "Start")]
    public static class Animator {
        public static void Postfix(PlayerCamera __instance) {
            Data.UI.Camera.SetAnim(__instance);
        }
    }
}