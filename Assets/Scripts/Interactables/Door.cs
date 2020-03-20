using System.Collections;
using UnityEngine;

namespace DeloG.Interactables
{
    public abstract class Door : MonoBehaviour
    {
        public abstract IEnumerator OpenAnimation();
        public abstract IEnumerator CloseAnimation();
    }
}