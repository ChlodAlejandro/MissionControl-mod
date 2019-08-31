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

        public VesselBasic(Vessel reference)
        {
            VesselId = reference.id;
            VesselName = reference.GetDisplayName();
            VesselLoaded = reference.loaded;
        }

    }
}