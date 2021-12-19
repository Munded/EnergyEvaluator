using BradyEnergyEvaluator.DTO.Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BradyEnergyEvaluator.DTO.XmlObjects
{
    [XmlRoot("GenerationReport")]
    public class GenerationReports
    {
        [XmlArray("Wind")]
        [XmlArrayItem("WindGenerator")]
        public List<WindGenerator> Wind { get; set; }
        [XmlArray("Gas")]
        [XmlArrayItem("GasGenerator")]
        public List<GasGenerator> Gas { get; set; }
        [XmlArray("Coal")]
        [XmlArrayItem("CoalGenerator")]
        public List<CoalGenerator> Coal { get; set; }

    }
}
