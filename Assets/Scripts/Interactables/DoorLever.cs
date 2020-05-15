using System.Collections;
using UnityEngine;

namespace DeloG.Interactables
{
    public class DoorLever : Lever
    {
        [SerializeField] Door Door = null;

        protected override void Awake()
        {
            if (Door is null || !Door)
            {
                var obj = transform;

                while (true)
                {
                    obj = obj.parent;
                    if (obj is null || !obj)
                    {
                        Debug.LogWarning("Дверная ручка " + name + " не смогла найти для себя дверь", this);
                        break;
                    }

                    var door = obj.GetComponent<Door>();
                    if (door)
                    {
                        Door = door;
                        break;
                    }
                }
            }

            base.Awake();
        }

        protected override void OnToggle(bool toggled) { }

        protected override IEnumerator ToggleAnimation() => Door.OpenAnimation();
        protected override IEnumerator UntoggleAnimation() => Door.CloseAnimation();
    }
}