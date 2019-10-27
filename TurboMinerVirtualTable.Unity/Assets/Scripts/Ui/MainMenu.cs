using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    private HostService hostService;
    private string path;

    private TMP_InputField playerNameInput;

    private Button joinButton;
    private Button createButton;

    void Start()
    {
        path = $"{Application.persistentDataPath}/player.txt";

        Screen.SetResolution(1664, 936, false);
        hostService = FindObjectOfType<HostService>();
        playerNameInput = GetComponentInChildren<TMP_InputField>();

        var buttons = GetComponentsInChildren<Button>();

        joinButton = buttons.Single(b => b.gameObject.name == "JoinButton");
        createButton = buttons.Single(b => b.gameObject.name == "CreateButton");

        LoadPlayerName();
        SetButtonsInteractability();
    }

    public void CreateGame()
    {
        hostService.CreateServer();
    }

    public void OnPlayerNameChange()
    {
        SetButtonsInteractability();
    }

    private void SetButtonsInteractability()
    {
        if (string.IsNullOrEmpty(playerNameInput.text))
        {
            joinButton.interactable = false;
            createButton.interactable = false;
        }
        else
        {
            joinButton.interactable = true;
            createButton.interactable = true;
        }
    }

    public void OnPlayerNameDeselect()
    {
        SavePlayerName();
    }

    private void SavePlayerName()
    {
        using (var stream = new StreamWriter(path))
        {
            stream.WriteLine(playerNameInput.text);
        }
    }

    private void LoadPlayerName()
    {
        if (!File.Exists(path))
        {
            return;
        }

        playerNameInput.text = File.ReadAllLines(path)[0];
    }
}