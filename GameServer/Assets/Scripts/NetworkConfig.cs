using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/NetworkConfig")]
public class NetworkConfig : ScriptableObject
{
    public int MaxPlayers = 100;
    public string IPAddress = "127.0.0.1";
    public int Port = 5555;
}