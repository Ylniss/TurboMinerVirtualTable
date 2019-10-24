using Assets.Scripts.Networking;
using System;
using System.Collections.Concurrent;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActionsQueue : MonoBehaviour
{
    public LobbyDropdowns LobbyDropdowns;

    private ConcurrentQueue<Action> mainThreadActions = new ConcurrentQueue<Action>();

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
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

    public void AddActionAccordingTo(string[] message)
    {
        switch (message[0])
        {
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
                    LobbyDropdowns.SetWidth(message[1]);
                });
                break;

            case MessageCommands.Server.HeightSettings:
                mainThreadActions.Enqueue(() =>
                {
                    LobbyDropdowns.SetHeight(message[1]);
                });
                break;

            case MessageCommands.Server.TilesConfigName:
                mainThreadActions.Enqueue(() =>
                {
                    LobbyDropdowns.SetTilesConfig(message[1]);
                });
                break;

            case MessageCommands.Server.CorridorsConfigName:
                mainThreadActions.Enqueue(() =>
                {
                    LobbyDropdowns.SetCorridorsConfig(message[1]);
                });
                break;

            case MessageCommands.Server.LobbySettings:
                mainThreadActions.Enqueue(() =>
                {
                    LobbyDropdowns.SetWidth(message[1]);
                    LobbyDropdowns.SetHeight(message[1]);
                    LobbyDropdowns.SetTilesConfig(message[1]);
                    LobbyDropdowns.SetCorridorsConfig(message[1]);
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
}