using BNE.Dashboard.ServerMonitor.Custom;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Security;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BNE.Dashboard.ServerMonitor
{
    public partial class ServerMonitor : ServiceBase
    {
        private Thread _objThread;
        private Thread thGetStatistic;
        private Thread thGetSolr;
        private Thread thGetSql;
        static EventLog eventLog = new EventLog();

        //Lista usada para armazenar as estatísticas de memória
        static List<ServerStatic> lstMemorySnapshots = new List<ServerStatic>();
        //Lista usada para armazenar as estatísticas de cpu
        static List<ServerStatic> lstCpuSnapshots = new List<ServerStatic>();
        //Lista usada para armazenar as estatísticas do Solr
        static List<ConnectionStatic> lstSolrSnapshots = new List<ConnectionStatic>();
        //Lista usada para armazenar as estatísticas do Sql Server 
        static List<ConnectionStatic> lstSqlSnapshots = new List<ConnectionStatic>();
        //Lista usada para armazenar as estatísticas do Sql Server 
        static List<Message> lstMessage = new List<Message>();

        static object lockListsStatistics = new object();
        static object lockListSolr = new object();
        static object lockListSql = new object();

        public ServerMonitor()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            var objTs = new ThreadStart(Monitor);
            _objThread = new Thread(objTs);
            _objThread.Start();
        }

        protected override void OnStop()
        {
            if (_objThread.IsAlive) _objThread.Abort();
            if (thGetStatistic != null && thGetStatistic.IsAlive) thGetStatistic.Abort();
            if (thGetSolr != null && thGetSolr.IsAlive) thGetSolr.Abort();
            if (thGetSql != null && thGetSql.IsAlive) thGetSql.Abort();
            eventLog.WriteEntry("ServerMonitor stopped");
        }

        public void Monitor()
        {
#if DEBUG
            if (!System.Diagnostics.EventLog.SourceExists("BNE.Dashboard.ServerMonitor"))
                System.Diagnostics.EventLog.CreateEventSource("BNE.Dashboard.ServerMonitor", "Application");
#endif
            eventLog.Source = "BNE.Dashboard.ServerMonitor";
            //eventLog.Log = "BNE.Dashboard";

            eventLog.WriteEntry("ServerMonitor started");

            PerformanceCounter cpuCounter;
            PerformanceCounter ramCounter;

            cpuCounter = new PerformanceCounter();

            cpuCounter.CategoryName = "Processor";
            cpuCounter.CounterName = "% Processor Time";
            cpuCounter.InstanceName = "_Total";

            ramCounter = new PerformanceCounter("Memory", "% Committed Bytes In Use");

            #region Thread que capturará os dados do servidor no intervalo de tempo determinado nos Settings
            thGetStatistic = new Thread(t =>
            {
                while (true)
                {
                    try
                    {
                        string solrMessage = string.Empty, sqlMessage = string.Empty;
                        float solrTime = 0, sqlTime = 0;
                        bool solrOk = true, sqlOk = true;

                        if (Settings.Default.SolrCheck)
                            solrOk = CheckSolr(out solrTime, out solrMessage);

                        if (Settings.Default.SqlCheck)
                            sqlOk = CheckSql(out sqlTime, out sqlMessage);

                        //Obtendo dados da RAM e CPU
                        lock (lockListsStatistics)
                        {
                            var percetage = (float)cpuCounter.NextValue();
                            if (percetage > 0)
                                lstCpuSnapshots.Add(new ServerStatic { percetage = percetage, date = DateTime.Now });

                            percetage = (float)ramCounter.NextValue();
                            if (percetage > 0)
                                lstMemorySnapshots.Add(new ServerStatic { percetage = percetage, date = DateTime.Now });
                        }
                    }
                    catch (Exception ex)
                    {
                        eventLog.WriteEntry(ex.Message);
                    }

                    //Aguardando tempo configurado para a captura de dados
                    Thread.Sleep(Settings.Default.GetStatisticInterval);
                }

            }) { IsBackground = true };
            thGetStatistic.Start();
            #endregion Thread que capturará os dados do servidor no intervalo de tempo determinado nos Settings

            #region Thread que capturará os dados de conexão com o solr
            if (Settings.Default.SolrCheck)
            {
                thGetSolr = new Thread(t =>
                {
                    while (true)
                    {
                        try
                        {
                            string solrMessage = string.Empty;
                            float solrTime = 0;
                            bool solrOk = true;

                            solrOk = CheckSolr(out solrTime, out solrMessage);

                            //Obtendo dados da RAM e CPU
                            lock (lockListSolr)
                            {
                                lstSolrSnapshots.Add(new ConnectionStatic { statusOk = solrOk, responseTime = solrTime, date = DateTime.Now });
                                if (!String.IsNullOrEmpty(solrMessage))
                                {
                                    lstMessage.RemoveAll(m => m.Key == "SOLR");
                                    lstMessage.Add(new Message() { Key = "SOLR", Descricao = solrMessage, Date = DateTime.Now });
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            eventLog.WriteEntry(ex.Message);
                        }

                        //Aguardando tempo configurado para a captura de dados
                        Thread.Sleep(Settings.Default.GetStatisticInterval);
                    }

                }) { IsBackground = true };
                thGetSolr.Start();
            }
            #endregion Thread que capturará os dados de conexão com o solr

            #region Thread que capturará os dados de conexão com o SQL Server
            if (Settings.Default.SqlCheck)
            {
                thGetSql = new Thread(t =>
                {
                    while (true)
                    {
                        try
                        {
                            string sqlMessage = string.Empty;
                            float sqlTime = 0;
                            bool sqlOk = true;

                            sqlOk = CheckSql(out sqlTime, out sqlMessage);

                            //Obtendo dados da RAM e CPU
                            lock (lockListSql)
                            {
                                lstSqlSnapshots.Add(new ConnectionStatic { statusOk = sqlOk, errorMessage = sqlMessage, responseTime = sqlTime, date = DateTime.Now });
                                if (!String.IsNullOrEmpty(sqlMessage))
                                {
                                    lstMessage.RemoveAll(m => m.Key == "SSERVER");
                                    lstMessage.Add(new Message() { Key = "SSERVER", Descricao = sqlMessage, Date = DateTime.Now });
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            eventLog.WriteEntry(ex.Message);
                        }

                        //Aguardando tempo configurado para a captura de dados
                        Thread.Sleep(Settings.Default.GetStatisticInterval);
                    }

                }) { IsBackground = true };
                thGetSql.Start();
            }
            #endregion Thread que capturará os dados do servidor no intervalo de tempo determinado nos Settings

            #region Loop infinito responsável pelo envio ao Dashboard das estatísticas capturadas
            while (true)
            {
                //Aguardando o tempo configurado para o envio
                Thread.Sleep(Settings.Default.SendStatisticsInterval);

                try
                {
                    float cpuPercentage, ramPercentage, solrTime = 0, sqlTime = 0;
                    List<string> messages;

                    //Calculando médias de ram e cpu
                    lock (lockListsStatistics)
                    {
                        cpuPercentage = lstCpuSnapshots.Sum(n => n.percetage) / lstCpuSnapshots.Count;
                        ramPercentage = lstMemorySnapshots.Sum(n => n.percetage) / lstMemorySnapshots.Count;
                        lstCpuSnapshots.Clear();
                        lstMemorySnapshots.Clear();
                    }

                    if (Settings.Default.SolrCheck)
                    {
                        solrTime = lstSolrSnapshots.Sum(n => n.responseTime) / lstSolrSnapshots.Count;
                        lock (lockListSolr)
                        {
                            lstSolrSnapshots.Clear();
                        }
                    }

                    if (Settings.Default.SqlCheck)
                    {
                        sqlTime = lstSqlSnapshots.Sum(n => n.responseTime) / lstSqlSnapshots.Count;
                        lock (lockListSql)
                        {
                            lstSqlSnapshots.Clear();
                        }
                    }

                    messages = lstMessage.Select(m => String.Format("[{0} - {1}] {2}", Settings.Default.AplicationName, LocalIPAddress(), m.Descricao)).ToList();
                    lstMessage.Clear();

                    //Criando objeto a ser enviado ao server
                    var data = new
                    {
                        applicationName = Settings.Default.AplicationName,
                        ip = LocalIPAddress(),
                        cpu = cpuPercentage,
                        ram = ramPercentage,
                        solrCheck = Settings.Default.SolrCheck,
                        sqlCheck = Settings.Default.SqlCheck,
                        solrResponseTime = solrTime,
                        sqlResponseTime = sqlTime,
                        messages = messages,
                        color = "#00559B"
                    };


                    //Enviando dados para a API
                    SendStatistics(data).Wait();
                }
                catch (Exception ex)
                {
                    eventLog.WriteEntry(ex.Message);
                }

            }
            #endregion Loop infinito responsável pelo envio ao Dashboard das estatísticas capturadas

        }

        /// <summary>
        /// Chama a API do Dashboard para o envio das estatísticas do servidor
        /// </summary>
        /// <param name="data">Estatísticas do servidor</param>
        /// <returns></returns>
        static async Task SendStatistics(object data)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    // New code:
#if DEBUG
                    client.BaseAddress = new Uri(Settings.Default.DashboardAPI_Dev);
#else
                    client.BaseAddress = new Uri(Settings.Default.DashboardAPI);
#endif
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = await client.PostAsJsonAsync("api/servermonitor", data);

                    if (!response.IsSuccessStatusCode)
                        eventLog.WriteEntry(String.Format("Ocorreu uma falha ao chamar a API (Status Code:{0}-{1})", (int)response.StatusCode, response.StatusCode.ToString()));
                }
            }
            catch (Exception ex)
            {
                eventLog.WriteEntry(ex.Message);
            }
        }

        /// <summary>
        /// Chama a API do Dashboard para o envio das estatísticas do servidor
        /// </summary>
        /// <param name="data">Estatísticas do servidor</param>
        /// <returns></returns>
        static async Task SendMessage(String data)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    // New code:
#if DEBUG
                    client.BaseAddress = new Uri(Settings.Default.DashboardAPI_Dev);
#else
                    client.BaseAddress = new Uri(Settings.Default.DashboardAPI);
#endif
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = await client.PostAsJsonAsync("api/servermonitor", data);

                    if (!response.IsSuccessStatusCode)
                        eventLog.WriteEntry(String.Format("Ocorreu uma falha ao chamar a API (Status Code:{0}-{1})", (int)response.StatusCode, response.StatusCode.ToString()));
                }
            }
            catch (Exception ex)
            {
                eventLog.WriteEntry(ex.Message);
            }
        }

        /// <summary>
        /// Recupera o IP da máquina local
        /// </summary>
        /// <returns>Ip da máquina local</returns>
        public static string LocalIPAddress()
        {
            IPHostEntry host;
            string localIP = "";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIP = ip.ToString();
                    break;
                }
            }
            return localIP;
        }

        public static bool CheckSolr(out float time, out string errorMessage)
        {
            Stopwatch sw = new Stopwatch();
            errorMessage = string.Empty;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Settings.Default.SolrURL);
            request.Method = "GET";
            request.ContentType = "application/json";

            sw.Start();
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    sw.Stop();
                    time = sw.ElapsedMilliseconds;
                    return response.StatusCode == HttpStatusCode.OK;
                }
            }
            catch (Exception ex)
            {
                if (sw.IsRunning) sw.Stop();
                time = sw.ElapsedMilliseconds;

                StringBuilder sb = new StringBuilder();
                while (ex != null)
                {
                    sb.AppendLine(ex.Message);
                    ex = ex.InnerException;
                }
                errorMessage = sb.ToString();
            }

            return false;
        }

        public static bool CheckSql(out float time, out string errorMessage)
        {
            Stopwatch sw = new Stopwatch();
            errorMessage = string.Empty;

            try
            {
                using (SqlConnection sqlConnection1 = new SqlConnection(Settings.Default.SqlConnection))
                {
                    SqlCommand cmd = new SqlCommand();
                    SqlDataReader reader;

                    cmd.CommandText = Settings.Default.SqlCommand;
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = sqlConnection1;

                    sw.Start();
                    sqlConnection1.Open();
                    reader = cmd.ExecuteReader();
                    sqlConnection1.Close();
                    sw.Stop();

                    time = sw.ElapsedMilliseconds;
                }

                return true;
            }
            catch (Exception ex)
            {
                if (sw.IsRunning) sw.Stop();
                time = sw.ElapsedMilliseconds;

                StringBuilder sb = new StringBuilder();
                while (ex != null)
                {
                    sb.AppendLine(ex.Message);
                    ex = ex.InnerException;
                }
                errorMessage = sb.ToString();
            }

            return false;
        }
    }
}
