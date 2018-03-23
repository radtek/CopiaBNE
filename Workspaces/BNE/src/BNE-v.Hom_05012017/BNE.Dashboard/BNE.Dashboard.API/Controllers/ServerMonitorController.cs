using BNE.Dashboard.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Configuration;
using System.Web.Http;

namespace BNE.Dashboard.API.Controllers
{
    public class ServerInformation
    {
        public string applicationName;
        public string ip;
        public decimal cpu;
        public decimal ram;
        public string color;
        public bool solrCheck;
        public bool sqlCheck;
        public decimal solrResponseTime;
        public decimal sqlResponseTime;
        public List<String> messages;
    }

    public class ServerMessage
    {
        public DateTime data;
        public String message;
    }

    public class ServerMonitorController : ApiController
    {
        static List<Server> servers = new List<Server>();
        static int timeToInactive = Convert.ToInt32(WebConfigurationManager.AppSettings["TimeForServerIsInactive"]);
        static int timeToDown = Convert.ToInt32(WebConfigurationManager.AppSettings["TimeForServerIsDown"]);
        static int RamLimitWarning = Convert.ToInt32(WebConfigurationManager.AppSettings["RamLimitWarning"]);
        static int CpuLimitWarning = Convert.ToInt32(WebConfigurationManager.AppSettings["CpuLimitWarning"]);

        // GET: api/ServerMonitor
        public HttpResponseMessage Get()
        {
            try
            {
                //Removing old servers
                servers.RemoveAll(s => (DateTime.Now - s.lastCommunication).Seconds >= timeToDown);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Erro ao remover servidores antigos: " + ex.Message + " Stack:" + ex.StackTrace);
            }

            try
            {
                //Set servers with warning
                servers.ForEach(s => s.status = s.lastCpu > CpuLimitWarning || s.lastRam > RamLimitWarning ? ServerStatus.warning : ServerStatus.up);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Erro ao setar servidores com alto processamento/memória: " + ex.Message + " Stack:" + ex.StackTrace);
            }

            try
            {
                //Set inactived servers
                servers.ForEach(s => s.status = (DateTime.Now - s.lastCommunication).Seconds >= timeToInactive ? ServerStatus.inactive : s.status);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Erro ao setar servidores como inativos: " + ex.Message + " Stack:" + ex.StackTrace);
            }

            //try
            //{
            //    //servers.ForEach(s => s.cpu.RemoveAll(c => (DateTime.Now - c.date).Seconds >= timeToDown));
            //    foreach (var server in servers)
            //    {
            //        try
            //        {
            //            server.cpu.RemoveAll(c => (DateTime.Now - c.date).Seconds >= timeToDown);
            //        }
            //        catch
            //        {
            //            server.cpu = new List<ServerItem>();
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Erro ao remover cpu's antigas: " + ex.Message + " Stack:" + ex.StackTrace);
            //}

            //try
            //{
            //    //servers.ForEach(s => s.ram.RemoveAll(c => (DateTime.Now - c.date).Seconds >= timeToDown));
            //    foreach (var server in servers)
            //    {
            //        try
            //        {
            //            server.ram.RemoveAll(c => (DateTime.Now - c.date).Seconds >= timeToDown);
            //        }
            //        catch
            //        {
            //            server.ram = new List<ServerItem>();
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Erro ao remover ram's antigas: " + ex.Message + " Stack:" + ex.StackTrace);
            //}

            List<string> messages = new List<string>();
            servers.ForEach(s => messages.AddRange(s.messages.Select(m => m.message).ToList()));
            messages = messages.Distinct().ToList();
            return Request.CreateResponse(HttpStatusCode.OK, new { servers = servers.OrderBy(s => s.applicationName).ToList(), messages = messages });
        }

        // GET: api/ServerMonitor/5
        public HttpResponseMessage Get(int id)
        {
            return Request.CreateResponse(HttpStatusCode.MethodNotAllowed);
        }

        // POST: api/ServerMonitor
        public HttpResponseMessage Post([FromBody]ServerInformation serverInfo)
        {
            Server objServer = servers.FirstOrDefault(s => s.ip == serverInfo.ip);
            if (objServer == null)
            {
                objServer = new Server()
                {
                    applicationName = serverInfo.applicationName,
                    color = serverInfo.color,
                    ip = serverInfo.ip,
                };

                servers.Add(objServer);
            }

            objServer.lastRam = serverInfo.ram;
            objServer.lastCpu = serverInfo.cpu;
            objServer.lastCommunication = DateTime.Now;
            //objServer.ram.Add(new ServerItem() { date = DateTime.Now, percent = serverInfo.ram });
            //objServer.cpu.Add(new ServerItem() { date = DateTime.Now, percent = serverInfo.cpu });

            if (serverInfo.solrCheck)
            {
                objServer.solrCheck = true;
                objServer.lastSolr = serverInfo.solrResponseTime;
            }

            if (serverInfo.sqlCheck)
            {
                objServer.sqlCheck = true;
                objServer.lastSql = serverInfo.sqlResponseTime;
            }

            foreach (var item in serverInfo.messages)
            {
                objServer.messages.Add(new Message() { data = DateTime.Now, message = item });
            }

            objServer.messages.RemoveAll(s => (DateTime.Now - s.data).Seconds >= timeToDown);

            return Request.CreateResponse(HttpStatusCode.Created);
        }

        // PUT: api/ServerMonitor/5
        public HttpResponseMessage Put(int id, [FromBody]string value)
        {
            return Request.CreateResponse(HttpStatusCode.MethodNotAllowed);
        }

        // DELETE: api/ServerMonitor/5
        public HttpResponseMessage Delete(int id)
        {
            return Request.CreateResponse(HttpStatusCode.MethodNotAllowed);
        }
    }
}
