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

    public void CloseTCPSocket()
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
                    ServerSendPacket.SendWelcomePacket(Server.Clients[i].ConnectionID);
                    // Test( Server.Clients[i].ConnectionID);
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

public class UDPServer
{
    public UdpClient Socket;

    public void BeginReading(int port)
    {
        Socket = new UdpClient(port);
        
        Socket.BeginReceive(ReceiveCallback, null);
    }

    private void ReceiveCallback(IAsyncResult result)
    {
        try
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 0);
            byte[] data = Socket.EndReceive(result, ref endPoint);
            Socket.BeginReceive(ReceiveCallback, null);

            if (data.Length <= 4)
            {
                return;
            }
            
            
            
        }
        catch (Exception e)
        {
            Debug.LogError(e);;
            throw;
        }
    }
}