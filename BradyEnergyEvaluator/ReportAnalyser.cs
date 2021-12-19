using BradyEnergyEvaluator.DTO;
using BradyEnergyEvaluator.DTO.Factors;
using BradyEnergyEvaluator.DTO.Generators;
using BradyEnergyEvaluator.DTO.Output;
using BradyEnergyEvaluator.DTO.XmlObjects;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BradyEnergyEvaluator
{
    public class ReportAnalyser
    {
        public static GenerationOutput AnalyseReport(ReferenceData referenceData, GenerationReports report)
        {
            //Get all Generator Outputs
            List<GeneratorOutput> totals = GetAllGeneratorOutputTotals(referenceData, report).ToList();


            //Get all Emission Generators
            var emissionFactor = referenceData.Factors.EmissionsFactor;
            List<DayOutput> maxEmissions = GetMaxEmissionsByDay(report, emissionFactor);

            // Get all Actual HeatRates
            var ActualHeatRates = report.Coal.Select(coalGenerator =>
            {
                return new HeatRateOutput()
                {
                    Name = coalGenerator.Name,
                    HeatRate = coalGenerator.GetActualHeatRate()
                };
            }).ToList();

            return new GenerationOutput() { Totals = totals, ActualHeatRates = ActualHeatRates, MaxEmissionGenerators = maxEmissions.ToList() };
        }

        private static List<DayOutput> GetMaxEmissionsByDay(GenerationReports report, FactorValue emissionFactor)
        {// I should have come up with shared interfaces instead of the base class to avoid repetition
            var CoalByDay = report.Coal.SelectMany(generator =>
            {
                var emissionFactorValue = emissionFactor.GetFactorValue(generator.ValueFactorLevel);
                return generator.Generation.Select(g => new DayOutput()
                {
                    Name = generator.Name,
                    Date = g.Date,
                    Emission = g.GetDailyEmission(emissionFactorValue, generator.EmissionsRating)
                });
            });

            var gasByDay = report.Gas.SelectMany(c =>
            {
                var emissionFactorValue = emissionFactor.GetFactorValue(c.ValueFactorLevel);
                return c.Generation.Select(generator => new DayOutput()
                {
                    Name = c.Name,
                    Date = generator.Date,
                    Emission = generator.GetDailyEmission(emissionFactorValue, c.EmissionsRating)
                });
            });
            var dayOutputs = CoalByDay.Concat(gasByDay);

            var groups = from r in dayOutputs
                         group r by r.Date into dateGroup
                         select new
                         {
                             key = dateGroup.Key,
                             count = dateGroup.Count(),
                             dayOutput = dateGroup.MaxBy(dg => dg.Emission)
                         };

            var test = groups.SelectMany(x => x.dayOutput).ToList();
            return test;
        }
        
        //generator generic defaults the emission factor value to 0 
        //private static IEnumerable<DayOutput> GetProcessedDayOutput<TGenerator>(TGenerator c, FactorValue emissionFactor) where TGenerator : Generator
        //{
        //    var emissionFactorValue = emissionFactor.GetFactorValue(c.ValueFactorLevel);
        //    return c.Generation.Select(g => new DayOutput()
        //    {
        //        Name = c.Name,
        //        Date = g.Date,
        //        Emission = g.GetDailyEmission(emissionFactorValue, c.EmissionRating)
        //    });
        //}

        private static IEnumerable<GeneratorOutput> GetAllGeneratorOutputTotals(ReferenceData referenceData, GenerationReports report)
        {
            var valueFactor = referenceData.Factors.ValueFactor;
            // would prefer to do this in on select but unfortunately i can't get concat to work on the shared type
            // and that will hide the different values
            var windTotals = report.Wind.Select(generator =>
            {
                var factorValue = valueFactor.GetFactorValue(generator.ValueFactorLevel);
                return generator.ToGeneratorOutput(factorValue);
            });

            var gasTotals = report.Gas.Select(generator =>
            {
                var factorValue = valueFactor.GetFactorValue(generator.ValueFactorLevel);
                return generator.ToGeneratorOutput(factorValue);
            });

            var coalTotals = report.Coal.Select(generator =>
            {
                var factorValue = valueFactor.GetFactorValue(generator.ValueFactorLevel);
                return generator.ToGeneratorOutput(factorValue);
            });

            return windTotals.Concat(gasTotals).Concat(coalTotals);
        }

        //generator generic defaults the factor value to 0 
        //private static GeneratorOutput GetGeneratorOutput<TGenerator>(TGenerator generator, FactorValue valueFactor) where TGenerator : Generator
        //{
        //    var factorValue = valueFactor.GetFactorValue(generator.ValueFactorLevel);
        //    return generator.ToGeneratorOutput(factorValue);
        //}
    }
}
