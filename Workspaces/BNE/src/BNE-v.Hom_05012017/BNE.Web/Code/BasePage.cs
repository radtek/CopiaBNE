using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using Ajax;
using BNE.Auth;
using BNE.Auth.Core.Enumeradores;
using BNE.BLL;
using BNE.BLL.Custom;
using BNE.Common.Session;
using BNE.EL;
using BNE.Web.Code.Enumeradores;
using BNE.Web.Master;
using BNE.Web.Resources;
using Microsoft.IdentityModel.Claims;
using Resources;
using Enumerador = BNE.BLL.Enumeradores;

namespace BNE.Web.Code
{
    public class BasePage : Page
    {
        #region Constantes
        private Dictionary<string, string> _seoParameters = new Dictionary<string, string>();
        #endregion

        /// <summary>
        ///     Essa propriedade é utilizada para que as páginas definam valores a serem substuídos nas descrições.
        ///     Ex.: na pesquisa de currículos, marcada com a string {Funcao} nas url's SEO.
        ///     Para evitar uma nova requisição no bd para montar a descrição, a página define esses valores no evento onLoad para
        ///     o item "Funcao" do dicionário
        /// </summary>
        internal Dictionary<string, string> SeoParameters
        {
            get { return _seoParameters; }
            set { _seoParameters = value; }
        }

        #region Session
        public SessionVariable<int> IdCurriculo = new SessionVariable<int>(Chave.Permanente.IdCurriculo.ToString());
        public SessionVariable<int> IdPessoaFisicaLogada = new SessionVariable<int>(Chave.Permanente.IdPessoaFisicaLogada.ToString());
        public SessionVariable<int> IdUsuarioLogado = new SessionVariable<int>(Chave.Permanente.IdUsuarioLogado.ToString());
        public SessionVariable<int> IdUsuarioFilialPerfilLogadoCandidato = new SessionVariable<int>(Chave.Permanente.IdUsuarioFilialPerfilLogadoCandidato.ToString());
        public SessionVariable<int> IdUsuarioFilialPerfilLogadoUsuarioInterno = new SessionVariable<int>(Chave.Permanente.IdUsuarioFilialPerfilLogadoUsuarioInterno.ToString());
        public SessionVariable<int> IdUsuarioFilialPerfilLogadoEmpresa = new SessionVariable<int>(Chave.Permanente.IdUsuarioFilialPerfilLogadoEmpresa.ToString());
        public SessionVariable<int> IdPerfil = new SessionVariable<int>(Chave.Permanente.IdPerfil.ToString());
        public SessionVariable<int> IdFilial = new SessionVariable<int>(Chave.Permanente.IdFilial.ToString());
        public SessionVariable<string> Tema = new SessionVariable<string>(Chave.Permanente.Theme.ToString());
        public SessionVariable<string> UrlDestinoPagamento = new SessionVariable<string>(Chave.Permanente.UrlDestinoPagamento.ToString());
        public SessionVariable<string> UrlDestino = new SessionVariable<string>(Chave.Permanente.UrlDestino.ToString());
        public SessionVariable<int> IdVaga = new SessionVariable<int>(Chave.Permanente.IdVaga.ToString());

        public SessionVariable<TipoBuscaMaster> TipoBusca = new SessionVariable<TipoBuscaMaster>(Chave.Permanente.TipoBuscaMaster.ToString());
        public SessionVariable<string> FuncaoMaster = new SessionVariable<string>(Chave.Permanente.FuncaoMaster.ToString());
        public SessionVariable<string> CidadeMaster = new SessionVariable<string>(Chave.Permanente.CidadeMaster.ToString());
        public SessionVariable<string> PalavraChaveMaster = new SessionVariable<string>(Chave.Permanente.PalavraChaveMaster.ToString());

        public SessionVariable<bool> STC = new SessionVariable<bool>(Chave.Permanente.STC.ToString());
        public SessionVariable<int> IdOrigem = new SessionVariable<int>(Chave.Permanente.IdOrigem.ToString());
        public SessionVariable<bool> IsSTCMaster = new SessionVariable<bool>(Chave.Permanente.IsSTCMaster.ToString());

        //Pagamento
        public SessionVariable<int> PagamentoIdentificadorPagamento = new SessionVariable<int>(Chave.Permanente.PagamentoIdentificadorPagamento.ToString());
        public SessionVariable<int> PagamentoIdentificadorPlano = new SessionVariable<int>(Chave.Permanente.PagamentoIdentificadorPlano.ToString());
        public SessionVariable<int> PagamentoIdentificadorPlanoAdquirido = new SessionVariable<int>(Chave.Permanente.PagamentoIdentificadorPlanoAdquirido.ToString());
        public SessionVariable<string> PagamentoJustificativaAbaixoMinimo = new SessionVariable<string>(Chave.Permanente.PagamentoJustificativaAbaixoMinimo.ToString());

        #region Adicional

        #region PagamentoAdicionalValorTotal
        /// <summary>
        ///     Utilizado quando não é a compra de um plano, mas de um adicional de plano.
        /// </summary>
        public SessionVariable<decimal> PagamentoAdicionalValorTotal = new SessionVariable<decimal>(Chave.Permanente.PagamentoAdicionalValorTotal.ToString());
        #endregion

        #region PagamentoAdicionalQuantidade
        /// <summary>
        ///     Utilizado quando não é a compra de um plano, mas de um adicional de plano.
        /// </summary>
        public SessionVariable<int> PagamentoAdicionalQuantidade = new SessionVariable<int>(Chave.Permanente.PagamentoAdicionalQuantidade.ToString());
        #endregion

        #endregion

        public SessionVariable<string> PagamentoUrlRetorno = new SessionVariable<string>(Chave.Permanente.PagamentoUrlRetorno.ToString());
        public SessionVariable<int> PagamentoIdCodigoDesconto = new SessionVariable<int>(Chave.Permanente.PagamentoIdCodigoDesconto.ToString());
        public SessionVariable<int> PagamentoFormaPagamento = new SessionVariable<int>(Chave.Permanente.PagamentoFormaPagamento.ToString());
        public SessionVariable<decimal> PagamentoValorPago = new SessionVariable<decimal>(Chave.Permanente.PagamentoValorPago.ToString());
        public SessionVariable<int> PrazoBoleto = new SessionVariable<int>(Chave.Permanente.PrazoBoleto.ToString());
        public SessionVariable<decimal> ValorBasePlano = new SessionVariable<decimal>(Chave.Permanente.ValorBasePlano.ToString());

        public SessionVariable<Dictionary<int, bool>> DicCurriculos = new SessionVariable<Dictionary<int,bool>>(Chave.Temporaria.Variavel1.ToString());

            //public SessionVariable<Dictionary<ResultadoPesquisaCurriculo.TipoPesquisa, Dictionary<int, List<KeyValuePair<int, bool>>>>> PesquisaCurriculoCurriculos = new SessionVariable<Dictionary<ResultadoPesquisaCurriculo.TipoPesquisa, Dictionary<int, List<KeyValuePair<int, bool>>>>>(Chave.Permanente.PesquisaCurriculo.ToString(), () =>
        //{
        //    return Enum.GetValues(typeof(ResultadoPesquisaCurriculo.TipoPesquisa)).Cast<ResultadoPesquisaCurriculo.TipoPesquisa>().ToDictionary(key => key, key => new Dictionary<int, List<KeyValuePair<int, bool>>>());
        //});

