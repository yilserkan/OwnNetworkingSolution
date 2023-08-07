using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class UDP
{
    private Client _client;
    public IPEndPoint Endpoint;

    public UDP(Client client)
    {
        _client = client;
    }
    
    public void Connect(IPEndPoint endPoint)
    {
        Endpoint = endPoint;
        Debug.Log("Send test response");
        Packet packet = new Packet();
        ServerSendPacket.TestUDPRespond(_client.ConnectionID, packet);
    }

    public void SendData(Packet packet)
    {
        packet.InsertInt(_client.ConnectionID);
        Server.SendUDPData(Endpoint, packet);
    }
}