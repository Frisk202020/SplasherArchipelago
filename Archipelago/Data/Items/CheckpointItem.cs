using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using UnityEngine;

namespace Archipelago.Data.Items {
    [Helpers.Assets.Loader]
    internal static class CheckpointItem {
        [Helpers.Assets.Asset]
        private static RuntimeAnimatorController CheckpointAnimController = null;
        private static readonly FieldInfo ckpVisual = AccessTools.DeclaredField(typeof(Checkpoint), "anim");
        private static readonly List<Checkpoint> lockedEnabled = new List<Checkpoint>();
        internal static void DisableLockedCheckpoints() {
            foreach(var c in lockedEnabled.Where(x => x != null)) {
                ((Animator)ckpVisual.GetValue(c)).Play("CheckpointIdle");
            }
            lockedEnabled.Clear();
        }

        private class PatchInfo {
            public string languageKey;
            public Color? color = null;
            public bool locked = false;
        }

        internal static int seedOption = 0;
        const string CATEGORY = "ArchipelagoCheckpoint";

        private static readonly CheckpointTable table = new CheckpointTable();

        private static PatchInfo Info(string name) {
            if (seedOption == 0 || table.Get(name))
                return new PatchInfo { languageKey = "saved", color = new Color(1, 1, 1) };

            return seedOption == 1
                ? new PatchInfo { languageKey = "checked", color = new Color(1, 1, .81f), locked = true }
                : new PatchInfo { languageKey = "missing", color = new Color(.76f, .13f, 0), locked = true };
        }

        internal static bool TriggerPrefix(Checkpoint instance, Animator visual) {
            var mesh = visual.gameObject.transform
                .FindChild("Text")
                .GetComponent<TextMesh>();

            var info = Info(instance.gameObject.name);
            mesh.text = Helpers.Language.Get(CATEGORY, info.languageKey);
            if (info.color != null) mesh.color = info.color.Value;
            if (info.locked) lockedEnabled.Add(instance);

            visual.runtimeAnimatorController = CheckpointAnimController;
            return info.locked;
        }

        internal static void Collect(string scene, int id) {
            table.Check(CheckpointTable.NameById(id, scene), scene);
        }

        internal static bool Unlocked(string name) => table.Get(name);
    }
}