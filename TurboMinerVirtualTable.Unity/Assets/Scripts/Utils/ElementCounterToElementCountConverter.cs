using Assets.Scripts.Utils;
using System.Collections.Generic;
using UnityEngine;

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

    public ElementCount[] ConvertMany(List<ElementCounter> source)
    {
        var elementCounts = new ElementCount[source.Count];

        for (var i = 0; i < source.Count; ++i)
        {
            var elementCount = Convert(source[i]);
            elementCounts[i] = elementCount;
        }

        return elementCounts;
    }
}
