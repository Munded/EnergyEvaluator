using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BradyEnergyEvaluator.DTO.Factors
{
    public class FactorValue
    {
        [XmlElement("High")]
        public decimal High { get; set; }
        [XmlElement("Medium")]
        public decimal Medium { get; set; }
        [XmlElement("Low")]
        public decimal Low { get; set; }

        public decimal GetFactorValue(FactorLevel factor) {
            decimal value;

            switch (factor)
            {
                case FactorLevel.High:
                    value = High;
                    break;
                case FactorLevel.Medium:
                    value = Medium;
                    break;
                case FactorLevel.Low:
                    value = Low;
                    break;
                default:
                    value = 0;
                    break;
            }

            return value;
        }
    }
}
