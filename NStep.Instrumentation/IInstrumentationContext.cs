using System;

namespace NStep.Instrumentation
{
    public interface IInstrumentationContext
    {
        IDisposable StartStep(string name);
        void StopStep(string name);
        string Render();
    }
}