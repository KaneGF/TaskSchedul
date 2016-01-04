using System;

namespace SchedulTask
{
    public class UnitTask
    {
        public UnitTask(int frequency, IUnitJob job, dynamic parameter)
        {
            Frequency = frequency;
            Job = job;
            Parameter = parameter;
        }

        public int Frequency { private set; get; }
        public TaskStatus Status { internal set; get; }
        public Exception Exception { internal set; get; }
        public IUnitJob Job { private set; get; }
        public dynamic Parameter { private set; get; }
    }
}
