using System.Reflection;
using HarmonyLib;
using UnityEngine;

namespace Archipelago.Patches.Controller.Checkpoints {
    [HarmonyPatch(typeof(Checkpoint), "Validate")]
    public static class Reached {
        private static readonly MethodInfo notify = AccessTools.Method(typeof(Checkpoint), "NotifyNoResets");

        public static bool Prefix(
            Checkpoint __instance, 
            Animator ___anim,
            ref bool ___alreadyValidated,
            bool ___visual
        ) {
            if (___alreadyValidated) return false;
            if (!___visual || GameManager.Mode != GameMode.Standard) return true;

            if (Data.TrapController.CheckpointAmnesty) Data.TrapController.Free();
            Data.Locations.Checkpoint.Check(__instance.gameObject.name);

            var validated = !Data.Items.CheckpointItem.TriggerPrefix(__instance, ___anim);

            if (validated || Data.Items.CheckpointItem.seedOption < 2) __instance.ForceValidation();
            notify.Invoke(__instance, new object[] {});

            var state = "CheckpointAppear";
            ___anim.Play(validated ? state : (state + "_Locked"));
            if (validated) ___alreadyValidated = true;
            
            return false;
        }
    }
}