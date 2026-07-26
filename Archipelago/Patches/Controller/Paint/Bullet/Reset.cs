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

        public static void Postfix() {
            foreach(var x in Pool.speedPool.Where(Predicate())) { x.gameObject.SetActive(false); }
        }
    }
}
