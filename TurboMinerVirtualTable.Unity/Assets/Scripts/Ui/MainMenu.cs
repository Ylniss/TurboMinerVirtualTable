using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameSettuper GameSettuper;
    public Button StartButton;

    private List<PlayerLabel> playerLabels;
    private HashSet<Color> labelsColors;

    void Start()
    {
        Screen.SetResolution(1664, 936, false);
        labelsColors = new HashSet<Color>();
    }

    private void Update()
    {
        playerLabels = transform.root.GetComponentsInChildren<PlayerLabel>().ToList();

        if (playerLabels.Count > 1)
        {
            foreach(var label in playerLabels)
            {
                labelsColors.Add(label.GetComponentInChildren<ColorPicker>().image.color);
            }

            if(labelsColors.Count == playerLabels.Count)
            {
                StartButton.gameObject.SetActive(true);
            }
            else
            {
                StartButton.gameObject.SetActive(false);
            }
        }
    }

    public void StartGame()
    {
        GameSettuper.Setup();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
