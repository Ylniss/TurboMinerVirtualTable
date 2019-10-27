using Assets.Scripts.Networking;
using Assets.Scripts.Networking.Models;
using Assets.Scripts.Settings.Models;
using System;
using System.Collections.Concurrent;
using System.Linq;
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

    public void AddActionAccordingTo(string[] message, bool isHost)
    {
        switch (message[0])
        {
            case MessageCommands.Server.Connected:
                if (isHost)
                {
                    mainThreadActions.Enqueue(() =>
                    {
                        // resend all dropdown selected options when new client connected
                        LobbyDropdowns.WidthChooserDropdown.onValueChanged.Invoke(0);
                        LobbyDropdowns.HeightChooserDropdown.onValueChanged.Invoke(0);
                        LobbyDropdowns.TilesConfigDropdown.onValueChanged.Invoke(0);
                        LobbyDropdowns.CorridorsConfigDropdown.onValueChanged.Invoke(0);
                    });
                }
                break;
            case MessageCommands.Server.Start:
                mainThreadActions.Enqueue(() =>
                {
                    SetTilesSettings(message[1]);
                    SetCorridorsSettings(message[2]);
                    SetMapSizeSettings(message[3], message[4]);
                    SetPlayerSettings(message[5]);
                    SceneManager.LoadScene("Table");
                });
                break;

            case MessageCommands.Server.WidthSettings:
                if (!isHost)
                {
                    mainThreadActions.Enqueue(() =>
                    {
                        LobbyDropdowns.SetWidth(message[1]);
                    });
                }
                break;

            case MessageCommands.Server.HeightSettings:
                if (!isHost)
                {
                    mainThreadActions.Enqueue(() =>
                    {
                        LobbyDropdowns.SetHeight(message[1]);
                    });
                }
                break;

            case MessageCommands.Server.TilesConfigName:
                if (!isHost)
                {
                    mainThreadActions.Enqueue(() =>
                    {
                        LobbyDropdowns.SetTilesConfig(message[1]);
                    });
                }
                break;

            case MessageCommands.Server.CorridorsConfigName:
                if (!isHost)
                {
                    mainThreadActions.Enqueue(() =>
                    {
                        LobbyDropdowns.SetCorridorsConfig(message[1]);
                    });
                }
                break;

            case MessageCommands.Server.ElementPosition:
                mainThreadActions.Enqueue(() =>
                {
                    SetElementsPositions(message[1]);
                });
                break;

            case MessageCommands.Server.ElementStopDrag:
                mainThreadActions.Enqueue(() =>
                {
                    StopElementDrag(int.Parse(message[1]));
                });
                break;

            case MessageCommands.Server.ElementLayer:
                mainThreadActions.Enqueue(() =>
                {
                    IncrementElementsLayers(message[1]);
                });
                break;

            case MessageCommands.Server.ElementTurn:
                mainThreadActions.Enqueue(() =>
                {
                    TurnElementOnOtherSide(int.Parse(message[1]));
                });
                break;

            case MessageCommands.Server.ElementRotate:
                mainThreadActions.Enqueue(() =>
                {
                    RotateElement(int.Parse(message[1]));
                });
                break;

            case MessageCommands.Server.ElementDestroy:
                mainThreadActions.Enqueue(() =>
                {
                    DestroyElement(int.Parse(message[1]));
                });
                break;

            case MessageCommands.Server.RollDice:
                mainThreadActions.Enqueue(() =>
                {
                    RollDice(int.Parse(message[1]));
                });
                break;

            case MessageCommands.Server.StackRefill:
                mainThreadActions.Enqueue(() =>
                {
                    RefillStack(message[1]);
                });
                break;
        }
    }

    private void RefillStack(string stackRefillJson)
    {
        var stackRefill = JsonUtility.FromJson<StackRefill>(stackRefillJson);
        var stack = Stack.Get(stackRefill.Id);
        stack.Elements = stackRefill.RefillArray.ToList();
        stack.SpawnOnTop();
    }

    private void DestroyElement(int elementId)
    {
        var element = Element.Get(elementId);
        Destroy(element.gameObject);
    }

    private void RollDice(int finalDiceSide)
    {
        var dice = FindObjectOfType<Dice>();
        dice.StartRolling(finalDiceSide);
    }

    private void IncrementElementsLayers(string jsonElementIds)
    {
        var elementIds = JsonUtility.FromJson<ElementIdArray>(jsonElementIds);
        foreach (var elementId in elementIds.Array)
        {
            var element = Element.Get(elementId.Id);
            element.IncrementLayerOrder();
        }
    }

    private void TurnElementOnOtherSide(int elementId)
    {
        var element = Element.Get(elementId);
        element.TurnOnOtherSide();
    }

    private void RotateElement(int elementId)
    {
        var element = Element.Get(elementId);
        element.Rotate();
    }

    private void SetElementsPositions(string elementsJson)
    {
        var elements = JsonUtility.FromJson<ElementPositionArray>(elementsJson);
        var containerElement = Element.Get(elements.Array[0].Id);
        containerElement.IsDragged = true;
        foreach (var elementPosition in elements.Array)
        {
            var element = Element.Get(elementPosition.Id);
            element.transform.position = elementPosition.Position;
        }
    }

    private void StopElementDrag(int elementId)
    {
        var element = Element.Get(elementId);
        element.IsDragged = false;
    }

    private void SetTilesSettings(string tilesCsv)
    {
        var tiles = tilesCsv.Split(',');
        GameSettings.Tiles = tiles.ToList();
    }

    private void SetCorridorsSettings(string corridorsCsv)
    {
        var corridors = corridorsCsv.Split(',');
        GameSettings.Corridors = corridors.ToList();
    }

    private void SetMapSizeSettings(string width, string height)
    {
        GameSettings.MapSize = new MapSize(int.Parse(width), int.Parse(height));
    }

    private void SetPlayerSettings(string playerSettingsJson)
    {
        GameSettings.PlayersSettings = JsonUtility.FromJson<PlayerSettingsArray>(playerSettingsJson).Array;
    }
}