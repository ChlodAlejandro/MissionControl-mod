using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using MissionControl.Objects;
using MissionControlCommon.Objects;
using UnityEngine.SceneManagement;
using UnityEngine.Windows.WebCam;

namespace MissionControl.Commands
{
    public class VesselsCommand : Command
    {
        public override string[] GetTriggers()
        {
            return new[] {"VESSELS", "V"};
        }

        public override string RunCommand(TcpClient sender, string command, string trigger, string[] arguments)
        {
            string match = null;
            string type;

            for (int i = 0; i < arguments.Length; i++)
            {
                var arg = arguments[i];
                if (arg.StartsWith("regex="))
                {
                    if (arg[6] == '"')
                    {
                        bool ended = false;
                        string toDigest = arg.Substring(6);
                        string totalMatch = "";

                        while (!ended)
                        {
                            if (toDigest[1] == '"' && toDigest[0] != '\\')
                            {
                                totalMatch += toDigest[0];
                                ended = true;
                                break;
                            }
                            else
                            {
                                totalMatch += toDigest[0];
                            }

                            toDigest = toDigest.Length == 1 ? "" : toDigest.Substring(1);
                            if (toDigest.Length != 0) continue;
                            if (i >= arguments.Length - 1)
                                ended = true;
                            else
                            {
                                i++;
                                toDigest += arguments[i];
                            }
                        }

                        match = totalMatch;
                    }
                    else
                    {
                        match = arg.Substring(6);
                    }
                }
                else if (arg.StartsWith("type="))
                {
                    bool converted = Enum.TryParse(arg.Substring(5), out VesselType eType);
                    if (converted)
                        type = eType.ToString();
                    else
                    {
                        throw new ArgumentException("Craft type is not valid.");
                    }
                }
            }

            if (HighLogic.LoadedSceneIsGame)
            {
                string vb = "";
                foreach (Vessel v in FlightGlobals.Vessels)
                {
                    vb += new VesselBasic(v) + "\n";
                }

                return vb;
            }
            else
            {
                throw new MCException("MCS_NOTLOADED", "The command requires a loaded save, but the game has not loaded any save file yet.");
            }
        }
    }
}
