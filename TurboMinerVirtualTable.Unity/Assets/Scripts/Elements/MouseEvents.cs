using System.Collections.Generic;
using UnityEngine;

public class MouseEvents : MonoBehaviour
{
    public bool IsDragging;

    private Transform frontSide;
    private Transform backSide;
    private bool spinnable;
    private List<Transform> containedElements;

    void Start()
    {
        var element = GetComponent<Element>();
        
        frontSide = element.FrontSide;
        backSide = element.BackSide;
        spinnable = element.Spinnable;
        containedElements = element.ContainedElements;
    }

    private Vector3 screenPoint;
    private Vector3 offset;

    void OnMouseDown()
    {
        offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z));

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

        foreach(var element in containedElements)
        {
            element.position = new Vector3(curPosition.x, curPosition.y, element.position.z);
        }

        IsDragging = true;
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1) && spinnable) // right mouse button
        {
            frontSide.Rotate(0, 0, 90);
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
        if (frontSide.gameObject.activeInHierarchy)
        {
            backSide.gameObject.SetActive(true);
            frontSide.gameObject.SetActive(false);
        }
        else
        {
            backSide.gameObject.SetActive(false);
            frontSide.gameObject.SetActive(true);
        }
    }
}