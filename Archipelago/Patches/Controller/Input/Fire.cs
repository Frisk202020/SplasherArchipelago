using HarmonyLib;

namespace Archipelago.Patches.Controller.Input {
    [HarmonyPatch(typeof(SauceMachine), "Fire")]
    public static class Fire {
        public static void Postfix(PaintBullet __result) {
            if (__result != null && __result.PaintType == PaintType.Water) {
                __result.PaintType = PaintType.Water;
            }
        }
    }
}
