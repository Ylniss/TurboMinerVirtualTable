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
            var offset = new Vector3(i / rowCount * 180, -70 * (i % rowCount), 0);
            ConfigMenu.SetupCounter(tileCounter,origin, offset, tileSprites[i]);
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
