using Assets.Scripts.Elements;
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
        diamondTileInstance.SetLayer(SortingLayers.Tiles);

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

    public Element SpawnTile(string tileFrontPath, Vector2 position)
    {
        var tileInstance = Instantiate(GetElementPrefab(), position, Quaternion.identity);
        tileInstance.Spinnable = false;

        var tileBackPath = "Graphics/Tiles/et_rubble__red_edit";
        SpritesLoader.Load(tileInstance, tileFrontPath, tileBackPath);
        tileInstance.SetLayer(SortingLayers.Tiles);

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
        var passage = Instantiate(GetElementPrefab(), position, Quaternion.identity);
        passage.Spinnable = true;

        //todo: finnish

    }

    public Element SpawnPawn(string color, Vector2 position)
    {
        var pawnInstance = Instantiate(GetElementPrefab(), position, Quaternion.identity);
        pawnInstance.Spinnable = false; //todo: true, create back graphics for that -> :|

        var pawnPath = $"Graphics/Pawns/pawn_{color}";
        SpritesLoader.Load(pawnInstance, pawnPath);
        pawnInstance.SetLayer(SortingLayers.Pawns);

        return pawnInstance;
    }

    public Element SpawnGetActionToken(Vector2 position)
    {
        return SpawnActionToken("Graphics/Pawns/Tokens/grabitem_v3_edit", "Graphics/Pawns/Tokens/grabitem_v3_back_edit", position);
    }

    public Element SpawnUseActionToken(Vector2 position)
    {
        return SpawnActionToken("Graphics/Pawns/Tokens/tkn_useitem_v2_edit", "Graphics/Pawns/Tokens/tkn_useitem_v2_back_edit", position);
    }

    public Element SpawnControlActionToken(Vector2 position)
    {
        return SpawnActionToken("Graphics/Pawns/Tokens/tkn_control_edit", "Graphics/Pawns/Tokens/tkn_control_back_edit", position);
    }

    private Element SpawnActionToken(string frontPath, string backPath, Vector2 position)
    {
        var actionTokenInstance = Instantiate(GetElementPrefab(), position, Quaternion.identity);
        actionTokenInstance.Spinnable = true;

        SpritesLoader.Load(actionTokenInstance, frontPath, backPath);
        actionTokenInstance.SetLayer(SortingLayers.Corridor);

        return actionTokenInstance;
    }
}