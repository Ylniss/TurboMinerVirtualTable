using UnityEngine;

public class TilesConfigList : MonoBehaviour
{
    public ConfigListSetupper ConfigListSettuper;
    private readonly string SubPath = "Tiles";

    void Start()
    {
        ConfigListSettuper.Setup(SubPath);
    }
}
