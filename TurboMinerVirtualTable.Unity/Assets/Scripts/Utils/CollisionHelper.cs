using UnityEngine;

public class CollisionHelper : MonoBehaviour
{
    //todo: change to IsMostlyContained and also make colider of little objects higher - it should resolve the problem
    public bool IsFullyContained(Collider collider, Collider otherCollider) 
    {
        if(collider == null || otherCollider == null)
        {
            return false;
        }

        return otherCollider.bounds.Contains(collider.bounds.max) && otherCollider.bounds.Contains(collider.bounds.min);
    }
}
