using AllInMail.Template;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AllInMail.Core.Model
{
    public class ProgressIntegration : IProgressIntegration
    {
        public ProgressIntegration()
        {
            this.TimeBatchLoadCompleted = new DumbStaticsItem();
            this.TimeBufferLoadCompleted = new DumbStaticsItem();
            this.TimeDbBatchQuery = new DumbStaticsItem();
        }
        public int LastTargetId
        {
            get;
            set;
        }

        public DateTime FinishTime
        {
            get;
            set;
        }

        public int QuantityLoaded
        {
            get;
            set;
        }

        public int QuantityProcessed
        {
            get;
            set;
        }

        public int QuantityQuery
        {
            get;
            set;
        }

        public DateTime StartedTime
        {
            get;
            set;
        }

        public IStaticsItem TimeBatchLoadCompleted
        {
            get;
            set;
        }

        public IStaticsItem TimeBufferLoadCompleted
        {
            get;
            set;
        }

        public IStaticsItem TimeDbBatchQuery
        {
            get;
            set;
        }

        public TimeSpan TimeDbFirstLoad
        {
            get;
            set;
        }

        public TimeSpan TimeDbStartFirstBatch
        {
            get;
            set;
        }
    }
}
