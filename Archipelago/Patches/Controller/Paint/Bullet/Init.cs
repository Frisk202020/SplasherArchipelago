using HarmonyLib;
using UnityEngine;

namespace Archipelago.Patches.Controller.Paint.Bullet {
    [HarmonyPatch(typeof(PaintBullet), "InitializeBulletPools")]
    public static class Init {
        public static void Postfix() {
            Pool.speedPool = new PaintBullet[PaintBullet.GD.BulletPoolCount];
            GameData.Instance.PaintColors[2] = 
                new Color(.42f, .32f, 96f, 1f); // speed
                //new Color(.35f, .85f, .7f); // polluted

            for (int i = 0; i < Pool.speedPool.Length; i++) {
                Pool.speedPool[i] = Object.Instantiate(PaintBullet.GD.PrefabPaintBullet);
                Pool.speedPool[i].PaintType = PaintType.SpeedyPaint;
                Pool.speedPool[i].gameObject.SetActive(false);
            }
        }
    }
}
