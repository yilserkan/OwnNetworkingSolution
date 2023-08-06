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
        Packets.Add((int)ServerPackets.Welcome, Packet_Welcome);
    }

    private static void Packet_Welcome(Packet packet)
    {
        string msg = packet.ReadString();
        string msg2 = packet.ReadString();
        int level = packet.ReadInt();
        Debug.Log(msg);
        Debug.Log(msg2);
        Debug.Log(level);
        ClientSendPacket.SendWelcomePacket();
    }
    
}