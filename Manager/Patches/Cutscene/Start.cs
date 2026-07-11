using System.Collections.Generic;
using HarmonyLib;

namespace Manager.Patches.Cutscene {
    [HarmonyPatch(typeof(Trigger), "Start")]
    public static class Start {
        private static readonly HashSet<string> Cutscenes = new HashSet<string> {
            "Docteur_IntroBoss1", "Docteur_IntroBoss2", "Docteur_IntroBoss3"
        };
        public static bool Prefix(Trigger __instance) {
            if (!Cutscenes.Contains(__instance.name)) return true;

            UnityEngine.Time.timeScale = Data.Time.TimeScale;

            var x = __instance.gameObject.AddComponent<TrackDestroy>();
            x.Resolve = () => Data.Time.Clean();
            x.Track();
            
            return true;
        }
    }
}
