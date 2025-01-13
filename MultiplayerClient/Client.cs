using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace MultiplayerClient
{
    public static class Client
    {
        private static IPAddress address;
        private static int port;
        private static IPEndPoint serverEndPoint;

        private static UdpClient client;
        private static Thread clientThread;

        private static bool isRunning = false;

        public static void Connect()
        {
            address = IPAddress.Parse("127.0.0.1");
            port = 11000;

            serverEndPoint = new IPEndPoint(address, port);

            isRunning = true;

            clientThread = new Thread(Handle);
            clientThread.Start();
        }

        private static void Handle()
        {
            try
            {
                client = new UdpClient();

                while (isRunning)
                {
                    byte[] messageData = Encoding.UTF8.GetBytes(PlayerToMessage());
                    client.Send(messageData, messageData.Length, serverEndPoint);

                    if (client.Available > 0)
                    {
                        IPEndPoint remoteEndPoint = null;
                        byte[] received = client.Receive(ref remoteEndPoint);

                        string message = Encoding.UTF8.GetString(received, 0, received.Length);
                        MessageToPlayer(message);

                        Game1.text = message;
                    }
                }
            }
            catch (Exception ex)
            {
                Game1.text = "Client error: " + ex.Message + "\n";
                Debug.WriteLine(ex);
            }
        }

        private static string PlayerToMessage()
        {
            string result = $"{Game1.playerPosition.X};{Game1.playerPosition.Y}";
            return result;
        }

        private static void MessageToPlayer(string message)
        {
            string[] coords = message.Split(";");

            if (float.TryParse(coords[0], out float x)
                && float.TryParse(coords[1], out float y))
            {
                Game1.otherPlayerPosition = new Vector2(x, y);
            }
        }

        public static void Close()
        {
            isRunning = false;
            client?.Close();
        }
    }
}
