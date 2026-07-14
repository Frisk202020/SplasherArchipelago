using UnityEngine;

namespace Archipelago.Data.UI {
    internal static class Sprites {
        internal static Sprite Jauge { get; private set; }
        
        internal static void Load(AssetBundle bundle) {
            var assets = bundle.LoadAssetWithSubAssets<Sprite>("Jauge_NoCount.png");
            Jauge = assets[0];
        } 
    }
}
