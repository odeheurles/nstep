using System;
using System.Globalization;
using System.IO;
using System.Linq;

namespace NStep.Viewer
{
    public class ReportBuilder
    {
        public static Report LoadFromFile(string filePath)
        {
            var steps = from line in File.ReadAllLines(filePath)
                        where line.Trim() != string.Empty
                        let split = line.Split('\t')
                        let timestamp = DateTime.Parse(split[0].Trim(), new CultureInfo("en-GB"))
                        let duration = int.Parse(split[1].Trim())
                        let name = split[2].Trim()
                        select new Step(name, timestamp, duration);

            return new Report(steps);
        } 
    }
}