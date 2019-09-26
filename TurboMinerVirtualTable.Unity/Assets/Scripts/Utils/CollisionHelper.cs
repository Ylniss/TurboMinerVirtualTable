using UnityEngine;

public class CollisionHelper : MonoBehaviour
{
    public bool IsFullyContained(Collider collider, Collider otherCollider)
    {
        return otherCollider.bounds.Contains(collider.bounds.max) && otherCollider.bounds.Contains(collider.bounds.min);
    }
}
