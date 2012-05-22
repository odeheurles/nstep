using System;

namespace NStep.Viewer
{
    public class Step
    {
        public string Name { get; private set; }
        public DateTime Timestamp { get; private set; }
        public int DurationMilli { get; private set; }
        public DateTime End {get { return Timestamp.AddMilliseconds(DurationMilli); }}
        public int Ranking { get; set; }
        public int Index { get; set; }
        public double Percent { get; set; }

        public Severity GetSeverity(int totalSteps)
        {
            if (Ranking < totalSteps / 3)
            {
                return Severity.High;
            }
            if (Ranking < 2 * totalSteps / 3)
            {
                return Severity.Medium;
            }
            return Severity.Low;
        }

        public Step(string name, DateTime timestamp, int durationMilli)
        {
            Name = name;
            Timestamp = timestamp;
            DurationMilli = durationMilli;
        }

        public override string ToString()
        {
            // [1 - 33%] Big Step (23,456ms)
            return string.Format("[{0} - {1:0.00}%] {2} ({3:#,##0}ms)", Ranking, Percent * 100, Name, DurationMilli);
        }
    }
}