using TMPro;
using UnityEngine;

public class LobbyDropdowns : MonoBehaviour
{
    public TMP_Dropdown TilesConfigDropdown;
    public TMP_Dropdown CorridorsConfigDropdown;
    public TMP_Dropdown WidthChooserDropdown;
    public TMP_Dropdown HeightChooserDropdown;

    public void SetWidth(string widthData)
    {
        SetDropdown(WidthChooserDropdown, widthData);
    }

    public void SetHeight(string heightData)
    {
        SetDropdown(HeightChooserDropdown, heightData);
    }

    public void SetTilesConfig(string tilesData)
    {
        SetDropdown(TilesConfigDropdown, tilesData);
    }

    public void SetCorridorsConfig(string corridorsData)
    {
        SetDropdown(CorridorsConfigDropdown, corridorsData);
    }

    private void SetDropdown(TMP_Dropdown dropdown, string settingsData)
    {
        dropdown.options.Add(new TMP_Dropdown.OptionData(settingsData));
        dropdown.SetValueWithoutNotify(dropdown.options.Count - 1);       
    }
}
