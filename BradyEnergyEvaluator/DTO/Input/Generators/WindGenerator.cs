using BradyEnergyEvaluator.DTO.Factors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BradyEnergyEvaluator.DTO.Generators
{
    public class WindGenerator : Generator
    {
        [XmlElement("Location")]
        public string LocationAsString
        {
            get { return Location.ToString(); }
            set
            {
                if (string.IsNullOrEmpty(value))
                    Location = default(Location);
                else
                    Location = (Location)Enum.Parse(typeof(Location), value);
            }
        }

        [XmlIgnore]
        public Location Location { get; set; }

        [XmlIgnore]
        public new FactorLevel ValueFactorLevel { get {
                if (Location == Location.Offshore)
                    return FactorLevel.Low;
                else if (Location == Location.Onshore)
                    return FactorLevel.High;
                else
                    return FactorLevel.NA;
            } 
        }
      
    }
}
