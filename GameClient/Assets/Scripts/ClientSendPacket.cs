using SharedLibrary;

public class ClientSendPacket
{
    public static void SendTCPPacket(Packet packet)
    {
        packet.WriteLength();
        Client.Instance.Tcp.Stream.BeginWrite(packet.ToArray(), 0, packet.Length(), null, null);
    }
    
    public static void SendWelcomePacket()
    {
        Packet packet = new Packet();
        packet.Write((int)ClientPackets.OnPlayerJoinedReceived);
        packet.Write("Test hello");
        packet.Write("Why");

        SendTCPPacket(packet);
    }

    public static void SendUDPPacket(Packet packet)
    {
        packet.WriteLength();
        Client.Instance.Udp.Socket.BeginSend(packet.ToArray(), packet.Length(), null, null);
    }
}