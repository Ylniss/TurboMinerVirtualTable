using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class ConfigSaver : MonoBehaviour
{
    public ElementCounterToElementCountConverter Converter;
    public void Save(List<ElementCounter> elementCounters, string subPath, string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return;
        }

        string directory = $"{Application.persistentDataPath}/{subPath}";
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        string path = $"{directory}/{name}.cfg";
        var stream = new FileStream(path, FileMode.Create);
        var formatter = new BinaryFormatter();
        var elementCounts = Converter.ConvertMany(elementCounters);
        formatter.Serialize(stream, elementCounts);
        stream.Close();
    }
}
