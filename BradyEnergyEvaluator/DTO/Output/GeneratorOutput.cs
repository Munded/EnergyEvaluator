using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BradyEnergyEvaluator.DTO.Output
{
    public class GeneratorOutput
    {
        [XmlElement("Name")]
        public string Name { get; set; }
        [XmlElement("Total")]
        public decimal Total { get; set; }
    }
}
