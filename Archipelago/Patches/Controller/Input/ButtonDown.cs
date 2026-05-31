using HarmonyLib;
using TSKGames.Inputs;

namespace SplasherArchipelago.Patches.Controller.Input {
    [HarmonyPatch(typeof(InputGamePadMgr), "GetButtonDown")]
    public static class ButtonDown {
        public static bool Prefix(InputGamepadButton button) {
            switch(button) {
                case GameManager.BUTTON_WATER: return Data.Items.Powers.HasWater;
                case GameManager.BUTTON_STICKY: return Data.Items.Powers.HasSticky;
                case GameManager.BUTTON_BOUNCY: return Data.Items.Powers.HasBouncy;
            }

            return true;
        }
    }
}
