using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Archipelago.Helpers.Assets {
    [AttributeUsage(AttributeTargets.Class)]
    class LoaderAttribute : Attribute {
        internal static void LoadAll(AssetBundle bundle) {
            var assembly = Assembly.GetExecutingAssembly();
            Type[] types = assembly.GetTypes();

            foreach(var type in types) {
                if (!type.GetCustomAttributes(typeof(LoaderAttribute), false).Any()) continue;

                type.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).ForEach((f) => {
                    f.GetCustomAttributes(typeof(AssetAttribute), false)
                        .Select((attr) => (AssetAttribute)attr)
                        .Select((attr) => {
                            var name = attr.AssetName ?? f.Name;
                            if (f.FieldType.IsArray) {
                                var arrType = f.FieldType.GetElementType();
                                var x = bundle.LoadAssetWithSubAssets(name, arrType);

                                if (x != null && x.Length > 0) {
                                    var arr = Array.CreateInstance(arrType, x.Length);
                                    Array.Copy(x, arr, x.Length);

                                    f.SetValue(null, arr);
                                } else return name;                                
                            } else {
                                var x = bundle.LoadAsset(name, f.FieldType);

                                if (x != null) f.SetValue(null, x);
                                else return name;
                            }
                            return null;
                        })
                        .ForEach((name) => {
                            if (name != null) Core.Static.Warn($"Failed to load asset : {name}");
                        });
                });
            }
        }
    }
}