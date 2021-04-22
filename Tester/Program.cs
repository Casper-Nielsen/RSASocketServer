using System;
using System.Net.Sockets;
using System.Text;

namespace Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpClient client = new TcpClient();
            RSACryptor cryptor = new RSACryptor();
            client.Connect("10.108.169.20", 5001);
            Socket socket = client.Client;
            byte[] buffer = new byte[64000];
            int lenght = socket.Receive(buffer);
            Array.Resize(ref buffer, lenght);
            cryptor.SetPublicKey(buffer);
            string input = "";
            do
            {
                Console.WriteLine("test sting");
                input = Console.ReadLine();
                buffer = cryptor.Encrypt(Encoding.UTF8.GetBytes(input));
                socket.Send(buffer);
                Console.WriteLine("encrypted message : ");
                Console.WriteLine(Convert.ToBase64String(buffer));
            } while (input != "end");
        }
    }
}
