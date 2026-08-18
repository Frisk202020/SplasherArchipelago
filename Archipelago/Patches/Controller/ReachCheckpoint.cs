using System.Reflection;
using HarmonyLib;
using UnityEngine;

namespace Archipelago.Patches.Controller.Checkpoint {
    [HarmonyPatch(typeof(global::Checkpoint), "Validate")]
    public static class Reached {
        private static readonly MethodInfo notify = AccessTools.Method(typeof(global::Checkpoint), "NotifyNoResets");

        public static bool Prefix(
            global::Checkpoint __instance, 
            Animator ___anim,
            ref bool ___alreadyValidated,
            bool ___visual
        ) {
            if (___alreadyValidated) return false;
            if (!___visual) return true;

            if (Data.TrapController.CheckpointAmnesty) Data.TrapController.Free();
            Data.Locations.Checkpoint.Check(__instance.gameObject.name);

            var validated = !Data.Items.CheckpointItem.TriggerPrefix(__instance, ___anim);
            
            if (validated) __instance.ForceValidation();
            if (validated || Data.Items.CheckpointItem.seedOption < 2) notify.Invoke(__instance, new object[] {});

            var state = "CheckpointAppear";
            ___anim.Play(validated ? state : (state + "_Locked"));
            if (validated) ___alreadyValidated = true;
            
            return false;
        }
    }
}