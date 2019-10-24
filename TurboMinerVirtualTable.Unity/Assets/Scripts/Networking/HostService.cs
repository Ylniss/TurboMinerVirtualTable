using System;
using TMPro;
using UnityEngine;

public class HostService : MonoBehaviour
{
    public GameObject ServerPrefab;
    public GameObject ClientPrefab;

    public TMP_InputField PlayerNameInput;
    public TMP_InputField HostIpInput;

    private Client client;
    private Server server;

    private DataSender dataSender;

    private void Start()
    {
        dataSender = FindObjectOfType<DataSender>();
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
            dataSender.Init();
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
            dataSender.Init();
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            throw;
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