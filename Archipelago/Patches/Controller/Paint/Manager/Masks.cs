using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;

namespace Archipelago.Patches.Controller.Paint.Manager {
    [HarmonyPatch(typeof(GameManager), "GetMaskPixels")]
    public static class Masks {
        private const string NAME = "Mask arrays";

        private static bool PatchArrayLength(TranspilerHelper helper) {
            if (!helper.Forward(
                new CodeMatch(OpCodes.Ldarg_0),
                new CodeMatch(i => i.LoadsConstant(9)),
                new CodeMatch(OpCodes.Newarr)
            )) return false;

            helper.Matcher.Advance(1);
            helper.Matcher
                .RemoveInstruction()
                .Insert(new CodeInstruction(OpCodes.Ldc_I4_S, 10));

            return true;
        }

        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator) {
            var gen = new TranspilerHelper(NAME, instructions, generator) { errors = false };
            var patched = 0;

            while (PatchArrayLength(gen)) { patched++; }
            if (patched == 3) return gen.Matcher.InstructionEnumeration();

            Core.Static.Error($"[{NAME}] Found {patched} arrays, expected 3.");
            return instructions;
        }
    }
}