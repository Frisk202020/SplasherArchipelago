using System.Collections.Generic;
using HarmonyLib;

namespace Manager.Patches.Cutscene {
    [HarmonyPatch(typeof(Trigger), "Start")]
    public static class OnStart {
        private static readonly Dictionary<string, string> Cutscenes = new Dictionary<string, string> {
            {"A1", "Docteur_PorteSplasher"},
            {"A_Boss", "Docteur_IntroBoss1"}, 
            {"B_Boss", "Docteur_IntroBoss2"}, 
            {"C_Boss", "Docteur_IntroBoss3"}
        };

        public static bool Prefix(Trigger __instance) {
            Data.Time.TryAccelerate(Cutscenes, __instance);
            return true;
        }
    }
}
