using Archipelago.Data;
using HarmonyLib;
using UnityEngine;

namespace Archipelago.Patches.Controller.Player.Collisions {
    [HarmonyPatch(typeof(PlayerController), "CheckGround")]
    public static class Ground {
        public static void Postfix(PlayerController __instance) {
            // assert player on ground and vulnerable
            if (
                __instance.State != PlayerState.Grounded || 
                GameManager.LockControl != LockControlType.None
            ) {
                Poison.EndInfection();
                return;
            }

            // check if a state should override feet state
            switch(__instance.PaintGround) {
                case PaintType.None: 
                    break;
                case Util.PollutedWater:
                    Poison.EndInfection();
                    Poison.Die(__instance);
                    return;
                default: 
                    Poison.EndInfection();
                    return;
            }
            
            // apply feet state
            switch(TrapController.FeetState) {
                case PaintType.None: return;
                case PaintType.AntiWater:
                    if (Poison.Infection == 180) {
                        Poison.EndInfection();
                        Poison.Die(__instance);
                        return;
                    }

                    if (Poison.Infection == 0) {
                        PlayerCamera.Instance.PlayEffect("Infection", 0);
                        Data.UI.Camera.UpdateCurves(PlayerCamera.Instance, 1, true);
                    } else if (Poison.Infection % 10 == 0) 
                        Data.UI.Camera.UpdateCurves(PlayerCamera.Instance, 1 - .5f * Poison.Infection / 180, false);

                    Poison.IncrementInfection();
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