using BNE.BLL;
using BNE.BLL.Integracoes.SalarioBR;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BNE.Web
{
    public partial class RelatorioSalarioBr : BasePage
    {
        #region Propriedades
        #region RSM
        public string RSM
        {
            get
            {
                if (Session[Chave.Temporaria.Variavel1.ToString()] != null)
                    return Session[Chave.Temporaria.Variavel1.ToString()].ToString();

                return null;
            }
            set
            {
                Session.Add(Chave.Temporaria.Variavel1.ToString(), value);
            }
        }
        #endregion
        #endregion

        #region Page_Load
        [ValidateInput(false)]
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    DivFaltaParametros.Visible = false;
                    DivBannerSalarioBR.Visible = false;
                    DivSemPlano.Visible = false;

                    if (!ValidarParametrosObrigatorios())
                    {
                        //Caso esteja faltando parametros obrigatórios para exibir o relatório, exibe mensagem
                        DivFaltaParametros.Visible = true;
                        return;
                    }

                    int idFuncao = Convert.ToInt32(Request.QueryString["idFuncao"].ToString());
                    string siglaEstado = Request.QueryString["SiglaEstado"].ToString();
                    int idPlanoAdquirido = Convert.ToInt32(Request.QueryString["IdPlanoAdquirido"].ToString());

                    //verificar o plano adquirido
                    PlanoAdquirido oPlanoAdquirido = PlanoAdquirido.LoadObject(idPlanoAdquirido);
                    if (oPlanoAdquirido.DataFimPlano == null || oPlanoAdquirido.DataFimPlano > DateTime.Now)
                    {
                        //se o plano estiver ok, permite visualização do relatório
                        hdnSiglaEstado.Value = siglaEstado;
                        hdnIdFuncao.Value = idFuncao.ToString();
                        CarregarRSM(idFuncao, siglaEstado);

                        DivBannerSalarioBR.Visible = true;
                        btnGerarPdf.Visible = true;
                    }
                    else
                    {
                        //Mostra div de plano inexistente ou vencido
                        DivSemPlano.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex, "Erro ao carregar tela de relatório salario br empresas");
                ExibirMensagem("Ocorreu um erro ao gerar o relatório.", TipoMensagem.Erro);
            }
        }
        #endregion

        #region ValidarParametrosObrigatorios
        private bool ValidarParametrosObrigatorios()
        {
            if (Request.QueryString["idFuncao"] == null)
                return false;

            if (Request.QueryString["SiglaEstado"] == null)
                return false;

            if (Request.QueryString["IdPlanoAdquirido"] == null)
                return false;

            return true;
        }
        #endregion

        #region CarregarRSM
        private void CarregarRSM(int idFuncao, string SiglaEstado)
        {
            try
            {
                var salarioBrReports = new SalarioBrReports.SalarioBrReportsClient();
                var retornoSbrReports = salarioBrReports.ObtemPreviewRSM(idFuncao, SiglaEstado, 0, 0);
                litConteudo.Text = retornoSbrReports;
                RSM = retornoSbrReports;
            }
            catch(Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex, "Erro ao carregar relatório salario br empresas");
                throw;
            }

        }
        #endregion

        #region bntGerarPdf_Clic
        [ValidateInput(false)]
        protected void bntGerarPdf_Click(object sender, EventArgs e)
        {
            try
            {

                if (ValidaGraficos())
                {
                    var salarioBrReports = new SalarioBrReports.SalarioBrReportsClient();
                    var pdf = salarioBrReports.ObterRelatorioSalarialMercado(Convert.ToInt32(hdnIdFuncao.Value), hdnSiglaEstado.Value, 0, 0, TratarSVG(svgValores.Value), TratarSVG(svgGenero.Value), TratarSVG(svgFaixaEtaria.Value), TratarSVG(svgExperiencias.Value));
                    Response.ContentType = "Application/pdf";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=Relatorio_Salarial_Mercado.pdf");
                    Response.OutputStream.Write(pdf, 0, pdf.Length);
                    Response.End();
                }
                else
                {
                    ExibirMensagem("Dados inválidos para gerar o PDF. Verifique.", TipoMensagem.Aviso);
                }
            }
            catch(Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex, "Falha ao gerar PDF do relatório salarial para empresas.");
            }
        }
        #endregion

        #region TratarSVG
        protected string TratarSVG(string svg)
        {
            string retorno = svg;
            retorno = Uri.UnescapeDataString(retorno);
            retorno = retorno.Replace("%FA", "ú");
            retorno = retorno.Replace("%EA", "ê");
            retorno = retorno.Replace("%E9", "é");
            retorno = retorno.Replace("%E1", "á");

            return retorno;
        }
        #endregion

        #region ValidaGraficos
        protected bool ValidaGraficos()
        {
            if (string.IsNullOrEmpty(svgValores.Value) || string.IsNullOrEmpty(svgGenero.Value) || string.IsNullOrEmpty(svgFaixaEtaria.Value) || string.IsNullOrEmpty(svgExperiencias.Value))
                return false;

            return true;
        }
        #endregion

    }
}