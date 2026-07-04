using System.Collections;
using UnityEngine;

namespace SplasherArchipelago.Patches.Controller.Hub.UnlockLevelAnimation {
    internal class UnlockRoutine : MonoBehaviour {
        public void StartRoutine() => StartCoroutine(Routine());
        
        private IEnumerator Routine() {
            yield return null;

            while (global::Hub.IsLoaded) {
                if (GameManager.LockControl != LockControlType.None) yield return null;
                DoorReference.TryUnlock();
                yield return null;
            }
        }
    }
}
