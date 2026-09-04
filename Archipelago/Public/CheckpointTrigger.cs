using System.Linq;
using Archipelago.Helpers.Assets;
using TSKGames.Inputs;
using UnityEngine;

namespace Archipelago.Public {
    [Loader]
    public class CheckpointTrigger : MonoBehaviour {
        const uint RADIUS = 8;
        const string NAME_ROOT = "LD_Checkpoint";

        [Asset]
        private static GameObject Button_Start = null;
        private static bool samePress = false;

        private SpriteRenderer spriteRenderer;
        private Vector3 checkpointPosition;
        private Vector3 target;
        private string next;

        private static void UpdatePress() {
            if (!samePress || InputGamePadMgr.GetButton(InputGamepadButton.Select)) return;
            samePress = false;
        }

        private static void FixGame(string name) {
            switch(GameData.Instance.CurrentLevelMetaData.SceneName) {
                case "C1":
                    if (GameObject.Find("LD_Checkpoint6") != null) return;
                    FindObjectsOfType<GameObject>().FirstOrDefault(
                        x => x.name == "LD_Checkpoint5" && x.transform.position.x > 1450
                    ).name = "LD_Checkpoint6";
                    return;
            }
        }

        public static void Init(Checkpoint c) {
            if (!c.gameObject.name.Contains(NAME_ROOT)) return;
            FixGame(c.gameObject.name);

            var withParenthesis = c.gameObject.name.Contains("(1)");
            var next = Data.CheckpointTable.Next(c.gameObject.name);
            if (next == null) return;

            var obj = c.gameObject.AddComponent<CheckpointTrigger>();
            var nextObj = GameObject.Find(next + (withParenthesis ? " (1)" : ""));

            obj.checkpointPosition = c.transform.position;
            obj.target = nextObj.transform.position;
            obj.next = next;

            var button = Instantiate(Button_Start, c.transform);
            button.SetActive(true);

            obj.spriteRenderer = button.GetComponent<SpriteRenderer>();
            obj.spriteRenderer.enabled = false;
        }

        private void Update() {
            if (next == null || !Data.Items.CheckpointItem.Unlocked(next)) return;

            UpdatePress();
            if (samePress) return;

            var pos = PlayerController.Instance.transform.position;
            spriteRenderer.enabled = (pos - checkpointPosition).magnitude < RADIUS;

            if (spriteRenderer.enabled && InputGamePadMgr.GetButton(InputGamepadButton.Select)) {
                samePress = true;
                PlayerController.Instance?.TeleportTo(target);
            }
        }
    }
}