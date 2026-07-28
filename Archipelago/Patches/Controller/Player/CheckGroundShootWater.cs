using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using Archipelago.Data.Items;
using HarmonyLib;
using TSKGames.Inputs;

namespace Archipelago.Patches.Controller.Player {
    [HarmonyPatch(typeof(PlayerController), "CheckGround")]
    public static class CheckGroundShootWater {
        private static bool IsShootingWater(PlayerController player) => 
            (Powers.WaterLevel == WaterState.Clean || Powers.WaterLevel == WaterState.None) && 
            (InputGamepadButton)Fields.shootButton.GetValue(player) == GameManager.BUTTON_WATER;
        private static readonly MethodInfo ConditionRef = AccessTools.Method(typeof(CheckGroundShootWater), nameof(IsShootingWater));

        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator) {
            var gen = new CodeMatcher(instructions, generator);

            // find branching relative to shootButtonPressed == ALtAction
            gen.MatchStartForward(
                new CodeMatch(i => i.opcode == OpCodes.Ldfld && i.operand?.ToString().Contains("shootButtonPressed") == true),
                new CodeMatch(OpCodes.Ldc_I4_3),
                new CodeMatch(OpCodes.Ceq)
            );
            if (gen.IsInvalid) {
                Core.Static.Error("Failed to find AltAction branch");
                return instructions;
            }

            gen.RemoveInstructions(3).Insert(new CodeInstruction(OpCodes.Call, ConditionRef));
            return gen.InstructionEnumeration();
        }
    }
}
