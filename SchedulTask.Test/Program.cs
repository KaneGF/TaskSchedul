using System;
using System.Threading;
using System.Threading.Tasks;

namespace SchedulTask.Test
{
    class Program
    {
        private static Schedul a;
        static void Main(string[] args)
        {
            DoTask();
        }

        private static void DoTask()
        {
            var task = new Task(DoTask);
            task.Start();
            Console.WriteLine("A");
            task.Wait();
        }

        private static void DoWhile()
        {
            while (true)
            {
                Console.WriteLine("A");
                Thread.Sleep(100);
            }
        }
    }
}
