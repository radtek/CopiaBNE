using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Web.Caching;
using System.Web.UI;
using System.Web.UI.WebControls;
using BNE.BLL;
using BNE.BLL.Custom;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using Enumeradores = BNE.BLL.Enumeradores;
using System.Collections.Generic;
using BNE.Auth.Core.Enumeradores;

namespace BNE.Web
{
    public partial class Default : BasePage
    {

        #region Propriedades

        #region MensagemAvisoPermissao - Variável 1
        /// <summary>
        /// Propriedade que armazena e recupera a mesagem de aviso caso ocorra um erro de permissão
        /// </summary>
        protected string MensagemTemporariaAvisoPermissao
        {
            get
            {
                return ViewState[Chave.Temporaria.MensagemPermissao.ToString()] != null ? ViewState[Chave.Temporaria.MensagemPermissao.ToString()].ToString() : null;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    ViewState.Remove(Chave.Temporaria.MensagemPermissao.ToString());
                }
                else
                {
                    ViewState[Chave.Temporaria.MensagemPermissao.ToString()] = value;
                }
            }
        }

        /// <summary>
        /// Propriedade que armazena e recupera a mesagem de aviso caso ocorra um erro de permissão
        /// </summary>
        protected string MensagemRedirecionamentoAvisoPermissao
        {
            get
            {
                return ViewState[Chave.Redirecionamento.MensagemPermissaoRedirecionamento.ToString()] != null ? ViewState[Chave.Redirecionamento.MensagemPermissaoRedirecionamento.ToString()].ToString() : null;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    ViewState.Remove(Chave.Redirecionamento.MensagemPermissaoRedirecionamento.ToString());
                }
                else
                {
                    ViewState[Chave.Redirecionamento.MensagemPermissaoRedirecionamento.ToString()] = value;
                }
            }
        }
        #endregion

        #region IdVaga - Variável 10
        /// <summary>
        /// Propriedade que armazena e recupera o ID
        /// </summary>
        protected int? IdVaga
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel10.ToString()] != null)
                    return Int32.Parse(ViewState[Chave.Temporaria.Variavel10.ToString()].ToString());
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

        #region UltimasVagas - Cache
        /// <summary>
        /// Propriedade que armazena e recupera o ID
        /// </summary>
        protected DataTable UltimasVagas
        {
            get
            {
                if (Cache[UltimasVagasKey()] == null)
                {
                    try
                    {
                        int TotalRegistros;
                        UltimasVagas = BLL.PesquisaVaga.BuscaVagaAPIDT(base.IdCurriculo.HasValue ? new BLL.PesquisaVaga() { Curriculo = new Curriculo(base.IdCurriculo.Value) } : null, 4, 1, null, null, true, OrdenacaoBuscaVaga.Padrao, out TotalRegistros);
                    }
                    catch (Exception ex)
                    {
                        EL.GerenciadorException.GravarExcecao(ex, "Erro ao consultar api de vagas - tela default");
                        if (base.IdCurriculo.HasValue)
                            UltimasVagas = Vaga.ListarVagasPaginaInicial(base.IdCurriculo.Value);
                        else
                            UltimasVagas = Vaga.ListarVagasPaginaInicial(null);
                    }

                }

                return (DataTable)Cache[UltimasVagasKey()];
            }
            set
            {
                if (value != null)
                {
                    Cache.Add(UltimasVagasKey(), value, null, DateTime.Now.AddSeconds(60), Cache.NoSlidingExpiration, CacheItemPriority.High, null);
                }
            }
        }
        #endregion

        private string UltimasVagasKey()
        {
            return "UltimasVagas" + (base.IdCurriculo.HasValue ? base.IdCurriculo.Value.ToString() : "");
        }

        #region VagasAreas - Cache
        /// <summary>
        /// Propriedade que armazena a lista 
        /// </summary>
        protected Dictionary<string, int> VagasAreas
        {
            get
            {
                if (Cache["VagasAreasHome"] == null)
                    VagasAreas = BLL.PesquisaVaga.RecuperarVagasPorArea(true, 20);

                return (Dictionary<string, int>)Cache["VagasAreasHome"];
            }
            set
            {
                if (value != null)
                {
                    Cache.Add("VagasAreasHome", value, null, DateTime.Now.AddHours(24), Cache.NoSlidingExpiration, CacheItemPriority.High, null);
                }
            }
        }
        #endregion

        #endregion

        #region Eventos

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                RecuperarValorRota();

                if (base.STC.ValueOrDefault)
                {
                    OrigemFilial objOrigemFilial;
                    if (OrigemFilial.CarregarPorOrigem(base.IdOrigem.Value, out objOrigemFilial))
                    {
                        string descricao = objOrigemFilial.DescricaoPaginaInicial;

                        if (!String.IsNullOrEmpty(descricao) && descricao.Contains("data:image"))
                        {
                            string a = string.Empty;
                            if (descricao.Contains("href"))
                                a = Regex.Match(descricao, "<a.+?href=[\"'](.+?)[\"'].+?>", RegexOptions.IgnoreCase).Groups[1].Value; //Recuperando o href

                            string imageUrl = GetRouteUrl("Download", new { tipoArquivo = "imagemSTC", infoArquivo = Helper.Criptografa(base.IdOrigem.Value.ToString()) });

                            litConteudoRHOffice.Text = string.IsNullOrEmpty(a) ? string.Format("<img src='{0}' />", imageUrl) : string.Format("<a href='{0}'><img src='{1}' /><a/>", a, imageUrl);
                        }
                        else
                            litConteudoRHOffice.Text = objOrigemFilial.DescricaoPaginaInicial;
                    }
                }
                else
                {
                    CarregarVagas();
                    CarregarEmpresas();
                    CarregarAreas();
                    CarregarDepoimentos();
                    CarregarEstatisticas();
                }

                if (String.IsNullOrEmpty(MensagemTemporariaAvisoPermissao))
                {
                    if (!String.IsNullOrWhiteSpace(MensagemRedirecionamentoAvisoPermissao))
                    {
                        ExibirMensagem(MensagemRedirecionamentoAvisoPermissao, TipoMensagem.Erro);
                        MensagemRedirecionamentoAvisoPermissao = string.Empty;
                    }
                }
                else
                {
                    ExibirMensagem(MensagemTemporariaAvisoPermissao, TipoMensagem.Erro);
                }

                pnlConteudoBNE.Visible = !base.STC.ValueOrDefault;
                pnlConteudoRHOffice.Visible = base.STC.ValueOrDefault;
                stc_spacer_.Visible = base.STC.ValueOrDefault;
                
                hlQueroMaisCandidato.NavigateUrl = GetRouteUrl(Enumeradores.RouteCollection.ProdutoVIP.ToString(), null);
                hlQueroMaisEmpresa.NavigateUrl = GetRouteUrl(Enumeradores.RouteCollection.SouEmpresa.ToString(), null);
            }

            //Ajustando fluxo do RHOffice
            if (base.STC.Value && base.IdUsuarioFilialPerfilLogadoEmpresa.HasValue)
                InicializarBarraBusca(TipoBuscaMaster.Curriculo, true, "OnLoad");
            else
                InicializarBarraBusca(TipoBuscaMaster.Vaga, true, "OnLoad");

        }
        #endregion

        #region rptVagas_ItemDataBound
        protected void rptVagas_ItemDataBound(Object source, RepeaterItemEventArgs e)
        {
            var lit = (Literal)e.Item.FindControl("litUltimasVagasCadastradaHa");
            var txtHorario = (Label)e.Item.FindControl("lblHorario");

            if (lit == null || txtHorario == null)
                return;

            var dtAbertura = Convert.ToDateTime(DataBinder.Eval(e.Item.DataItem, "Dta_Abertura"));
            lit.Text = !dtAbertura.Day.Equals(DateTime.Now.Day) ? "Cadastrada há" : $"Cadastrada às";
            txtHorario.Text = !dtAbertura.Day.Equals(DateTime.Now.Day) ? $"{DateTime.Now.Day - dtAbertura.Day} dia{(DateTime.Now.Day - dtAbertura.Day > 1 ? "s" : "")}" : dtAbertura.TimeOfDay.ToString("hh\\:mm");
        }
        #endregion

        #region btnCurriculoPesquisa_Click
        protected void btnCurriculoPesquisa_Click(object sender, EventArgs e)
        {
            Redirect(string.Concat("http://", Helper.RecuperarURLVagas(), "/cadastro-de-empresa-gratis"));
        }
        #endregion

        #region btiCadastreCurriculo_Click
        protected void btiCadastreCurriculo_Click(object sender, EventArgs e)
        {
            if (base.IdPessoaFisicaLogada.HasValue)
            {
                if (base.IdCurriculo.HasValue)
                {
                    Session.Add(Chave.Temporaria.Variavel1.ToString(), base.IdPessoaFisicaLogada.Value);
                    Redirect(Page.GetRouteUrl(Enumeradores.RouteCollection.CadastroCurriculoMini.ToString(), null));
                }
                else
                    Redirect(Page.GetRouteUrl(Enumeradores.RouteCollection.CadastroCurriculoMini.ToString(), null));
            }
            else
                Redirect(Page.GetRouteUrl(Enumeradores.RouteCollection.CadastroCurriculoMini.ToString(), null));
        }
        #endregion

        #endregion

        #region Métodos

        #region CarregarVagas
        private void CarregarVagas()
        {
            UIHelper.CarregarRepeater(rptUltimasVagas, UltimasVagas);
            btnUltimasVagas.HRef = string.Concat("http://", Helper.RecuperarURLVagas(), "/vagas-de-emprego");
        }
        #endregion

        #region CarregarAreas
        private void CarregarAreas()
        {
            UIHelper.CarregarRepeater(rptAreas, VagasAreas);
        }
        #endregion

        #region CarregarEmpresas
        private void CarregarEmpresas()
        {
            UIHelper.CarregarRepeater(rptEmpresas, EmpresaHome.ListarEmpresasHome());
        }
        #endregion

        #region CarregarDepoimentos
        private void CarregarDepoimentos()
        {
            UIHelper.CarregarRepeater(rptDepoimentos, Agradecimento.ListarAgradecimentosParaHome());
        }
        #endregion

        private void CarregarEstatisticas()
        {
            litQtdCurriculos.Text = Estatistica.Estatisticas.QuantidadeCurriculo.ToString("N0");
            litQtdVagas.Text = Estatistica.Estatisticas.QuantidadeVaga.Value.ToString("N0");
            litQtdEmpresas.Text = Estatistica.Estatisticas.QuantidadeEmpresa.ToString("N0");
        }

        #region RetornarDesricaoSalario
        protected string RetornarDesricaoSalario(object salarioDe, object salarioAte)
        {
            decimal? valorSalarioDe = null;
            decimal? valorSalarioAte = null;

            if (salarioDe != DBNull.Value && Convert.ToDecimal(salarioDe) > 0)
                valorSalarioDe = Convert.ToDecimal(salarioDe);

            if (salarioAte != DBNull.Value && Convert.ToDecimal(salarioAte) > 0)
                valorSalarioAte = Convert.ToDecimal(salarioAte);

            return UIHelper.RetornarDesricaoSalario(valorSalarioDe, valorSalarioAte);
        }
        #endregion

        #region RecuperarValorRota
        protected void RecuperarValorRota()
        {
            if (RouteData.Values.Count > 0)
            {
                if (RouteData.Values["stc"] != null)
                {
                    var stc = RouteData.Values["stc"].ToString();
                    string rotaOriginal = (string)RouteData.Values["rotaOriginal"];

                    OrigemFilial objOrigemFilial;
                    if (OrigemFilial.CarregarPorDiretorio(stc, out objOrigemFilial))
                    {
                        if (IdPessoaFisicaLogada.HasValue &&
                            IdUsuarioFilialPerfilLogadoEmpresa.HasValue &&
                            !IdUsuarioFilialPerfilLogadoUsuarioInterno.HasValue &&
                            objOrigemFilial.Origem != null && objOrigemFilial.Origem.IdOrigem > 0 &&
                            objOrigemFilial.Origem.IdOrigem != IdOrigem.ValueOrDefault)
                        {
                            BNE.Auth.BNEAutenticacao.DeslogarPadrao(LogoffType.COMPANY_TRYING_TO_USE_OTHER_STC);
                            Session[Chave.Redirecionamento.MensagemPermissaoRedirecionamento.ToString()] =
                                "Seu acesso foi encerrado, você não tem permissão de utilizar está área como empresa.";
                        }

                        objOrigemFilial.Template.CompleteObject();

                        Tema.Value = objOrigemFilial.Template.NomeTemplate;
                        IdOrigem.Value = objOrigemFilial.Origem.IdOrigem;
                        STC.Value = true;
                        Redirect("/" + (rotaOriginal ?? String.Empty));
                    }
                }
            }
        }
        #endregion

        #region RetornarURLVaga
        protected string RetornarURLVaga(string areaBNE, string funcao, string nomeCidade, string siglaEstado, int identificador)
        {
            return SitemapHelper.MontarUrlVaga(funcao, areaBNE, nomeCidade, siglaEstado, identificador);
        }
        #endregion

        #region RetornarURLVagasEmpresa
        protected string RetornarURLVagasEmpresa(string nomeEmpresa, int idFilial)
        {
            return Helper.RecuperarURLVagasEmpresa(nomeEmpresa, idFilial);
        }
        #endregion

        #endregion

        #region RecuperarJobFuncao
        /// <summary>
        /// Validar Funcao
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.Read)]
        public string RecuperarJobFuncao(string valor)
        {
            Funcao objFuncao;
            if (Funcao.CarregarPorDescricao(valor, out objFuncao))
            {
                if (!string.IsNullOrEmpty(objFuncao.DescricaoJob))
                    return objFuncao.DescricaoJob;
                return String.Empty;
            }
            return String.Empty;
        }
        #endregion

    }
}
