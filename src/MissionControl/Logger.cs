using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MissionControl
{
    class Logger
    {

        public static void Log(object message)
        {
            Debug.Log("[MissionControl] " + message);
        }

        public static void Warn(object message)
        {
            Debug.LogWarning("[MissionControl] " + message);
        }

        public static void Error(object message)
        {
            Debug.LogError("[MissionControl] " + message);
        }

        public static void Exception(string message, Exception e)
        {
            Error(message + " (" + e.GetType().Name + ") " + e.Message + ": " + e.StackTrace);
        }

        public static void Exception(Exception e)
        {
            Error("(" + e.GetType().Name + ") " + e.Message + ": " + e.StackTrace);
        }

    }
}
