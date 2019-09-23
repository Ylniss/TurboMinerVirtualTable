using TMPro;
using UnityEngine;

public class ConfigDopdown : MonoBehaviour
{
    public ConfigLoader ConfigLoader;
    public TMP_Dropdown Dropdown;
    public string SubPath;

    void Start()
    {
        var names = ConfigLoader.GetConfigNames(SubPath);
        foreach (var name in names)
        {
            Dropdown.options.Add(new TMP_Dropdown.OptionData(name));
        }
    }
}
