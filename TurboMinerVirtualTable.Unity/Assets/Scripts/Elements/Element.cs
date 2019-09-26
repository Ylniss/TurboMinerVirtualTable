using System.Collections.Generic;
using UnityEngine;

public class Element : MonoBehaviour
{
    public Transform FrontSide;
    public Transform BackSide;
    public bool Spinnable;
    public CollisionHelper CollisionHelper;
    public List<Transform> ContainedElements = new List<Transform>();

    private BoxCollider boxCollider;

    // max size that will put above others on Z axis (to make little elements like tiles takeable from bigger elements like corridors
    private const int maximumAboveAllSize = 8;

    void Start()
    {
        var frontSprite = FrontSide.GetComponent<SpriteRenderer>();

        boxCollider = GetComponent<BoxCollider>();
        boxCollider.size = new Vector3(frontSprite.bounds.size.x, frontSprite.bounds.size.y, 1);

        // turn box collider towards smaller elements (up)
        boxCollider.center = new Vector3(0, 0, -0.5f);

        // if area is small enough put it above (z = -1) and turn boxcollider towards bigger element (down)
        if (boxCollider.size.x* boxCollider.size.y < maximumAboveAllSize)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -1);
            boxCollider.center = new Vector3(0, 0, 0.5f);
        }
        
    }

    void OnTriggerStay(Collider otherCollider)
    {
        if (CollisionHelper.IsFullyContained(boxCollider, otherCollider))
        {
            var otherElement = otherCollider.transform;

            var otherElementContainedElements = otherElement.gameObject.GetComponent<Element>().ContainedElements;
            var isOtherElementDragging = otherElement.GetComponent<MouseEvents>().IsDragging;
            var isCurrentDragging = GetComponent<MouseEvents>().IsDragging;
            if (!otherElementContainedElements.Contains(transform) && !isOtherElementDragging && isCurrentDragging)
            {
                otherElementContainedElements.Add(transform);
            }
        }
    }

    void OnTriggerExit(Collider collider)
    {
        var element = collider.transform;
        if (ContainedElements.Contains(element))
        {
            ContainedElements.Remove(element);
        }
    }

    public void Rotate()
    {
        if (Spinnable)
        {
            transform.Rotate(0, 0, 90);
        }
    }

    public void TurnOnOtherSide()
    {
        if (FrontSide.gameObject.activeInHierarchy)
        {
            BackSide.gameObject.SetActive(true);
            FrontSide.gameObject.SetActive(false);
        }
        else
        {
            BackSide.gameObject.SetActive(false);
            FrontSide.gameObject.SetActive(true);
        }
    }
}