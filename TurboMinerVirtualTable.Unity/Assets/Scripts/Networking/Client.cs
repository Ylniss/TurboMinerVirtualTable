﻿using Assets.Scripts.Networking;
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
                SceneManager.LoadScene("Table");
                break;

            case MessageCommands.Server.WidthSettings:
                ServerClientManager.Instance.SetWidthSettings(data);
                break;

            case MessageCommands.Server.HeightSettings:
                ServerClientManager.Instance.SetHeightSettings(data);
                break;

            case MessageCommands.Server.TilesSettings:
                ServerClientManager.Instance.SetTilesSettings(data);
                break;

            case MessageCommands.Server.CorridorsSettings:
                ServerClientManager.Instance.SetCorridorsSettings(data);
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