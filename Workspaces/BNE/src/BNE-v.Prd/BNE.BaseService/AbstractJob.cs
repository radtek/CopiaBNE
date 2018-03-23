using System;
using System.Net;
using log4net;
using Quartz;

namespace BNE.BaseService
{
    [DisallowConcurrentExecution]
    public abstract class AbstractJob : IJob
    {
        protected readonly ILog _logger;

        protected AbstractJob(ILog logger)
        {
            ServicePointManager.DefaultConnectionLimit = 50000;
            ServicePointManager.Expect100Continue = false;

            _logger = logger;
        }

        public abstract void Execute();

        public void Execute(IJobExecutionContext context)
        {
            _logger.Debug($"Job {GetType().FullName}  started now " + DateTime.Now);

            try
            {
                Execute();
            }
            catch (Exception ex)
            {
                _logger.Error($"Job {GetType().FullName}  processing error", ex);
            }
            _logger.Debug($"Job {GetType().FullName}  ended...");
        }
    }
}