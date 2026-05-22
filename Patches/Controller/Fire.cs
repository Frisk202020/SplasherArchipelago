using HarmonyLib;
using System;
using TSKGames.Inputs;

namespace SplasherArchipelago.Patches.Controller {
    [HarmonyPatch(typeof(SauceMachine), "Fire")]
    public static class Fire {
        public static bool Prefix(InputGamepadButton button) {
            switch (button) {
                case InputGamepadButton.AltAction: return Data.Powers.HasWater; // only allow fire if has water, since not re-checked inside method
                case InputGamepadButton.Back: return Data.Powers.HasSticky;
                case InputGamepadButton.Menu: return Data.Powers.HasBouncy;
            }
            return true;
        }
    }
}