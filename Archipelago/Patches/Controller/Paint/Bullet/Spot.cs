using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;

namespace Archipelago.Patches.Controller.Paint.Bullet {
    [HarmonyPatch(typeof(GameManager), nameof(GameManager.PaintSpot))]
    public static class Spot {
        private static bool UseSmallTexture(
            PaintType type
        ) => type != PaintType.StickyPaint && type != PaintType.BouncyPaint && type != PaintType.SpeedyPaint;
        private static readonly MethodInfo useSmallTextureRef = AccessTools.Method(typeof(Spot), nameof(UseSmallTexture));

        private static CodeMatch[] GetBranchCodes() => new[] {
            new CodeMatch(OpCodes.Ldarg_2),
            new CodeMatch(OpCodes.Brfalse)
        };

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

        private static void ReplaceBranchCondition(CodeMatcher matcher, int[] startEnd, Label label) {
            matcher.Start().Advance(startEnd[0]).RemoveInstructions(startEnd[1] - startEnd[0]).Insert(
                new CodeInstruction(OpCodes.Ldarg_2),
                new CodeInstruction(OpCodes.Call, useSmallTextureRef),
                new CodeInstruction(OpCodes.Brtrue, label)
            );
        }

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
            if (!PatchPaintSpotBranch(matcher)) return instructions;
            if (!PatchPixelsBranch(matcher)) return instructions;
            
            return matcher.InstructionEnumeration();
        }
    }
}
