using UnityEngine;

public class ConnectMenu : MonoBehaviour
{
    private HostService hostService;
    public void JoinGame()
    {
        hostService = FindObjectOfType<HostService>();
        hostService.Connect();
    }
}
