using System;

namespace MissionControl.Objects
{
    // ReSharper disable once InconsistentNaming
    class MCException : Exception
    {

        public readonly Exception UnderlyingException;
        private readonly string _message = "An unknown error occured.";
        public override string Message => _message;
        public readonly string Code = "MCX-UNKNOWN";

        public MCException(Exception underlyingException, string code, string message)
        {
            if (underlyingException != null)
                UnderlyingException = underlyingException;
            if (code != null)
                Code = code;
            if (message != null)
                _message = message;
        }

        public MCException(string code, string message)
        {
            if (code != null)
                Code = code;
            if (message != null)
                _message = message;
        }

        public override string ToString()
        {
            return "Code: " + Code + "\n" +
                   "Message: " + _message + "\n\n" +
                   (UnderlyingException == null ? "" : UnderlyingException.ToString());
        }
    }
}
