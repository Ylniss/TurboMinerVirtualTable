using System.IO;
using UnityEngine;

public class PlayerName : MonoBehaviour
{
    private string path
    {
        get
        {
            return $"{Application.persistentDataPath}/player.txt";
        }
    }

    public void Save(string name)
    {
        using (var stream = new StreamWriter(path))
        {
            stream.WriteLine(name);
        }
    }

    public string Load()
    {
        if (!File.Exists(path))
        {
            return string.Empty;
        }

        return File.ReadAllLines(path)[0];
    }

}
