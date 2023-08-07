using System.Collections.Generic;
using SharedLibrary;
using UnityEngine;

public class PacketHandlers
{
    public delegate void PacketHandler(Packet packet);
    public static Dictionary<int, PacketHandler> Packets;

    public static void InitializePacketHandlers()
    {
        Packets = new Dictionary<int, PacketHandler>();
        Packets.Add((int)ServerPackets.OnClientConnectedToServer, Packet_OnClientConnectedToServer);
        Packets.Add((int)ServerPackets.OnPlayerJoined, Packet_OnPlayerJoined);
        Packets.Add((int)ServerPackets.OnTestUDPResponse, Packet_TestUDPResponse);
    }

    private static void Packet_TestUDPResponse(Packet packet)
    {
        Debug.Log("UDP response received");
    }

    private static void Packet_OnClientConnectedToServer(Packet packet)
    {
        var connectionId= packet.ReadInt();
        Client.Instance.SetConnectionId(connectionId);
    }

    private static void Packet_OnPlayerJoined(Packet packet)
    {
        // packet
    }
    
}