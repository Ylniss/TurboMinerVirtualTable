using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConfigMenu : MonoBehaviour
{
    public ScrollRect ConfigList;
    public TMP_InputField ConfigName;
    public ConfigSaver ConfigSaver;
    public ConfigLoader ConfigLoader;

    private List<ElementCounter> elementCounters = new List<ElementCounter>();

    public void SetupCounter(ElementCounter counter, Vector3 position, Vector3 offset, Sprite sprite)
    {
        var elementCounterInstance = Instantiate(counter);
        elementCounterInstance.transform.parent = transform;

        elementCounterInstance.Image.sprite = sprite;
        elementCounterInstance.transform.localPosition = position + offset;
        elementCounters.Add(elementCounterInstance);
    }

    public void SaveConfig(string subPath)
    {
        if (ConfigName == null)
        {
            return;
        }

        ConfigSaver.Save(elementCounters, subPath, ConfigName.text);
    }

    public void LoadConfig(string subPath, string name)
    {
        var elementCounts = ConfigLoader.Load(subPath, name);
        var elementCounters = gameObject.transform.parent.GetComponentsInChildren<ElementCounter>();

        for(var i = 0; i < elementCounts.Length; ++i)
        {
            elementCounters[i].Count = elementCounts[i].Count;
        }
    }

    public void RemoveConfig()
    {

    }
}