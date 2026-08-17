using System.Collections.Generic;
using System.Linq;
using Archipelago.Helpers.Assets;
using HarmonyLib;
using UnityEngine;

namespace Archipelago.Patches.Controller.Paint.Bullet {
    [HarmonyPatch(typeof(PaintBullet), "InitializeBulletPools")]
    [Loader]
    public static class Init {
        [Asset]
        private static Texture2D SpotPollued_mask = null;
        private static Color _maskColor;
        private static Color MaskColor() {
            if (_maskColor == Color.clear) _maskColor = SpotPollued_mask.GetPixels().First((pixel) => pixel.a > .1f);
            return _maskColor;
        }

        private static void InitBullet(PaintBullet[] array, PaintType type, int index) {
            array[index] = Object.Instantiate(PaintBullet.GD.PrefabPaintBullet);
            array[index].PaintType = type;
            array[index].gameObject.SetActive(false);
        }

        private static void FillIfNeeded<T>(List<T> l, T toAdd) {
            if (l.Count == (int)Util.PollutedWater + 1) return;

            while (l.Count < (int)Util.PollutedWater) {
                l.Add(default);
            }
            l.Add(toAdd);
        }

        public static void Postfix() {
            Pool.speedPool = new PaintBullet[PaintBullet.GD.BulletPoolCount];
            Pool.pollutedPool = new PaintBullet[PaintBullet.GD.BulletPoolCount];
            GameData.Instance.PaintColors[2] = new Color(.42f, .32f, 96f, 1f); // speedink

            var mColor = MaskColor();
            FillIfNeeded(GameData.Instance.PaintColors, new Color(.35f, .85f, .7f));
            FillIfNeeded(GameData.Instance.PaintMaskColors, mColor);
            FillIfNeeded(GameData.Instance.PaintMaskTextures, SpotPollued_mask);

            if (!GameData.MaskColorIndices.ContainsKey(mColor))
                GameData.MaskColorIndices.Add(mColor, (int)Util.PollutedWater);
            
            for (int i = 0; i < Pool.speedPool.Length; i++) {
                InitBullet(Pool.speedPool, PaintType.SpeedyPaint, i);
                InitBullet(Pool.pollutedPool, Util.PollutedWater, i);
            }
        }
    }
}
