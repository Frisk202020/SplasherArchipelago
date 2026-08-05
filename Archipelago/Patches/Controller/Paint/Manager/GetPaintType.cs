using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;

namespace Archipelago.Patches.Controller.Paint.Manager {
    [HarmonyPatch(typeof(GameManager), "GetPaintType")]
    public static class GetPaintType {
        private const string NAME = "Manager Get Paint Type";

        private static bool TypeIsInLoop(int k) => k < 3 || k == (int)Util.PollutedWater;

        public static IEnumerable<CodeInstruction> Transpiler(
            IEnumerable<CodeInstruction> instructions, ILGenerator generator
        ) {
            var gen = new TranspilerHelper(NAME, instructions, generator);
            var bounds = (int)Util.PollutedWater + 1;

            // patch array alloc
            if (!gen.Forward(
                OpCodes.Stloc_0,
                OpCodes.Ldc_I4_6,
                OpCodes.Newarr
            )) return instructions;
            gen.Matcher.Advance(1).SetInstruction(
                new CodeInstruction(OpCodes.Ldc_I4, bounds)
            );

            // find target loop
            if (!gen.Forward(
                TranspilerHelper.MatchWithName(OpCodes.Ldfld, "DetectionMinPxelCount") //@cspell:disable-line
            )) return instructions;

            // find loop's end (after body)
            if (!gen.Forward(
                OpCodes.Ldloc_S,
                OpCodes.Ldc_I4_1,
                OpCodes.Add,
                OpCodes.Stloc_S
            )) return instructions;

            // find an instruction that loads k on stack
            var loadK = new CodeInstruction(gen.Matcher.InstructionAt(0));
            loadK.labels.Clear();

            // keep loop's end to perform early exit
            var continueLoop = gen.Matcher.Labels[0];

            // find loop's bounds
            if (!gen.Forward(
                OpCodes.Ldloc_S,
                OpCodes.Ldc_I4_3,
                OpCodes.Blt
            )) return instructions;

            // inject new loop bounds
            gen.Matcher.Advance(1).SetInstruction(
                new CodeInstruction(OpCodes.Ldc_I4, bounds)
            );

            // return to the head of the loop and inject k actual bounds
            if (!gen.Backwards(
                OpCodes.Ldc_I4_0,
                OpCodes.Stloc_S,
                OpCodes.Br
            )) return instructions;

            // steal loop's head
            gen.Matcher.Advance(3);
            loadK.MoveLabelsFrom(gen.Matcher.Instruction);

            gen.Matcher.Insert(
                loadK,
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(GetPaintType), nameof(TypeIsInLoop))),
                new CodeInstruction(OpCodes.Brfalse, continueLoop)
            );

            return gen.Matcher.InstructionEnumeration();
        }
    }
}