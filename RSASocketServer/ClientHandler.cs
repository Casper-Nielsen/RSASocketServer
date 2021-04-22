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
        private readonly TcpClient client;
        private readonly Task task;
        private RSACryptor cryptor;
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
            cryptor = new RSACryptor(2048,true);
            Console.WriteLine("show rsa (true/(false))");
            bool inputbool = false;
            string input = Console.ReadLine();
            bool.TryParse(input, out inputbool);
            if (inputbool)
            {
                // Writes the rsa parametes
                RSAParameters parameters = cryptor.GetParameters(true);
                Console.WriteLine("Exponent: " + Convert.ToBase64String(parameters.Exponent));
                Console.WriteLine();
                Console.WriteLine("Modulus: " + Convert.ToBase64String(parameters.Modulus));
                Console.WriteLine();
                Console.WriteLine("P: " + Convert.ToBase64String(parameters.P));
                Console.WriteLine();
                Console.WriteLine("Q: " + Convert.ToBase64String(parameters.Q));
                Console.WriteLine();
                Console.WriteLine("InverseQ: " + Convert.ToBase64String(parameters.InverseQ));
                Console.WriteLine();
                Console.WriteLine("D: " + Convert.ToBase64String(parameters.D));
                Console.WriteLine();
                Console.WriteLine("DP: " + Convert.ToBase64String(parameters.DP));
                Console.WriteLine();
                Console.WriteLine("DQ: " + Convert.ToBase64String(parameters.DQ));
                Console.WriteLine();
            }
            Socket socket = client.Client;
            socket.Send(cryptor.GetPublicKey());
            byte[] bytes = new byte[0];
            // runs while client is connected
            while (client.Connected)
            {
                bytes = new byte[64000];
                int lenght = socket.Receive(bytes);
                Array.Resize(ref bytes, lenght);
                Console.WriteLine(Encoding.UTF8.GetString(cryptor.Decrypt(bytes)));
            }
            Console.WriteLine("closing client");
        }

        /// <summary>
        /// Closes the Client
        /// </summary>
        public void Close()
        {
            cryptor.DeleteKeyInCsp();
            client.Close();
        }
    }
}