        #region LimparSession
        /// <summary>
        ///     Método responsável por limpar os valores necessarios na identificação de um usuário.
        /// </summary>
        [Obsolete("Utilizar BNEAutenticacao")]
        public void LimparSession()
        {
            IdPessoaFisicaLogada.Clear();
            IdCurriculo.Clear();
            IdFilial.Clear();
            IdUsuarioLogado.Clear();
            IdUsuarioFilialPerfilLogadoCandidato.Clear();
            IdUsuarioFilialPerfilLogadoEmpresa.Clear();
            IdUsuarioFilialPerfilLogadoUsuarioInterno.Clear();
            FuncaoMaster.Clear();
            CidadeMaster.Clear();
            PalavraChaveMaster.Clear();
            IdPerfil.Clear();
            IsSTCMaster.Clear();

            var context = HttpContext.Current ?? Context;

            SessionAbandonRestoreMediator.Instance.RaiseSessionCloseManually(new HttpContextWrapper(context).Session);
            context.Session.Abandon();
        }
        #endregion

        #region LimparSessionPagamento
        /// <summary>
        ///     Limpa as propriedades relacionado a compra de planos armazenadas na sessão do usuário
        /// </summary>
        public void LimparSessionPagamento()
        {
            PagamentoIdentificadorPagamento.Clear();
            PagamentoIdentificadorPlano.Clear();
            PagamentoUrlRetorno.Clear();
            PagamentoIdCodigoDesconto.Clear();
            PagamentoAdicionalValorTotal.Clear();
            PagamentoAdicionalQuantidade.Clear();
            PagamentoFormaPagamento.Clear();
            PagamentoValorPago.Clear();
            ValorBasePlano.Clear();
            
        }
        #endregion

        #region SessionDefault
        /// <summary>
        ///     Método responsável por limpar os valores necessarios na identificação de um usuário.
        /// </summary>
        public void SessionDefault()
        {
            Tema.Value = string.Empty;
            STC.Value = false;
            IdOrigem.Value = 1; //BNE
            TipoBusca.Value = TipoBuscaMaster.Vaga;
        }
        #endregion

        #endregion

        #region Métodos

        #region ExibirMensagem
        protected void ExibirMensagem(string mensagem, TipoMensagem tipo, bool aumentarTamanhoPainelMensagem = false)
        {
            var principal = (Principal)Page.Master;

            if (principal != null)
            {
                principal.ExibirMensagem(mensagem, tipo, aumentarTamanhoPainelMensagem);
                return;
            }

            var cur = this as VisualizacaoCurriculo;
            if (cur != null)
            {
                cur.ExibirMensagem(mensagem, tipo, aumentarTamanhoPainelMensagem);
                return;
            }

            var lPage = this as Login;
            if (lPage != null)
            {
                lPage.ExibirMensagem(mensagem, tipo, aumentarTamanhoPainelMensagem);
            }
        }
        #endregion

        #region EmpresaEmAuditoria
        protected bool EmpresaEmAuditoria(Filial objFilial)
        {
            var principal = (Principal)Page.Master;

            if (principal != null)
                return principal.EmpresaEmAuditoria(objFilial);

            return false;
        }
        #endregion

        #region EmpresaBloqueada
        protected bool EmpresaBloqueada(Filial objFilial)
        {
            var principal = (Principal)Page.Master;

            if (principal != null)
                return principal.EmpresaBloqueada(objFilial);

            return false;
        }
        #endregion

        #region SetarFoco
        protected void SetarFoco(Control controle)
        {
            var principal = (Principal)Page.Master;

            if (principal != null)
                principal.SetarFoco(controle);
        }
        #endregion

        #region ExibirMensagemConfirmacao
        protected void ExibirMensagemConfirmacao(string titulo, string mensagem, bool cliqueAqui)
        {
            var principal = (Principal)Page.Master;

            if (principal != null)
                principal.ExibirMensagemConfirmacao(titulo, mensagem, cliqueAqui);
        }
        #endregion

        #region ExibirLogin
        protected void ExibirLogin()
        {
            var principal = (Principal)Page.Master;

            if (principal != null)
                principal.ExibirLogin();
        }
        
        protected void ExibirLoginEmpresa()
        {
            var principal = (Principal)Page.Master;

            if (principal != null)
                principal.ExibirLoginEmpresa();
        }
        #endregion

        #region PropriedadeAjustarTopoUsuarioEmpresa
        protected void PropriedadeAjustarTopoUsuarioEmpresa(bool ajustarTopo)
        {
            var principal = (Principal)Page.Master;

            if (principal != null)
                principal.PropriedadeAjustarTopoUsuarioEmpresa = ajustarTopo;
        }
        #endregion

        #region PropriedadeAjustarTopoUsuarioCandidato
        protected void PropriedadeAjustarTopoUsuarioCandidato(bool ajustarTopo)
        {
            var principal = (Principal)Page.Master;

            if (principal != null)
                principal.PropriedadeAjustarTopoUsuarioCandidato = ajustarTopo;
        }
        #endregion

        #region AjustarTopoRodape
        public virtual void AjustarTopoRodape()
        {
            var principal = (Principal)Page.Master;

            if (principal != null)
                principal.AjustarTopoRodape();
        }
        #endregion

        #region AjustarLogin
        protected void AjustarLogin()
        {
            var principal = (Principal)Page.Master;
            if (principal != null)
                principal.AjustarLogin();
        }
        #endregion

        #region RecuperarCookieAcesso
        public HttpCookie RecuperarCookieAcesso()
        {
            HttpCookie c;

            var req = (HttpContext.Current != null ? HttpContext.Current.Request : Request);
            if (req.Cookies["BNE_Acesso"] != null) // for mvc
                c = req.Cookies["BNE_Acesso"];
            else
            {
                var tempo = Convert.ToInt32(Parametro.RecuperaValorParametro(Enumerador.Parametro.CookieAcessoHorasExpiracao));

                c = new HttpCookie("BNE_Acesso")
                {
                    Expires = DateTime.Now.AddHours(tempo)
                };
            }

#if DEBUG
            c.Domain = "localhost";
#else
            c.Domain = ".bne.com.br";
#endif

            return c;
        }
        #endregion

        #region GravarCookieAcesso
        public void GravarCookieAcesso(PessoaFisica objPessoaFisica)
        {
            var cookie = RecuperarCookieAcesso();
            cookie.Values["HashAcesso"] = LoginAutomatico.GerarHashAcessoLogin(objPessoaFisica);
            (HttpContext.Current != null ? HttpContext.Current.Response : Response).Cookies.Add(cookie);
        }
        #endregion

