using UnityEngine;

public class TilesConfigMenu : MonoBehaviour
{
    public ConfigMenu ConfigMenu;

    private Vector3 origin = new Vector3(-126, 216, 0);
    private readonly int rowCount = 5;
    private readonly string SubPath = "Tiles";

    void Start()
    {
        var tileCounter = Resources.Load<ElementCounter>("Prefabs/TileCounter");
        var tileSprites = Resources.LoadAll<Sprite>("Graphics/Tiles/Common");

        for (var i = 0; i < tileSprites.Length; ++i)
        {
            var offset = new Vector3(i / rowCount * 180, -70 * (i % rowCount), 0);
            ConfigMenu.SetupCounter(tileCounter, origin, offset, tileSprites[i]);
        }
    }

    public void SaveConfig()
    {
        ConfigMenu.SaveConfig(SubPath);
    }

    public void LoadConfig(string name)
    {
        ConfigMenu.LoadConfig(SubPath, name);
    }

    public void RemoveConfig()
    {
        ConfigMenu.RemoveConfig();
    }
}
