using System;
using System.Collections;
using UnityEngine;

namespace Core.Tools {
    public abstract class VoidTracker : MonoBehaviour {
        abstract protected bool IsResolved();
        public Action Resolve;

        public void Track() => StartCoroutine(TrackerRoutine());

        private IEnumerator TrackerRoutine() {
            System.Console.WriteLine("Start routine");
            yield return null;

            while (!IsResolved()) yield return null;
            System.Console.WriteLine("End routine");
            Resolve?.Invoke();
        }
    }
}

