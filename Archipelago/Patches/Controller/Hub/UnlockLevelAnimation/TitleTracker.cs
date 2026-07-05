using HarmonyLib;

namespace SplasherArchipelago.Patches.Controller.Hub.UnlockLevelAnimation {
    [HarmonyPatch(typeof(HUD), "PlayShowUp")]
    public static class TitleTracker {
        public static void Postfix(HUD __instance) {
            if (!global::Hub.IsLoaded) return;

            var tracker = __instance.gameObject.AddComponent<Helpers.AnimationTracker>();
            tracker.ResolveAction = () => {
                var routine = global::Hub.Instance.gameObject.AddComponent<UnlockRoutine>();
                routine.StartRoutine();
            };

            tracker.StartTrack(__instance.levelName);
        }
    }
}
