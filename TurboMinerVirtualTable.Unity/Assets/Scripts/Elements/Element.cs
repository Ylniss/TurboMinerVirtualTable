using UnityEngine;

public class Element : MonoBehaviour
{
    public Transform FrontSide;
    public Transform BackSide;
    public BoxCollider2D BoxCollider;
    public RectTransform ButtonRect;

    private float lastClick = 0f;
    private float interval = 0.3f;

    void Start()
    {
        var frontSprite = FrontSide.GetComponent<SpriteRenderer>();
        var backSprite = BackSide.GetComponent<SpriteRenderer>();

        BoxCollider.size = frontSprite.bounds.size;
        ButtonRect.sizeDelta = new Vector2(frontSprite.bounds.size.x, frontSprite.bounds.size.y);
    }

    public void TurnOnOtherSide()
    {
        // on double click
        if ((lastClick + interval) > Time.time)
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
        else
        {
            // one click
        }

        lastClick = Time.time;
    }
}
