using System;
using System.Collections;
using UnityEngine;

namespace SplasherArchipelago.Helpers {
    internal class AnimationTracker : MonoBehaviour {
        internal Action ResolveAction;

        internal void StartTrack(Animator anim) => StartCoroutine(Track(anim));

        private IEnumerator Track(Animator anim)  {
            yield return null;
            while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1) yield return null;

            if (ResolveAction != null)
                ResolveAction();
        }
    }
}