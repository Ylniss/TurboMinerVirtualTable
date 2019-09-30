using Assets.Scripts.Table.Models;
using Assets.Scripts.Utils.Extensions;
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

        GetTilesStacks(panelPosition, BoardPositioner.InitialPositions);

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

    private Stack[] GetTilesStacks(Vector2 panelPosition, InitialPosition[] initialPosition)
    {
        var elementCounts = ConfigLoader.Load("Tiles", GameSettings.TilesConfig);
        var elementsList = Stack.GetElements(elementCounts);
        int countInStack = elementsList.Count / GameSettings.NumberOfPlayers;
        elementsList.Shuffle();
        var stackElementLists = elementsList.ChunkBy(countInStack + 1);

        var stacks = new Stack[GameSettings.NumberOfPlayers];

        for (var i = 0; i < stackElementLists.Count; ++i)
        {
            stacks[i] = Spawner.SpawnStack(panelPosition * initialPosition[i].BoardQuarter + new Vector2(-1.5f, -4.5f), stackElementLists[i]);
        }

        return stacks;
    }
}
