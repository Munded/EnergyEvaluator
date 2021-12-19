using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BradyEnergyEvaluator.DTO.Output
{
    public class DayOutput
    {
        [XmlElement("Name")]
        public string Name { get; set; }
        [XmlElement("Date")]
        public DateTime Date { get; set; }

        [XmlElement("Emission")]
        public decimal Emission { get; set; }
    }
}
