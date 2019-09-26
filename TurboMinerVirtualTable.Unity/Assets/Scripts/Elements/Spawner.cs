using UnityEngine;

public class Spawner : MonoBehaviour
{
    public SpritesLoader SpritesLoader;

    private Element GetElementPrefab()
    {
        return Resources.Load<Element>("Prefabs/Element");
    }

    public Element SpawnDiamond(Vector2 position)
    {
        var diamondTileInstance = Instantiate(GetElementPrefab(), position, Quaternion.identity);
        diamondTileInstance.Spinnable = false;

        SpritesLoader.Load(diamondTileInstance, "Graphics/Tiles/et_diamond_edit");

        return diamondTileInstance;
    }

    public Element SpawnDiamondCorridor(Vector2 position)
    {
        var diamondCorridorInstance = Instantiate(GetElementPrefab(), position, Quaternion.identity);
        diamondCorridorInstance.Spinnable = false;

        var frontSpritePath = "Graphics/Corridors/road5_tex_p3p10";
        var backSpritePath = "Graphics/Corridors/road5_explo_tex_v2";
        SpritesLoader.Load(diamondCorridorInstance, frontSpritePath, backSpritePath);

        return diamondCorridorInstance;
    }

    public Element SpawnTile(string pathFront, Vector2 position)
    {
        var tileInstance = Instantiate(GetElementPrefab(), position, Quaternion.identity);
        tileInstance.Spinnable = false;

        var tileBackPath = "Graphics/Tiles/et_rubble__red_edit";
        SpritesLoader.Load(tileInstance, pathFront, tileBackPath);
        return tileInstance;
    }

    public Element SpawnLCorridor(Vector2 position)
    {
        return SpawnCorridor("Graphics/Corridors/Common/road3_tex_p3p10", position);
    }

    public Element SpawnTCorridor(Vector2 position)
    {
        return SpawnCorridor("Graphics/Corridors/Common/road2_tex_p3p10", position);
    }

    public Element SpawnXCorridor(Vector2 position)
    {
        return SpawnCorridor("Graphics/Corridors/Common/road1_tex_p3p10", position);
    }
    public Element SpawnICorridor(Vector2 position)
    {
        return SpawnCorridor("Graphics/Corridors/Common/road4_tex_p3p10", position);
    }

    private Element SpawnCorridor(string pathFront, Vector2 position)
    {
        var corridorInstance = Instantiate(GetElementPrefab(), position, Quaternion.identity);
        corridorInstance.Spinnable = true;

        var corridorBackPath = "Graphics/Corridors/road1_explo_tex2_p3p10";
        SpritesLoader.Load(corridorInstance, pathFront, corridorBackPath);
        return corridorInstance;
    }

    public void SpawnPassage(Vector2 position)
    {
        //todo: implement
    }

    public void SpawnPawn(string color, Vector2 position)
    {
        //todo: implement
    }

    public void SpawnActionToken(Vector2 position)
    {
        //todo: implement
    }
}