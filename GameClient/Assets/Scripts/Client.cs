using System;
using System.Collections;
using UnityEngine;

public class Client : MonoBehaviour
{
    [SerializeField] private NetworkConfig _networkConfig;

    public static Client Instance;
    
    private TCP _tcp;

    public TCP Tcp => _tcp;
    
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

    private void Disconnect()
    {
        _tcp.Disconnect();
    }
}