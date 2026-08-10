namespace Archipelago.Data {
    internal static class Poison {
        private static bool poisoned = false;
        internal static uint Infection { get; private set; } = 0;

        internal static bool Poisoned() {
            if (poisoned) {
                poisoned = false;
                return true;
            }

            return false;
        }
        
        internal static void Die(PlayerController instance) {
            poisoned = true;
            instance.Die();
        } 

        internal static void IncrementInfection() => Infection++;

        internal static void EndInfection() {
            if (Infection == 0) return;

            Infection = 0;
            UI.Camera.ResetCurves(PlayerCamera.Instance);
            PlayerCamera.Instance.PlayEffect("EndInfection", 0);
        }
    }
}