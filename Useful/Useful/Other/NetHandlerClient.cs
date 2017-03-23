using System;
using System.Net.Sockets;
using System.Text;

namespace Useful.Other
{
    /// <summary>
    ///     Class utilising a Tcp client connection.
    /// </summary>
    public class NetHandlerClient
    {
        private TcpClient _client;
        private IAsyncResult _connectResult;
        private string _ip;
        private int _port;
        private Socket _socket;

        public NetHandlerClient()
        {
            TimeOut = -1;
        }

        /// <summary>
        ///     Is client open?
        /// </summary>
        public bool Available => _client?.Available > 0;

        /// <summary>
        ///     Is client connected? Performs ping.
        /// </summary>
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

        /// <summary>
        ///     Returns Timeout time.
        /// </summary>
        public int TimeOut { get; private set; }

        /// <summary>
        ///     Open a client on given port.
        ///     <para>
        ///         Returns:
        ///         <para> 0: if everything went good.</para>
        ///         <para>-1: if port is not vacant.</para>
        ///         <para>-2: if connection could not be started.</para>
        ///     </para>
        /// </summary>
        /// <param name="ip">IP of server to connect to</param>
        /// <param name="port">Port to open a server (1024-65535)</param>
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

        /// <summary>
        ///     Attempts to reconnect after disconnecting.
        /// </summary>
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

        /// <summary>
        ///     Closes the socket and net client.
        /// </summary>
        public void Close()
        {
            _socket?.Close();
            _client?.Close();
            _socket = null;
            _client = null;
        }

        /// <summary>
        ///     Sends a byte array to server.
        /// </summary>
        /// <param name="data">Array to be sent</param>
        public void Send(byte[] data)
        {
            _socket.Send(data);
        }

        /// <summary>
        ///     Sends a string to server.
        /// </summary>
        /// <param name="data">String to be sent</param>
        public void Send(string data)
        {
            var buffer = new byte[(data.Length << 1) + 2];
            Encoding.Unicode.GetBytes(data).CopyTo(buffer, 0);
            buffer[buffer.Length - 1] = 0;
            buffer[buffer.Length - 2] = 0;
            _socket.Send(buffer);
        }

        /// <summary>
        ///     Sends an object to server.
        /// </summary>
        /// <param name="data">Object to be sent</param>
        public void Send(ISendable data)
        {
            var buffer = new byte[data.ByteLength() + 1];
            buffer[0] = data.Identifier();
            data.GetBytes().CopyTo(buffer, 1);
            _socket.Send(buffer);
        }

        /// <summary>
        ///     Receives one byte from the server.
        /// </summary>
        public byte Receive()
        {
            var buffer = new byte[1];
            _socket.Receive(buffer);
            return buffer[0];
        }

        /// <summary>
        ///     Receives a byte array from the server.
        /// </summary>
        /// <param name="length">The length of received array</param>
        public byte[] Receive(int length)
        {
            var buffer1 = new byte[length];
            if (length <= 64)
            {
                _socket.Receive(buffer1);
            }
            else
            {
                var num = 0;
                var buffer2 = new byte[64];
                while (num < length - 64)
                {
                    var length1 = _socket.Receive(buffer2);
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