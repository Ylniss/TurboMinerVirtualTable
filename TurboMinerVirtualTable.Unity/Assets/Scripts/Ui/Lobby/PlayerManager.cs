using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class PlayerManager : MonoBehaviour
{
    public GameObject PlayersList;
    public Button StartButton;

    private List<PlayerLabel> PlayerLabels;

    private void Start()
    {
        PlayerLabels = new List<PlayerLabel>();
    }

    private void Update()
    {
        if(PlayerLabels.Count > 1 && 
           PlayerLabels.Select(l => l.GetComponentInChildren<ColorPicker>().image.color).Distinct().Count() == PlayerLabels.Count)
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
        if(PlayerLabels.Count < 4)
        {
            CreateUi();
        }
    }

    private void CreateUi()
    {
        var position = new Vector3(PlayersList.transform.position.x, PlayersList.transform.position.y - PlayerLabels.Count * 45);
        var playerLabel = Instantiate(Resources.Load<PlayerLabel>("Prefabs/PlayerLabel"), position, Quaternion.identity, PlayersList.transform);
        playerLabel.NameLabel.text = "Player" + (PlayerLabels.Count + 1);
        playerLabel.GetComponentInChildren<Button>().onClick.AddListener(() => RemovePlayer(playerLabel));

        for(int i = 0; i < PlayerLabels.Count; i++)
        {
            playerLabel.GetComponentInChildren<ColorPicker>().ChangeColor();
        }

        PlayerLabels.Add(playerLabel);
    }

    public void RemovePlayers()
    {
        foreach(var label in PlayerLabels)
        {
            Destroy(label.gameObject);
        }

        PlayerLabels.Clear();
    }

    public void RemovePlayer(PlayerLabel label)
    {
        if (!(PlayerLabels.IndexOf(label) == PlayerLabels.Count))
        {
            FixLabelsPosition();
        }

        Destroy(label.gameObject);

        PlayerLabels.Remove(label);
    }

    private void FixLabelsPosition()
    {

    }
}
