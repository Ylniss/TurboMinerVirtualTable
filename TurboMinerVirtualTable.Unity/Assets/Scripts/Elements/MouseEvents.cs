using UnityEngine;

public class MouseEvents : MonoBehaviour
{
    public bool IsDragging;
   
    private Element element;

    void Start()
    {
        element = GetComponentInParent<Element>();
        SetupMouseBoxCollider();
    }

    private Vector3 offset;
    private Vector2[] ContainedElementsOffsets;
    void OnMouseDown()
    {
        offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z));
        ContainedElementsOffsets = new Vector2[element.ContainedElements.Count];
        element.IncrementLayerOrder();
        MultiplayerManager.Instance.SendIncrementElementLayer(element.Id);

        for (var i = 0; i < ContainedElementsOffsets.Length; ++i)
        {
            ContainedElementsOffsets[i] = transform.position - element.ContainedElements[i].position;
            var containedElement = element.ContainedElements[i].gameObject.GetComponent<Element>();
            containedElement.IncrementLayerOrder();
            MultiplayerManager.Instance.SendIncrementElementLayer(containedElement.Id);
        }

        OnDoubleClick();
    }

    void OnMouseUp()
    {
        IsDragging = false;
    }

    void OnMouseDrag()
    {
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z);

        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
        element.transform.position = curPosition;
        MultiplayerManager.Instance.SendElementPosition(element.Id, curPosition);

        for(var i = 0; i < element.ContainedElements.Count; ++i)
        {
            var containedElementPosition = new Vector2(curPosition.x - ContainedElementsOffsets[i].x, curPosition.y - ContainedElementsOffsets[i].y);
            element.ContainedElements[i].position = containedElementPosition;
            var containedElement = element.ContainedElements[i].gameObject.GetComponent<Element>();
            MultiplayerManager.Instance.SendElementPosition(containedElement.Id, containedElementPosition); //todo: change to send elements position and send at once
        }

        IsDragging = true;
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1)) // right mouse button
        {
            element.Rotate();
        }

        if (Input.GetKeyDown(KeyCode.Delete) && element.Removable)
        {
            Destroy(element.gameObject);
        }
    }

    private float lastClick = 0f;
    private float interval = 0.3f;

    private void OnDoubleClick()
    {
        if ((lastClick + interval) > Time.time)
        {
            element.TurnOnOtherSide();
        }

        lastClick = Time.time;
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