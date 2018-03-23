using System.Diagnostics;
using BNE.BLL;
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
using Telerik.Web.UI;
using Enumeradores = BNE.BLL.Enumeradores;
using Label = System.Web.UI.WebControls.Label;
using PessoaFisicaFoto = BNE.Web.Handlers.PessoaFisicaFoto;

namespace BNE.Web
{
    public partial class PesquisaCurriculo : BasePage
    {

        private Control _modalConfimacaoEnvio;

        #region Propriedades

        #region DicCurriculos - Variável 1
        /// <summary>
        /// Propriedade que armazena e recupera o ID
        /// </summary>
        private Dictionary<int, bool> DicCurriculos
        {
            get
            {
                return (Dictionary<int, bool>)(ViewState[Chave.Temporaria.Variavel1.ToString()]);
            }
            set
            {
                ViewState[Chave.Temporaria.Variavel1.ToString()] = value;
            }
        }
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
        private List<int> ListCurriculosVisualizados
        {
            get
            {
                return (List<int>)(ViewState[Chave.Temporaria.Variavel5.ToString()]);
            }
            set
            {
                ViewState[Chave.Temporaria.Variavel5.ToString()] = value;
            }
        }
        #endregion

        #region IdPesquisaCurriculo - Variável 6
        /// <summary>
        /// Propriedade que armazena e recupera o IdPesquisaCurriculo
        /// </summary>
        private int? IdPesquisaCurriculo
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel6.ToString()] != null)
                    return Int32.Parse(ViewState[Chave.Temporaria.Variavel6.ToString()].ToString());
                return null;
            }
            set
            {
                if (value != null)
                    ViewState.Add(Chave.Temporaria.Variavel6.ToString(), value);
                else
                    ViewState.Remove(Chave.Temporaria.Variavel6.ToString());
            }
        }
        #endregion

        #region IdVagaSession - Variável 7
        /// <summary>
        /// Propriedade que armazena e recupera o ID
        /// </summary>
        private int? IdVaga
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel7.ToString()] != null)
                    return Int32.Parse(ViewState[Chave.Temporaria.Variavel7.ToString()].ToString());
                return null;
            }
            set
            {
                if (value != null)
                {
                    base.IdVaga.Value = (int)value;
                    ViewState.Add(Chave.Temporaria.Variavel7.ToString(), value);
                }
                else
                {
                    base.IdVaga.Clear();
                    ViewState.Remove(Chave.Temporaria.Variavel7.ToString());
                }
            }
        }
        #endregion

        #region BoolCandidatosNaoVisualizados - Variável 8
        public bool BoolCandidatosNaoVisualizados
        {
            get
            {
                return Convert.ToBoolean(ViewState[Chave.Temporaria.Variavel8.ToString()]);
            }
            set
            {
                ViewState[Chave.Temporaria.Variavel8.ToString()] = value;
            }
        }
        #endregion

        #region BoolCandidatosNoPerfil - Variável 11
        public bool BoolCandidatosNoPerfil
        {
            get
            {
                return Convert.ToBoolean(ViewState[Chave.Temporaria.Variavel11.ToString()]);
            }
            set
            {
                ViewState[Chave.Temporaria.Variavel11.ToString()] = value;
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

        #region ClicouComparar - Variável 14
        private bool ClicouComparar
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel14.ToString()] != null)
                    return Convert.ToBoolean(ViewState[Chave.Temporaria.Variavel14.ToString()]);

                return false;
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Variavel14.ToString(), value);
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

        #endregion

        #region Eventos

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            IdVaga = IdVaga;

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

            var principal = (Principal)Master;
            if (principal != null) principal.PesquisaRapidaCurriculo += PesquisaCurriculo_PesquisaRapidaCurriculo;

            ucConfirmacaoExclusao.Cancelar += ucConfirmacaoExclusao_Cancelar;
            ucConfirmacaoExclusao.Confirmar += ucConfirmacaoExclusao_Confirmar;
        }

        void ucEnvioMensagem_EnviarConfirmacao(string titulo, string mensagem, bool cliqueAqui)
        {
            DicCurriculos = new Dictionary<int, bool>();
            ucModalConfirmacao.PreencherCampos(titulo, mensagem, cliqueAqui);
            ucModalConfirmacao.MostrarModal();
            pnlM.Visible = true;
            upM.Update();
        }
        void ucEnvioCurriculo_EnviarConfirmacao()
        {
            DicCurriculos = new Dictionary<int, bool>();
            ucModalConfirmacao.PreencherCampos("Confirmação", "Currículo enviado com sucesso!", false, "OK");
            ucModalConfirmacao.MostrarModal();
            pnlM.Visible = true;
            upM.Update();
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

        #region btnR1_Click
        protected void btnR1_Click(object sender, EventArgs e)
        {
            Redirect(Page.GetRouteUrl(Enumeradores.RouteCollection.ApresentarR1.ToString(), null));
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
            if (IdCurriculoPesquisaCurriculo.HasValue)
            {
                VagaCandidato objVagaCandidato;
                if (VagaCandidato.CarregarPorVagaCurriculo(IdVaga.Value, IdCurriculoPesquisaCurriculo.Value, out objVagaCandidato))
                    objVagaCandidato.Inativar();
            }
            else
                VagaCandidato.Inativar(new Vaga(IdVaga.Value), RecuperarCurrriculosSelecionados());

            CarregarGridCurriculo(false);
            DicCurriculos = new Dictionary<int, bool>();

            IdCurriculoPesquisaCurriculo = null;
            ucConfirmacaoExclusao.FecharModal();
        }
        #endregion

        #region gvResultadoPesquisa_ItemDataBound
        protected void gvResultadoPesquisa_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                string id = gvResultadoPesquisa.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Curriculo"].ToString();

                e.Item.FindControl("btiExcluirCurriculo").Visible = IdVaga.HasValue;
                e.Item.FindControl("btiAssociar").Visible = base.IdFilial.HasValue;
                e.Item.FindControl("btiCompleto").Visible = !EmpresaBloqueada;

                if (IdVaga.HasValue)
                    ((HtmlGenericControl)e.Item.FindControl("divIcones")).Attributes.Add("class", "icones_pesquisa_curriculo_vagas icones");

                var imgAvaliacao = (LinkButton)e.Item.FindControl("imgAvaliacao");
                var lblSMS = (Label)e.Item.FindControl("lblSMS");

                //Ajustando balão saiba mais
                var bsm = (BalaoSaibaMais)e.Item.FindControl("bsmAvaliacao");
                bsm.TargetControlID = imgAvaliacao.ClientID;
                var bsmSMS = (BalaoSaibaMais)e.Item.FindControl("bsmSMS");
                bsmSMS.TargetControlID = lblSMS.ClientID;

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
                ckbDataItem.Attributes.Add("CommandArgument", id);

                if (DicCurriculos.ContainsKey(Convert.ToInt32(id)))
                    ckbDataItem.Checked = DicCurriculos[Convert.ToInt32(id)];

                if (base.IdFilial.HasValue)
                {
                    if (ListCurriculosVisualizados.Contains(Convert.ToInt32(id)))
                    {
                        var btlNomeCurriculo = (LinkButton)e.Item.FindControl("lblNomeCurriculo");
                        btlNomeCurriculo.CssClass = "nome_descricao_curriculo_visited";
                    }
                }
            }

            if (e.Item is GridNestedViewItem)
            {
                var dt = (DataTable)gvResultadoPesquisa.DataSource;
                var gridDataItem = ((GridNestedViewItem)e.Item).ParentItem as GridDataItem;
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
            DicCurriculos[Convert.ToInt32(ckbDataItem.Attributes["CommandArgument"])] = ckbDataItem.Checked;
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
                DicCurriculos[idCurriculo] = ckbDataItem.Checked = ckbHeaderItem.Checked;
            }

            upCurriculos.Update();
        }
        #endregion

        #region gvResultadoPesquisa_ItemCommand
        protected void gvResultadoPesquisa_ItemCommand(object source, GridCommandEventArgs e)
        {
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
                int idCurriculo = Convert.ToInt32(gvResultadoPesquisa.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Curriculo"]);

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
                int idCurriculo = Convert.ToInt32(gvResultadoPesquisa.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Curriculo"]);

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
                IdCurriculoPesquisaCurriculo = Convert.ToInt32(gvResultadoPesquisa.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Curriculo"]);

                var objPessoaFisica = new PessoaFisica(PessoaFisica.RecuperarIdPorCurriculo(new Curriculo((int)IdCurriculoPesquisaCurriculo)));

                ucConfirmacaoExclusao.Inicializar("Atenção!", "Tem certeza que deseja excluir o currículo de " + objPessoaFisica.NomeCompleto + " da sua lista?");
                ucConfirmacaoExclusao.MostrarModal();
            }
            else if (e.CommandName.Equals("Associar"))
            {
                var objFilial = new Filial(base.IdFilial.Value);
                if (!EmpresaBloqueada(objFilial))
                {
                int idCurriculo = Convert.ToInt32(gvResultadoPesquisa.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Curriculo"]);
                ucAssociarCurriculoVaga.Inicializar(idCurriculo);
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
                if (ClicouComparar)
                    AjustarRedirectComparaCv();
                else if (ClicouPesquisaAvancada)
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
                    base.UrlDestino.Value = "PesquisaCurriculo.aspx";
                    Redirect(Page.GetRouteUrl(Enumeradores.RouteCollection.AnunciarVaga.ToString(), null));
                }
                else //Somente empresas
                    ExibirMensagem(MensagemAviso._101001, TipoMensagem.Aviso);
            }
            else
                Redirect(GetRouteUrl(Enumeradores.RouteCollection.LoginComercialEmpresa.ToString(), new { Destino = Enumeradores.LoginEmpresaDestino.AnunciarVaga }));
        }
        #endregion

        #region btnBuscaAvancada_Click
        protected void btnBuscaAvancada_Click(object sender, EventArgs e)
        {
            InicializarBuscaAvancada();
        }
        #endregion

        #region btnComparar_Click
        protected void btnComparar_Click(object sender, EventArgs e)
        {
            if (base.IdPessoaFisicaLogada.HasValue)
                AjustarRedirectComparaCv();
            else
            {
                ClicouComparar = true;
                AbrirModalLogin();
            }
        }
        #endregion

        #region PesquisaCurriculo_PesquisaRapidaCurriculo
        void PesquisaCurriculo_PesquisaRapidaCurriculo(int idPesquisaCurriculo)
        {
            IdPesquisaCurriculo = idPesquisaCurriculo; //Limpando a view state para não fazer a pesquisa errada.
            gvResultadoPesquisa.CurrentPageIndex = 0; //Ajustando para a primeira página para evitar erro na pesquisa.
            IdVaga = null;//Limpando o IdVagaSession para fazer a pesquisa certa.
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

                ((UserControls.Modais.ModalQuestionarioVagas)questionario).Inicializar(IdVaga.Value, idCurriculo);
                ((UserControls.Modais.ModalQuestionarioVagas)questionario).MostrarModal();

                upCphModais.Update();
            }
            else if (e.CommandName.Equals("DownloadAnexo"))
            {
                int idCurriculo = Convert.ToInt32(e.ListViewItem.OwnerListView.DataKeyValues[((RadListViewDataItem)e.ListViewItem).DisplayIndex]["Idf_Curriculo"]);

                if (base.STC.Value)
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

                    upPesquisa.Update();
                }
            }
        }
        #endregion

        #region rlvCurriculos_ItemDataBound
        protected void rlvCurriculos_ItemDataBound(object sender, RadListViewItemEventArgs e)
        {
            int idCurriculo = Convert.ToInt32(e.Item.OwnerListView.DataKeyValues[((RadListViewDataItem)e.Item).DisplayIndex]["Idf_Curriculo"]);
            var rptEscolaridade = (Repeater)e.Item.FindControl("rptEscolaridade");
            var rptExperiencia = (Repeater)e.Item.FindControl("rptExperiencia");

            var datasource = (DataTable)(e.Item).OwnerListView.DataSource;

            DataTable dtEscolaridade;
            DataTable dtExperiencia;

            if (BLL.PesquisaCurriculo.BuscarSolr && !IdVaga.HasValue) //As buscas vindas da sala do selecionador > minhas vagas ainda estão no sql
            {
                #region Excolaridade
                dtEscolaridade = new DataTable();
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

                    dtEscolaridade.Rows.Add(descricaoGrauEscolaridade,
                        datasource.Rows[0]["Des_Curso_Formacao"],
                        datasource.Rows[0]["Des_Fonte_Formacao"],
                        datasource.Rows[0]["Dta_Conclusao_Formacao"]);
                }
                #endregion

                #region Experiencia
                dtExperiencia = new DataTable();
                dtExperiencia.Columns.Add("Raz_Social");
                dtExperiencia.Columns.Add("Dta_Admissao");
                dtExperiencia.Columns.Add("Dta_Demissao");
                dtExperiencia.Columns.Add("Des_Funcao");
                dtExperiencia.Columns.Add("Des_Atividade");

                if (!string.IsNullOrWhiteSpace(datasource.Rows[0]["Experiencia_Raz_Social_1"].ToString()))
                {
                    dtExperiencia.Rows.Add(
                        datasource.Rows[0]["Experiencia_Raz_Social_1"],
                        datasource.Rows[0]["Experiencia_Dta_Admissao_1"],
                        datasource.Rows[0]["Experiencia_Dta_Demissao_1"],
                        datasource.Rows[0]["Experiencia_Des_Funcao_1"],
                        datasource.Rows[0]["Experiencia_Des_Atividade_1"]);
                }

                if (!string.IsNullOrWhiteSpace(datasource.Rows[0]["Experiencia_Raz_Social_2"].ToString()))
                {
                    dtExperiencia.Rows.Add(
                        datasource.Rows[0]["Experiencia_Raz_Social_2"],
                        datasource.Rows[0]["Experiencia_Dta_Admissao_2"],
                        datasource.Rows[0]["Experiencia_Dta_Demissao_2"],
                        datasource.Rows[0]["Experiencia_Des_Funcao_2"],
                        datasource.Rows[0]["Experiencia_Des_Atividade_2"]);
                }

                if (!string.IsNullOrWhiteSpace(datasource.Rows[0]["Experiencia_Raz_Social_3"].ToString()))
                {
                    dtExperiencia.Rows.Add(
                        datasource.Rows[0]["Experiencia_Raz_Social_3"],
                        datasource.Rows[0]["Experiencia_Dta_Admissao_3"],
                        datasource.Rows[0]["Experiencia_Dta_Demissao_3"],
                        datasource.Rows[0]["Experiencia_Des_Funcao_3"],
                        datasource.Rows[0]["Experiencia_Des_Atividade_3"]);
                }

                #endregion

            }
            else
            {
                dtEscolaridade = Curriculo.RecuperarEscolaridadeMiniCurriculo(idCurriculo);
                dtExperiencia = Curriculo.RecuperarExperienciaMiniCurriculo(idCurriculo);
            }

            UIHelper.CarregarRepeater(rptEscolaridade, dtEscolaridade);
            UIHelper.CarregarRepeater(rptExperiencia, dtExperiencia);

            if (base.IdFilial.HasValue)
            {
                if (BLL.PesquisaCurriculo.BuscarSolr && !IdVaga.HasValue)//As buscas vindas da sala do selecionador > minhas vagas ainda estão no sql
                {
                    if (!string.IsNullOrWhiteSpace(datasource.Rows[0]["Nme_Anexo"].ToString()))
                        e.Item.FindControl("btiDownload").Visible = true;
                }
                else
                    {
                    if (new Curriculo(idCurriculo).PossuiAnexo())
                        e.Item.FindControl("btiDownload").Visible = true;
                    }
                }

            if (this.IdVaga.HasValue)
            {
                Vaga objVaga = new Vaga(this.IdVaga.Value);

                if (objVaga.ExistePerguntas())
                {
                    var btiQuestionario = (LinkButton)e.Item.FindControl("btiQuestionario");
                    btiQuestionario.Visible = true;
                }
            }
        }
        #endregion

        #region gvResultadoPesquisa_PageIndexChanged
        protected void gvResultadoPesquisa_PageIndexChanged(object source, GridPageChangedEventArgs e)
        {
            gvResultadoPesquisa.CurrentPageIndex = e.NewPageIndex;
            CarregarGridCurriculo(true);
            upCurriculos.Update();

            ScriptManager.RegisterStartupScript(upCurriculos, upCurriculos.GetType(), "AjustarRolagemParaTopo", "javaScript:AjustarRolagemParaTopo();", true);
        }
        #endregion

        #region PageSizeComboBox_SelectedIndexChanged
        void PageSizeComboBox_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            gvResultadoPesquisa.PageSize = Convert.ToInt32(e.Value);
            CarregarGridCurriculo(true);
            upCurriculos.Update();

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
                if (paginaRedirect.Equals("PesquisaCurriculoAvancada.aspx"))
                    Session.Add(Chave.Temporaria.Variavel10.ToString(), IdPesquisaCurriculo.Value);

                Redirect(paginaRedirect);
            }
            else
            {
                if (!string.IsNullOrEmpty(UrlOrigem))
                {
                    //Se a pagina origem é a pesquisa de curriculos avançado, preencher os campos com os dados da ultima pesquisa
                    if (PaginaAspxOrigem.Equals("/PesquisaCurriculoAvancada.aspx"))
                    {
                        if (IdPesquisaCurriculo.HasValue)
                            Session.Add(Chave.Temporaria.Variavel10.ToString(), IdPesquisaCurriculo.Value);
                        Redirect("PesquisaCurriculoAvancada.aspx");
                    }
                    else if (UrlOrigem.Contains("CompararCurriculo"))
                        AjustarRedirectComparaCv();
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
                lt.Text += string.Format(" de {0} - até {1} - ", Convert.ToDateTime(drv["Dta_Admissao"].ToString()).ToString("dd/MM/yyyy"), (drv["Dta_Demissao"] != DBNull.Value ? Convert.ToDateTime(drv["Dta_Demissao"].ToString()).ToString("dd/MM/yyyy") : "Não informada"));
                lt.Text += string.Format("<span class=\"texte_cor\">{0}</span></div>", Helper.CalcularTempoEmprego(drv["Dta_Admissao"].ToString(), (drv["Dta_Demissao"] != DBNull.Value ? drv["Dta_Demissao"].ToString() : DateTime.Now.ToString())));
                lt.Text += string.Format("<div class=\"espaco_descricao\"><span class=\"descricao_bold\">{0}: </span>", drv["Des_Funcao"]);
                lt.Text += string.Format("<span class=\"texte_cor\">{0}</span></div>", (drv["Des_Atividade"].ToString().Length > 90 ? drv["Des_Atividade"].ToString().Substring(0, 87) + "..." : drv["Des_Atividade"]));
            }
        }
        #endregion //rptExperiencia_ItemDataBound

        #endregion

        #region Métodos

        #region Inicializar
        private void Inicializar()
        {
            //Setando visibilidade dos botoes de salas empresa e sala candidato
            AjustarPermissao();
            RecuperarInformacoesRota();

            if (Request.UrlReferrer != null)
            {
                UrlOrigem = Request.UrlReferrer.AbsoluteUri;
                PaginaAspxOrigem = Request.UrlReferrer.AbsolutePath;
            }

            //Setando propriedades da radgrid
            gvResultadoPesquisa.PageSize = Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.QuantidadeItensPorPaginaPesquisaCurriculo));

            ListCurriculosVisualizados = new List<int>();

            EmpresaPossuePlanoNaoBloqueada = false;
            EmpresaBloqueada = EmpresaAssociacao = false;
            if (base.IdFilial.HasValue)
            {
                var objFilial = new Filial(base.IdFilial.Value);

                EmpresaBloqueada = objFilial.EmpresaBloqueada();
                EmpresaAssociacao = objFilial.EmpresaAssociacao();

                if (!objFilial.EmpresaEmAuditoria())
                {
                    EmpresaPossuePlanoNaoBloqueada = true;

                ListCurriculosVisualizados = CurriculoQuemMeViu.ListarCurriculoVisualizados(base.IdFilial.Value);

                EmpresaLogadaPossuiPlanoLiberado = PlanoAdquirido.ExistePlanoAdquiridoFilial(base.IdFilial.Value, Enumeradores.PlanoSituacao.Liberado);
            }
            }

            btnExcluirCurriculos.Visible = IdVaga.HasValue;

            InicializarBarraBusca(TipoBuscaMaster.Curriculo, true, "PesquisaCurriculo");
            DeletarArquivosTemporarios();

            CarregarGridCurriculo(false);
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

                if ((base.FuncaoMaster.HasValue || base.CidadeMaster.HasValue) && !IdPesquisaCurriculo.HasValue) //Se identificou que veio da rota valor para funcao e ou cidade e já não tem uma pesquisa atrelada
                {
                    BLL.PesquisaCurriculo objPesquisaCurriculo;
                    if (base.RecuperarDadosPesquisaCurriculo(base.FuncaoMaster.HasValue ? base.FuncaoMaster.Value : string.Empty, base.CidadeMaster.HasValue ? base.CidadeMaster.Value : string.Empty, string.Empty, out objPesquisaCurriculo))
                        IdPesquisaCurriculo = objPesquisaCurriculo.IdPesquisaCurriculo;
                }
            }
        }
        #endregion

        #region CarregarParametrosParaRota_VagaCvsNaoVistos
        private void CarregarParametrosParaRota_VagaCvsNaoVistos(string vagaCvsNaoVistos)
        {
            if (!base.IdFilial.HasValue || !base.IdUsuarioFilialPerfilLogadoEmpresa.HasValue)
                DeslogarUsuario();

            var idVaga = Helper.FromBase64(RouteData.Values["VagaCvsNaoVistos"].ToString());

            if (idVaga == "ErrorUnknownValue")
                DeslogarUsuario();

            var objVaga = BLL.Vaga.LoadObject(Convert.ToInt32(idVaga));
            objVaga.Filial.CompleteObject();

            //verifica se usuario_filial_perfil logado é o mesmo que anunciou a vaga
            if (objVaga.UsuarioFilialPerfil.IdUsuarioFilialPerfil == base.IdUsuarioFilialPerfilLogadoEmpresa.Value)
            {
                IdVaga = Convert.ToInt32(idVaga);
                BoolCandidatosNaoVisualizados = true;
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
            BNE.Auth.BNEAutenticacao.DeslogarPadrao(Auth.LogoffType.UNAUTHORIZED); //se ocorrer alguma ação inválida
            Redirect("/"); //Redireciona para Home do BNE
        }
        #endregion DeslogarUsuario

        #region CarregarGridCurriculo
        private void CarregarGridCurriculo(bool paginacao)
        {
            try
            {
                //Instanciando um novo dicionário de currículos
                if (!paginacao)
                    DicCurriculos = new Dictionary<int, bool>();

                BLL.PesquisaCurriculo objPesquisaCurriculo = null;

                int totalRegistros;
                decimal valorMediaSalarial;

                if (IdVaga.HasValue)
                {
                    if (BoolCandidatosNaoVisualizados)
                        UIHelper.CarregarRadGrid(gvResultadoPesquisa, BLL.PesquisaCurriculo.RecuperarCurriculosNaoVisualisadosVaga(gvResultadoPesquisa.PageSize, gvResultadoPesquisa.CurrentPageIndex, base.IdFilial.Value, (int)IdVaga, EmpresaAssociacao ? base.IdUsuarioFilialPerfilLogadoEmpresa.Value : (int?)null, out totalRegistros, out valorMediaSalarial), totalRegistros);
                    else if (BoolCandidatosNoPerfil)
                        UIHelper.CarregarRadGrid(gvResultadoPesquisa, BLL.PesquisaCurriculo.RecuperarCurriculosNoPerfilVaga(gvResultadoPesquisa.PageSize, gvResultadoPesquisa.CurrentPageIndex, base.IdFilial.Value, (int)IdVaga, EmpresaAssociacao ? base.IdUsuarioFilialPerfilLogadoEmpresa.Value : (int?)null, out totalRegistros, out valorMediaSalarial), totalRegistros);
                    else
                        UIHelper.CarregarRadGrid(gvResultadoPesquisa, BLL.PesquisaCurriculo.RecuperarCurriculosRelacionadosVaga(gvResultadoPesquisa.PageSize, gvResultadoPesquisa.CurrentPageIndex, base.IdFilial.Value, (int)IdVaga, EmpresaAssociacao ? base.IdUsuarioFilialPerfilLogadoEmpresa.Value : (int?)null, out totalRegistros, out valorMediaSalarial), totalRegistros);
                }
                else
                {
                    //TODO: Podemos melhorar armazenando em memória
                    objPesquisaCurriculo = IdPesquisaCurriculo.HasValue ? BLL.PesquisaCurriculo.LoadObject(IdPesquisaCurriculo.Value) : null;

                    var stopwatch = new Stopwatch();
                    stopwatch.Start();

                    var retorno = BLL.PesquisaCurriculo.BuscaCurriculo(gvResultadoPesquisa.PageSize, gvResultadoPesquisa.CurrentPageIndex, base.IdOrigem.Value, base.IdFilial.HasValue ? base.IdFilial.Value : (int?)null, EmpresaAssociacao ? base.IdUsuarioFilialPerfilLogadoEmpresa.Value : (int?)null, objPesquisaCurriculo, out totalRegistros, out valorMediaSalarial);
                    var limiteResultado = Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.LimiteResultadoPesquisa));
                    if (totalRegistros > limiteResultado)
                        totalRegistros = limiteResultado;
                    UIHelper.CarregarRadGrid(gvResultadoPesquisa, retorno, totalRegistros);

                    stopwatch.Stop();
                    System.Diagnostics.Trace.WriteLine("Busca total " + stopwatch.Elapsed);
                }

                pnlDescricaoFuncao.Visible = false;
                PalavraChavePesquisa = string.Empty;

                //Funcao
                if (objPesquisaCurriculo != null)
                {
                    if (!string.IsNullOrEmpty(objPesquisaCurriculo.DescricaoPalavraChave))
                        PalavraChavePesquisa = objPesquisaCurriculo.DescricaoPalavraChave;

                    if (objPesquisaCurriculo.Funcao != null)
                    {
                        objPesquisaCurriculo.Funcao.CompleteObject();
                        lblDescricaoFuncao.Text = objPesquisaCurriculo.Funcao.DescricaoJob;

                        //Se tiver descrição para a função
                        if (!String.IsNullOrEmpty(lblDescricaoFuncao.Text))
                            pnlDescricaoFuncao.Visible = true;
                    }

                    if (!paginacao)
                        AjustarInformacaoPesquisa(objPesquisaCurriculo.Cidade, objPesquisaCurriculo.Funcao, totalRegistros, valorMediaSalarial);
                }
                else
                {
                    if (!paginacao)
                        AjustarInformacaoPesquisa(null, null, totalRegistros, valorMediaSalarial);
                }

                upDescricaoFuncao.Update();
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
                btnR1.Visible = false;

            upBotoes.Update();
        }
        #endregion

        #region AjustarInformacaoPesquisa
        private void AjustarInformacaoPesquisa(Cidade objCidade, Funcao objFuncao, int totalRegistros, decimal mediaSalarial)
        {
            var sb = new StringBuilder();

            sb.Append(objFuncao != null
                          ? String.Format("{0} - ", objFuncao.DescricaoFuncao)
                          : String.Format("{0} - ", "Todas as funções"));

            if (objCidade != null)
            {
                objCidade.CompleteObject();
                sb.Append(String.Format("{0} - ", objCidade.NomeCidade + "/" + objCidade.Estado.SiglaEstado));
            }
            else
                sb.Append(String.Format("{0} - ", "Todas as cidades"));

            sb.Append(String.Format("{0} currículos", totalRegistros));

            if (mediaSalarial > 1)
            {
                sb.Append(String.Format(" - Média Salarial: R$ {0}", mediaSalarial.ToString("N2")));
            }
            lblInformacoesPesquisa.Text = sb.ToString();
            upInformacoesPesquisa.Update();
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
                    if (IdPesquisaCurriculo.HasValue)
                        Session.Add(Chave.Temporaria.Variavel10.ToString(), IdPesquisaCurriculo.Value);
                    Redirect("~/PesquisaCurriculoAvancada.aspx");
                }
                else //Somente empresas
                    ExibirMensagem(MensagemAviso._101001, TipoMensagem.Aviso);
            }
            else
            {
                ClicouPesquisaAvancada = true;
                AbrirModalLogin();
            }
        }
        #endregion

        #region AbrirModalLogin
        private void AbrirModalLogin()
        {
            ucModalLogin.Inicializar();
            ucModalLogin.Mostrar();
            pnlL.Visible = true;
            upL.Update();
        }
        #endregion

        #region RetornarUrlFoto
        protected string RetornarUrlFoto(string strCpf)
        {
            return UIHelper.RetornarUrlFoto(strCpf.Trim(), PessoaFisicaFoto.OrigemFoto.Local);
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
                var funcoes = funcao.Split(';').Where(f => !string.IsNullOrEmpty(f));
                nomeFuncao = funcoes.ElementAt(0);
            }

            return SitemapHelper.MontarUrlVisualizacaoCurriculo(nomeFuncao, nomeCidade, siglaEstado, identificadorCurriculo);
        }
        #endregion

        #region AjustarRedirectComparaCv
        private void AjustarRedirectComparaCv()
        {
            if (base.IdUsuarioFilialPerfilLogadoEmpresa.HasValue && UsuarioFilialPerfil.ValidarTipoPerfil(base.IdUsuarioFilialPerfilLogadoEmpresa.Value, (int)Enumeradores.TipoPerfil.Empresa))
            {
                if (base.IdFilial.HasValue)
                {
                    if (!EmpresaBloqueada(new Filial(base.IdFilial.Value)))
                    {
                        var count = RecuperarCurrriculosSelecionados().Count;

                        if (count.Equals(0))
                            ExibirMensagem(MensagemAviso._101002, TipoMensagem.Aviso);
                        else if (count.Equals(1))
                            ExibirMensagem(MensagemAviso._101007, TipoMensagem.Aviso);
                        else
                        {
                            var ids = RecuperarCurrriculosSelecionados();
                            Session.Add(Chave.Temporaria.Variavel1.ToString(), ids);
                            //Quando o usuário voltar para a pesquisa de currículo 
                            if (IdPesquisaCurriculo.HasValue)
                                Session.Add(Chave.Temporaria.Variavel3.ToString(), IdPesquisaCurriculo);

                            if (IdVaga.HasValue)
                            {
                                Session.Add(Chave.Temporaria.Variavel7.ToString(), IdVaga);
                                Session.Add(Chave.Temporaria.Variavel8.ToString(), BoolCandidatosNaoVisualizados);
                                Session.Add(Chave.Temporaria.Variavel9.ToString(), BoolCandidatosNoPerfil);
                            }

                            Redirect("CompararCurriculo.aspx");
                        }
                    }

                    upPesquisa.Update();
                }
            }
            else //Somente empresas
                ExibirMensagem(MensagemAviso._101001, TipoMensagem.Aviso);
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
            int idCurriculo = Convert.ToInt32(gvResultadoPesquisa.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Curriculo"]);

            ((GridDataItem)e.Item).ChildItem.FindControl("ContainerCurriculo").Visible = !e.Item.Expanded;

            //Ajustando design
            var gtc = (GridTableCell)(((GridDataItem)e.Item).ChildItem.FindControl("ContainerCurriculo").Parent).Parent;
            gtc.Attributes.Add("style", "padding: 0px !important");

            var btlNomeCurriculo = (LinkButton)e.Item.FindControl("lblNomeCurriculo");

            if (!e.Item.Expanded)
            {
                if (base.IdFilial.HasValue)
                {
                    var objFilial = new Filial(base.IdFilial.Value);
                    if (!objFilial.EmpresaBloqueada())
                        CurriculoQuemMeViu.SalvarQuemMeViu(objFilial, new Curriculo(idCurriculo));

                    ListCurriculosVisualizados.Add(idCurriculo);
                    CurriculoVisualizacaoHistorico.SalvarHistoricoVisualizacao(objFilial, new Curriculo(idCurriculo), false, PageHelper.RecuperarIP());
                }
            }

            //Ajustando visualizacao de curriculo
            IdCurriculoPesquisaCurriculo = idCurriculo;

            if (base.IdFilial.HasValue)
                btlNomeCurriculo.CssClass = ListCurriculosVisualizados.Contains(Convert.ToInt32(idCurriculo)) ? "nome_descricao_curriculo_visited" : "nome_descricao_curriculo_padrao";

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
            PessoaFisicaComplemento objPessoaFisicaComplemento;
            if (PessoaFisicaComplemento.CarregarPorPessoaFisica(PessoaFisica.RecuperarIdPorCurriculo(new Curriculo(objCurriculo.IdCurriculo)), out objPessoaFisicaComplemento))
            {
                if (objPessoaFisicaComplemento.ArquivoAnexo != null)
                {
                    string serverName = Request.ServerVariables["HTTP_HOST"];
                    Session.Add(Chave.Temporaria.Variavel1.ToString(), objPessoaFisicaComplemento.ArquivoAnexo);
                    Session.Add(Chave.Temporaria.Variavel2.ToString(), objPessoaFisicaComplemento.NomeAnexo);
                    ScriptManager.RegisterStartupScript(this, GetType(), "AbrirPopup", string.Format("AbrirPopup('http://{0}/DownloadAnexo.aspx', 600, 800);", serverName), true);
                }
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
            return DicCurriculos.Where(d => d.Value).Select(d => d.Key).ToList();
        }
        #endregion

        #endregion

    }
}