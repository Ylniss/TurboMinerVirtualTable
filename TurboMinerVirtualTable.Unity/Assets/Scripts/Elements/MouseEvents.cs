using UnityEngine;

public class MouseEvents : MonoBehaviour
{
    public bool IsDragging;

    private Element element;
    private Vector2[] ContainedElementsOffsets;

    void Start()
    {
        element = GetComponent<Element>();
    }

    private Vector3 offset;
    void OnMouseDown()
    {
        offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z));
        ContainedElementsOffsets = new Vector2[element.ContainedElements.Count];
        element.SetLayerOrder(++Element.MaxOrderInLayer);

        for(var i = 0; i < ContainedElementsOffsets.Length; ++i)
        {
            ContainedElementsOffsets[i] = transform.position - element.ContainedElements[i].position;
            var containedElement = element.ContainedElements[i].gameObject.GetComponent<Element>();
            containedElement.SetLayerOrder(Element.MaxOrderInLayer);
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
        transform.position = curPosition;

        for(var i = 0; i < element.ContainedElements.Count; ++i)
        {
            element.ContainedElements[i].position = new Vector3(curPosition.x - ContainedElementsOffsets[i].x, curPosition.y - ContainedElementsOffsets[i].y, element.ContainedElements[i].position.z);
        }

        IsDragging = true;
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1)) // right mouse button
        {
            element.Rotate();
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
}