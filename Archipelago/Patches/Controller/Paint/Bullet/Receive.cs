using System.Collections.Generic;
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
        private static readonly HashSet<PaintType> customWater = new HashSet<PaintType> { PaintType.SpeedyPaint, Util.PollutedWater };

        public static bool Prefix(GameCharacter __instance, PaintBullet bullet) {
            if (!customWater.Contains(bullet.PaintType)) return true;

            switch(__instance.CurrentState) {
                case GameCharacter.State.StuckToCeiling:
                    var vCeiling = __instance.ColliderCenter + Vector3.up * ((Collider)collider.GetValue(__instance)).bounds.extents.y;
                    GameActor.GM.PaintSpot(vCeiling, bullet.PaintType);
                    GameActor.GM.PaintSpot(vCeiling + Vector3.left, bullet.PaintType);
                    GameActor.GM.PaintSpot(vCeiling + Vector3.right, bullet.PaintType);
                    Unstick.Invoke(__instance, noParams);
                    return false;

                case GameCharacter.State.StuckToWall:
                    var vector = __instance.ColliderCenter - Vector3.right * ((Collider)collider.GetValue(__instance)).bounds.extents.x * __instance.OnWall;
					GameActor.GM.PaintSpot(vector, bullet.PaintType);
					GameActor.GM.PaintSpot(vector + Vector3.up, bullet.PaintType);
					GameActor.GM.PaintSpot(vector + Vector3.down, bullet.PaintType);
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
