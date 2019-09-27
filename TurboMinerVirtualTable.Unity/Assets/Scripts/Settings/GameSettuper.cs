using Assets.Scripts.Settings.Models;
using TMPro;
using UnityEngine;

public class GameSettuper : MonoBehaviour
{
    public TMP_Text TilesConfig;
    public TMP_Text CorridorsConfig;
    public TMP_Text MapWidth;
    public TMP_Text MapHeight;
    public PlayerLabel[] PlayerLabels;

    public void Setup()
    {
        GameSettings.TilesConfig = TilesConfig.text;
        GameSettings.CorridorsConfig = CorridorsConfig.text;
        GameSettings.MapSize = new MapSize(int.Parse(MapWidth.text), int.Parse(MapHeight.text));

        PlayerLabels = transform.root.GetComponentsInChildren<PlayerLabel>();
        GameSettings.PlayersSettings = new PlayerSettings[PlayerLabels.Length];
        for (var i = 0; i < PlayerLabels.Length; ++i)
        {
            GameSettings.PlayersSettings[i] = new PlayerSettings(PlayerLabels[i].Name, PlayerLabels[i].Color);
        }
    }
}
