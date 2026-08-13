using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;

namespace Archipelago.Patches.Controller.Damage {
    [HarmonyPatch(typeof(BossBubon), "IsColliderValid")]
    public static class Bubon {
        private const string NAME = "Bubon Damage For Universal Water";
        private static bool IsPaintValid(PaintType p) {
            return Data.Items.Powers.ProgressiveWater
                ? p == PaintType.SpeedyPaint
                : p == PaintType.Water;
        }

        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator) {
            var helper = new TranspilerHelper(NAME, instructions, generator);

            if (!helper.Forward(
                new CodeMatch(OpCodes.Ldarg_0),
                new CodeMatch(OpCodes.Ldc_R4),
                TranspilerHelper.MatchWithName(OpCodes.Stfld, "callElapsedTime")
            )) return instructions;
            
            var label = helper.Matcher.Labels[0];
            if (!helper.Backwards(
                TranspilerHelper.MatchWithName(OpCodes.Callvirt, "get_PaintType"),
                new CodeMatch(OpCodes.Ldc_I4_6),
                new CodeMatch(OpCodes.Beq)
            )) return instructions;

            helper.Matcher.Advance(1).RemoveInstructions(2).Insert(
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Bubon), nameof(IsPaintValid))),
                new CodeInstruction(OpCodes.Brtrue, label)
            );

            return helper.Matcher.InstructionEnumeration();
        }
    }
}
