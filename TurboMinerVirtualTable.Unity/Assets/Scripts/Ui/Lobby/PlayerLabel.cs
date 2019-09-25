using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLabel : MonoBehaviour
{
    public string Color
    {
        get
        {
            return ColorPicker.color.ToString();
        }
    }

    public string Name
    {
        get
        {
            return NameLabel.text;
        }
    }

    public Image ColorPicker;
    public TMP_Text NameLabel;
}
