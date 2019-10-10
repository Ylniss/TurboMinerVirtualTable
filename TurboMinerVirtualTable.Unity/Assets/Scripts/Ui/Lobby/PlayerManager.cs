using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class PlayerManager : MonoBehaviour
{
    public GameObject PlayersList;
    public Button StartButton;

    private List<PlayerLabel> PlayersLabels;

    private void Start()
    {
        PlayersLabels = new List<PlayerLabel>();
    }

    private void Update()
    {
        if(PlayersLabels.Count > 1 && 
           PlayersLabels.Select(l => l.GetComponentInChildren<ColorPicker>().image.color).Distinct().Count() == PlayersLabels.Count &&
           PlayersLabels.Select(l => l.NameLabel.text).Distinct().Count() == PlayersLabels.Count)
        {
            StartButton.interactable = true;
        }
        else
        {
            StartButton.interactable = false;
        }
    }

    public void AddPlayer()
    {
        if(PlayersLabels.Count < 4)
        {
            CreateUi();
        }
    }

    private void CreateUi()
    {
        var position = new Vector3(PlayersList.transform.position.x, PlayersList.transform.position.y - PlayersLabels.Count * 45);
        var playerLabel = Instantiate(Resources.Load<PlayerLabel>("Prefabs/PlayerLabel"), position, Quaternion.identity, PlayersList.transform);
        playerLabel.NameLabel.text = "Player" + (PlayersLabels.Count + 1);
        playerLabel.GetComponentInChildren<Button>().onClick.AddListener(() => RemovePlayer(playerLabel));

        SetColor(playerLabel);

        PlayersLabels.Add(playerLabel);
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

    public void RemovePlayers()
    {
        foreach(var label in PlayersLabels)
        {
            Destroy(label.gameObject);
        }

        PlayersLabels.Clear();
    }

    public void RemovePlayer(PlayerLabel label)
    {
        var removedLabelIndex = PlayersLabels.IndexOf(label);

        PlayersLabels.Remove(label);

        if (!(removedLabelIndex == PlayersLabels.Count))
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
