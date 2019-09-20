using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Assets.Scripts.Utils;

public class ConfigMenu : MonoBehaviour
{
    public ScrollRect configList;
    public TMP_InputField configName;

    public ElementCounterToElementCountConverter converter;

    private List<ElementCounter> elementCounters = new List<ElementCounter>();

    public void SetupCounter(ElementCounter counter, Vector3 position, Vector3 offset, Sprite sprite)
    {
        var elementCounterInstance = Instantiate(counter);
        elementCounterInstance.transform.parent = transform;

        elementCounterInstance.Image.sprite = sprite;
        elementCounterInstance.transform.localPosition = position + offset;
        elementCounters.Add(elementCounterInstance);
    }

    public void SaveConfig()
    {
        var elementCounts = new ElementCount[elementCounters.Count];
        for(var i = 0; i < elementCounters.Count; ++i)
        {
            var elementCount = converter.Convert(elementCounters[i]);
            elementCounts[i] = elementCount;
        }

        var formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/" + configName.text + ".cfg";
        var stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, elementCounts);
        stream.Close();
    }

    public void AddConfig()
    {

    }

    public void RemoveConfig()
    {

    }



}
