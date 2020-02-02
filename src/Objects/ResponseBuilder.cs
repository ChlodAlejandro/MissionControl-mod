using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MissionControl.Objects
{
    class ResponseBuilder
    {

        private enum ResponseType
        {
            Error,
            Normal
        }

        private static string GetPrimaryHeader(string code)
        {
            return Reference.MCS_HEADER + " " + code;
        }

        private static string GetBasicHeaders(ResponseType type, string response)
        {
            // ReSharper disable once StringLiteralTypo
            return "Timestamp: " + (Int64)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds + "\n" +
                   "KSP-BuildID: " + Versioning.BuildID + "\n" +
                   "Response-Type: " + type + "\n" +
                   "Response-Length: " + response.Length;
        }

        public static string Build(MCException e)
        {
            return GetPrimaryHeader(e.Code) + "\n" + GetBasicHeaders(ResponseType.Error, e.ToString()) + "\n\n" + e + Reference.MCS_ENDOFTEXT;
        }
        public static string Build(Exception exception)
        {
            MCException e = new MCException(exception, "MCS_UNKNOWN", "An unknown error has occurred.");
            return GetPrimaryHeader(e.Code) + "\n" + GetBasicHeaders(ResponseType.Error, e.ToString()) + "\n\n" + e + Reference.MCS_ENDOFTEXT;
        }
        public static string Build(string code, string content)
        {
            return GetPrimaryHeader(code) + "\n" + GetBasicHeaders(ResponseType.Normal, content) + "\n\n" + content + Reference.MCS_ENDOFTEXT;
        }

    }
}
