using Assets.Scripts.Settings.Models;

public static class GameSettings
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
