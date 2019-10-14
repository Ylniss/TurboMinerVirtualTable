using Assets.Scripts.Settings.Models;
using Assets.Scripts.Utils.Extensions;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameSettuper : MonoBehaviour
{
    public TMP_Text TilesConfig;
    public TMP_Text CorridorsConfig;
    public TMP_Text MapWidth;
    public TMP_Text MapHeight;
    public PlayerLabel[] PlayerLabels;

    public ConfigLoader ConfigLoader;

    public void Setup()
    {
        GameSettings.Tiles = GetShuffledStackElements("Tiles", TilesConfig.text);
        GameSettings.Corridors = GetShuffledStackElements("Corridors", CorridorsConfig.text);
        GameSettings.MapSize = new MapSize(int.Parse(MapWidth.text), int.Parse(MapHeight.text));

        PlayerLabels = transform.root.GetComponentsInChildren<PlayerLabel>();
        GameSettings.PlayersSettings = new PlayerSettings[PlayerLabels.Length];
        for (var i = 0; i < PlayerLabels.Length; ++i)
        {
            GameSettings.PlayersSettings[i] = new PlayerSettings(PlayerLabels[i].Name, PlayerLabels[i].Color);
        }
    }

    private List<string> GetShuffledStackElements(string subPath, string config)
    {
        var counts = ConfigLoader.Load(subPath, config);
        var elements = Stack.GetElements(counts);
        elements.Shuffle();
        return elements;
    }
}
