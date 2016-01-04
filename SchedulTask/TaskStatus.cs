namespace SchedulTask
{
    /// <summary>
    /// 任务状态
    /// </summary>
    public enum TaskStatus
    {
        /// <summary>
        /// 待命，初始状态
        /// </summary>
        Standby = 0,

        /// <summary>
        /// 运行中，主动状态
        /// </summary>
        Running = 1,

        /// <summary>
        /// 暂停中，主动状态
        /// </summary>
        Stop = 2,

        /// <summary>
        /// 异常（程序空跑）,被动状态
        /// 恢复：修复后，执行Start方法即可重新开启任务
        /// 移除：若无法处理，请执行Remove方法
        /// </summary>
        Exception = 3,
    }
}
