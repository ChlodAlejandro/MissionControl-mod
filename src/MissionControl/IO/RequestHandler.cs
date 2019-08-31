using MissionControl.Endpoints;
using MissionControlCommon.Objects;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using UnityEngine;

namespace MissionControl.IO
{
    class RequestHandler
    {

        /// <summary>
        /// The context of the request.
        /// </summary>
        private HttpListenerContext context;

        /// <summary>
        /// Create a new RequestHandler to process a request.
        /// </summary>
        /// <param name="ctxt">The context of the request.</param>
        public RequestHandler(HttpListenerContext ctxt)
        {
            context = ctxt;
        }

        /// <summary>
        /// Process a request sent by an endpoint.
        /// </summary>
        public void ProcessRequest()
        {
            Log.I("Processing request...");
            try
            {
                // The time that processing started
                DateTime start = DateTime.Now;

                // The method used (e.g. /ships or /vessel)
                string method = context.Request.RawUrl.Substring(1);

                // The response status code
                context.Response.StatusCode = 500;
                
                // for debugging
                Log.I("Processing request to '" + method + "' from ''");

                // The response data (500, by default)
                Response data = Response.BuildError(new ErrorResponse(true));

                // Attempt to get data
                try
                {
                    // Get the endpoint to use from the endpoint manager
                    Endpoint endpoint = EndpointManager.Instance.GetEndpoint(method);

                    // Check if the endpoint has been registered
                    if (endpoint == null)
                    {
                        // Send back a 404
                        data = Response.BuildError(new ErrorResponse(404, "Not Found", "This method is not registered to any endpoint."));
                    }
                    else
                    {
                        // Get the response from the endpoint
                        data = endpoint.Process(method, context);
                    }
                }
                catch (Exception e)
                {
                    Log.E(e);
                }

                WriteReponse(data, ref context);

                // for debugging
                Log.I("Request to '" + method + "' processed. (" + DateTime.Compare(start, DateTime.Now) + "ms)");
            }
            catch (Exception e)
            {
                Log.E(e);
            }
        }

        /// <summary>
        /// Send a response to the listener.
        /// </summary>
        /// <param name="data">The data to send the listener.</param>
        /// <param name="ctxt">The context of the request.</param>
        void WriteReponse(Response data, ref HttpListenerContext ctxt)
        {
            Log.I("Writing response...");

            // Encode the data
            byte[] b = Encoding.UTF8.GetBytes(JsonUtility.ToJson(data.Content));

            Log.I("Response: " + JsonUtility.ToJson(data.Content));

            // Clear response headers
            ctxt.Response.Headers.Clear();

            // Send chunked response?
            ctxt.Response.SendChunked = false;

            // Set the status code
            ctxt.Response.StatusCode = (data.Error.Erred ? data.Error.Code : data.Code) ?? 500;

            // Set the content type (JSON)
            ctxt.Response.ContentType = "application/json";

            // Set the content length
            ctxt.Response.ContentLength64 = b.Length;

            // Send the response
            ctxt.Response.OutputStream.Write(b, 0, b.Length);

            // Close the connection
            ctxt.Response.OutputStream.Close();
        }

    }
}
