using UnityEngine;

public class CorridorsConfigMenu : MonoBehaviour
{
    public ConfigMenu ConfigMenu;

    private Vector3 origin = new Vector3(-109, 232, 0);
    private readonly string SubPath = "Corridors";

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
