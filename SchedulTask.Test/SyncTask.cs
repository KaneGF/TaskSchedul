using System;
using System.Threading;
using System.Threading.Tasks;

namespace SchedulTask.Test
{
    public class SyncTask : IUnitJob
    {

        public void Execute(dynamic param)
        {
            var t = new Task(A);
            t.Start();
            Task.WaitAll(new[] {t});
        }

        public void A()
        {
            Thread.Sleep(3000);
        }
    }
}
