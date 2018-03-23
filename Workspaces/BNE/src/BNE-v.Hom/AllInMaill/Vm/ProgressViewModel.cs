using AllInMail.Base.Vm;
using AllInMail.Template;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace AllInMail.Vm
{
    public class ProgressViewModel : ThottleNotifiable, IProgressIntegration
    {
        private int _lastCurriculoId;
        private TimeSpan _averageTimeDatabaseLoad;
        private IStaticsItem _averageTimeDatabaseQuery = new StaticsItem();
        private IStaticsItem _averageTimeWriteTarget = new StaticsItem();
        private IStaticsItem _timeCompleteBatch = new StaticsItem();
        private TimeSpan _timeDatabaseFirstLoad;
        private DateTime _startedTime;
        private DateTime _finishTime;
        private int _quantityQuery;
        private int _quantityLoaded;
        private int _quantityProcessed;

        public int QuantityQuery
        {
            get { return _quantityQuery; }
            set
            {
                _quantityQuery = value;
                OnPropertyChanged(() => QuantityQuery);
            }
        }

        public int QuantityLoaded
        {
            get { return _quantityLoaded; }
            set
            {
                _quantityLoaded = value;
                OnPropertyChanged(() => QuantityLoaded);
            }
        }
        public int QuantityProcessed
        {
            get
            {
                return _quantityProcessed;
            }
            set
            {
                _quantityProcessed = value;
                OnPropertyChanged(() => QuantityProcessed);
            }
        }


        public DateTime StartedTime
        {
            get { return _startedTime; }
            set
            {
                _startedTime = value;
                OnPropertyChanged(() => StartedTime);
            }
        }


        public DateTime FinishTime
        {
            get { return _finishTime; }
            set
            {
                _finishTime = value;
                OnPropertyChanged(() => FinishTime);
            }
        }

        public int LastTargetId
        {
            get { return _lastCurriculoId; }
            set
            {
                _lastCurriculoId = value;
                OnPropertyChanged(() => LastTargetId);
            }
        }

        public void RaiseChangesOfProperty(Expression<Func<IProgressIntegration,object>> propertyName)
        {
            RaiseChangesOfProperty(Helper.ExtGen.GetMemName(propertyName));
        }

        public void RaiseChangesOfProperty(string propertyName)
        {
            OnPropertyChanged(propertyName);
        }

        public TimeSpan TimeDbFirstLoad
        {
            get { return _timeDatabaseFirstLoad; }
            set
            {
                _timeDatabaseFirstLoad = value;
                OnPropertyChanged(() => TimeDbFirstLoad);
            }
        }

        public TimeSpan TimeDbStartFirstBatch
        {
            get { return _averageTimeDatabaseLoad; }
            set
            {
                _averageTimeDatabaseLoad = value;
                OnPropertyChanged(() => TimeDbStartFirstBatch);
            }
        }

        [ExpandableObject]
        public IStaticsItem TimeBufferLoadCompleted
        {
            get { return _averageTimeWriteTarget; }
            set
            {
                _averageTimeWriteTarget = value;
                OnPropertyChanged(() => TimeBufferLoadCompleted);
            }
        }



        [ExpandableObject]
        public IStaticsItem TimeDbBatchQuery
        {
            get { return _averageTimeDatabaseQuery; }
            set
            {
                _averageTimeDatabaseQuery = value;
                OnPropertyChanged(() => TimeDbBatchQuery);
            }
        }

        [ExpandableObject]
        public IStaticsItem TimeBatchLoadCompleted
        {
            get { return _timeCompleteBatch; }
            set
            {
                _timeCompleteBatch = value;
                OnPropertyChanged(() => TimeBatchLoadCompleted);
            }
        }






    }
}
