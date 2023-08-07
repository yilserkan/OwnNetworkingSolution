using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using UnityEngine;

public class TCPServer
{
    public TcpListener Socket;
    public void StartTCPSocket(int port)
    {
        Socket = new TcpListener(IPAddress.Any, port);
        Socket.Start();
        Socket.BeginAcceptTcpClient(OnClientConnected, null);
    }

    public void CloseSocket()
    {
        Socket.Stop();
        Socket = null;
    }
    
    private void OnClientConnected(IAsyncResult result)
    {
        try
        {
            var tempSocket = Socket.EndAcceptTcpClient(result);
            Socket.BeginAcceptTcpClient(OnClientConnected, null);

            for (int i = 0; i < Server.Clients.Length; i++)
            {
                if (Server.Clients[i].IsTCPSocketEmpty())
                {
                    Server.Clients[i].ConnectTCPSocket(tempSocket);
                    ServerSendPacket.SendPlayerConnectedToServerData(Server.Clients[i].ConnectionID);
                    return;
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log(e);
            throw;
        }
    }
}