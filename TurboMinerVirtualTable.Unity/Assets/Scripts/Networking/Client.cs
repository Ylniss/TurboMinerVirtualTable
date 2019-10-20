using Assets.Scripts.Networking;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading;
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
    private DataSender dataSender;

    private Thread readThread;
    private bool threadStop;

    private ConcurrentQueue<Action> mainThreadActions = new ConcurrentQueue<Action>();

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        dataSender = FindObjectOfType<DataSender>();
    }

    private void Update()
    {
        if (!mainThreadActions.IsEmpty)
        {
            while (mainThreadActions.TryDequeue(out var action))
            {
                action.Invoke();
            }
        }
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

            readThread = new Thread(new ThreadStart(ReadData));
            readThread.IsBackground = true;
            readThread.Start();
        }
        catch (Exception e)
        {
            Debug.Log($"Socket error: {e.Message}");
        }

        return socketReady;
    }

    private void ReadData()
    {
        while (!threadStop)
        {
            if (!socketReady)
            {
                continue;
            }

            if (stream.DataAvailable)
            {
                string data;
                while ((data = reader.ReadLine()) != null)
                {
                    if (data != null)
                    {
                        OnIncomingData(data);
                    }

                    if (threadStop)
                    {
                        break; // prevents socket error, because on app exit socket is closed from another thread before this thread ends
                    }
                }
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
                if (IsHost)
                {
                    dataSender.SendAllLobbySettings();
                }
                break;

            case MessageCommands.Server.Start:
                mainThreadActions.Enqueue(() =>
                {
                    MultiplayerManager.Instance.SetTilesSettings(message[1]);
                    MultiplayerManager.Instance.SetCorridorsSettings(message[2]);
                    MultiplayerManager.Instance.SetMapSizeSettings(message[3], message[4]);
                    MultiplayerManager.Instance.SetPlayerSettings(message[5]);
                    SceneManager.LoadScene("Table");
                });
                break;

            case MessageCommands.Server.WidthSettings:
                mainThreadActions.Enqueue(() =>
                {
                    MultiplayerManager.Instance.SetLobbyWidthDropdown(message[1]);
                });
                break;

            case MessageCommands.Server.HeightSettings:
                mainThreadActions.Enqueue(() =>
                {
                    MultiplayerManager.Instance.SetLobbyHeightDropdown(message[1]);
                });
                break;

            case MessageCommands.Server.TilesConfigName:
                mainThreadActions.Enqueue(() =>
                {
                    MultiplayerManager.Instance.SetLobbyTilesConfigDropdown(message[1]);
                });
                break;

            case MessageCommands.Server.CorridorsConfigName:
                mainThreadActions.Enqueue(() =>
                {
                    MultiplayerManager.Instance.SetLobbyCorridorsConfigDropdown(message[1]);
                });
                break;

            case MessageCommands.Server.LobbySettings:
                mainThreadActions.Enqueue(() =>
                {
                    MultiplayerManager.Instance.SetLobbyWidthDropdown(message[1]);
                    MultiplayerManager.Instance.SetLobbyHeightDropdown(message[2]);
                    MultiplayerManager.Instance.SetLobbyTilesConfigDropdown(message[3]);
                    MultiplayerManager.Instance.SetLobbyCorridorsConfigDropdown(message[4]);
                });
                break;

            case MessageCommands.Server.ElementPosition:
                mainThreadActions.Enqueue(() =>
                {
                    MultiplayerManager.Instance.SetElementsPositions(message[1]);
                });
                break;

            case MessageCommands.Server.ElementStopDrag:
                mainThreadActions.Enqueue(() =>
                {
                    MultiplayerManager.Instance.StopElementDrag(int.Parse(message[1]));
                });
                break;

            case MessageCommands.Server.ElementLayer:
                mainThreadActions.Enqueue(() =>
                {
                    MultiplayerManager.Instance.IncrementElementsLayers(message[1]);
                });
                break;

            case MessageCommands.Server.ElementTurn:
                mainThreadActions.Enqueue(() =>
                {
                    MultiplayerManager.Instance.TurnElementOnOtherSide(int.Parse(message[1]));
                });
                break;

            case MessageCommands.Server.ElementRotate:
                mainThreadActions.Enqueue(() =>
                {
                    MultiplayerManager.Instance.RotateElement(int.Parse(message[1]));
                });
                break;

            case MessageCommands.Server.ElementDestroy:
                mainThreadActions.Enqueue(() =>
                {
                    MultiplayerManager.Instance.DestroyElement(int.Parse(message[1]));
                });
                break;

            case MessageCommands.Server.RollDice:
                mainThreadActions.Enqueue(() =>
                {
                    MultiplayerManager.Instance.RollDice(int.Parse(message[1]));
                });
                break;

            case MessageCommands.Server.StackRefill:
                mainThreadActions.Enqueue(() =>
                {
                    MultiplayerManager.Instance.RefillStack(message[1]);
                });
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

        threadStop = true;
        stream.Close();
        writer.Close();
        reader.Close();
        socket.Close();
        socketReady = false;
    }
}