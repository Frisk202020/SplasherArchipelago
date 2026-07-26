using HarmonyLib;
using UnityEngine;

namespace Archipelago.Patches.Controller.Player {
    [HarmonyPatch(typeof(PlayerController), "UpdateVelocity")]
    public static class Velocity {
        public static bool Prefix(PlayerController __instance) {
            var paint = (PaintType)Fields.groundState.GetValue(__instance);
            if (
                (PositionFreezeType)Fields.freeze.GetValue(__instance) != PositionFreezeType.None || 
                (bool)Fields.autoStickCorner.GetValue(__instance) ||
                paint != PaintType.SpeedyPaint
            ) return true;

            var v = (Vector3)Fields.velocity.GetValue(__instance);
            var control = ((Vector2)Fields.leftStickAxis.GetValue(__instance)).x == 0f
                ? __instance.CD.GroundControlStop
                : (__instance.OnWind
                    ? __instance.CD.GroundControlWind
                    : __instance.CD.GroundControl
                );

            var x = Mathf.Lerp(
                __instance.Velocity.x, 
                ((Vector2)Fields.leftStickSign.GetValue(__instance)).x * __instance.CD.RunSpeed * 2,
                control * Time.deltaTime * (float)Fields.bounceControl.GetValue(__instance)
            );

            Fields.velocity.SetValue(__instance, new Vector3(x, v.y, v.z));
            return false;
        }
    }
}
