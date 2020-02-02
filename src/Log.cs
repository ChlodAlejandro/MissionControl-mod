using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MissionControl
{
    public class Log
    {
        public static void I(object message)
        {
            Debug.Log("[MissionControl] " + message);
        }

        public static void W(object message)
        {
            Debug.LogWarning("[MissionControl] " + message);
        }

        public static void E(object message)
        {
            Debug.LogError("[MissionControl] " + message);
        }

        public static void E(Exception ex, string message)
        {
            E(message + " (" + ex.GetType().Name + ") " + ex.Message + ": " + ex.StackTrace);
        }

        public static void E(Exception ex)
        {
            E("(" + ex.GetType().Name + ") " + ex.Message + ": " + ex.StackTrace);
        }

        public static void Info(object message)
        {
            Debug.Log("[MissionControl] " + message);
        }

        public static void Warn(object message)
        {
            Debug.LogWarning("[MissionControl] " + message);
        }

        public static void Err(object message)
        {
            Debug.LogError("[MissionControl] " + message);
        }

        public static void Err(Exception ex, string message)
        {
            Err(message + " (" + ex.GetType().Name + ") " + ex.Message + ": " + ex.StackTrace);
        }

        public static void Err(Exception ex)
        {
            Err("(" + ex.GetType().Name + ") " + ex.Message + ": " + ex.StackTrace);
        }

        public static void Write(LogLevel level, object message)
        {
            switch (level)
            {
                case LogLevel.Info:
                    Debug.Log("[MissionControl] " + message);
                    break;
                case LogLevel.Warn:
                    Debug.LogWarning("[MissionControl] " + message);
                    break;
                case LogLevel.Error:
                    Debug.LogError("[MissionControl] " + message);
                    break;
            }
        }
    }

    public enum LogLevel
    {
        Info,
        Warn,
        Error
    }
}
