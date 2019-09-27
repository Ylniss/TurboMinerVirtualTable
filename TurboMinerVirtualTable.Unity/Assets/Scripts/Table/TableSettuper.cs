using Assets.Scripts.Table.Models;
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

        for(var i = 0; i < GameSettings.NumberOfPlayers; ++i)
        {
            SetupPlayer(i, boardPosition, BoardPositioner.InitialPositions[i]);
        }
    }

    private void SetupPlayer(int playerIndex, Vector2 boardPosition, InitialPosition initialPosition)
    {
        var worldPosition = BoardPositioner.ToWorldPosition(boardPosition * initialPosition.BoardQuarter);
        var corridor = Spawner.SpawnLCorridor(worldPosition);
        corridor.Rotate(initialPosition.Rotation);

        var pawn = Spawner.SpawnPawn(GameSettings.PlayersSettings[playerIndex].Color, worldPosition);
        corridor.ContainedElements.Add(pawn.transform);
    }

}
