using Assets.Scripts.Settings.Models;
using System.Collections.Generic;

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
    public static List<string> Tiles;
    public static List<string> Corridors;
    public static MapSize MapSize;
}
