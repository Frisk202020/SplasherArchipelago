using System.Collections.Generic;
using System.Reflection.Emit;
using Archipelago.Data.Items;
using HarmonyLib;
using TSKGames.Inputs;

namespace Archipelago.Patches.Controller.Player {
    [HarmonyPatch(typeof(PlayerController), "CheckGround")]
    public static class CheckGroundShootWater {
        private const string NAME = "Check Universal Water Paint Collisions";
        private static bool IsShootingWater(PlayerController player) => 
            (Powers.WaterLevel == WaterState.Clean || Powers.WaterLevel == WaterState.None) && 
            (InputGamepadButton)Fields.shootButton.GetValue(player) == GameManager.BUTTON_WATER;

        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator) {
            var helper = new TranspilerHelper(NAME, instructions, generator);

            // find branching relative to shootButtonPressed == ALtAction
            if (!helper.Forward(
                TranspilerHelper.MatchWithName(OpCodes.Ldfld, "shootButtonPressed"),
                new CodeMatch(OpCodes.Ldc_I4_3),
                new CodeMatch(OpCodes.Ceq)
            )) return instructions;

            helper.Matcher.RemoveInstructions(3).Insert(
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(CheckGroundShootWater), nameof(IsShootingWater)))
            );

            return helper.Matcher.InstructionEnumeration();
        }
    }
}
