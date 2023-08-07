using System;
using System.Collections;
using UnityEngine;

public class Client : MonoBehaviour
{
    [SerializeField] private NetworkConfig _networkConfig;

    public static Client Instance;

    public int ConnectionID;
    private TCP _tcp;
    private UDP _udp;

    public TCP Tcp => _tcp;
    public UDP Udp => _udp;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        PacketHandlers.InitializePacketHandlers();
        ConnectToServer();
    }

    private void OnApplicationQuit()
    {
        Disconnect();
    }

    private void ConnectToServer()
    {
        _tcp = new TCP();
        _tcp.Connect(_networkConfig.IPAddress, _networkConfig.Port);
    }

    public void ConnectUDPSocket(int localPort)
    {
        _udp = new UDP();
        _udp.Connect(localPort, _networkConfig.IPAddress, _networkConfig.Port);
    }

    public void SetConnectionId(int connectionId)
    {
        ConnectionID = connectionId;
    }
    
    private void Disconnect()
    {
        _tcp.Disconnect();
    }
}