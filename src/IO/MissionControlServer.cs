using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using MissionControl.Commands;
using MissionControl.Configuration;
using Smooth.Collections;

namespace MissionControl.IO
{
    /// <summary>
    /// The Mission Control Server.
    /// 
    /// <para>This class contains the code for starting and
    /// stopping the Mission Control server. The server
    /// handles the primary functions for handling the
    /// TCP server connection.</para>
    /// </summary>
    public class MissionControlServer
    {
        /// <summary>
        /// Refers to this Mission Control server.
        /// </summary>
        private static MissionControlServer _instance;

        /// <summary>
        /// The TcpListener for the plugin.
        /// </summary>
        public TcpListener TcpListener;
        /// <summary>
        /// Defines if this server is accepting connections.
        /// </summary>
        public bool AcceptConnections = true;
        /// <summary>
        /// A list of all MCS connections.
        /// </summary>
        public List<TcpClient> Clients = new List<TcpClient>();
        /// <summary>
        /// The command registry to be used for client commands.
        /// </summary>
        public CommandRegistry CommandRegistry = new CommandRegistry();


        /// <summary>
        /// Returns the Mission Control Server attached.
        /// </summary>
        public static MissionControlServer Instance => _instance ?? (_instance = new MissionControlServer());

        /// <summary>
        /// Attempt server start.
        /// </summary>
        /// <returns>error code</returns>
        public int Start()
        {
            StartListener();
            return 0;
        }

        /// <summary>
        /// Start the connection listener.
        /// </summary>
        private void StartListener()
        {
            TcpListener = new TcpListener(
                IPAddress.Parse(ConfigManager.Instance.GetValue<string>("listenerHost")),
                ConfigManager.Instance.GetValue<ushort>("listenerPort"));
            TcpListener.Start();

            Log.I("Started the Mission Control Server.");
            new Thread(ConnectionListener.Instance.ListenForConnections).Start();
        }
    }
}
