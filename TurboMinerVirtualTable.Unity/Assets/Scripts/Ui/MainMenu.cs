using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameSettuper GameSettuper;
    private DataSender dataSender;

    void Start()
    {
        Screen.SetResolution(1664, 936, false);
        dataSender = FindObjectOfType<DataSender>();
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
        dataSender.SendStartGame(GameSettuper);
        SceneManager.LoadScene("Table");
    }

    public void OnWidthSettingChanged()
    {
        dataSender.SendLobbyWidth();
    }

    public void OnHeightSettingChanged()
    {
        dataSender.SendLobbyHeight();
    }

    public void OnTilesSettingChanged()
    {
        dataSender.SendLobbyTilesConfigName();
    }

    public void OnCorridorsSettingChanged()
    {
        dataSender.SendLobbyCorridorsConfigName();
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
