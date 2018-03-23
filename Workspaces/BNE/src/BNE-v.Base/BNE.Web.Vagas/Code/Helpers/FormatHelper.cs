namespace BNE.Web.Vagas.Code.Helpers
{
    public class FormatHelper
    {
        #region RetornarDesricaoSalario
        public string RetornarDesricaoSalario(decimal? valorSalarioDe, decimal? valorSalarioAte)
        {
            return BLL.Custom.Helper.RetornarDesricaoSalario(valorSalarioDe, valorSalarioAte);
        }
        #endregion
    }
}