using AllInMail.Base.Vm;
using AllInMail.Template;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace AllInMail.Vm
{

    public class MainViewModel : ThottleNotifiable, IMainExporter
    {
        private string _actionTitle;
        private IProgressIntegration _progress;
        private IStarterSettings _startSettings;
        private string _outputFile = @"C:\temp\data.csv";
        private bool _canInvokeAction;
        private bool _enableEditStartedSettings = true;
        private ExportationType _processType;
        [Browsable(false)]
        public bool EnableEditStartedSettings
        {
            get { return _enableEditStartedSettings; }
            set
            {
                _enableEditStartedSettings = value;
                OnPropertyChanged(() => EnableEditStartedSettings);
            }
        }

        public MainViewModel()
        {
            _startSettings = new MetricSettings();
            _progress = new ProgressViewModel();
        }
        [Browsable(false)]
        public string ActionTitle
        {
            get
            {
                return _actionTitle;
            }
            set
            {
                _actionTitle = value;
                OnPropertyChanged(() => ActionTitle);
            }
        }

        [ExpandableObject]
        public IProgressIntegration Progress
        {
            get { return _progress; }
            set
            {
                _progress = value;
                OnPropertyChanged(() => Progress);
            }
        }
        [ExpandableObject]
        public IStarterSettings StartSettings
        {
            get { return _startSettings; }
            set
            {
                _startSettings = value;
                OnPropertyChanged(() => StartSettings);
            }
        }

        [Browsable(false)]
        public bool CanInvokeAction
        {
            get { return _canInvokeAction; }
            set
            {
                _canInvokeAction = value;
                OnPropertyChanged(() => CanInvokeAction);
            }
        }
        [Browsable(false)]
        public string OutputFile
        {
            get { return _outputFile; }
            set
            {
                _outputFile = value;
                OnPropertyChanged(() => OutputFile);
            }
        }

        public ExportationType ProcessType
        {
            get { return _processType; }
            set
            {
                _processType = value;
                OnPropertyChanged(() => ProcessType);
            }
        }
        
    }

    public enum ExportationType
    {
        Curriculo,
        Empresas
    }
}
