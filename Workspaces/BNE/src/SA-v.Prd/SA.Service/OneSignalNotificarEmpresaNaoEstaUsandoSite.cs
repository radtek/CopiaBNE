using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using AdminLTE_Application;
using log4net;
using Quartz;
using SA.Service.DTO;
using SA.WebPush.Interfaces;

namespace SA.Service
{
    [DisallowConcurrentExecution]
    public class OneSignalNotificarEmpresaNaoEstaUsandoSite : IJob
    {
        private static readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IWebPushService _webPushService;

        public OneSignalNotificarEmpresaNaoEstaUsandoSite(IWebPushService webPushService)
        {
            ServicePointManager.DefaultConnectionLimit = 50000;
            ServicePointManager.Expect100Continue = false;

            _webPushService = webPushService;
        }

        public void Execute(IJobExecutionContext context)
        {
            _logger.Debug($"Job {GetType().FullName} started now " + DateTime.Now);

            try
            {
                string titulo = "Empresa não está usando o BNE";
                string mensagem = "Nos últimos dias notamos que essa empresa diminuiu o uso do site! Clique e veja os detalhes da empresa.";

                using (var dbContext = new Model())
                {
                    var empresas = dbContext.Database.SqlQuery<Empresa>("dbo.[Webpush-RecuperarEmpresasNaoUsandoSite]");

                    if (empresas.Any())
                    {
                        foreach (var empresa in empresas)
                        {
                            string url = $"https://sa.bne.com.br/Empresa/Details/{empresa.CNPJ}";
                            _webPushService.Send(new List<string> { empresa.OneSignalTokenVendedor.ToString() }, titulo, mensagem, url);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Job {GetType().FullName} processing error", ex);
            }
            _logger.Debug($"Job {GetType().FullName} ended...");
        }
    }
}