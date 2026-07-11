using System.Collections.Generic;
using UnityEngine;

namespace Manager.Data {
    internal static class Time {
        internal static float TimeScale = 1;
        internal static bool SpeedOnCredits = false;

        private static readonly List<AudioSource> patchedAudio = new List<AudioSource>();

        internal static void PatchAudio(AudioSource source) {
            patchedAudio.Add(source);
            source.pitch = TimeScale;
        }

        internal static void Clean() {
            UnityEngine.Time.timeScale = 1;
            foreach(var audio in patchedAudio) {
                if (audio == null) continue;
                audio.pitch = 1;
            }

            patchedAudio.Clear();
        }
    }
}
