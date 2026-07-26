using HarmonyLib;
using UnityEngine;

namespace Archipelago.Patches.Controller.Paint.Bullet {
    [HarmonyPatch(typeof(PaintBullet), "InitializeBulletPools")]
    public static class Init {
        public static void Postfix() {
            Pool.speedPool = new PaintBullet[PaintBullet.GD.BulletPoolCount];
            GameData.Instance.PaintColors[2] = new Color(.85f, .56f, 1f, 1f);

            for (int i = 0; i < Pool.speedPool.Length; i++) {
                Pool.speedPool[i] = Object.Instantiate(PaintBullet.GD.PrefabPaintBullet);
                Pool.speedPool[i].PaintType = PaintType.SpeedyPaint;
                Pool.speedPool[i].gameObject.SetActive(false);
            }
        }
    }
}
