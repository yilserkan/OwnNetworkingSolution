using SharedLibrary;
using UnityEngine;

public class ServerSendPacket
{
    public static void SendTCPPacket(int connectionId, Packet packet)
    {
        packet.WriteLength();
        Server.Clients[connectionId].Tcp.Stream.BeginWrite(packet.ToArray(), 0, packet.Length(), null, null);
    }

    public static void SendTCPPacketToAll(Packet packet)
    {
        for (int i = 0; i < Server.Clients.Length; i++)
        {
            if (Server.Clients[i].Tcp.Socket != null)
            {
                Server.Clients[i].Tcp.Stream.BeginWrite(packet.ToArray(), 0, packet.Length(), null, null);
            }
        }
    }
    
    public static void SendPlayerConnectedToServerData(int connectionId)
    {
        Packet packet = new Packet();
        packet.Write((int)ServerPackets.OnClientConnectedToServer);
        packet.Write(connectionId);
        SendTCPPacket(connectionId, packet);
    }
    
    public static void SendPlayerJoinedData(int connectionId)
    {
        Debug.Log("Sending packet to client " + Server.Clients[connectionId].Tcp.Socket.Client.RemoteEndPoint);
        
        for (int i = 0; i < Server.Clients.Length; i++)
        {
            if (Server.Clients[i].Tcp.Socket != null && Server.Clients[i].ConnectionID != connectionId)
            {
                SendTCPPacket(connectionId, GetPlayerData(i));
            }
        }
        
        SendTCPPacketToAll(GetPlayerData(connectionId));
    }

    private static Packet GetPlayerData(int connectionId)
    {
        Packet packet = new Packet();
        packet.Write((int)ServerPackets.OnPlayerJoined);
        packet.Write(connectionId);
        return packet;   
    }
    
    
    public static void SendUDPData(int connectionID, Packet packet)
    {
        packet.WriteLength();
        Server.Clients[connectionID].Udp.SendData(packet);
    }

    public static void TestUDPRespond(int connectionId, Packet packet)
    {
        packet.Write((int)ServerPackets.OnTestUDPResponse);
        SendUDPData(connectionId, packet);
    }
}