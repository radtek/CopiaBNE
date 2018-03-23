using AllInMail.Base.Vm;
using AllInMail.Template;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllInMail.Vm
{
    public class MetricSettings : ThottleNotifiable, IStarterSettings
    {
        private int _startFromCurriculoId;
        private string _queryParam;

        public DateTime? FromDate { get; set; }
        public int StartAboveTargetId
        {
            get { return _startFromCurriculoId; }
            set
            {
                _startFromCurriculoId = value;
                OnPropertyChanged(() => StartAboveTargetId);
            }
        }

        private int _maxQuantity;

        public int MaxQuantity
        {
            get { return _maxQuantity; }
            set
            {
                _maxQuantity = value;
                OnPropertyChanged(() => MaxQuantity);
            }
        }
        public string QueryParam
        {
            get { return _queryParam; }
            set
            {
                _queryParam = value;
                OnPropertyChanged(() => QueryParam);
            }
        }


        private int _batchSize = 5000;
        private int _bufferSize = 100;

        public int BufferSize
        {
            get { return _bufferSize; }
            set
            {
                _bufferSize = value;
                OnPropertyChanged(() => BufferSize);
            }
        }

        public int BatchSize
        {
            get { return _batchSize; }
            set
            {
                _batchSize = value;
                OnPropertyChanged(() => BatchSize);
            }
        }


        private bool _continueOfLastPoint;

        public bool ContinueInFinalOfTheFile
        {
            get { return _continueOfLastPoint; }
            set
            {
                _continueOfLastPoint = value;

                OnPropertyChanged(() => ContinueInFinalOfTheFile);
                if (value)
                    WriterHeader = false;
            }
        }

        private bool _writerHeader = true;

        public bool WriterHeader
        {
            get { return _writerHeader; }
            set
            {
                if (ContinueInFinalOfTheFile && _writerHeader)
                    _writerHeader = false;
                else
                    _writerHeader = value;

                OnPropertyChanged(() => WriterHeader);
            }
        }

    }
}
