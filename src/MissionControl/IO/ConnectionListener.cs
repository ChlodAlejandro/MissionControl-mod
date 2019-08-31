using MissionControl.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace MissionControl
{
    class ConnectionListener
    {
        public void ListenForConnections()
        {
            Log.I("Listening for connections...");
            while (MissionControlServer.Instance.AcceptConnections)
            {
                IAsyncResult result = MissionControlServer.Instance.HTTPListener.BeginGetContext((IAsyncResult requestResult) =>
                {
                    Log.I("Connection received...");

                    HttpListener listener = (HttpListener) requestResult.AsyncState;
                    HttpListenerContext context = listener.EndGetContext(requestResult);

                    Log.I($"Responding to connection from '{context.User.Identity.Name}'...");
                    (new RequestHandler(context)).ProcessRequest();
                }, MissionControlServer.Instance.HTTPListener);

                Log.I("Waiting for a request...");
                result.AsyncWaitHandle.WaitOne();
                Log.I("Request processed. Awaiting next request...");
            }

            Log.I("No longer accepting connections. Closing server...");
            MissionControlServer.Instance.HTTPListener.Close();
        }

    }
}
