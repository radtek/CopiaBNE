using System;
using BNE.Web.Code;
using Enumeradores = BNE.BLL.Enumeradores;

namespace BNE.Web.UserControls.Forms.SalaAdministrador
{
    public partial class MenuSalaAdministrador : BaseUserControl
    {
        #region Eventos

        #region lnkEditarCurriculo_Click
        /// <summary>
        /// Evento lnkEditarCurriculo_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkEditarCurriculo_Click(object sender, EventArgs e)
        {
            if (base.IdPerfil.HasValue)
                Redirect("SalaAdministradorEdicaoCV.aspx");
            else
                Redirect("Default.aspx");
        }
        #endregion

        #region lnkFinanceiro_Click
        protected void lnkFinanceiro_Click(object sender, EventArgs e)
        {
            Redirect("SalaAdministradorFinanceiro.aspx");
        }
        #endregion

        #region lnkEditarEmpresas_Click
        protected void lnkEditarEmpresas_Click(object sender, EventArgs e)
        {
            Redirect("SalaAdministradorEmpresas.aspx");
        }
        #endregion

        #region lnkConfiguracoes_Click
        protected void lnkConfiguracoes_Click(object sender, EventArgs e)
        {
            Redirect("SalaAdministradorConfiguracoes.aspx");
        }
        #endregion

        #region lnkEditarVagas_Click
        protected void lnkEditarVagas_Click(object sender, EventArgs e)
        {
            Redirect("SalaAdministradorVagas.aspx");
        }
        #endregion

        #region lnkRelatorios_Click
        protected void lnkRelatorios_Click(object sender, EventArgs e)
        {
            Redirect("SalaAdministradorRelatorios.aspx");
        }
        #endregion

        #endregion

    }
}