using Assets.Scripts.Elements;
using Assets.Scripts.Utils.Extensions;
using System.Collections.Generic;
using UnityEngine;

public class Element : MonoBehaviour
{
    public static int IdIncrement = 0;
    public int Id;

    public Transform FrontSide;
    public Transform BackSide;
    public MouseEvents MouseEvents;
    public bool Spinnable;
    public bool Removable;
    public CollisionHelper CollisionHelper;
    public List<Transform> ContainedElements = new List<Transform>();
  
    public string Name { get; private set; }

    public static int MaxOrderInLayer = 0;

    private BoxCollider2D boxCollider;

    void Start()
    {
        Id = ++IdIncrement;

        var frontSpriteRenderer = FrontSide.GetComponent<SpriteRenderer>();
        Name = frontSpriteRenderer.sprite.name;

        boxCollider = GetComponent<BoxCollider2D>();
        boxCollider.size = new Vector2(frontSpriteRenderer.bounds.size.x, frontSpriteRenderer.bounds.size.y);   
    }

    void OnTriggerStay2D(Collider2D otherCollider)
    {
        if (boxCollider != null && boxCollider.name == otherCollider.name && CollisionHelper.IsFullyContained(boxCollider, otherCollider))
        {
            //Debug.Log($"{boxCollider.name} - {boxCollider.transform.position} collides with {otherCollider.name} - {otherCollider.transform.position}");
            var otherElement = otherCollider.transform;

            var otherElementContainedElements = otherElement.gameObject.GetComponent<Element>().ContainedElements;
            var isOtherElementDragging = otherElement.GetComponentInChildren<MouseEvents>().IsDragging;
            var isCurrentDragging = GetComponentInChildren<MouseEvents>().IsDragging;
            if (!otherElementContainedElements.Contains(transform) && !isOtherElementDragging && isCurrentDragging)
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

    public void Rotate()
    {
        Rotate(1);
    }

    public void Rotate(int numOfTimes)
    {
        if (Spinnable)
        {
            transform.Rotate(0, 0, -90 * numOfTimes);
            //todo: send to server
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

    public void SetLayer(string layer)
    {
        var frontSpriteRenderer = FrontSide.GetComponent<SpriteRenderer>();
        var backSpriteRenderer = BackSide.GetComponent<SpriteRenderer>();

        frontSpriteRenderer.sortingLayerName = layer;
        backSpriteRenderer.sortingLayerName = layer;

        var mouseEventsBoxCollider = GetComponentInChildren<BoxCollider>();
        mouseEventsBoxCollider.SetHeight(SortingLayers.LayerHeights[layer]);
    }

    public void IncrementLayerOrder()
    {
        var frontSpriteRenderer = FrontSide.GetComponent<SpriteRenderer>();
        var backSpriteRenderer = BackSide.GetComponent<SpriteRenderer>();

        frontSpriteRenderer.sortingOrder = ++MaxOrderInLayer;
        backSpriteRenderer.sortingOrder = MaxOrderInLayer;

        //todo: send that to server...
    }
}