using BradyEnergyEvaluator.DTO.Factors;
using System.Xml.Serialization;

namespace BradyEnergyEvaluator.DTO.Generators
{
    public class CoalGenerator : Generator
    {
        [XmlElement("TotalHeatInput")]
        public decimal TotalHeatInput { get; set; }

        [XmlElement("ActualNetGeneration")]
        public decimal ActualNetGeneration { get; set; }

        [XmlIgnore]
        public new FactorLevel EmissionFactorLevel = FactorLevel.High;
        [XmlIgnore]
        public new FactorLevel ValueFactorLevel = FactorLevel.Medium;

        public decimal GetActualHeatRate() => TotalHeatInput / ActualNetGeneration;
    }
}
