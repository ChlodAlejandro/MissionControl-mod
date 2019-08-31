using MissionControlCommon.Objects;
using System.Net;

namespace MissionControl.Endpoints
{
    public abstract class Endpoint
    {

        public abstract Response Process(string endpointName, HttpListenerContext context);

    }
}
