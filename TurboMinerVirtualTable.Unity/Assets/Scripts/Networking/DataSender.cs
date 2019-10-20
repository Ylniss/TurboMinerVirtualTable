using Assets.Scripts.Networking;
using Assets.Scripts.Networking.Models;
using Assets.Scripts.Settings.Models;
using Assets.Scripts.Utils.Extensions;
using System.Collections;
using TMPro;
using UnityEngine;

public class DataSender : MonoBehaviour
{
    private Client client;

    public TMP_Dropdown TilesConfigDropdown;
    public TMP_Dropdown CorridorsConfigDropdown;
    public TMP_Dropdown WidthChooserDropdown;
    public TMP_Dropdown HeightChooserDropdown;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void Init()
    {
        client = FindObjectOfType<Client>();
    }

    public void SendStartGame(GameSettuper gameSettuper)
    {
        gameSettuper.Setup();
        var tiles = GameSettings.Tiles;
        var corridors = GameSettings.Corridors;
        var tilesCsv = string.Join(",", tiles.ToArray());
        var corridorsCsv = string.Join(",", corridors.ToArray());
        var mapWidth = GameSettings.MapSize.Width;
        var mapHeight = GameSettings.MapSize.Height;

        var playerSettingsArray = new PlayerSettingsArray();
        playerSettingsArray.Array = GameSettings.PlayersSettings;

        var playerSettingsJson = JsonUtility.ToJson(playerSettingsArray);
        client.Send($"{MessageCommands.Client.Start}|{tilesCsv}|{corridorsCsv}|{mapWidth}|{mapHeight}|{playerSettingsJson}");
    }

    public void SendLobbyWidth()
    {
        var width = GetDropdownCurrentChoice(WidthChooserDropdown);
        client.Send($"{MessageCommands.Client.WidthSettings}|{width}");
    }

    public void SendLobbyHeight()
    {
        var height = GetDropdownCurrentChoice(HeightChooserDropdown);
        client.Send($"{MessageCommands.Client.HeightSettings}|{height}");
    }

    public void SendLobbyTilesConfigName()
    {
        var tilesConfigName = GetDropdownCurrentChoice(TilesConfigDropdown);
        client.Send($"{MessageCommands.Client.TilesConfigName}|{tilesConfigName}");
    }

    public void SendLobbyCorridorsConfigName()
    {
        var corridorsConfigName = GetDropdownCurrentChoice(CorridorsConfigDropdown);
        client.Send($"{MessageCommands.Client.CorridorsConfigName}|{corridorsConfigName}");
    }

    public void SendAllLobbySettings()
    {
        var width = GetDropdownCurrentChoice(WidthChooserDropdown);
        var height = GetDropdownCurrentChoice(HeightChooserDropdown);
        var tilesConfigName = GetDropdownCurrentChoice(TilesConfigDropdown);
        var corridorsConfigName = GetDropdownCurrentChoice(CorridorsConfigDropdown);
        client.Send($"{MessageCommands.Client.LobbySettings}|{width}|{height}|{tilesConfigName}|{corridorsConfigName}");
    }

    public void SendElementsPositions(ElementPositionArray elements)
    {
        var jsonElements = JsonUtility.ToJson(elements);
        client.Send($"{MessageCommands.Client.ElementPosition}|{jsonElements}");
    }

    public void SendStopElementDrag(int elementId)
    {
        client.Send($"{MessageCommands.Client.ElementStopDrag}|{elementId}");
    }

    public void SendIncrementElementsLayers(ElementIdArray elementIds)
    {
        var jsonElements = JsonUtility.ToJson(elementIds);
        client.Send($"{MessageCommands.Client.ElementLayer}|{jsonElements}");
    }

    public void SendTurnElementOnOtherSide(int elementId)
    {
        client.Send($"{MessageCommands.Client.ElementTurn}|{elementId}");
    }

    public void SendRotateElement(int elementId)
    {
        client.Send($"{MessageCommands.Client.ElementRotate}|{elementId}");
    }

    public void SendRollDice()
    {
        client.Send($"{MessageCommands.Client.RollDice}");
    }

    public void SendDestroyElement(int elementId)
    {
        client.Send($"{MessageCommands.Client.ElementDestroy}|{elementId}");
    }

    public void SendRefillStack(StackRefill stackRefill)
    {
        if (client.IsHost)
        {
            stackRefill.RefillArray.Shuffle();
            var stackRefillJson = JsonUtility.ToJson(stackRefill);
            client.Send($"{MessageCommands.Client.StackRefill}|{stackRefillJson}");
        }
    }

    private string GetDropdownCurrentChoice(TMP_Dropdown dropdown)
    {
        var index = dropdown.value != -1 ? dropdown.value : 0;
        return dropdown.options[index].text;
    }
}