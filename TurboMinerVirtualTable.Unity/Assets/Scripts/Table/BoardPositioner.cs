using UnityEngine;

public class BoardPositioner : MonoBehaviour
{
    public SpriteRenderer boardSprite;

    private float borderToFieldRatio = 0.1125f;
    private float fieldSize = 5.3f;

    public Vector2 ToWorldPosition(Vector2 boardPosition)
    {
        var jumpLength = fieldSize * (borderToFieldRatio + 1);
        return new Vector2(boardPosition.x * jumpLength, boardPosition.y * jumpLength);
    }
}
