using Assets.Scripts.Networking;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Client : MonoBehaviour
{
    public string ClientName;
    public bool IsHost;

    private bool socketReady;
    private TcpClient socket;
    private NetworkStream stream;
    private StreamWriter writer;
    private StreamReader reader;

    private List<GameClient> players = new List<GameClient>();

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public bool ConnectToServer(string host, int port)
    {
        if (socketReady)
        {
            return false;
        }

        try
        {
            socket = new TcpClient(host, port);
            stream = socket.GetStream();
            writer = new StreamWriter(stream);
            reader = new StreamReader(stream);
            socketReady = true;
        }
        catch (Exception e)
        {
            Debug.Log($"Socket error: {e.Message}");
        }

        return socketReady;
    }

    private void Update()
    {
        if (!socketReady)
        {
            return;
        }

        if (stream.DataAvailable)
        {
            string data = reader.ReadLine();
            if (data != null)
            {
                OnIncomingData(data);
            }
        }
    }

    public void Send(string data)
    {
        if (!socketReady)
        {
            return;
        }

        writer.WriteLine(data);
        writer.Flush();
    }

    private void OnIncomingData(string data)
    {
        Debug.Log($"Client: {data}");

        var message = data.Split('|');

        switch (message[0])
        {
            case MessageCommands.Server.Who:
                for (int i = 1; i < message.Length - 1; ++i)
                {
                    UserConnected(message[i], false);
                }
                Send($"{MessageCommands.Client.Who}|{ClientName}|{(IsHost ? 1 : 0)}");
                break;

            case MessageCommands.Server.Connected:
                UserConnected(message[1], false);
                break;

            case MessageCommands.Server.Start:
                MultiplayerManager.Instance.SetTilesSettings(message[1]);
                MultiplayerManager.Instance.SetCorridorsSettings(message[2]);
                MultiplayerManager.Instance.SetMapSizeSettings(message[3], message[4]);
                MultiplayerManager.Instance.SetPlayerSettings(message[5]);
                SceneManager.LoadScene("Table");
                break;

            case MessageCommands.Server.WidthSettings:
                MultiplayerManager.Instance.SetLobbyWidthDropdown(message[1]);
                break;

            case MessageCommands.Server.HeightSettings:
                MultiplayerManager.Instance.SetLobbyHeightDropdown(message[1]);
                break;

            case MessageCommands.Server.TilesConfigName:
                MultiplayerManager.Instance.SetLobbyTilesConfigDropdown(message[1]);
                break;

            case MessageCommands.Server.CorridorsConfigName:
                MultiplayerManager.Instance.SetLobbyCorridorsConfigDropdown(message[1]);
                break;

            case MessageCommands.Server.ElementPosition:
                MultiplayerManager.Instance.SetElementsPositions(message[1]);
                break;

            case MessageCommands.Server.ElementStopDrag:
                MultiplayerManager.Instance.StopElementDrag(int.Parse(message[1]));
                break;

            case MessageCommands.Server.ElementLayer:
                MultiplayerManager.Instance.IncrementElementsLayers(message[1]);
                break;

            case MessageCommands.Server.ElementTurn:
                MultiplayerManager.Instance.TurnElementOnOtherSide(int.Parse(message[1]));
                break;

            case MessageCommands.Server.ElementRotate:
                MultiplayerManager.Instance.RotateElement(int.Parse(message[1]));
                break;

            case MessageCommands.Server.RollDice:
                MultiplayerManager.Instance.RollDice(int.Parse(message[1]));
                break;
        }

    }

    private void UserConnected(string name, bool host)
    {
        var gameClient = new GameClient();
        gameClient.Name = name;
        gameClient.IsHost = host;

        players.Add(gameClient);
    }

    private void OnApplicationQuit()
    {
        CloseSocket();
    }
    private void OnDisable()
    {
        CloseSocket();
    }

    private void CloseSocket()
    {
        if (!socketReady)
        {
            return;
        }

        stream.Close();
        writer.Close();
        reader.Close();
        socket.Close();

        socketReady = false;
    }
}