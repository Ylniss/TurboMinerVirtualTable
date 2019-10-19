using Assets.Scripts.Networking.Models;
using UnityEngine;

public class MouseEvents : MonoBehaviour
{
    public bool IsDragged;
   
    private Element element;
    private Vector2 lastCursorPosition;

    void Start()
    {
        element = GetComponentInParent<Element>();
        SetupMouseBoxCollider();
    }

    private Vector2 offset;
    private Vector2[] ContainedElementsOffsets;
    void OnMouseDown()
    {
        offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
        ContainedElementsOffsets = new Vector2[element.ContainedElements.Count];
        element.IncrementLayerOrder();

        // element positions to send via network
        var elementIds = new ElementIdArray(element.ContainedElements.Count + 1);
        elementIds.Array[0] = new ElementId(element.Id);

        for (var i = 0; i < ContainedElementsOffsets.Length; ++i)
        {
            ContainedElementsOffsets[i] = transform.position - element.ContainedElements[i].position;
            var containedElement = element.ContainedElements[i].gameObject.GetComponent<Element>();
            containedElement.IncrementLayerOrder();
            elementIds.Array[i + 1] = new ElementId(containedElement.Id);
        }
     
        if (!OnDoubleClick())
        {
            MultiplayerManager.Instance.SendIncrementElementsLayers(elementIds);
        }
    }

    void OnMouseUp()
    {
        IsDragged = false;
        MultiplayerManager.Instance.SendStopElementDrag(element.Id);
    }

    void OnMouseDrag()
    {
        var cursorScreenPoint = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        var cursorPosition = (Vector2)Camera.main.ScreenToWorldPoint(cursorScreenPoint) + offset;

        if(lastCursorPosition == cursorPosition)
        {
            return;
        }

        element.transform.position = cursorPosition;

        // element positions to send via network
        var elementPositions = new ElementPositionArray(element.ContainedElements.Count + 1);
        elementPositions.Array[0] = new ElementPosition(element.Id, cursorPosition);

        for (var i = 0; i < element.ContainedElements.Count; ++i)
        {
            var containedElementPosition = new Vector2(cursorPosition.x - ContainedElementsOffsets[i].x, cursorPosition.y - ContainedElementsOffsets[i].y);
            element.ContainedElements[i].position = containedElementPosition;
            var containedElement = element.ContainedElements[i].gameObject.GetComponent<Element>();
            elementPositions.Array[i + 1] = new ElementPosition(containedElement.Id, containedElementPosition);
        }

        MultiplayerManager.Instance.SendElementsPositions(elementPositions);
        IsDragged = true;

        lastCursorPosition = cursorPosition;
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1)) // right mouse button
        {
            MultiplayerManager.Instance.SendRotateElement(element.Id);
        }

        if (Input.GetKeyDown(KeyCode.Delete) && element.Removable)
        {
            MultiplayerManager.Instance.SendDestroyElement(element.Id);
        }
    }

    private float lastClick = 0f;
    private float interval = 0.3f;

    private bool OnDoubleClick()
    {
        if ((lastClick + interval) > Time.time)
        {
            element.TurnOnOtherSide();
            MultiplayerManager.Instance.SendTurnElementOnOtherSide(element.Id);
            return true;
        }

        lastClick = Time.time;
        return false;
    }

    private void SetupMouseBoxCollider()
    {
        var boxCollider = GetComponent<BoxCollider>();
        var frontSpriteRenderer = element.FrontSide.GetComponent<SpriteRenderer>();
        var xSize = frontSpriteRenderer.bounds.size.x;
        var ySize = frontSpriteRenderer.bounds.size.y;
        boxCollider.size = new Vector3(xSize, ySize, boxCollider.size.z);
    }
}