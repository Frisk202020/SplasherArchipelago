using HarmonyLib;
using UnityEngine;

namespace Archipelago.Patches.Controller.Player {
    [HarmonyPatch(typeof(PlayerController), "UpdateVelocity")]
    public static class Velocity {
        private static float getControl(PlayerController player) => player.LeftJoystickAxis.x == 0f
            ? player.CD.GroundControlStop
            : (player.OnWind
                ? player.CD.GroundControlWind
                : player.CD.GroundControl
            );

        internal static void SpeedinkVelocity(PlayerController player) {
            var v = player.Velocity;
            if ((bool)Fields.autoStickCorner.GetValue(player)) {
                if (v.x == 0f) return;

                var num = Mathf.Sign(v.x);
                v.x = Mathf.Lerp(v.x, 2 * num * player.CD.RunSpeed, getControl(player) * Time.deltaTime);
                return;
            }
            
            v.x = Mathf.Lerp(
                v.x, 
                ((Vector2)Fields.leftStickSign.GetValue(player)).x * player.CD.RunSpeed * 2,
                getControl(player) * Time.deltaTime * (float)Fields.bounceControl.GetValue(player)
            );
            player.Velocity = v;
        }

        public static bool Prefix(PlayerController __instance) {
            var paint = (PaintType)Fields.groundState.GetValue(__instance);
            if (
                (PositionFreezeType)Fields.freeze.GetValue(__instance) != PositionFreezeType.None || 
                paint != PaintType.SpeedyPaint
            ) return true;

            SpeedinkVelocity(__instance);
            return false;
        }
    }
}
