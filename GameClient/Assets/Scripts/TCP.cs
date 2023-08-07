using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class TCP
{
    public TcpClient Socket;
    public NetworkStream Stream;
    public byte[] _readBuffer;
    private Packet _packet;

    private const int BUFFER_SIZE = 4096;

    public void Connect(string ipAddress, int port)
    {
        Socket = new TcpClient()
        {
            ReceiveBufferSize = BUFFER_SIZE,
            SendBufferSize = BUFFER_SIZE
        };

        _packet = new Packet();
        _readBuffer = new byte[BUFFER_SIZE];
        
        Socket.BeginConnect(ipAddress, port, ConnectCallback, Socket);
    }

    public void Disconnect()
    {
        Socket.Close();
        Socket = null;
    }

    private void ConnectCallback(IAsyncResult result)
    {
        try
        {
            Socket.EndConnect(result);

            if (!Socket.Connected)
            {
                Debug.Log("Unable to connect to server");
                return;
            }

            _readBuffer = new byte[BUFFER_SIZE];
            
            Stream = Socket.GetStream();
            Stream.BeginRead(_readBuffer, 0, BUFFER_SIZE, ReceiveCallback, null);
            Debug.Log("Connected to server");
            Debug.Log("Server Socket " + ((IPEndPoint)Socket.Client.RemoteEndPoint).Port);
            Debug.Log("Client Socket " + ((IPEndPoint)Socket.Client.LocalEndPoint).Port);
            Client.Instance.ConnectUDPSocket(GetClientLocalPort());
        }
        catch (Exception e)
        {
            Debug.Log("Unable to connect to server " + e);
            throw;
        }
    }
    
    private void ReceiveCallback(IAsyncResult result)
    {
        try
        {
            var packetLength = Stream.EndRead(result);

            if (packetLength <= 0)
            {
                Application.Quit();
                return;
            }

            byte[] receivedBytes = new byte[packetLength];
            Array.Copy(_readBuffer, receivedBytes, packetLength);
            HandleData(receivedBytes);
            Stream.BeginRead(_readBuffer, 0, BUFFER_SIZE, ReceiveCallback, Socket);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            Application.Quit();
            throw;
        }
        
    }

    private void HandleData(byte[] data)
    {
        if (data.Length <= 0) { return; }

        _packet.SetData(data);
        int packetLength = 0;

        if (_packet.Length() >= 4)
        {
            packetLength = _packet.ReadInt();

            if (packetLength <= 0)
            {
                return;
            }
        }
        
        while (packetLength > 0 && packetLength <= _packet.UnreadLength())
        {
            if (packetLength >= 4)
            {
                byte[] bytes = _packet.ReadBytes(packetLength);
                HandleDataPacket(bytes);
            }

            packetLength = 0;
            
            if (_packet.UnreadLength() >= 4)
            {
                packetLength = _packet.ReadInt();

                if (packetLength <= 0)
                {
                    return;
                }
            }
        }
        
        _packet.Clear();
    }

    private void HandleDataPacket(byte[] data)
    {
        Packet packet = new Packet(data);
        int identifier = packet.ReadInt();
        PacketHandlers.Packets[identifier].Invoke(packet);
    }
    
    private int GetClientLocalPort()
    {
        return ((IPEndPoint)Socket.Client.LocalEndPoint).Port;
    }
}