using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using UnityEngine;

namespace Archipelago.Patches.Controller.Paint.Bullet {
    [HarmonyPatch(typeof(PaintBullet), "InitializeBulletPools")]
    public static class Init {
        private static Texture2D pollutedMask;
        private static Color _maskColor;
        private static Color MaskColor() {
            if (_maskColor == Color.clear) _maskColor = pollutedMask.GetPixels().First((pixel) => pixel.a > .1f);
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

        internal static void Load(AssetBundle bundle) {
            pollutedMask = bundle.LoadAsset<Texture2D>("SpotPollued_mask");
        }

        public static void Postfix() {
            Pool.speedPool = new PaintBullet[PaintBullet.GD.BulletPoolCount];
            Pool.pollutedPool = new PaintBullet[PaintBullet.GD.BulletPoolCount];
            GameData.Instance.PaintColors[2] = new Color(.42f, .32f, 96f, 1f); // speedink

            var mColor = MaskColor();
            System.Console.WriteLine(mColor);
            FillIfNeeded(GameData.Instance.PaintColors, new Color(.35f, .85f, .7f));
            FillIfNeeded(GameData.Instance.PaintMaskColors, mColor);
            FillIfNeeded(GameData.Instance.PaintMaskTextures, pollutedMask);

            if (!GameData.MaskColorIndices.ContainsKey(mColor))
                GameData.MaskColorIndices.Add(mColor, (int)Util.PollutedWater);
            
            for (int i = 0; i < Pool.speedPool.Length; i++) {
                InitBullet(Pool.speedPool, PaintType.SpeedyPaint, i);
                InitBullet(Pool.pollutedPool, Util.PollutedWater, i);
            }
        }
    }
}
