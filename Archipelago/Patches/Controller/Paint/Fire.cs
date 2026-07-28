using System.Reflection;
using Archipelago.Data.Items;
using HarmonyLib;
using TSKGames.Inputs;
using UnityEngine;

namespace Archipelago.Patches.Controller.Paint {
    [HarmonyPatch(typeof(SauceMachine), "Fire")]
    public static class Fire {
        private static readonly MethodInfo canShoot = AccessTools.DeclaredPropertySetter(typeof(SauceMachine), "CanShoot");
        private static readonly FieldInfo delay = AccessTools.DeclaredField(typeof(SauceMachine), "fireElapsedTime");

        private static PaintType GetPaint(InputGamepadButton button) {
            switch(button) {
                case GameManager.BUTTON_WATER: return Powers.WaterLevel == WaterState.Speedy ? PaintType.SpeedyPaint : PaintType.Water;
                case GameManager.BUTTON_STICKY: return PaintType.StickyPaint;
                case GameManager.BUTTON_BOUNCY: return PaintType.BouncyPaint;
                default: return PaintType.None;
            }
        }

        private static PaintBullet GetBullet(SauceMachine machine, InputGamepadButton button, Vector3 dir) {
            if (!machine.CanShoot) {
                return null;
            }

            var paint = GetPaint(button);
            if (paint == PaintType.None) return null;

            var bullet = PaintBullet.GetBullet(paint);
            if (!bullet) return null;

            bullet.transform.position = PlayerController.Instance.GunProjectileAnchor;
            bullet.Sender = PlayerController.Instance.gameObject;
            bullet.Launch(dir, machine.CD.BulletVelocity);
            canShoot.Invoke(machine, new object[] { false });
            delay.SetValue(machine, machine.CD.FireDelayMin);
            return bullet;
        }

        public static bool Prefix(SauceMachine __instance, InputGamepadButton button, Vector3 dir, ref PaintBullet __result) {
            __result = GetBullet(__instance, button, dir);
            return false;
        }
    }
}
