using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace MissionControl.Objects
{
    [Serializable]
    public class VesselBasic
    {
        private XmlSerializer _xmlSerializer = new XmlSerializer(typeof(VesselBasic));
        public Guid VesselId;
        public string VesselName;
        public bool VesselLoaded;
        public VesselType VesselType;
        public Vessel.Situations VesselSituation;

        public VesselBasic() {}

        public VesselBasic(Vessel reference)
        {
            VesselId = reference.id;
            VesselName = reference.GetDisplayName();
            VesselLoaded = reference.loaded;
            VesselType = reference.vesselType;
            VesselSituation = reference.situation;
        }

        public string ToXML()
        {
            string xmlString;
            using (var stringWriter = new StringWriter())
            using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter))
            {
                new XmlSerializer(typeof(VesselBasic)).Serialize(xmlWriter, this);
                xmlWriter.Flush();
                xmlString = stringWriter.GetStringBuilder().ToString();
            }

            return xmlString;
        }


        public override string ToString()
        {
            return ToXML();
        }
    }
}