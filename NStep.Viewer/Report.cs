using System;
using System.Collections.Generic;
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
            StepsCount = stepsByTimestamp.Count;

            for (int i = 0; i < stepsByTimestamp.Count; i++)
            {
                var step = stepsByTimestamp[i];
                var ranking = stepsByDuration.IndexOf(step);

                step.Ranking = ranking;
                step.Index = i;
                step.Percent = step.DurationMilli/TotalDurationMilli;
            }

            Steps = stepsByTimestamp;
        }

        public IEnumerable<Step> Steps { get; private set; }
        public DateTime Start { get; private set; }
        public DateTime End { get; private set; }
        public double TotalDurationMilli { get; private set; }
        public int StepsCount { get; private set; }
    }
}