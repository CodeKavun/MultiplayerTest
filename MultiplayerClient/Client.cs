using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MultiplayerClient
{
    public static class Client
    {
        private static IPAddress address;
        private static int port;
        private static IPEndPoint serverEndPoint;

        public static string guid;
        private static UdpClient client;

        public static Dictionary<string, Vector2> players = [];

        public static void Connect()
        {
            address = IPAddress.Parse("127.0.0.1");
            port = 11000;

            guid = Guid.NewGuid().ToString();

            serverEndPoint = new IPEndPoint(address, port);

            try
            {
                client = new UdpClient();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public static void Handle()
        {
            try
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
            catch (Exception ex)
            {
                Game1.text += "Client error: " + ex.Message + "\n";
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
            client?.Close();
        }
    }
}
