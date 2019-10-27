using Assets.Scripts.Networking;
using System;
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

    private ActionsQueue actions;

    private Thread readThread;
    private bool threadStop;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Init()
    {
        actions = FindObjectOfType<ActionsQueue>();
    }

    public bool ConnectToServer(string host, int port)
    {
        if (socketReady)
        {
            return false;
        }

        Init();
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
                    OnIncomingData(data); 
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

        try
        {
            writer.WriteLine(data);
            writer.Flush();
        }
        catch(Exception)
        {
            //todo: message about server disconnection
            CloseSocket();
            return;
        }   
    }

    private void OnIncomingData(string data)
    {
        Debug.Log($"Client: {data}");

        var messageFromServer = data.Split('|');

        if (messageFromServer[0] == MessageCommands.Server.Who)
        {
            Send($"{MessageCommands.Client.Who}|{ClientName}|{(IsHost ? 1 : 0)}");
        }

        actions.AddActionAccordingTo(messageFromServer, IsHost);
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
        readThread.Abort();

        stream.Close();
        writer.Close();
        reader.Close();
        socket.Close();
        socketReady = false;
    }
}