using HarmonyLib;
using System;
using TSKGames.Inputs;

namespace SplasherArchipelago.Patches.Controller {
    [HarmonyPatch(typeof(SauceMachine), "Fire")]
    public static class Fire {
        public static bool Prefix(InputGamepadButton button) {
            switch (button) {
                case InputGamepadButton.AltAction: return Data.Items.Powers.HasWater; // only allow fire if has water, since not re-checked inside method
                case InputGamepadButton.Back: return Data.Items.Powers.HasSticky;
                case InputGamepadButton.Menu: return Data.Items.Powers.HasBouncy;
            }
            return true;
        }
    }
}