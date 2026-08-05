using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;

namespace Archipelago.Patches {
    internal class TranspilerHelper {
        internal static CodeMatch MatchWithName(OpCode code, string subName) => new CodeMatch(
            i => i.opcode == code && i.operand?.ToString().Contains(subName) == true
        );

        private readonly string name;
        public bool errors = true;
        private string Error => $"Failed to apply transpiler patch on : {name}";

        internal CodeMatcher Matcher { get; private set; }

        private bool Result() {
            if (Matcher.IsInvalid) {
                if (errors) Core.Static.Error(Error);
                return false;
            }

            return true;
        }

        internal TranspilerHelper(string name, IEnumerable<CodeInstruction> instructions, ILGenerator generator) {
            this.name = name;
            Matcher = new CodeMatcher(instructions, generator);
        }

        internal bool Forward(params CodeMatch[] pattern) {
            Matcher.MatchStartForward(pattern);
            return Result();
        }

        internal bool Backwards(params CodeMatch[] pattern) {
            Matcher.MatchStartBackwards(pattern);
            return Result();
        }

        internal void ForwardsThrows(params CodeMatch[] pattern) {
            Matcher.MatchStartForward(pattern);
            if (Matcher.IsInvalid) throw new System.Exception(Error);
        }

        internal void BackwardsThrows(params CodeMatch[] pattern) {
            Matcher.MatchStartBackwards(pattern);
            if (Matcher.IsInvalid) throw new System.Exception(Error);
        }
    }
}
