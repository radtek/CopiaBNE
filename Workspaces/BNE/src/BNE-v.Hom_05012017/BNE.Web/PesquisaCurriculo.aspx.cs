﻿using BNE.BLL;
using BNE.BLL.Custom;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using BNE.Web.Master;
using Employer.Componentes.UI.Web;
using Resources;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BNE.Auth.Core.Enumeradores;
using Telerik.Web.UI;
using Enumeradores = BNE.BLL.Enumeradores;
using Label = System.Web.UI.WebControls.Label;
using Panel = System.Web.UI.WebControls.Panel;
using PessoaFisicaFoto = BNE.Web.Handlers.PessoaFisicaFoto;
using BNE.BLL.Custom.EnvioMensagens;
using System.Net;
using System.IO;
using BNE.BLL.DTO.SINE;
using System.Web.Script.Serialization;
using BNE.BLL.Common;
using BNE.Web.Code.ViewStateObjects;
using System.Web;

namespace BNE.Web
{
    public partial class PesquisaCurriculo : BasePage
    {

        public NavegacaoCurriculos NavegacaoCurriculos = new NavegacaoCurriculos();
        private Control _modalConfimacaoEnvio;
        
        #region Propriedades

        #region DicCurriculos - Variável 1
       
        #endregion

        #region IdCurriculoPesquisaCurriculo - Variável 2
        /// <summary>
        /// Propriedade que armazena e recupera o ID
        /// </summary>
        private int? IdCurriculoPesquisaCurriculo
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel2.ToString()] != null)
                    return Int32.Parse(ViewState[Chave.Temporaria.Variavel2.ToString()].ToString());
                return null;
            }
            set
            {
                if (value != null)
                    ViewState.Add(Chave.Temporaria.Variavel2.ToString(), value);
                else
                    ViewState.Remove(Chave.Temporaria.Variavel2.ToString());
            }
        }
        #endregion

        #region ClicouMensagem - Variável 3
        private bool ClicouMensagem
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel3.ToString()] != null)
                    return Convert.ToBoolean(ViewState[Chave.Temporaria.Variavel3.ToString()]);

                return false;
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Variavel3.ToString(), value);
            }
        }
        #endregion

        #region ClicouCurriculo - Variável 4
        private bool ClicouCurriculo
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel4.ToString()] != null)
                    return Convert.ToBoolean(ViewState[Chave.Temporaria.Variavel4.ToString()]);

                return false;
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Variavel4.ToString(), value);
            }
        }
        #endregion

        #region ListCurriculosVisualizados - Variável 5
        /// <summary>
        /// Variável 6
        /// </summary>
        private List<Tuple<int, string, DateTime>> ListCurriculosVisualizados
        {
            get
            {
                return (List<Tuple<int, string, DateTime>>)(ViewState[Chave.Temporaria.Variavel5.ToString()]);
            }
            set
            {
                ViewState[Chave.Temporaria.Variavel5.ToString()] = value;
            }
        }
        #endregion

        #region BoolModalConfirmacaoEnvio - Variável 10
        private bool BoolModalConfirmacaoEnvio
        {
            get
            {
                return Convert.ToBoolean(ViewState[Chave.Temporaria.Variavel10.ToString()]);
            }
            set
            {
                ViewState[Chave.Temporaria.Variavel10.ToString()] = value;
            }
        }
        #endregion

        #region EmpresaPossuePlanoNaoBloqueada - Variável 12
        private bool EmpresaPossuePlanoNaoBloqueada
        {
            get
            {
                return Convert.ToBoolean(ViewState[Chave.Temporaria.Variavel12.ToString()]);
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Variavel12.ToString(), value);
            }
        }
        #endregion

        #region UrlOrigem - Variável 13
        /// <summary>
        /// </summary>
        private string UrlOrigem
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel13.ToString()] != null)
                    return (ViewState[Chave.Temporaria.Variavel13.ToString()]).ToString();
                return null;
            }
            set
            {
                if (value != null)
                    ViewState.Add(Chave.Temporaria.Variavel13.ToString(), value);
                else
                    ViewState.Remove(Chave.Temporaria.Variavel13.ToString());
            }
        }
        #endregion

        #region ClicouPesquisaAvancada - Variável 15
        private bool ClicouPesquisaAvancada
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel15.ToString()] != null)
                    return Convert.ToBoolean(ViewState[Chave.Temporaria.Variavel15.ToString()]);

                return false;
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Variavel15.ToString(), value);
            }
        }
        #endregion

        #region PaginaAspxOrigem - Variável 16
        /// <summary>
        /// </summary>
        private string PaginaAspxOrigem
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel16.ToString()] != null)
                    return (ViewState[Chave.Temporaria.Variavel16.ToString()]).ToString();
                return null;
            }
            set
            {
                if (value != null)
                    ViewState.Add(Chave.Temporaria.Variavel16.ToString(), value);
                else
                    ViewState.Remove(Chave.Temporaria.Variavel16.ToString());
            }
        }
        #endregion

        #region EmpresaLogadaPossuiPlanoLiberado - Variável 17
        private bool EmpresaLogadaPossuiPlanoLiberado
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel17.ToString()] != null)
                    return Convert.ToBoolean(ViewState[Chave.Temporaria.Variavel17.ToString()]);
                return false;
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Variavel17.ToString(), value);
            }
        }
        #endregion

        #region EmpresaBloqueada - Variável 18
        private bool EmpresaBloqueada
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel18.ToString()] != null)
                    return Convert.ToBoolean(ViewState[Chave.Temporaria.Variavel18.ToString()]);
                return false;
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Variavel18.ToString(), value);
            }
        }
        #endregion

        #region EmpresaAssociacao - Variável 19
        private bool EmpresaAssociacao
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel19.ToString()] != null)
                    return Convert.ToBoolean(ViewState[Chave.Temporaria.Variavel19.ToString()]);
                return false;
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Variavel19.ToString(), value);
            }
        }
        #endregion

        #region MostrarNomeCandidatoEEmpresaNaExperienciaProfissional - Variável 20
        private bool MostrarNomeCandidatoEEmpresaNaExperienciaProfissional
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel20.ToString()] != null)
                    return Convert.ToBoolean(ViewState[Chave.Temporaria.Variavel20.ToString()]);
                return false;
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Variavel20.ToString(), value);
            }
        }
        #endregion

        #region BuscaBaseGratis - Variável 21
        private bool BuscaBaseGratis
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel21.ToString()] != null)
                    return Convert.ToBoolean(ViewState[Chave.Temporaria.Variavel21.ToString()]);
                return false;
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Variavel21.ToString(), value);
            }
        }
        #endregion

        #region PalavraChavePesquisa
        /// <summary>
        /// Usado para transporta para a session e usar na visualização do cv aberto
        /// </summary>
        private string PalavraChavePesquisa
        {
            set
            {
                Session.Add(Chave.Permanente.PalavraChavePesquisa.ToString(), value);
            }
        }
        #endregion

        #region ResultadoPesquisaCurriculo - ViewStateObject_ResultadoPesquisaCurriculo
        private ResultadoPesquisaCurriculo ResultadoPesquisaCurriculo
        {
            get
            {
                if (ViewState[Chave.Temporaria.ViewStateObject_ResultadoPesquisaCurriculo.ToString()] != null)
                    return (ResultadoPesquisaCurriculo)ViewState[Chave.Temporaria.ViewStateObject_ResultadoPesquisaCurriculo.ToString()];
                return new ResultadoPesquisaCurriculo();
            }
            set
            {
                ViewState.Add(Chave.Temporaria.ViewStateObject_ResultadoPesquisaCurriculo.ToString(), value);
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

            if (BoolModalConfirmacaoEnvio)
            {
                AjustarMostrarModalConfirmacaoEnvio(false, false, String.Empty, String.Empty, false);
                upCphModais.Update();
            }

            ucModalLogin.Logar += ucModalLogin_Logar;
            ucEnvioMensagem.EnviarConfirmacao += ucEnvioMensagem_EnviarConfirmacao;
            ucEnvioCurriculo.EnviarConfirmacao += ucEnvioCurriculo_EnviarConfirmacao;
            ucModalConfirmacao.CliqueAqui += ucModalConfirmacao_CliqueAqui;
            ucModalConfirmacao.eventVoltar += ucModalConfirmacao_Voltar;

            var principal = (Principal)Master;
            if (principal != null) principal.PesquisaRapidaCurriculo += PesquisaCurriculo_PesquisaRapidaCurriculo;

            ucConfirmacaoExclusao.Cancelar += ucConfirmacaoExclusao_Cancelar;
            ucConfirmacaoExclusao.Confirmar += ucConfirmacaoExclusao_Confirmar;


            if (Page.IsPostBack)
            {
                if (Request["__EVENTTARGET"] == "ver_cv_sine")
                {
                    BaixarCVSine(Convert.ToInt32(Request["__EVENTARGUMENT"]));
                }
            }

        }

        void ucEnvioMensagem_EnviarConfirmacao(string titulo, string mensagem, bool cliqueAqui)
        {
            base.DicCurriculos.Value = new Dictionary<int, bool>();
            ucModalConfirmacao.PreencherCampos(titulo, mensagem, cliqueAqui);
            ucModalConfirmacao.MostrarModal();
            pnlM.Visible = true;
            upM.Update();
        }
        void ucEnvioCurriculo_EnviarConfirmacao()
        {
            base.DicCurriculos.Value = new Dictionary<int, bool>();
            ucModalConfirmacao.PreencherCampos("Confirmação", "Currículo enviado com sucesso!", false, false);
            ucModalConfirmacao.MostrarModal();
            pnlM.Visible = true;
            upM.Update();

        }
        #endregion

        #region ucModalConfirmacao_Voltar
        void ucModalConfirmacao_Voltar()
        {
            ucModalConfirmacao.FecharModal();
        }
        #endregion

        #region ucModalConfirmacao_CliqueAqui
        //Por agora apenas a modal de cadastro de vaga que chama este método.
        void ucModalConfirmacao_CliqueAqui()
        {
            AjustarMostrarModalConfirmacaoEnvio(false, true, String.Empty, String.Empty, false);
            ucModalConfirmacao.FecharModal();
            pnlM.Visible = true;
            upM.Update();
        }
        #endregion

        #region btnEnviarMensagem_Click
        protected void btnEnviarMensagem_Click(object sender, EventArgs e)
        {
            if (base.IdPessoaFisicaLogada.HasValue)
            {
                if (base.IdFilial.HasValue)
                {
                    var objFilial = new Filial(base.IdFilial.Value);
                    if (!EmpresaEmAuditoria(objFilial) && !EmpresaBloqueada(objFilial))
                    {
                        ClicouCurriculo = false;
                        ClicouMensagem = true;
                        InicializarEnvioMensagem();
                    }

                    upPesquisa.Update();
                }
                else
                    ExibirMensagem(MensagemAviso._101001, TipoMensagem.Aviso);
            }
            else
                AbrirModalLogin();
        }
        #endregion

        #region btnEnviarCurriculo_Click
        protected void btnEnviarCurriculo_Click(object sender, EventArgs e)
        {
            if (base.IdPessoaFisicaLogada.HasValue)
            {
                if (base.IdFilial.HasValue)
                {
                    var objFilial = new Filial(base.IdFilial.Value);
                    if (!EmpresaEmAuditoria(objFilial) && !EmpresaBloqueada(objFilial))
                    {
                        ClicouCurriculo = true;
                        ClicouMensagem = false;
                        InicializarEnvioCurriculo(null);
                    }

                    upPesquisa.Update();
                }
                else
                    ExibirMensagem(MensagemAviso._101001, TipoMensagem.Aviso);
            }
            else
                AbrirModalLogin();
        }
        #endregion

        #region btnCampanhaRecrutamento_Click
        protected void btnCampanhaRecrutamento_Click(object sender, EventArgs e)
        {
            if (base.IdFilial.HasValue)
            {
                var objFilial = new Filial(base.IdFilial.Value);
                if (!EmpresaBloqueada(objFilial))
                    Redirect(Page.GetRouteUrl(Enumeradores.RouteCollection.CampanhaRecrutamento.ToString(), null));
            }
            else
                ExibirMensagem(MensagemAviso._101001, TipoMensagem.Aviso);
        }
        #endregion

        #region ucConfirmacaoExclusao_Cancelar
        void ucConfirmacaoExclusao_Cancelar()
        {
            IdCurriculoPesquisaCurriculo = null;
        }
        #endregion

        #region ucConfirmacaoExclusao_Confirmar
        void ucConfirmacaoExclusao_Confirmar()
        {
            if (ResultadoPesquisaCurriculo.PorVaga())
            {
                if (IdCurriculoPesquisaCurriculo.HasValue)
                {
                    VagaCandidato objVagaCandidato;
                    if (VagaCandidato.CarregarPorVagaCurriculo(ResultadoPesquisaCurriculo.Vaga.IdVaga, IdCurriculoPesquisaCurriculo.Value, out objVagaCandidato))
                        objVagaCandidato.Inativar();
                }
                else
                    VagaCandidato.Inativar(new Vaga(ResultadoPesquisaCurriculo.Vaga.IdVaga), RecuperarCurrriculosSelecionados());

                CarregarGridCurriculo(false);
                base.DicCurriculos.Value = new Dictionary<int, bool>();

                IdCurriculoPesquisaCurriculo = null;
                ucConfirmacaoExclusao.FecharModal();
            }
        }
        #endregion

        #region gvResultadoPesquisa_ItemDataBound
        protected void gvResultadoPesquisa_ItemDataBound(object sender, GridItemEventArgs e)
        {
            RadGrid grid = e.Item.OwnerGridID == "cphConteudo_gvResultadoPesquisaCampanha" ? gvResultadoPesquisaCampanha : gvResultadoPesquisa;

            if (ResultadoPesquisaCurriculo.PorVaga())
            {
                var item = e.Item as GridGroupHeaderItem;
                if (item != null)
                {
                    var groupDataRow = (DataRowView)e.Item.DataItem;
                    string dataCellText = groupDataRow["DentroPerfil"].Equals(0) ? "Outros inscritos" : "No Perfil";

                    item.DataCell.Text = dataCellText;
                }
            }
            else
            {
                var item = e.Item as GridGroupHeaderItem;
                grid.GroupingEnabled = false;
                grid.EnableLinqExpressions = false;
                if (item != null)
                {
                    item.Cells[0].Controls.Clear();
                    item.Cells[0].Visible = false;
                }
                if (e.Item is GridGroupHeaderItem)
                {
                    (e.Item as GridGroupHeaderItem).Cells[1].Controls.Clear();
                    (e.Item as GridGroupHeaderItem).Cells[1].Visible = false;
                }
            }


            if (e.Item is GridDataItem)
            {
                var idCurriculo = Convert.ToInt32(grid.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Curriculo"].ToString());

                //Adicionando os currículo na session para fazer a navegação entre os currículos, talvez colocar no bound e mandar todos de uma só vez?
                if (ResultadoPesquisaCurriculo.PorCurriculo())
                    NavegacaoCurriculos.AdicionarCurriculo(ResultadoPesquisaCurriculo.TipoPesquisa.Curriculo, ResultadoPesquisaCurriculo.Curriculo.IdPesquisaCurriculo, idCurriculo);

                if (ResultadoPesquisaCurriculo.PorVaga())
                {
                    NavegacaoCurriculos.AdicionarCurriculo(ResultadoPesquisaCurriculo.TipoPesquisa.Vaga, ResultadoPesquisaCurriculo.Vaga.IdVaga, idCurriculo);
                    e.Item.FindControl("btiExcluirCurriculo").Visible = true;
                    ((HtmlGenericControl)e.Item.FindControl("divIcones")).Attributes.Add("class", "icones_pesquisa_curriculo_vagas icones");
                }

                if (ResultadoPesquisaCurriculo.PorRastreador())
                {
                    NavegacaoCurriculos.AdicionarCurriculo(ResultadoPesquisaCurriculo.TipoPesquisa.Rastreador, ResultadoPesquisaCurriculo.Rastreador.IdRastreadorCurriculo, idCurriculo);
                }

                e.Item.FindControl("btiAssociar").Visible = base.IdFilial.HasValue;
                e.Item.FindControl("btiCompleto").Visible = !EmpresaBloqueada;

                var imgAvaliacao = (LinkButton)e.Item.FindControl("imgAvaliacao");
                var lblSMS = (Label)e.Item.FindControl("lblSMS");

                //Ajustando balão saiba mais
                var bsm = (BalaoSaibaMais)e.Item.FindControl("bsmAvaliacao");
                bsm.TargetControlID = imgAvaliacao.ClientID;
                var bsmSMS = (BalaoSaibaMais)e.Item.FindControl("bsmSMS");
                bsmSMS.TargetControlID = lblSMS.ClientID;

                var btlQuemVisualiza = (Label)e.Item.FindControl("lblQuemVisualiza");
                //Ajusando imagem de avaliação
                if (!String.IsNullOrEmpty(imgAvaliacao.CommandArgument))
                {
                    if (Convert.ToInt32(imgAvaliacao.CommandArgument).Equals(Enumeradores.CurriculoClassificacao.AvaliacaoNegativa.GetHashCode()))
                        imgAvaliacao.CssClass = "fa fa-frown-o fa-2x";
                    else if (Convert.ToInt32(imgAvaliacao.CommandArgument).Equals(Enumeradores.CurriculoClassificacao.AvaliacaoPositiva.GetHashCode()))
                        imgAvaliacao.CssClass = "fa fa-smile-o fa-2x";
                    else if (Convert.ToInt32(imgAvaliacao.CommandArgument).Equals(Enumeradores.CurriculoClassificacao.AvaliacaoNeutra.GetHashCode()))
                        imgAvaliacao.CssClass = "fa fa-meh-o fa-2x";
                }

                var ckbDataItem = (CheckBox)e.Item.FindControl("ckbDataItem");
                ckbDataItem.Attributes.Add("CommandArgument", idCurriculo.ToString());
                if (!base.DicCurriculos.HasValue)
                    base.DicCurriculos.Value = new Dictionary<int, bool>();
                if (base.DicCurriculos.Value.ContainsKey(idCurriculo))
                    ckbDataItem.Checked = base.DicCurriculos.Value[idCurriculo];

                if (base.IdFilial.HasValue)
                {
                    if (ListCurriculosVisualizados.Where(t => t.Item1 == idCurriculo).Any())
                    {
                        var btlNomeCurriculo = (LinkButton)e.Item.FindControl("lblNomeCurriculo");
                        btlNomeCurriculo.CssClass = "tooltipB balao nome_descricao_curriculo_visited";
                        string Nome = ListCurriculosVisualizados.Where(t => t.Item1.Equals(idCurriculo)).Select(s => s.Item2).FirstOrDefault();
                        var DtaVisualizacao = ListCurriculosVisualizados.Where(t => t.Item1 == idCurriculo).Select(t => t.Item3).FirstOrDefault();
                        if (!String.IsNullOrEmpty(Nome))
                        {
                            btlQuemVisualiza.Text = String.Format("Último a visualizar: {0} em {1} ", Nome, DtaVisualizacao);
                            btlQuemVisualiza.Visible = true;
                        }

                    }
                }
            }

            if (e.Item is GridNestedViewItem)
            {
                var dt = (DataTable)grid.DataSource;
                var gridDataItem = ((GridNestedViewItem)e.Item).ParentItem;
                var id = gridDataItem.GetDataKeyValue("Idf_Curriculo");

                var dataTableMini = dt.Clone();

                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["Idf_Curriculo"].Equals(id))
                    {
                        dataTableMini.ImportRow(dr);
                    }
                }

                var rlvCurriculo = (RadListView)e.Item.FindControl("rlvCurriculos");

                UIHelper.CarregarRadListView(rlvCurriculo, dataTableMini);
            }
        }
        #endregion

        #region ckbDataItem_CheckedChanged
        protected void ckbDataItem_CheckedChanged(object sender, EventArgs e)
        {
            var ckbDataItem = (CheckBox)sender;
            base.DicCurriculos.Value[Convert.ToInt32(ckbDataItem.Attributes["CommandArgument"])] = ckbDataItem.Checked;
        }
        #endregion

        #region ckbHeaderItem_CheckedChanged
        protected void ckbHeaderItem_CheckedChanged(object sender, EventArgs e)
        {
            var ckbHeaderItem = (CheckBox)sender;

            foreach (GridDataItem gdi in gvResultadoPesquisa.Items)
            {
                int idCurriculo = Convert.ToInt32(gvResultadoPesquisa.MasterTableView.DataKeyValues[gdi.ItemIndex]["Idf_Curriculo"].ToString());
                var ckbDataItem = (CheckBox)gdi.FindControl("ckbDataItem");
                base.DicCurriculos.Value[idCurriculo] = ckbDataItem.Checked = ckbHeaderItem.Checked;
            }

            upCurriculos.Update();
        }
        #endregion

        #region ckbHeaderItem_CheckedChanged
        protected void ckbHeaderItemCampanha_CheckedChanged(object sender, EventArgs e)
        {
            var ckbHeaderItem = (CheckBox)sender;

            foreach (GridDataItem gdi in gvResultadoPesquisaCampanha.Items)
            {
                int idCurriculo = Convert.ToInt32(gvResultadoPesquisaCampanha.MasterTableView.DataKeyValues[gdi.ItemIndex]["Idf_Curriculo"].ToString());
                var ckbDataItem = (CheckBox)gdi.FindControl("ckbDataItem");
                base.DicCurriculos.Value[idCurriculo] = ckbDataItem.Checked = ckbHeaderItem.Checked;
            }

            upCurriculos.Update();
        }
        #endregion

        #region gvResultadoPesquisa_ItemCommand
        protected void gvResultadoPesquisa_ItemCommand(object source, GridCommandEventArgs e)
        {
            RadGrid grid;
            if (e.Item.OwnerGridID == "cphConteudo_gvResultadoPesquisaCampanha")
            {
                grid = gvResultadoPesquisaCampanha;
            }
            else
            {
                grid = gvResultadoPesquisa;
            }
            if (e.CommandName.Equals(RadGrid.ExpandCollapseCommandName))
            {
                AjustarVisualizacaoDadosCurriculo(e);
            }
            else if (e.CommandName.Equals("RowClick") || e.CommandName.Equals("MostrarModal"))
            {
                AjustarVisualizacaoDadosCurriculo(e);
                e.Item.Expanded = !e.Item.Expanded;
            }
            else if (e.CommandName.Equals("EnviarMensagem"))
            {
                int idCurriculo = Convert.ToInt32(grid.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Curriculo"]);

                if (base.IdFilial.HasValue)
                {
                    var objFilial = new Filial(base.IdFilial.Value);
                    if (!EmpresaEmAuditoria(objFilial) && !EmpresaBloqueada(objFilial))
                    {
                        InicializarEnvioMensagem_imgMensagemAcaoCarta(idCurriculo);
                    }
                }
                else
                    ExibirMensagem(MensagemAviso._101001, TipoMensagem.Aviso);
            }
            else if (e.CommandName.Equals("EnviarCurriculo"))
            {
                int idCurriculo = Convert.ToInt32(grid.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Curriculo"]);

                if (base.IdFilial.HasValue)
                {
                    var objFilial = new Filial(base.IdFilial.Value);
                    if (!EmpresaEmAuditoria(objFilial) && !EmpresaBloqueada(objFilial))
                        InicializarEnvioCurriculo(idCurriculo);
                }
                else
                    ExibirMensagem(MensagemAviso._101001, TipoMensagem.Aviso);
            }
            else if (e.CommandName.Equals("RemoverCurriculo"))
            {
                IdCurriculoPesquisaCurriculo = Convert.ToInt32(grid.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Curriculo"]);

                var objPessoaFisica = new PessoaFisica(PessoaFisica.RecuperarIdPorCurriculo(new Curriculo((int)IdCurriculoPesquisaCurriculo)));

                ucConfirmacaoExclusao.Inicializar("Atenção!", "Tem certeza que deseja excluir o currículo de " + objPessoaFisica.NomeCompleto + " da sua lista?");
                ucConfirmacaoExclusao.MostrarModal();
            }
            else if (e.CommandName.Equals("Associar"))
            {
                var objFilial = new Filial(base.IdFilial.Value);
                if (!EmpresaBloqueada(objFilial))
                {
                    int idCurriculo = Convert.ToInt32(grid.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Curriculo"]);
                    ucAssociarCurriculoVaga.Inicializar(idCurriculo);
                }
            }
            else if (e.CommandName.Equals("ChamarAgora"))
            {
                int idCurriculo = Convert.ToInt32(grid.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Curriculo"]);
                ChamarAgora(idCurriculo);
            }
        }
        #endregion

        #region gvResultadoPesquisa_DataBound
        protected void gvResultadoPesquisa_DataBound(object sender, EventArgs e)
        {
            //Fluxo para salvar os currículos que apareceram no resultado da pesquisa de currículos
            if (ResultadoPesquisaCurriculo.PorCurriculo())
            {
                var idPesquisaCurriculo = ResultadoPesquisaCurriculo.Curriculo.IdPesquisaCurriculo;

                if (NavegacaoCurriculos.PesquisaCurriculoCurriculos.Value[ResultadoPesquisaCurriculo.TipoPesquisa.Curriculo].ContainsKey(idPesquisaCurriculo))
                {
                    var lista = NavegacaoCurriculos.PesquisaCurriculoCurriculos.Value[ResultadoPesquisaCurriculo.TipoPesquisa.Curriculo][idPesquisaCurriculo].Where(kvp => !kvp.Value).ToList();
                    //Se tem algum que não está flagado
                    if (lista.Any())
                    {
                        var objPesquisaCurriculo = new BLL.PesquisaCurriculo(idPesquisaCurriculo);

                        DataTable dtPesquisaCurriculoCurriculos = null;

                        foreach (var cv in lista)
                        {
                            var objPesquisaCurriculoCurriculos = new PesquisaCurriculoCurriculos
                            {
                                Curriculo = new Curriculo(cv.Key),
                                PesquisaCurriculo = objPesquisaCurriculo
                            };
                            objPesquisaCurriculoCurriculos.AddBulkTable(ref dtPesquisaCurriculoCurriculos);

                            var index = NavegacaoCurriculos.PesquisaCurriculoCurriculos.Value[ResultadoPesquisaCurriculo.TipoPesquisa.Curriculo][idPesquisaCurriculo].FindIndex(x => x.Key == cv.Key);
                            NavegacaoCurriculos.PesquisaCurriculoCurriculos.Value[ResultadoPesquisaCurriculo.TipoPesquisa.Curriculo][idPesquisaCurriculo][index] = new KeyValuePair<int, bool>(cv.Key, true);
                        }

                        BLL.PesquisaCurriculoCurriculos.SaveBulkTable(dtPesquisaCurriculoCurriculos);
                    }
                }
            }
        }
        #endregion

        #region ucModalLogin_Logar
        void ucModalLogin_Logar(string urlDestino)
        {
            AjustarLogin();

            //Setando visibilidade dos botoes de salas empresa e sala candidato
            AjustarPermissao();

            if (ClicouCurriculo && !ClicouMensagem)
                InicializarEnvioCurriculo(null);
            else if (!ClicouCurriculo && ClicouMensagem)
                InicializarEnvioMensagem();

            ClicouCurriculo = ClicouMensagem = false;

            //Mosrando mensagem quando o usuário clicou em mensagem, enviar cv ou r1 e acabou de se logar.
            if (base.IdUsuarioFilialPerfilLogadoEmpresa.HasValue && !UsuarioFilialPerfil.ValidarTipoPerfil(base.IdUsuarioFilialPerfilLogadoEmpresa.Value, (int)Enumeradores.TipoPerfil.Empresa))
                ExibirMensagem(MensagemAviso._101001, TipoMensagem.Aviso);
            else
            {
                if (ClicouPesquisaAvancada)
                    Redirect("PesquisaCurriculoAvancada.aspx");
            }
        }
        #endregion

        #region btnAnunciarVaga_Click
        protected void btnAnunciarVaga_Click(object sender, EventArgs e)
        {
            if (base.IdPessoaFisicaLogada.HasValue)
            {
                if (base.IdFilial.HasValue && base.IdUsuarioFilialPerfilLogadoEmpresa.HasValue && UsuarioFilialPerfil.ValidarTipoPerfil(base.IdUsuarioFilialPerfilLogadoEmpresa.Value, (int)BNE.BLL.Enumeradores.TipoPerfil.Empresa))
                {
                    base.UrlDestino.Value = "PesquisaCurriculoCurriculos.aspx";
                    Redirect(Page.GetRouteUrl(Enumeradores.RouteCollection.AnunciarVaga.ToString(), null));
                }
                else //Somente empresas
                    ExibirMensagem(MensagemAviso._101001, TipoMensagem.Aviso);
            }
            else
                Redirect(Page.GetRouteUrl(Enumeradores.RouteCollection.Empresa.ToString(), null));
        }
        #endregion

        #region btnBuscaAvancada_Click
        protected void btnBuscaAvancada_Click(object sender, EventArgs e)
        {
            InicializarBuscaAvancada();
        }
        #endregion

        #region PesquisaCurriculo_PesquisaRapidaCurriculo
        void PesquisaCurriculo_PesquisaRapidaCurriculo(int idPesquisaCurriculo)
        {
            ResultadoPesquisaCurriculo = new ResultadoPesquisaCurriculo(new ResultadoPesquisaCurriculoCurriculo { IdPesquisaCurriculo = idPesquisaCurriculo });
            gvResultadoPesquisa.CurrentPageIndex = 0; //Ajustando para a primeira página para evitar erro na pesquisa.
            CarregarGridCurriculo(false);
        }
        #endregion

        #region btnExcluirCurriculos_Click
        protected void btnExcluirCurriculos_Click(object sender, EventArgs e)
        {
            IdCurriculoPesquisaCurriculo = null;

            if (RecuperarCurrriculosSelecionados().Count > 0)
            {
                ucConfirmacaoExclusao.Inicializar("Atenção!", string.Format("Tem certeza que deseja excluir {0} currículo(s) da sua lista?", RecuperarCurrriculosSelecionados().Count));
                ucConfirmacaoExclusao.MostrarModal();
            }
            else
                ExibirMensagem(MensagemAviso._101002, TipoMensagem.Aviso);
        }
        #endregion

        #region rlvCurriculos_ItemCommand
        protected void rlvCurriculos_ItemCommand(object sender, RadListViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("RowClick"))
            {
                e.ListViewItem.OwnerListView.Parent.FindControl("ContainerCurriculo").Visible = false;
                ((GridNestedViewItem)(e.ListViewItem.OwnerListView.Parent.Parent.Parent.Parent)).ParentItem.Expanded = false;
                upCurriculos.Update();
            }
            else if (e.CommandName.Equals("Questionario"))
            {
                int idCurriculo = Convert.ToInt32(e.ListViewItem.OwnerListView.DataKeyValues[((RadListViewDataItem)e.ListViewItem).DisplayIndex]["Idf_Curriculo"]);

                Control questionario = Page.LoadControl("~/UserControls/Modais/ModalQuestionarioVagas.ascx");
                cphModais.Controls.Add(questionario);

                if (ResultadoPesquisaCurriculo.PorVaga())
                {
                    ((UserControls.Modais.ModalQuestionarioVagas)questionario).Inicializar(ResultadoPesquisaCurriculo.Vaga.IdVaga, idCurriculo);
                    ((UserControls.Modais.ModalQuestionarioVagas)questionario).MostrarModal();

                    upCphModais.Update();
                }
            }
            else if (e.CommandName.Equals("DownloadAnexo"))
            {
                int idCurriculo = Convert.ToInt32(e.ListViewItem.OwnerListView.DataKeyValues[((RadListViewDataItem)e.ListViewItem).DisplayIndex]["Idf_Curriculo"]);

                if (base.STC.Value || base.IdUsuarioFilialPerfilLogadoUsuarioInterno.HasValue)
                {
                    if (CurriculoOrigem.ExisteCurriculoNaOrigem(new Curriculo(idCurriculo), new Origem(base.IdOrigem.Value)))
                    {
                        RedirectDownloadAnexo(new Curriculo(idCurriculo));
                        return;
                    }
                }

                if (base.IdPessoaFisicaLogada.HasValue && base.IdFilial.HasValue)
                {
                    var objFilial = new Filial(base.IdFilial.Value);

                    if (!EmpresaEmAuditoria(objFilial) && !EmpresaBloqueada(objFilial))
                    {
                        var objCurriculo = new Curriculo(idCurriculo);
                        var autorizacaoPelaWebEstagios = objFilial.AvalWebEstagios() && objCurriculo.CurriculoCompativelComEstagio();

                        if (CurriculoVisualizacao.FilialPodeVerDadosCurriculo(objFilial, objCurriculo, autorizacaoPelaWebEstagios))
                            RedirectDownloadAnexo(objCurriculo);
                        else
                            Redirect(GetRouteUrl(Enumeradores.RouteCollection.ProdutoCIA.ToString(), null));
                    }

                    //upPesquisa.Update(); 
                }
            }
            else if (e.CommandName.Equals("ChamarAgora"))
            {
                int idCurriculo = Convert.ToInt32(e.ListViewItem.OwnerListView.DataKeyValues[((RadListViewDataItem)e.ListViewItem).DisplayIndex]["Idf_Curriculo"]);
                ChamarAgora(idCurriculo);
            }
        }
        #endregion

        #region rlvCurriculos_ItemDataBound
        protected void rlvCurriculos_ItemDataBound(object sender, RadListViewItemEventArgs e)
        {
            int idCurriculo = Convert.ToInt32(e.Item.OwnerListView.DataKeyValues[((RadListViewDataItem)e.Item).DisplayIndex]["Idf_Curriculo"]);
            var rptEscolaridade = (Repeater)e.Item.FindControl("rptEscolaridade");
            var rptExperiencia = (Repeater)e.Item.FindControl("rptExperiencia");
            var rptFuncaoPretendida = (Repeater)e.Item.FindControl("rptFuncaoPretendida");
            var pnlMinhasExperiencias = (Panel)e.Item.FindControl("pnlMinhasExperiencias");


            var dtEscolaridade = new DataTable();
            var dtFuncaoPretendida = new DataTable();
            var dtExperiencia = new DataTable();

            var datasource = (DataTable)(e.Item).OwnerListView.DataSource;

            if (ResultadoPesquisaCurriculo.PorVaga()) //As buscas vindas da sala do selecionador > minhas vagas ainda estão no sql
            {
                dtEscolaridade = Curriculo.RecuperarEscolaridadeMiniCurriculo(idCurriculo);
                dtExperiencia = Curriculo.RecuperarExperienciaMiniCurriculo(idCurriculo);
                dtFuncaoPretendida = Curriculo.RecuperarFuncaoPretendidaMiniCurriculo(idCurriculo);
            }
            else
            {

                #region Escolaridade
                dtEscolaridade.Columns.Add("Des_Grau_Escolaridade");
                dtEscolaridade.Columns.Add("Des_Escolaridade");
                dtEscolaridade.Columns.Add("Des_Escolaridade_Fonte");
                dtEscolaridade.Columns.Add("Dta_Conclusao");

                var idfGrauEscolaridade = datasource.Rows[0]["Idf_Grau_Escolaridade_Formacao"].ToString();
                var descricaoGrauEscolaridade = datasource.Rows[0]["Des_Grau_Escolaridade_Formacao"];

                int idf;
                if (Int32.TryParse(idfGrauEscolaridade, out idf))
                {
                    if (idf == 3)
                        descricaoGrauEscolaridade = "Graduação:";
                    else if (idf == 4)
                        descricaoGrauEscolaridade = "Pós-Graduação:";
                    else
                        descricaoGrauEscolaridade += ":";

                    dtEscolaridade.Rows.Add(descricaoGrauEscolaridade,
                        BNE.BLL.Custom.Helper.AjustarString(datasource.Rows[0]["Des_Curso_Formacao"].ToString()),
                        BNE.BLL.Custom.Helper.AjustarString(datasource.Rows[0]["Des_Fonte_Formacao"].ToString()),
                        datasource.Rows[0]["Dta_Conclusao_Formacao"]);
                }
                #endregion

                #region Experiencia
                dtExperiencia.Columns.Add("Raz_Social");
                dtExperiencia.Columns.Add("Dta_Admissao");
                dtExperiencia.Columns.Add("Dta_Demissao");
                dtExperiencia.Columns.Add("Des_Funcao");
                dtExperiencia.Columns.Add("Des_Atividade");

                if (!string.IsNullOrWhiteSpace(datasource.Rows[0]["Experiencia_Raz_Social_1"].ToString()))
                {
                    dtExperiencia.Rows.Add(
                        MostrarNomeCandidatoEEmpresaNaExperienciaProfissional ? datasource.Rows[0]["Experiencia_Raz_Social_1"] : "Ramo: " + datasource.Rows[0]["Experiencia_Des_Area_BNE_1"],
                        datasource.Rows[0]["Experiencia_Dta_Admissao_1"],
                        datasource.Rows[0]["Experiencia_Dta_Demissao_1"],
                        datasource.Rows[0]["Experiencia_Des_Funcao_1"],
                        datasource.Rows[0]["Experiencia_Des_Atividade_1"]);
                }

                if (!string.IsNullOrWhiteSpace(datasource.Rows[0]["Experiencia_Raz_Social_2"].ToString()))
                {
                    dtExperiencia.Rows.Add(
                        MostrarNomeCandidatoEEmpresaNaExperienciaProfissional ? datasource.Rows[0]["Experiencia_Raz_Social_2"] : "Ramo: " + datasource.Rows[0]["Experiencia_Des_Area_BNE_2"],
                        datasource.Rows[0]["Experiencia_Dta_Admissao_2"],
                        datasource.Rows[0]["Experiencia_Dta_Demissao_2"],
                        datasource.Rows[0]["Experiencia_Des_Funcao_2"],
                        datasource.Rows[0]["Experiencia_Des_Atividade_2"]);
                }

                if (!string.IsNullOrWhiteSpace(datasource.Rows[0]["Experiencia_Raz_Social_3"].ToString()))
                {
                    dtExperiencia.Rows.Add(
                        MostrarNomeCandidatoEEmpresaNaExperienciaProfissional ? datasource.Rows[0]["Experiencia_Raz_Social_3"] : "Ramo: " + datasource.Rows[0]["Experiencia_Des_Area_BNE_3"],
                        datasource.Rows[0]["Experiencia_Dta_Admissao_3"],
                        datasource.Rows[0]["Experiencia_Dta_Demissao_3"],
                        datasource.Rows[0]["Experiencia_Des_Funcao_3"],
                        datasource.Rows[0]["Experiencia_Des_Atividade_3"]);
                }

                if (!string.IsNullOrWhiteSpace(datasource.Rows[0]["Experiencia_Raz_Social_4"].ToString()))
                {
                    dtExperiencia.Rows.Add(
                        MostrarNomeCandidatoEEmpresaNaExperienciaProfissional ? datasource.Rows[0]["Experiencia_Raz_Social_4"] : "Ramo: " + datasource.Rows[0]["Experiencia_Des_Area_BNE_4"],
                        datasource.Rows[0]["Experiencia_Dta_Admissao_4"],
                        datasource.Rows[0]["Experiencia_Dta_Demissao_4"],
                        datasource.Rows[0]["Experiencia_Des_Funcao_4"],
                        datasource.Rows[0]["Experiencia_Des_Atividade_4"]);
                }
                if (!string.IsNullOrWhiteSpace(datasource.Rows[0]["Experiencia_Raz_Social_5"].ToString()))
                {
                    dtExperiencia.Rows.Add(
                        MostrarNomeCandidatoEEmpresaNaExperienciaProfissional ? datasource.Rows[0]["Experiencia_Raz_Social_5"] : "Ramo: " + datasource.Rows[0]["Experiencia_Des_Area_BNE_5"],
                        datasource.Rows[0]["Experiencia_Dta_Admissao_5"],
                        datasource.Rows[0]["Experiencia_Dta_Demissao_5"],
                        datasource.Rows[0]["Experiencia_Des_Funcao_5"],
                        datasource.Rows[0]["Experiencia_Des_Atividade_5"]);
                }
                #endregion

                #region Funcao Pretendida
                dtFuncaoPretendida.Columns.Add("Des_Funcao");
                dtFuncaoPretendida.Columns.Add("Qtd_Experiencia");

                if (!string.IsNullOrWhiteSpace(datasource.Rows[0]["Funcao_Pretendida_1"].ToString()))
                    dtFuncaoPretendida.Rows.Add(datasource.Rows[0]["Funcao_Pretendida_1"], datasource.Rows[0]["Quantidade_Funcao_Pretendida_1"]);

                if (!string.IsNullOrWhiteSpace(datasource.Rows[0]["Funcao_Pretendida_2"].ToString()))
                    dtFuncaoPretendida.Rows.Add(datasource.Rows[0]["Funcao_Pretendida_2"], datasource.Rows[0]["Quantidade_Funcao_Pretendida_2"]);

                if (!string.IsNullOrWhiteSpace(datasource.Rows[0]["Funcao_Pretendida_3"].ToString()))
                    dtFuncaoPretendida.Rows.Add(datasource.Rows[0]["Funcao_Pretendida_3"], datasource.Rows[0]["Quantidade_Funcao_Pretendida_3"]);
                #endregion
            }

            UIHelper.CarregarRepeater(rptEscolaridade, dtEscolaridade);
            UIHelper.CarregarRepeater(rptExperiencia, dtExperiencia);
            var dt = (dtFuncaoPretendida.AsEnumerable()).Where(dr => Convert.ToInt32(dr["Qtd_Experiencia"]) > 0);
            if (dt.Any())
                pnlMinhasExperiencias.Visible = true;

            UIHelper.CarregarRepeater(rptFuncaoPretendida, dt);

            if (base.IdFilial.HasValue || base.IdUsuarioFilialPerfilLogadoUsuarioInterno.HasValue)
            {
                if (datasource.Columns.Contains("Nme_Anexo") && !string.IsNullOrWhiteSpace(datasource.Rows[0]["Nme_Anexo"].ToString()))
                    e.Item.FindControl("btiDownload").Visible = true;

                if (base.IdFilial.HasValue)
                    e.Item.FindControl("btiCvCompleto").Visible = !EmpresaBloqueada;
            }

            if (ResultadoPesquisaCurriculo.PorVaga())
            {
                Vaga objVaga = new Vaga(ResultadoPesquisaCurriculo.Vaga.IdVaga);

                if (objVaga.ExistePerguntas())
                {
                    var btiQuestionario = (LinkButton)e.Item.FindControl("btiQuestionario");
                    btiQuestionario.Visible = true;
                }
            }

            if (base.IdPessoaFisicaLogada.HasValue)
            {
                var divUltimoSalario = (HtmlControl)e.Item.FindControl("divUltimoSalario");
                divUltimoSalario.Visible = true;
            }
        }

        #endregion

        #region gvResultadoPesquisa_PageIndexChanged
        protected void gvResultadoPesquisa_PageIndexChanged(object source, GridPageChangedEventArgs e)
        {

            RadGrid grid;
            if (e.Item.OwnerGridID == "cphConteudo_gvResultadoPesquisaCampanha")
            {
                grid = gvResultadoPesquisaCampanha;
            }
            else
            {
                grid = gvResultadoPesquisa;
            }
            grid.CurrentPageIndex = e.NewPageIndex;
            gvResultadoPesquisa.CurrentPageIndex = e.NewPageIndex;
            if (!ResultadoPesquisaCurriculo.PorVaga())
                AtualizarUrl();
            else
                CarregarGridCurriculo(true);
            upCurriculos.Update();

            ScriptManager.RegisterStartupScript(upCurriculos, upCurriculos.GetType(), "AjustarRolagemParaTopo", "javaScript:AjustarRolagemParaTopo();", true);
        }
        #endregion

        #region PageSizeComboBox_SelectedIndexChanged
        void PageSizeComboBox_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            gvResultadoPesquisa.PageSize = Convert.ToInt32(e.Value);
            if (!ResultadoPesquisaCurriculo.PorVaga())
                AtualizarUrl();
            else
                CarregarGridCurriculo(true);

            ScriptManager.RegisterStartupScript(upCurriculos, upCurriculos.GetType(), "AjustarRolagemParaTopo", "javaScript:AjustarRolagemParaTopo();", true);
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
            if (ResultadoPesquisaCurriculo.PorVaga() && ResultadoPesquisaCurriculo.Vaga.Campanha)
            {
                var item = e.Item as GridGroupHeaderItem;
                if (item != null)
                {
                    item.Cells[0].Controls.Clear();
                    item.Cells[0].Visible = false;
                }
            }

            if (e.Item is GridPagerItem)
            {
                var valores = new Dictionary<string, string>()
                    {
                        {"20", "20"},
                        {"50", "50"},
                        {"100", "100"}
                    };

                var pageSizeCombo = (RadComboBox)e.Item.FindControl("PageSizeComboBox");

                pageSizeCombo.SelectedIndexChanged += PageSizeComboBox_SelectedIndexChanged;

                pageSizeCombo.Items.Clear();

                foreach (var valor in valores)
                {
                    var rcbi = new RadComboBoxItem(valor.Key, valor.Value);
                    rcbi.Attributes.Add("ownerTableViewId", e.Item.OwnerTableView.ClientID);
                    if (valor.Value == e.Item.OwnerTableView.PageSize.ToString())
                        rcbi.Selected = true;
                    pageSizeCombo.Items.Add(rcbi);
                }
            }
        }
        #endregion

        #region btnVoltar_Click
        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            if (base.UrlDestinoPagamento.HasValue)
            {
                string paginaRedirect = base.UrlDestinoPagamento.Value;
                base.UrlDestinoPagamento.Clear();
                Redirect(paginaRedirect);
            }
            else if (base.UrlDestino.HasValue)
            {
                string paginaRedirect = base.UrlDestino.Value;
                base.UrlDestino.Clear();

                //Se a pagina destino é a pesquisa de curriculos avançado, preencher os campos com os dados da ultima pesquis
                if (paginaRedirect.Equals("PesquisaCurriculoAvancada.aspx") && ResultadoPesquisaCurriculo.PorCurriculo())
                    Session.Add(Chave.Temporaria.Variavel10.ToString(), ResultadoPesquisaCurriculo.Curriculo.IdPesquisaCurriculo);

                Redirect(paginaRedirect);
            }
            else
            {
                if (!string.IsNullOrEmpty(UrlOrigem))
                {
                    //Se a pagina origem é a pesquisa de curriculos avançado, preencher os campos com os dados da ultima pesquisa
                    if (PaginaAspxOrigem.Equals("/PesquisaCurriculoAvancada.aspx") || PaginaAspxOrigem.Equals("/pesquisa-de-curriculo"))
                    {
                        if (ResultadoPesquisaCurriculo.PorCurriculo())
                            Session.Add(Chave.Temporaria.Variavel10.ToString(), ResultadoPesquisaCurriculo.Curriculo.IdPesquisaCurriculo);
                        Redirect(Page.GetRouteUrl(Enumeradores.RouteCollection.PesquisaCurriculoAvancada.ToString(), null));
                    }
                    else
                        Redirect(UrlOrigem);
                }
                else
                    Redirect("Default.aspx");
            }
        }
        #endregion

        #region rptExperiencia_ItemDataBound
        protected void rptExperiencia_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
                return;

            DataRowView drv = (DataRowView)e.Item.DataItem;
            Literal lt = (Literal)e.Item.FindControl("ltExperiencia");

            if (drv != null)
            {
                lt.Text = string.Format("<div><span class=\"descricao_bold\">{0}: </span>", drv["Raz_Social"]);
                lt.Text += string.Format(" de {0} - até {1} - ", Convert.ToDateTime(drv["Dta_Admissao"].ToString()).ToString("dd/MM/yyyy"), (drv["Dta_Demissao"] != DBNull.Value ? Convert.ToDateTime(drv["Dta_Demissao"].ToString()).ToString("dd/MM/yyyy") : "Emprego atual"));
                lt.Text += string.Format("<span class=\"texte_cor\">{0}</span></div>", BNE.BLL.Custom.Helper.CalcularTempoEmprego(drv["Dta_Admissao"].ToString(), (drv["Dta_Demissao"] != DBNull.Value ? drv["Dta_Demissao"].ToString() : DateTime.Now.ToString())));
                lt.Text += string.Format("<div class=\"espaco_descricao\"><span class=\"descricao_bold\">{0}: </span>", drv["Des_Funcao"]);
                lt.Text += string.Format("<span class=\"texte_cor\">{0}</span></div>", (drv["Des_Atividade"].ToString().Length > 90 ? drv["Des_Atividade"].ToString().Substring(0, 87) + "..." : drv["Des_Atividade"]));
            }
        }
        #endregion //rptExperiencia_ItemDataBound

        #region rptFuncaoPretendida_ItemDataBound
        protected void rptFuncaoPretendida_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
                return;

            DataRow drv = (DataRow)e.Item.DataItem;
            Literal lt = (Literal)e.Item.FindControl("ltFuncaoPretendida");

            if (drv != null)
            {
                var obj = new FuncaoPretendida { QuantidadeExperiencia = Convert.ToInt16(drv["Qtd_Experiencia"]), DescricaoFuncaoPretendida = drv["Des_Funcao"].ToString() };
                lt.Text = obj.ToString();
            }
        }
        #endregion

        #region btnNext_OnClick
        protected void btnNext_OnClick(object sender, EventArgs e)
        {
            if (gvResultadoPesquisa.CurrentPageIndex + 1 < gvResultadoPesquisa.PageCount)
            {
                gvResultadoPesquisa.CurrentPageIndex++;
                if (!ResultadoPesquisaCurriculo.PorVaga())
                    AtualizarUrl();
                else
                    CarregarGridCurriculo(true);
                ScriptManager.RegisterStartupScript(upCurriculos, upCurriculos.GetType(), upCurriculos.GetType().ToString(), "CallbackPaging();", true);
            }
        }
        #endregion

        #region btnPrevious_OnClick
        protected void btnPrevious_OnClick(object sender, EventArgs e)
        {
            gvResultadoPesquisa.CurrentPageIndex--;
            if (!ResultadoPesquisaCurriculo.PorVaga())
                AtualizarUrl();
            else
                CarregarGridCurriculo(true);
            ScriptManager.RegisterStartupScript(upCurriculos, upCurriculos.GetType(), upCurriculos.GetType().ToString(), "CallbackPaging();", true);
        }
        #endregion

        #region btnBasePaga_Click
        protected void btlBasePaga_Click(object sender, EventArgs e)
        {
            gvResultadoPesquisa.CurrentPageIndex = 0;
            BuscaBaseGratis = false;
            CarregarGridCurriculo(false);
            pnlBasePaga.Visible = false;
            pnlBaseGratis.Visible = true;
            upBase.Update();
        }
        #endregion

        #region btnBaseGratis_Click
        protected void btlBaseGratis_Click(object sender, EventArgs e)
        {
            gvResultadoPesquisa.CurrentPageIndex = 0;
            BuscaBaseGratis = true;
            CarregarGridCurriculo(false);
            pnlBasePaga.Visible = true;
            pnlBaseGratis.Visible = false;
            upBase.Update();
        }
        #endregion

        #endregion

        #region Métodos

        #region Inicializar
        private void Inicializar()
        {

            //Setando propriedades da radgrid
            gvResultadoPesquisaCampanha.GroupingEnabled = true;
            gvResultadoPesquisa.GroupingEnabled = true;
            gvResultadoPesquisa.PageSize = Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.QuantidadeItensPorPaginaPesquisaCurriculo));
            gvResultadoPesquisaCampanha.PageSize = Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.QuantidadeItensPorPaginaPesquisaCurriculo));
            ListCurriculosVisualizados = new List<Tuple<int, string, DateTime>>();

            //Setando visibilidade dos botoes de salas empresa e sala candidato
            AjustarPermissao();
            RecuperarInformacoesRota();
            IdPF.Value = base.IdUsuarioFilialPerfilLogadoEmpresa.HasValue ? base.IdPessoaFisicaLogada.Value.ToString() : string.Empty;
            IdFi.Value = base.IdFilial.HasValue ? base.IdFilial.Value.ToString() : string.Empty;
            if (Request.UrlReferrer != null)
            {
                UrlOrigem = Request.UrlReferrer.AbsoluteUri;
                PaginaAspxOrigem = Request.UrlReferrer.AbsolutePath;
            }

            EmpresaPossuePlanoNaoBloqueada = false;
            EmpresaBloqueada = EmpresaAssociacao = MostrarNomeCandidatoEEmpresaNaExperienciaProfissional = false;

            if (base.IdFilial.HasValue)
            {
                var objFilial = new Filial(base.IdFilial.Value);
                
                EmpresaBloqueada = objFilial.EmpresaBloqueada();
                EmpresaAssociacao = objFilial.EmpresaAssociacao();
                MostrarNomeCandidatoEEmpresaNaExperienciaProfissional = base.MostrarNomeCandidatoEEmpresaNaExperienciaProfissional();

                pnlBaseGratis.Visible = !objFilial.PossuiPlanoAtivo() && ResultadoPesquisaCurriculo.Tipo == ResultadoPesquisaCurriculo.TipoPesquisa.Curriculo;

                if (!objFilial.EmpresaEmAuditoria())
                {
                    EmpresaPossuePlanoNaoBloqueada = true;

                    ListCurriculosVisualizados = CurriculoQuemMeViu.ListarCurriculoVisualizados(base.IdFilial.Value);

                    EmpresaLogadaPossuiPlanoLiberado = PlanoAdquirido.ExistePlanoAdquiridoFilial(base.IdFilial.Value, Enumeradores.PlanoSituacao.Liberado);
                }
            }

            btnExcluirCurriculos.Visible = ResultadoPesquisaCurriculo.PorVaga();

            InicializarBarraBusca(TipoBuscaMaster.Curriculo, true, "PesquisaCurriculoCurriculos");

            CarregarGridCurriculo(false);
        }
        #endregion

        #region ChamarAgora
        protected void ChamarAgora(int idCurriculo)
        {
            if (!base.IdPessoaFisicaLogada.HasValue)
            {
                AbrirModalLogin();
                return;
            }

            if (!base.IdUsuarioFilialPerfilLogadoEmpresa.HasValue || !UsuarioFilialPerfil.ValidarTipoPerfil(base.IdUsuarioFilialPerfilLogadoEmpresa.Value, (int)Enumeradores.TipoPerfil.Empresa))
            {
                ExibirMensagem(MensagemAviso._101001, TipoMensagem.Aviso);
                return;
            }

            UsuarioFilial objUsuarioFilial;
            if (UsuarioFilial.CarregarUsuarioFilialPorUsuarioFilialPerfil(base.IdUsuarioFilialPerfilLogadoEmpresa.Value, out objUsuarioFilial))
                objUsuarioFilial.UsuarioFilialPerfil.CompleteObject();

            Filial objFilial = new Filial(base.IdFilial.Value);

            var parametros = new
            {
                telefone = String.Format("({0}) {1}", objUsuarioFilial.NumeroDDDComercial, objUsuarioFilial.NumeroComercial.Trim()),
                selecionador = objUsuarioFilial.UsuarioFilialPerfil.PessoaFisica.PrimeiroNome
            };

            var templateSMS = new CampanhaTanque().GetTextoCampanha(Enumeradores.CampanhaTanque.ChamarAgora);
            string conteudoMSG = parametros.ToString(templateSMS.mensagem);

            string mensagemErro;
            bool retornoEnvioMensagem;

            //Criar Objeto da Campanha
            CampanhaMensagem objCampanhaMensagem = new CampanhaMensagem();
            objCampanhaMensagem.DataDisparo = DateTime.Now;
            objCampanhaMensagem.DescricaomensagemEmail = conteudoMSG + " www.bne.com.br";
            objCampanhaMensagem.DescricaomensagemSMS = conteudoMSG + " www.bne.com.br";
            objCampanhaMensagem.FlagEnviaEmail = true;
            objCampanhaMensagem.FlagEnviaSMS = true;
            objCampanhaMensagem.UsuarioFilialPerfil = objUsuarioFilial.UsuarioFilialPerfil;
            objCampanhaMensagem.Save();

            //Cria registro do curriculo para envio
            CampanhaMensagemEnvios objCampanhaMensagemEnvios = new CampanhaMensagemEnvios();
            objCampanhaMensagemEnvios.Curriculo = new Curriculo(idCurriculo);
            objCampanhaMensagemEnvios.CampanhaMensagem = objCampanhaMensagem;
            objCampanhaMensagemEnvios.Save();

            retornoEnvioMensagem = EnvioMensagens.EnviarMensagemCV(objCampanhaMensagem, new List<CampanhaMensagemEnvios> { objCampanhaMensagemEnvios }, out mensagemErro, false, templateSMS.id, (int)Enumeradores.CampanhaTanque.ChamarAgora);

            if (!retornoEnvioMensagem)
            {
                ExibirMensagem(mensagemErro, TipoMensagem.Erro);
                return;
            }

            ucEnvioMensagem_EnviarConfirmacao("Confirmação", "Pronto, candidato avisado! Solicitamos que ele ligue no seu telefone.", false);

        }
        #endregion

        #region RecuperarInformacoesRota
        private void RecuperarInformacoesRota()
        {
            if (RouteData.Values.Count > 0)
            {
                if (RouteData.Values["Funcao"] != null)
                    base.FuncaoMaster.Value = RouteData.Values["Funcao"].ToString().DesnormalizarURL();

                if (RouteData.Values["Cidade"] != null && RouteData.Values["SiglaEstado"] != null)
                    base.CidadeMaster.Value = UIHelper.FormatarCidade(RouteData.Values["Cidade"].ToString(), RouteData.Values["SiglaEstado"].ToString());

                if (RouteData.Values["VagaCvsNaoVistos"] != null)
                    CarregarParametrosParaRota_VagaCvsNaoVistos(RouteData.Values["VagaCvsNaoVistos"].ToString());

                if ((base.FuncaoMaster.HasValue || base.CidadeMaster.HasValue) && !ResultadoPesquisaCurriculo.PorCurriculo()) //Se identificou que veio da rota valor para funcao e ou cidade e já não tem uma pesquisa atrelada
                {

                    BLL.PesquisaCurriculo objPesquisaCurriculo;
                    if (base.RecuperarDadosPesquisaCurriculo(base.FuncaoMaster.HasValue ? base.FuncaoMaster.Value : string.Empty, base.CidadeMaster.HasValue ? base.CidadeMaster.Value : string.Empty, string.Empty, out objPesquisaCurriculo))
                        ResultadoPesquisaCurriculo = new ResultadoPesquisaCurriculo(new ResultadoPesquisaCurriculoCurriculo { IdPesquisaCurriculo = objPesquisaCurriculo.IdPesquisaCurriculo });
                }
                if (RouteData.Values["idPesquisaAvancada"] != null)
                {
                    int idPesquisaAvancada = Convert.ToInt32(RouteData.Values["IdPesquisaAvancada"]);
                    ResultadoPesquisaCurriculo = new ResultadoPesquisaCurriculo(new ResultadoPesquisaCurriculoCurriculo { IdPesquisaCurriculo = idPesquisaAvancada });
                }
            }

            try
            {
                if (Request.QueryString["Pag"] != null)
                    gvResultadoPesquisa.CurrentPageIndex = Convert.ToInt32(Request.QueryString["Pag"]);
                else
                    gvResultadoPesquisa.CurrentPageIndex = 0;

                if (Request.QueryString["Itens"] != null)
                    gvResultadoPesquisa.PageSize = Convert.ToInt32(Request.QueryString["Itens"]);

            }
            catch (Exception)
            {
                gvResultadoPesquisa.CurrentPageIndex = 0;
            }
            
        }
        #endregion

        #region CarregarParametrosParaRota_VagaCvsNaoVistos
        private void CarregarParametrosParaRota_VagaCvsNaoVistos(string vagaCvsNaoVistos)
        {
            if (!base.IdFilial.HasValue || !base.IdUsuarioFilialPerfilLogadoEmpresa.HasValue)
                DeslogarUsuario();

            var idVaga = BNE.BLL.Custom.Helper.FromBase64(RouteData.Values["VagaCvsNaoVistos"].ToString());

            if (idVaga == "ErrorUnknownValue")
                DeslogarUsuario();

            var objVaga = BLL.Vaga.LoadObject(Convert.ToInt32(idVaga));
            objVaga.Filial.CompleteObject();

            //verifica se usuario_filial_perfil logado é o mesmo que anunciou a vaga
            if (objVaga.UsuarioFilialPerfil.IdUsuarioFilialPerfil == base.IdUsuarioFilialPerfilLogadoEmpresa.Value)
            {
                ResultadoPesquisaCurriculo = new ResultadoPesquisaCurriculo(new ResultadoPesquisaCurriculoVaga
                {
                    IdVaga = Convert.ToInt32(idVaga),
                    InscritosNaoLidos = true
                });
            }
            else
            {
                DeslogarUsuario(); //se ocorrer alguma ação inválida
            }
        }
        #endregion

        #region DeslogarUsuario
        private void DeslogarUsuario()
        {
            BNE.Auth.BNEAutenticacao.DeslogarPadrao(LogoffType.UNAUTHORIZED); //se ocorrer alguma ação inválida
            Redirect("/"); //Redireciona para Home do BNE
        }
        #endregion DeslogarUsuario

        #region CarregarGridCurriculo
        private void CarregarGridCurriculo(bool paginacao)
        {
            try
            {
                if(!base.DicCurriculos.HasValue)
                    base.DicCurriculos.Value = new Dictionary<int, bool>();

                BLL.PesquisaCurriculo objPesquisaCurriculo = null;

                int totalRegistros = 0;
                decimal valorMediaSalarial = 0;

                if (ResultadoPesquisaCurriculo.PorVaga() && base.IdFilial.HasValue) //Garante que tem alguma filial logada para evitar erros no fluxo
                {
                    if (ResultadoPesquisaCurriculo.Vaga.InscritosNaoLidos)
                    {
                        if (ResultadoPesquisaCurriculo.Vaga.Campanha)
                        {
                            gvResultadoPesquisa.Visible = false;
                            gvResultadoPesquisaCampanha.Visible = true;
                            UIHelper.CarregarRadGrid(gvResultadoPesquisaCampanha, BLL.PesquisaCurriculo.RecuperarCurriculosNaoVisualisadosVagaCampanha(gvResultadoPesquisaCampanha.PageSize, gvResultadoPesquisaCampanha.CurrentPageIndex, base.IdFilial.Value, ResultadoPesquisaCurriculo.Vaga.IdVaga, EmpresaAssociacao ? base.IdUsuarioFilialPerfilLogadoEmpresa.Value : (int?)null, out totalRegistros, out valorMediaSalarial), totalRegistros);
                        }
                        else
                            UIHelper.CarregarRadGrid(gvResultadoPesquisa, BLL.PesquisaCurriculo.RecuperarCurriculosNaoVisualisadosVaga(gvResultadoPesquisa.PageSize, gvResultadoPesquisa.CurrentPageIndex, base.IdFilial.Value, ResultadoPesquisaCurriculo.Vaga.IdVaga, EmpresaAssociacao ? base.IdUsuarioFilialPerfilLogadoEmpresa.Value : (int?)null, out totalRegistros, out valorMediaSalarial), totalRegistros);
                    }
                    else if (ResultadoPesquisaCurriculo.Vaga.BancoCurriculo)
                    {
                        var objVaga = Vaga.LoadObject(ResultadoPesquisaCurriculo.Vaga.IdVaga);
                        var retorno = BLL.PesquisaCurriculo.BuscaCurriculo(gvResultadoPesquisa.PageSize, gvResultadoPesquisa.CurrentPageIndex, base.IdOrigem.Value, base.IdFilial.HasValue ? base.IdFilial.Value : (int?)null, EmpresaAssociacao ? base.IdUsuarioFilialPerfilLogadoEmpresa.Value : (int?)null, objVaga, out totalRegistros, out valorMediaSalarial);

                        UIHelper.CarregarRadGrid(gvResultadoPesquisa, retorno, totalRegistros);
                    }
                    else
                    {
                        //Se for campanha entra no if condição e chama dois novos métodos que vão usar a query gerada pelo bruno e que vão setar o campo para 
                        if (ResultadoPesquisaCurriculo.Vaga.Campanha)
                        {
                            gvResultadoPesquisa.Visible = false;
                            gvResultadoPesquisaCampanha.Visible = true;
                            UIHelper.CarregarRadGrid(gvResultadoPesquisaCampanha, BLL.PesquisaCurriculo.RecuperarCurriculosRelacionadosVagaCampanha(gvResultadoPesquisaCampanha.PageSize, gvResultadoPesquisaCampanha.CurrentPageIndex, base.IdFilial.Value, ResultadoPesquisaCurriculo.Vaga.IdVaga, EmpresaAssociacao ? base.IdUsuarioFilialPerfilLogadoEmpresa.Value : (int?)null, out totalRegistros, out valorMediaSalarial), totalRegistros);
                        }
                        else
                            UIHelper.CarregarRadGrid(gvResultadoPesquisa, BLL.PesquisaCurriculo.RecuperarCurriculosRelacionadosVaga(gvResultadoPesquisa.PageSize, gvResultadoPesquisa.CurrentPageIndex, base.IdFilial.Value, ResultadoPesquisaCurriculo.Vaga.IdVaga, EmpresaAssociacao ? base.IdUsuarioFilialPerfilLogadoEmpresa.Value : (int?)null, out totalRegistros, out valorMediaSalarial), totalRegistros);
                    }
                }
                else if (ResultadoPesquisaCurriculo.PorRastreador())
                {
                    var objRastreadorCurriculo = RastreadorCurriculo.LoadObject(ResultadoPesquisaCurriculo.Rastreador.IdRastreadorCurriculo);
                    var retorno = BLL.PesquisaCurriculo.BuscaCurriculo(gvResultadoPesquisa.PageSize, gvResultadoPesquisa.CurrentPageIndex, base.IdOrigem.Value, base.IdFilial.HasValue ? base.IdFilial.Value : (int?)null, EmpresaAssociacao ? base.IdUsuarioFilialPerfilLogadoEmpresa.Value : (int?)null, objRastreadorCurriculo, ResultadoPesquisaCurriculo.Rastreador.DataVisualizacao, out totalRegistros, out valorMediaSalarial);

                    UIHelper.CarregarRadGrid(gvResultadoPesquisa, retorno, totalRegistros);
                }
                else
                {
                    if (ResultadoPesquisaCurriculo.PorCurriculo())
                        objPesquisaCurriculo = BLL.PesquisaCurriculo.LoadObject(ResultadoPesquisaCurriculo.Curriculo.IdPesquisaCurriculo);

                    if (objPesquisaCurriculo == null)
                        base.RecuperarDadosPesquisaCurriculo(string.Empty, string.Empty, string.Empty, out objPesquisaCurriculo);

                    var retorno = BLL.PesquisaCurriculo.BuscaCurriculo(gvResultadoPesquisa.PageSize, gvResultadoPesquisa.CurrentPageIndex, base.IdOrigem.Value, base.IdFilial.HasValue ? base.IdFilial.Value : (int?)null, EmpresaAssociacao ? base.IdUsuarioFilialPerfilLogadoEmpresa.Value : (int?)null, objPesquisaCurriculo, out totalRegistros, out valorMediaSalarial, BuscaBaseGratis);

                    UIHelper.CarregarRadGrid(gvResultadoPesquisa, retorno, totalRegistros);
                }

                PalavraChavePesquisa = string.Empty;

                //Funcao
                if (objPesquisaCurriculo != null)
                {
                    if (!string.IsNullOrEmpty(objPesquisaCurriculo.DescricaoPalavraChave))
                        PalavraChavePesquisa = objPesquisaCurriculo.DescricaoPalavraChave;

                    var funcoesPesquisa = PesquisaCurriculoFuncao.ListarIdentificadoresFuncaoPorPesquisa(objPesquisaCurriculo).Select(Funcao.LoadObject).ToList();

                    var objFuncao = funcoesPesquisa.FirstOrDefault();
                    if (objFuncao != null)
                    {
                        var descricaoJob = objFuncao.DescricaoJob;
                        if (descricaoJob.Length > 60)
                            descricaoJob = descricaoJob.Substring(0, 60) + "...";

                        lblDescricaoFuncao.Text = descricaoJob;
                      
                    }

                    if (!paginacao)
                        AjustarInformacaoPesquisa(objPesquisaCurriculo.Cidade, funcoesPesquisa.Select(x => x.DescricaoFuncao).ToList(), totalRegistros);
                }
                else
                {
                    if (!paginacao)
                        AjustarInformacaoPesquisa(null, null, totalRegistros);
                }
               
                upCurriculos.Update();

            }
            catch (Exception ex)
            {
                ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region AjustarPermissao
        private void AjustarPermissao()
        {
            if (base.STC.Value)
                btnCampanhaRecrutamento.Visible = false;

            upBotoes.Update();
        }
        #endregion

        #region AjustarInformacaoPesquisa
        private void AjustarInformacaoPesquisa(Cidade objCidade, List<string> listaFuncao, int totalRegistros)
        {
            var sb = new StringBuilder();
            bool SemFuncao = false;
            pnlBannerMedia.Visible = false;
            pnlBannerAtribuicoes.Visible = false;
            if (listaFuncao != null && listaFuncao.Any())
            {
                lblFuncao2.Text = lblFuncao.Text = string.Join(", ", listaFuncao.First());

                sb.Append(String.Format("{0} ", string.Join(", ", listaFuncao.First())));

                if (new Random().Next(0, 2) > 0)
                {
                    linkSalarioBrMedia.HRef = String.Format("http://www.salariobr.com.br/PesquisaSalarialPorPorte?funcao={0}&idadeDe=16&idadeAte=80&utm_source=SalarioBR&utm_medium=banner&utm_campaign=BneBannerMedia", lblFuncao2.Text);
                    pnlBannerMedia.Visible = true;
                    pnlBannerAtribuicoes.Visible = false;
                }
                else
                {
                     linkSalarioBrAtribuicoes.HRef = String.Format("http://www.salariobr.com.br/PesquisaSalarialPorPorte?funcao={0}&idadeDe=16&idadeAte=80&utm_source=SalarioBR&utm_medium=banner&utm_campaign=BneBannerAtribuicoes", lblFuncao2.Text);
                     pnlBannerAtribuicoes.Visible = true;
                     pnlBannerMedia.Visible = false;
                }
                    
            }
            else
            {
               SemFuncao = true;
               lblFuncao2.Text = lblFuncao.Text = "Todas as funções";
            }

            if (objCidade != null)
            {
                objCidade.CompleteObject();
                lblCidade2.Text = lblCidade.Text = String.Format("{0}/{1}", objCidade.NomeCidade, objCidade.Estado.SiglaEstado);
                sb.Append(String.Format("<b>{0}</b> ", objCidade.NomeCidade + "/" + objCidade.Estado.SiglaEstado));
            }
            else
            {
                lblCidade2.Text = lblCidade.Text = "todas as cidades";
                 sb.Append("<b>Todas</b> as Cidades");
            }

           sb.Append(String.Format("<b> ({0} Currículos</b>)", totalRegistros));

            if (SemFuncao)
                lblTituloResultadoPesquisa.Text = String.Format("Resultado da Pesquisa de Currículos - <b>Todas</b> as Funções em {0}", sb);
            else
                lblTituloResultadoPesquisa.Text = "Resultado da Pesquisa de Currículos";
            upInformacoesPesquisa.Update();
            upTitulo.Update();
        }
        #endregion

        #region InicializarEnvioCurriculo
        private void InicializarEnvioCurriculo(int? idCurriculo)
        {
            if (base.IdPessoaFisicaLogada.HasValue)
            {
                if (base.IdUsuarioFilialPerfilLogadoEmpresa.HasValue && UsuarioFilialPerfil.ValidarTipoPerfil(base.IdUsuarioFilialPerfilLogadoEmpresa.Value, (int)Enumeradores.TipoPerfil.Empresa))
                {
                    if (idCurriculo.HasValue)
                        ucEnvioCurriculo.ListIdCurriculos.Add(idCurriculo.Value);
                    else
                        ucEnvioCurriculo.ListIdCurriculos = RecuperarCurrriculosSelecionados();

                    if (ucEnvioCurriculo.ListIdCurriculos.Count.Equals(0)) //Nenhum curriculo foi selecionado
                        ExibirMensagem(MensagemAviso._101002, TipoMensagem.Aviso);
                    else
                    {
                        ucEnvioCurriculo.SetUltimoEmail();
                        //Passa os curriculos selecionados na pesquisa de curriculo
                        ucEnvioCurriculo.TipoEnvioCurriculo = TipoEnvioCurriculo.Empresa;
                        ucEnvioCurriculo.CarregarAssunto();
                        ucEnvioCurriculo.MostrarModal();
                    }
                }
                else
                    ExibirMensagem(MensagemAviso._101001, TipoMensagem.Aviso);
            }
            else
                AbrirModalLogin();
        }
        #endregion

        #region InicializarEnvioMensagem
        private void InicializarEnvioMensagem()
        {
            if (base.IdPessoaFisicaLogada.HasValue)
            {
                var lista = RecuperarCurrriculosSelecionados();
                if (lista.Count > 0)
                {
                    if (base.IdUsuarioFilialPerfilLogadoEmpresa.HasValue && UsuarioFilialPerfil.ValidarTipoPerfil(base.IdUsuarioFilialPerfilLogadoEmpresa.Value, (int)Enumeradores.TipoPerfil.Empresa))
                    {
                        //Recupera o(s) currículo(s) checado(s)                        
                        ucEnvioMensagem.Curriculos = RecuperarCurrriculosSelecionados();
                        ucEnvioMensagem.InicializarComponentes();
                    }
                    else //Somente empresas
                        ExibirMensagem(MensagemAviso._101001, TipoMensagem.Aviso);
                }
                else //Nenhum curriculo foi selecionado
                    ExibirMensagem(MensagemAviso._101002, TipoMensagem.Aviso);
            }
            else
                AbrirModalLogin();
        }
        #endregion

        #region InicializarEnvioMensagem_imgMensagemAcaoCarta
        private void InicializarEnvioMensagem_imgMensagemAcaoCarta(int? idCurriculo)
        {
            if (base.IdPessoaFisicaLogada.HasValue)
            {
                if (idCurriculo != null)
                {
                    if (base.IdUsuarioFilialPerfilLogadoEmpresa.HasValue && UsuarioFilialPerfil.ValidarTipoPerfil(base.IdUsuarioFilialPerfilLogadoEmpresa.Value, (int)Enumeradores.TipoPerfil.Empresa))
                    {
                        //Recupera apenas o currículo do clique no ícone de Ações 'Enviar Mensagem' (imgMensagem/carta) 
                        ucEnvioMensagem.Curriculos = new List<int> { idCurriculo.Value };
                        ucEnvioMensagem.InicializarComponentes();
                    }
                    else //Somente empresas
                        ExibirMensagem(MensagemAviso._101001, TipoMensagem.Aviso);
                }
                else //Nenhum curriculo foi selecionado
                    ExibirMensagem(MensagemAviso._101002, TipoMensagem.Aviso);
            }
            else
                AbrirModalLogin();
        }
        #endregion

        #region InicializarBuscaAvancada
        private void InicializarBuscaAvancada()
        {
            if (base.IdPessoaFisicaLogada.HasValue)
            {
                if (base.IdFilial.HasValue && base.IdUsuarioFilialPerfilLogadoEmpresa.HasValue && UsuarioFilialPerfil.ValidarTipoPerfil(base.IdUsuarioFilialPerfilLogadoEmpresa.Value, (int)Enumeradores.TipoPerfil.Empresa))
                {
                    if (ResultadoPesquisaCurriculo.PorCurriculo())
                        Session.Add(Chave.Temporaria.Variavel10.ToString(), ResultadoPesquisaCurriculo.Curriculo.IdPesquisaCurriculo);
                    Redirect(GetRouteUrl(Enumeradores.RouteCollection.PesquisaCurriculoAvancada.ToString(), null));
                }
                else //Somente empresas
                    ExibirMensagem(MensagemAviso._101001, TipoMensagem.Aviso);
            }
            else
            {
                ClicouPesquisaAvancada = true;
                Redirect(Page.GetRouteUrl(Enumeradores.RouteCollection.PesquisaCurriculoAvancada.ToString(), null));
            }
        }
        #endregion

        #region AbrirModalLogin
        private void AbrirModalLogin()
        {
            ucModalLogin.InicializarEmpresa();
            ucModalLogin.Mostrar();
            pnlL.Visible = true;
            upL.Update();
        }
        #endregion

        #region RetornarUrlFoto
        protected string RetornarUrlFoto(decimal numeroCPF)
        {
            return UIHelper.RetornarUrlFoto(numeroCPF, PessoaFisicaFoto.OrigemFoto.Local);
        }
        #endregion

        #region RetornarNome
        protected string RetornarNome(string nomeCompleto, bool flagVIP)
        {
            if (base.IdFilial.HasValue)
            {
                if (flagVIP && EmpresaLogadaPossuiPlanoLiberado)
                    return nomeCompleto;
                if (EmpresaPossuePlanoNaoBloqueada && EmpresaLogadaPossuiPlanoLiberado)
                    return nomeCompleto;
            }

            return PessoaFisica.RetornarPrimeiroNome(nomeCompleto);
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
                // TODO Melhorar o tratamento para as URL's de curriculo sem função ou cidade.
                try
                {
                    var funcoes = funcao.Split(';').Where(f => !string.IsNullOrEmpty(f));
                    nomeFuncao = funcoes.ElementAt(0);
                }
                catch (Exception)
                {
                    nomeFuncao = " ";
                }
            }

            var url = SitemapHelper.MontarUrlVisualizacaoCurriculo(nomeFuncao, nomeCidade, siglaEstado, identificadorCurriculo);

            //Passando o id da pesquisa na url, para quando visualizar o currículo identificar que é desta pesquisa
            if (ResultadoPesquisaCurriculo.PorCurriculo())
                url = string.Format("{0}?idpesquisacurriculo={1}", url, ResultadoPesquisaCurriculo.Curriculo.IdPesquisaCurriculo);

            if (ResultadoPesquisaCurriculo.PorRastreador())
                url = string.Format("{0}?idrastreadorcurriculo={1}", url, ResultadoPesquisaCurriculo.Rastreador.IdRastreadorCurriculo);

            if (ResultadoPesquisaCurriculo.PorVaga())
                url = string.Format("{0}?idvaga={1}", url, ResultadoPesquisaCurriculo.Vaga.IdVaga);

            return url;
        }
        #endregion

        #region AjustarMostrarModalConfirmacaoEnvio
        private void AjustarMostrarModalConfirmacaoEnvio(bool show, bool hide, string titulo, string texto, bool aviso)
        {
            _modalConfimacaoEnvio = Page.LoadControl("~/UserControls/Modais/ModalConfirmacao.ascx");
            pnlConfirmacaoEnvio.Controls.Add(_modalConfimacaoEnvio);
            ((UserControls.Modais.ModalConfirmacao)_modalConfimacaoEnvio).CliqueAqui += ucModalConfirmacao_CliqueAqui;
            if (show)
            {
                ((UserControls.Modais.ModalConfirmacao)_modalConfimacaoEnvio).PreencherCampos(titulo, texto, aviso);
                ((UserControls.Modais.ModalConfirmacao)_modalConfimacaoEnvio).MostrarModal();
                BoolModalConfirmacaoEnvio = true;
            }
            else if (hide)
            {
                ((UserControls.Modais.ModalConfirmacao)_modalConfimacaoEnvio).FecharModal();
                BoolModalConfirmacaoEnvio = false;
            }

            upConfirmacaoEnvio.Update();
        }
        #endregion

        #region AjustarVisualizacaoDadosCurriculo
        private void AjustarVisualizacaoDadosCurriculo(GridCommandEventArgs e)
        {
            RadGrid grid;
            if (e.Item.OwnerGridID == "cphConteudo_gvResultadoPesquisaCampanha")
            {
                grid = gvResultadoPesquisaCampanha;
            }
            else
            {
                grid = gvResultadoPesquisa;
            }
            int idCurriculo = Convert.ToInt32(grid.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Curriculo"]);

            ((GridDataItem)e.Item).ChildItem.FindControl("ContainerCurriculo").Visible = !e.Item.Expanded;

            //Ajustando design
            var gtc = (GridTableCell)(((GridDataItem)e.Item).ChildItem.FindControl("ContainerCurriculo").Parent).Parent;
            gtc.Attributes.Add("style", "padding: 0px !important");

            var btlNomeCurriculo = (LinkButton)e.Item.FindControl("lblNomeCurriculo");
            var btlQuemVisualiza = (Label)e.Item.FindControl("lblQuemVisualiza");
            if (!e.Item.Expanded)
            {
                if (base.IdFilial.HasValue)
                {
                    var objFilial = new Filial(base.IdFilial.Value);
                    CurriculoQuemMeViu objCurriculoQuemMeViu = null;
                    if (!objFilial.EmpresaBloqueada())
                        objCurriculoQuemMeViu = CurriculoQuemMeViu.SalvarQuemMeViuSite(objFilial, new Curriculo(idCurriculo), base.IdUsuarioFilialPerfilLogadoEmpresa.Value);
                    if (objCurriculoQuemMeViu != null)
                        ListCurriculosVisualizados.Add(new Tuple<int, string, DateTime>(idCurriculo, UsuarioFilialPerfil.RecuperarNomeUsuario(objCurriculoQuemMeViu.UsuarioFilialPerfil.IdUsuarioFilialPerfil), objCurriculoQuemMeViu.DataQuemMeViu));

                    BLL.PesquisaCurriculo objPesquisaCurriculo = null;
                    if (ResultadoPesquisaCurriculo.PorCurriculo())
                        objPesquisaCurriculo = new BLL.PesquisaCurriculo(ResultadoPesquisaCurriculo.Curriculo.IdPesquisaCurriculo);

                    BLL.RastreadorCurriculo objRastreadorCurriculo = null;
                    if (ResultadoPesquisaCurriculo.PorRastreador())
                        objRastreadorCurriculo = new BLL.RastreadorCurriculo(ResultadoPesquisaCurriculo.Rastreador.IdRastreadorCurriculo);

                    BLL.Vaga objVaga = null;
                    if (ResultadoPesquisaCurriculo.PorVaga())
                        objVaga = new BLL.Vaga(ResultadoPesquisaCurriculo.Vaga.IdVaga);

                    if (ResultadoPesquisaCurriculo.Vaga != null)
                        VagaCandidato.SalvarVisualizacaoCandidato(idCurriculo, ResultadoPesquisaCurriculo.Vaga.IdVaga);


                    CurriculoVisualizacaoHistorico.SalvarHistoricoVisualizacao(objFilial, new UsuarioFilialPerfil(base.IdUsuarioFilialPerfilLogadoEmpresa.Value), new Curriculo(idCurriculo), false, PageHelper.RecuperarIP(), objPesquisaCurriculo, objVaga, objRastreadorCurriculo);
                }

                #region Mostra currículo anexo caso exista no SINE
                Curriculo cv = new Curriculo(idCurriculo);
                CurriculoOrigem OrigemSine = cv.RecuperarListCurriculoOrigem().Find(org => org.Origem.IdOrigem == 2);
                if (OrigemSine != null)
                {

                    string sURL;

                    cv.CompleteObject();
                    cv.PessoaFisica.CompleteObject();

#if DEBUG
                    sURL = String.Format("http://localhost:51899/v1.0/User/ArquivoExiste?cpf={0}", cv.PessoaFisica.CPF);
#else
                    string api_target = Parametro.RecuperaValorParametro(Enumeradores.Parametro.SineApi);
                    sURL = String.Format("{0}/User/ArquivoExiste?cpf={1}", api_target, cv.PessoaFisica.CPF);
#endif

                    WebRequest wrGETURL;
                    wrGETURL = WebRequest.Create(sURL);
                    try
                    {
                        HttpWebResponse response = (HttpWebResponse)wrGETURL.GetResponse();

                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            StreamReader objReader = new StreamReader(response.GetResponseStream());
                            ArquivoExisteResult objResult = new JavaScriptSerializer().Deserialize<ArquivoExisteResult>(objReader.ReadToEnd());

                            if (objResult.Message && IdFilial.HasValue)
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ValidadeSineCV", "ExibirDownloadSineCV(" + idCurriculo + ");", true);
                            }

                        }
                    }
                    catch (Exception) { }
                }
                #endregion

            }


            //Ajustando visualizacao de curriculo
            IdCurriculoPesquisaCurriculo = idCurriculo;

            if (base.IdFilial.HasValue)
            {
                if (ListCurriculosVisualizados.Where(t => t.Item1.Equals(idCurriculo)).OrderByDescending(t => t.Item3).Any())
                {
                    btlNomeCurriculo.CssClass = "tooltipB balao nome_descricao_curriculo_visited";
                    string Nome = ListCurriculosVisualizados.Where(t => t.Item1.Equals(idCurriculo)).OrderByDescending(a => a.Item3).Select(s => s.Item2).FirstOrDefault();
                    var DtaVisualizacao = ListCurriculosVisualizados.Where(t => t.Item1 == idCurriculo).OrderByDescending(a => a.Item3).Select(t => t.Item3).FirstOrDefault();
                    if (!String.IsNullOrEmpty(Nome))
                    {
                        btlQuemVisualiza.Text = String.Format("Último a visualizar: {0} em {1} ", Nome, DtaVisualizacao);
                        btlQuemVisualiza.Visible = true;
                    }
                }
                else
                    btlNomeCurriculo.CssClass = "nome_descricao_curriculo_padrao";
            }


            upCurriculos.Update();
        }
        #endregion

        #region RedirectDownloadAnexo
        /// <summary>
        // Metodo responsavel por fazer o redirect do Download de arquivos do curriculo
        /// </summary>
        /// <returns></returns>
        private void RedirectDownloadAnexo(Curriculo objCurriculo)
        {
            int idPesoaFisica = PessoaFisica.RecuperarIdPorCurriculo(objCurriculo);
            objCurriculo.PessoaFisica = PessoaFisica.LoadObject(idPesoaFisica);

            PessoaFisicaComplemento objPessoaFisicaComplemento;
            if (PessoaFisicaComplemento.CarregarPorPessoaFisica(PessoaFisica.RecuperarIdPorCurriculo(new Curriculo(objCurriculo.IdCurriculo)), out objPessoaFisicaComplemento))
            {
                if (objPessoaFisicaComplemento.NomeAnexo != null && objPessoaFisicaComplemento.NomeAnexo != "")
                {
                    string objPath = objCurriculo.PessoaFisica.CPF + "_" + objPessoaFisicaComplemento.NomeAnexo;

                    if (BLL.StorageManager.ArquivoExiste("curriculos", objPath))
                    {
                        EncaminharParaDownload(objPessoaFisicaComplemento.NomeAnexo, BLL.StorageManager.CarregarArquivo("curriculos", objPath));
                    }
                }
            }
        }
        #endregion

        #region EncaminharParaDownload
        private void EncaminharParaDownload(string file_name, byte[] filebytes)
        {
            string serverName = Request.ServerVariables["HTTP_HOST"];
            Session.Add(Chave.Temporaria.Variavel1.ToString(), filebytes);
            Session.Add(Chave.Temporaria.Variavel2.ToString(), file_name);
            ScriptManager.RegisterStartupScript(this, GetType(), "AbrirPopup", string.Format("AbrirPopup('http://{0}/DownloadAnexo.aspx', 600, 800);", serverName), true);
        }
        #endregion

        #region BaixarCVSine
        private void BaixarCVSine(int IdCv)
        {
            string sURL;

            Curriculo objCurriculo = new Curriculo(IdCv);
            objCurriculo.CompleteObject();
            objCurriculo.PessoaFisica.CompleteObject();

#if DEBUG
            sURL = String.Format("http://localhost:51899/v1.0/User/RecuperarArquivo?cpf={0}", objCurriculo.PessoaFisica.CPF);
#else
            string api_target = Parametro.RecuperaValorParametro(Enumeradores.Parametro.SineApi);
            sURL = String.Format("{0}/User/RecuperarArquivo?cpf={1}", api_target, objCurriculo.PessoaFisica.CPF);
#endif

            WebRequest wrGETURL;
            wrGETURL = WebRequest.Create(sURL);
            try
            {
                HttpWebResponse response = (HttpWebResponse)wrGETURL.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    StreamReader objReader = new StreamReader(response.GetResponseStream());
                    AnexoCV anexoCV = new JavaScriptSerializer().Deserialize<AnexoCV>(objReader.ReadToEnd());
                    EncaminharParaDownload(anexoCV.Nome, Convert.FromBase64String(anexoCV.Arquivo));
                }
                else
                {
                    base.ExibirMensagem("Não foi possível fazer o download do arquivo desejado.", TipoMensagem.Erro);
                }
            }
            catch (System.Net.WebException ex)
            {
                base.ExibirMensagem(ex.Message, TipoMensagem.Erro);
            }
            catch (Exception ex)
            {
                base.ExibirMensagem(String.Format("Problema de genérico de acesso a API: {0}", ex.Message), TipoMensagem.Erro);
            }

        }
        #endregion

        #region Check
        public bool Check(string propName)
        {
            try
            {
                var obj = Eval(propName);

                return obj != null;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region RecuperarCurrriculosSelecionados
        private List<int> RecuperarCurrriculosSelecionados()
        {
            return base.DicCurriculos.Value.Where(d => d.Value).Select(d => d.Key).ToList();
        }
        #endregion

        #region AtualizarUrl
        private void AtualizarUrl()
        {
            StringBuilder url = new StringBuilder();

            if (ResultadoPesquisaCurriculo.Curriculo != null)
            {
                url.Append("/lista-de-curriculos/");
                url.Append(ResultadoPesquisaCurriculo.Curriculo.IdPesquisaCurriculo.ToString());
            }
            else
                url.Append(HttpContext.Current.Request.Path);

            url.Append("?pag=" + gvResultadoPesquisa.CurrentPageIndex);
            url.Append("&itens=" + gvResultadoPesquisa.PageSize);
            Response.Redirect(url.ToString());
        }
        #endregion

        #endregion
                
    }
}