using System;
using System.Net;

namespace BNE.BLL.Notificacao
{
    public class JornalVagas
    {
        public static void ProcessarJornal()
        {
            ServicePointManager.DefaultConnectionLimit = 10000;
            ServicePointManager.Expect100Continue = false;
            try
            {
                ProcessamentoJornalVagas.ProcessarJornaldeVagas();
            }
            catch (Exception ex)
            {
                var guid = EL.GerenciadorException.GravarExcecao(ex);
                LogErroJornal.Logar(ex.Message + " Guid: " + guid, string.Empty, string.Empty);
            }
        }
    }
}
