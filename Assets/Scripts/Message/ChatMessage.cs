using UnityEngine;

public struct ChatMessage : Mirror.NetworkMessage
{
    public string sender;
    public string message;
}