        #region LimparCookieAcesso
        [Obsolete("Utilizar BNEAutenticacao")]
        public void LimparCookieAcesso()
        {
            var cookie = RecuperarCookieAcesso();
            cookie.Expires = DateTime.Now.AddDays(-1d);
            (HttpContext.Current != null ? HttpContext.Current.Response : Response).Cookies.Add(cookie);
        }
        #endregion

        #region GravarCookieLoginVagas
        public void GravarCookieLoginVagas(PessoaFisica objPessoaFisica)
        {
            var cookie = RecuperarCookieLoginVagas();
            cookie.Values["HashAcesso"] = LoginAutomatico.GerarHashAcessoLogin(objPessoaFisica);
            (HttpContext.Current != null ? HttpContext.Current.Response : Response).Cookies.Add(cookie);
        }
        #endregion

        #region RecuperarCookieLoginVagas
        public HttpCookie RecuperarCookieLoginVagas()
        {
            const string cookie = "BNE";
            HttpCookie c;

            var req = (HttpContext.Current != null ? HttpContext.Current.Request : Request);
            if (req.Cookies[cookie] != null) // mvc
                c = req.Cookies[cookie];
            else
            {
                var tempo = Convert.ToInt32(Parametro.RecuperaValorParametro(Enumerador.Parametro.CookieAcessoHorasExpiracao));

                c = new HttpCookie(cookie)
                {
                    Expires = DateTime.Now.AddHours(tempo)
                };
            }

#if DEBUG
            c.Domain = "localhost";
#else
            c.Domain = ".bne.com.br";
#endif

            return c;
        }
        #endregion

        #region LimparCookieLoginVagas
        [Obsolete("Utilizar BNEAutenticacao")]
        public void LimparCookieLoginVagas()
        {
            var cookie = RecuperarCookieLoginVagas();
            cookie.Expires = DateTime.Today.AddDays(-1);
            (HttpContext.Current != null ? HttpContext.Current.Response : Response).Cookies.Set(cookie); // for mvc
        }
        #endregion

        #region LimparCookieLoginVagasMaster
        [Obsolete("Utilizar BNEAutenticacao")]
        public HttpCookie LimparCookieLoginVagasMaster()
        {
            var principal = (Principal)Page.Master;
            if (principal != null)
                principal.LimparCookieLoginVagas();

            return null;
        }
        #endregion

        #region RecuperarCookieTelefoneChameFacil
        public HttpCookie RecuperarCookieTelefoneChameFacil()
        {
            HttpCookie c;

            if (Request.Cookies["TelefoneChameFacil"] != null)
                c = Request.Cookies["TelefoneChameFacil"];
            else
            {
                var tempo = Convert.ToInt32(Parametro.RecuperaValorParametro(Enumerador.Parametro.CookieAcessoHorasExpiracao));

                c = new HttpCookie("TelefoneChameFacil")
                {
                    Expires = DateTime.Now.AddHours(tempo)
                };
            }

            return c;
        }
        #endregion

        #region GravarCookieTelefoneChameFacil
        public void GravarCookieTelefoneChameFacil(string telefone)
        {
            var cookie = RecuperarCookieTelefoneChameFacil();
            cookie.Values["TelefoneChameFacil"] = telefone;
            Response.Cookies.Add(cookie);
        }
        #endregion

        #region LimparCookieTelefoneChameFacil
        public void LimparCookieTelefoneChameFacil()
        {
            var cookie = RecuperarCookieTelefoneChameFacil();
            cookie.Expires = DateTime.Now.AddDays(-1d);
            Response.Cookies.Add(cookie);
        }
        #endregion

        #region GravarCookiePreCadastro
        public void GravarCookiePreCadastro(string key, string value)
        {
            if (string.IsNullOrEmpty(key) == false)
            {
                var cookie = RecuperarCookiePreCadastro();
                cookie.Values[key] = HttpUtility.UrlEncode(value);
                Response.Cookies.Add(cookie);
            }
        }
        #endregion

        #region Cookie

        #region RecuperarCookie
        private HttpCookie RecuperarCookie(BLL.Enumeradores.Cookie cookie, bool gerarSeNaoExistir = true, int? horasExpiracao = null)
        {
            return RecuperarCookie(cookie.ToString(), gerarSeNaoExistir, horasExpiracao);
        }
        public HttpCookie RecuperarCookie(string cookie, bool gerarSeNaoExistir = true, int? horasExpiracao = null)
        {
            var cookieName = Convert.ToString(cookie);

            HttpCookie httpCookie = null;

            var req = (HttpContext.Current != null ? HttpContext.Current.Request : Request); //mvc
            if (req.Cookies[cookieName] != null)
                httpCookie = req.Cookies[cookieName];

            if (gerarSeNaoExistir && httpCookie == null)
            {
                var dataExpiracao = horasExpiracao.HasValue ? DateTime.Now.AddHours(horasExpiracao.Value) : DateTime.MaxValue;

                httpCookie = new HttpCookie(cookieName)
                {
                    Expires = dataExpiracao
                };
            }

            if (httpCookie != null)
            {
#if DEBUG
                httpCookie.Domain = "localhost";
#else
                httpCookie.Domain = ".bne.com.br";
#endif
            }

            return httpCookie;
        }
        #endregion

        #region RecuperarValorCookie
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cookie">Nome do cookie</param>
        /// <param name="key">Chave que será lido o valor, se passar vazio o value vem do cookie</param>
        public string RecuperarValorCookie(BLL.Enumeradores.Cookie cookie, string key = "")
        {
            var httpCookie = RecuperarCookie(cookie);
            if (httpCookie == null)
                return string.Empty;

            return !string.IsNullOrWhiteSpace(key) ? httpCookie.Values[key] : httpCookie.Value;
        }
        #endregion

        #region GravarValorCookie
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cookie">Nome do cookie</param>
        /// <param name="key">Chave que será atribuido o valor, se passar vazio o value vai para o cookie</param>
        /// <param name="value">Valor que será armazenado</param>
        public void GravarValorCookie(BLL.Enumeradores.Cookie cookie, string key, string value)
        {
            var httpCookie = RecuperarCookie(cookie);

            var response = (HttpContext.Current != null ? HttpContext.Current.Response : Response); //mvc

            if (!string.IsNullOrWhiteSpace(key))
            {
                httpCookie.Values[key] = HttpUtility.UrlEncode(value);
            }
            else
            {
                httpCookie.Value = HttpUtility.UrlEncode(value);
            }

            response.Cookies.Add(httpCookie);
        }
        #endregion

        #endregion

        #region RecuperarCookiePreCadastro
        public HttpCookie RecuperarCookiePreCadastro()
        {
            HttpCookie c;
            //var req = (HttpContext.Current != null ? HttpContext.Current.Request : Request);
            if (Request.Cookies["PreCadastro"] != null)
                c = Request.Cookies["PreCadastro"];
            else
            {
                c = new HttpCookie("PreCadastro")
                {
                    Expires = DateTime.MaxValue
                };
            }
            return c;
        }
        #endregion

