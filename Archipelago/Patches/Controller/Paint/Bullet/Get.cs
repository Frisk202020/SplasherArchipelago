using System.Linq;
using HarmonyLib;

namespace Archipelago.Patches.Controller.Paint.Bullet {
    [HarmonyPatch(typeof(PaintBullet), "GetBullet")]
    public static class Get {
        public static bool Prefix(PaintType p, ref PaintBullet __result) {
            if (p != PaintType.SpeedyPaint) return true;

            var bullet = Pool.speedPool.FirstOrDefault(x => !x.gameObject.activeInHierarchy);
            if (bullet != null && bullet.trailRenderer == null) bullet.PaintType = p;

            __result = bullet;
            return false;
        }
    }
}
