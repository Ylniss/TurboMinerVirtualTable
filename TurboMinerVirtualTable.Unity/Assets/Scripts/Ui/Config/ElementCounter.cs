using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElementCounter : MonoBehaviour
{
    public InputField CountInput;
    public Image Image;

    public int MaxCount;

    private int count = 0;

    public void Start()
    {
        SetCountInputText();
    }

    public void UpCounter()
    {
        ++count;

        if (count > MaxCount)
        {
            count = MaxCount;
        }

        SetCountInputText();
    }

    public void DownCounter()
    {
        --count;

        if(count < 0)
        {
            count = 0;
        }

        SetCountInputText();
    }

    private void SetCountInputText()
    {
        CountInput.text = count.ToString();
    }
}
