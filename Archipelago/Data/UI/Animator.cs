using UnityEngine;

namespace Archipelago.Data.UI {
    internal static class Animator {
        internal static RuntimeAnimatorController Backpack { get; private set; }

        internal static void Load(AssetBundle bundle) {
            Backpack = bundle.LoadAsset<RuntimeAnimatorController>("Backpack_Archipelago");
        }
    }
}
