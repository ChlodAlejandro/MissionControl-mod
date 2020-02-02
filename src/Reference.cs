using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MissionControl
{
    public static class Reference
    {

        public static readonly string MCS_VERSION = "0.0.1";
        public static readonly string MCS_HEADER = "MCS/" + MCS_VERSION;


        public static readonly char MCS_COMMANDEND = '\n';
        public static readonly char MCS_ACKNOWLEDGE = (char) 6;
        public static readonly char MCS_ENDOFTEXT = (char) 4;

    }
}
