using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BradyEnergyEvaluator.DTO.Output
{
    public class HeatRateOutput
    {
        [XmlElement("Name")]
        public string Name { get; set; }
        [XmlElement("HeatRate")]
        public decimal HeatRate { get; set; }
    }
}
