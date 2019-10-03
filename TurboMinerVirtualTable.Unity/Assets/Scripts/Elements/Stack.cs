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
    private bool corridorsStack;

    public void Initialize(List<string> elements)
    {
        this.elements = elements;

        var sprite = Resources.Load<Sprite>($"Graphics/Tiles/Common/{elements[0]}");
        if(sprite == null)
        {
            sprite = Resources.Load<Sprite>($"Graphics/Corridors/Common/{elements[0]}");
            corridorsStack = true;
        }

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

        Element spawnedElement;

        if (corridorsStack)
        {
            spawnedElement = Spawner.SpawnCorridor($"Graphics/Corridors/Common/{elements.Last()}", position);
        }
        else
        {
            spawnedElement = Spawner.SpawnTile($"Graphics/Tiles/Common/{elements.Last()}", position);
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
