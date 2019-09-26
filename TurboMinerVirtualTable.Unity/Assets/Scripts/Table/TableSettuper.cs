using UnityEngine;
using System.Collections;

public class TableSettuper : MonoBehaviour
{
    public Spawner Spawner;
    public BoardPositioner BoardPositioner;
        

    void Start()
    {
        var diamond = Spawner.SpawnDiamond(new Vector2(0, 0));
        var diamondCorridor = Spawner.SpawnDiamondCorridor(new Vector2(0, 0));
        diamondCorridor.ContainedElements.Add(diamond.transform);


        int xPosition = GameSettings.MapSize.Width / 2 - 1;
        int yPosition = GameSettings.MapSize.Height / 2 - 1;

        if(GameSettings.NumberOfPlayers == 1)
        {
            var topRight = BoardPositioner.ToWorldPosition(new Vector2(xPosition, yPosition));
            var topRightCorridor = Spawner.SpawnLCorridor(topRight);
        }
        if (GameSettings.NumberOfPlayers == 2)
        {
            var bottomLeft = BoardPositioner.ToWorldPosition(new Vector2(-xPosition, -yPosition));
            var bottomLeftCorridor = Spawner.SpawnLCorridor(bottomLeft);
        }
        if (GameSettings.NumberOfPlayers == 3)
        {
            var bottomLeft = BoardPositioner.ToWorldPosition(new Vector2(-xPosition, -yPosition));
            var bottomLeftCorridor = Spawner.SpawnLCorridor(bottomLeft);
        }
        if (GameSettings.NumberOfPlayers == 4)
        {
            var bottomLeft = BoardPositioner.ToWorldPosition(new Vector2(-xPosition, -yPosition));
            var bottomLeftCorridor = Spawner.SpawnLCorridor(bottomLeft);
        }


    }

}
