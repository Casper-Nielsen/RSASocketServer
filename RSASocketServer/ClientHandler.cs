using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RSASocketServer
{
    class ClientHandler
    {
        private TcpClient client;
        private Task task;
        private RSACryptor Cryptor;
        public ClientHandler(TcpClient client)
        {
            this.client = client;
            task = RunConnection();
        }

        /// <summary>
        /// Runs the comunication with one client
        /// </summary>
        private async Task RunConnection()
        {
            await Task.Delay(1);
            Console.WriteLine("new client");
            Cryptor = new RSACryptor(2048);
            RSAParameters parameters = Cryptor.GetParameters();
            Console.WriteLine("Exponent: " + BitConverter.ToString(parameters.Exponent));
            Console.WriteLine("Modulus: " + BitConverter.ToString(parameters.Modulus));
            Socket socket = client.Client;
            socket.Send(Cryptor.GetPublicKey());
            byte[] bytes = new byte[0];
            // runs while client is connected
            while (client.Connected)
            {
                bytes = new byte[64000];
                int lenght = socket.Receive(bytes);
                Array.Resize(ref bytes, lenght);
                Console.WriteLine(Encoding.UTF8.GetString(Cryptor.Decrypt(bytes)));
            }
            Console.WriteLine("closing client");
        }
    }
}
