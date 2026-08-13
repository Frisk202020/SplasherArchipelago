using Core.Patches.Cutscene;
using System;
using System.Collections.Generic;

namespace Core.Data {
    public static class Time {
        internal class Cutscene {
            internal string name;
            internal float? maxSpeed;

            public static implicit operator Cutscene(string value) => new Cutscene { name = value };
        }

        public static float TimeScale { get; internal set; } = 1;
        public static float UnlockScale { get; internal set; } = 1;

        internal static void TryAccelerate(Dictionary<string, Cutscene> whitelist, Trigger trigger) {
            var scene = GameData.Instance?.CurrentLevelMetaData?.SceneName;
            if (
                scene is null ||
                UnityEngine.Time.timeScale == TimeScale || 
                !whitelist.ContainsKey(scene) ||  
                trigger.name != whitelist[scene].name
            ) return;

            Accelerate(whitelist[scene].maxSpeed == null
                ? TimeScale
                : Math.Min(TimeScale, whitelist[scene].maxSpeed.Value)
            );

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
