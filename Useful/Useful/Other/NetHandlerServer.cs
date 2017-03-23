using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Useful.Other
{
    /// <summary>
    ///     Class utilising a Tcp server connections.
    /// </summary>
    public class NetHandlerServer
    {
        private readonly List<Socket> _sockets = new List<Socket>(10);
        private TcpListener _listener;

        /// <summary>
        ///     Is server open?
        /// </summary>
        public bool Opened { get; private set; }

        /// <summary>
        ///     IP of a server.
        /// </summary>
        public string Ip { get; private set; }

        /// <summary>
        ///     Check if ClientId is connected to server.
        /// </summary>
        /// <param name="id">ID of a client to check</param>
        public bool Connected(int id)
        {
            if (_sockets[id] == null)
                return false;
            if (_sockets[id].Poll(100, SelectMode.SelectRead))
                return (uint) _sockets[id].Available > 0U;
            return true;
        }

        /// <summary>
        ///     Open a server on given port.
        ///     <para>
        ///         Returns:
        ///         <para> 0: if everything went good.</para>
        ///         <para>-1: if IPAddress could not be received.</para>
        ///         <para>-2: if port is not vacant.</para>
        ///         <para>-3: if listening could not be started.</para>
        ///     </para>
        /// </summary>
        /// <param name="port">Port to open a server (1024-65535)</param>
        public int Open(short port)
        {
            IPAddress localaddr;
            try
            {
                localaddr = IPAddress.Any;
                Ip = localaddr.ToString();
            }
            catch
            {
                return -1;
            }
            try
            {
                _listener = new TcpListener(localaddr, port)
                {
                    ExclusiveAddressUse = true
                };
            }
            catch
            {
                _listener = null;
                return -2;
            }
            try
            {
                _listener.Start();
                Opened = true;
            }
            catch
            {
                _listener = null;
                Opened = false;
                return -3;
            }
            return 0;
        }

        /// <summary>
        ///     Accept pending connection requests.
        ///     <para>
        ///         Returns:
        ///         <para>-1: if there were no connections.</para>
        ///         <para>-2: if there was connection error.</para>
        ///         <para>Default: returns new client's number</para>
        ///     </para>
        /// </summary>
        public int AcceptPending()
        {
            if (!_listener.Pending())
                return -1;
            try
            {
                Socket socket = _listener.AcceptSocket();
                socket.NoDelay = false;
                _sockets.Add(socket);
                return _sockets.Count - 1;
            }
            catch
            {
                return -2;
            }
        }

        /// <summary>
        ///     Updates clients, removing not connected ones.
        ///     Returns list containing all connected client numbers.
        /// </summary>
        public List<int> UpdateSockets()
        {
            var intList = new List<int>();
            for (var index = 0; index < _sockets.Count; ++index)
                if (!Connected(index))
                {
                    _sockets.RemoveAt(index);
                    intList.Add(index + intList.Count);
                    --index;
                }
            return intList;
        }

        /// <summary>
        ///     Closes the server connection, disconnecting every client.
        /// </summary>
        public void Close()
        {
            foreach (Socket socket in _sockets)
                socket.Close();
            _sockets.Clear();
            _listener?.Stop();
            _listener = null;
            Opened = false;
        }

        /// <summary>
        ///     Sends raw byte array to given client.
        /// </summary>
        /// <param name="client">ID of a client to send the data to</param>
        /// <param name="data">Byte array to be sent</param>
        public void Send(int client, byte[] data)
        {
            _sockets[client].Send(data);
        }

        /// <summary>
        ///     Send raw byte array to all clients.
        /// </summary>
        /// <param name="data">Byte array to be sent</param>
        public void SendAll(byte[] data)
        {
            foreach (Socket socket in _sockets)
                socket.Send(data);
        }

        /// <summary>
        ///     Sends byte array to any clients but given.
        /// </summary>
        /// <param name="client">ID of a client who is not receiving the data.</param>
        /// <param name="data">Byte array to be sent</param>
        public void SendAllBut(int client, byte[] data)
        {
            for (var index = 0; index < _sockets.Count; ++index)
                if (index != client)
                    _sockets[index].Send(data);
        }

        /// <summary>
        ///     Sends a string to given client.
        /// </summary>
        /// <param name="client">ID of a client to send the data to</param>
        /// <param name="data">String to be sent</param>
        public void Send(int client, string data)
        {
            var buffer = new byte[(data.Length << 1) + 2];
            Encoding.Unicode.GetBytes(data).CopyTo(buffer, 0);
            buffer[buffer.Length - 2] = 0;
            buffer[buffer.Length - 1] = 0;
            _sockets[client].Send(buffer);
        }

        /// <summary>
        ///     Send a string to all clients.
        /// </summary>
        /// <param name="data">String to be sent</param>
        public void SendAll(string data)
        {
            var buffer = new byte[(data.Length << 1) + 2];
            Encoding.Unicode.GetBytes(data).CopyTo(buffer, 0);
            buffer[buffer.Length - 2] = 0;
            buffer[buffer.Length - 1] = 0;
            foreach (Socket socket in _sockets)
                socket.Send(buffer);
        }

        /// <summary>
        ///     Sends a string to any clients but given.
        /// </summary>
        /// <param name="client">ID of a client who is not receiving the data.</param>
        /// <param name="data">String to be sent</param>
        public void SendAllBut(int client, string data)
        {
            var buffer = new byte[(data.Length << 1) + 2];
            Encoding.Unicode.GetBytes(data).CopyTo(buffer, 0);
            buffer[buffer.Length - 2] = 0;
            buffer[buffer.Length - 1] = 0;
            for (var index = 0; index < _sockets.Count; ++index)
                if (index != client)
                    _sockets[index].Send(buffer);
        }

        /// <summary>
        ///     Sends an object to given client.
        /// </summary>
        /// <param name="client">ID of a client to send the data to</param>
        /// <param name="data">Object to be sent</param>
        public void Send(int client, ISendable data)
        {
            var buffer = new byte[data.ByteLength() + 1];
            buffer[0] = data.Identifier();
            data.GetBytes().CopyTo(buffer, 1);
            _sockets[client].Send(buffer);
        }

        /// <summary>
        ///     Send an object to all clients.
        /// </summary>
        /// <param name="data">Object to be sent</param>
        public void SendAll(ISendable data)
        {
            var buffer = new byte[data.ByteLength() + 1];
            buffer[0] = data.Identifier();
            data.GetBytes().CopyTo(buffer, 1);
            foreach (Socket socket in _sockets)
                socket.Send(buffer);
        }

        /// <summary>
        ///     Sends an object to any clients but given.
        /// </summary>
        /// <param name="client">ID of a client who is not receiving the data.</param>
        /// <param name="data">Object to be sent</param>
        public void SendAllBut(int client, ISendable data)
        {
            var buffer = new byte[data.ByteLength() + 1];
            buffer[0] = data.Identifier();
            data.GetBytes().CopyTo(buffer, 1);
            for (var index = 0; index < _sockets.Count; ++index)
                if (index != client)
                    _sockets[index].Send(buffer);
        }

        /// <summary>
        ///     Receive one byte from any client connected.
        ///     <para>Returns KeyValuePair with client ID and received byte.</para>
        /// </summary>
        public KeyValuePair<int, byte> Receive()
        {
            for (var index = 0; index < _sockets.Count; ++index)
                if (_sockets[index].Available > 0)
                    return new KeyValuePair<int, byte>(index, Receive(index));
            return new KeyValuePair<int, byte>(-1, 0);
        }

        /// <summary>
        ///     Receives one byte from given client.
        /// </summary>
        /// <param name="client">ID of a client from which to receive byte</param>
        public byte Receive(int client)
        {
            var buffer = new byte[1];
            _sockets[client].Receive(buffer);
            return buffer[0];
        }

        /// <summary>
        ///     Receive byte array from given client.
        /// </summary>
        /// <param name="client">ID of a client from which to receive byte</param>
        /// <param name="length">Length of received array</param>
        public byte[] Receive(int client, int length)
        {
            var buffer1 = new byte[length];
            if (length <= 64)
            {
                _sockets[client].Receive(buffer1);
            }
            else
            {
                var num = 0;
                var buffer2 = new byte[64];
                while (num < length - 64)
                {
                    var length1 = _sockets[client].Receive(buffer2);
                    Array.Copy(buffer2, 0, buffer1, num, length1);
                    num += length1;
                }
                if (length != num)
                    Receive(client, length - num).CopyTo(buffer1, num);
            }
            return buffer1;
        }
    }
}