        #region DefinirTituloPagina
        public OrigemFilial DefinirTituloPagina()
        {
            //Pega o caminho da página que está sendo iniciada, e substitui .aspx por vazio
            var pagina = Path.GetFileNameWithoutExtension(Page.AppRelativeVirtualPath);

            if (string.IsNullOrWhiteSpace(pagina))
                return null;

            var caminho = Path.GetFileNameWithoutExtension(Page.Request.Url.AbsolutePath);
            if (string.IsNullOrWhiteSpace(caminho))
                return null;

            caminho = caminho.Replace('-', '_');

            //Instancia um ResourceManager para trabalhar com o arquivo de títulos
            var rmTit = new ResourceManager("BNE.Web.Resources.TituloPagina", Assembly.GetExecutingAssembly());
            var rmDesc = new ResourceManager("BNE.Web.Resources.DescricaoPagina",
                Assembly.GetExecutingAssembly());
            var rmKey = new ResourceManager("BNE.Web.Resources.PalavraChave", Assembly.GetExecutingAssembly());

            //Recupera o título da tela
            var strTituloTela = rmTit.GetString(pagina.ToUpper()) ??
                                rmTit.GetString(string.Format("{0}_{1}", pagina.ToUpper(), caminho.ToUpper()));
            var strDescricaoTela = rmDesc.GetString(pagina.ToUpper()) ??
                                   rmDesc.GetString(string.Format("{0}_{1}", pagina.ToUpper(),
                                       caminho.ToUpper()));
            var strPalavraChaveTela = rmKey.GetString(pagina.ToUpper()) ??
                                      rmKey.GetString(string.Format("{0}_{1}", pagina.ToUpper(),
                                          caminho.ToUpper()));
            var strNomeProjeto = rmTit.GetString("TITULOPADRAOPROJETO");

            OrigemFilial objOrigemFilial;
            if (STC.ValueOrDefault)
            {
                if (OrigemFilial.CarregarPorOrigem(IdOrigem.Value, out objOrigemFilial))
                {
                    //TODO: Performance carregar só o nome fantasia, estudar a possibilidade de armazenar na session para evitar consultas desnecessárias ao banco de dados
                    objOrigemFilial.Filial.CompleteObject();
                    strNomeProjeto = objOrigemFilial.Filial.NomeFantasia;
                }
            }
            else
            {
                objOrigemFilial = null;
            }

            Page.Title = string.Format("{0} | {1}", strTituloTela, strNomeProjeto);

            //Definindo META TAGS
            if (!string.IsNullOrEmpty(strPalavraChaveTela))
            {
                var metaTagKeywords = new HtmlMeta
                {
                    Name = "keywords",
                    Content = strPalavraChaveTela
                };

                Page.Header.Controls.AddAt(0, metaTagKeywords);
            }

            if (!string.IsNullOrEmpty(strDescricaoTela))
            {
                var metaTagDescription = new HtmlMeta
                {
                    Name = "description",
                    Content = strDescricaoTela
                };

                Page.Header.Controls.AddAt(0, metaTagDescription);
            }
            return objOrigemFilial;
        }
        #endregion

        #region InicializarBarraBusca
        protected void InicializarBarraBusca(TipoBuscaMaster tipoBuscaMaster, bool expandida, string strKey = "")
        {
            if (string.IsNullOrWhiteSpace(strKey))
                strKey = DateTime.Now.Ticks.ToString();

            var principal = (Principal)Page.Master;
            if (principal != null)
                principal.InicializarBarraBusca(tipoBuscaMaster, expandida, strKey);
        }
        #endregion

        #region RegistrarTipoAjax
        private void RegistrarTipoAjax()
        {
            //Registra o tipo para funcionar com Ajax
            Utility.RegisterTypeForAjax(Page.GetType().BaseType);
        }
        #endregion

        #region AjustarTituloTela
        protected void AjustarTituloTela(string tituloTela)
        {
            var principal = (Principal)Page.Master;

            if (principal != null)
                principal.AjustarTituloTela(tituloTela);
        }
        #endregion

        #region ExibirMenuSecaoEmpresa
        protected void ExibirMenuSecaoEmpresa()
        {
            var principal = (Principal)Page.Master;

            if (principal != null)
                principal.ExibirMenuSecaoEmpresa();
        }
        #endregion

        #region ExibirMenuApenasAtualizarEmpresa
        protected void ExibirMenuApenasAtualizarEmpresa()
        {
            var principal = (Principal)Page.Master;
            if (!IsSTCMaster.HasValue && IdUsuarioFilialPerfilLogadoEmpresa.HasValue && STC.HasValue)
            {
                if (STC.Value)
                {
                    var usuarioFilialPerfil = UsuarioFilialPerfil.LoadObject(IdUsuarioFilialPerfilLogadoEmpresa.Value);
                    if (usuarioFilialPerfil != null)
                    {
                        if (usuarioFilialPerfil.Perfil.IdPerfil == (int)Enumerador.Perfil.AcessoEmpresaMaster)
                        {
                            IsSTCMaster.Value = true;
                            if (principal != null)
                                principal.ExibirMenuApenasAtualizarEmpresa();
                        }
                    }
                }
            }
            else if (IsSTCMaster.HasValue)
            {
                if (IsSTCMaster.Value)
                {
                    if (principal != null)
                        principal.ExibirMenuApenasAtualizarEmpresa();
                }
            }
        }
        #endregion

        #region ExibirMenuSecaoCandidato
        protected void ExibirMenuSecaoCandidato()
        {
            var principal = (Principal)Page.Master;

            if (principal != null)
                principal.ExibirMenuSecaoCandidato();
        }
        #endregion

        #region GerarLinkDownload
        /// <summary>
        ///     Passa a string para gerar o arquivo para a tela de geração de arquivo (DownloadArquivo.aspx)
        /// </summary>
        /// <param name="bytes">Byte array de dados que devem estar no arquivo</param>
        /// <param name="nomeArquivo">Nome do arquivo a ser gerado</param>
        public string GerarLinkDownload(byte[] bytes, string nomeArquivo)
        {
            var path = Path.Combine(Configuracao.PastaArquivoTemporario, nomeArquivo);

            if (!Directory.Exists(Configuracao.PastaArquivoTemporario))
                Directory.CreateDirectory(Configuracao.PastaArquivoTemporario);

            File.WriteAllBytes(path, bytes);

            return GetRouteUrl("Download", new { tipoArquivo = "temp", infoArquivo = Helper.Criptografa(nomeArquivo) });
        }
        #endregion

        #region ExibirMensagemErro
        protected void ExibirMensagemErro(Exception ex)
        {
            ExibirMensagemErro(ex, null);
        }

        protected void ExibirMensagemErro(Exception ex, string customMessage)
        {
            if (!(ex is ThreadAbortException))
            {
                string message;
                GerenciadorException.GravarExcecao(ex, out message, customMessage);
                ExibirMensagem(string.IsNullOrEmpty(customMessage) ? message : customMessage, TipoMensagem.Erro);
            }
        }
        #endregion

