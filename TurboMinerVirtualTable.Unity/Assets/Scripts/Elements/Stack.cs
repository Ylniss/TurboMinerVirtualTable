using Assets.Scripts.Elements;
using Assets.Scripts.Utils.Extensions;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Stack : MonoBehaviour
{
    public Spawner Spawner;

    private List<string> elements = new List<string>();
    private List<string> elementsRefill = new List<string>();

    private Element lastSpawned;
    private StackType stackType;

    public void Initialize(StackType stackType, string path, List<string> elements)
    {
        this.elements = elements;
        this.stackType = stackType;

        var sprite = Resources.Load<Sprite>($"{path}/{elements[0]}");

        transform.localScale = new Vector3(sprite.rect.width / 100, sprite.rect.height / 100, 1);

        elements.Shuffle();
    }

    void OnTriggerExit(Collider collider)
    {
        // only taking object that is on stack from it can spawn another element on top
        if (collider.gameObject.GetComponentInParent<Element>().GetInstanceID() != lastSpawned.GetInstanceID())
        {
            return;
        }

        SpawnOnTop();
    }

    public Element SpawnOnTop()
    {
        var position = GetComponentsInChildren<Transform>().Last().position;

        Element spawnedElement = null;

        switch (stackType)
        {
            case StackType.Corridor:
                spawnedElement = Spawner.SpawnCorridor($"Graphics/Corridors/Common/{elements.Last()}", position);
                break;
            case StackType.Tile:
                spawnedElement = Spawner.SpawnTile($"Graphics/Tiles/Common/{elements.Last()}", position);
                break;
            case StackType.Passage:
                spawnedElement = Spawner.SpawnPassage(position);
                break;
        }

        elementsRefill.Add(elements.Last());
        elements.RemoveAt(elements.Count - 1);
        if (elements.Count == 0)
        {
            Refill();
        }
        lastSpawned = spawnedElement;
        return spawnedElement;
    }

    private void Refill()
    {
        elements = new List<string>(elementsRefill);  
        elementsRefill.Clear();
        elements.Shuffle();
    }

    public static List<string> GetElements(ElementCount[] elementCounts)
    {
        var elements = new List<string>();

        foreach (var elementCount in elementCounts)
        {
            for (var i = 0; i < elementCount.Count; ++i)
            {
                elements.Add(elementCount.Name);
            }
        }

        return elements;
    }

}
