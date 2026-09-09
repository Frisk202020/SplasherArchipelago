using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;
using UnityEngine;

namespace Archipelago.Patches.Controller.Checkpoints {
    [HarmonyPatch(typeof(Checkpoint), "Start")]
    public static class StartVisual {
        private static void SetSprite(Checkpoint ckp, GameObject visual) {
            var sprite = Data.Items.CheckpointItem.PeekItemInfo(ckp.name);
            if (sprite == null) return;

            visual.GetComponent<SpriteRenderer>().sprite = sprite;
        }

        public static IEnumerable<CodeInstruction> Transpiler(
            IEnumerable<CodeInstruction> instructions, ILGenerator generator
        ) {
            var gen = new TranspilerHelper("Start Checkpoint Visual", instructions, generator);   
            if (!gen.Forward(
                OpCodes.Ldarg_0,
                OpCodes.Call,
                OpCodes.Callvirt,
                OpCodes.Stloc_1,
                OpCodes.Ldloca_S,
                OpCodes.Ldfld,
                OpCodes.Ldc_R4,
                OpCodes.Bge_Un
            )) return instructions;

            gen.Matcher.Insert(
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Ldloc_0),
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(StartVisual), nameof(SetSprite)))
            );  

            return gen.Matcher.InstructionEnumeration();   
        }
    }
}