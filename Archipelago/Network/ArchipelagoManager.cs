using Archipelago.MultiClient.Net;
using Archipelago.Data.Locations;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Archipelago.Network {
    static class ArchipelagoManager {
        private static readonly Version version = new Version(0, 6, 7);
        private static Helpers.FailableSession session;
        private static bool enabled = false;

        private static Dictionary<string, object> slotData;

        internal static bool SaveLoaded { get; private set; } = false;
        internal static void FinalizeSaveLoading() {
            SaveLoaded = true;

            if ((long)slotData["include_keys"] == 0) {
                Data.Items.LevelKeys.UnlockAll();
            } else {
                Data.Items.LevelKeys.UnlockFirst();
            }

            RestoreCheckedLocations();
            Items.ItemManager.CollectPending();
        }

        private static void InitSession(Core.Tools.Config conf) {
            Data.Items.LevelKeys.ShowName = conf.ShowLevelTitle.Value;
            
            var targetAddress = new Helpers.Address { domain = conf.Address.Value, port = (int)conf.Port.Value };
            var session = ArchipelagoSessionFactory.CreateSession(conf.Proxy.Value ? $"ws://localhost:8080" : targetAddress.ToString());

            session.Items.ItemReceived += (recvItemHelper) => {
                if (SaveLoaded) {
                    Items.ItemManager.Collect(recvItemHelper.DequeueItem());
                } else {
                    Items.ItemManager.Enqueue(recvItemHelper.DequeueItem());
                }
            };

            session.Locations.CheckedLocationsUpdated += (recvLocHelper) => {
                foreach(var loc in recvLocHelper) {
                    Restore(loc);
                }
            };

            ArchipelagoManager.session = new Helpers.FailableSession(
                session, conf.Slot.Value, version,
                conf.Proxy.Value ? targetAddress : null
            );
        }

        internal static bool Start(Core.Tools.Config conf) {
            if (enabled) return true;
            if (session is null) InitSession(conf);

            try {
                if (!session.FirstConnection()) return false;
            } catch (Exception e) {
                Core.Static.Log($"Failed to initialize Archipelago : {e.Message}");
                return false;
            }

            enabled = true;

            var bundle = AssetBundle.LoadFromFile("BepInEx/assets/archipelago");
            Data.UI.Animator.Load(bundle);
            Data.UI.Sprites.Load(bundle);
            bundle.Unload(false);

            slotData = ApplyOptions();
            Core.Static.DataStoreBlacklist.Add(Util.SaveFileExtension());
            Data.SaveData.Init();

            Util.Harmony.PatchAll();
            GameData.Initialized = false;
            GameData.Instance.InitializePlayerData();
            Hub.Load();
            return true;
        }

        private static Dictionary<string, object> ApplyOptions() {
            return session.ApplyOptions();
        }

        private static void Restore(long locId) {
            int id = (int)(locId - Util.BaseId);
            if (id < 0) return;

            var type = LocationExtensions.FindRange(id);
            switch (type) {
                case LocationType.Water: Powers.RestoreWater(); break;
                case LocationType.Stickink: Powers.RestoreStickink(); break;
                case LocationType.Bouncink: Powers.RestoreBouncink(); break;
                case LocationType.Splasher: Splashers.Restore(id); break;
                case LocationType.Clear: Clears.Restore(id); break;
                case LocationType.Bronze: Speedrun.Restore(LocationType.Bronze, id); break;
                case LocationType.Silver: Speedrun.Restore(LocationType.Silver, id); break;
                case LocationType.Gold: Speedrun.Restore(LocationType.Gold, id); break;
                case LocationType.Platinum: Speedrun.Restore(LocationType.Platinum, id); break;
            }
        }

        public static void RestoreCheckedLocations() {
            session.Execute((session) => {
                foreach (var loc in session.Locations.AllLocationsChecked) {
                    Restore(loc);
                }
            });
        }

        internal static void Check(LocationType loc, long id) {
            session.Execute(session => session.Locations.CompleteLocationChecks(Util.BaseId + (int)loc + id));
        }

        internal static void SendDeathLink() {
            session.SendDeathLink();
        }

        internal static void Victory() {
            session.Execute(session => session.SetGoalAchieved());
        }
    }
}