        #region Redirect
        protected void Redirect(string url)
        {
            try
            {
                Response.Redirect(url, true);
            }
            catch (Exception ex)
            {
                if (!(ex is ThreadAbortException))
                    GerenciadorException.GravarExcecao(ex);
            }
        }
        #endregion

        #region FormatarCidade
        protected string FormatarCidade(string nomeCidade, string siglaEstado)
        {
            return UIHelper.FormatarCidade(nomeCidade, siglaEstado);
        }
        #endregion

        #region LogarAutomatico
        /// <summary>
        ///     Processo para logar o usuário automaticamente
        ///     1 - Ver se tem hash de acesso na url
        ///     2 - Tenta recuperar do cookie
        /// </summary>
        private void LogarAutomatico()
        {
            if (RouteData.Values.Count > 0 && RouteData.Values["HashAcesso"] != null) //Recupera a hash de acesso
            {
                var hashAcesso = RouteData.Values["HashAcesso"].ToString();
                var objLoginAutomatico = LoginAutomatico.RecuperarInformacaoHash(hashAcesso);
                if (objLoginAutomatico.IdVaga.HasValue && objLoginAutomatico.IdVaga > 0)
                {
                    objLoginAutomatico.Url = Vaga.MontarUrlVaga(objLoginAutomatico.IdVaga.Value).Replace("www.bne.com.br", "vagas.bne.com.br");
                }
                if (!string.IsNullOrEmpty(Request.QueryString.ToString()))
                {
                    // BNE.Web.Master.Principal pm = new Principal();
                    //pm.PegarOrigemAcesso();
                    Session["OrigemHTTP_REFERER"] = Request.ServerVariables["HTTP_REFERER"] != null ? Request.ServerVariables["HTTP_REFERER"] : "";
                    Session["OrigemQUERY_STRING"] = Request.ServerVariables["QUERY_STRING"];
                    Session["OrigemUtmSource"] = Request.QueryString["utm_Source"] != null ? Request.QueryString["utm_Source"] : "";
                    Session["OrigemUtmMedium"] = Request.QueryString["utm_Medium"] != null ? Request.QueryString["utm_Medium"] : "";
                    Session["OrigemUtmCampaign"] = Request.QueryString["utm_campaign"] != null ? Request.QueryString["utm_campaign"] : "";
                    Session["OrigemUtmTerm"] = Request.QueryString["utm_term"] != null ? Request.QueryString["utm_term"] : "";
                    Session["OrigemSalva"] = "1";

                    objLoginAutomatico.Url += "?" + Request.QueryString;
                }

                LogarAutomaticoPessoaFisica(objLoginAutomatico);
            }
            else if (!IdPessoaFisicaLogada.HasValue) //Recupera o cookie se tiver
            {
                var cookie = RecuperarCookieAcesso();
                if (cookie != null)
                {
                    var hashAcesso = cookie["HashAcesso"];

                    if (hashAcesso != null)
                    {
                        var objLoginAutomatico = LoginAutomatico.RecuperarInformacaoHash(hashAcesso);
                        LogarAutomaticoPessoaFisica(objLoginAutomatico);
                    }
                }
            }
        }
        #endregion

        #region GravarOrigemAcesso
        public void GravarOrigemAcesso(int idUsuarioFilialPerfil)
        {
            try
            {
                var strOrigemUrlReferr = Session["OrigemHTTP_REFERER"] != null ? Session["OrigemHTTP_REFERER"].ToString() : "";
                var strOrigemQuery = Session["OrigemQUERY_STRING"] != null ? Session["OrigemQUERY_STRING"].ToString() : "";
                var strOrigemUtmSource = Session["OrigemUtmSource"] != null ? Session["OrigemUtmSource"].ToString() : "";
                var strOrigemUtmMedium = Session["OrigemUtmMedium"] != null ? Session["OrigemUtmMedium"].ToString() : "";
                var strOrigemUtmCampaign = Session["OrigemUtmCampaign"] != null ? Session["OrigemUtmCampaign"].ToString() : "";
                var strOrigemUtmTerm = Session["OrigemUtmTerm"] != null ? Session["OrigemUtmTerm"].ToString() : "";
                var strOrigemUtmKeyWords = ""; // Session["OrigemQUERY_STRING"].ToString();

                UsuarioFilialPerfil.GravarDadosOrigemAcesso(idUsuarioFilialPerfil,
                    strOrigemUrlReferr,
                    strOrigemQuery,
                    strOrigemUtmSource,
                    strOrigemUtmMedium,
                    strOrigemUtmCampaign,
                    strOrigemUtmTerm,
                    strOrigemUtmKeyWords);
            }
            catch (Exception ex)
            {
                GerenciadorException.GravarExcecao(ex, "Erro no GravarOrigemAcesso");
            }
        }
        #endregion

        #region LogarAutomaticoPessoaFisica
        /// <summary>
        ///     Regras para logar uma pessoa física automáticamente
        /// </summary>
        private void LogarAutomaticoPessoaFisica(LoginAutomatico objLogin)
        {
            if (objLogin != null)
            {
                PessoaFisica objPessoaFisica;
                if (objLogin.IdPessoFisica.HasValue)
                {
                    objPessoaFisica = PessoaFisica.LoadObject(objLogin.IdPessoFisica.Value);
                    ValidarPessoaFisica(objPessoaFisica, objLogin.Url);
                }
                else
                {
                    objPessoaFisica = PessoaFisica.CarregarPorCPF(objLogin.NumeroCPF);
                    if (objPessoaFisica.DataNascimento.Equals(objLogin.DataNascimento))
                    {
                        ValidarPessoaFisica(objPessoaFisica, objLogin.Url);
                    }
                }
            }
        }

        /// <summary>
        ///     Regras para logar uma pessoa física automáticamente
        /// </summary>
        internal void LogarAutomaticoPessoaFisica(int idPessoaFisica)
        {
            PessoaFisica objPessoaFisica;
            objPessoaFisica = PessoaFisica.LoadObject(idPessoaFisica);
            ValidarPessoaFisica(objPessoaFisica, ".");
        }

        #region Regras do Login

        #region ValidarPessoaFisica
        private void ValidarPessoaFisica(PessoaFisica objPessoaFisica, string paginaParaRedirecionar)
        {
            //Se o usuário for inativo mostrar mensagem especifica.
            if (Convert.ToBoolean(objPessoaFisica.FlagInativo))
                ExibirMensagem(MensagemAviso._202406, TipoMensagem.Aviso);
            else
            {
                //Se possui perfil ativo												  
                if (UsuarioFilialPerfil.PossuiPerfilAtivo(objPessoaFisica))
                {
                    //Gravar no cookie para não aparecer mais a popup
                    //GravarCookiePreCadastro("PreCadastro", "Logado");
                    //Verifica se o usuário é interno para ser redirecionado para a tela de manutenção do sistema
                    int idUsuarioFilialPerfil, idPerfil;
                    if (PessoaFisica.VerificaPessoaFisicaUsuarioInterno(objPessoaFisica.IdPessoaFisica, out idUsuarioFilialPerfil, out idPerfil))
                        CarregarUsuarioInterno(idUsuarioFilialPerfil, idPerfil, objPessoaFisica);
                    else
                    {
                        var quantidadeEmpresa = UsuarioFilialPerfil.QuantidadeUsuarioEmpresa(objPessoaFisica);

                        if (quantidadeEmpresa != 0) //Se o usuário tiver empresa relacionada.
                            CarregarUsuarioEmpresa(quantidadeEmpresa, objPessoaFisica, paginaParaRedirecionar);
                        else
                            CarregarUsuarioCandidato(objPessoaFisica, paginaParaRedirecionar);
                    }
                }
                else
                    ExibirMensagem(MensagemAviso._202406, TipoMensagem.Aviso);
            }
        }
        #endregion

