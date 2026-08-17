using System.Reflection;
using HarmonyLib;
using UnityEngine;

/**
 * This patch allows to increment the death counter when the player dies.
 * This is useful for death link. The logic of wether the death should actually increase the counter is managed in data.
 */

namespace Archipelago.Patches.Controller {
    [HarmonyPatch(typeof(PlayerController), "Die")]
    public static class Die {
        private static readonly FieldInfo ckpVisual = AccessTools.DeclaredField(typeof(global::Checkpoint), "anim");

        public static bool Prefix() {
            Data.Death.AddDeath();
            Data.Poison.EndInfection();
            Data.Items.CheckpointItem.DisableLockedCheckpoints();

            return true;
        }
    }
}
