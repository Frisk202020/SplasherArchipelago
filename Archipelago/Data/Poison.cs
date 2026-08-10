namespace Archipelago.Data {
    internal static class Poison {
        private static bool poisoned = false;
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
    }
}