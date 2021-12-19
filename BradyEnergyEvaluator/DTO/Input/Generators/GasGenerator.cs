using BradyEnergyEvaluator.DTO.Factors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BradyEnergyEvaluator.DTO.Generators
{
    public class GasGenerator : Generator
    { 
        [XmlIgnore]
        public new FactorLevel EmissionFactorLevel = FactorLevel.Medium;

        [XmlIgnore]
        public new FactorLevel ValueFactorLevel = FactorLevel.Medium;
    }
}
