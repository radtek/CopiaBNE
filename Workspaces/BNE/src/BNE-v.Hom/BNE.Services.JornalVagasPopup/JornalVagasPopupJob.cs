using System;
using System.Reflection;
using BNE.BLL.Notificacao;
using BNE.EL;
using Common.Logging;
using Quartz;

namespace BNE.Services.JornalVagasPopup
{
    [DisallowConcurrentExecution]
    public class JornalVagasPopupJob : IJob
    {
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public void Execute(IJobExecutionContext context)
        {
            _logger.Debug("Job started...");
            _logger.Debug("Job started now " + DateTime.Now);

            try
            {
                new ProcessamentoJornalPopup().IniciarProcessoPopup();
            }
            catch (Exception ex)
            {
                var guid = GerenciadorException.GravarExcecao(ex);
                LogErroJornal.Logar(ex.Message + " Guid: " + guid, string.Empty, string.Empty);
                _logger.Error(ex);
            }

            _logger.Debug("Job ended...");
        }
    }
}