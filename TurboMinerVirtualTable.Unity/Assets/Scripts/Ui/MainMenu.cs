using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameSettuper GameSettuper;

    void Start()
    {
        Screen.SetResolution(1664, 936, false);
    }

    public void CreateGame()
    {
        MultiplayerManager.Instance.CreateServer();
    }

    public void JoinGame()
    {
        MultiplayerManager.Instance.Connect();
        SetControlsInteractable(false);
    }

    public void StartGame()
    {
        MultiplayerManager.Instance.StartGame(GameSettuper);
        SceneManager.LoadScene("Table");
    }

    public void OnWidthSettingChanged()
    {
        MultiplayerManager.Instance.SendLobbyWidth();
    }

    public void OnHeightSettingChanged()
    {
        MultiplayerManager.Instance.SendLobbyHeight();
    }

    public void OnTilesSettingChanged()
    {
        MultiplayerManager.Instance.SendLobbyTilesConfigName();
    }

    public void OnCorridorsSettingChanged()
    {
        MultiplayerManager.Instance.SendLobbyCorridorsConfigName();
    }

    public void Back()
    {
        SetControlsInteractable(true);
        DestroyIfExists<Client>();
        DestroyIfExists<Server>();
    }

    private void DestroyIfExists<T>() where T : MonoBehaviour
    {
        var server = FindObjectOfType<T>();
        if (server != null)
        {
            Destroy(server.gameObject);
        }
    }

    private void SetControlsInteractable(bool interactable)
    {
        MultiplayerManager.Instance.StartButton.interactable = interactable;
        MultiplayerManager.Instance.TilesConfigDropdown.interactable = interactable;
        MultiplayerManager.Instance.WidthChooserDropdown.interactable = interactable;
        MultiplayerManager.Instance.HeightChooserDropdown.interactable = interactable;
        MultiplayerManager.Instance.CorridorsConfigDropdown.interactable = interactable;
    }
}
