using Assets.Scripts.Elements;
using Assets.Scripts.Utils.Extensions;
using System.Collections.Generic;
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
        diamondTileInstance.Removable = false;

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

    public Element SpawnCorridor(string pathFront, Vector2 position)
    {
        var corridorInstance = Instantiate(GetElementPrefab(), position, Quaternion.identity);
        corridorInstance.Spinnable = true;
        var corridorBackPath = "Graphics/Corridors/road1_explo_tex2_p3p10";
        SpritesLoader.Load(corridorInstance, pathFront, corridorBackPath);
        corridorInstance.SetLayer(SortingLayers.Corridors);

        return corridorInstance;
    }

    public Element SpawnPassage(Vector2 position)
    {
        var passageInstance = Instantiate(GetElementPrefab(), position, Quaternion.identity);
        passageInstance.Spinnable = true;
        SpritesLoader.Load(passageInstance, "Graphics/Corridors/road_explo2_tex_v2");
        passageInstance.SetLayer(SortingLayers.Passages);

        return passageInstance;
    }

    public Element SpawnPawn(string color, Vector2 position)
    {
        var pawnInstance = Instantiate(GetElementPrefab(), position, Quaternion.identity);
        pawnInstance.Spinnable = false; //todo: true, create back graphics for that -> :|
        pawnInstance.Removable = false;

        var pawnPath = $"Graphics/Pawns/pawn_{color}";
        SpritesLoader.Load(pawnInstance, pawnPath);
        pawnInstance.SetLayer(SortingLayers.Pawns);

        return pawnInstance;
    }

    public Element SpawnGetActionToken(Vector2 position)
    {
        return SpawnActionToken("Graphics/Tokens/grabitem_v3_edit", "Graphics/Tokens/grabitem_v3_back_edit", position);
    }

    public Element SpawnUseActionToken(Vector2 position)
    {
        return SpawnActionToken("Graphics/Tokens/tkn_useitem_v2_edit", "Graphics/Tokens/tkn_useitem_v2_back_edit", position);
    }

    public Element SpawnControlActionToken(Vector2 position)
    {
        return SpawnActionToken("Graphics/Tokens/tkn_control_edit", "Graphics/Tokens/tkn_control_back_edit", position);
    }

    private Element SpawnActionToken(string frontPath, string backPath, Vector2 position)
    {
        var actionTokenInstance = Instantiate(GetElementPrefab(), position, Quaternion.identity);
        actionTokenInstance.Spinnable = false;
        actionTokenInstance.Removable = false;

        SpritesLoader.Load(actionTokenInstance, frontPath, backPath);

        return actionTokenInstance;
    }

    public PlayerPanel SpawnPlayerPanel(string color, string name, Vector2 position, Vector2 boardQuarter)
    {
        var playerPanelInstance = Instantiate(Resources.Load<PlayerPanel>("Prefabs/PlayerPanel"), position * boardQuarter, Quaternion.identity);

        var spriteColor = new Color().ToColor(color);
        spriteColor.a = 0.45f;
        playerPanelInstance.Color = spriteColor;
        playerPanelInstance.Name = name;

        return playerPanelInstance;
    }

    public Stack SpawnTilesStack(StackType stackType, Vector2 position, string path, List<string> elements)
    {
        var stack = Instantiate(Resources.Load<Stack>("Prefabs/Stack"), position, Quaternion.identity);
        stack.Initialize(stackType, path, elements);
        stack.SpawnOnTop();

        return stack;
    }

    public Stack SpawnStack(StackType stackType, Vector2 position, string path, List<string> elements)
    {
        var stack = Instantiate(Resources.Load<Stack>("Prefabs/Stack"), position, Quaternion.identity);
        stack.Initialize(stackType, path, elements);
        stack.SpawnOnTop();

        return stack;
    }
}