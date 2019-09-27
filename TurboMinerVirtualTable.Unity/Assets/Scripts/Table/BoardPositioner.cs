using Assets.Scripts.Table.Models;
using UnityEngine;

public class BoardPositioner : MonoBehaviour
{
    // initial board quarters and number of rotations to be towards center
    public InitialPosition[] InitialPositions = new InitialPosition[]
    {
        new InitialPosition
        {
            BoardQuarter = new Vector2(1, 1),
            Rotation = 3
        },
        new InitialPosition
        {
            BoardQuarter = new Vector2(-1, -1),
            Rotation = 1
        },
        new InitialPosition
        {
            BoardQuarter = new Vector2(1, -1),
            Rotation = 0
        },
        new InitialPosition
        {
            BoardQuarter = new Vector2(-1, 1),
            Rotation = 2
        }
    };

    private const float borderToFieldRatio = 0.1125f;
    private const float fieldSize = 5.3f;

    public Vector2 ToWorldPosition(Vector2 boardPosition)
    {
        var jumpLength = fieldSize * (borderToFieldRatio + 1);
        return new Vector2(boardPosition.x * jumpLength, boardPosition.y * jumpLength);
    }
}
