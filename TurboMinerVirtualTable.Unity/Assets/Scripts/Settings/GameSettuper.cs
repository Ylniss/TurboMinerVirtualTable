using Assets.Scripts.Settings.Models;
using Assets.Scripts.Utils.Extensions;
using System.Collections.Generic;
using UnityEngine;

public class GameSettuper : MonoBehaviour
{
    public LobbyDropdowns LobbyDropdowns;
    public ConfigLoader ConfigLoader;

    private PlayerLabel[] PlayerLabels;

    public void Setup()
    {
        GameSettings.Tiles = GetShuffledStackElements("Tiles", LobbyDropdowns.GetTilesConfigText());
        GameSettings.Corridors = GetShuffledStackElements("Corridors", LobbyDropdowns.GetCorridorsConfigText());
        GameSettings.MapSize = GetMapSize();

        SetupPlayerSettings();
    }

    private MapSize GetMapSize()
    {
        return new MapSize(int.Parse(LobbyDropdowns.GetWidthText()), int.Parse(LobbyDropdowns.GetHeightText()));
    }

    private void SetupPlayerSettings()
    {
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
