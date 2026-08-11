using HarmonyLib;

namespace Archipelago.Patches.Controller {
    [HarmonyPatch(typeof(Checkpoint), "ForceValidation")]
    public static class CheckpointReached {
        public static bool Prefix() {
            if (Data.TrapController.CheckpointAmnesty) Data.TrapController.Free();
            return true;
        }
    }
}