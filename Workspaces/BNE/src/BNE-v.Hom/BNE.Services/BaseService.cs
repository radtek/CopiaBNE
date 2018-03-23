using System.ServiceProcess;
using BNE.Services.Base.EventLog;
using BNE.Services.Properties;

namespace BNE.Services
{
    public partial class BaseService : ServiceBase
    {
        protected EventLogWriter EventLogWriter { get; set; }

        public BaseService()
        {
            InitializeComponent();
            EventLogWriter = new EventLogWriter(Settings.Default.LogName, GetType().Name);
        }
    }
}