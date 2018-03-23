using System;
using System.Web.UI.WebControls;
using BNE.BLL;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using Telerik.Web.UI;
using Enumeradores = BNE.BLL.Enumeradores;
using System.Web.UI.HtmlControls;

namespace BNE.Web.UserControls.Forms.SalaVip
{
    public partial class EscolherEmpresa : BaseUserControl
    {

        #region Propriedades

        #region PageIndex - PageIndex
        /// <summary>
        /// Propriedade que armazena e recupera o ID
        /// </summary>
        protected int PageIndex
        {
            get
            {
                if (ViewState[Chave.Temporaria.PageIndex.ToString()] != null)
                    return Int32.Parse(ViewState[Chave.Temporaria.PageIndex.ToString()].ToString());
                
                return 0;
            }
            set
            {
                ViewState.Add(Chave.Temporaria.PageIndex.ToString(), value);
            }
        }
        #endregion

        #region IdIndexManipular - Variável 1
        /// <summary>
        /// Propriedade que armazena e recupera o IdPesquisaVaga
        /// </summary>
        private int? IdIndexManipular
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel1.ToString()] != null)
                    return Int32.Parse(ViewState[Chave.Temporaria.Variavel1.ToString()].ToString());
                
                return null;
            }
            set
            {
                if (value != null)
                    ViewState.Add(Chave.Temporaria.Variavel1.ToString(), value);
                else
                    ViewState.Remove(Chave.Temporaria.Variavel1.ToString());
            }
        }
        #endregion

        #endregion

        #region Eventos

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                Inicializar();

            ucVerDadosEmpresa.Candidatar += ucVerDadosEmpresa_Candidatar;
            ucModalConfirmacao.ModalConfirmada += ucModalConfirmacao_ModalConfirmada;
            ucIndicarEmpresa.Indicar += ucIndicarEmpresa_Indicar;
        }
        #endregion

        #region ucModalConfirmacao_ModalConfirmada
        void ucModalConfirmacao_ModalConfirmada()
        {
            if (IdIndexManipular.HasValue)
                AjustarPanelButtonCandidatarSucesso();
        }
        #endregion

        #region ucIndicarEmpresa_Indicar
        void ucIndicarEmpresa_Indicar()
        {
            ucModalConfirmacao.PreencherCampos("Confirmação de Indicação", "Obrigado pela indicação! <br>Nossa equipe entrará em contato com a empresa para inclusão no BNE. ", String.Empty, false);
            ucModalConfirmacao.MostrarModal();
        }
        #endregion

        #region gvEscolherEmpresa_PageIndexChanged
        protected void gvEscolherEmpresa_PageIndexChanged(object source, GridPageChangedEventArgs e)
        {
            PageIndex = e.NewPageIndex + 1;
            CarregarGrid();
        }
        #endregion

        #region gvEscolherEmpresa_ItemCommand
        protected void gvEscolherEmpresa_ItemCommand(object source, GridCommandEventArgs e)
        {
            if (e.CommandName.Equals("Candidatar"))
            {
                var objCurriculo = new Curriculo(base.IdCurriculo.Value);
                
                if (objCurriculo.VIP())
                {
                    int idFilial = Convert.ToInt32(gvEscolherEmpresa.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Filial"]);
                    IdIndexManipular = e.Item.ItemIndex;

                    ucVerDadosEmpresa.IdFilial = idFilial;
                    ucVerDadosEmpresa.MostrarModal();
                }
                else
                    Redirect(UIHelper.RecuperarCaminhoProdutoVIP(GetRouteUrl(Enumeradores.RouteCollection.ProdutoVIP.ToString(), null), Enumeradores.VantagensVIP.EscolherEmpresa));
            }
        }
        #endregion

        #region btnFiltrar_Click
        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            gvEscolherEmpresa.CurrentPageIndex = 0;
            CarregarGrid();
        }
        #endregion

        #region ucVerDadosEmpresa_Candidatar
        void ucVerDadosEmpresa_Candidatar()
        {
            try
            {
                IntencaoFilial objIntencaoFilial;
                if (!IntencaoFilial.CarregarPorFilialCurriculo(base.IdCurriculo.Value, ucVerDadosEmpresa.IdFilial, out objIntencaoFilial))
                {
                    objIntencaoFilial = new IntencaoFilial
                        {
                            Curriculo = new Curriculo(base.IdCurriculo.Value), 
                            Filial = new Filial(ucVerDadosEmpresa.IdFilial)
                        };
                }
                objIntencaoFilial.FlagInativo = false;
                objIntencaoFilial.Save();
                ucVerDadosEmpresa.FecharModal();
                ucModalConfirmacao.PreencherCampos("Confirmação de Envio", "Notificação enviada com sucesso!", "Havendo interesse a própria empresa fará<br /> contato com você, sem intermediários.", false);
                ucModalConfirmacao.MostrarModal();
            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region btnNaoAcheiEmpresa_Click
        protected void btnNaoAcheiEmpresa_Click(object sender, EventArgs e)
        {
            ucIndicarEmpresa.MostrarModal();
        }
        #endregion

        #endregion

        #region Métodos

        #region Inicializar
        private void Inicializar()
        {
            PageIndex = 1;
            //Carregando a quantidade de itens a ser mostrado em tela
            gvEscolherEmpresa.PageSize = Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.QuantidadeItensPorPaginaSalaVip));

            CarregarGrid();
        }
        #endregion

        #region CarregarGrid
        private void CarregarGrid()
        {
            if (base.IdCurriculo.HasValue)
            {
                int totalRegistros;
                string busca = tbxFiltroBusca.Text;
                UIHelper.CarregarRadGrid(gvEscolherEmpresa, Filial.ListarFilialPorRazaoRamoCidade(busca, base.IdCurriculo.Value, PageIndex, gvEscolherEmpresa.PageSize, out totalRegistros), totalRegistros);
            }
            else
            {
                base.ExibirLogin();
            }
        }
        #endregion

        #region AjustarPanelButtonCandidatarSucesso
        private void AjustarPanelButtonCandidatarSucesso()
        {
            if (IdIndexManipular.HasValue)
            {
                GridDataItem gvDi = gvEscolherEmpresa.Items[IdIndexManipular.Value];

                var ib = (LinkButton)gvDi.FindControl("btiCandidatar");
                ib.Visible = false;
                var i = (HtmlGenericControl)gvDi.FindControl("spanJaEnviei");
                i.Visible = true;
                
                upGvEscolherEmpresa.Update();

                IdIndexManipular = null;
            }
        }
        #endregion
        #endregion

    }
}