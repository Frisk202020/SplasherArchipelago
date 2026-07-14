using HarmonyLib;

/** 
 * Fix Refresh Leds logic, somehow not executed correctly with our mod active 
 * It is rewritten with less complex logic and removal of nested ternary, weak on older Mono runtime
 */

namespace Archipelago.Patches.Controller.Hub {
    [HarmonyPatch(typeof(Door), "RefreshLeds")]
    public static class PatchLeds {
        public static bool Prefix(Door __instance) {
            if (__instance.State == HubDoorState.Locked) return false;

            for (int i = 0; i < __instance.leds.Length; i++) {
                if (__instance.levelData.RescuedSplashers[i]) {
                    __instance.leds[i].sprite = __instance.levelData.GetRescuedSplashersCount() == __instance.levelData.RescuedSplashers.Length
                        ? GameActor.GD.HubData.DoorLedAll_Sprite
                        : GameActor.GD.HubData.DoorLedYes_Sprite;
                } else {
                    __instance.leds[i].sprite = GameActor.GD.HubData.DoorLedOff_Sprite;
                }
            }

            return false;
        }
    }
}
