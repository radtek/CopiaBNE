using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Diagnostics;
using System.ServiceProcess;
using BNE.Services.AsyncExecutor.Properties;

namespace BNE.Services.AsyncExecutor
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
        }

        readonly SortedList<string, string> _eventSources = new SortedList<string, string>();

        #region ServiceProcessInstaller_BeforeInstall
        private void ServiceProcessInstaller_BeforeInstall(object sender, InstallEventArgs e)
        {
            RemoveServiceEventLogs();
        }
        #endregion

        #region ServiceProcessInstaller_Committed
        private void ServiceProcessInstaller_Committed(object sender, InstallEventArgs e)
        {
            RemoveServiceEventLogs();
            foreach (KeyValuePair<string, string> eventSource in _eventSources)
            {
                if (EventLog.SourceExists(eventSource.Key))
                    EventLog.DeleteEventSource(eventSource.Key);

                EventLog.CreateEventSource(eventSource.Key, eventSource.Value);
            }
        }
        #endregion

        #region RemoveServiceEventLogs
        private void RemoveServiceEventLogs()
        {
            foreach (Installer installer in Installers)
            {
                if (installer is ServiceInstaller)
                {
                    var serviceInstaller = installer as ServiceInstaller;
                    if (EventLog.SourceExists(serviceInstaller.ServiceName))
                    {
                        //eventSources.Add(serviceInstaller.ServiceName, EventLog.LogNameFromSourceName(serviceInstaller.ServiceName, Environment.MachineName));
                        _eventSources.Add(serviceInstaller.ServiceName, Settings.Default.LogName);
                        EventLog.DeleteEventSource(serviceInstaller.ServiceName);
                    }
                }
            }
        }
        #endregion

    }
}
