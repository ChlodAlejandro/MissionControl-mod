using System;
using System.Reflection;
using UnityEngine;

namespace MissionControlCommon.Objects
{
    [Serializable]
    public class VesselBasic
    {
        
        public Guid VesselId;
        public string VesselName;
        public bool VesselLoaded;
        public VesselType VesselType;
        public Vessel.Situations VesselSituation;

        public VesselBasic(Vessel reference)
        {
            VesselId = reference.id;
            VesselName = reference.GetDisplayName();
            VesselLoaded = reference.loaded;
            VesselType = reference.vesselType;
            VesselSituation = reference.situation;
        }

        public override string ToString()
        {
            return VesselId + " {\n" +
                   "\tName=" + VesselName + "\n" +
                   "\tLoaded=" + VesselLoaded + "\n" +
                   "\tType=" + VesselType.ToString().Substring(11) + "\n" +
                   "\tSituation=" + VesselSituation.ToString().Substring(16) + "\n" +
                   "}";
        }
    }
}