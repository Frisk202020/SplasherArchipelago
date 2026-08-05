using HarmonyLib;

namespace Archipelago.Patches.Controller.Damage {
    [HarmonyPatch(typeof(global::Splasher), "ReceivePaintBullet")]
    public static class Splasher {
        public static void Postfix(global::Splasher __instance, PaintBullet bullet) {
            if (bullet.PaintType == Util.PollutedWater) __instance.Die();
        }
    }
}