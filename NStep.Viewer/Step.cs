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

        public Step(string name, DateTime timestamp, int durationMilli)
        {
            Name = name;
            Timestamp = timestamp;
            DurationMilli = durationMilli;
        }
    }
}