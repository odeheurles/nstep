using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;

namespace NStep.Instrumentation
{
    public class InstrumentationContext : IInstrumentationContext
    {
        private readonly ConcurrentDictionary<string, Step> _steps = new ConcurrentDictionary<string, Step>();

        public IDisposable StartStep(string name)
        {
            var step = new Step(name);

            _steps.AddOrUpdate(name, _ => step, (_, __) => step);

            return step;
        }

        public void StopStep(string name)
        {
            Step step;
            _steps.TryGetValue(name, out step);

            if (step == null)
            {
                throw new InvalidOperationException(string.Format("Attempted to stop step {0} but this step has not been started.", name));
            }

            step.Dispose();
        }

        public string Render()
        {
            StopRemainingSteps();
            var stepsOrderedByStartTime = _steps.ToArray().Select(kvp => kvp.Value).OrderBy(s => s.Start).ToList();

            var sb = new StringBuilder();
            foreach (var step in stepsOrderedByStartTime)
            {
                sb.AppendLine(step.ToString());
            }
            return sb.ToString();
        }

        private void StopRemainingSteps()
        {
            foreach (var step in _steps)
            {
                step.Value.Dispose();
            }
        }
    }
}