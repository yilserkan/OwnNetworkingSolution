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
        Packets.Add((int)ClientPackets.WelcomeRespond, Packet_WelcomeRespond);
    }

    private static void Packet_WelcomeRespond(Packet packet)
    {
        string msg = packet.ReadString();
        string msg2 = packet.ReadString();
        Debug.Log(msg);
        Debug.Log(msg2);
    }
}