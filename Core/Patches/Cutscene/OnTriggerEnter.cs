using System.Collections.Generic;
using HarmonyLib;

namespace Core.Patches.Cutscene {
    [HarmonyPatch(typeof(Trigger), "OnTriggerEnter")]
    public static class OnTriggerEnter {
        private static readonly Dictionary<string, Data.Time.Cutscene> Cutscenes = new Dictionary<string, Data.Time.Cutscene> {
            {"A1", "LD_PlayerTriggerBox (6)"}, 
            {"A_Boss", "CheckpointStartChase"}, 
            {"B_Boss", new Data.Time.Cutscene { name = "LD_CameraZone (10)", maxSpeed = 5 }}
        };

        public static bool Prefix(Trigger __instance) {
            Data.Time.TryAccelerate(Cutscenes, __instance);
            return true;
        }
    }
}