        #region CarregarUsuarioInterno
        private void CarregarUsuarioInterno(int idUsuarioFilialPerfil, int idPerfil, PessoaFisica objPessoaFisica)
        {
            //Guarda o IdUsuarioFilialPerfil e o IdPerfilSession do Usuário interno em váriavel global
            IdUsuarioFilialPerfilLogadoUsuarioInterno.Value = idUsuarioFilialPerfil;
            IdPerfil.Value = idPerfil;
            LogarPessoaFisica(objPessoaFisica);

            BNEAutenticacao.LogarCPF(objPessoaFisica.NomeCompleto, objPessoaFisica.IdPessoaFisica, objPessoaFisica.CPF, objPessoaFisica.DataNascimento);
            Redirect("~/SalaAdministrador.aspx");
        }
        #endregion

        #region CarregarUsuarioEmpresa
        /// <summary>
        ///     Inicializa em tela as configurações de um Usuário Empresa.
        /// </summary>
        /// <param name="quantidadeEmpresa">Quantidade de empresas ligadas ao CPF</param>
        /// <param name="objPessoaFisica">Pessoa Física</param>
        /// <param name="paginaParaRedirecionar">Página para redirecionar ao final do fluxo</param>
        private void CarregarUsuarioEmpresa(int quantidadeEmpresa, PessoaFisica objPessoaFisica, string paginaParaRedirecionar)
        {
            Curriculo objCurriculo;
            if (Curriculo.CarregarPorPessoaFisica(objPessoaFisica.IdPessoaFisica, out objCurriculo) && !objCurriculo.FlagInativo)
            {
                BNEAutenticacao.LogarCandidato(objPessoaFisica.NomeCompleto, objPessoaFisica.IdPessoaFisica, objPessoaFisica.CPF, objPessoaFisica.DataNascimento, objCurriculo.IdCurriculo);

                if (objCurriculo.SituacaoCurriculo.IdSituacaoCurriculo.Equals((int)Enumerador.SituacaoCurriculo.Bloqueado))
                    ExibirMensagem(MensagemAviso._103703, TipoMensagem.Erro, true);
                else
                {
                    UsuarioFilialPerfil objUsuarioFilialPerfil;
                    if (UsuarioFilialPerfil.CarregarUsuarioFilialPerfilCandidatoAtivo(objPessoaFisica, out objUsuarioFilialPerfil))
                        IdUsuarioFilialPerfilLogadoCandidato.Value = objUsuarioFilialPerfil.IdUsuarioFilialPerfil;

                    IdCurriculo.Value = objCurriculo.IdCurriculo;
                    LogarPessoaFisica(objPessoaFisica);
                }
            }
            else
            {
                BNEAutenticacao.LogarCPF(objPessoaFisica.NomeCompleto, objPessoaFisica.IdPessoaFisica, objPessoaFisica.CPF, objPessoaFisica.DataNascimento);
            }

            if (quantidadeEmpresa > 1)
            {
                Usuario objUsuario;
                if (Usuario.CarregarPorPessoaFisica(objPessoaFisica.IdPessoaFisica, out objUsuario))
                {
                    int idFilial = 0;
                    //Se é um usuário que tem uma empresa e vai cadastrar uma nova empresa, ele pode vir sem ultima filial logada
                    if (objUsuario.UltimaFilialLogada == null)
                    {
                        //Recupera o ultimo id de filial cadastrada
                        idFilial = objPessoaFisica.RecuperarUltimaFilialCadastrada();
                    }

                    //Verificar se não tem ultima filial logada e recupera a ultima filial ativa cadastrada ou verifica se o usuário filial perfil está inativo para a IDUltimaFilialLogada
                    if ((objUsuario.UltimaFilialLogada == null && idFilial > 0) || (idFilial == 0 && UsuarioFilialPerfil.VerificaUsuarioFilialPorUltimaEmpresaLogada(objPessoaFisica.IdPessoaFisica, idFilial)))
                    {
                        VerificarLoginEmpresa(objPessoaFisica.IdPessoaFisica, idFilial);

                        LogarPessoaFisica(objPessoaFisica);
                        IdFilial.Value = idFilial;

                        //Carrega o usuário filial perfil pela pessoa física e filial
                        UsuarioFilialPerfil objUsuarioFilialPerfil;
                        if (UsuarioFilialPerfil.CarregarPorPessoaFisicaFilial(objPessoaFisica.IdPessoaFisica, IdFilial.Value, out objUsuarioFilialPerfil))
                            IdUsuarioFilialPerfilLogadoEmpresa.Value = objUsuarioFilialPerfil.IdUsuarioFilialPerfil;

                        if (string.IsNullOrWhiteSpace(paginaParaRedirecionar))
                            Redirect(GetRouteUrl(Enumerador.RouteCollection.SalaSelecionador.ToString(), null));
                        else if (paginaParaRedirecionar != ".")
                            Redirect(paginaParaRedirecionar);
                    }
                    else
                    {
                        objUsuario.UltimaFilialLogada = null;
                        objUsuario.Save();
                    }
                }
            }
            else
            {
                UsuarioFilialPerfil objUsuarioFilialPerfil;
                if (UsuarioFilialPerfil.CarregarUsuarioEmpresaPorPessoaFisica(objPessoaFisica.IdPessoaFisica, out objUsuarioFilialPerfil))
                {
                    VerificarLoginEmpresa(objPessoaFisica.IdPessoaFisica, objUsuarioFilialPerfil.Filial.IdFilial);

                    LogarPessoaFisica(objPessoaFisica);
                    IdUsuarioFilialPerfilLogadoEmpresa.Value = objUsuarioFilialPerfil.IdUsuarioFilialPerfil;
                    IdFilial.Value = objUsuarioFilialPerfil.Filial.IdFilial;

                    if (string.IsNullOrWhiteSpace(paginaParaRedirecionar))
                        Redirect(GetRouteUrl(Enumerador.RouteCollection.SalaSelecionador.ToString(), null));
                    else if (paginaParaRedirecionar != ".")
                        Redirect(paginaParaRedirecionar);
                }
            }
        }
        #endregion

