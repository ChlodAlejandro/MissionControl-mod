using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using MissionControl.Commands;
using MissionControl.Objects;

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
            while (MissionControlServer.Instance.AcceptConnections)
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

            while (client.Connected)
            {
                bool typing = true;
                int bytesRead = 0;
                string cmd = "";

                while (true)
                {
                    byte[] buffer = new byte[1];
                    try
                    {
                        bytesRead += clientStream.Read(buffer, 0, buffer.Length);
                    }
                    catch (Exception e)
                    {
                        Log.E(e, "Error reading message.");
                        break;
                    }

                    // DEBUG
                    Log.I(encoder.GetString(buffer));

                    if (bytesRead == 0)
                    {
                        client.Close();
                        return;
                    }
                    else if (encoder.GetString(buffer) == "\n")
                    {
                        if (cmd[cmd.Length - 1] == '\r')
                            cmd = cmd.Substring(0, cmd.Length - 1);
                        break;
                    }
                    else
                    {
                        cmd += encoder.GetString(buffer);
                    }
                }

                Log.I("Command: " + cmd
                      .Replace("\r", "{CR}")
                      .Replace("\n", "{LF}"));
                string cmdRoot = cmd.Split(' ')[0];

                string[] cmdSplit = cmd.Split(' ');
                string[] args = new string[cmdSplit.Length - 1];
                for (int ai = 1; ai < cmdSplit.Length; ai++)
                {
                    args[ai] = cmdSplit[ai];
                }

                clientStream.Write(new[] { (byte) Reference.MCS_ACKNOWLEDGE }, 0, 1);

                Command command = new UnknownCommand();
                try
                {
                    command = MissionControlServer.Instance.CommandRegistry.GetMatchingCommand(cmdRoot);
                }
                catch { /* ignored */ }

                string response;
                try
                {
                    response = ResponseBuilder.Build("MC_OK", command.RunCommand(client, cmd, cmdRoot, args));
                }
                catch (MCException ex)
                {
                    response = ResponseBuilder.Build(ex);
                }
                catch (Exception e)
                {
                    response = ResponseBuilder.Build(e);
                }

                byte[] responseBytes = encoder.GetBytes(response);

                clientStream.Write(responseBytes, 0, responseBytes.Length);
            }
        }
    }
}
