using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class Server : MonoBehaviour
{
    [SerializeField] private NetworkConfig _networkConfig;
    
    public static Client[] Clients;
    private static TCPServer _tcpServer;
    private static UDPServer _udpServer;

    private void Start()
    {
        Debug.Log("Starting Server...");
        InitializeServerData();

        _tcpServer = new TCPServer();
        _tcpServer.StartTCPSocket(_networkConfig.Port);
        
        _udpServer = new UDPServer();
        _udpServer.BeginReading(_networkConfig.Port, _networkConfig.IPAddress);
                 
        // byte[] sendBytes = Encoding.ASCII.GetBytes("Hello from Server");
        //
        // _udpServer.Socket.Send(sendBytes, sendBytes.Length);
        Debug.Log("Server started waiting for clients");
    }

    private void OnApplicationQuit()
    {
        _tcpServer.CloseSocket();
        _udpServer.CloseSocket();
        for (int i = 0; i < Clients.Length; i++)
        {
            if (Clients[i].IsTCPSocketEmpty()) { continue; }
            
            Clients[i].Disconnect();
        }
    }

    private void InitializeServerData()
    {        
        PacketHandlers.InitializePacketHandlers();
        
        Clients = new Client[_networkConfig.MaxPlayers];
        for (int i = 0; i < Clients.Length; i++)
        {
            Clients[i] = new Client(i);
        }
    }

    public static void SendUDPData(IPEndPoint endPoint, Packet packet)
    {
        _udpServer.SendData(endPoint, packet);
    }
}