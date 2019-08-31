using MissionControl.Endpoints.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MissionControl.Endpoints
{
    /// <summary>
    /// <para>This class manages all methods and their respective endpoints.</para>
    /// <para>It is also responsible for handling custom converters for the RequestHandler's JSON converter.</para>
    /// </summary>
    public class EndpointManager
    {

        /// <summary>
        /// This instance of the Endpoint Manager.
        /// </summary>
        private static EndpointManager instance;
        /// <summary>
        /// All of the registered endpoints for the Mission Control server.
        /// </summary>
        private Dictionary<string, Endpoint> Endpoints = new Dictionary<string, Endpoint>();

        /// <summary>
        /// Get this instance of the Endpoint Manager.
        /// </summary>
        public static EndpointManager Instance
        {
            get
            {
                return instance ?? (instance = new EndpointManager());
            }
        }

        /// <summary>
        /// Create a new Endpoint Manager
        /// 
        /// <para>WARNING: Do not create more than one endpoint manager unless you know what you're doing! Use EndpointManager.Instance instead.</para>
        /// </summary>
        public EndpointManager()
        {
            RegisterBaseEndpoints();
        }

        /// <summary>
        /// Register an endpoint with this endpoint manager.
        /// </summary>
        /// <param name="methodName">The method name of the endpoint. This is subdirectory the user must visit.</param>
        /// <param name="endpoint">The endpoint object that the method points to.</param>
        /// <param name="overwritePrevious">Should this overwrite any previous entry with the same method name?</param>
        public void RegisterEndpoint(string methodName, Endpoint endpoint, bool overwritePrevious = false)
        {
            if (EndpointRegistered(methodName))
            {
                if (!overwritePrevious)
                    Endpoints.Remove(methodName);
                else
                    throw new ArgumentException("This endpoint is already registered.");
            }
            else
                Endpoints.Add(methodName, endpoint);
        }

        /// <summary>
        /// Deregister an endpoint using its method.
        /// 
        /// <para>Grammar note: The verb is "deregister", and the adjective is "unregistered."</para>
        /// </summary>
        /// <param name="methodName">The method name of the endpoint to be deregistered.</param>
        public void DeregisterEndpoint(string methodName)
        {
            if (EndpointRegistered(methodName))
                Endpoints.Remove(methodName);
            else
                Log.W("Tried to deregister an unregistered endpoint ('" + methodName + "')");
        }

        /// <summary>
        /// Deregister all methods that point to the given endpoint.
        /// 
        /// <para>Grammar note: The verb is "deregister", and the adjective is "unregistered."</para>
        /// </summary>
        /// <param name="endpoint">The endpoint to completely remove.</param>
        public void DeregisterEndpoints(Endpoint endpoint)
        {
            if (EndpointRegistered(endpoint))
            {
                foreach (string method in Endpoints.Keys)
                {
                    if (Endpoints[method] == endpoint)
                        Endpoints.Remove(method);
                }
            }
            else
                Log.W("Tried to deregister an unregistered endpoint.");
        }

        /// <summary>
        /// Check if a method name has been registered.
        /// </summary>
        /// <param name="methodName">The method name to check.</param>
        /// <returns></returns>
        public bool EndpointRegistered(string methodName)
        {
            return Endpoints.ContainsKey(methodName);
        }

        /// <summary>
        /// Check if an endpoint has been registered.
        /// </summary>
        /// <param name="endpoint">The endpoint to check.</param>
        /// <returns></returns>
        public bool EndpointRegistered(Endpoint endpoint)
        {
            return Endpoints.ContainsValue(endpoint);
        }

        /// <summary>
        /// Get an endpoint from the given method name.
        /// </summary>
        /// <param name="methodName">The method name of the endpoint.</param>
        /// <param name="throwIfMissing">Should an exception be thrown if the endpoint does not exist?</param>
        /// <returns></returns>
        public Endpoint GetEndpoint(string methodName, bool throwIfMissing = false)
        {
            if (EndpointRegistered(methodName))
                return Endpoints[methodName];
            else
            {
                if (throwIfMissing)
                    throw new ArgumentException("The method given does not have an associated endpoint.");
                else
                {
                    Log.W("An attempt was made to get an unregistered endpoint ('" + methodName + "')");
                    return null;
                }
            }
        }

        /// <summary>
        /// Get all method names that point to the given endpoint.
        /// </summary>
        /// <param name="endpoint">The endpoint to check.</param>
        /// <returns></returns>
        public string[] GetMethods(Endpoint endpoint)
        {
            List<string> methods = new List<string>();
            foreach(string method in Endpoints.Keys)
            {
                if (Endpoints[method] == endpoint)
                    methods.Add(method);
            }
            return methods.ToArray();
        }

        /// <summary>
        /// Register Mission Control's base endpoints.
        /// </summary>
        private void RegisterBaseEndpoints()
        {
            RegisterEndpoint("vessels", new Vessels());
        }
    }
}
