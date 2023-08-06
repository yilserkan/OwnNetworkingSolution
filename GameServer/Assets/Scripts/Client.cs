using System.Net.Sockets;
using UnityEngine;

public class Client
{
    public int ConnectionID;
    public TCP Tcp;
    // public UDP Udp;

    public Client(int connectionID)
    {
        ConnectionID = connectionID;
        Tcp = new TCP(this);
        // Udp = new UDP(this);
    }
    
    public bool IsTCPSocketEmpty()
    {
        return Tcp.Socket == null;
    }

    public void ConnectTCPSocket(TcpClient client)
    {
        Tcp.Socket = client;
        Tcp.Connect();
    }

    public void Disconnect()
    {
        Debug.Log("Client disconnected with ip " + Tcp.Socket.Client.RemoteEndPoint);
        Tcp.Disconnect();
    }
}