using HarmonyLib;
using UnityEngine;

namespace SplasherManager.Patches {
    [HarmonyPatch(declaringType: typeof(AudioSource), methodName: "Play", argumentTypes: new System.Type[] {})]
    public static class SoundManager {
        public static bool Prefix(AudioSource __instance) {
            if (Time.timeScale > 1) Data.Time.PatchAudio(__instance); 

            return true;
        }
    }
}
