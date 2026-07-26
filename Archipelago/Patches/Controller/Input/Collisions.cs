using System.Reflection;
using Archipelago.Data;
using HarmonyLib;
using UnityEngine;

namespace Archipelago.Patches.Controller.Input {
    [HarmonyPatch(typeof(PlayerController), "CheckGround")]
    public static class Collisions {
        private static uint infection = 0;
        private static readonly FieldInfo groundState = AccessTools.DeclaredField(typeof(PlayerController), "paint_Ground");
        private static readonly FieldInfo groundCollider = AccessTools.DeclaredField(typeof(PlayerController), "groundCollider");
        private static readonly MethodInfo trail = AccessTools.DeclaredPropertySetter(typeof(PlayerController), "CurrenTrailAnchor");

        public static void Postfix(PlayerController __instance) {
            if (__instance.State != PlayerState.Grounded || GameManager.LockControl != LockControlType.None) {
                infection = 0;
                return;
            }
            
            switch(TrapController.FeetState) {
                case PaintType.None: return;

                case PaintType.AntiWater:
                    if (infection == 180) {
                        infection = 0;
                        __instance.Die();
                        return;
                    }

                    infection++;
                    break;

                case PaintType.BouncyPaint:
                    trail.Invoke(__instance, new object[] { TrailAnchor.Bottom });
					__instance.Feedback_PlayJump(PaintType.BouncyPaint, __instance.transform.position);

                    var collider = ((Collider)groundCollider.GetValue(__instance)).GetComponent<TilePaint>();
                    if ((bool)collider) {
                        collider.AnimateBounce();
                    }

                    __instance.State = PlayerState.GroundBouncing;
                    break;
            }
  
            groundState.SetValue(__instance, TrapController.FeetState);
            return;
        }
    }
}
