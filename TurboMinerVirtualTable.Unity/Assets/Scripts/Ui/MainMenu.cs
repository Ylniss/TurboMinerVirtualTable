using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameSettuper GameSettuper;

    void Start()
    {
        Screen.SetResolution(1664, 936, false);
    }

    public void CreateGame()
    {
        ServerClientManager.Instance.CreateServer();
    }

    public void JoinGame()
    {
        ServerClientManager.Instance.Connect();
        ServerClientManager.Instance.StartButton.interactable = false;
        ServerClientManager.Instance.TilesConfigDropdown.interactable = false;
        ServerClientManager.Instance.WidthChooserDropdown.interactable = false;
        ServerClientManager.Instance.HeightChooserDropdown.interactable = false;
        ServerClientManager.Instance.CorridorsConfigDropdown.interactable = false;
    }

    public void StartGame()
    {
        GameSettuper.Setup();
        ServerClientManager.Instance.StartGame();
    }

    public void OnWidthSettingChanged()
    {
        ServerClientManager.Instance.SendWidthSettings();
    }

    public void OnHeightSettingChanged()
    {
        ServerClientManager.Instance.SendHeightSettings();
    }

    public void OnTilesSettingChanged()
    {
        ServerClientManager.Instance.SendTilesSettings();
    }

    public void OnCorridorsSettingChanged()
    {
        ServerClientManager.Instance.SendCorridorsSettings();
    }

    public void Back()
    {
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
}
