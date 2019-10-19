using Assets.Scripts.Networking;
using Assets.Scripts.Networking.Models;
using Assets.Scripts.Settings.Models;
using Assets.Scripts.Utils.Extensions;
using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MultiplayerManager : MonoBehaviour
{
    public static MultiplayerManager Instance { get; set; }

    public GameObject ServerPrefab;
    public GameObject ClientPrefab;

    public TMP_InputField PlayerNameInput;
    public TMP_InputField HostIpInput;

    public TMP_Dropdown TilesConfigDropdown;
    public TMP_Dropdown CorridorsConfigDropdown;
    public TMP_Dropdown WidthChooserDropdown;
    public TMP_Dropdown HeightChooserDropdown;
    public Button StartButton;

    private Client client;
    private Server server;

    private void Start()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void CreateServer()
    {
        try
        {
            server = Instantiate(ServerPrefab).GetComponent<Server>();
            server.Init();
            client = GetClient("Host");
            client.IsHost = true;
            client.ConnectToServer("127.0.0.1", server.Port);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    public void Connect()
    {
        string hostAddress = HostIpInput.text;

        if (string.IsNullOrEmpty(hostAddress))
        {
            hostAddress = "127.0.0.1";
        }

        try
        {
            client = GetClient("Client");
            client.ConnectToServer(hostAddress, 58964);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            throw;
        }
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
        var tilesConfig = GetDropdownCurrentChoice(TilesConfigDropdown);
        client.Send($"{MessageCommands.Client.TilesConfigName}|{tilesConfig}");
    }

    public void SendLobbyCorridorsConfigName()
    {
        var corridorsConfig = GetDropdownCurrentChoice(CorridorsConfigDropdown);
        client.Send($"{MessageCommands.Client.CorridorsConfigName}|{corridorsConfig}");
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

    public void RefillStack(string stackRefillJson)
    {
        var stackRefill = JsonUtility.FromJson<StackRefill>(stackRefillJson);
        var stack = Stack.Get(stackRefill.Id);
        stack.Elements = stackRefill.RefillArray.ToList();
        stack.SpawnOnTop();
    }

    public void DestroyElement(int elementId)
    {
        var element = Element.Get(elementId);
        Destroy(element.gameObject);
    }

    public void RollDice(int finalDiceSide)
    {
        var dice = FindObjectOfType<Dice>();
        dice.StartRolling(finalDiceSide);
    }

    public void IncrementElementsLayers(string jsonElementIds)
    {
        var elementIds = JsonUtility.FromJson<ElementIdArray>(jsonElementIds);
        foreach (var elementId in elementIds.Array)
        {
            var element = Element.Get(elementId.Id);
            element.IncrementLayerOrder();
        }
    }

    public void TurnElementOnOtherSide(int elementId)
    {
        var element = Element.Get(elementId);
        element.TurnOnOtherSide();
    }

    public void RotateElement(int elementId)
    {
        var element = Element.Get(elementId);
        element.Rotate();
    }

    public void SetElementsPositions(string elementsJson)
    {
        var elements = JsonUtility.FromJson<ElementPositionArray>(elementsJson);
        var containerElement = Element.Get(elements.Array[0].Id);
        containerElement.IsDragged = true;
        foreach (var elementPosition in elements.Array)
        {
            var element = Element.Get(elementPosition.Id);    
            element.transform.position = elementPosition.Position;
        }
    }

    public void StopElementDrag(int elementId)
    {
        var element = Element.Get(elementId);
        element.IsDragged = false;
    }

    public void SetTilesSettings(string tilesCsv)
    {
        var tiles = tilesCsv.Split(',');
        GameSettings.Tiles = tiles.ToList();
    }

    public void SetCorridorsSettings(string corridorsCsv)
    {
        var corridors = corridorsCsv.Split(',');
        GameSettings.Corridors = corridors.ToList();
    }

    public void SetMapSizeSettings(string width, string height)
    {
        GameSettings.MapSize = new MapSize(int.Parse(width), int.Parse(height));
    }

    public void SetPlayerSettings(string playerSettingsJson)
    {
        GameSettings.PlayersSettings = JsonUtility.FromJson<PlayerSettingsArray>(playerSettingsJson).Array;
    }

    private string GetDropdownCurrentChoice(TMP_Dropdown dropdown)
    {
        var index = dropdown.value != -1 ? dropdown.value : 0;
        return dropdown.options[index].text;
    }

    public void SetLobbyWidthDropdown(string widthData)
    {
        SetLobbyDropdown(WidthChooserDropdown, widthData);
    }

    public void SetLobbyHeightDropdown(string heightData)
    {
        SetLobbyDropdown(HeightChooserDropdown, heightData);
    }

    public void SetLobbyTilesConfigDropdown(string tilesData)
    {
        SetLobbyDropdown(TilesConfigDropdown, tilesData);
    }

    public void SetLobbyCorridorsConfigDropdown(string corridorsData)
    {
        SetLobbyDropdown(CorridorsConfigDropdown, corridorsData);
    }

    private void SetLobbyDropdown(TMP_Dropdown dropdown, string settingsData)
    {
        if (!client.IsHost)
        {
            dropdown.options.Add(new TMP_Dropdown.OptionData(settingsData));
            dropdown.SetValueWithoutNotify(dropdown.options.Count - 1);
        }
    }

    private Client GetClient(string clientName)
    {
        var client = Instantiate(ClientPrefab).GetComponent<Client>();
        client.ClientName = PlayerNameInput.text;
        if (string.IsNullOrEmpty(client.ClientName))
        {
            client.ClientName = clientName;
        }

        return client;
    }
}