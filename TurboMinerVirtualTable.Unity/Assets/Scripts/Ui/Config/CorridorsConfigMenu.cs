using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorridorsConfigMenu : MonoBehaviour
{
    public ConfigMenu ConfigMenu;

    private Vector3 origin = new Vector3(-109, 232, 0);

    void Start()
    {
        var corridorCounter = Resources.Load<ElementCounter>("Prefabs/CorridorCounter");
        var corridorSprites = Resources.LoadAll<Sprite>("Graphics/Corridors/Common");

        for (var i = 0; i < corridorSprites.Length; ++i)
        {
            var offset = new Vector3(i * 180, 0, 0);
            ConfigMenu.SetupCounter(corridorCounter, origin, offset, corridorSprites[i]);
        }
    }

    public void SaveConfig()
    {
        ConfigMenu.SaveConfig();
    }

    public void AddConfig()
    {
        ConfigMenu.AddConfig();
    }

    public void RemoveConfig()
    {
        ConfigMenu.RemoveConfig();
    }
}
