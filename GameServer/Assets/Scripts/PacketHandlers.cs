using System.Collections.Generic;
using SharedLibrary;
using UnityEngine;

public class PacketHandlers
{
    public delegate void PacketHandler(int clientId, Packet packet);
    public static Dictionary<int, PacketHandler> Packets;

    public static void InitializePacketHandlers()
    {
        Packets = new Dictionary<int, PacketHandler>();
        Packets.Add((int)ClientPackets.OnPlayerJoinedReceived, Packet_WelcomeRespond);
    }

    private static void Packet_WelcomeRespond(int clientId, Packet packet)
    {
        string msg = packet.ReadString();
        string msg2 = packet.ReadString();
        Debug.Log(msg);
        Debug.Log(msg2);
    }
}