using UnityEngine;
using UnityEngine.UI;

public class ColorPicker : MonoBehaviour
{
    public Image image;

    private Color[] colors;
    private int currentColorIndex = 0;

    void Awake()
    {
        colors = new Color[]
        {
            Color.white,
            Color.black,
            Color.red,
            Color.green,
            Color.blue,
            Color.yellow,
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
