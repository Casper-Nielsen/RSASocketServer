using System;

namespace RSASocketServer
{
    class Program
    {
        static void Main(string[] args)
        {
            SocketManager socketManager = new SocketManager();
            _ = socketManager.Start();
            string input = "";
            do
            {
                input = Console.ReadLine();
            } while (input != "end");
            socketManager.Close();
        }
    }
}
