using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Server : MonoBehaviour
{
    [SerializeField] private NetworkConfig _networkConfig;
    
    public static Client[] Clients;
    private TCPServer _tcpServer;

    private void Start()
    {
        Debug.Log("Starting Server...");
        InitializeServerData();

        _tcpServer = new TCPServer();
        _tcpServer.StartTCPSocket(_networkConfig.Port);
        Debug.Log("Server started waiting for clients");
    }

    private void OnApplicationQuit()
    {
        _tcpServer.CloseTCPSocket();
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
}