        #region CarregarUsuarioCandidato
        private void CarregarUsuarioCandidato(PessoaFisica objPessoaFisica, string paginaParaRedirecionar)
        {
            Curriculo objCurriculo;
            if (Curriculo.CarregarPorPessoaFisica(objPessoaFisica.IdPessoaFisica, out objCurriculo))
            {
                //Verifica se o curriculo está bloqueado
                if (objCurriculo.FlagInativo || objCurriculo.SituacaoCurriculo.IdSituacaoCurriculo.Equals((int)Enumerador.SituacaoCurriculo.Bloqueado))
                    ExibirMensagem(MensagemAviso._103703, TipoMensagem.Erro, true);
                else
                {
                    IdCurriculo.Value = objCurriculo.IdCurriculo;
                    LogarPessoaFisica(objPessoaFisica);
                    BNEAutenticacao.LogarCandidato(objPessoaFisica.NomeCompleto, objPessoaFisica.IdPessoaFisica, objPessoaFisica.CPF, objPessoaFisica.DataNascimento, objCurriculo.IdCurriculo);

                    //Atualizar curriculo no estado de invisivel para Aguardando Publicação
                    if (objCurriculo.SituacaoCurriculo.IdSituacaoCurriculo.Equals((int)Enumerador.SituacaoCurriculo.Invisivel))
                    {
                        objCurriculo.SituacaoCurriculo = new SituacaoCurriculo((int)Enumerador.SituacaoCurriculo.AguardandoPublicacao);
                        objCurriculo.Salvar();
                    }

                    //@reinaldo: Grava origem acesso independente da origem
                    UsuarioFilialPerfil objUsuarioFilialPerfil;
                    if (UsuarioFilialPerfil.CarregarUsuarioFilialPerfilCandidatoAtivo(objPessoaFisica, out objUsuarioFilialPerfil))
                    {
                        if (objUsuarioFilialPerfil != null)
                        {
                            GravarOrigemAcesso(objUsuarioFilialPerfil.IdUsuarioFilialPerfil);

                            IdUsuarioFilialPerfilLogadoCandidato.Value = objUsuarioFilialPerfil.IdUsuarioFilialPerfil;
                        }
                    }
                    //@reinaldo: fim

                    if (CurriculoOrigem.ExisteCurriculoNaOrigem(objCurriculo, new Origem(IdOrigem.Value)))
                    {
                        objCurriculo.Salvar();

                        if (objUsuarioFilialPerfil != null)
                        {
                            if (string.IsNullOrWhiteSpace(paginaParaRedirecionar))
                                Redirect(GetRouteUrl(Enumerador.RouteCollection.SalaVIP.ToString(), null));
                            else if (paginaParaRedirecionar != ".")
                                Redirect(paginaParaRedirecionar);
                        }
                        else
                            Redirect(GetRouteUrl(Enumerador.RouteCollection.CadastroCurriculoMini.ToString(), null));
                    }
                    else
                    {
                        //Se a origem do acesso for BNE, Chama salvar do cv para atualização da data de atualização do CV
                        if (IdOrigem.HasValue && IdOrigem.Value == 1)
                            objCurriculo.Salvar();

                        if (string.IsNullOrWhiteSpace(paginaParaRedirecionar))
                            Redirect(GetRouteUrl(Enumerador.RouteCollection.CadastroCurriculoMini.ToString(), null));
                        else if (paginaParaRedirecionar != ".")
                            Redirect(paginaParaRedirecionar);
                    }
                }
            }
            else //Se o usuário não existir
                Redirect(GetRouteUrl(Enumerador.RouteCollection.CadastroCurriculoMini.ToString(), null));
        }
        #endregion

        #region LogarPessoaFisica
        private void LogarPessoaFisica(PessoaFisica objPessoaFisica)
        {
            IdPessoaFisicaLogada.Value = objPessoaFisica.IdPessoaFisica;
            PageHelper.AtualizarSessaoUsuario(objPessoaFisica.IdPessoaFisica, Session.SessionID);
            GravarCookieLoginVagas(objPessoaFisica);
        }
        #endregion

        #endregion

        #endregion

        #region VerificarLoginEmpresa
        public void VerificarLoginEmpresa(int idPessoaFisica, int idFilial)
        {
            if (!PageHelper.VerificarLoginEmpresa(idPessoaFisica, idFilial))
            {
                BNEAutenticacao.DeslogarPadrao(LogoffType.EXCEEDED_USER);

                Session.Add(Chave.Temporaria.MensagemPermissao.ToString(), "Sua empresa excedeu a quantidade de usuários logados permitido.");
                Redirect("/");
            }
        }
        #endregion

        #region RecuperarDadosPesquisaCurriculo
        protected bool RecuperarDadosPesquisaCurriculo(string funcao, string cidade, string palavraChave, out BLL.PesquisaCurriculo objPesquisaCurriculo)
        {
            objPesquisaCurriculo = null;

            var principal = (Principal)Page.Master;

            if (principal != null)
                principal.RecuperarDadosPesquisaCurriculo(funcao, cidade, palavraChave, out objPesquisaCurriculo);

            return objPesquisaCurriculo != null;
        }
        #endregion

        #region RedirecionarCandidatoPesquisaVaga
        public void RedirecionarCandidatoPesquisaVaga(PessoaFisica objPessoaFisica)
        {
            Curriculo objCurriculo = null;
            if (IdCurriculo.HasValue)
                objCurriculo = new Curriculo(IdCurriculo.Value);

            UsuarioFilialPerfil objUsuarioFilialPerfil = null;
            if (IdUsuarioFilialPerfilLogadoCandidato.HasValue)
                objUsuarioFilialPerfil = new UsuarioFilialPerfil(IdUsuarioFilialPerfilLogadoCandidato.Value);

            var objPesquisaVaga = BLL.PesquisaVaga.RecuperarDadosPesquisaVagaCandidato(objPessoaFisica, objCurriculo, objUsuarioFilialPerfil, PageHelper.RecuperarIP());

            /*
            TipoGatilho.DispararGatilhoPesquisaCandidato(HttpContext.Current ?? Context, objPesquisaVaga);
            
            var url = string.Concat("http://", Helper.RecuperarURLVagas(), "/resultado-pesquisa-avancada-de-vagas/", objPesquisaVaga.IdPesquisaVaga);
            
            Redirect(url);
            */

            if (STC.HasValue && STC.Value) //Se for STC mantém o redirecionamento para o projeto antigo
            {
                Session.Add(Chave.Temporaria.Variavel6.ToString(), objPesquisaVaga.IdPesquisaVaga);
                Redirect(GetRouteUrl(Enumerador.RouteCollection.PesquisaVaga.ToString(), null));
            }
            else
            {
                TipoGatilho.DispararGatilhoPesquisaCandidato(HttpContext.Current ?? Context, objPesquisaVaga);
                var url = string.Concat("http://", Helper.RecuperarURLVagas(), "/resultado-pesquisa-avancada-de-vagas/", objPesquisaVaga.IdPesquisaVaga);
                Redirect(url);
            }
        }
        #endregion RedirecionarCandidatoPesquisaVaga

