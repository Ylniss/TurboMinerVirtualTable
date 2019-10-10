using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ColorPicker : MonoBehaviour
{
    public Image image;

    private List<Color> colors;
    private int currentColorIndex = 0;

    void Awake()
    {
        colors = new List<Color>
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
        if(currentColorIndex == colors.Count)
        {
            currentColorIndex = 0;
        }

        image.color = colors[currentColorIndex];
    }

    public void ChooseFromAvailable(List<Color> excludedColors)
    {
        image.color = colors.Except(excludedColors).ToList().FirstOrDefault(); 
    }
}
