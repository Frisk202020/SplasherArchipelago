using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.Enums;
using BepInEx;
using HarmonyLib;
using System;

namespace SplasherArchipelago {
    [BepInPlugin(pluginId, "SplasherArchipelago", "0.0.1")]
    public class Main : BaseUnityPlugin {
        private static readonly string game = "Splasher";
        private static readonly Version version = new Version(0, 6, 7);
        public const string pluginId = "com.frisk.splahser_archipelago";

        public void Awake() {
            var domain = "localhost";
            var port = 38281;
            var session = ArchipelagoSessionFactory.CreateSession(domain, port);

            var player = "Frisk";
            var connectResult = session.TryConnectAndLogin(
                game, player, ItemsHandlingFlags.AllItems,
                version, null, null, null, true
            );

            if (connectResult is LoginFailure error) {
                string msg = $"Failed to connect to {domain}:{port} as {player}\n";
                foreach (string err in error.Errors) { 
                    msg += $"{err}\n";
                }

                Logger.LogError(msg);
                return;
            }

            var success = (LoginSuccessful)connectResult;
            Logger.LogInfo(success);

            var harmony = new Harmony(pluginId);
            harmony.PatchAll();
            Logger.LogMessage("Archipelago Loaded !");
        }
    }
}