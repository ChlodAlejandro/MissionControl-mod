using System;
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

        public XmlDocument ToXDocument()
        {
            XmlDocument xmlDocument = new XmlDocument();
            using (XmlWriter xmlWriter = xmlDocument.CreateNavigator().AppendChild())
                _xmlSerializer.Serialize(xmlWriter,
                    this);
            return xmlDocument;
        }

        public VesselBasic FromXElement(XmlDocument xmlDocument)
        {
            return _xmlSerializer.Deserialize(xmlDocument.CreateNavigator().ReadSubtree()) as VesselBasic;
        }

        public override string ToString()
        {
            return ToXDocument().ToString();
        }
    }
}