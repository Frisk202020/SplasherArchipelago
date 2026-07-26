using Archipelago.Data;
using Archipelago.Data.Items;
using HarmonyLib;
using TSKGames.Inputs;

/**
 * Prevent use of paint gun buttons if not unlocked and swap paints if needed.
 */

namespace Archipelago.Patches.Controller.Paint {
    [HarmonyPatch(typeof(PlayerController), "UpdateInputParameters")]
    public static class Button {
        private static InputGamepadButton GetButton() {
            foreach(var btn in Util.ShootButtons) {
                if (InputGamePadMgr.GetButton(btn)) return TrapController.GetMapped(btn);
            }

            return TrapController.AlwaysShoot;
        }

        private static bool IsAllowed(InputGamepadButton button) {
            switch(button) {
                case GameManager.BUTTON_WATER: return Powers.HasWater;
                case GameManager.BUTTON_STICKY: return Powers.HasSticky;
                case GameManager.BUTTON_BOUNCY: return Powers.HasBouncy;
                default: return true;
            }
        }

        public static void Postfix(PlayerController __instance) {
            var button = GetButton();    
            AccessTools.DeclaredField(typeof(PlayerController), "shootButtonPressed").SetValue(
                __instance, IsAllowed(button) ? button : TrapController.AlwaysShoot
            );
        }
    }
}
