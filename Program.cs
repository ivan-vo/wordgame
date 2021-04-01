using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
namespace PrimeService
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpListener server = null;
            try
            {
                IPAddress localAddr = IPAddress.Parse("10.0.0.178");
                Int32 port = 13000;
                server = new TcpListener(localAddr, port);
                server.Start();
                while (true)
                {

                    Game game = new Game();
                    game.CreateRoomForSomePlayer(2, server);
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                server.Stop();
            }
            Console.WriteLine("\nHit enter to continue...");
            Console.Read();
        }
    }
}