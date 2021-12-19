using BradyEnergyEvaluator.DTO.Factors;
using BradyEnergyEvaluator.DTO.XmlObjects;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace BradyEnergyEvaluator
{
    class Program
    {

        static void Main(string[] args)
        {
            // get folder locations
            string inputFolder = ConfigurationManager.AppSettings["inputFolder"];
            string outputFolder = ConfigurationManager.AppSettings["outputFolder"];

            Console.WriteLine("Awaiting File Drop");

            var referenceData = GetReferenceData();
            FileSystemWatcher fileWatcher = new FileSystemWatcher(inputFolder, "*.*")
            {
                
                EnableRaisingEvents = true,
                IncludeSubdirectories = true,
                NotifyFilter = NotifyFilters.LastWrite
            };

            fileWatcher.Changed += (sender, eventArgs) => AnalyseReports(inputFolder, outputFolder, referenceData);

            //get all files in a folder
            //cAnalyseReports(inputFolder, outputFolder, referenceData);
            Console.ReadLine();
        }

        private static void AnalyseReports(string inputFolder, string outputFolder, ReferenceData referenceData)
        {
            try {
                IEnumerable<string> reportsToBeProcessed = Directory.GetFiles(inputFolder, "*.xml");

                Console.WriteLine("Files Found");

                foreach (string fileName in reportsToBeProcessed)
                {
                    GenerationReports report = getReport(fileName);

                    var output = ReportAnalyser.AnalyseReport(referenceData, report);

                    var writer = new XmlSerializer(typeof(GenerationOutput));
                    var inputFileName = Path.GetFileNameWithoutExtension(fileName);

                    var file = File.Create($"{outputFolder}\\{inputFileName}-result.xml");
                    writer.Serialize(file, output);
                    file.Close();

                    Console.WriteLine("Report Created");
                }
            } catch (IOException e)
            {
                // TO DO find a more graceful way to wait for file to finish copying
                Console.WriteLine("Awaiting File to become unlocked");
                // TO DO Readline stops the recursive loop of constant accessing and failure
               // Console.ReadLine();
                AnalyseReports(inputFolder, outputFolder, referenceData);

            }
        }

        private static GenerationReports getReport(string fileName)
        {
            var inputSerialiser = new XmlSerializer(typeof(GenerationReports));
            var inputFileStream = new FileStream(fileName, FileMode.Open);
            var report = (GenerationReports)inputSerialiser.Deserialize(inputFileStream);
            return report;
        }

        private static  ReferenceData GetReferenceData()
        {
            //Get reference data xml
            string referenceFolder = ConfigurationManager.AppSettings["referenceFolder"];
            string referanceFilePath = Path.GetFullPath(referenceFolder + "\\ReferenceData.xml");

            //deserialise reference xml
            var referenceSerialiser = new XmlSerializer(typeof(ReferenceData));
            var referenceFileStream = new FileStream(referanceFilePath, FileMode.Open);
            var referenceData = (ReferenceData)referenceSerialiser.Deserialize(referenceFileStream);

            return referenceData;
        }

        
    }
}
