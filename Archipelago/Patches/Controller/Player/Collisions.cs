using Archipelago.Data;
using HarmonyLib;
using UnityEngine;

namespace Archipelago.Patches.Controller.Player {
    [HarmonyPatch(typeof(PlayerController), "CheckGround")]
    public static class Collisions {
        private static uint infection = 0;

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
                    Fields.trail.Invoke(__instance, new object[] { TrailAnchor.Bottom });
					__instance.Feedback_PlayJump(PaintType.BouncyPaint, __instance.transform.position);

                    var collider = ((Collider)Fields.groundCollider.GetValue(__instance)).GetComponent<TilePaint>();
                    if ((bool)collider) {
                        collider.AnimateBounce();
                    }

                    __instance.State = PlayerState.GroundBouncing;
                    break;
            }
  
            Fields.groundState.SetValue(__instance, TrapController.FeetState);
            return;
        }
    }
}
