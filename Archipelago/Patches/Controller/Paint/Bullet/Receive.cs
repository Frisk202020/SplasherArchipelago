using System.Reflection;
using HarmonyLib;
using UnityEngine;

namespace Archipelago.Patches.Controller.Paint.Bullet {
    [HarmonyPatch(typeof(GameCharacter), nameof(GameCharacter.ReceivePaintBullet))]
    public static class Receive {
        private static readonly FieldInfo collider = AccessTools.DeclaredField(typeof(GameCharacter), "colliderComponent");
        private static readonly MethodInfo Unstick = AccessTools.Method(typeof(GameCharacter), "UnstickFromWall");
        private static readonly MethodInfo State = AccessTools.DeclaredPropertySetter(typeof(GameCharacter), "CurrentState");
        private static readonly object[] noParams = new object[] {};

        public static bool Prefix(GameCharacter __instance, PaintBullet bullet) {
            if (bullet.PaintType != PaintType.SpeedyPaint) return true;

            switch(__instance.CurrentState) {
                case GameCharacter.State.StuckToCeiling:
                    var vCeiling = __instance.ColliderCenter + Vector3.up * ((Collider)collider.GetValue(__instance)).bounds.extents.y;
                    GameActor.GM.PaintSpot(vCeiling, PaintType.SpeedyPaint);
                    GameActor.GM.PaintSpot(vCeiling + Vector3.left, PaintType.SpeedyPaint);
                    GameActor.GM.PaintSpot(vCeiling + Vector3.right, PaintType.SpeedyPaint);
                    Unstick.Invoke(__instance, noParams);
                    return false;

                case GameCharacter.State.StuckToWall:
                    var vector = __instance.ColliderCenter - Vector3.right * ((Collider)collider.GetValue(__instance)).bounds.extents.x * __instance.OnWall;
					GameActor.GM.PaintSpot(vector, PaintType.SpeedyPaint);
					GameActor.GM.PaintSpot(vector + Vector3.up, PaintType.SpeedyPaint);
					GameActor.GM.PaintSpot(vector + Vector3.down, PaintType.SpeedyPaint);
                    Unstick.Invoke(__instance, noParams);
                    return false;

                case GameCharacter.State.Glued:
                    State.Invoke(__instance, new object[] { GameCharacter.State.Aiming });
                    return false;
                    
                default: return true;
            }
        }
    }
}
