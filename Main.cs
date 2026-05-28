using Archipelago.MultiClient.Net;
using BepInEx;
using HarmonyLib;
using System;

namespace SplasherArchipelago {
    [BepInPlugin(pluginId, "SplasherArchipelago", "0.0.1")]
    public class Main : BaseUnityPlugin {
        public const string pluginId = "com.frisk.splahser_archipelago";

        public void Awake() {
            var connectResult = Network.InternalArchipelagoManager.Init();

            if (connectResult is LoginFailure error) {
                string msg = $"Failed to connect to server\n";
                foreach (string err in error.Errors) { 
                    msg += $"{err}\n";
                }

                Logger.LogError(msg);
                return;
            }

            var success = (LoginSuccessful)connectResult;
            Network.InternalArchipelagoManager.Slot = success.Slot;

            Data.Items.LevelKeys.UnlockAll();

            var harmony = new Harmony(pluginId);
            harmony.PatchAll();

            Logger.LogMessage("Archipelago Loaded !");

            Network.InternalArchipelagoManager.ApplyOptions();
            Network.InternalArchipelagoManager.ReceiveAllItems();
        }
    }
}