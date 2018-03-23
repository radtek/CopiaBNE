using BNE.BLL;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using BNE.Web.Master;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BNE.BLL.Common;
using Enumeradores = BNE.BLL.Enumeradores;

namespace BNE.Web
{
    public partial class PesquisaVaga : BasePage
    {

        #region Propriedades

        #region IdVagaPesquisaVaga - Variavel 2
        /// <summary>
        /// Propriedade que armazena e recupera o IdVagaSession
        /// </summary>
        protected int? IdVagaPesquisaVaga
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

        #region IdFuncaoPesquisaVaga - Variavel 3
        /// <summary>
        /// Propriedade que armazena e recupera o IdFuncaoPesquisaVaga
        /// </summary>
        protected int? IdFuncaoPesquisaVaga
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel3.ToString()] != null)
                    return Int32.Parse(ViewState[Chave.Temporaria.Variavel3.ToString()].ToString());
                return null;
            }
            set
            {
                if (value != null)
                    ViewState.Add(Chave.Temporaria.Variavel3.ToString(), value);
                else
                    ViewState.Remove(Chave.Temporaria.Variavel3.ToString());
            }
        }
        #endregion

        #region IdCidadePesquisaVaga - Variavel 4
        /// <summary>
        /// Propriedade que armazena e recupera o IdVagaSession
        /// </summary>
        protected int? IdCidadePesquisaVaga
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel4.ToString()] != null)
                    return Int32.Parse(ViewState[Chave.Temporaria.Variavel4.ToString()].ToString());
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

        #region IdIndexManipular - Variável 5
        /// <summary>
        /// Propriedade que armazena e recupera o IdPesquisaVaga
        /// </summary>
        private int IdIndexManipular
        {
            get
            {
                return Int32.Parse(ViewState[Chave.Temporaria.Variavel5.ToString()].ToString());
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Variavel5.ToString(), value);
            }
        }
        #endregion

        #region IdPesquisaVaga - Variável 6
        /// <summary>
        /// Propriedade que armazena e recupera o IdPesquisaVaga
        /// </summary>
        protected int? IdPesquisaVaga
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

        #region PropriedadeVerDadosEmpresa - Variavel 7
        /// <summary>
        /// Propriedade que armazena e recupera o VerDadosEmpresa
        /// </summary>
        protected bool PropriedadeVerDadosEmpresa
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel7.ToString()] != null)
                    return Convert.ToBoolean(ViewState[Chave.Temporaria.Variavel7.ToString()].ToString());
                return false;
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Variavel7.ToString(), value);
            }
        }
        #endregion

        #region PropriedadeCandidatarVaga  - Variavel 8
        /// <summary>
        /// Propriedade que armazena e recupera o CandidatarVaga
        /// </summary>
        protected bool PropriedadeCandidatarVaga
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel8.ToString()] != null)
                    return Convert.ToBoolean(ViewState[Chave.Temporaria.Variavel8.ToString()].ToString());
                return false;
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Variavel8.ToString(), value);
            }
        }
        #endregion

        #region FlagConfidencial  - Variavel 9
        /// <summary>
        /// Propriedade que armazena e recupera o PropriedadeCandidatarVaga
        /// </summary>
        protected bool FlagConfidencial
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel9.ToString()] != null)
                    return Convert.ToBoolean(ViewState[Chave.Temporaria.Variavel9.ToString()].ToString());
                return false;
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Variavel9.ToString(), value);
            }
        }
        #endregion

        #region IdOrigemPesquisaVaga - Variavel 10
        /// <summary>
        /// </summary>
        public int? IdOrigemPesquisaVaga
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel10.ToString()] != null)
                    return Convert.ToInt32(ViewState[Chave.Temporaria.Variavel10.ToString()]);
                return null;
            }
            set
            {
                if (value != null)
                    ViewState.Add(Chave.Temporaria.Variavel10.ToString(), value);
                else
                    ViewState.Remove(Chave.Temporaria.Variavel10.ToString());
            }
        }
        #endregion

        #region IdVagaCandidatar - Variavel 11
        /// <summary>
        /// Propriedade que armazena e recupera o IdVagaCandidatar
        /// </summary>
        protected int? IdVagaCandidatar
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel11.ToString()] != null)
                    return Int32.Parse(ViewState[Chave.Temporaria.Variavel11.ToString()].ToString());
                return null;
            }
            set
            {
                if (value != null)
                    ViewState.Add(Chave.Temporaria.Variavel11.ToString(), value);
                else
                    ViewState.Remove(Chave.Temporaria.Variavel11.ToString());
            }
        }
        #endregion

        #region IdFilialPesquisaVaga - Variavel 12
        /// <summary>
        /// Propriedade que armazena e recupera o IdVagaSession
        /// </summary>
        protected int? IdFilialPesquisaVaga
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel12.ToString()] != null)
                    return Int32.Parse(ViewState[Chave.Temporaria.Variavel12.ToString()].ToString());

                return null;
            }
            set
            {
                if (value != null)
                    ViewState.Add(Chave.Temporaria.Variavel12.ToString(), value);
                else
                    ViewState.Remove(Chave.Temporaria.Variavel12.ToString());
            }
        }
        #endregion

        #region UrlOrigem - Variavel 13
        /// <summary>
        /// </summary>
        public string UrlOrigem
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

        #region QuantidadeCandidaturas - Variável 15
        /// <summary>
        /// Propriedade que armazena e recupera a QuantidadeCandidaturas
        /// </summary>
        protected int? QuantidadeCandidaturas
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel15.ToString()] != null)
                    return Int32.Parse(ViewState[Chave.Temporaria.Variavel15.ToString()].ToString());
                return null;
            }
            set
            {
                if (value != null)
                    ViewState.Add(Chave.Temporaria.Variavel15.ToString(), value);
                else
                    ViewState.Remove(Chave.Temporaria.Variavel15.ToString());
            }
        }
        #endregion

        #region BuscaComGeolocalizacaoPorCidade - Variavel 16
        /// <summary>
        /// Propriedade para identificar se o usuário quer ver vagas próximas ao cep que ele informou na pesquisa avançada, filtrando essas vagas por cidade
        /// </summary>
        protected bool BuscaComGeolocalizacaoPorCidade
        {
            get
            {
                return Convert.ToBoolean(ViewState[Chave.Temporaria.Variavel16.ToString()].ToString());
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Variavel16.ToString(), value);
            }
        }
        #endregion

        #region PropriedadeCompartilhamentoEmail - Variavel 17
        protected bool PropriedadeCompartilhamentoEmail
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

        #region UrlVaga - Variavel 18
        protected string UrlVaga
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel18.ToString()] != null)
                    return Convert.ToString(ViewState[Chave.Temporaria.Variavel18.ToString()]);
                return String.Empty;
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Variavel18.ToString(), value);
            }
        }
        #endregion

        #region ConfirmacaoIPhone  - Variavel 20
        /// <summary>
        /// Propriedade que permite identificar se o request foi originário de um IPhone
        /// É possível identificar atrávez da query string que o WebRewrite passa.
        /// </summary>
        protected bool ConfirmacaoIPhone
        {
            get
            {
                return Convert.ToBoolean(ViewState[Chave.Temporaria.Variavel20.ToString()].ToString());
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Variavel20.ToString(), value);
            }
        }
        #endregion

        #region MostrarMaisDadosEmpresa  - Variavel 21
        /// <summary>
        /// Propriedade que permite guardar a informação se deve ou não mostrar mais dados da empresa
        /// </summary>
        protected bool MostrarMaisDadosEmpresa
        {
            get
            {
                return Convert.ToBoolean(ViewState[Chave.Temporaria.Variavel21.ToString()].ToString());
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Variavel21.ToString(), value);
            }
        }
        #endregion

        #endregion

        #region Eventos

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            // Essa variável muda de valor quando é chamado pelo IPhone
            ConfirmacaoIPhone = false;

            MostrarMaisDadosEmpresa = false;
            OrigemFilial objOrigemFilial;
            if (base.STC.Value && OrigemFilial.CarregarPorOrigem(base.IdOrigem.Value, out objOrigemFilial) && (objOrigemFilial != null && objOrigemFilial.Filial.PossuiSTCUniversitario()))
                MostrarMaisDadosEmpresa = true;

            if (!IsPostBack)
                Inicializar();

            ucModalQuestionarioVagas.Salvar += ucModalQuestionarioVagas_Salvar;
            ucModalLogin.Logar += ucModalLogin_Logar;
            ucModalConfirmacaoEnvioCurriculo.Fechar += ucModalConfirmacaoEnvioCurriculo_Fechar;
            ucModalCompartilhamentoVaga.EnviarConfirmacao += ucModalCompartilhamentoVaga_EnviarConfirmacao;
            Paginacao.MudaPagina += Paginacao_MudaPagina;

            var principal = (Principal)Master;
            if (principal != null)
            {
                principal.PesquisaRapidaVaga += PesquisaVaga_PesquisaRapidaVaga;
                principal.LoginEfetuadoSucesso += principal_LoginEfetuadoSucesso;
            }

            if (!base.STC.Value || (base.STC.Value && !base.IdUsuarioFilialPerfilLogadoEmpresa.HasValue))
                base.InicializarBarraBusca(TipoBuscaMaster.Vaga, true, "PesquisaVaga");
            else
                base.InicializarBarraBusca(TipoBuscaMaster.Curriculo, false, "PesquisaVaga");

            if (ConfirmacaoIPhone) // Abre os dados da Vaga para o IPhone
                ScriptManager.RegisterStartupScript(this, GetType(), "click()", "javaScript:MaisDadosIPhone();", true);

            if (DeveMostrarModalSucessoFacebook())
            {
                ucModalCompartilhamentoVaga.Inicializar();
                ucModalCompartilhamentoVaga.MostrarModal();
                pnlCompVaga.Visible = true;
                upCompVaga.Update();
            }
        }
        #endregion

        #region principal_LoginEfetuadoSucesso
        void principal_LoginEfetuadoSucesso()
        {
        }
        #endregion

        #region ucModalCompartilhamentoVaga_EnviarConfirmacao
        void ucModalCompartilhamentoVaga_EnviarConfirmacao(List<string> emailsDestinatarios)
        {
            ucModalCompartilhamentoVaga.Resetar();
        }
        #endregion

        #region ucModalConfirmacaoEnvioCurriculo_Fechar
        void ucModalConfirmacaoEnvioCurriculo_Fechar()
        {
            if (IdCurriculo.HasValue)
            {
                if (!new Curriculo(base.IdCurriculo.Value).VIP() && (QuantidadeCandidaturas.HasValue && QuantidadeCandidaturas.Equals(0)))
                    Redirect(UIHelper.RecuperarCaminhoProdutoVIP(GetRouteUrl(Enumeradores.RouteCollection.ProdutoVIP.ToString(), null), Enumeradores.VantagensVIP.CandidaturaVagas));
            }
            else
            {
                Redirect(GetRouteUrl(Enumeradores.RouteCollection.LoginComercialCandidato.ToString(), null));
            }
        }
        #endregion

        #region ucModalLogin_Logar
        void ucModalLogin_Logar(string urlDestino)
        {
            if (base.IdCurriculo.HasValue)
                ValidarCandidato();
            else
                Redirect(GetRouteUrl(Enumeradores.RouteCollection.CadastroCurriculoMini.ToString(), null));
        }
        #endregion

        #region PesquisaVaga_PesquisaRapida
        void PesquisaVaga_PesquisaRapidaVaga(int idPesquisaVaga)
        {
            IdPesquisaVaga = idPesquisaVaga;

            //Ajustando page index para esta busca, devido a paginação.
            Paginacao.PaginaCorrente = 1;

            CarregarListaVagas();

            upRlvVaga.Update();

            ScriptManager.RegisterStartupScript(this, this.GetType(), "FadeToggleMaisDados_PesquisaVaga_PesquisaRapidaVaga", "javaScript:FadeToggleMaisDados();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "AplicarBneRecomenda_PesquisaVaga_PesquisaRapidaVaga", "javaScript:AplicarBneRecomenda();", true);
        }
        #endregion

        #region Paginacao_MudaPagina
        /// <summary>
        /// Ação ao mudar de página
        /// </summary>
        protected void Paginacao_MudaPagina()
        {
            CarregarListaVagas();

            ScriptManager.RegisterStartupScript(this, this.GetType(), "AplicarBneRecomenda_PageIndexChanged", "javaScript:AplicarBneRecomenda();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "FadeToggleMaisDados_PageIndexChanged", "javaScript:FadeToggleMaisDados();", true);
            ScriptManager.RegisterStartupScript(this, GetType(), "AjustarRolagemParaTopo", "javaScript:AjustarRolagemParaTopo();", true);
        }
        #endregion

        #region rptVagas_ItemDataBound
        protected void rptVagas_ItemDataBound(Object source, RepeaterItemEventArgs e)
        {
            if (e.Item is RepeaterItem)
            {
                var liMaisDadosEmpresa = (HtmlGenericControl)e.Item.FindControl("liMaisDadosEmpresa");
                liMaisDadosEmpresa.Visible = !base.STC.Value || MostrarMaisDadosEmpresa;
            }
        }
        #endregion

        #region rptVagas_ItemCommand
        protected void rptVagas_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            var idVaga = Convert.ToInt32(e.CommandArgument);

            IdVagaCandidatar = idVaga;
            IdIndexManipular = e.Item.ItemIndex;

            if (e.CommandName.Equals("candidatar"))
            {
                PropriedadeCandidatarVaga = true;
                PropriedadeVerDadosEmpresa = false;
                PropriedadeCompartilhamentoEmail = false;

                if (base.IdCurriculo.HasValue)
                    ValidarCandidato();
                else
                    AbrirModalLogin();
            }
            else if (e.CommandName.Equals("empresa"))
            {
                Vaga objVaga = Vaga.LoadObject(idVaga);

                IdFilialPesquisaVaga = objVaga.Filial.IdFilial;
                FlagConfidencial = objVaga.FlagConfidencial;

                PropriedadeCandidatarVaga = false;
                PropriedadeVerDadosEmpresa = true;
                PropriedadeCompartilhamentoEmail = false;

                if (base.IdCurriculo.HasValue)
                    ValidarCandidato();
                else
                    AbrirModalLogin();
            }
            else if (e.CommandName.Equals("compartilharEmail"))
            {
                PropriedadeCandidatarVaga = false;
                PropriedadeVerDadosEmpresa = false;
                PropriedadeCompartilhamentoEmail = true;

                UrlVaga =
                    ((HiddenField)rptVagas.Items[IdIndexManipular].FindControl("hfUrlVaga")).Value;

                if (base.IdUsuarioFilialPerfilLogadoCandidato.HasValue)
                    ValidarCandidato();
                else
                    AbrirModalLogin();
            }
        }
        #endregion

        #region ucModalQuestionarioVagas_Salvar
        void ucModalQuestionarioVagas_Salvar(object sender, UserControls.Modais.VagaRespostaEventArgs e)
        {
            if (base.IdCurriculo.HasValue)
            {
                Vaga objVaga = Vaga.LoadObject(e.IdVaga);
                Curriculo objCurriculo = Curriculo.LoadObject(base.IdCurriculo.Value);
                CandidatarVaga(objVaga, objCurriculo, e);
            }
        }
        #endregion

        #region btnVoltar_Click
        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(UrlOrigem))
            {
                //Se a pagina destino é a pesquisa de vagas avançada, preencher os campos com os dados da ultima pesquisa
                //TODO: Que lixo
                if (UrlOrigem.ToLower().Contains("pesquisavagaavancada.aspx"))
                    Session.Add(Chave.Temporaria.Variavel1.ToString(), (int)IdPesquisaVaga);

                Redirect(UrlOrigem);
            }
            else
                Redirect("Default.aspx");
        }
        #endregion

        #region btiLocalizacaoAproximada_Click
        protected void btiLocalizacaoAproximada_Click(object sender, ImageClickEventArgs e)
        {
            BuscaComGeolocalizacaoPorCidade = true;

            CarregarListaVagas();
        }
        #endregion

        #endregion

        #region Métodos

        #region Inicializar
        private void Inicializar()
        {
            RecuperarInformacoesRota();

            BuscaComGeolocalizacaoPorCidade = false;

            ConfirmacaoIPhone = false;

            Funcao objFuncao;
            if (base.FuncaoMaster.HasValue && Funcao.CarregarPorDescricao(base.FuncaoMaster.Value, out objFuncao))
                IdFuncaoPesquisaVaga = objFuncao.IdFuncao;

            Cidade objCidade;
            if (base.CidadeMaster.HasValue && Cidade.CarregarPorNome(base.CidadeMaster.Value, out objCidade))
                IdCidadePesquisaVaga = objCidade.IdCidade;

            // Se a query string não for vazia
            int vaga;
            if (!String.IsNullOrEmpty(Request.QueryString["Vaga"]) && int.TryParse(Request.QueryString["vaga"], out vaga))
            {
                var url = Vaga.MontarUrlVaga(vaga);
                Response.Redirect(url);
            }

            if (IdVagaPesquisaVaga.HasValue)
            {
                var url = Vaga.MontarUrlVaga((int)IdVagaPesquisaVaga);
                Response.Redirect(url);
            }

            Paginacao.TamanhoPagina = Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.QuantidadeItensPorPaginaPesquisaVaga));

            if (Request.UrlReferrer != null)
                UrlOrigem = Request.UrlReferrer.AbsoluteUri;

            CarregarListaVagas();
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

                if (RouteData.Values["CodigoEmpresa"] != null)
                    IdFilialPesquisaVaga = Convert.ToInt32(RouteData.Values["CodigoEmpresa"]);
            }
        }
        #endregion

        #region CarregarListaVagas
        public void CarregarListaVagas()
        {
            try
            {
                int totalRegistros = 0;

                BLL.PesquisaVaga objPesquisaVaga = null;
                if (IdPesquisaVaga.HasValue)
                    objPesquisaVaga = BLL.PesquisaVaga.LoadObject(IdPesquisaVaga.Value);

                //Seta Origem na busca de vagas somente quando estiver no RHOffice
                if (base.STC.Value)
                    IdOrigemPesquisaVaga = base.IdOrigem.Value;

                int? idCurriculo = null;
                if (base.IdCurriculo.HasValue)
                    idCurriculo = base.IdCurriculo.Value;

                var especificaTipoContrato = AdicionarFiltroTipoContratoEm(objPesquisaVaga);

                #region Tratamento STC
                var mostrarVagasBNEnoSTC = false;
                OrigemFilial objOrigemFilial = null;
                if (base.STC.Value && OrigemFilial.CarregarPorOrigem(base.IdOrigem.Value, out objOrigemFilial))
                    mostrarVagasBNEnoSTC = objOrigemFilial.Filial.PossuiSTCUniversitario() || objOrigemFilial.Filial.PossuiSTCLanhouse();
                #endregion Tratamento STC

                try
                {
                    UIHelper.CarregarRepeater(rptVagas, TratamentoParaWebEstagios(objPesquisaVaga,
                                       BLL.PesquisaVaga.BuscaVagaAPIDT(objPesquisaVaga, Paginacao.TamanhoPagina,
                                           Paginacao.PaginaCorrente, null, IdOrigemPesquisaVaga, mostrarVagasBNEnoSTC, OrdenacaoBuscaVaga.Padrao, out totalRegistros)));
                }
                catch (Exception ex)
                {
                    EL.GerenciadorException.GravarExcecao(ex, "Erro ao carregar as vagas pela api no STC");
                    UIHelper.CarregarRepeater(rptVagas, TratamentoParaWebEstagios(objPesquisaVaga,
                   BLL.PesquisaVaga.BuscaVagaFullText(objPesquisaVaga, Paginacao.TamanhoPagina,
                       Paginacao.PaginaCorrente, idCurriculo, IdFuncaoPesquisaVaga, IdCidadePesquisaVaga, String.Empty,
                       IdOrigemPesquisaVaga, mostrarVagasBNEnoSTC, null, IdFilialPesquisaVaga, null, OrdenacaoBuscaVaga.Padrao,
                       out totalRegistros)));
                }

                var forcarTipoContrato = base.STC.HasValue && base.STC.Value && especificaTipoContrato;

                if (objPesquisaVaga == null || !objPesquisaVaga.FlagPesquisaAvancada)
                {
                    var filialPerfil = objPesquisaVaga != null
                        ? objPesquisaVaga.UsuarioFilialPerfil
                        : null;

                    if (totalRegistros.Equals(0) && !IdFilialPesquisaVaga.HasValue)
                    {
                        Cidade objCidade = null;
                        if (objPesquisaVaga != null && objPesquisaVaga.Cidade != null)
                            objCidade = objPesquisaVaga.Cidade;
                        else if (IdCidadePesquisaVaga.HasValue)
                            objCidade = new Cidade(IdCidadePesquisaVaga.Value);

                        int? idFuncao = null;
                        int? idFuncaoArea = null;

                        if (objPesquisaVaga != null && objPesquisaVaga.Funcao != null)
                            idFuncaoArea = idFuncao = objPesquisaVaga.Funcao.IdFuncao;

                        if (objCidade != null)
                        {
                            string desPalavraChave = String.Empty;

                            if (objPesquisaVaga != null)
                                desPalavraChave = objPesquisaVaga.DescricaoPalavraChave;

                            objCidade.CompleteObject();

                            var objPesquisaVagaExtra = forcarTipoContrato
                                ? CriarPesquisaVagaTipoContratoExclusiva(filialPerfil)
                                : null;

                            UIHelper.CarregarRepeater(rptVagas, TratamentoParaWebEstagios(objPesquisaVaga,
                                BLL.PesquisaVaga.BuscaVagaFullText(objPesquisaVagaExtra, Paginacao.TamanhoPagina,
                                    Paginacao.PaginaCorrente, null, idFuncao, null, desPalavraChave,
                                    IdOrigemPesquisaVaga, mostrarVagasBNEnoSTC, objCidade.Estado.SiglaEstado,
                                    null, null, OrdenacaoBuscaVaga.Padrao, out totalRegistros)));

                            if (totalRegistros.Equals(0))
                                UIHelper.CarregarRepeater(rptVagas, TratamentoParaWebEstagios(objPesquisaVaga,
                                    BLL.PesquisaVaga.BuscaVagaFullText(objPesquisaVagaExtra, Paginacao.TamanhoPagina,
                                        Paginacao.PaginaCorrente, null, idFuncao, null, desPalavraChave,
                                        IdOrigemPesquisaVaga, mostrarVagasBNEnoSTC, null, null, null, OrdenacaoBuscaVaga.Padrao,
                                         out totalRegistros)));

                            if (totalRegistros.Equals(0))
                                UIHelper.CarregarRepeater(rptVagas, TratamentoParaWebEstagios(objPesquisaVaga,
                                    BLL.PesquisaVaga.BuscaVagaFullText(objPesquisaVagaExtra, Paginacao.TamanhoPagina,
                                        Paginacao.PaginaCorrente, null, null, null, desPalavraChave,
                                        IdOrigemPesquisaVaga, mostrarVagasBNEnoSTC, null, null, idFuncaoArea, OrdenacaoBuscaVaga.Padrao,
                                         out totalRegistros)));
                        }
                        else
                        {
                            if (forcarTipoContrato)
                            {
                                objPesquisaVaga =
                                    CriarPesquisaVagaTipoContratoExclusiva(filialPerfil);

                                UIHelper.CarregarRepeater(rptVagas, TratamentoParaWebEstagios(objPesquisaVaga,
                                 BLL.PesquisaVaga.BuscaVagaFullText(objPesquisaVaga, Paginacao.TamanhoPagina,
                                     Paginacao.PaginaCorrente, null, null, null, String.Empty, IdOrigemPesquisaVaga,
                                     mostrarVagasBNEnoSTC, null, null, idFuncaoArea, OrdenacaoBuscaVaga.Padrao,
                                      out totalRegistros)));
                            }
                            else
                            {
                                UIHelper.CarregarRepeater(rptVagas, TratamentoParaWebEstagios(objPesquisaVaga,
                                    BLL.PesquisaVaga.BuscaVagaFullText(null, Paginacao.TamanhoPagina,
                                        Paginacao.PaginaCorrente, null, null, null, String.Empty, IdOrigemPesquisaVaga,
                                        mostrarVagasBNEnoSTC, null, null, idFuncaoArea, OrdenacaoBuscaVaga.Padrao,
                                        out totalRegistros)));
                            }
                        }
                    }
                }

                Paginacao.TotalResultados = totalRegistros;
                Paginacao.AjustarPaginacao();


                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AjaxLoad", "AjaxLoad()", true);
            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
            }
        }

        private IEnumerable TratamentoParaWebEstagios(BLL.PesquisaVaga objPesquisaVaga, DataTable buscaVagaFullTextResult)
        {
            if (buscaVagaFullTextResult == null || buscaVagaFullTextResult.Rows.Count == 0)
                return Enumerable.Empty<dynamic>();

            if (objPesquisaVaga != null)
            {
                var tipoVinculos = PesquisaVagaTipoVinculo.ListarIdentificadores(objPesquisaVaga);

                if (tipoVinculos.Count == 1 && tipoVinculos.Contains((int)Enumeradores.TipoVinculo.Estágio))
                {
                    var originalResult = Code.DataTableX.AsDynamicEnumerable(buscaVagaFullTextResult);
                    return originalResult;
                }
            }

            var originalCopy = Code.DataTableX.AsDynamicEnumerable(buscaVagaFullTextResult).ToArray();

            var collectionResult = Code.DataTableX.AsDynamicEnumerable(buscaVagaFullTextResult) as List<dynamic>;
            if (collectionResult == null)
            {
                collectionResult = new List<dynamic>(originalCopy);
            }

            return collectionResult;
        }

        private BLL.PesquisaVaga CriarPesquisaVagaTipoContratoExclusiva(UsuarioFilialPerfil perfil)
        {
            var newPesquisaVag = new BLL.PesquisaVaga();
            newPesquisaVag.UsuarioFilialPerfil = perfil;
            var novosVinculos = new List<PesquisaVagaTipoVinculo>();
            novosVinculos.Add(new PesquisaVagaTipoVinculo { TipoVinculo = new TipoVinculo((int)Enumeradores.TipoVinculo.Aprendiz), PesquisaVaga = newPesquisaVag });
            novosVinculos.Add(new PesquisaVagaTipoVinculo { TipoVinculo = new TipoVinculo((int)Enumeradores.TipoVinculo.Estágio), PesquisaVaga = newPesquisaVag });
            newPesquisaVag.Salvar(new List<PesquisaVagaDisponibilidade>(), novosVinculos);
            return newPesquisaVag;
        }

        private bool AdicionarFiltroTipoContratoEm(BLL.PesquisaVaga objPesquisaVaga)
        {
            if ((IdOrigemPesquisaVaga ?? 0) <= 1) return false;

            if ((objPesquisaVaga != null && objPesquisaVaga.FlagPesquisaAvancada)) return false;

            OrigemFilial objOrigemFilial;
            if (!OrigemFilial.CarregarPorOrigem(base.IdOrigem.Value, out objOrigemFilial)) return false;

            ParametroFilial limiParametro;
            if (!ParametroFilial.CarregarParametroPorFilial(
                Enumeradores.Parametro.LimitarPesquisaVagaPadraoPorTipoContratoEstag, objOrigemFilial.Filial,
                out limiParametro)) return false;

            if (!StringComparer.OrdinalIgnoreCase.Equals(
                (limiParametro.ValorParametro ?? string.Empty).Trim(), "true")) return false;

            bool addPesquisaVinculoAprendiz;
            bool addPesquisaVinculoEstagio;
            if ((IdPesquisaVaga ?? 0) <= 0 || objPesquisaVaga == null)
            {
                if (objPesquisaVaga == null)
                    objPesquisaVaga = new BLL.PesquisaVaga();

                addPesquisaVinculoAprendiz = true;
                addPesquisaVinculoEstagio = true;
            }
            else
            {
                var vinculos = PesquisaVagaTipoVinculo.ListarIdentificadores(IdPesquisaVaga != null ? new BLL.PesquisaVaga(IdPesquisaVaga.Value) : objPesquisaVaga);

                if ((vinculos == null || vinculos.Count == 0))
                {
                    addPesquisaVinculoAprendiz = true;
                    addPesquisaVinculoEstagio = true;
                }
                else
                {
                    addPesquisaVinculoAprendiz = vinculos.All(obj => obj != (int)Enumeradores.TipoVinculo.Aprendiz);
                    addPesquisaVinculoEstagio = vinculos.All(obj => obj != (int)Enumeradores.TipoVinculo.Estágio);
                }
            }
            var novosVinculos = new List<PesquisaVagaTipoVinculo>();
            bool save = false;
            if (addPesquisaVinculoAprendiz)
            {
                novosVinculos.Add(new PesquisaVagaTipoVinculo() { TipoVinculo = new TipoVinculo((int)Enumeradores.TipoVinculo.Aprendiz), PesquisaVaga = objPesquisaVaga });
                save = true;
            }
            if (addPesquisaVinculoEstagio)
            {
                novosVinculos.Add(new PesquisaVagaTipoVinculo() { TipoVinculo = new TipoVinculo((int)Enumeradores.TipoVinculo.Estágio), PesquisaVaga = objPesquisaVaga });
                save = true;
            }

            if (save)
                objPesquisaVaga.Salvar(new List<PesquisaVagaDisponibilidade>(), novosVinculos);
            return true;
        }

        #endregion

        #region CandidatarVaga
        private void CandidatarVaga(Vaga objVaga, Curriculo objCurriculo, UserControls.Modais.VagaRespostaEventArgs e)
        {
            var listVagaPergunta = new List<VagaCandidatoPergunta>();

            if (e != null)
            {
                VagaCandidatoPergunta objVagaCandidatoPergunta;
                if (e.IdPergunta1.HasValue)
                {
                    objVagaCandidatoPergunta = new VagaCandidatoPergunta
                    {
                        VagaPergunta = new VagaPergunta(e.IdPergunta1.Value),
                        FlagResposta = e.FlagRespostaPergunta1.Value,
                        DescricaoResposta = e.RespostaPergunta1
                    };
                    listVagaPergunta.Add(objVagaCandidatoPergunta);
                }
                if (e.IdPergunta2.HasValue)
                {
                    objVagaCandidatoPergunta = new VagaCandidatoPergunta
                    {
                        VagaPergunta = new VagaPergunta(e.IdPergunta2.Value),
                        FlagResposta = e.FlagRespostaPergunta2.Value,
                        DescricaoResposta = e.RespostaPergunta2
                    };
                    listVagaPergunta.Add(objVagaCandidatoPergunta);
                }
                if (e.IdPergunta3.HasValue)
                {
                    objVagaCandidatoPergunta = new VagaCandidatoPergunta
                    {
                        VagaPergunta = new VagaPergunta(e.IdPergunta3.Value),
                        FlagResposta = e.FlagRespostaPergunta3.Value,
                        DescricaoResposta = e.RespostaPergunta3
                    };
                    listVagaPergunta.Add(objVagaCandidatoPergunta);
                }
                if (e.IdPergunta4.HasValue)
                {
                    objVagaCandidatoPergunta = new VagaCandidatoPergunta
                    {
                        VagaPergunta = new VagaPergunta(e.IdPergunta4.Value),
                        FlagResposta = e.FlagRespostaPergunta4.Value,
                        DescricaoResposta = e.RespostaPergunta4
                    };
                    listVagaPergunta.Add(objVagaCandidatoPergunta);
                }

                ucModalQuestionarioVagas.FecharModal();
            }

            //Se a empresa possui Minha Empresa Oferece Cursos é usada a origem da empresa como principal e não mais da vaga.
            Origem objOrigem = null;
            OrigemFilial objOrigemFilial = null;
            if (base.STC.Value && OrigemFilial.CarregarPorOrigem(base.IdOrigem.Value, out objOrigemFilial) && (objOrigemFilial != null && objOrigemFilial.Filial.PossuiSTCUniversitario()))
                objOrigem = new Origem(base.IdOrigem.Value);

            int? quantidadeCandidaturas;
            VagaCandidato.Candidatar(objCurriculo, objVaga, objOrigem, listVagaPergunta,
                Common.Helper.RecuperarIP(), base.STC.Value, (objOrigemFilial != null && !objOrigemFilial.Filial.PossuiSTCUniversitario() && !objOrigemFilial.Filial.PossuiSTCLanhouse()), false, Enumeradores.OrigemCandidatura.Site, out quantidadeCandidaturas);

            QuantidadeCandidaturas = quantidadeCandidaturas;
            if (QuantidadeCandidaturas == null)
            {
                MostrarConfirmacao(objCurriculo, objVaga);
                AjustarPanelButtonCandidatarSucesso();
            }
            else if (QuantidadeCandidaturas >= 0)
            {
                MostrarDegustacao(quantidadeCandidaturas - 1);
                if (quantidadeCandidaturas - 1 >= 0)
                    AjustarPanelButtonCandidatarSucesso();
            }
        }
        #endregion

        #region ValidarCandidato
        private void ValidarCandidato()
        {
            if (PropriedadeCandidatarVaga && !PropriedadeVerDadosEmpresa && !PropriedadeCompartilhamentoEmail) //Candidatar Vaga
            {
                Curriculo objCurriculo = Curriculo.LoadObject(base.IdCurriculo.Value);
                Vaga objVaga = Vaga.LoadObject(IdVagaCandidatar.Value);

                if (AjustaPerguntaExistente(objVaga, objCurriculo))
                    CandidatarVaga(objVaga, objCurriculo, null);
            }
            else if (PropriedadeVerDadosEmpresa && !PropriedadeCandidatarVaga && !PropriedadeCompartilhamentoEmail) //Ver Dados Empresa
            {
                Vaga objVaga = Vaga.LoadObject(IdVagaCandidatar.Value);
                ucVerDadosEmpresa.FlagConfidencial = FlagConfidencial;
                ucVerDadosEmpresa.IdFilial = (int)IdFilialPesquisaVaga;
                ucVerDadosEmpresa.MostrarModal(objVaga);
                pnlVDE.Visible = true;
                upVDE.Update();
            }
            else if (PropriedadeCompartilhamentoEmail && !PropriedadeCandidatarVaga && !PropriedadeVerDadosEmpresa) //Compartilhamento Email
            {
                int idUsuarioFilialPerfil = base.IdUsuarioFilialPerfilLogadoCandidato.Value;
                int idVaga = IdVagaCandidatar.Value;
                string urlVaga = UrlVaga;

                MostrarCompartilhamentoEmail(idUsuarioFilialPerfil, idVaga, urlVaga);
            }
        }
        #endregion

        #region AjustaPerguntaExistente
        /// <summary>
        /// Metodo responsavel por ajustar perguntar existente
        /// </summary>
        /// <param name="objVaga"></param>
        /// <param name="objCurriculo"></param>
        /// <returns>Se existir pergunta retorna false senão true</returns>
        private bool AjustaPerguntaExistente(Vaga objVaga, Curriculo objCurriculo)
        {
            if (objVaga.ExistePerguntas() && !VagaCandidato.CurriculoJaCandidatouVaga(objCurriculo, objVaga))
            {
                ucModalQuestionarioVagas.Inicializar(objVaga.IdVaga, null);
                ucModalQuestionarioVagas.MostrarModal();
                pnlMQV.Visible = true;
                upMQV.Update();
                return false;
            }
            return true;
        }
        #endregion

        #region TruncarTextoAtribuicoes
        protected string TruncarTextoAtribuicoes(object oTextoAtribuicoesCompleto)
        {
            string textoAtribuicoesCompleto = oTextoAtribuicoesCompleto.ToString();

            return textoAtribuicoesCompleto.Truncate(220);
        }
        #endregion

        #region AjustarPanelButtonCandidatarSucesso
        private void AjustarPanelButtonCandidatarSucesso()
        {
            if (rptVagas.Items.Count > IdIndexManipular) //O index a manipular pode ser 0, então se a quantidade de itens for maior que IdIndexManipular, pode executar o código
            {
                var rpti = rptVagas.Items[IdIndexManipular];

                var ib = (LinkButton)rpti.FindControl("ibtQueroMeCandidatar");
                ib.Visible = false;
                var p = (Panel)rpti.FindControl("pnlJaEnviei");
                p.Visible = true;

                var up = (UpdatePanel)rpti.FindControl("upControles");
                up.Update();
            }
        }
        #endregion

        #region MostrarCompartilhamentoEmail
        private void MostrarCompartilhamentoEmail(int idUsuarioFilialPerfil, int idVaga, string urlVaga)
        {
            ucModalCompartilhamentoVaga.Inicializar(idUsuarioFilialPerfil, idVaga, urlVaga);
            ucModalCompartilhamentoVaga.MostrarModal();
            pnlCompVaga.Visible = true;
            upCompVaga.Update();
        }
        #endregion

        #region MostrarConfirmacao
        private void MostrarConfirmacao(Curriculo objCurriculo, Vaga objVaga)
        {
            string protocolo = Vaga.RetornarProtocolo(objCurriculo.IdCurriculo, objVaga.CodigoVaga);

            ucModalConfirmacaoEnvioCurriculo.Inicializar(objCurriculo.PessoaFisica.PrimeiroNome, protocolo);
            ucModalConfirmacaoEnvioCurriculo.MostrarModal();
            pnlMCEC.Visible = true;
            upMCEC.Update();
        }
        #endregion

        #region MostrarDegustacao
        private void MostrarDegustacao(int? quantidadeCandidaturasRestantes)
        {
            ucModalDegustacaoCandidatura.Inicializar(false, false, true, quantidadeCandidaturasRestantes);
            pnlMDC.Visible = true;
            upMDC.Update();
        }
        #endregion

        #region RecuperarCssPainelVaga
        /// <summary>
        /// Ajusta o css do painel Painel Vaga
        /// </summary>
        /// <param name="flagBneRecomenda"></param>
        /// <param name="idOrigem">Identificador da origem </param>
        /// <returns></returns>
        protected string RecuperarCssPainelVaga(object flagBneRecomenda, object idOrigem)
        {
            if (Convert.ToBoolean(flagBneRecomenda))
            {
                if (base.STC.Value)
                    return "painel_vaga";

                return "painel_vaga bne_recomenda";
            }

            if (base.STC.Value)
            {
                OrigemFilial objOrigemFilial = null;
                if (OrigemFilial.CarregarPorOrigem(base.IdOrigem.Value, out objOrigemFilial) && (objOrigemFilial != null && objOrigemFilial.Filial.PossuiSTCUniversitario()) && base.IdOrigem.Value.Equals(idOrigem))
                    return "painel_vaga vip_universitario_recomenda";
            }

            return "painel_vaga";
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

        #region RetornarDesricaoSalario
        protected string RetornarDesricaoSalario(object salarioDe, object salarioAte)
        {
            decimal? valorSalarioDe = null;
            decimal? valorSalarioAte = null;

            if (salarioDe != DBNull.Value)
                valorSalarioDe = Convert.ToDecimal(salarioDe);

            if (salarioAte != DBNull.Value)
                valorSalarioAte = Convert.ToDecimal(salarioAte);

            return BLL.Custom.Helper.RetornarDesricaoSalario(valorSalarioDe, valorSalarioAte);

        }
        #endregion

        #region DeveMostrarModalSucessoFacebook
        private bool DeveMostrarModalSucessoFacebook()
        {
            return Request["__EVENTARGUMENT"] != null && Request["__EVENTARGUMENT"].ToString().Equals("mostrarSucessoFacebook");
        }
        #endregion

        #endregion

    }
}
