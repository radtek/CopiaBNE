using System;
namespace AllInMail.Template
{
    public interface IProgressIntegration
    {
        int LastTargetId { get; set; }
        DateTime FinishTime { get; set; }
        int QuantityLoaded { get; set; }
        int QuantityProcessed { get; set; }
        int QuantityQuery { get; set; }
        DateTime StartedTime { get; set; }
        IStaticsItem TimeBatchLoadCompleted { get; set; }
        IStaticsItem TimeBufferLoadCompleted { get; set; }
        IStaticsItem TimeDbBatchQuery { get; set; }
        TimeSpan TimeDbFirstLoad { get; set; }
        TimeSpan TimeDbStartFirstBatch { get; set; }
    }
}
