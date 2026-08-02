using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;

/** Replace a ternary condition in the source to bind custom paint types to big paint spots */
namespace Archipelago.Patches.Controller.Paint.Bullet {
    [HarmonyPatch(typeof(GameManager), nameof(GameManager.PaintSpot))]
    public static class Spot {
        private const string NAME = "Big Paint Spots for Universal Water";

        // the condition by which is replaced the vanilla ternary. Can add more types if needed
        private static bool UseSmallTexture(
            PaintType type
        ) => type != PaintType.StickyPaint && type != PaintType.BouncyPaint && !Util.CustomPaintTypes.Contains(type);

        // A ref to pass when injecting the instruction
        private static readonly MethodInfo useSmallTextureRef = AccessTools.Method(typeof(Spot), nameof(UseSmallTexture));

        // Find instructions starting the vanilla branch (paint != StickyPaint)
        private static CodeMatch[] GetBranchCodes() => new[] {
            new CodeMatch(OpCodes.Ldarg_2),
            new CodeMatch(OpCodes.Brfalse)
        };

        // Find the instruction's label of the small paint call (true branch)
        private static Label FindCallLabel(TranspilerHelper helper, string name) {
            helper.ForwardsThrows(new CodeMatch(i => 
                (i.opcode == OpCodes.Callvirt || i.opcode == OpCodes.Call) && 
                i.operand?.ToString().Contains(name) == true
            ));
            helper.BackwardsThrows(new CodeMatch(OpCodes.Ldarg_0));

            return helper.Matcher.Labels[0];
        }

        // Find the range of instructions between the branching and the first branch
        private static int[] GetStartEnd(TranspilerHelper helper) {
            helper.Matcher.Start();
            if (!helper.Forward(GetBranchCodes())) return null;

            var start = helper.Matcher.Pos;
            if (!helper.Forward(new CodeMatch(OpCodes.Ldarg_0))) return null;

            return new int[] { start, helper.Matcher.Pos };
        }

        // Inject a call to our custom condition instead of the vanilla condition
        private static void ReplaceBranchCondition(TranspilerHelper helper, int[] startEnd, Label label) {
            helper.Matcher.Start().Advance(startEnd[0]).RemoveInstructions(startEnd[1] - startEnd[0]).Insert(
                new CodeInstruction(OpCodes.Ldarg_2), // loading paint type
                new CodeInstruction(OpCodes.Call, useSmallTextureRef), // condition injection
                new CodeInstruction(OpCodes.Brtrue, label) // jump if true
            );
        }

        // Inject our condition for the texture call
        private static bool PatchPaintSpotBranch(TranspilerHelper helper) {
            Label label;
            try {
                label = FindCallLabel(helper, "GetPaintTextureSmall");
            } catch (System.Exception e) {
                Core.Static.Error(e.Message);
                return false;
            }

            var startEnd = GetStartEnd(helper);
            if (startEnd is null) return false;

            ReplaceBranchCondition(helper, startEnd, label);
            return true;
        }

        // Inject our condition for the array call
        private static bool PatchPixelsBranch(TranspilerHelper helper) {
            Label label;
            try {
                label = FindCallLabel(helper, "GetPaintSmallPixels");
            } catch (System.Exception e) {
                Core.Static.Error(e.Message);
                return false;
            }

            var startEnd = GetStartEnd(helper);
            if (startEnd is null) return false;

            ReplaceBranchCondition(helper, startEnd, label);
            return true;
        }

        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator) {
            var helper = new TranspilerHelper(NAME, instructions, generator);

            // Only write our instructions if both patches are successfully applied
            if (!PatchPaintSpotBranch(helper)) return instructions;
            if (!PatchPixelsBranch(helper)) return instructions;
            
            return helper.Matcher.InstructionEnumeration();
        }
    }
}
