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


        int xPosition = GameSettings.MapSize.Width / 2;
        int yPosition = GameSettings.MapSize.Height / 2;

        if(GameSettings.NumberOfPlayers >= 1)
        {
            var topRight = BoardPositioner.ToWorldPosition(new Vector2(xPosition, yPosition));
            var topRightCorridor = Spawner.SpawnLCorridor(topRight);
            topRightCorridor.Rotate(3);

            Spawner.SpawnPawn("red", topRight);
        }
        if (GameSettings.NumberOfPlayers >= 2)
        {
            var bottomLeft = BoardPositioner.ToWorldPosition(new Vector2(-xPosition, -yPosition));
            var bottomLeftCorridor = Spawner.SpawnLCorridor(bottomLeft);
            bottomLeftCorridor.Rotate(1);
        }
        if (GameSettings.NumberOfPlayers >= 3)
        {
            var topLeft = BoardPositioner.ToWorldPosition(new Vector2(xPosition, -yPosition));
            var topLeftCorridor = Spawner.SpawnLCorridor(topLeft);
        }
        if (GameSettings.NumberOfPlayers >= 4)
        {
            var bottomRight = BoardPositioner.ToWorldPosition(new Vector2(-xPosition, yPosition));
            var bottomRightCorridor = Spawner.SpawnLCorridor(bottomRight);
            bottomRightCorridor.Rotate(2);
        }


    }

    private void SetupPlayer()
    {

    }

}
