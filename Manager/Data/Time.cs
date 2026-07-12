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

        internal static void TryAccelerate(Dictionary<string, string> whitelist, Trigger trigger) {
            var scene = GameData.Instance.CurrentLevelMetaData.SceneName;
            if (
                TimeScale <= 1 || 
                !whitelist.ContainsKey(scene) ||  
                trigger.name != whitelist[scene]
            ) return;

            Accelerate();
        }

        internal static void Accelerate() {
            UnityEngine.Time.timeScale = TimeScale;
            AudioManager.Instance?.SetMusicPitch(TimeScale);

            var tracker = Core.Static.PersistentObject().AddComponent<TrackDestroy>();
            tracker.Resolve = () => Clean();
            tracker.Track();
        }
    }
}
