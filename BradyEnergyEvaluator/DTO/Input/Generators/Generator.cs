using BradyEnergyEvaluator.DTO.Factors;
using BradyEnergyEvaluator.DTO.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BradyEnergyEvaluator.DTO.Generators
{
    public abstract class Generator
    {
        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlArray("Generation")]
        [XmlArrayItem("Day")]
        public List<Day> Generation { get; set; }

        [XmlElement("EmissionsRating")]
        public decimal EmissionsRating { get; set; }

        [XmlIgnore]
        public FactorLevel EmissionFactorLevel = FactorLevel.NA;
        [XmlIgnore]
        public FactorLevel ValueFactorLevel = FactorLevel.NA;

        public GeneratorOutput ToGeneratorOutput(decimal valueFactor)
        {
            var total = Generation.Sum(g => g.GetDailyGenerationValue(valueFactor));
            return new GeneratorOutput()
            {
                Name = Name,
                Total = total
            };
        }



    }
}
