using System;
using System.Net.Sockets;
using UnityEngine;

public class TCP
{
    private Client _client;
    public TcpClient Socket;
    public NetworkStream Stream;
    private Packet _packet;
    private byte[] _readBuffer;
    
    private const int BUFFER_SIZE = 4096;

    public TCP(Client client)
    {
        _client = client;
    }
    
    public void Connect()
    {
        Debug.Log("Client connected with ip " + Socket.Client.RemoteEndPoint);
        Socket.ReceiveBufferSize = BUFFER_SIZE;
        Socket.SendBufferSize = BUFFER_SIZE;
        _readBuffer = new byte[BUFFER_SIZE];

        _packet = new Packet();
        Stream = Socket.GetStream();

        Stream.BeginRead(_readBuffer, 0, BUFFER_SIZE, ReceiveCallback, null);
    }
    
    public void Disconnect()
    {
        Socket.Close();
        Socket = null;
    }
    
    private void ReceiveCallback(IAsyncResult result)
    {
        try
        {
            var packetLength = Stream.EndRead(result);

            if (packetLength <= 0)
            {
                _client.Disconnect();
                return;
            }

            byte[] receivedBytes = new byte[packetLength];
            Array.Copy(_readBuffer, receivedBytes, packetLength);
            HandleData(receivedBytes);
            
            Stream.BeginRead(_readBuffer, 0, BUFFER_SIZE, ReceiveCallback, null);
        }
        catch (Exception e)
        {
            _client.Disconnect();
            Debug.Log(e);
            throw;
        }
    }

    private void HandleData(byte[] data)
    {
        if (data.Length <= 0 ) { return; }

        _packet.SetData(data);

        int packetLength = 0;

        if (_packet.UnreadLength() >= 4)
        {
            packetLength = _packet.ReadInt();

            if (packetLength <= 0)
            {
                return;
            }
        }

        while (packetLength > 0 && packetLength <= _packet.UnreadLength())
        {
            byte[] bytes = _packet.ReadBytes(packetLength);
            HandleDataPackets(bytes);

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

    private void HandleDataPackets(byte[] data)
    {
        Packet packet = new Packet(data);
        int identifier = packet.ReadInt();
        PacketHandlers.Packets[identifier].Invoke(packet);
    }
}

// public class UDP
// {
//     private Client _client;
//     public UdpClient Socket;
//
//     public UDP(Client client)
//     {
//         _client = client;
//     }
//     
//     public void Connect()
//     {
//         Socket = new UdpClient();
//         
//         Socket.Connect(_client.);
//         Socket.
//     }
//     
//
// }