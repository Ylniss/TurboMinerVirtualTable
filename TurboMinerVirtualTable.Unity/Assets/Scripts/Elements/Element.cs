using System.Collections.Generic;
using UnityEngine;

public class Element : MonoBehaviour
{
    public Transform FrontSide;
    public Transform BackSide;
    public bool Spinnable;
    public CollisionHelper CollisionHelper;
    public List<Transform> ContainedElements = new List<Transform>();

    private BoxCollider2D boxCollider;

    void Start()
    {
        var frontSprite = FrontSide.GetComponent<SpriteRenderer>();
        var backSprite = BackSide.GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        

        if (frontSprite.bounds.size != backSprite.bounds.size)
        {
            Debug.Log($"Front sprite size '{frontSprite.bounds.size}' is different than back '{backSprite.bounds.size}'");
        }

        boxCollider.size = frontSprite.bounds.size;
    }

    void OnTriggerStay2D(Collider2D otherCollider)
    {
        if (CollisionHelper.IsFullyContained(boxCollider, otherCollider))
        {
            var otherElement = otherCollider.transform;

            var otherElementContainedElements = otherElement.gameObject.GetComponent<Element>().ContainedElements;
            var isOtherElementDragging = otherElement.GetComponent<MouseEvents>().IsDragging;
            if (!otherElementContainedElements.Contains(transform) && !isOtherElementDragging)
            {
                otherElementContainedElements.Add(transform);
            }
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        var element = collider.transform;
        if (ContainedElements.Contains(element))
        {
            ContainedElements.Remove(element);
        }
    }
}
