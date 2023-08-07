using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class UDPServer
{
    public UdpClient Socket;

    public void BeginReading(int port, string ipAddress)
    {
        IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, port);
        
        Socket = new UdpClient(endPoint);
        // Socket.Connect(ipAddress, port);
        Socket.BeginReceive(ReceiveCallback, null);
    }

    private void ReceiveCallback(IAsyncResult result)
    {
        try
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 0);
            byte[] data = Socket.EndReceive(result, ref endPoint);
            
            Debug.Log("Receive Callback " + Encoding.ASCII.GetString(data));

            Socket.BeginReceive(ReceiveCallback, null);
            
            if (data.Length < 4 )
            {
                return;
            }
            
            Packet packet = new Packet(data);

            int connectionId = packet.ReadInt();

            if (connectionId == 0)
            {
                return;
            }

            if (Server.Clients[connectionId].Udp.Endpoint == null)
            {
                Server.Clients[connectionId].Udp.Connect(endPoint);
                return;
            }

            if (Server.Clients[connectionId].Udp.Endpoint.ToString() == endPoint.ToString())
            {
                HandleData(connectionId, packet);   
            }

        }
        catch (Exception e)
        {
            Debug.LogError(e);;
            throw;
        }
    }

    private void HandleData(int clientId, Packet packet)
    {
        int packetLength = packet.ReadInt();
        int identifier = packet.ReadInt();
        PacketHandlers.Packets[identifier].Invoke(clientId, packet);
    }

    public void SendData(IPEndPoint endPoint, Packet packet)
    {
        try
        {
            Socket.BeginSend(packet.ToArray(), packet.Length(), endPoint, null,null);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }   
    }

    public void CloseSocket()
    {
        Socket.Close();
        Socket = null;
    }
}