using UnityEngine;

public class CorridorsConfigList : MonoBehaviour
{
    public ConfigListSetupper ConfigListSettuper;
    private readonly string SubPath = "Corridors";

    void Start()
    {
        ConfigListSettuper.Setup(SubPath);
    }
}
