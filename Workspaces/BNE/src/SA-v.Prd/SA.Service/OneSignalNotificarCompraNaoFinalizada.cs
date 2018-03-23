using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Reflection;
using AdminLTE_Application;
using log4net;
using Quartz;
using SA.WebPush.Interfaces;
using SA.Service.DTO;

namespace SA.Service
{
    [DisallowConcurrentExecution]
    public class OneSignalNotificarCompraNaoFinalizada : IJob
    {
        private static readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Guid templateid = Guid.Parse(ConfigurationManager.AppSettings["OneSignal-NovaTentativaCompraTemplateID"]);
        private readonly IWebPushService _webPushService;

        public OneSignalNotificarCompraNaoFinalizada(IWebPushService webPushService)
        {
            ServicePointManager.DefaultConnectionLimit = 50000;
            ServicePointManager.Expect100Continue = false;

            _webPushService = webPushService;
        }

        public void Execute(IJobExecutionContext context)
        {
            _logger.Debug($"Job {GetType().FullName}  started now " + DateTime.Now);

            try
            {
                int empSemVendedor = 0;
                var users = new List<string>();
                using (var dbContext = new Model())
                {
                    var listEmpresa = dbContext.Database.SqlQuery<Empresa>("dbo.[Webpush-RecuperarTentativasCompra]");

                    foreach (var item in listEmpresa)
                    {
                        string titulo = "Empresa com tentativa de compra";
                        string mensagem = "Esta empresa esta tentando comprar plano.";
                        string url = $"https://sa.bne.com.br/Empresa/Details/{item.CNPJ}";
                        var empresa = dbContext.CRM_Empresa.FirstOrDefault(c => c.Num_CNPJ == item.CNPJ);
                        var vendedor = empresa?.RecuperarVendedorResponsavel();
                        if (vendedor?.Des_OneSignalToken != null)
                        {
                            _webPushService.Send(new List<string> { vendedor.Des_OneSignalToken.ToString() }, titulo, mensagem, url);
                        }
                        else
                            empSemVendedor++;
                    }
                    if (empSemVendedor > 0)
                    {
                        users = dbContext.CRM_Vendedor.Where(c => c.Des_OneSignalToken != null).Select(c => c.Des_OneSignalToken.ToString()).ToList();
                    }
                }

                if (empSemVendedor > 0 && users.Any())
                {
                    _logger.Debug(_webPushService.SendTemplate(users, templateid));
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Job {GetType().FullName}  processing error", ex);
            }
            _logger.Debug($"Job {GetType().FullName}  ended...");
        }
    }
}