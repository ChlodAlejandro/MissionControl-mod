using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using MissionControlCommon.Objects;

namespace MissionControl.Endpoints.Base
{
    /// <summary>
    /// Endpoint that returns all vessels.
    /// </summary>
    public class Vessels : Endpoint
    {
        public override Response Process(string endpointName, HttpListenerContext context)
        {
            if (HighLogic.CurrentGame == null)
                return Response.BuildError(new ErrorResponse(405, "Unloaded", "There is no game currently loaded."));

            // check arguments
            bool respectCommNet = context.Request.QueryString["respectCommNet"] != null ? context.Request.QueryString["respectCommNet"] == "true" : true;

            // create serializable VesselBasic out of KSP Vessel objects
            List<VesselBasic> vessels = new List<VesselBasic>();
            foreach(Vessel vessel in FlightGlobals.Vessels)
            {
                if (respectCommNet && !vessel.Connection.CanComm)
                    continue;
                vessels.Add(new VesselBasic(vessel));
            }

            return Response.BuildResponse(200, vessels.ToArray());
        }
    }
}
