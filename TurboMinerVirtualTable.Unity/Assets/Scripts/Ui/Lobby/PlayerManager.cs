using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public GameObject PlayersList;

    private List<PlayerLabel> PlayerLabels;

    private void Start()
    {
        PlayerLabels = new List<PlayerLabel>();
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
            RemovePlayer(label);
        }
    }

    public void RemovePlayer(PlayerLabel label)
    {
        Destroy(label.gameObject);

        if (!(PlayerLabels.IndexOf(label) == PlayerLabels.Count))
        {
            FixLabelsPosition();
        }

        PlayerLabels.Remove(label);
    }

    private void FixLabelsPosition()
    {
    }
}
