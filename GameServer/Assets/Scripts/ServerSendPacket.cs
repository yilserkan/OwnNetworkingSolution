using SharedLibrary;
using UnityEngine;

public class ServerSendPacket
{
    public static void SendWelcomePacket(int connectionId)
    {
        Debug.Log("Sending packet to client " + Server.Clients[connectionId].Tcp.Socket.Client.RemoteEndPoint);
        Packet packet = new Packet();
        packet.Write((int)ServerPackets.Welcome);
        packet.Write("Test hello");
        packet.Write("Why");
        packet.Write(10);
        
        SendTCPPacket(connectionId, packet);
    }
    
    public static void SendTCPPacket(int connectionId, Packet packet)
    {
        packet.WriteLength();
        Server.Clients[connectionId].Tcp.Stream.BeginWrite(packet.ToArray(), 0, packet.Length(), null, null);
    }

    public static void SendTCPPacketToAll()
    {
        
    }
}