using HarmonyLib;

namespace Archipelago.Patches.Controller.Checkpoints {
    [HarmonyPatch(typeof(Checkpoint), "Start")]
    public static class Start {
        public static void Postfix(Checkpoint __instance) {
            if (GameManager.Mode != GameMode.Standard) return;
            Public.CheckpointTrigger.Init(__instance);
        }
    }
}