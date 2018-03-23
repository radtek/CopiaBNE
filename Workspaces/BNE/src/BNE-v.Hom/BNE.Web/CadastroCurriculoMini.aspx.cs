using System;
using BNE.Web.Code;

namespace BNE.Web
{
    public partial class CadastroCurriculoMini : BasePage
    {

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IdPessoaFisicaLogada.HasValue)
                ucMiniCurriculo.IdPessoaFisica = IdPessoaFisicaLogada.Value;
        }
        #endregion

    }
}
