using DeloG.Items;
using UnityEngine;

namespace DeloG
{
    public class PlayerItemHolder : ItemHolder
    {
        protected override Quaternion GetRotation(Item item) =>
            item is PhotoItem
            ? Quaternion.Euler(-80, 0, 0)
            : Quaternion.identity;
    }
}