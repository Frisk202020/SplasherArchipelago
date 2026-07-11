using System;
using UnityEngine;

namespace Core {
    public static class Static {
        #region Variables
        public const string Game = "Splasher";
        public const string PluginIdRoot = "com.frisk.splasher";
        public const string VersionStr = "0.0.7";
        public static readonly Version Version = new Version(0, 0, 7);
        public const string VanillaSave = "Save1";
        private static GameObject persistentObject;
        public static GameObject PersistentObject() {
            if (persistentObject is null) {
                persistentObject = new GameObject("ArchipelagoPersist");
                UnityEngine.Object.DontDestroyOnLoad(persistentObject);
            }

            return persistentObject;
        }
        #endregion

        #region Logger
        private static readonly BepInEx.Logging.ManualLogSource logger = BepInEx.Logging.Logger.CreateLogSource("Archipelago");

        public static void Log(string msg) {
            logger.LogInfo(msg);
        }

        public static void Warn(string msg) {
            logger.LogWarning(msg);
        }

        public static void Error(string msg) {
            logger.LogError(msg);
        }
        #endregion

        #region ConfigEvent
        public delegate void ConfigHandler(Tools.Config config);
        public static event ConfigHandler OnConfigParsed;

        internal static void StartConfigEvents(Tools.Config conf) {
            OnConfigParsed(conf);
        }
        #endregion

        #region Bell
        public delegate void BellHandler();
        public static event BellHandler OnBellTriggered;

        internal static void StartBellEvents() {
            OnBellTriggered();
        }
        #endregion
    }
}
