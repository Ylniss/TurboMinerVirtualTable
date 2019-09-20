using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilesConfigMenu : MonoBehaviour
{
    public ConfigMenu ConfigMenu;

    private Vector3 origin = new Vector3(-126, 241, 0);
    private readonly int rowCount = 5;
    void Start()
    {
        var tileCounter = Resources.Load<ElementCounter>("Prefabs/TileCounter");
        var tileSprites = Resources.LoadAll<Sprite>("Graphics/Tiles/Common");

        for (var i = 0; i < tileSprites.Length; ++i)
        {
            SetupTileCounter(tileCounter, i, origin, tileSprites[i]);
        }
    }

    private void SetupTileCounter(ElementCounter tileCounter, int index, Vector3 position, Sprite sprite)
    {
        var elementCounterInstance = Instantiate(tileCounter);
        elementCounterInstance.transform.parent = transform;

        elementCounterInstance.Image.sprite = sprite;
        var offset = new Vector3(index / rowCount * 180, -(elementCounterInstance.Image.rectTransform.rect.height + 20) * (index % rowCount), 0);
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
