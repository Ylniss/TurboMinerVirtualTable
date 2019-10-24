using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameSettuper GameSettuper;
    public LobbyDropdowns LobbyDropdowns;
    private DataSender dataSender;

    void Start()
    {
        Screen.SetResolution(1664, 936, false);
        dataSender = FindObjectOfType<DataSender>();
    }

    public void CreateGame()
    {
        MultiplayerManager.Instance.CreateServer();

        var dropdowns = FindObjectsOfType<ConfigDopdown>();
        foreach (var dropdown in dropdowns)
        {
            dropdown.Init();
        }

        SetupMapSizeDropdown(LobbyDropdowns.WidthChooserDropdown);
        SetupMapSizeDropdown(LobbyDropdowns.HeightChooserDropdown);
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

    private void SetupMapSizeDropdown(TMP_Dropdown dropdown)
    {
        dropdown.options = new List<TMP_Dropdown.OptionData>()
        {
            new TMP_Dropdown.OptionData("5"),
            new TMP_Dropdown.OptionData("7"),
            new TMP_Dropdown.OptionData("9")
        };
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
        LobbyDropdowns.TilesConfigDropdown.interactable = interactable;
        LobbyDropdowns.WidthChooserDropdown.interactable = interactable;
        LobbyDropdowns.HeightChooserDropdown.interactable = interactable;
        LobbyDropdowns.CorridorsConfigDropdown.interactable = interactable;
    }
}