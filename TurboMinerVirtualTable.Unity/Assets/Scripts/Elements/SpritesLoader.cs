using UnityEngine;

public class SpritesLoader : MonoBehaviour
{
    /// <summary>
    /// Gets front and back sprite renderer
    /// </summary>
    public SpriteRenderer[] GetSpriteRenderers(Element element)
    {
        return element.GetComponentsInChildren<SpriteRenderer>(true);
    }

    public void Load(Element element, string path)
    {
        var sprites = GetSpriteRenderers(element);

        var sprite = Resources.Load<Sprite>(path);
        sprites[0].sprite = sprite;
        sprites[1].sprite = sprite;
    }

    public void Load(Element element, string pathFront, string pathBack)
    {
        var sprites = GetSpriteRenderers(element);

        var frontSprite = Resources.Load<Sprite>(pathFront);
        var backSprite = Resources.Load<Sprite>(pathBack);
        sprites[0].sprite = frontSprite;
        sprites[1].sprite = backSprite;
    }
}
