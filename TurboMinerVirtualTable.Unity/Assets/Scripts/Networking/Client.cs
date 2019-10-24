using Assets.Scripts.Networking;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

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
    private ActionsQueue actions;

    private Thread readThread;
    private bool threadStop;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        dataSender = FindObjectOfType<DataSender>();
        actions = FindObjectOfType<ActionsQueue>();
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

        var messageFromServer = data.Split('|');
        switch (messageFromServer[0])
        {
            case MessageCommands.Server.Who:
                for (int i = 1; i < messageFromServer.Length - 1; ++i)
                {
                    UserConnected(messageFromServer[i], false);
                }
                Send($"{MessageCommands.Client.Who}|{ClientName}|{(IsHost ? 1 : 0)}");

                break;

            case MessageCommands.Server.Connected:
                UserConnected(messageFromServer[1], false);
                if (IsHost)
                {
                    dataSender.SendAllLobbySettings();
                }
                break;
        }

        actions.AddActionAccordingTo(messageFromServer);
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