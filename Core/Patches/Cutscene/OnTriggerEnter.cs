using System.Collections.Generic;
using HarmonyLib;

namespace Core.Patches.Cutscene {
    [HarmonyPatch(typeof(Trigger), "OnTriggerEnter")]
    public static class OnTriggerEnter {
        private static readonly Dictionary<string, string> Cutscenes = new Dictionary<string, string> {
            {"A1", "LD_PlayerTriggerBox (6)"}, 
            {"A_Boss", "CheckpointStartChase"}, 
            {"B_Boss", "LD_CameraZone (10)"}
        };

        public static bool Prefix(Trigger __instance) {
            Data.Time.TryAccelerate(Cutscenes, __instance);
            return true;
        }
    }
}
