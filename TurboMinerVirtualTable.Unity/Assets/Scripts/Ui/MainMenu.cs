using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private HostService hostService;

    void Start()
    {
        Screen.SetResolution(1664, 936, false);
        hostService = FindObjectOfType<HostService>();
    }

    public void CreateGame()
    {
        hostService.CreateServer();
    }
}