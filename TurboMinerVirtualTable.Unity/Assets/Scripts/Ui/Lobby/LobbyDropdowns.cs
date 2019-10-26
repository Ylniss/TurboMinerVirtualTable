using System.Collections.Generic;
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

    public string GetWidthText()
    {
        return GetDropdownText(WidthChooserDropdown);
    }

    public void SetHeight(string heightData)
    {
        SetDropdown(HeightChooserDropdown, heightData);
    }

    public string GetHeightText()
    {
        return GetDropdownText(HeightChooserDropdown);
    }

    public void SetTilesConfig(string tilesData)
    {
        SetDropdown(TilesConfigDropdown, tilesData);
    }

    public string GetTilesConfigText()
    {
        return GetDropdownText(TilesConfigDropdown);
    }

    public void SetCorridorsConfig(string corridorsData)
    {
        SetDropdown(CorridorsConfigDropdown, corridorsData);
    }

    public string GetCorridorsConfigText()
    {
        return GetDropdownText(CorridorsConfigDropdown);
    }

    private void SetDropdown(TMP_Dropdown dropdown, string settingsData)
    {
        dropdown.options.Add(new TMP_Dropdown.OptionData(settingsData));
        dropdown.SetValueWithoutNotify(dropdown.options.Count - 1);       
    }

    public void InitConfigs()
    {
        var dropdowns = FindObjectsOfType<ConfigDropdown>();
        foreach (var dropdown in dropdowns)
        {
            dropdown.Init();
        }
    }

    public void SetInteractable(bool interactable)
    {
        TilesConfigDropdown.interactable = interactable;
        WidthChooserDropdown.interactable = interactable;
        HeightChooserDropdown.interactable = interactable;
        CorridorsConfigDropdown.interactable = interactable;
    }

    public void ResetWidthAndHeight()
    {
        ResetSizeDropdown(WidthChooserDropdown);
        ResetSizeDropdown(HeightChooserDropdown);
    }

    private string GetDropdownText(TMP_Dropdown dropdown)
    {
        return dropdown.GetComponentInChildren<TMP_Text>().text;
    }

    private void ResetSizeDropdown(TMP_Dropdown dropdown)
    {
        dropdown.options = new List<TMP_Dropdown.OptionData>()
        {
            new TMP_Dropdown.OptionData("5"),
            new TMP_Dropdown.OptionData("7"),
            new TMP_Dropdown.OptionData("9")
        };    
    }
}