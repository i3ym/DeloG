using System.Collections;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

namespace DeloG.Interactables
{
    public class BackDoor : Door
    {
        public override IEnumerator OpenAnimation()
        {
            var anim = Animator.MoveTo(transform, transform.localPosition + new Vector3(.2f, 0, 0), .7f, Easing.Linear);
            while (anim.MoveNext())
                yield return anim.Current;

            anim = Animator.MoveTo(transform, transform.localPosition + new Vector3(0f, 0, -1.7f), 1f, Easing.Linear);
            while (anim.MoveNext())
                yield return anim.Current;
        }
        public override IEnumerator CloseAnimation()
        {
            var anim = Animator.MoveTo(transform, transform.localPosition - new Vector3(0f, 0, -1.7f), 1f, Easing.Linear);
            while (anim.MoveNext())
                yield return anim.Current;

            anim = Animator.MoveTo(transform, transform.localPosition - new Vector3(.2f, 0, 0), .7f, Easing.Linear);
            while (anim.MoveNext())
                yield return anim.Current;

        }
    }
}