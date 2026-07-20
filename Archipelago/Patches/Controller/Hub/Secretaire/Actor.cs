using System.Collections;
using System.Reflection;
using HarmonyLib;
using UnityEngine;

/** 
 * Override Secretaire dialogue logic
 */
namespace Archipelago.Patches.Controller.Hub.Secretaire {
    public class Actor : GameActor {
        internal float ElapsedTime = 0f;
        internal int AngryCount = 0;
        internal bool CanDisplay { get; private set; } = true;
        internal bool Angry { get; private set; } = false;
        internal Coroutine displaying;
        private bool firstSentence = true;

        private readonly static FieldInfo bubbleAccess = AccessTools.DeclaredField(typeof(HubSecretaire), "buble");
        private readonly static FieldInfo audioSource = AccessTools.DeclaredField(typeof(HubSecretaire), "audioSource");
        private readonly static FieldInfo gameFinished = AccessTools.DeclaredField(typeof(HubSecretaire), "gameFinished");
        private readonly static FieldInfo animator = AccessTools.DeclaredField(typeof(HubSecretaire), "animator");
        public static Actor Instance { get; private set; }
        private const string ANGRY_TEXT = "...";
        private const string CATEGORY = "ArchipelagoSecretaire";

        public override void ResetActor() {}

        private void Awake() {
            Instance = this;
        }

        internal void Call(HubSecretaire self) {
            AngryCount++;
            ElapsedTime = 0f;
            if (!CanDisplay) return;

            if (displaying != null) {
                StopCoroutine(displaying);
                if (self.bubleText.text == ANGRY_TEXT || Angry) 
                    ((Animator)animator.GetValue(self)).CrossFadeInFixedTime("Idle", .2f);
            }

            displaying = StartCoroutine(Routine(self));
        }

        private IEnumerator Routine(HubSecretaire self) {
            CanDisplay = false;
            var id = GetStringId();

            switch (id) {
                case "0": 
                    var completion = Network.ArchipelagoManager.Completion();
                    self.bubleText.text = completion < 100f 
                        ? Helpers.Language.Get(CATEGORY, id, new[] { completion.ToString() })
                        : Helpers.Language.Get(CATEGORY, "finished"); 
                    break;
                case null: self.bubleText.text = ANGRY_TEXT; break;
                default: self.bubleText.text = Helpers.Language.Get(CATEGORY, id); break;
            }

            var bubbleAnim = (Animator)bubbleAccess.GetValue(self);
            var playOpenClose = (bool)bubbleAnim;
            if ((bool)bubbleAnim) bubbleAnim.Play("Open");

            var audioSourceInstance = (AudioSource)audioSource.GetValue(self);
            var gameFinishedInstance = (bool)gameFinished.GetValue(self);
            var anim = (Animator)animator.GetValue(self);

            if (self.bubleText.text == ANGRY_TEXT) self.soundPoop.ForceSetup(audioSourceInstance);
            else {
                if (!gameFinishedInstance) anim.CrossFadeInFixedTime("Detect", .2f);
                yield return new WaitForSeconds(.2f);

                if (Angry) self.soundAngry.ForceSetup(audioSourceInstance);
                else self.soundTalks.ForceSetup(audioSourceInstance);  
            }
            yield return new WaitForSeconds(1f);

            CanDisplay = true;
            yield return new WaitForSeconds(self.bubleDuration - 1f);

            if (!gameFinishedInstance && self.bubleText.text != ANGRY_TEXT) anim.CrossFadeInFixedTime("Detect", .2f);

            if (playOpenClose){ 
                bubbleAnim.Play("Close");
                Cursor.visible = false;
            }
            displaying = null;
        }   

        private void Update() {
            ElapsedTime += Time.deltaTime;
            if (ElapsedTime > 2f) {
                AngryCount = 0;
            }

            if (ElapsedTime > 10f) {
                Angry = false;
            }
        }

        private string GetStringId() {
            if (!Network.ArchipelagoManager.Connected())
                return "disconnected";

            if (firstSentence) {
                firstSentence = false;
                return "start";
            }

            if (Angry) return null;

            if (AngryCount > 30) {
                Angry = true;
                return "angry";
            }

            int id = Random.Range(0, 6);
            return id.ToString();
        }
    }
}
