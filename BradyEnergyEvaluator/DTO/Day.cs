using BradyEnergyEvaluator.DTO.Factors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BradyEnergyEvaluator.DTO
{
    public class Day
    {
        public DateTime Date { get; set; }
        public decimal Energy { get; set; }
        public decimal Price { get; set; }

        public decimal GetDailyGenerationValue(decimal valueFactor)
        {
            return Energy * Price * valueFactor;
        }

        public decimal GetDailyEmission(decimal emissionFactorValue, decimal emissionRating)
        {
            return Energy * emissionFactorValue * emissionRating;
        }
    }
}
