using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class UDP
{
    public UdpClient Socket;
    private IPEndPoint _endPoint;
    
    public void Connect(int localPort, string ipAddress, int port)
    {
        Debug.Log("Started connecting UDP...");
        Socket = new UdpClient(localPort);

        _endPoint = new IPEndPoint(IPAddress.Parse(ipAddress), port);
        Socket.Connect(_endPoint);
        Socket.BeginReceive(ReceiveCallback, null);

        Packet packet = new Packet();
        SendData(packet);
    }

    private void ReceiveCallback(IAsyncResult result)
    {
        IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 0);
        byte[] data = Socket.EndReceive(result, ref endPoint);
        Socket.BeginReceive(ReceiveCallback, null);

        if (data.Length < 4)
        {
            return;
        }

        Packet packet = new Packet(data);
        HandleData(packet);
    }

    private void HandleData(Packet packet)
    {
        int connectionId = packet.ReadInt();
        int packetLength = packet.ReadInt();
        int packetIdentifier = packet.ReadInt();
        
        PacketHandlers.Packets[packetIdentifier].Invoke(packet);
    }
    
    private void SendData(Packet packet)
    {
        packet.InsertInt(Client.Instance.ConnectionID);
        ClientSendPacket.SendUDPPacket(packet);
    }
}