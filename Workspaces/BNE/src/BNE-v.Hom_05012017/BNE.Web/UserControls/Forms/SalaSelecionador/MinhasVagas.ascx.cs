using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using BNE.BLL;
using BNE.BLL.Common;
using BNE.BLL.Custom;
using BNE.Solr;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using Resources;
using Telerik.Web.UI;
using Enumeradores = BNE.BLL.Enumeradores;
using BNE.Web.Code.ViewStateObjects;

namespace BNE.Web.UserControls.Forms.SalaSelecionador
{
    public partial class MinhasVagas : BaseUserControl
    {

        #region Propriedades

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

        #region UrlVaga - Variável 5
        /// <summary>
        /// </summary>
        public string UrlVaga
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel5.ToString()] != null)
                    return (ViewState[Chave.Temporaria.Variavel5.ToString()]).ToString();
                return null;
            }
            set
            {
                if (value != null)
                    ViewState.Add(Chave.Temporaria.Variavel5.ToString(), value);
                else
                    ViewState.Remove(Chave.Temporaria.Variavel5.ToString());
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
            gvVagas.CurrentPageIndex = e.NewPageIndex;
            CarregarGridVagas();
        }
        #endregion

        #region gvVagas_ItemCommand
        protected void gvVagas_ItemCommand(object source, GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName != "Page")
                {
                    var objResultadoPesquisaCurriculo = new ResultadoPesquisaCurriculo(new ResultadoPesquisaCurriculoVaga());

                    int idVaga = Convert.ToInt32(gvVagas.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Vaga"]);

                    //Setando o id da vaga no objeto de redirect para a pesquisa de currículo
                    objResultadoPesquisaCurriculo.Vaga.IdVaga = idVaga;

                    if (e.CommandName.Equals("VisualizarCurriculo"))
                    {
                        base.UrlDestino.Value = "SalaSelecionadorVagasAnunciadas.aspx";

                        Session.Add(Chave.Temporaria.ViewStateObject_ResultadoPesquisaCurriculo.ToString(), objResultadoPesquisaCurriculo);

                        Redirect(GetRouteUrl(Enumeradores.RouteCollection.PesquisaCurriculo.ToString(), null));
                    }
                    else if (e.CommandName.Equals("VisualizarCurriculosNoPerfil"))
                    {
                        objResultadoPesquisaCurriculo.Vaga.BancoCurriculo = true;

                        Session.Add(Chave.Temporaria.ViewStateObject_ResultadoPesquisaCurriculo.ToString(), objResultadoPesquisaCurriculo);

                        Redirect(GetRouteUrl(Enumeradores.RouteCollection.PesquisaCurriculo.ToString(), null));
                    }
                    else if (e.CommandName.Equals("VisualizarCurriculosNaoLidos"))
                    {
                        base.UrlDestino.Value = "SalaSelecionadorVagasAnunciadas.aspx";

                        objResultadoPesquisaCurriculo.Vaga.InscritosNaoLidos = true;

                        Session.Add(Chave.Temporaria.ViewStateObject_ResultadoPesquisaCurriculo.ToString(), objResultadoPesquisaCurriculo);

                        Redirect(GetRouteUrl(Enumeradores.RouteCollection.PesquisaCurriculo.ToString(), null));
                    }
                    else if (e.CommandName.Equals("EditarVaga"))
                    {
                        base.UrlDestino.Value = "SalaSelecionadorVagasAnunciadas.aspx";
                        Session.Add(Chave.Temporaria.Variavel2.ToString(), idVaga);
                        Redirect(Page.GetRouteUrl(Enumeradores.RouteCollection.AnunciarVaga.ToString(), null));
                    }
                    else if (e.CommandName.Equals("ArquivarVaga"))
                    {
                        Vaga objVaga = Vaga.LoadObject(idVaga);
                        objVaga.ArquivarVaga(base.IdUsuarioFilialPerfilLogadoEmpresa.HasValue ? base.IdUsuarioFilialPerfilLogadoEmpresa.Value : base.IdUsuarioFilialPerfilLogadoUsuarioInterno.Value, null);

                        CarregarComboFuncoes();
                        CarregarGridVagas();
                    }
                    else if (e.CommandName.Equals("AtivarVaga"))
                    {
                        Vaga objVaga = Vaga.LoadObject(idVaga);
                        objVaga.AtivarVaga(base.IdUsuarioFilialPerfilLogadoEmpresa.HasValue ? base.IdUsuarioFilialPerfilLogadoEmpresa.Value : base.IdUsuarioFilialPerfilLogadoUsuarioInterno.Value, null);

                        CarregarGridVagas();
                        CarregarComboFuncoes();
                    }
                    else if (e.CommandName.Equals("AnuncioMassa"))
                    {
                        if (new Filial(base.IdFilial.Value).PossuiPlanoAtivo())
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
                        if (EventExcluir != null)
                            EventExcluir(idVaga);
                    }
                    else if (e.CommandName.Equals("ClonarVaga"))
                    {
                        base.UrlDestino.Value = "SalaSelecionadorVagasAnunciadas.aspx";
                        Session.Add(Chave.Temporaria.Variavel2.ToString(), idVaga); //Id da vaga
                        Session.Add(Chave.Temporaria.Variavel8.ToString(), true); //ClonarVaga
                        Redirect(Page.GetRouteUrl(Enumeradores.RouteCollection.AnunciarVaga.ToString(), null));
                    }
                    else if (e.CommandName.Equals("VisualizarVaga"))
                    {
                        string funcao = gvVagas.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Des_Funcao"].ToString().Replace("(Estágio)", "");
                        Funcao oFuncao;
                        Funcao.CarregarPorDescricao(funcao, out oFuncao);
                        AreaBNE oArea;
                        oArea = AreaBNE.LoadObject(oFuncao.AreaBNE.IdAreaBNE);
                        string _bebug_link = "";
#if DEBUG
                        _bebug_link = "http://localhost:2500";
#endif
                        string url = String.Format("{0}/vaga-de-emprego-na-area-{1}-em-{2}-{3}/{4}/{5}",
                            _bebug_link, oArea.DescricaoAreaBNE, gvVagas.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Nme_Cidade"].ToString(),
                            gvVagas.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Sig_Estado"].ToString(),
                            funcao, idVaga).Replace("C#","csharp");

                        Redirect(url.NormalizarURL());
                    }
                    else if (e.CommandName.Equals("PublicacaoImediataVaga"))
                    {
                        int idPlano;
                        var objUsuarioFilialPerfil = UsuarioFilialPerfil.LoadObject(base.IdUsuarioFilialPerfilLogadoEmpresa.Value);
                        var url = new Vaga(idVaga).ComprarPublicacaoImediata(objUsuarioFilialPerfil, out idPlano);
                        base.PagamentoIdentificadorPlano.Value = idPlano;

                        Redirect(url);
                    }
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
            var item = e.Item as GridGroupHeaderItem;
            if (item != null)
            {
                item.Cells[0].Controls.Clear();
                item.Cells[0].Visible = false;
            }
        }

        #endregion

        #endregion

        #region gvVagasCampanha

        #region gvVagas_PageIndexChanged
        protected void gvVagasCampanha_PageIndexChanged(object source, GridPageChangedEventArgs e)
        {
            gvVagasCampanha.CurrentPageIndex = e.NewPageIndex;
            CarregarGridVagasCampanha();
        }
        #endregion

        #region gvVagasCampanha_ItemCommand
        protected void gvVagasCampanha_ItemCommand(object source, GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName != "Page")
                {
                    var objResultadoPesquisaCurriculo = new ResultadoPesquisaCurriculo(new ResultadoPesquisaCurriculoVaga()) { Vaga = { Campanha = true } };

                    int idVaga = Convert.ToInt32(gvVagasCampanha.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Vaga"]);

                    //Setando o id da vaga no objeto de redirect para a pesquisa de currículo
                    objResultadoPesquisaCurriculo.Vaga.IdVaga = idVaga;

                    if (e.CommandName.Equals("VisualizarCurriculo"))
                    {
                        base.UrlDestino.Value = "SalaSelecionadorVagasAnunciadas.aspx";

                        Session.Add(Chave.Temporaria.ViewStateObject_ResultadoPesquisaCurriculo.ToString(), objResultadoPesquisaCurriculo);

                        Redirect(GetRouteUrl(Enumeradores.RouteCollection.PesquisaCurriculo.ToString(), null));
                    }
                    else if (e.CommandName.Equals("VisualizarCurriculosNoPerfil"))
                    {
                        objResultadoPesquisaCurriculo.Vaga.BancoCurriculo = true;

                        Session.Add(Chave.Temporaria.ViewStateObject_ResultadoPesquisaCurriculo.ToString(), objResultadoPesquisaCurriculo);

                        Redirect(GetRouteUrl(Enumeradores.RouteCollection.PesquisaCurriculo.ToString(), null));
                    }
                    else if (e.CommandName.Equals("VisualizarCurriculosNaoLidos"))
                    {
                        base.UrlDestino.Value = "SalaSelecionadorVagasAnunciadas.aspx";

                        objResultadoPesquisaCurriculo.Vaga.InscritosNaoLidos = true;

                        Session.Add(Chave.Temporaria.ViewStateObject_ResultadoPesquisaCurriculo.ToString(), objResultadoPesquisaCurriculo);

                        Redirect(GetRouteUrl(Enumeradores.RouteCollection.PesquisaCurriculo.ToString(), null));
                    }
                    else if (e.CommandName.Equals("EditarVaga"))
                    {
                        base.UrlDestino.Value = "SalaSelecionadorVagasAnunciadas.aspx";
                        Session.Add(Chave.Temporaria.Variavel2.ToString(), idVaga);
                        Redirect(Page.GetRouteUrl(Enumeradores.RouteCollection.AnunciarVaga.ToString(), null));
                    }
                    else if (e.CommandName.Equals("ArquivarVaga"))
                    {
                        Vaga objVaga = Vaga.LoadObject(idVaga);
                        objVaga.ArquivarVaga(base.IdUsuarioFilialPerfilLogadoEmpresa.HasValue ? base.IdUsuarioFilialPerfilLogadoEmpresa.Value : base.IdUsuarioFilialPerfilLogadoUsuarioInterno.Value, null);

                        CarregarComboFuncoes();
                        CarregarGridVagasCampanha();
                    }
                    else if (e.CommandName.Equals("AtivarVaga"))
                    {
                        Vaga objVaga = Vaga.LoadObject(idVaga);
                        objVaga.AtivarVaga(base.IdUsuarioFilialPerfilLogadoEmpresa.HasValue ? base.IdUsuarioFilialPerfilLogadoEmpresa.Value : base.IdUsuarioFilialPerfilLogadoUsuarioInterno.Value, null);

                        CarregarGridVagasCampanha();
                        CarregarComboFuncoes();
                    }
                    else if (e.CommandName.Equals("AnuncioMassa"))
                    {
                        if (new Filial(base.IdFilial.Value).PossuiPlanoAtivo())
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
                        if (EventExcluir != null)
                            EventExcluir(idVaga);
                    }
                    else if (e.CommandName.Equals("ClonarVaga"))
                    {
                        base.UrlDestino.Value = "SalaSelecionadorVagasAnunciadas.aspx";
                        Session.Add(Chave.Temporaria.Variavel2.ToString(), idVaga); //Id da vaga
                        Session.Add(Chave.Temporaria.Variavel8.ToString(), true); //ClonarVaga
                        Redirect(Page.GetRouteUrl(Enumeradores.RouteCollection.AnunciarVaga.ToString(), null));
                    }
                    else if (e.CommandName.Equals("VisualizarVaga"))
                    {
                        string funcao = gvVagasCampanha.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Des_Funcao"].ToString().Replace("(Estágio)", "");
                        Funcao oFuncao;
                        Funcao.CarregarPorDescricao(funcao, out oFuncao);
                        AreaBNE oArea;
                        oArea = AreaBNE.LoadObject(oFuncao.AreaBNE.IdAreaBNE);
                        string _bebug_link = "";
#if DEBUG
                        _bebug_link = "http://localhost:2500";
#endif
                        string url = String.Format("{0}/vaga-de-emprego-na-area-{1}-em-{2}-{3}/{4}/{5}",
                             _bebug_link, oArea.DescricaoAreaBNE, gvVagasCampanha.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Nme_Cidade"].ToString(),
                              gvVagasCampanha.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Sig_Estado"].ToString(),
                             funcao, idVaga);

                        Redirect(UIHelper.RemoverAcentos(url.Replace(" ", "-").ToLower()));
                    }
                    else if (e.CommandName.Equals("PublicacaoImediataVaga"))
                    {
                        int idPlano;
                        var objUsuarioFilialPerfil = UsuarioFilialPerfil.LoadObject(base.IdUsuarioFilialPerfilLogadoEmpresa.Value);
                        var url = new Vaga(idVaga).ComprarPublicacaoImediata(objUsuarioFilialPerfil, out idPlano);
                        base.PagamentoIdentificadorPlano.Value = idPlano;

                        Redirect(url);
                    }
                }
            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region gvVagasCampanha_ItemDataBound
        protected void gvVagasCampanha_ItemDataBound(object sender, GridItemEventArgs e)
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

        #region gvVagasCampanha_ColumnCreated
        protected void gvVagasCampanha_ColumnCreated(object sender, GridColumnCreatedEventArgs e)
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

        #region gvVagasCampanha_ItemCreated
        protected void gvVagasCampanha_ItemCreated(object sender, GridItemEventArgs e)
        {
            var item = e.Item as GridGroupHeaderItem;
            if (item != null)
            {
                item.Cells[0].Controls.Clear();
                item.Cells[0].Visible = false;
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
            gvVagas.CurrentPageIndex = 0;
            gvVagasCampanha.CurrentPageIndex = 0;

            CarregarGridVagas();
            CarregarGridVagasCampanha();
        }
        #endregion

        #region btnVoltar_Click
        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            Redirect(base.STC.Value ? "SiteTrabalheConoscoMenu.aspx" : "SalaSelecionador.aspx");
        }

        #endregion

        #region ccbAnunciante_SelectedIndexChanged
        protected void ccbAnunciante_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            CarregarGridVagas();
        }
        #endregion

        #endregion

        #region Métodos

        #region Inicializar
        private void Inicializar()
        {
            if (Request.UrlReferrer != null)
                UrlOrigem = Request.UrlReferrer.AbsoluteUri;

            CriarUrlVagas();

            CarregarComboAnunciantes();
            CarregarComboStatus();
            CarregarComboFuncoes();

            gvVagas.GroupingEnabled = true;
            gvVagas.PageSize = Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.QuantidadeItensPorPaginaSalaSelecionador));

            gvVagasCampanha.GroupingEnabled = true;
            gvVagasCampanha.PageSize = Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.QuantidadeItensPorPaginaSalaSelecionador));

            CarregarGridVagas();
            CarregarGridVagasCampanha();
        }
        protected void CriarUrlVagas()
        {
#if DEBUG
                UrlVaga = "http://localhost:2500/Vaga/VisualizarVagaEmpresa?identificador=";
#endif
            UrlVaga = "http://" + Helper.RecuperarURLAmbiente() + "/vagas-de-emprego/Vaga/VisualizarVagaEmpresa?identificador=";

        }
        #endregion

        #region CarregarGridVagas
        public void CarregarGridVagas()
        {
            //ajustando quais vagas devem aparecer.
            bool? vagasAnunciadas = null; //NULL busca todas as vagas
            if (rcbStatus.SelectedValue.Equals("1")) //Se for Vagas Ativas
                vagasAnunciadas = true;
            else if (rcbStatus.SelectedValue.Equals("2")) //Se for Vagas Inativas
                vagasAnunciadas = false;

            var objFilial = new Filial(IdFilial.Value);
            var empresaAssociacao = objFilial.EmpresaAssociacao();
            var objUsuarioUsuarioFilialPerfil = UsuarioFilialPerfil.LoadObject(base.IdUsuarioFilialPerfilLogadoEmpresa.Value);

            int? usuarioFilialPerfil = null;
            if ((empresaAssociacao && objUsuarioUsuarioFilialPerfil.Perfil.IdPerfil == (int)Enumeradores.Perfil.AcessoEmpresa) || ccFiltrarApenasMinhasVagas.Checked)
                usuarioFilialPerfil = base.IdUsuarioFilialPerfilLogadoEmpresa.Value;

            int? usuarioFilialPerfilConfidencial = null;
            if (objUsuarioUsuarioFilialPerfil.Perfil.IdPerfil == (int)Enumeradores.Perfil.AcessoEmpresa)
                usuarioFilialPerfilConfidencial = objUsuarioUsuarioFilialPerfil.IdUsuarioFilialPerfil;

            int totalRegistros;
            DataTable dt = Vaga.ListarVagasFilial(base.IdFilial.Value, gvVagas.CurrentPageIndex, gvVagas.PageSize, vagasAnunciadas, usuarioFilialPerfil, usuarioFilialPerfilConfidencial, RecuperarListFuncoes(), out totalRegistros, false, RecuperarListAnunciantes());
            UIHelper.CarregarRadGrid(gvVagas, dt, totalRegistros);

            AjustarInformacaoVagas(vagasAnunciadas, usuarioFilialPerfil);

            upGvVagas.Update();
        }
        #endregion

        #region CarregarGridVagasCampanha
        public void CarregarGridVagasCampanha()
        {
            //ajustando quais vagas devem aparecer.
            bool? vagasAnunciadas = null; //NULL busca todas as vagas
            if (rcbStatus.SelectedValue.Equals("1")) //Se for Vagas Ativas
                vagasAnunciadas = true;
            else if (rcbStatus.SelectedValue.Equals("2")) //Se for Vagas Inativas
                vagasAnunciadas = false;

            var objFilial = new Filial(IdFilial.Value);
            var empresaAssociacao = objFilial.EmpresaAssociacao();
            var objUsuarioUsuarioFilialPerfil = UsuarioFilialPerfil.LoadObject(base.IdUsuarioFilialPerfilLogadoEmpresa.Value);

            int? usuarioFilialPerfil = null;
            if ((empresaAssociacao && objUsuarioUsuarioFilialPerfil.Perfil.IdPerfil == (int)Enumeradores.Perfil.AcessoEmpresa) || ccFiltrarApenasMinhasVagas.Checked)
                usuarioFilialPerfil = base.IdUsuarioFilialPerfilLogadoEmpresa.Value;

            int? usuarioFilialPerfilConfidencial = null;
            if (objUsuarioUsuarioFilialPerfil.Perfil.IdPerfil == (int)Enumeradores.Perfil.AcessoEmpresa)
                usuarioFilialPerfilConfidencial = objUsuarioUsuarioFilialPerfil.IdUsuarioFilialPerfil;

            int totalRegistrosVagaCampanha;
            DataTable dataVagasCampanha = Vaga.ListarVagasFilial(base.IdFilial.Value, gvVagasCampanha.CurrentPageIndex, gvVagasCampanha.PageSize, vagasAnunciadas, usuarioFilialPerfil, usuarioFilialPerfilConfidencial, RecuperarListFuncoes(), out totalRegistrosVagaCampanha, true, RecuperarListAnunciantes());
            UIHelper.CarregarRadGrid(gvVagasCampanha, dataVagasCampanha, totalRegistrosVagaCampanha);

            AjustarInformacaoVagas(vagasAnunciadas, usuarioFilialPerfil);

            upGvVagasCampanha.Update();
        }
        #endregion

        #region AjustarInformacaoVagas
        public void AjustarInformacaoVagas(bool? vagasAnunciadas, int? usuarioFilialPerfil)
        {
            if (vagasAnunciadas.HasValue)
            {
                if ((bool)vagasAnunciadas)
                    lblInformacaoVagas.Text = String.Format("{0} {1}", gvVagas.MasterTableView.VirtualItemCount + gvVagasCampanha.MasterTableView.VirtualItemCount, "vaga(s) ativas");
                else
                    lblInformacaoVagas.Text = String.Format("{0} {1}", gvVagas.MasterTableView.VirtualItemCount + gvVagasCampanha.MasterTableView.VirtualItemCount, "vaga(s) inativas");
            }
            else
            {
                int ativas = Filial.RecuperarQuantidadeVagasAnunciadas(base.IdFilial.Value, usuarioFilialPerfil);
                int inativas = Filial.RecuperarQuantidadeVagasArquivadas(base.IdFilial.Value, usuarioFilialPerfil);

                lblInformacaoVagas.Text = String.Format("{0} {1} e {2} {3}", ativas, "vaga(s) ativas", inativas, "vaga(s) inativas");
            }
            upInformacaoVagas.Update();
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

        #region CarregarComboAnunciantes
        private void CarregarComboAnunciantes()
        {
            if (UsuarioFilialPerfil.ValidarPerfil(base.IdUsuarioFilialPerfilLogadoEmpresa.Value, (int)Enumeradores.Perfil.AcessoEmpresaMaster))
            {
                UIHelper.CarregarRadComboBox(ccbAnunciante, Vaga.ListaAnunciatesVagaDaFilial(base.IdFilial.Value), "Idf_usuario_filial_perfil", "Nme_Pessoa");
                pnlAnunciantes.Visible = true;
                pnlMinhasVagasFiltro.Visible = false;
            }

        }
        #endregion

        #region ExcluirVaga
        public void ExcluirVaga(int idVaga)
        {
            Vaga.InativarVaga(idVaga, base.IdUsuarioFilialPerfilLogadoEmpresa.HasValue ? base.IdUsuarioFilialPerfilLogadoEmpresa.Value : base.IdUsuarioFilialPerfilLogadoUsuarioInterno.Value, null);
        }
        #endregion

        #region RecuperarListFuncoes
        private List<int> RecuperarListFuncoes()
        {
            return ccFiltrarVagasFuncao.GetCheckedItems().Select(item => Convert.ToInt32(item.Value)).ToList();
        }
        #endregion

        #region RecuperarListAnunciantes
        private List<string> RecuperarListAnunciantes()
        {
            return ccbAnunciante.GetCheckedItems().Select(item => item.Value).ToList();
        }
        #endregion

        #region RetornarBancoCurriculos
        protected string RetornarBancoCurriculos(int idVaga)
        {
            var objVaga = Vaga.LoadObject(idVaga);
            var resultado = CurriculoIdentificador.EfetuarRequisicao(BLL.Custom.BuscaCurriculo.MontarQuery(objVaga));

            try
            {
                return resultado.response.numFound.ToString();
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
                return "0";
            }

        }
        #endregion

        #endregion
    }
}