using System;
using System.Web.UI;
using BNE.BLL;

namespace BNE.Web.UserControls
{
    public partial class ucEstatistica : UserControl
    {
        #region Page_Load

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblNumeroCurriculos.Text = Estatistica.Estatisticas.QuantidadeCurriculo.ToString("N0");
                lblNumeroVagas.Text = Convert.ToDecimal(Estatistica.Estatisticas.QuantidadeVaga).ToString("N0");
                lblNumeroEmpresa.Text = Convert.ToDecimal(Estatistica.Estatisticas.QuantidadeEmpresa).ToString("N0");
            }
        }

        #endregion
    }
}