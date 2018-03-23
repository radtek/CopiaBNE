using System;
using BNE.BLL;
using BNE.BLL.Integracoes.SalarioBR;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using BNE.Web.Master;

namespace BNE.Web
{
    public partial class RelatorioSalarialMercado : BasePage
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
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                Inicializar();

            var master = (Principal)Page.Master;

            if (master != null)
                master.LoginEfetuadoSucesso += master_LoginEfetuadoSucesso;
        }
        #endregion

        #region Inicializar
        private void Inicializar()
        {
            if (base.IdCurriculo.HasValue)
                CarregarRSM();
            else
                ExibirLogin();
        }
        #endregion

        #region CarregarRSM
        private void CarregarRSM()
        {
            try
            {
                if (new Curriculo(base.IdCurriculo.Value).VIP())
                {
                    var salarioBrReports = new SalarioBrReports.SalarioBrReportsClient();
                    var objDadosRSM = DadosRSM.RecuperarRSM(new Curriculo(base.IdCurriculo.Value));
                    var retornoSbrReports = salarioBrReports.ObtemPreviewRSM(objDadosRSM.IdentificadorFuncaoPretendida, objDadosRSM.SiglaEstado, objDadosRSM.IdentificadorNivel, objDadosRSM.ValorPretensaoSalarial);
                    litConteudo.Text = retornoSbrReports;
                    RSM = retornoSbrReports;
                }
                else
                    ModalVendaRSM.Inicializar();
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
                ExibirMensagem("Ocorreu um erro ao gerar o RSM", TipoMensagem.Erro);
            }
        }
        #endregion

        #region master_LoginEfetuadoSucesso
        protected void master_LoginEfetuadoSucesso()
        {
            CarregarRSM();
            upRSM.Update();
        }
        #endregion

        #region bntGerarPdf_Click
        protected void bntGerarPdf_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(RSM))
            {
                var pdf = BLL.Custom.PDF.RecuperarPDFUsandoAbc(RSM);

                Response.ContentType = "Application/pdf";
                Response.AppendHeader("Content-Disposition", "attachment; filename=Relatorio_Salarial_Mercado.pdf");
                Response.OutputStream.Write(pdf, 0, pdf.Length);
                Response.End();
            }
        }
        #endregion

    }
}