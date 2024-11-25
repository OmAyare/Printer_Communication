//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Net.Sockets;
//using System.Text;
//using System.Threading.Tasks;

//namespace VideoJetApp.ViewModels
//{
//    public class ConnectionService
//    {
//        private TcpClient _client;
//        private NetworkStream ns;
//        MemoryStream ms;
//        public string ConnectionStatus { get; private set; } = "Disconnected";

//        //public async Task<bool> ConnectAsync(string ipAddress, int port)
//        //{
//        //    try
//        //    {
//        //        _client = new TcpClient();
//        //        await _client.ConnectAsync(ipAddress, port);
//        //        ns = _client.GetStream();
//        //        ConnectionStatus = "Connected";
//        //        return true;
//        //    }
//        //    catch (Exception)
//        //    {
//        //        ConnectionStatus = "Failed to connect";
//        //        return false;
//        //    }
//        //}

//        public bool Connect(string ipAddress, int port)
//        {
//            bool connected = false;
//            try
//            {
//                if (!_client.Connected)
//                {
//                    _client = new TcpClient();
//                    _client.Connect(ipAddress, port);
//                    ns = _client.GetStream();
//                    ms = new MemoryStream();
//                    //Recieve();
//                }

//            }
//            catch (Exception ex)
//            {

//            }
//            return connected;
//        }
//        public void Disconnect()
//        {
//            ns.Close();
//            _client?.Close();
//            ConnectionStatus = "Disconnected";
//        }

//        //public async Task SendMessageAsync(string message)
//        //{
//        //    if (_client != null && _client.Connected)
//        //    {
//        //        byte[] buffer = Encoding.ASCII.GetBytes(message);
//        //        await _stream.WriteAsync(buffer, 0, buffer.Length);
//        //    }
//        //}

//        public bool IsConnected => _client?.Connected ?? false;

//        public string SendAndRecieve(string message)
//        {

//            string response = "";
//            try
//            {
//                var msgLength = message.Length;
//                ms.Write(BitConverter.GetBytes(msgLength), 0, sizeof(int));
//                byte[] msgBytes = Encoding.UTF8.GetBytes(message);
//                ms.Write(msgBytes, 0, msgBytes.Length);
//                _client.Client.Send(ms.ToArray());


//                byte[] buffer = new byte[1024];
//                int read = ns.Read(buffer, 0, buffer.Length);
//                if (read > 0)
//                {
//                    response = Encoding.UTF8.GetString(buffer, 0, read);

//                }
//            }
//            catch (Exception ex)
//            {

//            }
//            return response;
//        }
//    }

//}
using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace VideoJetApp.ViewModels
{
    public class ConnectionService
    {
        private TcpClient _client;
        public NetworkStream ns;
        private MemoryStream ms;
        public string ConnectionStatus { get; private set; } = "Disconnected";

        // Constructor: Initialize client, stream and memory stream
        public ConnectionService()
        {
            _client = new TcpClient(); // Ensure _client is initialized here
            ms = new MemoryStream();
        }

        // Method to connect to the server
        public bool Connect(string ipAddress, int port)
        {
            try
            {
                // Check if _client is null, if so, initialize it
                if (_client == null)
                {
                    _client = new TcpClient();
                }

                // If the client is not already connected, try to establish a connection
                if (!_client.Connected)
                {
                    _client.Connect(ipAddress, port); // Connect to the server
                    ns = _client.GetStream(); // Get the network stream for communication
                    ConnectionStatus = "Connected"; // Set the connection status
                    return true;
                }
                else
                {
                    // If already connected, do nothing or handle as needed
                    ConnectionStatus = "Already connected";
                    return false;
                }
            }
            catch (Exception ex)
            {
                // Handle connection failure and update connection status
                ConnectionStatus = $"Failed to connect: {ex.Message}";
                return false;
            }
        }

        // Method to disconnect the client
        public void Disconnect()
        {
            try
            {
                ns?.Close();
                _client?.Close();
                ConnectionStatus = "Disconnected"; // Update status after disconnecting
            }
            catch (Exception ex)
            {
                ConnectionStatus = $"Error disconnecting: {ex.Message}";
            }
        }

        // Check if the client is still connected
        public bool IsConnected => _client?.Connected ?? false;

        public void Send(string message)
        {
            try
            {
                byte[] msgBytes = Encoding.UTF8.GetBytes(message); // Convert the message to a byte array
                _client.Client.Send(msgBytes); // Send the message directly
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Send: {ex.Message}");
            }
        }

        public bool IsStreamOpen()
        {
            return ns != null && ns.CanRead && ns.CanWrite;
        }


        public async Task<string> Receive()
        {
            try
            {
                byte[] buffer = new byte[1024];
                int bytesRead = await ns.ReadAsync(buffer, 0, buffer.Length);  // Use ReadAsync for async operation
                if (bytesRead > 0)
                {
                    string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Console.WriteLine($"Received response: {response}");
                    return response.Trim();  // Return the response to be used in SendRowToServer
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Receive: {ex.Message}");
            }
            return string.Empty;
        }

    }
}
    