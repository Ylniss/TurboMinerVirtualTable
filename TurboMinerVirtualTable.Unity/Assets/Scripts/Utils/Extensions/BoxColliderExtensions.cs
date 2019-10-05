using UnityEngine;

namespace Assets.Scripts.Utils.Extensions
{
    public static class BoxColliderExtensions
    {
        public static void SetHeight(this BoxCollider boxCollider, float height)
        {
            boxCollider.size = new Vector3(boxCollider.size.x, boxCollider.size.y, height);
            boxCollider.center = new Vector3(0, 0, -height / 2);
        }
    }
}
