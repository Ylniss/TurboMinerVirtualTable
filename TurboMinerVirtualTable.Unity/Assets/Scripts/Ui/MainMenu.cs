using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    private HostService hostService;
    private TMP_InputField playerNameInput;
    private PlayerName playerName;
    private Button joinButton;
    private Button createButton;

    void Start()
    {
        Screen.SetResolution(1664, 936, false);
        hostService = FindObjectOfType<HostService>();
        playerNameInput = GetComponentInChildren<TMP_InputField>();
        playerName = GetComponent<PlayerName>();

        var buttons = GetComponentsInChildren<Button>();

        joinButton = buttons.Single(b => b.gameObject.name == "JoinButton");
        createButton = buttons.Single(b => b.gameObject.name == "CreateButton");

        playerNameInput.text = playerName.Load();
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
        playerName.Save(playerNameInput.text);
    }
}