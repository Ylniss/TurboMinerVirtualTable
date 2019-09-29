using TMPro;
using UnityEngine;

public class PlayerPanel : MonoBehaviour
{
    public SpriteRenderer SpriteRenderer;
    public TMP_Text Text;

    private const float textOffset = 10.8f;
    private const float textOpacity = 0.9f;

    void Start()
    {
        if (transform.position.y < 0)
        {
            Text.transform.position = new Vector2(transform.position.x, -textOffset);
        }  
        else
        {
            Text.transform.position = new Vector2(transform.position.x, textOffset);
        }

        if (transform.position.x < 0)
        {
            Text.alignment = TextAlignmentOptions.Right;
        }
        else
        {
            Text.alignment = TextAlignmentOptions.Left;
        }     
    }

    public Color Color
    {
        get { return SpriteRenderer.color; }
        set
        {
            SpriteRenderer.color = value;
            Text.color = new Color(value.r, value.g, value.b, textOpacity);
        }
    }

    public string Name
    {
        get { return Text.text; }
        set { Text.text = value; }
    }
}
