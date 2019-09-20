using UnityEngine;
using System.Collections;
using Assets.Scripts.Utils;

public class ElementCounterToElementCountConverter : MonoBehaviour, IConverter<ElementCounter, ElementCount>
{
    public ElementCount Convert(ElementCounter source)
    {
        return new ElementCount
        {
            Name = source.Image.sprite.name,
            Count = int.Parse(source.CountInput.text)
        };
    }
}
