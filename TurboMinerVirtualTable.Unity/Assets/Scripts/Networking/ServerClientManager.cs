using Assets.Scripts.Networking;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ServerClientManager : MonoBehaviour
{
    public static ServerClientManager Instance { get; set; }

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

    public void StartGame()
    {
        client.Send(MessageCommands.Client.Start);
    }

    public void SendWidthSettings()
    {
        var width= GetDropdownCurrentChoice(WidthChooserDropdown);
        client.Send($"{MessageCommands.Client.WidthSettings}|{width}");
    }

    public void SendHeightSettings()
    {
        var height = GetDropdownCurrentChoice(HeightChooserDropdown);
        client.Send($"{MessageCommands.Client.HeightSettings}|{height}");
    }

    public void SendTilesSettings()
    {
        var tilesConfig = GetDropdownCurrentChoice(TilesConfigDropdown);
        client.Send($"{MessageCommands.Client.TilesSettings}|{tilesConfig}");
    }

    public void SendCorridorsSettings()
    {
        var corridorsConfig = GetDropdownCurrentChoice(CorridorsConfigDropdown);
        client.Send($"{MessageCommands.Client.CorridorsSettings}|{corridorsConfig}");
    }

    private string GetDropdownCurrentChoice(TMP_Dropdown dropdown)
    {
        var index = dropdown.value != -1 ? dropdown.value : 0;
        return dropdown.options[index].text;
    }

    public void SetWidthSettings(string widthData)
    {
        SetSettings(WidthChooserDropdown, widthData);
    }

    public void SetHeightSettings(string heightData)
    {
        SetSettings(HeightChooserDropdown, heightData);
    }

    public void SetTilesSettings(string tilesData)
    {
        SetSettings(TilesConfigDropdown, tilesData);
    }

    public void SetCorridorsSettings(string corridorsData)
    {
        SetSettings(CorridorsConfigDropdown, corridorsData);
    }

    private void SetSettings(TMP_Dropdown dropdown, string settingsData)
    {
        var settings = settingsData.Split('|');
        if (!client.IsHost)
        {
            dropdown.options.Add(new TMP_Dropdown.OptionData(settings[1]));
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