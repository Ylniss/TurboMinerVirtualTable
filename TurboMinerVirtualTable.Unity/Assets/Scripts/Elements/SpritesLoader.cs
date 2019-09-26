using UnityEngine;

public class SpritesLoader : MonoBehaviour
{
    public void Load(Element element, string path)
    {
        var sprites = element.GetComponentsInChildren<SpriteRenderer>(true);

        var sprite = Resources.Load<Sprite>(path);
        sprites[0].sprite = sprite;
        sprites[1].sprite = sprite;
    }

    public void Load(Element element, string pathFront, string pathBack)
    {
        var sprites = element.GetComponentsInChildren<SpriteRenderer>(true);

        var spriteFront = Resources.Load<Sprite>(pathFront);
        var spriteBack = Resources.Load<Sprite>(pathBack);
        sprites[0].sprite = spriteFront;
        sprites[1].sprite = spriteBack;
    }
}
