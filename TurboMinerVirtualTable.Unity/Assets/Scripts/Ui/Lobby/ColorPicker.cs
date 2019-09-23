using UnityEngine;
using UnityEngine.UI;

public class ColorPicker : MonoBehaviour
{
    public Image image;

    private Color[] colors;
    private int currentColorIndex = 0;

    void Start()
    {
        colors = new Color[]
        {
            new Color(255,255,255),
            new Color(0,0,0),
            new Color(255,0,0),
            new Color(0,255,0),
            new Color(0,0,255),
            new Color(254,224,16),
        };
    }

    public void ChangeColor()
    {
        ++currentColorIndex;
        if(currentColorIndex == colors.Length)
        {
            currentColorIndex = 0;
        }

        image.color = colors[currentColorIndex];
    }

}
