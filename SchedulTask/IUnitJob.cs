using System;

namespace SchedulTask
{
    public interface IUnitJob
    {
        void Execute(dynamic param);
    }
}
