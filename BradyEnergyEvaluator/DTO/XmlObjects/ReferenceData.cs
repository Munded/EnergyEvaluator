using BradyEnergyEvaluator.DTO.Factors;
using System.Xml.Serialization;

namespace BradyEnergyEvaluator.DTO.XmlObjects
{
     [XmlRoot("ReferenceData")]
    public class ReferenceData
    {
        [XmlElement("Factors")]
        public Factors Factors {get;set;}
    }

    public class Factors
    {
        [XmlElement("ValueFactor")]
        public FactorValue ValueFactor { get; set; }
        [XmlElement("EmissionsFactor")]
        public FactorValue EmissionsFactor { get; set; }
    }
}
