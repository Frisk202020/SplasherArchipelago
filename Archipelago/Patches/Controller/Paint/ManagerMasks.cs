using System.Reflection;
using HarmonyLib;
using UnityEngine;

namespace Archipelago.Patches.Controller.Paint {
    [HarmonyPatch(typeof(GameManager), "GetMaskPixels")]
    public static class ManagerMasks {
        private static readonly FieldInfo masks = AccessTools.DeclaredField(typeof(GameManager), "_paintMasks");
        private static readonly FieldInfo masksWidth = AccessTools.DeclaredField(typeof(GameManager), "_paintMaskWidth");
        private static readonly FieldInfo masksHeight = AccessTools.DeclaredField(typeof(GameManager), "_paintMaskHeight");

        private static void Init<T>(GameManager g, FieldInfo f) {
            var x = (T[])f.GetValue(g);
            if (x == null || x.Length == 9)
                f.SetValue(g, new T[9 + Util.CustomPaintTypes.Count]);
        }

        public static bool Prefix(GameManager __instance) { 
            Init<Color[]>(__instance, masks);
            Init<int>(__instance, masksWidth);
            Init<int>(__instance, masksHeight);

            return true;
        }
    }
}
