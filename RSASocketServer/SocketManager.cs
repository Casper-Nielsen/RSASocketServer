using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace RSASocketServer
{
    class SocketManager
    {
        bool connectionAllowed;
        TcpListener server;
        List<ClientHandler> clients;

        public async Task Start()
        {
            await Task.Delay(1);
            clients = new List<ClientHandler>();
            connectionAllowed = true;
            //string ip = LocalIPAddress().ToString();
            string ip = "10.108.169.20";
            int port = 5001;

            server = new TcpListener(IPAddress.Parse(ip), port);
            server.Start();
            Console.WriteLine("Server has started on {0}:{1}, Waiting for a connection...", ip, port);
            while (connectionAllowed)
            {
                clients.Add(new ClientHandler(server.AcceptTcpClient()));
            }
        }

        /// <summary>
        /// Finds the local IP Address
        /// </summary>
        /// <returns></returns>
        private IPAddress LocalIPAddress()
        {
            if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
            {
                return null;
            }
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            return host
               .AddressList
               .FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);
        }

        /// <summary>
        /// Closes the websocket server
        /// </summary>
        public void Close()
        {
            connectionAllowed = false;
            server.Stop();
        }
    }
}
