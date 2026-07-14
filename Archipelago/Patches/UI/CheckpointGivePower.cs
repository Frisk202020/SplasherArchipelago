using HarmonyLib;
using System.Reflection;

namespace Archipelago.Patches.UI {
    [HarmonyPatch]
    public static class CheckpointGivePower {
        [HarmonyTargetMethod]
        public static MethodBase TargetMethod() {
            return AccessTools.Method(typeof(CheckpointNewPower).GetNestedType("<CoroutineGivePower>c__Iterator1", BindingFlags.NonPublic), "MoveNext");
        }

        [HarmonyPostfix]
        public static void Postfix(bool __result) {
            if (__result) return;
            Backpack.Update();
        }
    }
}
