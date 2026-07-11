using System.Collections.Generic;
using HarmonyLib;

namespace Manager.Patches.Cutscene {
    [HarmonyPatch(typeof(Trigger), "Start")]
    public static class OnStart {
        private static readonly HashSet<string> Cutscenes = new HashSet<string> {
            "Docteur_PorteSplasher",
            "Docteur_IntroBoss1", "Docteur_IntroBoss2", "Docteur_IntroBoss3"
        };

        public static bool Prefix(Trigger __instance) {
            Data.Time.TryAccelerate(Cutscenes, __instance);
            return true;
        }
    }
}
