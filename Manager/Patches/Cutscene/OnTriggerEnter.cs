using System.Collections.Generic;
using HarmonyLib;

namespace Manager.Patches.Cutscene {
    [HarmonyPatch(typeof(Trigger), "OnTriggerEnter")]
    public static class OnTriggerEnter {
        private static readonly HashSet<string> Cutscenes = new HashSet<string> {
            "LD_PlayerTriggerBox (6)"
        };

        public static bool Prefix(Trigger __instance) {
            Data.Time.TryAccelerate(Cutscenes, __instance);
            return true;
        }
    }
}
