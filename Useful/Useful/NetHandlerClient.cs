using System;
using System.Net.Sockets;
using System.Text;

namespace Useful
{
  public class NetHandlerClient
  {
    private TcpClient _client;
    private Socket _socket;
    private string _ip;
    private int _port;
    private IAsyncResult _connectResult;

    public bool Available => _client?.Available > 0;

      public bool Connected
    {
      get
      {
        if (_socket == null)
          return false;
        if (_socket.Poll(100, SelectMode.SelectRead))
          return (uint) _socket.Available > 0U;
        return true;
      }
    }

    public int TimeOut { get; private set; }

    public NetHandlerClient()
    {
      TimeOut = -1;
    }

    public int Open(string ip, int port)
    {
      _ip = ip;
      _port = port;
      try
      {
        _client = new TcpClient(_ip, _port)
        {
          NoDelay = true
        };
        _socket = _client.Client;
      }
      catch (SocketException ex)
      {
        return ex.SocketErrorCode == SocketError.AddressNotAvailable ? -1 : -2;
      }
      return 0;
    }

    public void AttemptReconnect()
    {
      switch (TimeOut)
      {
        case -1:
          Close();
          _client = new TcpClient();
          _connectResult = _client.BeginConnect(_ip, _port, null, null);
          TimeOut = 120;
          break;
        case 0:
          TimeOut = -1;
          try
          {
            _client.EndConnect(_connectResult);
            if (!_client.Connected)
              break;
            _socket = _client.Client;
            break;
          }
          catch
          {
            _client = null;
            _socket = null;
            break;
          }
        default:
          TimeOut = TimeOut - 1;
          break;
      }
    }

    public void Close()
    {
        _socket?.Close();
        _client?.Close();
        _socket = null;
      _client = null;
    }

    public void Send(int data)
    {
      byte[] buffer = new byte[5];
      buffer[0] = 105;
      BitConverter.GetBytes(data).CopyTo(buffer, 1);
      _socket.Send(buffer);
    }

    public void Send(long data)
    {
      byte[] buffer = new byte[9];
      buffer[0] = 108;
      BitConverter.GetBytes(data).CopyTo(buffer, 1);
      _socket.Send(buffer);
    }

    public void Send(byte data)
    {
      _socket.Send(new []{ (byte) 98, data });
    }

    public void Send(byte[] data)
    {
      byte[] buffer = new byte[1 + data.Length];
      buffer[0] = 66;
      data.CopyTo(buffer, 1);
      _socket.Send(buffer);
    }

    public void SendRaw(byte[] data)
    {
      _socket.Send(data);
    }

    public void Send(float data)
    {
      byte[] buffer = new byte[5];
      buffer[0] = 102;
      BitConverter.GetBytes(data).CopyTo(buffer, 1);
      _socket.Send(buffer);
    }

    public void Send(double data)
    {
      byte[] buffer = new byte[9];
      buffer[0] = 100;
      BitConverter.GetBytes(data).CopyTo(buffer, 1);
      _socket.Send(buffer);
    }

    public void Send(string data)
    {
      byte[] buffer = new byte[(data.Length << 1) + 2];
      buffer[0] = 115;
      Encoding.Unicode.GetBytes(data).CopyTo(buffer, 1);
      buffer[buffer.Length - 1] = 0;
      _socket.Send(buffer);
    }

    public void Send(ISendable data)
    {
      byte[] buffer = new byte[data.ByteLength() + 1];
      buffer[0] = data.Identifier();
      data.GetBytes().CopyTo(buffer, 1);
      _socket.Send(buffer);
    }

    public byte Receive()
    {
      byte[] buffer = new byte[1];
      _socket.Receive(buffer);
      return buffer[0];
    }

    public byte[] Receive(int length)
    {
      byte[] buffer1 = new byte[length];
      if (length <= 64)
      {
        _socket.Receive(buffer1);
      }
      else
      {
        int num = 0;
        byte[] buffer2 = new byte[64];
        while (num < length - 64)
        {
          int length1 = _socket.Receive(buffer2);
          Array.Copy(buffer2, 0, buffer1, num, length1);
          num += length1;
        }
        if (length != num)
          Receive(length - num).CopyTo(buffer1, num);
      }
      return buffer1;
    }
  }
}
