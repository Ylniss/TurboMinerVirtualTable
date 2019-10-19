using Assets.Scripts.Elements;
using Assets.Scripts.Utils.Extensions;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Element : MonoBehaviour
{
    public int Id;

    public Transform FrontSide;
    public Transform BackSide;
    public bool Spinnable;
    public bool Removable;
    public CollisionHelper CollisionHelper;
    public List<Transform> ContainedElements = new List<Transform>();

    public string Name { get; private set; }

    public bool IsDragged
    {
        get
        {
            return GetComponentInChildren<MouseEvents>().IsDragged;
        }
        set
        {
            GetComponentInChildren<MouseEvents>().IsDragged = value;
        }
    }

    public static int MaxOrderInLayer = 1;

    private BoxCollider2D boxCollider;
    private static int idIncrement = 0;

    void Start()
    {
        Id = ++idIncrement;

        var frontSpriteRenderer = FrontSide.GetComponent<SpriteRenderer>();
        Name = frontSpriteRenderer.sprite.name;

        boxCollider = GetComponent<BoxCollider2D>();
        boxCollider.size = new Vector2(frontSpriteRenderer.bounds.size.x, frontSpriteRenderer.bounds.size.y);
    }

    void OnTriggerStay2D(Collider2D otherCollider)
    {
        if (boxCollider != null && boxCollider.name == otherCollider.name && CollisionHelper.IsFullyContained(boxCollider, otherCollider))
        {
            var otherElement = otherCollider.transform.gameObject.GetComponent<Element>();
            var otherElementContainedElements = otherElement.ContainedElements;
            if (!otherElementContainedElements.Contains(transform) && !otherElement.IsDragged && IsDragged)
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

        if (frontSpriteRenderer.sortingOrder == MaxOrderInLayer)
        {
            return;
        }

        frontSpriteRenderer.sortingOrder = ++MaxOrderInLayer;
        backSpriteRenderer.sortingOrder = MaxOrderInLayer;

        var boxCollider = GetComponentInChildren<BoxCollider>();
        var startingHeight = SortingLayers.LayerHeights[frontSpriteRenderer.sortingLayerName];
        boxCollider.SetHeight(startingHeight + 0.01f * MaxOrderInLayer);
    }

    public static Element Get(int id)
    {
        var elements = FindObjectsOfType<Element>();
        return elements.Single(e => e.Id == id);
    }
}