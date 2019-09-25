using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameSettuper GameSettuper;
    public void StartGame()
    {
        GameSettuper.Setup();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
