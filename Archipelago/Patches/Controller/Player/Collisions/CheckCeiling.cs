using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;

namespace Archipelago.Patches.Controller.Player.Collisions {
    [HarmonyPatch(typeof(PlayerController), "CheckCeiling")]
    public static class CheckCeiling {
        private const string NAME = "Paint ceiling collision";

        private static PaintType actualPaintType = PaintType.None;
        internal static PaintType ActualPaintType {
            get {
                var ret = actualPaintType;
                actualPaintType = PaintType.None;
                return ret;
            }
        }
        private static PaintType SaveDetectedType(PaintType paint) {
            actualPaintType = paint;
            return paint;
        } 

        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator) {
            var gen = new TranspilerHelper(NAME, instructions, generator);
            if (!gen.Forward(
                TranspilerHelper.MatchWithName(OpCodes.Callvirt, "GetPaintType") // call a stack-neutral method (consumes but returns consumed)
            )) return instructions;

            return gen.Matcher
                .Advance(1)
                .Insert(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(CheckCeiling), nameof(SaveDetectedType))))
                .InstructionEnumeration();
        }
    }
}