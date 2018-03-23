using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BNE.BLL;
using BNE.Web.Code;
using System.Reflection;
using System.Data;

namespace BNE.Web.UserControls.Forms.SalaAdministrador
{
    /// <summary>
    /// UserControl para mostrar a opção de gerar o relatório de Novas Empresas Cadastradas
    /// </summary>
    public partial class NovasEmpresas : BaseUserControl
    {
        #region Page_Load
        /// <summary>
        /// Carregamento da Página, inicia os campos de data
        /// A Data Final é a Data Atual menos 30 dias, e esta é a regra para
        /// a data máxima que pode ser escolhida em qualquer dos campos.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            txtDataFinal.DataMaxima = DateTime.Now.AddDays(-30);
            txtDataInicial.DataMaxima = txtDataFinal.DataMaxima;

            if (string.IsNullOrEmpty(txtDataFinal.Valor))
            {
                txtDataFinal.ValorDatetime = txtDataFinal.DataMaxima;
            }
            if (string.IsNullOrEmpty(txtDataInicial.Valor))
            {
                txtDataInicial.ValorDatetime = (txtDataInicial.DataMaxima).AddDays(-30);
            }
        }
        #endregion

        #region btnGerar_Click1
        /// <summary>
        /// Evento Click do Botão Gerar
        /// Abre o Destino de geração do Excel
        /// </summary>
        /// <param name="sender">Botão</param>
        /// <param name="e">Evento</param>
        protected void btnGerar_Click1(object sender, EventArgs e)
        {
            ///dataIni = Data Inicio do período para o relatório
            ///dataFim = Data Limite do período para o relatório
            ///nomeRel = O Nome do Relatório e controle de ação em HtmlExport.aspx.cs

            if (verificaDatas())
            {

                string dataIni = txtDataInicial.ValorDatetime.ToString();
                string dataFim = txtDataFinal.ValorDatetime.ToString();
                string nomeRel = "NovasEmpresas";
                ScriptManager.RegisterStartupScript(this, GetType(), "AbrirPopup", string.Format("AbrirPopup('http://{0}/HtmlExport.aspx?dtIn={1}&dtFn={2}&rel={3}', 600, 800);", Request.ServerVariables["HTTP_HOST"], dataIni, dataFim, nomeRel), true);
            }
        }
        #endregion

        protected void txtDt_ValorAlterado(object sender, EventArgs e)
        {
            verificaDatas();
        }

        private bool verificaDatas()
        {
            if (!string.IsNullOrEmpty(txtDataInicial.Valor) && !string.IsNullOrEmpty(txtDataFinal.Valor))
            {
                if (txtDataInicial.ValorDatetime > txtDataFinal.ValorDatetime)
                {
                    base.ExibirMensagem("A Data Inicial não pode ser maior que a Data Final!", Code.Enumeradores.TipoMensagem.Erro);
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                base.ExibirMensagem("As Datas devem estar preenchidas!", Code.Enumeradores.TipoMensagem.Erro);
                return false;
            }
        }

    }
}