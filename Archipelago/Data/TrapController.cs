using System;
using System.Collections.Generic;
using TSKGames.Inputs;

namespace Archipelago.Data {
    public static class TrapController {
        private static readonly Random rng = new Random();
        private static readonly InputGamepadButton[] buttons = (InputGamepadButton[])Util.ShootButtons.Clone();

        private static Dictionary<InputGamepadButton, InputGamepadButton> Mapping = new Dictionary<InputGamepadButton, InputGamepadButton> {
            {GameManager.BUTTON_WATER, GameManager.BUTTON_WATER},
            {GameManager.BUTTON_STICKY, GameManager.BUTTON_STICKY},
            {GameManager.BUTTON_BOUNCY, GameManager.BUTTON_BOUNCY}
        };

        public static InputGamepadButton GetMapped(InputGamepadButton btn) {
            if (!Mapping.ContainsKey(btn)) return InputGamepadButton.None;
            return Mapping[btn];
        }

        // Fisher Yates
        public static void SetRandomMapping() {
            var n = buttons.Length;
            while (n > 1) {
                var k = rng.Next(n);
                n--;

                var x = buttons[n];
                buttons[n] = buttons[k];
                buttons[k] = x;
            }

            foreach(var x in buttons) {
                System.Console.WriteLine(x);
            }

            Mapping[GameManager.BUTTON_WATER] = buttons[0];
            Mapping[GameManager.BUTTON_STICKY] = buttons[1];
            Mapping[GameManager.BUTTON_BOUNCY] = buttons[2];
        }
    }
}