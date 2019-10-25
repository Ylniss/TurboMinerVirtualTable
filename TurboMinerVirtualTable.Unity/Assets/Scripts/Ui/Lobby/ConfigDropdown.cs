using TMPro;
using UnityEngine;

public class ConfigDropdown : MonoBehaviour
{
    public ConfigLoader ConfigLoader;
    public TMP_Dropdown Dropdown;
    public string SubPath;

    void Start()
    {
        Init();
    }

    public void Init()
    {
        var names = ConfigLoader.GetConfigNames(SubPath);

        Dropdown.options.Clear();
        foreach (var name in names)
        {
            Dropdown.options.Add(new TMP_Dropdown.OptionData(name));
        }

        Dropdown.value = 0;
    }
}
