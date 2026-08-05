using HarmonyLib;

namespace Archipelago.Patches.Controller.Player.Collisions {
    [HarmonyPatch(typeof(PlayerController), "CheckCeiling")]
    public static class ApplyCeiling {
        public static void Postfix(PlayerController __instance) {
            if (CheckCeiling.ActualPaintType == Util.PollutedWater)
                __instance.Die();
        }
    }
}