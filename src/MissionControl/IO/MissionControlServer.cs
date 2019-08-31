using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace MissionControl.IO
{
    /// <summary>
    /// The Mission Control Server.
    /// 
    /// <para>This class contains the code for starting and
    /// stopping the Mission Control server. The server
    /// handles the primary functions for handling the
    /// webhook connection.</para>
    /// </summary>
    public class MissionControlServer
    {
        /// <summary>
        /// Refers to this Mission Control server.
        /// </summary>
        private static MissionControlServer instance;

        /// <summary>
        /// The HttpListener for the plugin.
        /// </summary>
        public HttpListener HTTPListener;
        /// <summary>
        /// Defines if this server is accepting connections.
        /// </summary>
        public bool AcceptConnections = true;


        /// <summary>
        /// Returns the Mission Control Server attached.
        /// </summary>
        public static MissionControlServer Instance
        {
            get
            {
                return instance ?? (instance = new MissionControlServer());
            }
        }

        /// <summary>
        /// Attempt server start.
        /// </summary>
        /// <returns>error code</returns>
        public int Start()
        {
            if (!HttpListener.IsSupported)
                return 2;

            StartListener();
            return 0;
        }

        /// <summary>
        /// Start the connection listener.
        /// </summary>
        private void StartListener()
        {
            HTTPListener = new HttpListener();
            HTTPListener.Prefixes.Add("http://127.0.0.1:21818/");

            HTTPListener.Start();
            Log.I("Started the Mission Control Server.");
            new Thread(new ConnectionListener().ListenForConnections).Start();
        }
    }
}
