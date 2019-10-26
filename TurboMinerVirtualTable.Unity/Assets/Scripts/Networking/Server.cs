using Assets.Scripts.Networking;
using Assets.Scripts.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

public class Server : MonoBehaviour
{
    public int Port = 58964;

    private List<ServerClient> clients;
    private List<ServerClient> disconnectList;

    private TcpListener server;

    private List<Thread> clientThreads = new List<Thread>();

    public void Init()
    {
        DontDestroyOnLoad(gameObject);
        clients = new List<ServerClient>();
        disconnectList = new List<ServerClient>();

        try
        {
            server = new TcpListener(IPAddress.Any, Port);
            server.Start();

            StartListening();
        }
        catch (Exception e)
        {
            Debug.Log($"Socket error: {e.Message}");
        }
    }

    private void StartListening()
    {
        server.BeginAcceptTcpClient(AcceptTcpClient, server);
    }

    private void AcceptTcpClient(IAsyncResult result)
    {
        var listener = (TcpListener)result.AsyncState;

        // get all clients names to message
        var allUseres = string.Empty;
        foreach (var client in clients)
        {
            allUseres += $"{client.ClientName}|";
        }

        var serverClient = new ServerClient(listener.EndAcceptTcpClient(result));
        clients.Add(serverClient);
        StartClientThread(serverClient);

        Debug.Log("Somebody has connected!");

        StartListening();

        Broadcast($"{MessageCommands.Server.Who}|{allUseres}", serverClient);
    }

    private void StartClientThread(ServerClient client)
    {
        var thread = new Thread(() => ReadData(client));
        thread.Start();
        thread.IsBackground = true;
        clientThreads.Add(thread);
    }

    private void ReadData(ServerClient client)
    {
        while (true)
        {
            if (client == null)
            {
                return;
            }

            if (!IsConnected(client.Tcp))
            {
                client.Tcp.Close();
                disconnectList.Add(client);
                clients.Remove(client);
                return;
            }

            var stream = client.Tcp.GetStream();

            if (stream.DataAvailable)
            {
                var reader = new StreamReader(stream, true);
                string data;
                while ((data = reader.ReadLine()) != null)
                {
                    OnIncomingData(client, data);
                }
            }
        }
    }

    private bool IsConnected(TcpClient client)
    {
        try
        {
            if (client != null && client.Client != null && client.Connected)
            {
                if (client.Client.Poll(0, SelectMode.SelectRead))
                {
                    return client.Client.Receive(new byte[1], SocketFlags.Peek) != 0;
                }

                return true;
            }

            return false;
        }
        catch (Exception)
        {
            return false;
        }
    }

    private void Broadcast(string data, ServerClient client)
    {
        try
        {
            var writer = new StreamWriter(client.Tcp.GetStream());
            writer.WriteLine(data);
            writer.Flush();
        }
        catch (Exception e)
        {
            Debug.Log($"Write error: {e.Message}");
        }
    }

    private void Broadcast(string data, IEnumerable<ServerClient> serverClients)
    {
        foreach (var serverClient in serverClients)
        {
            Broadcast(data, serverClient);
        }
    }

    private void OnIncomingData(ServerClient client, string data)
    {
        Debug.Log($"Server: {data}");

        var message = data.Split('|');

        var clientsNoSender = clients.Except(new List<ServerClient>() { client });

        switch (message[0])
        {
            case MessageCommands.Client.Who:
                client.ClientName = message[1];
                client.IsHost = message[2] == "1";
                Broadcast($"{MessageCommands.Server.Connected}|{client.ClientName}", clients);
                break;

            case MessageCommands.Client.Start:
                Broadcast($"{MessageCommands.Server.Start}|{message[1]}|{message[2]}|{message[3]}|{message[4]}|{message[5]}", clientsNoSender);
                break;

            case MessageCommands.Client.WidthSettings:
                Broadcast($"{MessageCommands.Server.WidthSettings}|{message[1]}", clientsNoSender);
                break;

            case MessageCommands.Client.HeightSettings:
                Broadcast($"{MessageCommands.Server.HeightSettings}|{message[1]}", clientsNoSender);
                break;

            case MessageCommands.Client.TilesConfigName:
                Broadcast($"{MessageCommands.Server.TilesConfigName}|{message[1]}", clientsNoSender);
                break;

            case MessageCommands.Client.CorridorsConfigName:
                Broadcast($"{MessageCommands.Server.CorridorsConfigName}|{message[1]}", clientsNoSender);
                break;

            case MessageCommands.Client.ElementPosition:
                Broadcast($"{MessageCommands.Server.ElementPosition}|{message[1]}", clientsNoSender);
                break;

            case MessageCommands.Client.ElementStopDrag:
                Broadcast($"{MessageCommands.Server.ElementStopDrag}|{message[1]}", clientsNoSender);
                break;

            case MessageCommands.Client.ElementLayer:
                Broadcast($"{MessageCommands.Server.ElementLayer}|{message[1]}", clientsNoSender);
                break;

            case MessageCommands.Client.ElementTurn:
                Broadcast($"{MessageCommands.Server.ElementTurn}|{message[1]}", clients);
                break;

            case MessageCommands.Client.ElementRotate:
                Broadcast($"{MessageCommands.Server.ElementRotate}|{message[1]}", clients);
                break;

            case MessageCommands.Client.ElementDestroy:
                Broadcast($"{MessageCommands.Server.ElementDestroy}|{message[1]}", clients);
                break;

            case MessageCommands.Client.RollDice:
                Broadcast($"{MessageCommands.Server.RollDice}|{RngHelper.GetRandom(1, 6)}", clients);
                break;

            case MessageCommands.Client.StackRefill:
                Broadcast($"{MessageCommands.Server.StackRefill}|{message[1]}", clients);
                break;
        }
    }
    private void OnApplicationQuit()
    {
        CloseServer();
    }
    private void OnDisable()
    {
        CloseServer();
    }

    private void CloseServer()
    {
        foreach(var thread in clientThreads)
        {
            thread.Abort();
        }
        server.Stop();
    }
}