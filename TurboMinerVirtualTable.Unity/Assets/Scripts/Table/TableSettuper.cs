using Assets.Scripts.Elements;
using Assets.Scripts.Table.Models;
using Assets.Scripts.Utils.Extensions;
using System.Collections.Generic;
using UnityEngine;

public class TableSettuper : MonoBehaviour
{
    public Spawner Spawner;
    public BoardPositioner BoardPositioner;
        
    void Start()
    {
        var diamond = Spawner.SpawnDiamond(new Vector2(0, 0));
        var diamondCorridor = Spawner.SpawnDiamondCorridor(new Vector2(0, 0));
        diamondCorridor.ContainedElements.Add(diamond.transform);

        var boardPosition = new Vector2(GameSettings.MapSize.Width / 2, GameSettings.MapSize.Height / 2);
        var panelPosition = new Vector2(37.5f, 19f);

        SetupStacks(StackType.Tile, GameSettings.Tiles, panelPosition, BoardPositioner.InitialPositions, new Vector2(1.1f, -6.0f));
        SetupStacks(StackType.Corridor, GameSettings.Corridors, panelPosition, BoardPositioner.InitialPositions, new Vector2(-6.0f, -4.6f));

        Spawner.SpawnStack(StackType.Passage, new Vector2(31.0f, 3.0f), "Graphics/Corridors", new List<string> { "road_explo2_tex_v2" });

        Spawner.SpawnStack(StackType.Tile, new Vector2(45.0f, -4.0f), "Graphics/Tiles/Common", new List<string> { "et_gold10_grey_edit" });
        Spawner.SpawnStack(StackType.Tile, new Vector2(45.0f, 0.0f), "Graphics/Tiles/Common", new List<string> { "et_gold20_grey_edit" });
        Spawner.SpawnStack(StackType.Tile, new Vector2(45.0f, 4.0f), "Graphics/Tiles/Common", new List<string> { "et_gold30_grey_edit" });

        Spawner.SpawnStack(StackType.Tile, new Vector2(-46.0f, -6.0f), "Graphics/Tiles/Common", new List<string> { "et_ankh_v2_edit" });
        Spawner.SpawnStack(StackType.Tile, new Vector2(-46.0f, -2.0f), "Graphics/Tiles/Common", new List<string> { "et_horseshoe_v2_edit" });
        Spawner.SpawnStack(StackType.Tile, new Vector2(-46.0f, 2.0f), "Graphics/Tiles/Common", new List<string> { "et_sword_v3_edit" });
        Spawner.SpawnStack(StackType.Tile, new Vector2(-46.0f, 6.0f), "Graphics/Tiles/Common", new List<string> { "et_backpack_v3_grey_edit" });

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

    private void SetupStacks(StackType stackType, List<string> elements, Vector2 panelPosition, InitialPosition[] initialPosition, Vector2 offset)
    {
        var subPath = stackType.ToString() + "s";
        int countInStack = elements.Count / GameSettings.NumberOfPlayers;
        var stackElementLists = elements.ChunkBy(countInStack + 1);

        for (var i = 0; i < stackElementLists.Count; ++i)
        {
            Spawner.SpawnStack(stackType, panelPosition * initialPosition[i].BoardQuarter + offset, $"Graphics/{subPath}/Common", stackElementLists[i]);
        }
    }
}
