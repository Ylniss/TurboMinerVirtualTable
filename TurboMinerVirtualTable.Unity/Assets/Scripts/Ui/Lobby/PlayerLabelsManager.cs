using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLabelsManager : MonoBehaviour
{
    public GameObject PlayersList;
    public Button StartButton;

    private List<PlayerLabel> PlayersLabels;

    private void Start()
    {
        PlayersLabels = new List<PlayerLabel>();
    }

    public void ToggleStartButtonInteractability()
    {
        if(PlayersLabels.Count > 1 && ArePlayersColoursDifferent())
        {
            StartButton.interactable = true;
        }
        else
        {
            StartButton.interactable = false;
        }
    }

    private bool ArePlayersColoursDifferent()
    {
        return PlayersLabels.Select(l => l.GetComponentInChildren<ColorPicker>().image.color).Distinct().Count() == PlayersLabels.Count;
    }

    public void AddPlayer()
    {
        if(PlayersLabels.Count < 4)
        {
            SetupUi();
        }
    }

    private void SetupUi()
    {
        var position = new Vector3(PlayersList.transform.position.x, PlayersList.transform.position.y - PlayersLabels.Count * 45);
        var playerLabel = Instantiate(Resources.Load<PlayerLabel>("Prefabs/PlayerLabel"), position, Quaternion.identity, PlayersList.transform);

        //if(playerLabel.NameLabel.text == "")
        //{
        //    playerLabel.NameLabel.text = "Player" + (PlayersLabels.Count + 1);
        //}

        AddListeners(playerLabel);
        SetColor(playerLabel);

        PlayersLabels.Add(playerLabel);
    }

    private void AddListeners(PlayerLabel playerLabel)
    {
        var buttons = playerLabel.GetComponentsInChildren<Button>();

        var colorPickerButton = buttons.FirstOrDefault(b => b.gameObject.name == "ColorPickerButton");
        colorPickerButton.onClick.AddListener(() => ToggleStartButtonInteractability());

        var removePlayerButton = buttons.FirstOrDefault(b => b.gameObject.name == "RemovePlayerButton");
        removePlayerButton.onClick.AddListener(() => RemovePlayer(playerLabel));
        removePlayerButton.onClick.AddListener(() => ToggleStartButtonInteractability());
    }

    private void SetColor(PlayerLabel playerLabel)
    {
        var playersColors = new List<Color>();

        foreach(var label in PlayersLabels)
        {
            playersColors.Add(label.GetComponentInChildren<ColorPicker>().image.color);
        }

        playerLabel.GetComponentInChildren<ColorPicker>().ChooseFromAvailable(playersColors);
    }

    public void RemovePlayer(PlayerLabel label)
    {
        var removedLabelIndex = PlayersLabels.IndexOf(label);

        PlayersLabels.Remove(label);

        if (removedLabelIndex != PlayersLabels.Count)
        {
            FixLabelsPosition();
        }

        Destroy(label.gameObject);
    }

    private void FixLabelsPosition()
    {
        foreach(var label in PlayersLabels)
        {
            label.transform.position = new Vector3(PlayersList.transform.position.x, PlayersList.transform.position.y - PlayersLabels.IndexOf(label) * 45);
        }
    }
}
