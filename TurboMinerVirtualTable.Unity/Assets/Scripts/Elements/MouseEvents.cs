using UnityEngine;

public class MouseEvents : MonoBehaviour
{
    public bool IsDragging;

    private Element element;

    void Start()
    {
        element = GetComponent<Element>();
    }

    private Vector3 offset;
    void OnMouseDown()
    {
        offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z));

        // set Z to 0 when taking element to allow collisions with other ellements (Z axis has to be equal)
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);

        OnDoubleClick();
    }

    void OnMouseUp()
    {
        if (element.IsContained)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -1);
        }

        IsDragging = false;
    }

    void OnMouseDrag()
    {
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z);

        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
        transform.position = curPosition;

        foreach(var element in element.ContainedElements)
        {
            element.position = new Vector3(curPosition.x, curPosition.y, element.position.z);
        }

        IsDragging = true;
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1) && element.Spinnable) // right mouse button
        {
            element.FrontSide.Rotate(0, 0, 90);
        }
    }

    private float lastClick = 0f;
    private float interval = 0.3f;

    private void OnDoubleClick()
    {
        if ((lastClick + interval) > Time.time)
        {
            TurnOnOtherSide();
        }

        lastClick = Time.time;
    }

    private void TurnOnOtherSide()
    {
        if (element.FrontSide.gameObject.activeInHierarchy)
        {
            element.BackSide.gameObject.SetActive(true);
            element.FrontSide.gameObject.SetActive(false);
        }
        else
        {
            element.BackSide.gameObject.SetActive(false);
            element.FrontSide.gameObject.SetActive(true);
        }
    }
}