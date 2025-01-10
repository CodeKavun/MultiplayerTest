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

namespace MultiplayerTest
{
    public static class Server
    {
        private static int port;

        private static UdpClient listener;
        private static Thread serverThread;

        private static bool isRunning = false;

        public static void Start()
        {
            port = 11000;

            isRunning = true;
            serverThread = new Thread(StartListening);
            serverThread.Start();
        }

        private static void StartListening()
        {
            try
            {
                listener = new UdpClient(port);
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 0);

                while (isRunning)
                {
                    if (listener.Available > 0)
                    {
                        byte[] receivedData = listener.Receive(ref endPoint);
                        string clientMessage = Encoding.UTF8.GetString(receivedData);

                        Game1.Text = clientMessage;

                        MessageToPlayer(clientMessage);

                        string message = PlayerToMessage();
                        byte[] bytes = Encoding.UTF8.GetBytes(message);

                        listener.Send(bytes, bytes.Length, endPoint);
                    }

                    Thread.Sleep(5);
                }
            }
            catch (Exception ex)
            {
                Game1.Text = "Server fault:\n" + ex.Message + "\n";
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

        public static void Stop()
        {
            isRunning = false;
            listener?.Close();
        }
    }
}
