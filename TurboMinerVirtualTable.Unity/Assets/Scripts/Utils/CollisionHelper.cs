using UnityEngine;

public class CollisionHelper : MonoBehaviour
{
    public bool IsFullyContained(Collider2D collider, Collider2D otherCollider)
    {
        return otherCollider.bounds.Contains(collider.bounds.max) && otherCollider.bounds.Contains(collider.bounds.min);
    }
}
