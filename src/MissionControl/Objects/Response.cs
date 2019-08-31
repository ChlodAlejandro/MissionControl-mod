using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MissionControlCommon.Objects
{
    [Serializable]
    public class Response
    {

        public ErrorResponse Error;
        public int? Code;
        public object Content;

        private Response(ErrorResponse error, int? responseCode, object body)
        {
            Error = error ?? new ErrorResponse();

            if (responseCode != null || body != null)
            {
                Code = responseCode;
                Content = body;
            }
            else if (error.Erred)
            {
                Code = null;
                Content = null;
            }
            else
            {
                Code = null;
                Content = null;
            }
        }

        public static Response BuildError(ErrorResponse error)
        {
            return new Response(error, null, null);
        }

        public static Response BuildResponse(int responseCode, object responseBody)
        {
            return new Response(null, responseCode, responseBody);
        }

    }
}
