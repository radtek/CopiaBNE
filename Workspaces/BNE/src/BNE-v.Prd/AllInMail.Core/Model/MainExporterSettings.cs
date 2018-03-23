using AllInMail.Template;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AllInMail.Core.Model
{
    public class MainExporterSettings : IMainExporter
    {
        public MainExporterSettings()
        {
            Progress = new ProgressIntegration();
            StartSettings = new StarterDefaultSettings();
        }
        public IProgressIntegration Progress
        {
            get;
            set;
        }

        public IStarterSettings StartSettings
        {
            get;
            set;
        }
    }
}
