using System;
using System.Diagnostics;

namespace NStep.Instrumentation
{
    public class Step : IDisposable
    {
        private readonly string _name;
        private readonly DateTime _start;
        private readonly Stopwatch _stopwatch;
        private bool _stopped;

        public Step(string name)
        {
            _name = name;
            _start = DateTime.UtcNow;
            _stopwatch = Stopwatch.StartNew();
        }

        public void Dispose()
        {
            if(!_stopped)
            {
                _stopwatch.Stop();
                _stopped = true;
            }
        }

        public string Name { get { return _name; } }
        public DateTime Start { get { return _start; } }
        public long DurationMilli { get { return _stopwatch.ElapsedMilliseconds; } }

        public override string ToString()
        {
            return string.Format("{0:dd/MM/yyyy hh:mm:ss.ffff}\t{1}\t{2}", _start, DurationMilli, _name);
        }
    }
}