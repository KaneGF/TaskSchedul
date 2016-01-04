using System;

namespace SchedulTask
{
    public class UnitTask
    {
        public UnitTask(string key,int frequency, IUnitJob job)
        {
            Key = key;
            Frequency = frequency;
            Job = job;
        }

        public string Key { set; get; }
        public int Frequency { private set; get; }
        public TaskStatus Status { internal set; get; }
        public IUnitJob Job { private set; get; }

        protected internal bool Equals(string key)
        {
            return Key.Equals(key);
        }

        public override bool Equals(object o)
        {
            var unitTask = (UnitTask)Convert.ChangeType(o, typeof(UnitTask));
            return Key.Equals(unitTask.Key);
        }
    }
}
