using System.Collections.Generic;
using Manager.Patches.Cutscene;

namespace Manager.Data {
    internal static class Time {
        internal static float TimeScale = 1;
        internal static bool SpeedOnCredits = false;

        private static void Clean() {
            UnityEngine.Time.timeScale = 1;
            AudioManager.Instance?.SetMusicPitch(1);
        }

        internal static void TryAccelerate(HashSet<string> whitelist, Trigger trigger) {
            if (TimeScale <= 1 || !whitelist.Contains(trigger.name)) return;

            UnityEngine.Time.timeScale = TimeScale;
            AudioManager.Instance?.SetMusicPitch(TimeScale);

            var tracker = Core.Static.PersistentObject().AddComponent<TrackDestroy>();
            tracker.Resolve = () => Clean();
            tracker.Track();
        }
    }
}
