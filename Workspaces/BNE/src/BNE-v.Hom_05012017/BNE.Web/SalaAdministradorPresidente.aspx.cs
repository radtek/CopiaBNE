using System;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;

namespace BNE.Web
{
    public partial class SalaAdministradorPresidente : BasePage
    {
        #region Eventos
        
        #region Page_Load

        protected void Page_Load(object sender, EventArgs e)
        {
             Inicializar();
        }

        #endregion

        #region btnAgradecimento_Click
        protected void btnAgradecimento_Click(object sender, EventArgs e)
        {
            liAgradecimentoSel.Visible = true;
            liAgradecimento.Visible = false;
            liRespostasAutomaticasSel.Visible = false;
            liRespostasAutomaticas.Visible = true;
            MostrarAgradecimentoDefault();
        }  
        #endregion

        #region btnRespostasAutomaticas_Click
        protected void btnRespostasAutomaticas_Click(object sender, EventArgs e)
        {
            liAgradecimentoSel.Visible = false;
            liAgradecimento.Visible = true;
            liRespostasAutomaticasSel.Visible = true;
            liRespostasAutomaticas.Visible = false;
            MostrarRespostasAutomaticasDefault();
        }   
        #endregion

        #region btnVoltar_Click
        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            if (ucPresidenteAgradecimento.FindControl("upAgradecimento").Visible || ucPresidenteRespostaAutomatica.FindControl("upRespostaAutomatica").Visible)
                Redirect("SalaAdministrador.aspx");
            else
            {
                if (ucPresidenteAgradecimento.Visible)
                    MostrarAgradecimentoDefault();
                else
                    MostrarRespostasAutomaticasDefault();
            }
        }
        #endregion

        #endregion

        #region Metodos

        #region Inicializar
        public void Inicializar()
        {
            AjustarTituloTela("Presidente");
            InicializarBarraBusca(TipoBuscaMaster.Vaga, false, "Presidente");
        }
        #endregion

        #region MostrarAgradecimentoDefault
        private void MostrarAgradecimentoDefault()
        {
            ucPresidenteAgradecimento.Visible = true;
            ucPresidenteRespostaAutomatica.Visible = false;
            ucPresidenteAgradecimento.FindControl("upAgradecimento").Visible = true;
            ucPresidenteAgradecimento.FindControl("upEditarAgradecimento").Visible = false;
            ucPresidenteAgradecimento.FindControl("upNovoAgradecimento").Visible = false;
            ucPresidenteAgradecimento.CarregarGrid();
            upConteudo.Update();
        }
        #endregion

        #region MostrarRespostasAutomaticasDefault
        private void MostrarRespostasAutomaticasDefault()
        {
            ucPresidenteAgradecimento.Visible = false;
            ucPresidenteRespostaAutomatica.Visible = true;

            ucPresidenteRespostaAutomatica.FindControl("upRespostaAutomatica").Visible = true;
            ucPresidenteRespostaAutomatica.FindControl("upEditarRespostaAutomatica").Visible = false;
            ucPresidenteRespostaAutomatica.FindControl("upNovaRespostaAutomatica").Visible = false;
            ucPresidenteRespostaAutomatica.FindControl("upTodasRespostaAutomatica").Visible = false;
            ucPresidenteRespostaAutomatica.CarregarGrid();
            upConteudo.Update();
        }
        #endregion

        #endregion
    }
}