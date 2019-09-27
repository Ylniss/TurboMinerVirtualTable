using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameSettuper GameSettuper;

    void Start()
    {
        Screen.SetResolution(1664, 936, false);
    }

    public void StartGame()
    {
        GameSettuper.Setup();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
