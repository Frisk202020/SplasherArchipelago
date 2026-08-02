using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;

namespace Archipelago.Patches.Controller.Paint.Bullet {
    [HarmonyPatch(typeof(PaintBullet), "InitializeBulletPools")]
    public static class Init {
        private static void InitBullet(PaintBullet[] array, PaintType type, int index) {
            array[index] = Object.Instantiate(PaintBullet.GD.PrefabPaintBullet);
            array[index].PaintType = type;
            array[index].gameObject.SetActive(false);
        }

        private static void FillWithVoid<T>(List<T> l) {
            while (l.Count < (int)Util.PollutedWater) {
                l.Add(default);
            }
        }

        public static void Postfix() {
            Pool.speedPool = new PaintBullet[PaintBullet.GD.BulletPoolCount];
            Pool.pollutedPool = new PaintBullet[PaintBullet.GD.BulletPoolCount];

            GameData.Instance.PaintColors[2] = new Color(.42f, .32f, 96f, 1f);

            FillWithVoid(GameData.Instance.PaintColors);
            FillWithVoid(GameData.Instance.PaintMaskColors);
            FillWithVoid(GameData.Instance.PaintMaskTextures);        

            GameData.Instance.PaintColors.Add(new Color(.35f, .85f, .7f));
            GameData.Instance.PaintMaskTextures.Add(GameData.Instance.PaintMaskTextures[0]);

            for (int i = 0; i < Pool.speedPool.Length; i++) {
                InitBullet(Pool.speedPool, PaintType.SpeedyPaint, i);
                InitBullet(Pool.pollutedPool, Util.PollutedWater, i);
            }
        }
    }
}
