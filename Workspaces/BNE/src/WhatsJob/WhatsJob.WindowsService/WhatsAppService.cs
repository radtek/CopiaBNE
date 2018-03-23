using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WhatsJob.Domain;
using WhatsJob.Model;

namespace WhatsJob.WindowsService
{
    public partial class WhatsAppService : ServiceBase
    {
        BNE.ExceptionLog.DatabaseLogger _logger = new BNE.ExceptionLog.DatabaseLogger();
        Boolean isRunning = true;
        Random r = new Random();
        Thread t;

        public WhatsAppService()
        {
            InitializeComponent();
            if (!System.Diagnostics.EventLog.SourceExists("WhatsJob"))
            {
                System.Diagnostics.EventLog.CreateEventSource("WhatsJob", "WhatsAppService");
            }
            eventLog.Source = "WhatsJob";
            eventLog.Log = "WhatsAppService";
            eventLog.MachineName = "localhost";
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                eventLog.WriteEntry("WhatsJob is starting...", EventLogEntryType.Information);
                isRunning = true;
                ThreadStart ts = new ThreadStart(Process);
                t = new Thread(ts);
                t.Start();
                eventLog.WriteEntry("WhatsJob started succesfully...", EventLogEntryType.Information);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                //eventLog.WriteEntry(string.Format("{0}: {1}", guidError, ex.Message), EventLogEntryType.Error);
            }
        }

        protected override void OnStop()
        {
            eventLog.WriteEntry("WhatsJob is stopping...", EventLogEntryType.Information);
            isRunning = false;
            while (t.IsAlive && t.ThreadState == System.Threading.ThreadState.Running) {}
            eventLog.WriteEntry("WhatsJob stoped succesfully...", EventLogEntryType.Information);
        }

        public void Process()
        {
            Data.WhatsJobsContext _ctx = new Data.WhatsJobsContext();

            while (isRunning)
            {
                try
                {
                    foreach (var channel in _ctx.Channel.Where(c => c.Ativo))
                    {
                        try
                        {
                            Domain.Channel chDomain = new Domain.Channel(channel);

                            chDomain.Login();
                            if (chDomain.Status != WhatsAppApi.ApiBase.CONNECTION_STATUS.LOGGEDIN)
                            {
                                _logger.Error(new Exception("Failed on login"), "Channel:"+channel.Number);
                                eventLog.WriteEntry(String.Format("Failed trying to login on whatsapp ({0})", channel.Number));
                                continue;
                            }

                            chDomain.SendQueuedMessages();
                            Domain.Message.RespondMessages(channel);
                            chDomain.PollMessages();
                            chDomain.Disconnect();
                        }
                        catch (Exception ex)
                        {
                            _logger.Error(ex, String.Format("Error processing channel {0}", channel.Number));
                        }

                        Thread.Sleep(r.Next(2500, 10000));
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Erro in Process While");
                }
            }
        }
    }
}
