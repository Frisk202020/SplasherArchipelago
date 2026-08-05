using System.Collections.Generic;
using System.Reflection.Emit;
using Archipelago.Data;
using Archipelago.Data.Items;
using HarmonyLib;
using TSKGames.Inputs;

namespace Archipelago.Patches.Controller.Player {
    [HarmonyPatch(typeof(PlayerController), "CheckGround")]
    public static class BouncySpotGround {
        private const string NAME = "Prevent not-allowed ground bouncy spot";

        private static CodeMatch[] CallMachineMethod(string name) => new[] {
            new CodeMatch(OpCodes.Ldarg_0),
            new CodeMatch(OpCodes.Call),
            TranspilerHelper.MatchWithName(OpCodes.Callvirt, name),
            new CodeMatch(OpCodes.Brfalse)
        };

        private static bool Authorized() =>
            InputGamePadMgr.GetButton(TrapController.GetMapped(GameManager.BUTTON_BOUNCY)) &&
            Powers.HasBouncy;

        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator) {
            var gen = new TranspilerHelper(NAME, instructions, generator);
            if (!gen.Forward(
                CallMachineMethod("get_CanShoot")
                .AddRangeToArray(CallMachineMethod("get_bouncyPaint"))
            )) return instructions;

            if (!gen.Forward(
                new CodeMatch(OpCodes.Ldc_I4_2),
                TranspilerHelper.MatchWithName(OpCodes.Call, "GetButton")
            )) return instructions;


            gen.Matcher.RemoveInstructions(2).Insert(
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(BouncySpotGround), nameof(Authorized)))
            );
            return gen.Matcher.InstructionEnumeration();
        }
    }
}
