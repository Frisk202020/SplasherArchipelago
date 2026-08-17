using System;

namespace Archipelago.Helpers.Assets {
    [AttributeUsage(AttributeTargets.Field)]
    class AssetAttribute : Attribute {
        internal string AssetName { get; private set; }
        internal AssetAttribute(string name=null) {
            AssetName = name;
        }
    }
}