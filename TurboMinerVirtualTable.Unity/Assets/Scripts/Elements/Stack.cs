using Assets.Scripts.Elements;
using Assets.Scripts.Networking.Models;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Stack : MonoBehaviour
{
    public int Id;
    public Spawner Spawner;

    public List<string> Elements = new List<string>();
    private List<string> elementsRefill = new List<string>();

    private Element lastSpawned;
    private StackType stackType;

    private static int idIncrement = 0;

    private void Start()
    {
        Id = ++idIncrement;
    }

    public void Initialize(StackType stackType, string path, List<string> elements)
    {
        Elements = elements;
        this.stackType = stackType;

        var sprite = Resources.Load<Sprite>($"{path}/{elements[0]}");

        transform.localScale = new Vector3(sprite.rect.width / 100, sprite.rect.height / 100, 1);
    }

    void OnTriggerExit(Collider collider)
    {
        var element = collider.gameObject.GetComponentInParent<Element>();

        // only taking object that is on stack from it can spawn another element on top
        if (element.Id != lastSpawned.Id)
        {
            return;
        }

        element.Removable = true;

        elementsRefill.Add(Elements.Last());
        Elements.RemoveAt(Elements.Count - 1);
        if (Elements.Count == 0)
        {
            Refill();
        }
        else
        {
            SpawnOnTop();
        }      
    }

    public Element SpawnOnTop()
    {
        var position = GetComponentsInChildren<Transform>().Last().position;

        Element spawnedElement = null;

        switch (stackType)
        {
            case StackType.Corridor:
                spawnedElement = Spawner.SpawnCorridor($"Graphics/Corridors/Common/{Elements.Last()}", position);
                break;
            case StackType.Tile:
                spawnedElement = Spawner.SpawnTile($"Graphics/Tiles/Common/{Elements.Last()}", position);
                break;
            case StackType.Passage:
                spawnedElement = Spawner.SpawnPassage(position);
                break;
        }

        spawnedElement.Removable = false; // cannot remove element on top of the stack
        lastSpawned = spawnedElement;
        return spawnedElement;
    }

    private void Refill()
    {
        var stackRefill = new StackRefill(Id, elementsRefill.ToArray());
        MultiplayerManager.Instance.SendRefillStack(stackRefill);
        elementsRefill.Clear();
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

    public static Stack Get(int id)
    {
        var stack = FindObjectsOfType<Stack>();
        return stack.Single(e => e.Id == id);
    }
}
