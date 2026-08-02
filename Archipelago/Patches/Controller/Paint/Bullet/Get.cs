using System.Collections.Generic;
using System.Linq;
using HarmonyLib;

namespace Archipelago.Patches.Controller.Paint.Bullet {
    [HarmonyPatch(typeof(PaintBullet), "GetBullet")]
    public static class Get {
        public static bool Prefix(PaintType p, ref PaintBullet __result) {
            PaintBullet[] arr = null;
            switch(p) {
                case PaintType.SpeedyPaint: arr = Pool.speedPool; break;
                case Util.PollutedWater: arr = Pool.pollutedPool; break;
                default: return true;
            }

            var bullet = arr.FirstOrDefault(x => !x.gameObject.activeInHierarchy);
            if (bullet != null && bullet.trailRenderer == null) bullet.PaintType = p;

            __result = bullet;
            return false;
        }
    }
}
