using System;
using System.Collections.Generic;
using Archipelago.Data.Items;
using TSKGames.Inputs;

namespace Archipelago.Data {
    internal static class TrapController {
        private static readonly Random rng = new Random();
        private static readonly InputGamepadButton[] buttons = (InputGamepadButton[])Util.ShootButtons.Clone();

        internal static InputGamepadButton AlwaysShoot { get; private set; } = InputGamepadButton.None;
        internal static PaintType FeetState { get; private set; } = PaintType.None;

        private static readonly Dictionary<InputGamepadButton, InputGamepadButton> Mapping = new Dictionary<InputGamepadButton, InputGamepadButton> {
            {GameManager.BUTTON_WATER, GameManager.BUTTON_WATER},
            {GameManager.BUTTON_STICKY, GameManager.BUTTON_STICKY},
            {GameManager.BUTTON_BOUNCY, GameManager.BUTTON_BOUNCY}
        };

        internal static InputGamepadButton GetMapped(InputGamepadButton btn) {
            if (!Mapping.ContainsKey(btn)) return InputGamepadButton.None;
            return Mapping[btn];
        }

        // Fisher Yates
        internal static void SetRandomMapping() {
            var n = buttons.Length;
            while (n > 1) {
                var k = rng.Next(n);
                n--;

                var x = buttons[n];
                buttons[n] = buttons[k];
                buttons[k] = x;
            }

            Mapping[GameManager.BUTTON_WATER] = buttons[0];
            Mapping[GameManager.BUTTON_STICKY] = buttons[1];
            Mapping[GameManager.BUTTON_BOUNCY] = buttons[2];
        }

        internal static void SetRandomAlwaysAction() {
            var buttons = new List<InputGamepadButton>();

            if (Powers.HasWater) buttons.Add(GameManager.BUTTON_WATER);
            if (Powers.HasSticky) buttons.Add(GameManager.BUTTON_STICKY);
            if (Powers.HasBouncy) buttons.Add(GameManager.BUTTON_BOUNCY);

            var n = buttons.Count;
            if (n == 0) return;
            
            AlwaysShoot = buttons[rng.Next(n)];
        }

        internal static void StickyFeet() { FeetState = PaintType.StickyPaint; }
        internal static void BouncyFeet() { FeetState = PaintType.BouncyPaint; }
        internal static void ToxinkFeet() { FeetState = PaintType.AntiWater; }
    }
}