using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SchedulTask
{
    /// <summary>
    /// 任务目录
    /// </summary>
    public class Schedul
    {
        private readonly Dictionary<string, UnitTask> _currentTasks;

        /// <summary>
        /// 任务字典
        /// </summary>
        public Dictionary<string, UnitTask> Tasks { private set; get; }

        private static readonly object LockOjb = new object();
        private static Schedul _job;
        private static Task _task;

        public static Schedul Instance
        {
            get
            {
                lock (LockOjb)
                {
                    return _job ?? (_job = new Schedul());
                }
            }
        }

        /// <summary>
        /// 实例化任务字典，创建监控任务
        /// </summary>
        public Schedul()
        {
            _currentTasks = new Dictionary<string, UnitTask>();
            Tasks = new Dictionary<string, UnitTask>();
        }

        public void StartTask()
        {
            if (_task == null
                || _task.Status != System.Threading.Tasks.TaskStatus.Running)
            {
                _task = Task.Factory.StartNew(TaskMonitor);
            }
        }

        /// <summary>
        /// 目录变动监控
        /// </summary>
        private void TaskMonitor()
        {
            while (true)
            {
                var newChannel = _currentTasks.Where(w => Tasks.All(a => !a.Key.Equals(w.Key))).ToDictionary(t => t.Key, t => t.Value);

                if (Tasks.Keys.Any(a => _currentTasks.All(c => !c.Key.Equals(a)))
                    || _currentTasks.Keys.Any(a => Tasks.All(c => !c.Key.Equals(a)))
                    )
                {
                    Tasks = _currentTasks;
                    if (newChannel.Count > 0)
                    {
                        CreateTask(newChannel);//添加并启动集合
                    }
                }
                Thread.Sleep(1000);
            }
        }

        /// <summary>
        /// 附加任务
        /// </summary>
        /// <param name="task"></param>
        public void AttchTask(Dictionary<string, UnitTask> task)
        {
            var add = task.Where(w => _currentTasks.All(a => !a.Key.Equals(w.Key))).ToDictionary(t => t.Key, t => t.Value);
            foreach (var unitTask in add)
            {
                _currentTasks.Add(unitTask.Key, unitTask.Value);
            }
        }

        /// <summary>
        /// 移除任务
        /// </summary>
        /// <param name="key"></param>
        public void RemoveTask(string key)
        {
            if (_currentTasks[key] != null)
            {
                _currentTasks.Remove(key);
            }
        }

        /// <summary>
        /// 创建任务
        /// </summary>
        /// <param name="tasks"></param>
        private void CreateTask(Dictionary<string, UnitTask> tasks)
        {
            foreach (var unitTask in tasks)
            {
                if (Tasks[unitTask.Key] != null)
                {
                    KeyValuePair<string, UnitTask> task = unitTask;
                    Task.Factory.StartNew(() => Excute(task));
                }
            }
        }

        /// <summary>
        /// 执行任务
        /// </summary>
        /// <param name="unitTask"></param>
        private void Excute(KeyValuePair<string, UnitTask> unitTask)
        {
            if (Tasks[unitTask.Key] != null)
            {
                Tasks[unitTask.Key].Status = TaskStatus.Running;
                while (Tasks[unitTask.Key] != null)
                {
                    var task = Tasks[unitTask.Key];
                    while (task != null && task.Status == TaskStatus.Running)
                    {
                        task = Tasks[unitTask.Key];
                        try
                        {
                            task.Job.Execute(task.Parameter);
                        }
                        catch (Exception ex)
                        {
                            Tasks[unitTask.Key].Status = TaskStatus.Exception;
                            Tasks[unitTask.Key].Exception = ex;
                        }
                        Thread.Sleep(task.Frequency);
                    }
                    Thread.Sleep(task.Frequency);
                }
            }
        }

        public void Start(string key)
        {
            ChangeStatus(key, TaskStatus.Running);
        }

        public void Stop(string key)
        {
            ChangeStatus(key, TaskStatus.Stop);
        }

        public void Shutdown(string key)
        {
            if (Tasks[key] != null)
            {
                Tasks.Remove(key);
            }
        }

        private void ChangeStatus(string key, TaskStatus status)
        {
            if (Tasks[key] != null)
            {
                Tasks.FirstOrDefault(f => f.Key.Equals(key)).Value.Status = status;
                Tasks.FirstOrDefault(f => f.Key.Equals(key)).Value.Exception = null;
                Excute(Tasks.FirstOrDefault(f => f.Key.Equals(key)));
            }
        }
    }
}
