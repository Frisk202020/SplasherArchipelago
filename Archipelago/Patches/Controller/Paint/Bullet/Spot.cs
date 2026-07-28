using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;

/** Replace a ternary condition in the source to bind custom paint types to big paint spots */
namespace Archipelago.Patches.Controller.Paint.Bullet {
    [HarmonyPatch(typeof(GameManager), nameof(GameManager.PaintSpot))]
    public static class Spot {
        // the condition by which is replaced the vanilla ternary. Can add more types if needed
        private static bool UseSmallTexture(
            PaintType type
        ) => type != PaintType.StickyPaint && type != PaintType.BouncyPaint && type != PaintType.SpeedyPaint;

        // A ref to pass when injecting the instruction
        private static readonly MethodInfo useSmallTextureRef = AccessTools.Method(typeof(Spot), nameof(UseSmallTexture));

        // Find instructions starting the vanilla branch (paint != StickyPaint)
        private static CodeMatch[] GetBranchCodes() => new[] {
            new CodeMatch(OpCodes.Ldarg_2),
            new CodeMatch(OpCodes.Brfalse)
        };

        // Find the instruction's label of the small paint call (true branch)
        private static Label FindCallLabel(CodeMatcher matcher, string name) {
            matcher.MatchStartForward(
                new CodeMatch(i => 
                    (i.opcode == OpCodes.Callvirt || i.opcode == OpCodes.Call) && 
                    i.operand?.ToString().Contains(name) == true
                )
            );

            if (matcher.IsInvalid) {
                throw new System.Exception($"Could not find {name} call");
            }

            matcher.MatchStartBackwards( new[] {
                new CodeMatch(OpCodes.Ldarg_0),
            });
            return matcher.Labels[0];
        }

        // Find the range of instructions between the branching and the first branch
        private static int[] GetStartEnd(CodeMatcher matcher) {
            matcher.Start().MatchStartForward(GetBranchCodes());
            if (matcher.IsInvalid) {
                Core.Static.Error("Failed to find condition start");
                return null;
            }

            var start = matcher.Pos;
            matcher.MatchStartForward(
                new CodeMatch(OpCodes.Ldarg_0)
            );

            if (matcher.IsInvalid) {
                Core.Static.Error("Failed to move to branch start");
                return null;
            }

            return new int[] { start, matcher.Pos };
        }

        // Inject a call to our custom condition instead of the vanilla condition
        private static void ReplaceBranchCondition(CodeMatcher matcher, int[] startEnd, Label label) {
            matcher.Start().Advance(startEnd[0]).RemoveInstructions(startEnd[1] - startEnd[0]).Insert(
                new CodeInstruction(OpCodes.Ldarg_2), // loading paint type
                new CodeInstruction(OpCodes.Call, useSmallTextureRef), // condition injection
                new CodeInstruction(OpCodes.Brtrue, label) // jump if true
            );
        }

        // Inject our condition for the texture call
        private static bool PatchPaintSpotBranch(CodeMatcher matcher) {
            Label label;
            try {
                label = FindCallLabel(matcher, "GetPaintTextureSmall");
            } catch (System.Exception e) {
                Core.Static.Error(e.Message);
                return false;
            }

            matcher.Start();
            var startEnd = GetStartEnd(matcher);
            if (startEnd is null) return false;

            ReplaceBranchCondition(matcher, startEnd, label);
            return true;
        }

        // Inject our condition for the array call
        private static bool PatchPixelsBranch(CodeMatcher matcher) {
            Label label;
            try {
                label = FindCallLabel(matcher, "GetPaintSmallPixels");
            } catch (System.Exception e) {
                Core.Static.Error(e.Message);
                return false;
            }

            matcher.MatchStartForward(GetBranchCodes());
            var startEnd = GetStartEnd(matcher);
            if (startEnd is null) return false;

            ReplaceBranchCondition(matcher, startEnd, label);
            return true;
        }

        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator) {
            var matcher = new CodeMatcher(instructions, generator);

            // Only write our instructions if both patches are successfully applied
            if (!PatchPaintSpotBranch(matcher)) return instructions;
            if (!PatchPixelsBranch(matcher)) return instructions;
            
            return matcher.InstructionEnumeration();
        }
    }
}
