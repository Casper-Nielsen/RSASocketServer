using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;

namespace Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            // Connects to the server
            TcpClient client = new TcpClient();
            client.Connect(LocalIPAddress().ToString(), 5001);
            Socket socket = client.Client;

            RSACryptor cryptor = new RSACryptor();

            // Gets the public key
            byte[] buffer = new byte[64000];
            int lenght = socket.Receive(buffer);
            Array.Resize(ref buffer, lenght);
            cryptor.SetPublicKey(buffer);

            Console.WriteLine("show rsa (true/(false))");
            bool inputbool = false;
            bool.TryParse(Console.ReadLine(), out inputbool);
            if (inputbool)
            {
                // Writes the rsa public Parameters
                RSAParameters parameters = cryptor.GetParameters();
                Console.WriteLine("Exponent: " + Convert.ToBase64String(parameters.Exponent));
                Console.WriteLine();
                Console.WriteLine("Modulus: " + Convert.ToBase64String(parameters.Modulus));
                Console.WriteLine();
            }
            string input = "";
            do
            {
                // Gets a messages that will be encrypted and send to the server
                Console.WriteLine("test sting");
                input = Console.ReadLine();
                // Encrypts the message
                buffer = cryptor.Encrypt(Encoding.UTF8.GetBytes(input));
                // Sends the encrypted message
                socket.Send(buffer);

                // Writes the Encrypted message in base64
                Console.WriteLine("encrypted message : ");
                Console.WriteLine(Convert.ToBase64String(buffer));
                Console.WriteLine();
            } while (input != "end");
        }

        /// <summary>
        /// Finds the local IP Address
        /// </summary>
        /// <returns></returns>
        private static IPAddress LocalIPAddress()
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

    }
}
