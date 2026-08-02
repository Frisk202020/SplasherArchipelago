using System;
using System.Linq;
using HarmonyLib;

namespace Archipelago.Patches.Controller.Paint.Bullet {
    [HarmonyPatch(typeof(PaintBullet), "ResetPool")]
    public static class Reset {
        private static Func<PaintBullet, bool> Predicate() {
            return (bool)PaintBullet.GM.CurrentResetGroup
                ? new Func<PaintBullet, bool>(x => x.gameObject.activeInHierarchy && PaintBullet.GM.CurrentResetGroup.BoundsContainsPosition(x.transform.position))
                : new Func<PaintBullet, bool>(x => x.gameObject.activeInHierarchy);
        }

        private static void ResetType(ref PaintBullet[] arr) {
            foreach (var x in arr.Where(Predicate())) { x.gameObject.SetActive(false); }
        }

        public static void Postfix() {
            ResetType(ref Pool.speedPool);
            ResetType(ref Pool.pollutedPool);
        }
    }
}
