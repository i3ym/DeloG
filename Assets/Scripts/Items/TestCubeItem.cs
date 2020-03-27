using UnityEngine;

namespace DeloG.Items
{
    public class TestCubeItem : Item
    {
        public override void Use(Player player) => player.Rigidbody.AddForce(0, 5, 0, ForceMode.VelocityChange);
    }
}