using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MissionControlCommon.Objects
{
    [Serializable]
    public class ErrorResponse
    {

        public bool Erred;
        public string Error;
        public string Description;
        public int Code;

        public ErrorResponse(bool erred = false)
        {
            Erred = erred;
            
            if (erred)
            {
                Error = "Internal Error Occured";
                Description = "An internal error occured while processing your request.";
                Code = 500;
            }
        }

        public ErrorResponse(int code, string shortName, string description)
        {
            Erred = true;
            Code = code;
            Error = shortName;
            Description = description;
        }

    }
}
