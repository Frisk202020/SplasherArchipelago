using Archipelago.Helpers.Assets;
using TSKGames.Inputs;
using UnityEngine;

namespace Archipelago.Public {
    [Loader]
    public class CheckpointTrigger : MonoBehaviour {
        const uint RADIUS = 8;

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

        public static void Init(Checkpoint c) {
            var next = Data.CheckpointTable.Next(c.gameObject.name);
            if (next is null) return;

            var obj = c.gameObject.AddComponent<CheckpointTrigger>();
            var nextObj = GameObject.Find(next);

            obj.checkpointPosition = c.transform.position;
            obj.target = nextObj.transform.position;
            obj.next = next;

            var button = Instantiate(Button_Start, c.transform);
            button.SetActive(true);

            obj.spriteRenderer = button.GetComponent<SpriteRenderer>();
            obj.spriteRenderer.enabled = false;
        }

        private void Update() {
            if (!Data.Items.CheckpointItem.Unlocked(next)) return;

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