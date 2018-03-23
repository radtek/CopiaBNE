using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using BNE.BLL;
using BNE.BLL.Custom;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using BNE.Web.Resources;
using Employer.Componentes.UI.Web;
using Resources;
using Telerik.Web.UI;
using ImageButton = System.Web.UI.WebControls.ImageButton;
using Enumeradores = BNE.BLL.Enumeradores;

namespace BNE.Web
{
    public partial class SalaSelecionadorMeusCurriculos : BasePage
    {

        #region Propriedades

        #region Permissoes - Variável Permissoes
        /// <summary>
        /// Propriedade que armazena e recupera o IdPesquisaCurriculo
        /// </summary>
        protected List<int> Permissoes
        {
            get
            {
                return (List<int>)ViewState[Chave.Temporaria.Permissoes.ToString()];
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Permissoes.ToString(), value);
            }
        }
        #endregion

        #endregion

        #region Eventos

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                AjustarPermissoes();
                CarregarGrid();
                CarregarComboFuncoes();
            }
            InicializarBarraBusca(TipoBuscaMaster.Curriculo, false, GetType().ToString());
        }
        #endregion


        #region btnVoltar_Click
        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            Redirect(base.STC.Value ? "SiteTrabalheConoscoMenu.aspx" : "SalaSelecionador.aspx");
        }
        #endregion

        #region Grid

        #region gvResultadoPesquisa_PageIndexChanged
        protected void gvResultadoPesquisa_PageIndexChanged(object source, GridPageChangedEventArgs e)
        {
            gvResultadoPesquisa.CurrentPageIndex = e.NewPageIndex;
            CarregarGrid();
            upMeusCurriculos.Update();
        }
        #endregion

        #region gvResultadoPesquisa_ColumnCreated
        protected void gvResultadoPesquisa_ColumnCreated(object sender, GridColumnCreatedEventArgs e)
        {
            if (e.Column is GridGroupSplitterColumn)
            {
                e.Column.HeaderStyle.Width = Unit.Pixel(0);
                e.Column.HeaderStyle.Font.Size = FontUnit.Point(0);
                e.Column.ItemStyle.Width = Unit.Pixel(0);
                e.Column.ItemStyle.Font.Size = FontUnit.Point(0);
                e.Column.Resizable = false;
            }
        }
        #endregion

        #region gvResultadoPesquisa_ItemCreated
        protected void gvResultadoPesquisa_ItemCreated(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridGroupHeaderItem)
            {
                (e.Item as GridGroupHeaderItem).Cells[0].Controls.Clear();
            }
        }
        #endregion

        #region gvResultadoPesquisa_ItemDataBound
        protected void gvResultadoPesquisa_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                string id = gvResultadoPesquisa.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Curriculo"].ToString();

                var imgAvaliacao = (LinkButton)e.Item.FindControl("imgAvaliacao");



                //Ajustando bailão saiba mais
                var bsm = (BNE.Componentes.BalaoSaibaMais)e.Item.FindControl("bsmAvaliacao");
                bsm.ID = imgAvaliacao.ClientID;

                //Ajusando imagem de avaliação
                if (!String.IsNullOrEmpty(imgAvaliacao.CommandArgument))
                {
                    if (Convert.ToInt32(imgAvaliacao.CommandArgument).Equals(Enumeradores.CurriculoClassificacao.AvaliacaoNegativa.GetHashCode()))
                        imgAvaliacao.CssClass = "fa fa-frown-o";
                    else if (Convert.ToInt32(imgAvaliacao.CommandArgument).Equals(Enumeradores.CurriculoClassificacao.AvaliacaoPositiva.GetHashCode()))
                        imgAvaliacao.CssClass = "fa fa-smile-o";
                    else if (Convert.ToInt32(imgAvaliacao.CommandArgument).Equals(Enumeradores.CurriculoClassificacao.AvaliacaoNeutra.GetHashCode()))
                        imgAvaliacao.CssClass = "fa fa-meh-o";
                }
            }
        }
        #endregion

        #endregion

        #region btnFiltrarCurriculos_Click
        protected void btnFiltrarCurriculos_Click(object sender, EventArgs e)
        {
            gvResultadoPesquisa.CurrentPageIndex = 0;

            CarregarGrid();
        }
        #endregion

        #endregion

        #region Métodos

        #region CarregarGrid
        private void CarregarGrid()
        {
            int totalRegistros;
            bool empresaAssociacao = false;

            if (base.IdFilial.HasValue)
            {
                var objFilial = Filial.LoadObject(base.IdFilial.Value);

                empresaAssociacao = objFilial.EmpresaAssociacao();
            }

            UIHelper.CarregarRadGrid(gvResultadoPesquisa, BLL.PesquisaCurriculo.BuscaCurriculoQuePertenceAOrigem(gvResultadoPesquisa.PageSize, gvResultadoPesquisa.CurrentPageIndex, base.IdOrigem.Value, base.IdFilial.Value, empresaAssociacao ? base.IdUsuarioFilialPerfilLogadoEmpresa.Value : (int?)null, RecuperarListFuncoes(), txtTecnicoGraduacao.Text, out totalRegistros), totalRegistros);
        }
        #endregion

        #region AjustarPermissoes
        /// <summary>
        /// Método responsável por ajustar as permissões da tela de acordo com o susuário filial perfil logado.
        /// </summary>
        private void AjustarPermissoes()
        {
            if (base.IdUsuarioFilialPerfilLogadoEmpresa.HasValue)
            {
                Permissoes = UsuarioFilialPerfil.CarregarPermissoes(base.IdUsuarioFilialPerfilLogadoEmpresa.Value, BLL.Enumeradores.CategoriaPermissao.SalaSelecionadora);

                if (!Permissoes.Contains((int)BLL.Enumeradores.Permissoes.SalaSelecionadora.AcessarTelaSalaSelecionadora))
                {
                    Session.Add(Chave.Temporaria.MensagemPermissao.ToString(), MensagemAviso._300034);
                    Redirect(Configuracao.UrlAvisoAcessoNegado);
                }
            }
            else
                Redirect(GetRouteUrl(Enumeradores.RouteCollection.LoginComercialEmpresa.ToString(), null));
        }
        #endregion

        #region RetornarFuncao
        protected string RetornarFuncao(string funcao)
        {
            if (string.IsNullOrEmpty(funcao))
                return string.Empty;

            var funcoes = funcao.Split(';').Where(f => !string.IsNullOrEmpty(f));

            funcao = string.Join(";<br>", funcoes.Select(f => f.Trim()).ToArray());

            return funcao;
        }
        #endregion

        #region RetornarURL
        protected string RetornarURL(string funcao, string nomeCidade, string siglaEstado, int identificadorCurriculo)
        {
            string nomeFuncao = string.Empty;

            if (!string.IsNullOrEmpty(funcao))
            {
                var funcoes = funcao.Split(';').Where(f => !string.IsNullOrEmpty(f));
                nomeFuncao = funcoes.ElementAt(0);
            }

            return SitemapHelper.MontarUrlVisualizacaoCurriculo(nomeFuncao, nomeCidade, siglaEstado, identificadorCurriculo);
        }
        #endregion

        #region CarregarComboFuncoes
        private void CarregarComboFuncoes()
        {
            UIHelper.CarregarRadComboBox(ccFiltrarCurriculosFuncao, OrigemFilial.ListarFuncoesCurriculosOrigem(base.IdOrigem.Value), "Idf_Funcao", "Des_Funcao");
        }
        #endregion

        #region RecuperarListFuncoes
        private List<int> RecuperarListFuncoes()
        {
            return ccFiltrarCurriculosFuncao.GetCheckedItems().Select(item => Convert.ToInt32(item.Value)).ToList();
        }
        #endregion

        #endregion

    }
}