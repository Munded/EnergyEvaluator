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
            List<string> processedReports = new List<string>();

            Console.WriteLine("Awaiting File Drop");

            var referenceData = GetReferenceData();
            FileSystemWatcher fileWatcher = new FileSystemWatcher(inputFolder, "*.*")
            {
                
                EnableRaisingEvents = true,
                IncludeSubdirectories = true,
                NotifyFilter = NotifyFilters.LastWrite
            };

            // File watcher is flawed and throws multiple events on file changes, a better tool decision would have been better
            fileWatcher.Changed += (sender, eventArgs) => AnalyseReports(inputFolder, outputFolder, referenceData, processedReports);

            Console.ReadLine();
        }



        private static void AnalyseReports(string inputFolder, string outputFolder, ReferenceData referenceData, List<string> processedReports)
        {
            IEnumerable<string> reportsToBeProcessed = Directory.GetFiles(inputFolder, "*.xml");

            foreach  (string fileName in reportsToBeProcessed)
            {
                // Due to file watcher throwing multiple events for a transaction, need to check the file has not already been changed in session
                if (!processedReports.Contains(fileName))
                {
                    while (true)
                    {
                        // To avoid locking of file, need to check that file is free
                        if (!IsFileLocked(fileName))
                        {
                            AnalyseReport(outputFolder, referenceData, fileName);
                            processedReports.Add(fileName);
                            break;
                        };
                    }
                }
            }
        }

        private static bool IsFileLocked(string filePath)
        {
            var ret = false;
            try
            {
                using (File.Open(filePath, FileMode.Open)) { }
            }
            catch (IOException e)
            {
            }
            return ret;
        }

        private static void AnalyseReport(string outputFolder, ReferenceData referenceData, string fileName)
        {
            GenerationReports report = getReport(fileName);

            var output = ReportAnalyser.AnalyseReport(referenceData, report);

            var writer = new XmlSerializer(typeof(GenerationOutput));
            var inputFileName = Path.GetFileNameWithoutExtension(fileName);

            var file = File.Create($"{outputFolder}\\{inputFileName}-result.xml");
            writer.Serialize(file, output);
            file.Close();

            Console.WriteLine($"Report Created - {inputFileName}-result.xml");
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
