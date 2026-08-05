using System.Reflection;
using HarmonyLib;

namespace Archipelago.Patches.Controller.Damage {
    [HarmonyPatch(typeof(ElectronicTrigger), "Start")]
    public static class Life {
        private static readonly MethodInfo SetLife = AccessTools.DeclaredPropertySetter(typeof(ElectronicTrigger), "RemainingLife");

        public static void Postfix(ElectronicTrigger __instance) {
            __instance.Life *= 4;
            SetLife.Invoke(__instance, new object[] { __instance.RemainingLife * 4 });
        }
    }
}