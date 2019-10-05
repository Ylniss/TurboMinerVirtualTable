using Assets.Scripts.Elements;
using Assets.Scripts.Table.Models;
using Assets.Scripts.Utils.Extensions;
using System.Collections.Generic;
using UnityEngine;

public class TableSettuper : MonoBehaviour
{
    public Spawner Spawner;
    public BoardPositioner BoardPositioner;
    public ConfigLoader ConfigLoader;
        
    void Start()
    {
        var diamond = Spawner.SpawnDiamond(new Vector2(0, 0));
        var diamondCorridor = Spawner.SpawnDiamondCorridor(new Vector2(0, 0));
        diamondCorridor.ContainedElements.Add(diamond.transform);

        var boardPosition = new Vector2(GameSettings.MapSize.Width / 2, GameSettings.MapSize.Height / 2);
        var panelPosition = new Vector2(37.5f, 19f);

        SetupStacks(StackType.Tile, GameSettings.TilesConfig, panelPosition, BoardPositioner.InitialPositions, new Vector2(1.1f, -6.0f));
        SetupStacks(StackType.Corridor, GameSettings.CorridorsConfig, panelPosition, BoardPositioner.InitialPositions, new Vector2(-6.0f, -4.6f));

        Spawner.SpawnStack(StackType.Passage, new Vector2(31.0f, 3.0f), "Graphics/Corridors", new List<string> { "road_explo2_tex_v2" });

        for (var i = 0; i < GameSettings.NumberOfPlayers; ++i)
        {
            SetupPlayer(i, boardPosition, panelPosition, BoardPositioner.InitialPositions[i]);
        }
    }

    private void SetupPlayer(int playerIndex, Vector2 boardPosition, Vector2 panelPosition, InitialPosition initialPosition)
    {
        var worldPosition = BoardPositioner.ToWorldPosition(boardPosition * initialPosition.BoardQuarter);
        var corridor = Spawner.SpawnLCorridor(worldPosition);
        corridor.Rotate(initialPosition.Rotation);

        var playerColor = GameSettings.PlayersSettings[playerIndex].Color; 
        var pawn = Spawner.SpawnPawn(playerColor, worldPosition);
        corridor.ContainedElements.Add(pawn.transform);

        var playerPanelPosition = panelPosition * initialPosition.BoardQuarter;
        var playerName = GameSettings.PlayersSettings[playerIndex].Name;
        Spawner.SpawnPlayerPanel(playerColor, playerName, panelPosition, initialPosition.BoardQuarter);

        Spawner.SpawnGetActionToken(playerPanelPosition + new Vector2(-6.5f, 3.5f));
        Spawner.SpawnUseActionToken(playerPanelPosition + new Vector2(-1.5f, 3.5f));
        Spawner.SpawnControlActionToken(playerPanelPosition + new Vector2(3.5f, 3.5f));
    }

    private void SetupStacks(StackType stackType, string configName, Vector2 panelPosition, InitialPosition[] initialPosition, Vector2 offset)
    {
        var subPath = stackType.ToString() + "s";
        var elementCounts = ConfigLoader.Load(subPath, configName);
        var elementsList = Stack.GetElements(elementCounts);
        int countInStack = elementsList.Count / GameSettings.NumberOfPlayers;
        elementsList.Shuffle();
        var stackElementLists = elementsList.ChunkBy(countInStack + 1);

        for (var i = 0; i < stackElementLists.Count; ++i)
        {
            Spawner.SpawnStack(stackType, panelPosition * initialPosition[i].BoardQuarter + offset, $"Graphics/{subPath}/Common", stackElementLists[i]);
        }
    }
}
