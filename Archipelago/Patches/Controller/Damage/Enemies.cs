using System.Reflection;
using HarmonyLib;
using UnityEngine;

namespace Archipelago.Patches.Controller.Damage {
    [HarmonyPatch(typeof(ElectronicTrigger), "BulletTake")]
    public static class Enemies {
        private static int GetDamage(PaintType paint, bool waterproof) {
            switch(paint) {
                case Util.PollutedWater: return waterproof ? 0 : 1;
                case PaintType.Water: return waterproof ? 0 : 4;
                case PaintType.SpeedyPaint: return 8;
                default: return 0;
            }
        }

        public static bool Prefix(ElectronicTrigger __instance, PaintBullet b) {
            if (__instance.RemainingLife == 0) return false;
            if (b.PaintType == PaintType.AntiWater) return true;

            var damage = GetDamage(b.PaintType, __instance.Waterproof);
            if (damage > 0) __instance.Hit(damage, b.Direction);

            return false;
        }
    }
}
