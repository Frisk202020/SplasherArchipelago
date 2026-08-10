using Archipelago.Data;
using HarmonyLib;

namespace Archipelago.Patches.Controller.Player.Collisions {
    [HarmonyPatch(typeof(PlayerController), "CheckWall")]
    public static class Wall {
        public static void Postfix(PlayerController __instance) {
            if (
                GameManager.LockControl == LockControlType.None &&
                __instance.PaintWall == Util.PollutedWater
            ) Poison.Die(__instance);
        }
    }
}