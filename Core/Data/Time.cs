using Core.Patches.Cutscene;
using System.Collections.Generic;

namespace Core.Data {
    public static class Time {
        public static float TimeScale { get; internal set; } = 1;
        public static float UnlockScale { get; internal set; } = 1;

        internal static void TryAccelerate(Dictionary<string, string> whitelist, Trigger trigger) {
            var scene = GameData.Instance?.CurrentLevelMetaData?.SceneName;
            if (
                scene is null ||
                UnityEngine.Time.timeScale == TimeScale || 
                !whitelist.ContainsKey(scene) ||  
                trigger.name != whitelist[scene]
            ) return;

            Accelerate(TimeScale);

            var tracker = Static.PersistentObject().AddComponent<TrackDestroy>();
            tracker.Resolve = () => Clean();
            tracker.Track();
        }

        public static void Accelerate(float scale) {
            if (scale == UnityEngine.Time.timeScale) return;

            UnityEngine.Time.timeScale = scale;
            AudioManager.Instance?.SetMusicPitch(scale);
        }

        public static void Clean() {
            UnityEngine.Time.timeScale = 1;
            AudioManager.Instance?.SetMusicPitch(1);
        }
    }
}
