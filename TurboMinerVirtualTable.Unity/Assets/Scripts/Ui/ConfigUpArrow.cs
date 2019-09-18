using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfigUpArrow : MonoBehaviour
{
    public void UpCounter()
    {
        var elementCounter = GameObject.Find("ElementCounter");

        var currentText = elementCounter.GetComponent<InputField>().text;

        var abc = currentText;
    }

}
