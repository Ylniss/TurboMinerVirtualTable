using Assets.Scripts.Settings;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    public static int NumberOfPlayers
    {
        get
        {
            return PlayersSettings.Length;
        }
    }

    public static PlayerSettings[] PlayersSettings;
    public static string TilesConfig;
    public static string CorridorsConfig;
    public static MapSize MapSize;
}
