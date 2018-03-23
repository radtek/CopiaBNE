using AllInMail.Base.Vm;
using AllInMail.Template;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllInMail.Vm
{
    [Serializable]
    public class StaticsItem : ThottleNotifiable, IStaticsItem
    {
        private int _count;
        public int TotalCount
        {
            get { return _count; }
            set
            {
                _count = value;
                OnPropertyChanged(() => TotalCount);
            }
        }

        private TimeSpan _min;

        public TimeSpan Min
        {
            get { return _min; }
            set
            {
                _min = value;
                OnPropertyChanged(() => Min);
            }
        }

        private TimeSpan _average;

        public TimeSpan Average
        {
            get { return _average; }
            set
            {
                _average = value;
                OnPropertyChanged(() => Average);
            }
        }

        private TimeSpan _max;

        public TimeSpan Max
        {
            get { return _max; }
            set
            {
                _max = value;
                OnPropertyChanged(() => Max);
            }
        }

        public void Increment(TimeSpan timeSpan)
        {
            if (default(TimeSpan) == _min)
            {
                Min = timeSpan;
            }
            else
            {
                if (_min.Ticks > timeSpan.Ticks)
                {
                    Min = timeSpan;
                }
            }

            if (default(TimeSpan) == _max)
            {
                Max = timeSpan;
            }
            else
            {
                if (_max.Ticks < timeSpan.Ticks)
                {
                    Max = timeSpan;
                }
            }
            if (TotalCount == 0)
            {
                Average = timeSpan;
                TotalCount = 1;
                return;
            }

            var newCount = TotalCount + 1;
            var average = ((Average.Ticks * TotalCount) + timeSpan.Ticks) / newCount;
            Average = new TimeSpan(average);
            TotalCount = newCount;
        }
    }
}
