﻿using MissionControl.Configuration;
using MissionControl.IO;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MissionControl
{

    /// <summary>
    /// The Mission Control main class.
    /// 
    /// This class basic startup mechanics and other
    /// static variables.
    /// </summary>
    [KSPAddon(KSPAddon.Startup.Instantly, true)]
    class MissionControlAddon : MonoBehaviour
    {
        /// <summary>
        /// All the dialogs used by this class.
        /// 
        /// <para>Why are they not inline? I don't know. What I do know is that I don't to see gigantic blocks of code when coding.</para>
        /// </summary>
        private static readonly Dictionary<string, MultiOptionDialog> dialogs = new Dictionary<string, MultiOptionDialog>()
        {
            {
                "networkDenyConfirm", new MultiOptionDialog(
                    "mc-denyconfirm",
                    "If you deny Mission Control's ability to open a network port, you may not use the mod at all. Are you sure that you do not want to open up this port?",
                    "Mission Controller",
                    HighLogic.UISkin,
                        new DialogGUIBase[] {
                            new DialogGUIButton("Yes", () => {
                                ConfigManager.Instance.SetValue("networkPermitted", false);
                            }, true),
                        new DialogGUIButton("Return", () => {
                            PopupDialog.SpawnPopupDialog(dialogs["networkRequestPermission"], true, HighLogic.UISkin);
                        }, true)
                    }
                )
            },
            {
                "networkRequestPermission", new MultiOptionDialog(
                    "mc-permissionrequest",
                    "For Mission Control to work, it must open up a network port on your computer. Mission Control will lock this port to only work with your computer. If you wish to use this plugin and allow connections from the GUI, you must have this port enabled. Otherwise, this mod will not broadcast any data towards any telemetry receivers. This port will be located at " + ConfigManager.Instance.GetValue<string>("listenerHost") + ":" + ConfigManager.Instance.GetValue<ushort>("listenerPort") + "\n" +
                    "\n" +
                    "There may be security implications from opening a port with your computer. Although the Mission Control development team try to prevent exploitation of this port, be advised that they are not liable for any damage from that port.\n" +
                    "\n" +
                    "Do you allow Mission Control to open a local port?",
                    "Mission Controller",
                    HighLogic.UISkin,
                    new DialogGUIBase[] {
                        new DialogGUIButton("Agree", () => {
                            ConfigManager.Instance.SetValue("networkPermitted", true);
                        }, true),
                        new DialogGUIButton("Cancel", () => {
                            PopupDialog.SpawnPopupDialog(dialogs["networkDenyConfirm"], true, HighLogic.UISkin);
                        }, true)
                    }
                )
            }
        };

        /// <summary>
        /// Check if the player has allowed Mission Control to enable its network services.
        /// </summary>
        /// <returns>allowed to enable network service</returns>
        bool CheckForPermission()
        {
            return ConfigManager.Instance.GetValue<bool>("networkPermitted");
        }

        void Awake()
        {
            KSPLog.Instance.SetScreenLogging(true);
        }

        void Start()
        {
            // load configuration
            if (ConfigManager.Instance == null)
            {
                Log.E(new Exception("Configuration failed to load."));
                return;
            }

            // Required due to addon rules.
            // Read more: https://forum.kerbalspaceprogram.com/index.php?/topic/154851-add-on-posting-rules-november-24-2017/
            if (CheckForPermission())
            {
                Log.I("Permission granted. Starting server...");
                AttemptStart();
            }
            else
            {
                Log.I("Permission not granted. Requesting permission...");
                AskForPermission();
            }
        }

        /// <summary>
        /// Try to start the Mission Control Server.
        /// </summary>
        private void AttemptStart()
        {
            // if permission not given, die. (precaution)
            if (!CheckForPermission())
                return;

            int startStatus = MissionControlServer.Instance.Start();
            switch (startStatus)
            {
                case 0:
                    {
                        // success!
                        Log.I("Mission Control Endpoint Server started.");
                        break;
                    }

                // case 1 should be reserved for unknown errors.

                case 2:
                    {
                        // HttpListener is not supported.
                        PopupDialog.SpawnPopupDialog(dialogs["httpListenerUnsupported"], true, HighLogic.UISkin);
                        break;
                    }
                default:
                    {
                        // unknown error.
                        Log.E("An unknown error occured.");
                        break;
                    }
            }
        }

        /// <summary>
        /// Ask for permission from the user.
        /// </summary>
        private void AskForPermission()
        {
            PopupDialog dialog = PopupDialog.SpawnPopupDialog(dialogs["networkRequestPermission"], true, HighLogic.UISkin);

            dialog.OnDismiss = PostPermissionRequest;
        }

        /// <summary>
        /// Method to be run after the permission request has been fulfilled or not.
        /// </summary>
        private void PostPermissionRequest()
        {
            // if permission not given, die.
            if (!CheckForPermission())
            {
                Log.I("Permission still not granted. Standing down...");
                return;
            }

            Log.I("Permission granted. Starting server...");
            AttemptStart();
        }
    }
}
