using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyMenu : MonoBehaviour
{
    public GameSettuper GameSettuper;
    public LobbyDropdowns LobbyDropdowns;
    public Button StartButton;

    private DataSender dataSender;

    private void OnEnable()
    {
        dataSender = FindObjectOfType<DataSender>();

        LobbyDropdowns.ResetWidthAndHeight();
        LobbyDropdowns.InitConfigs();

        var isHost = FindObjectOfType<Client>().IsHost;
        LobbyDropdowns.SetInteractable(isHost);
    }

    public void StartGame()
    {
        dataSender.SendStartGame(GameSettuper);
        SceneManager.LoadScene("Table");
    }

    public void OnWidthSettingChanged()
    {
        var width = LobbyDropdowns.GetWidthText();
        dataSender.SendLobbyWidth(width);
    }

    public void OnHeightSettingChanged()
    {
        var height = LobbyDropdowns.GetHeightText();
        dataSender.SendLobbyHeight(height);
    }

    public void OnTilesSettingChanged()
    {
        var tilesConfigName = LobbyDropdowns.GetTilesConfigText();
        dataSender.SendLobbyTilesConfigName(tilesConfigName);
    }

    public void OnCorridorsSettingChanged()
    {
        var corridorsConfigName = LobbyDropdowns.GetCorridorsConfigText();
        dataSender.SendLobbyCorridorsConfigName(corridorsConfigName);
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
