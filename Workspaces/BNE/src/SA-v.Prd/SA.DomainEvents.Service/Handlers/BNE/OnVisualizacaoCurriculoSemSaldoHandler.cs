using System;
using System.Collections.Generic;
using System.Linq;
using AdminLTE_Application;
using BNE.Domain.Events.CrossDomainEvents;
using log4net;
using Newtonsoft.Json;
using SA.WebPush.Interfaces;
using SharedKernel.DomainEvents.Core;

namespace SA.DomainEvents.Service.Handlers.BNE
{
    public class OnVisualizacaoCurriculoSemSaldoHandler : IHandler<OnVisualizacaoCurriculoSemSaldo>
    {
        private readonly ILog _log;
        private readonly IWebPushService _webPushService;

        public OnVisualizacaoCurriculoSemSaldoHandler(ILog log, IWebPushService webPushService)
        {
            _webPushService = webPushService;
            _log = log;
        }

        public void Handle(OnVisualizacaoCurriculoSemSaldo args)
        {
            _log.Debug($"Handling {GetType().FullName}: " + JsonConvert.SerializeObject(args));

            try
            {
                string titulo = "Empresa Sem Saldo";
                string mensagem = "Esta empresa com plano está tentando visualizar currículo mas o saldo de visualização já acabou! Clique e veja os detalhes da empresa.";
                string url = $"https://sa.bne.com.br/Empresa/Details/{args.CNPJ}";

                using (var context = new Model())
                {
                    var empresa = context.CRM_Empresa.FirstOrDefault(c => c.Num_CNPJ == args.CNPJ);
                    var vendedor = empresa?.RecuperarVendedorResponsavel();
                    if (vendedor?.Des_OneSignalToken != null)
                    {
                        _webPushService.Send(new List<string> { vendedor.Des_OneSignalToken.ToString() }, titulo, mensagem, url);
                    }
                }
            }
            catch (Exception e)
            {
                _log.Error($"An error occurred while handling event {GetType().FullName}", e);
                throw;
            }
        }
    }
}
