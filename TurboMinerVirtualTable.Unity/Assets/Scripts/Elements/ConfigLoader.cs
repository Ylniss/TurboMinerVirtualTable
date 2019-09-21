using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class ConfigLoader : MonoBehaviour
{
    public ElementCount[] Load(string subPath, string name)
    {
        string path = $"{Application.persistentDataPath}/{subPath}/{name}.cfg";
        if (File.Exists(path))
        {
            var formatter = new BinaryFormatter();
            var stream = new FileStream(path, FileMode.Open);
            var elementCounts = formatter.Deserialize(stream) as ElementCount[];
            stream.Close();

            return elementCounts;
        }
        else
        {
            Debug.LogError($"File with name '{name}' not found in path '{path}'.");
            return null;
        }
    }

    public string[] GetConfigNames(string subPath)
    {
        string path = $"{Application.persistentDataPath}/{subPath}";
        var directoryInfo = new DirectoryInfo(path);
        var files = directoryInfo.GetFiles("*.cfg");

        var names = new string[files.Length];

        for (var i = 0; i < files.Length; ++i)
        {
            names[i] = files[i].Name.Remove(files[i].Name.Length - 4);
        }

        return names;
    }
}
