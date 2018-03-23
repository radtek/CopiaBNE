using BNE.BLL;
using Hangfire;

namespace BNE.Services.Hangfire.BLL
{
    public class EnvioCampanha
    {

        #region Enviar
        [AutomaticRetry(Attempts = 0)]
        public static void Enviar()
        {
            foreach (var objCampanhaRecrutamento in CampanhaRecrutamento.CampanhasEmAberto())
            {
                objCampanhaRecrutamento.DispararGatilhoParaAssincrono();
            }
        }
        #endregion

    }
}
