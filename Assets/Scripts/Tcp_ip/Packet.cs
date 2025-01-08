using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public enum PacketType
{
    Welcome = 1,
    Attack,
    Skill,
    Switch,
    Turn,
    CostUpdate,
    CostAdd,
    PlayerCount,
    TurnTimeRequest,
    TurnTimeSync
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
    
    public void Write(string value)
    {
        byte[] stringBytes = Encoding.UTF8.GetBytes(value); // 문자열을 UTF-8로 변환
        Write(stringBytes.Length); // 문자열 길이 기록
        _buffer.AddRange(stringBytes); // 실제 문자열 데이터 추가
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
    
    public string ReadString()
    {
        try
        {
            int length = ReadInt(); // 문자열 길이 읽기
            string value = Encoding.UTF8.GetString(_buffer.ToArray(), _readPos, length); // UTF-8로 변환
            _readPos += length; // 읽은 위치 이동
            return value;
        }
        catch (Exception e)
        {
            Debug.LogError($"ReadString Error: {e.Message}");
            return null;
        }
    }

}
