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

    public void SendSettings()
    {
        var tilesConfig = GetDropdownCurrentChoice(TilesConfigDropdown);
        var corridorsConfig = GetDropdownCurrentChoice(CorridorsConfigDropdown);
        var width= GetDropdownCurrentChoice(WidthChooserDropdown);
        var height = GetDropdownCurrentChoice(HeightChooserDropdown);
        client.Send($"{MessageCommands.Client.Settings}|{tilesConfig}|{corridorsConfig}|{width}|{height}");
    }

    private string GetDropdownCurrentChoice(TMP_Dropdown dropdown)
    {
        var index = dropdown.value != -1 ? dropdown.value : 0;
        return dropdown.options[index].text;
    }

    public void SetSettings(string settingsData)
    {
        var settings = settingsData.Split('|');

        TilesConfigDropdown.options.Add(new TMP_Dropdown.OptionData(settings[1]));
        CorridorsConfigDropdown.options.Add(new TMP_Dropdown.OptionData(settings[2]));
        WidthChooserDropdown.options.Add(new TMP_Dropdown.OptionData(settings[3]));
        HeightChooserDropdown.options.Add(new TMP_Dropdown.OptionData(settings[4]));
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
