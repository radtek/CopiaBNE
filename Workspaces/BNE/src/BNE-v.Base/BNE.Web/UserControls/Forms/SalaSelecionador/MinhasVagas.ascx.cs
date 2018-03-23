using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using BNE.BLL;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using Resources;
using Telerik.Web.UI;
using Enumeradores = BNE.BLL.Enumeradores;

namespace BNE.Web.UserControls.Forms.SalaSelecionador
{
    public partial class MinhasVagas : BaseUserControl
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

        #region Idfs_Vaga - Variável 1
        /// <summary>
        /// Propriedade que armazena e recupera o ID
        /// </summary>
        protected List<int> IdfsVaga
        {
            get
            {
                return (List<int>)(ViewState[Chave.Temporaria.Variavel1.ToString()]);
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Variavel1.ToString(), value);
            }
        }
        #endregion

        #region Idfs_Curriculo - Variável 2
        /// <summary>
        /// Propriedade que armazena e recupera o ID
        /// </summary>
        protected List<int> IdfsCurriculo
        {
            get
            {
                return (List<int>)(ViewState[Chave.Temporaria.Variavel2.ToString()]);
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Variavel2.ToString(), value);
            }
        }
        #endregion

        #region Idfs_Vaga_Candidato - Variável 3
        /// <summary>
        /// Propriedade que armazena e recupera o ID
        /// </summary>
        protected List<int> IdfsVagaCandidato
        {
            get
            {
                return (List<int>)(ViewState[Chave.Temporaria.Variavel3.ToString()]);
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Variavel3.ToString(), value);
            }
        }
        #endregion

        #region UrlOrigem - Variável 4
        /// <summary>
        /// </summary>
        public string UrlOrigem
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel4.ToString()] != null)
                    return (ViewState[Chave.Temporaria.Variavel4.ToString()]).ToString();
                return null;
            }
            set
            {
                if (value != null)
                    ViewState.Add(Chave.Temporaria.Variavel4.ToString(), value);
                else
                    ViewState.Remove(Chave.Temporaria.Variavel4.ToString());
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
        }
        #endregion

        #region Delegates

        public delegate void DelegateExcluir(int idVaga);
        public event DelegateExcluir EventExcluir;

        #endregion

        #region GvVagas

        #region gvVagas_PageIndexChanged
        protected void gvVagas_PageIndexChanged(object source, GridPageChangedEventArgs e)
        {
            PageIndex = e.NewPageIndex + 1;
            CarregarGrid();
        }
        #endregion

        #region gvVagas_ItemCommand
        protected void gvVagas_ItemCommand(object source, GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("VisualizarCurriculo"))
                {
                    int idVaga = Convert.ToInt32(gvVagas.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Vaga"]);

                    base.UrlDestino.Value = "SalaSelecionadorVagasAnunciadas.aspx";
                    Session.Add(Chave.Temporaria.Variavel7.ToString(), idVaga);
                    Redirect(GetRouteUrl(Enumeradores.RouteCollection.PesquisaCurriculo.ToString(), null));
                }
                else if (e.CommandName.Equals("VisualizarCurriculosNoPerfil"))
                {
                    int idVaga = Convert.ToInt32(gvVagas.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Vaga"]);

                    base.UrlDestino.Value = "SalaSelecionadorVagasAnunciadas.aspx";
                    Session.Add(Chave.Temporaria.Variavel7.ToString(), idVaga);
                    Session.Add(Chave.Temporaria.Variavel11.ToString(), true);
                    Redirect(GetRouteUrl(Enumeradores.RouteCollection.PesquisaCurriculo.ToString(), null));
                }
                else if (e.CommandName.Equals("VisualizarCurriculosNaoLidos"))
                {
                    int idVaga = Convert.ToInt32(gvVagas.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Vaga"]);

                    base.UrlDestino.Value = "SalaSelecionadorVagasAnunciadas.aspx";
                    Session.Add(Chave.Temporaria.Variavel7.ToString(), idVaga);
                    Session.Add(Chave.Temporaria.Variavel8.ToString(), true);
                    Redirect(GetRouteUrl(Enumeradores.RouteCollection.PesquisaCurriculo.ToString(), null));
                }
                else if (e.CommandName.Equals("EditarVaga"))
                {
                    int idVaga = Convert.ToInt32(gvVagas.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Vaga"]);

                    base.UrlDestino.Value = "SalaSelecionadorVagasAnunciadas.aspx";
                    Session.Add(Chave.Temporaria.Variavel2.ToString(), idVaga);
                    Redirect(Page.GetRouteUrl(Enumeradores.RouteCollection.AnunciarVaga.ToString(), null));
                }
                else if (e.CommandName.Equals("ArquivarVaga"))
                {
                    int idVaga = Convert.ToInt32(gvVagas.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Vaga"]);

                    Vaga objVaga = Vaga.LoadObject(idVaga);
                    objVaga.ArquivarVaga();

                    CarregarComboFuncoes();
                    CarregarGrid();
                }
                else if (e.CommandName.Equals("AtivarVaga"))
                {
                    int idVaga = Convert.ToInt32(gvVagas.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Vaga"]);

                    Vaga objVaga = Vaga.LoadObject(idVaga);
                    objVaga.AtivarVaga();

                    CarregarGrid();
                    CarregarComboFuncoes();
                }
                else if (e.CommandName.Equals("AnuncioMassa"))
                {
                    var idVaga = Convert.ToInt32(gvVagas.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Vaga"]);

                    PlanoQuantidade objPlanoQuantidade;
                    if (PlanoQuantidade.CarregarPlanoAtualVigente(base.IdFilial.Value, out objPlanoQuantidade))
                    {
                        var objVaga = Vaga.LoadObject(idVaga);
                        objVaga.AnunciarEmMassa();
                        ucModalConfirmacao.PreencherCampos("Divulgação em Massa", MensagemAviso._24026, false, "OK");
                        ucModalConfirmacao.MostrarModal();
                    }
                    else
                        Redirect(GetRouteUrl(Enumeradores.RouteCollection.ProdutoCIA.ToString(), null));
                }
                else if (e.CommandName.Equals("ExcluirVaga"))
                {
                    int idVaga = Convert.ToInt32(gvVagas.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Vaga"]);

                    if (EventExcluir != null)
                        EventExcluir(idVaga);
                }
                else if (e.CommandName.Equals("ClonarVaga"))
                {
                    int idVaga = Convert.ToInt32(gvVagas.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Vaga"]);

                    base.UrlDestino.Value = "SalaSelecionadorVagasAnunciadas.aspx";
                    Session.Add(Chave.Temporaria.Variavel2.ToString(), idVaga); //Id da vaga
                    Session.Add(Chave.Temporaria.Variavel8.ToString(), true); //BoolClonarVaga
                    Redirect(Page.GetRouteUrl(Enumeradores.RouteCollection.AnunciarVaga.ToString(), null));
                }
            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region gvVagas_ItemDataBound
        protected void gvVagas_ItemDataBound(object sender, GridItemEventArgs e)
        {
            var item = e.Item as GridGroupHeaderItem;
            if (item != null)
            {
                var groupDataRow = (DataRowView)e.Item.DataItem;

                string dataCellText = groupDataRow["FlgVagaArquivada"].Equals(true) ? "Vagas Inativas" : "Vagas Ativas";

                item.DataCell.Text = dataCellText;
            }
        }

        #endregion

        #region gvVagas_ColumnCreated
        protected void gvVagas_ColumnCreated(object sender, GridColumnCreatedEventArgs e)
        {
            if (e.Column is GridGroupSplitterColumn)
            {
                e.Column.HeaderStyle.Width = Unit.Pixel(0);
                e.Column.HeaderStyle.Font.Size = FontUnit.Point(0);
                e.Column.ItemStyle.Width = Unit.Pixel(0);
                e.Column.ItemStyle.Font.Size = FontUnit.Point(0);
                e.Column.Resizable = false;
                e.Column.Visible = false;
            }
        }
        #endregion

        #region gvVagas_ItemCreated
        protected void gvVagas_ItemCreated(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridGroupHeaderItem)
            {
                (e.Item as GridGroupHeaderItem).Cells[0].Controls.Clear();
                (e.Item as GridGroupHeaderItem).Cells[0].Visible = false;
            }
        }
        #endregion

        #endregion

        #region rcbStatus_SelectedIndexChanged
        protected void rcbStatus_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            CarregarComboFuncoes();
        }
        #endregion

        #region btnNovaVaga_Click
        protected void btnNovaVaga_Click(object sender, EventArgs e)
        {
            base.UrlDestino.Value = "SalaSelecionadorVagasAnunciadas.aspx";
            Redirect(Page.GetRouteUrl(Enumeradores.RouteCollection.AnunciarVaga.ToString(), null));
        }
        #endregion

        #region btnFiltrarVaga_Click
        protected void btnFiltrarVaga_Click(object sender, EventArgs e)
        {
            PageIndex = 1;

            CarregarGrid();
        }
        #endregion

        #region btnVoltar_Click
        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            Redirect(base.STC.Value ? "SiteTrabalheConoscoMenu.aspx" : "SalaSelecionador.aspx");
        }

        #endregion

        #endregion

        #region Métodos

        #region Inicializar
        private void Inicializar()
        {
            if (Request.UrlReferrer != null)
                UrlOrigem = Request.UrlReferrer.AbsoluteUri;

            CarregarComboStatus();
            CarregarComboFuncoes();

            gvVagas.GroupingEnabled = true;
            gvVagas.PageSize = Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.QuantidadeItensPorPaginaSalaSelecionador));

            PageIndex = 1;

            CarregarGrid();
        }
        #endregion

        #region CarregarGrid
        public void CarregarGrid()
        {
            //ajustando quais vagas devem aparecer.
            bool? vagasAnunciadas = null; //NULL busca todas as vagas
            if (rcbStatus.SelectedValue.Equals("1")) //Se for Vagas Ativas
                vagasAnunciadas = true;
            else if (rcbStatus.SelectedValue.Equals("2")) //Se for Vagas Inativas
                vagasAnunciadas = false;

            var objFilial = new Filial(IdFilial.Value);
            var empresaAssociacao = objFilial.EmpresaAssociacao();
            int? usuarioFilialPerfil = empresaAssociacao || ccFiltrarApenasMinhasVagas.Checked ? base.IdUsuarioFilialPerfilLogadoEmpresa.Value : (int?) null;

            int totalRegistros;
            DataTable dt = Vaga.ListarVagasFilial(base.IdFilial.Value, base.STC.Value ? base.IdOrigem.Value : (int?)null, PageIndex, gvVagas.PageSize, vagasAnunciadas, usuarioFilialPerfil, RecuperarListFuncoes(), out totalRegistros);

            UIHelper.CarregarRadGrid(gvVagas, dt, totalRegistros);

            if (vagasAnunciadas.HasValue)
            {
                if ((bool)vagasAnunciadas)
                    lblInformacaoVagas.Text = String.Format("{0} {1}", totalRegistros, "vaga(s) ativas");
                else
                    lblInformacaoVagas.Text = String.Format("{0} {1}", totalRegistros, "vaga(s) inativas");
            }
            else
            {
                int ativas = Filial.RecuperarQuantidadeVagasAnunciadas(base.IdFilial.Value, usuarioFilialPerfil);
                int inativas = Filial.RecuperarQuantidadeVagasArquivadas(base.IdFilial.Value, usuarioFilialPerfil);

                lblInformacaoVagas.Text = String.Format("{0} {1} e {2} {3}", ativas, "vaga(s) ativas", inativas, "vaga(s) inativas");
            }

            upGvVagas.Update();
        }

        #endregion

        #region CarregarComboFuncoes
        public void CarregarComboFuncoes()
        {
            //ajustando quais vagas devem aparecer.
            bool? vagasAnunciadas = null; //NULL busca todas as vagas
            if (rcbStatus.SelectedValue.Equals("1")) //Se for Vagas Ativas
                vagasAnunciadas = true;
            else if (rcbStatus.SelectedValue.Equals("2")) //Se for Vagas Inativas
                vagasAnunciadas = false;

            UIHelper.CarregarRadComboBox(ccFiltrarVagasFuncao, Vaga.ListarFuncoesVagasFilial(base.IdFilial.Value, vagasAnunciadas), "Idf_Funcao", "Des_Funcao");
        }
        #endregion

        #region CarregarComboStatus
        public void CarregarComboStatus()
        {
            var dicionario = new Dictionary<string, string>
                {
                    {"1", "Vagas Ativas"},
                    {"2", "Vagas Inativas"},
                    {"3", "Vagas Ativas e Inativas"}
                };

            UIHelper.CarregarRadComboBox(rcbStatus, dicionario);

            rcbStatus.SelectedValue = "3";
        }
        #endregion

        #region ExcluirVaga
        public void ExcluirVaga(int idVaga)
        {
            Vaga.InativarVaga(idVaga);
        }
        #endregion

        #region RecuperarListFuncoes
        private List<int> RecuperarListFuncoes()
        {
            return ccFiltrarVagasFuncao.GetCheckedItems().Select(item => Convert.ToInt32(item.Value)).ToList();
        }
        #endregion

        #endregion

    }
}