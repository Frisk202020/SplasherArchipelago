using HarmonyLib;

namespace Archipelago.Patches.Controller.Damage {
    [HarmonyPatch(typeof(ElectronicTrigger), "BulletTake")]
    public static class Enemies {
        public static bool Prefix(ElectronicTrigger __instance, PaintBullet b) {
            if (__instance.RemainingLife == 0) return false;

            switch(b.PaintType) {
                case PaintType.Water:
                case PaintType.SpeedyPaint:
                    if (!__instance.Waterproof) __instance.Hit(1, b.Direction);
                    return false;
                case PaintType.AntiWater:
                    return true;
            }

            return false;
        }
    }
}
