using HarmonyLib;

namespace SplasherArchipelago.Patches.Controller {
    [HarmonyPatch(typeof(StarManager), "OnCheckpoint")]
    public static class Essence {
        public static bool Prefix(StarManager __instance) {
            if (GameManager.HasCollectables && __instance.CurrentScore < Data.Items.Essence.MAX) {
                __instance.Add((int)Data.Items.Essence.Release());
            }
            return true;
        }
    }
}