        #region GerarSEO
        private void GerarSEO()
        {
            try
            {
                var objRotaSEODefault = Rota.BuscarPorRouteName(PageRoute.Routes.Default.ToString());

                Rota objRotaSEOEspecifica = null;
                try
                {
                    if (RouteData.DataTokens["RouteName"] != null)
                    {
                        var route = PageRoute.GetPageRoute(RouteData.DataTokens["RouteName"].ToString());
                        if (route.ToString() != "Default")
                            objRotaSEOEspecifica = Rota.BuscarPorRouteName(route.ToString());
                    }
                }
                catch (Exception ex)
                {
                    GerenciadorException.GravarExcecao(ex);
                }

                string desBreadcrumb;
                string desBreadcrumbURL;

                if (objRotaSEOEspecifica != null)
                {
                    desBreadcrumb = string.IsNullOrEmpty(objRotaSEOEspecifica.DesBreadcrumb) ? objRotaSEODefault.DesBreadcrumb : objRotaSEOEspecifica.DesBreadcrumb;
                    desBreadcrumbURL = string.IsNullOrEmpty(objRotaSEOEspecifica.DesBreadcrumbURL) ? objRotaSEODefault.DesBreadcrumbURL : objRotaSEOEspecifica.DesBreadcrumbURL;
                }
                else
                {
                    desBreadcrumb = objRotaSEODefault.DesBreadcrumb;
                    desBreadcrumbURL = objRotaSEODefault.DesBreadcrumbURL;
                }

                var master = (Principal)Page.Master;
                if (master != null)
                {
                    master.DesBreadcrumb = FormataDescricoes(desBreadcrumb);
                    master.DesBreadcrumbURL = Helper.RemoverAcentos(FormataDescricoes(desBreadcrumbURL).ToLower()).Trim().Replace(" ", "-").Replace("#", "(csharp)");
                }
            }
            catch (Exception ex)
            {
                GerenciadorException.GravarExcecao(ex);
            }
        }
        #endregion GerarSEO

        #region FormataDescricoes
        public string FormataDescricoes(string descricao)
        {
            //Fazendo replace dos valores passados por parametros
            descricao = SeoParameters.Aggregate(descricao, (current, item) => current.Replace("{" + item.Key + "}", item.Value));

            if (RouteData.Values["Cidade"] != null && RouteData.Values["SiglaEstado"] != null)
            {
                var cidadeEstado = RouteData.Values["Cidade"].ToString().Replace("-", " ") + "/" + RouteData.Values["SiglaEstado"];

                Cidade objCidade;
                Cidade.CarregarPorNome(cidadeEstado, out objCidade);
                if (objCidade != null)
                {
                    if (objCidade.NomeCidade.Contains("/"))
                    {
                        var arrayCidadeEstado = objCidade.NomeCidade.Split('/');

                        descricao = descricao.Replace("{Cidade}", arrayCidadeEstado[0].Trim());
                        descricao = descricao.Replace("{SiglaEstado}", arrayCidadeEstado[1].Trim());
                    }
                    else
                    {
                        descricao = descricao.Replace("{Cidade}", objCidade.NomeCidade);
                        descricao = descricao.Replace("{SiglaEstado}", objCidade.Estado.SiglaEstado);
                    }
                }
                else
                {
                    descricao = descricao.Replace("{Cidade}", RouteData.Values["Cidade"].ToString().Replace("-", " "));
                    descricao = descricao.Replace("{SiglaEstado}", RouteData.Values["SiglaEstado"].ToString().ToUpper());
                }
            }

            if (RouteData.Values["Funcao"] != null && descricao.Contains("{Funcao}"))
            {
                var objFuncao = Funcao.CarregarPorDescricao(RouteData.Values["Funcao"].ToString().Replace("-", " ").Replace("(csharp)", "#"));
                if (objFuncao != null)
                    descricao = descricao.Replace("{Funcao}", objFuncao.DescricaoFuncao);
                else
                    descricao = descricao.Replace("{Funcao}", RouteData.Values["Funcao"].ToString().Replace("-", " ").Replace("(csharp)", "#"));

                descricao = descricao.Replace("{Funcao}", RouteData.Values["Funcao"].ToString().Replace("-", " ").Replace("(csharp)", "#"));
            }

            return descricao;
        }
        #endregion FormataDescricoes

        #endregion

        #region Eventos

        #region Page
        protected override void OnLoad(EventArgs e)
        {
            RegistrarTipoAjax();

            if (!Page.IsPostBack)
            {
                UIHelper.LimparSession(Session, ViewState, typeof(Chave.Temporaria));

                if (User == null ||
                    User.Identity == null ||
                    !User.Identity.IsAuthenticated)
                {
                    LogarAutomatico();
                }
                else if (RouteData.Values.Count > 0 && RouteData.Values["HashAcesso"] != null) //Recupera a hash de acesso
                {
                    LogarAutomatico();
                }

                if (IdPessoaFisicaLogada.HasValue)
                {
                    if (PageHelper.ValidarUsuarioLogado(IdPessoaFisicaLogada.Value, Session.SessionID) == false)
                    {
                        if (GetType().BaseType != typeof(Default))
                        {
                            BNEAutenticacao.DeslogarPadrao(LogoffType.OVERRIDDEN_SESSION);

                            Session[Chave.Redirecionamento.MensagemPermissaoRedirecionamento.ToString()] = "Dados utilizados em outro computador, seu acesso foi encerrado.";
                            Redirect("~/");
                        }
                    }
                    else
                    {
                        new PessoaFisica(IdPessoaFisicaLogada.Value).AtualizarDataInteracaoUsuario();
                    }
                }

                DefinirTituloPagina();

                GerarSEO();
            }

            InicializarBarraBusca(TipoBuscaMaster.Vaga, true, GetType().ToString());

            base.OnLoad(e);
        }
        #endregion

        #region InitializeCulture
        protected override void InitializeCulture()
        {
            Page.Culture = "pt-BR";
            Page.UICulture = "pt-BR";
        }
        #endregion

        #region OnPreInit
        protected override void OnPreInit(EventArgs e)
        {
            if (Tema.HasValue && (Page.Theme == null))
                Page.Theme = Tema.Value;

            base.OnPreInit(e);
        }
        #endregion

        #region RecuperarUrlIconeFacebook
        public string RecuperarUrlIconeFacebook()
        {
            var url = "/img/bne/logo_bne_facebook.png";

            if (STC.HasValue && STC.Value)
            {
                url = "/img/bne/logo_vaga_facebook.png";
            }

            return string.Concat("http://", UIHelper.RecuperarURLAmbiente(), url);
        }
        #endregion RecuperarUrlIconeFacebook

        #region MostrarNomeCandidatoEEmpresaNaExperienciaProfissional
        public bool MostrarNomeCandidatoEEmpresaNaExperienciaProfissional()
        {
            if (IdUsuarioFilialPerfilLogadoEmpresa.HasValue || IdUsuarioFilialPerfilLogadoUsuarioInterno.HasValue)
            {
                if ((IdUsuarioFilialPerfilLogadoEmpresa.HasValue && new Filial(IdFilial.Value).PossuiPlanoAtivo()) || IdUsuarioFilialPerfilLogadoUsuarioInterno.HasValue)
                    return true;
            }

            return false;
        }
        #endregion

        #endregion
    }
}