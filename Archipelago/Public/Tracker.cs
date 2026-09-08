using UnityEngine;
using UnityEngine.UI;

namespace Archipelago.Public {
    public class Tracker : MonoBehaviour {
        internal static uint Delay = 300;

        private uint? count = null;
        private GameObject trackerObject = null;
        private Text trackerText = null;

        internal void Init(GameObject _trackerObject, Text _trackerText) {
            trackerObject = _trackerObject; trackerText = _trackerText;
        }

        private void Update() {
            if (count != null) {
                count++;
                
                if (count == Delay) {
                    count = null;
                    trackerObject.gameObject.SetActive(false);
                } else return;
            }
                
            var msg = Data.UI.Tracker.Get();
            if (msg == null) return;

            if (trackerText == null) {
                Core.Static.Warn("The tracker can't be found");
                return;
            }

            count = 0;
            trackerObject.gameObject.SetActive(true);
            trackerText.text = msg;
            GameActor.GM.AudioManager.PlayFX(GameActor.GD.SoundData.SoundEffect_Yeah);
        }
    }
}