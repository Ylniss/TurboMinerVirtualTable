using UnityEngine;

public class CollisionHelper : MonoBehaviour
{
    public bool IsFullyContained(Collider collider, Collider otherCollider)
    {
        if(collider == null || otherCollider == null)
        {
            return false;
        }

        return otherCollider.bounds.Contains(collider.bounds.max) && otherCollider.bounds.Contains(collider.bounds.min);
    }
}
