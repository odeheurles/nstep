using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace NStep.Viewer
{
    public class Report
    {
        public Report(IEnumerable<Step> steps)
        {
            var stepsByTimestamp = steps.OrderBy(s=>s.Timestamp).ToList();
            var stepsByDuration = stepsByTimestamp.OrderByDescending(s => s.DurationMilli).ToList();

            Start = stepsByTimestamp.Min(s => s.Timestamp);
            End = stepsByTimestamp.Max(s => s.End);
            TotalDurationMilli = (End - Start).TotalMilliseconds;

            for (int i = 0; i < stepsByTimestamp.Count; i++)
            {
                var step = stepsByTimestamp[i];
                var ranking = stepsByDuration.IndexOf(step);

                step.Ranking = ranking;
            }
        }

        public DateTime Start { get; private set; }
        public DateTime End { get; private set; }
        public double TotalDurationMilli { get; private set; }
    }

    public class ReportBuilded
    {
        public static Report LoadFromFile(string filePath)
        {
            var steps = from line in File.ReadAllLines(filePath)
                        where line.Trim() != string.Empty
                        let split = line.Split('\t')
                        let timestamp = DateTime.Parse(split[0].Trim(), CultureInfo.InvariantCulture)
                        let duration = int.Parse(split[1].Trim())
                        let name = split[2].Trim()
                        select new Step(name, timestamp, duration);

            
        } 
    }
}