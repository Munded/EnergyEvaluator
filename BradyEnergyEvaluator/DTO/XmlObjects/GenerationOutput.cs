using BradyEnergyEvaluator.DTO.Output;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BradyEnergyEvaluator.DTO.XmlObjects
{
    [XmlRoot("GenerationOutput")]
    public class GenerationOutput
    {
        [XmlArray("Totals")]
        [XmlArrayItem("Generator")]
        public List<GeneratorOutput> Totals { get; set; }

        [XmlArray("MaxEmissionGenerators")]
        [XmlArrayItem("Day")]
        public List<DayOutput> MaxEmissionGenerators { get; set; }

        [XmlArray("ActualHeatRates")]
        [XmlArrayItem("ActualHeatRate")]
        public List<HeatRateOutput> ActualHeatRates { get; set; }
    }
}
