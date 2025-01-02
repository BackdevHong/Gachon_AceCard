using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public enum PacketType
{
    Welcome = 1,
    Default,
    ColorChange
}

public class Packet
{
    private List<byte> _buffer;
    private int _readPos = 0;

    public Packet()
    {
        _buffer = new List<byte>();
        _readPos = 0;
    }

    public Packet(byte[] data)
    {
        _buffer = data.ToList();
        _readPos = 0;
    }

    public int ReadInt()
    {
        int value = BitConverter.ToInt32(_buffer.ToArray(), _readPos);
        _readPos += 4;
        return value;
    }

    public byte[] UnreadBytes()
    {
        return _buffer.Skip(_readPos).ToArray();
    }

    public void Write(int value)
    {
        _buffer.AddRange(BitConverter.GetBytes(value));
    }

    public byte[] ToArray()
    {
        return _buffer.ToArray();
    }

    public void Write(float value)
    {
        _buffer.AddRange(BitConverter.GetBytes(value));
    }

    public float ReadFloat()
    {
        float value = BitConverter.ToSingle(_buffer.ToArray(), _readPos);
        _readPos += 4;
        return value;
    }

}
