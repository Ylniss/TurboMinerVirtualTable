using System.Net.Sockets;

namespace Assets.Scripts.Networking
{
    public class ServerClient
    {
        public string ClientName;
        public TcpClient Tcp;
        public bool IsHost;

        public ServerClient(TcpClient tcp)
        {
            Tcp = tcp;
        }

    }
}
