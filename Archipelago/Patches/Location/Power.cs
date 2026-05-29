using HarmonyLib;
using System;

namespace SplasherArchipelago.Patches.Location {
    [HarmonyPatch(typeof(CheckpointNewPower), "CoroutineGivePower")]
    public static class Power {
        public static bool Prefix(CheckpointNewPower __instance) {
            switch(__instance.power) {
                case CheckpointNewPower.PaintPower.Water:
                    Data.Locations.Powers.CheckWater();
                    break;
                case CheckpointNewPower.PaintPower.Sticky:
                    Data.Locations.Powers.CheckStickink();
                    break;
                case CheckpointNewPower.PaintPower.Bouncy:
                    Data.Locations.Powers.CheckBouncink();
                    break;
            }
            return true;
        }
    }
}
