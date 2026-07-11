using System;
using System.Collections;
using UnityEngine;

namespace Core.Tools {
    public abstract class Tracker<T> : MonoBehaviour {
        abstract protected bool IsResolved(T arg);
        public Action Resolve;

        public void Track(T arg) => StartCoroutine(TrackerRoutine(arg));

        private IEnumerator TrackerRoutine(T arg) {
            yield return null;

            while (!IsResolved(arg)) yield return null;
            Resolve?.Invoke();
        }
    }
}
