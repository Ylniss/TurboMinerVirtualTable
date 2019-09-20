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
            SetupTileCounter(corridorCounter, i, origin, corridorSprites[i]);
        }
    }

    private void SetupTileCounter(ElementCounter tileCounter, int index, Vector3 position, Sprite sprite)
    {
        var elementCounterInstance = Instantiate(tileCounter);
        elementCounterInstance.transform.parent = transform;

        elementCounterInstance.Image.sprite = sprite;
        var offset = new Vector3(index * 180, 0, 0);
        elementCounterInstance.transform.localPosition = position + offset;
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
