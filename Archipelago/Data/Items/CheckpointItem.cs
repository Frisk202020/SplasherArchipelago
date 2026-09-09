using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Archipelago.Helpers.Assets;
using Archipelago.MultiClient.Net.Enums;
using HarmonyLib;
using UnityEngine;

namespace Archipelago.Data.Items {
    [Loader]
    internal static class CheckpointItem {
        internal const uint NO_ITEMS = 1;
        internal const uint USEFUL = 2;
        internal const uint PROGRESSION = 3;

        [Asset]
        private static RuntimeAnimatorController CheckpointAnimController = null;
        [Asset]
        private static Sprite Checkpoint_Progression = null;
        [Asset]
        private static Sprite Checkpoint_Useful = null;
        [Asset]
        private static Sprite Checkpoint_Filler = null;
        [Asset]
        private static Sprite Checkpoint_Trap = null;

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

        internal static Sprite PeekItemInfo(string name) {
            var id = CheckpointTable.LocationId(name);
            var item = Network.ArchipelagoManager.LocationItemInfo(Locations.LocationType.Checkpoint, id);

            if (item.ItemGame == Core.Static.Game && item.ItemName.Contains("Checkpoint")) return null;
            switch (item.Flags) {
                case ItemFlags.Advancement: return Checkpoint_Progression;
                case ItemFlags.NeverExclude: return Checkpoint_Useful;
                case ItemFlags.None: return Checkpoint_Filler;
                case ItemFlags.Trap: return Checkpoint_Trap;
                default: return null;
            }
        }

        private static PatchInfo Info(string name) {
            if (seedOption <= NO_ITEMS || table.Get(name))
                return new PatchInfo { languageKey = "saved", color = new Color(1, 1, 1) };

            return seedOption == USEFUL
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

        internal static void Collect(string scene, int id) => table.Check(id, scene);
        internal static void CollectLevel(string scene) => table.CheckLevel(scene);
        internal static bool Unlocked(string name) => table.Get(name);
    }
}