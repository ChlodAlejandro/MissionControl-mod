using System;
using System.IO.Pipes;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using MissionControl.Commands;

namespace MissionControl.IO
{
    class ConnectionListener
    {
        /// <summary>
        /// Refers to this ConnectionListener.
        /// </summary>
        private static ConnectionListener _instance;
        /// <summary>
        /// The ConnectionListener instance.
        /// </summary>
        public static ConnectionListener Instance => _instance ?? (_instance = new ConnectionListener());

        /// <summary>
        /// Listen for incoming connections to the MCS TcpServer.
        /// </summary>
        public void ListenForConnections()
        {
            Log.I("Listening for connections...");
            while (MissionControlServer.Instance.AcceptConnections && MissionControlServer.Instance.TcpListener.Server.Connected)
            {
                TcpClient connection;
                try
                {
                    connection = MissionControlServer.Instance.TcpListener.AcceptTcpClient();
                    Log.I("Connection acquired: " + connection);
                }
                catch (SocketException e)
                {
                    Log.E(e, "SocketError occured.");
                    continue;
                }

                MissionControlServer.Instance.Clients.Add(connection);

                new Thread(() => HandleConnection(connection)).Start();
            }

            Log.I("No longer accepting connections. Closing server...");
            MissionControlServer.Instance.TcpListener.Stop();
        }

        /// <summary>
        /// Handle a TcpClient connection.
        /// </summary>
        /// <param name="client"></param>
        public void HandleConnection(TcpClient client)
        {
            NetworkStream clientStream = client.GetStream();
            UTF8Encoding encoder = new UTF8Encoding();
            byte[] message = new byte[ushort.MaxValue * 16];

            while (client.Connected)
            {
                int bytesRead;

                try
                {
                    bytesRead = clientStream.Read(message, 0, ushort.MaxValue * 16);
                }
                catch (Exception e)
                {
                    Log.E(e, "Error reading message.");
                    break;
                }

                if (bytesRead == 0)
                {
                    client.Close();
                }

                string cmd = encoder.GetString(message);
                string cmdRoot = cmd.Split(' ')[0];

                string[] cmdSplit = cmd.Split(' ');
                string[] args = new string[cmdSplit.Length - 1];
                for (int ai = 1; ai < cmdSplit.Length; ai++)
                {
                    args[ai] = cmdSplit[ai];
                }

                clientStream.Write(new[] { (byte) 6 }, 0, 1);

                MissionControlServer.Instance.CommandRegistry.GetMatchingCommand(cmdRoot)
                    .RunCommand(client, cmd, cmdRoot, args);

                message = new byte[ushort.MaxValue * 16];
            }
        }
    }
}
