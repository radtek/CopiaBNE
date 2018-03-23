using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using BNE.BLL;
using BNE.BLL.Custom;
using BNE.Common.Session;
using BNE.Web.Code.Enumeradores;
using BNE.Web.Master;
using Employer.Componentes.UI.Web;
using Resources;
using Enumerador = BNE.BLL.Enumeradores;
using Microsoft.IdentityModel.Claims;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Principal;

namespace BNE.Web.Code
{
    public class BasePage : Page
    {

        #region Session

        public SessionVariable<int> IdCurriculo = new SessionVariable<int>(Chave.Permanente.IdCurriculo.ToString());
        public SessionVariable<int> IdPessoaFisicaLogada = new SessionVariable<int>(Chave.Permanente.IdPessoaFisicaLogada.ToString());
        public SessionVariable<int> IdVaga = new SessionVariable<int>(Chave.Permanente.IdVaga.ToString());
        public SessionVariable<int> IdUsuarioLogado = new SessionVariable<int>(Chave.Permanente.IdUsuarioLogado.ToString());
        public SessionVariable<int> IdUsuarioFilialPerfilLogadoCandidato = new SessionVariable<int>(Chave.Permanente.IdUsuarioFilialPerfilLogadoCandidato.ToString());
        public SessionVariable<int> IdUsuarioFilialPerfilLogadoUsuarioInterno = new SessionVariable<int>(Chave.Permanente.IdUsuarioFilialPerfilLogadoUsuarioInterno.ToString());
        public SessionVariable<int> IdUsuarioFilialPerfilLogadoEmpresa = new SessionVariable<int>(Chave.Permanente.IdUsuarioFilialPerfilLogadoEmpresa.ToString());
        public SessionVariable<int> IdPerfil = new SessionVariable<int>(Chave.Permanente.IdPerfil.ToString());
        public SessionVariable<int> IdFilial = new SessionVariable<int>(Chave.Permanente.IdFilial.ToString());
        public SessionVariable<string> Tema = new SessionVariable<string>(Chave.Permanente.Theme.ToString());
        public SessionVariable<string> UrlDestinoPagamento = new SessionVariable<string>(Chave.Permanente.UrlDestinoPagamento.ToString());
        public SessionVariable<string> UrlDestino = new SessionVariable<string>(Chave.Permanente.UrlDestino.ToString());

        public SessionVariable<TipoBuscaMaster> TipoBusca = new SessionVariable<TipoBuscaMaster>(Chave.Permanente.TipoBuscaMaster.ToString());
        public SessionVariable<string> FuncaoMaster = new SessionVariable<string>(Chave.Permanente.FuncaoMaster.ToString());
        public SessionVariable<string> CidadeMaster = new SessionVariable<string>(Chave.Permanente.CidadeMaster.ToString());
        public SessionVariable<string> PalavraChaveMaster = new SessionVariable<string>(Chave.Permanente.PalavraChaveMaster.ToString());

        public SessionVariable<bool> STC = new SessionVariable<bool>(Chave.Permanente.STC.ToString());
        public SessionVariable<int> IdOrigem = new SessionVariable<int>(Chave.Permanente.IdOrigem.ToString());

        //Pagamento
        public SessionVariable<int> PagamentoIdentificadorPagamento = new SessionVariable<int>(Chave.Permanente.PagamentoIdentificadorPagamento.ToString());
        public SessionVariable<int> PagamentoIdentificadorPlano = new SessionVariable<int>(Chave.Permanente.PagamentoIdentificadorPlano.ToString());
        public SessionVariable<int> PagamentoIdentificadorPlanoAdquirido = new SessionVariable<int>(Chave.Permanente.PagamentoIdentificadorPlanoAdquirido.ToString());

        #region Adicional

        #region PagamentoAdicionalValorTotal
        /// <summary>
        /// Utilizado quando não é a compra de um plano, mas de um adicional de plano.
        /// </summary>
        public SessionVariable<decimal> PagamentoAdicionalValorTotal = new SessionVariable<decimal>(Chave.Permanente.PagamentoAdicionalValorTotal.ToString());
        #endregion

        #region PagamentoAdicionalQuantidade
        /// <summary>
        /// Utilizado quando não é a compra de um plano, mas de um adicional de plano.
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

        #region LimparSession
        /// <summary>
        /// Método responsável por limpar os valores necessarios na identificação de um usuário.
        /// </summary>
        /// 
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

            var context = HttpContext.Current != null ? HttpContext.Current : Context;

            //não funciona
            //var saveState = new List<KeyValuePair<string, object>>();
            //foreach (var item in Session.Keys)
            //{
            //    var key = (string)item;
            //    saveState.Add(new KeyValuePair<string, object>(key, Session[key]));
            //}
            BNE.Auth.SessionAbandonRestoreMediator.Instance.RaiseSessionCloseManually(new HttpContextWrapper(context).Session);
            context.Session.Abandon();

            //não funciona
            //foreach (var pair in saveState) // mantém outras informações
            //{
            //    Session[pair.Key] = pair.Value;
            //}
        }
        #endregion

        #region LimparSessionPagamento
        /// <summary>
        /// Limpa as propriedades relacionado a compra de planos armazenadas na sessão do usuário
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
        /// Método responsável por limpar os valores necessarios na identificação de um usuário.
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
                return;
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
                var tempo = Convert.ToInt32(Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.CookieAcessoHorasExpiracao));

                c = new HttpCookie("TelefoneChameFacil")
                {
                    Expires = DateTime.Now.AddHours(tempo),
                };
            }

            return c;
        }
        #endregion

        #region GravarCookieTelefoneChameFacil
        public void GravarCookieTelefoneChameFacil(String telefone)
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

        #region RecuperarCookieChameFacil
        public HttpCookie RecuperarCookieChameFacil()
        {
            HttpCookie c;

            if (Request.Cookies["ChameFacil"] != null)
                c = Request.Cookies["ChameFacil"];
            else
            {
                var tempo = Convert.ToInt32(Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.CookieAcessoHorasExpiracao));

                c = new HttpCookie("ChameFacil")
                {
                    Expires = DateTime.Now.AddHours(tempo)
                };
            }

            return c;
        }
        #endregion

        #region GravarCookieChameFacil
        public void GravarCookieChameFacil(String key, String value)
        {
            if (string.IsNullOrEmpty(key) == false)
            {
                var cookie = RecuperarCookieChameFacil();
                cookie.Values[key] = HttpUtility.UrlEncode(value);
                Response.Cookies.Add(cookie);
            }
        }
        #endregion

        #region DefinirTituloPagina
        public OrigemFilial DefinirTituloPagina()
        {
            //Pega o caminho da página que está sendo iniciada, e substitui .aspx por vazio
            string pagina = Path.GetFileNameWithoutExtension(Page.AppRelativeVirtualPath);

            if (string.IsNullOrWhiteSpace(pagina))
                return null;

            string caminho = Path.GetFileNameWithoutExtension(Page.Request.Url.AbsolutePath);
            if (string.IsNullOrWhiteSpace(caminho))
                return null;

            caminho = caminho.Replace('-', '_');

            //Instancia um ResourceManager para trabalhar com o arquivo de títulos
            var rmTit = new ResourceManager("BNE.Web.Resources.TituloPagina", Assembly.GetExecutingAssembly());
            var rmDesc = new ResourceManager("BNE.Web.Resources.DescricaoPagina",
                Assembly.GetExecutingAssembly());
            var rmKey = new ResourceManager("BNE.Web.Resources.PalavraChave", Assembly.GetExecutingAssembly());

            //Recupera o título da tela
            string strTituloTela = rmTit.GetString(pagina.ToUpper()) ??
                                   rmTit.GetString(string.Format("{0}_{1}", pagina.ToUpper(), caminho.ToUpper()));
            string strDescricaoTela = rmDesc.GetString(pagina.ToUpper()) ??
                                      rmDesc.GetString(string.Format("{0}_{1}", pagina.ToUpper(),
                                          caminho.ToUpper()));
            string strPalavraChaveTela = rmKey.GetString(pagina.ToUpper()) ??
                                         rmKey.GetString(string.Format("{0}_{1}", pagina.ToUpper(),
                                             caminho.ToUpper()));
            string strNomeProjeto = rmTit.GetString("TITULOPADRAOPROJETO");

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

            Page.Title = string.Format("{0} - {1}", strNomeProjeto, strTituloTela);

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
        public virtual void InicializarBarraBusca(TipoBuscaMaster tipoBuscaMaster, bool expandida, string strKey)
        {
            var principal = (Principal)Page.Master;
            if (principal != null)
                principal.InicializarBarraBusca(tipoBuscaMaster, expandida, strKey);
        }
        #endregion

        #region RegistrarTipoAjax
        private void RegistrarTipoAjax()
        {
            //Registra o tipo para funcionar com Ajax
            Ajax.Utility.RegisterTypeForAjax(Page.GetType().BaseType);
        }
        #endregion

        #region DeletarArquivosTemporarios
        public void DeletarArquivosTemporarios()
        {
            try
            {
                var pasta = new DirectoryInfo(Server.MapPath(Resources.Configuracao.PastaArquivoTemporario));
                FileInfo[] files = pasta.GetFiles();
                foreach (FileInfo t in files)
                {
                    DateTime ultimoAcesso = t.LastAccessTime;
                    if (ultimoAcesso < DateTime.Now.AddMinutes(-10))
                        t.Delete();
                }
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
            }
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
        /// Passa a string para gerar o arquivo para a tela de geração de arquivo (DownloadArquivo.aspx)
        /// </summary>
        /// <param name="arrayFinal">Byte array de dados que devem estar no arquivo</param>
        /// <param name="nomeArquivo">Nome do arquivo a ser gerado</param>
        public string GerarLinkDownload(byte[] arrayFinal, string nomeArquivo)
        {
            string diretorioArquivo = String.Format("{0}{1}_{2}", Resources.Configuracao.PastaArquivoTemporario, DateTime.Now.Ticks, Session.SessionID);
            string diretorioArquivoMapeado = Server.MapPath(diretorioArquivo);

            if (!Directory.Exists(diretorioArquivoMapeado))
                Directory.CreateDirectory(diretorioArquivoMapeado);

            string caminhoArquivo = String.Format("{0}/{1}", diretorioArquivo, nomeArquivo);
            string caminhoArquivoMapeado = String.Format("{0}/{1}", diretorioArquivoMapeado, nomeArquivo);

            FileStream fs = null;
            try
            {
                fs = new FileStream(caminhoArquivoMapeado, FileMode.CreateNew, FileAccess.Write);
                fs.Write(arrayFinal, 0, arrayFinal.Length);
                fs.Close();
            }
            finally
            {
                if (fs != null)
                    fs.Dispose();
            }

            return caminhoArquivo;
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
                EL.GerenciadorException.GravarExcecao(ex, out message, customMessage);
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
                    EL.GerenciadorException.GravarExcecao(ex);
            }
        }
        #endregion

        #region FormatarCPF
        protected string FormatarCPF(string cpf)
        {
            return UIHelper.FormatarCPF(cpf);
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
        /// Processo para logar o usuário automaticamente
        /// 1 - Ver se tem hash de acesso na url
        /// 2 - Tenta recuperar do cookie
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
                if (!String.IsNullOrEmpty(Request.QueryString.ToString()))
                {
                    objLoginAutomatico.Url += "?" + Request.QueryString.ToString();
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

        #region LogarAutomaticoPessoaFisica
        /// <summary>
        /// Regras para logar uma pessoa física automáticamente
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
                        ValidarPessoaFisica(objPessoaFisica, objLogin.Url);
                }
            }
        }

        /// <summary>
        /// Regras para logar uma pessoa física automáticamente
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
                UsuarioFilialPerfil objPerfisPessoa;
                if (UsuarioFilialPerfil.CarregarPorPessoaFisica(objPessoaFisica.IdPessoaFisica, out objPerfisPessoa))
                {
                    //Verifica se o usuário é interno para ser redirecionado para a tela de manutenção do sistema
                    int idUsuarioFilialPerfil, idPerfil;
                    if (PessoaFisica.VerificaPessoaFisicaUsuarioInterno(objPessoaFisica.IdPessoaFisica, out idUsuarioFilialPerfil, out idPerfil))
                    {
                        CarregarUsuarioInterno(idUsuarioFilialPerfil, idPerfil, objPessoaFisica);
                    }
                    else
                    {
                        int quantidadeEmpresa = UsuarioFilialPerfil.QuantidadeUsuarioEmpresa(objPessoaFisica.IdPessoaFisica);

                        if (quantidadeEmpresa != 0) //Se o usuário tiver empresa relacionada.
                        {
                            CarregarUsuarioEmpresa(quantidadeEmpresa, objPessoaFisica, paginaParaRedirecionar);
                        }
                        else
                        {
                            CarregarUsuarioCandidato(objPessoaFisica, paginaParaRedirecionar);
                        }
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

            BNE.Auth.BNEAutenticacao.LogarCPF(objPessoaFisica.NomeCompleto, objPessoaFisica.IdPessoaFisica, objPessoaFisica.CPF);
            Redirect("~/SalaAdministrador.aspx");
        }
        #endregion

        #region CarregarUsuarioEmpresa
        /// <summary>
        /// Inicializa em tela as configurações de um Usuário Empresa.
        /// </summary>
        /// <param name="quantidadeEmpresa">Quantidade de empresas ligadas ao CPF</param>
        /// <param name="objPessoaFisica">Pessoa Física</param>
        private void CarregarUsuarioEmpresa(int quantidadeEmpresa, PessoaFisica objPessoaFisica, string paginaParaRedirecionar)
        {
            Curriculo objCurriculo;
            if (Curriculo.CarregarPorPessoaFisica(objPessoaFisica.IdPessoaFisica, out objCurriculo) && !objCurriculo.FlagInativo)
            {
                BNE.Auth.BNEAutenticacao.LogarCandidato(objPessoaFisica.NomeCompleto, objPessoaFisica.IdPessoaFisica, objPessoaFisica.CPF, objCurriculo.IdCurriculo);

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
                BNE.Auth.BNEAutenticacao.LogarCPF(objPessoaFisica.NomeCompleto, objPessoaFisica.IdPessoaFisica, objPessoaFisica.CPF);
            }

            if (quantidadeEmpresa > 1)
            {
                Usuario objUsuario;
                if (Usuario.CarregarPorPessoaFisica(objPessoaFisica.IdPessoaFisica, out objUsuario))
                {
                    if (objUsuario.UltimaFilialLogada != null)
                    {
                        //Verifica se o usuário filial perfil está inativo para a IDUltimaFilialLogada
                        if (UsuarioFilialPerfil.VerificaUsuarioFilialPorUltimaEmpresaLogada(objPessoaFisica.IdPessoaFisica, objUsuario.UltimaFilialLogada.IdFilial))
                        {
                            VerificarLoginEmpresa(objPessoaFisica.IdPessoaFisica, objUsuario.UltimaFilialLogada.IdFilial);

                            LogarPessoaFisica(objPessoaFisica);
                            IdFilial.Value = objUsuario.UltimaFilialLogada.IdFilial;

                            //Carrega o usuário filial perfil pela pessoa física e filial
                            UsuarioFilialPerfil objUsuarioFilialPerfil;
                            if (UsuarioFilialPerfil.CarregarPorPessoaFisicaFilial(objPessoaFisica.IdPessoaFisica, IdFilial.Value, out objUsuarioFilialPerfil))
                                IdUsuarioFilialPerfilLogadoEmpresa.Value = objUsuarioFilialPerfil.IdUsuarioFilialPerfil;

                            if (string.IsNullOrWhiteSpace(paginaParaRedirecionar))
                                Redirect("~/SalaSelecionador.aspx");
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
                        Redirect("~/SalaSelecionador.aspx");
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
                    BNE.Auth.BNEAutenticacao.LogarCandidato(objPessoaFisica.NomeCompleto, objPessoaFisica.IdPessoaFisica, objPessoaFisica.CPF, objCurriculo.IdCurriculo);

                    if (CurriculoOrigem.ExisteCurriculoNaOrigem(objCurriculo, new Origem(IdOrigem.Value)))
                    {
                        objCurriculo.Salvar();

                        //Guardar IdUsuarioFilialPerfil em variável global conforme requisito
                        UsuarioFilialPerfil objUsuarioFilialPerfil;
                        if (UsuarioFilialPerfil.CarregarPorPessoaFisica(objPessoaFisica.IdPessoaFisica, out objUsuarioFilialPerfil))
                        {
                            IdUsuarioFilialPerfilLogadoCandidato.Value = objUsuarioFilialPerfil.IdUsuarioFilialPerfil;
                            if (string.IsNullOrWhiteSpace(paginaParaRedirecionar))
                                Redirect("~/SalaVip.aspx");
                            else if (paginaParaRedirecionar != ".")
                                Redirect(paginaParaRedirecionar);
                        }
                        else
                            Redirect(GetRouteUrl(Enumerador.RouteCollection.CadastroCurriculoMini.ToString(), null));
                    }
                    else
                    {
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
                BNE.Auth.BNEAutenticacao.DeslogarPadrao(Auth.LogoffType.EXCEEDED_USER);

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
                Redirect(GetRouteUrl(BLL.Enumeradores.RouteCollection.PesquisaVaga.ToString(), null));
            }
            else
            {
                TipoGatilho.DispararGatilhoPesquisaCandidato(HttpContext.Current ?? Context, objPesquisaVaga);
                var url = string.Concat("http://", Helper.RecuperarURLVagas(), "/resultado-pesquisa-avancada-de-vagas/", objPesquisaVaga.IdPesquisaVaga);
                Redirect(url);
            }
        }
        #endregion RedirecionarCandidatoPesquisaVaga

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
                            BNE.Auth.BNEAutenticacao.DeslogarPadrao(Auth.LogoffType.OVERRIDDEN_SESSION);

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
            }

            InicializarBarraBusca(TipoBuscaMaster.Vaga, true, this.GetType().ToString());

            base.OnLoad(e);
        }

        private bool LogarAutomaticoViaIdentity(IIdentity principal)
        {
            var claims = (principal is ClaimsIdentity) ? (((ClaimsIdentity)principal).Claims.AsEnumerable() ?? new Claim[0]) : new Claim[0];

            var cpfClaim = claims.FirstOrDefault(a => a.ClaimType == BNE.Auth.BNEClaimTypes.CPF);

            decimal cpfValue;
            if (cpfClaim != null && decimal.TryParse(cpfClaim.Value, out cpfValue))
            {
                var pfId = PessoaFisica.CarregarIdPorCPF(cpfValue);
                if (pfId > 0)
                {
                    LogarAutomaticoPessoaFisica(pfId);
                    return true;
                }
            }
            else
            {
                var idClaim = claims.FirstOrDefault(a => a.ClaimType == BNE.Auth.BNEClaimTypes.PessoaFisicaId);

                int idValue;
                if (idClaim != null && Int32.TryParse(idClaim.Value, out idValue))
                {
                    LogarAutomaticoPessoaFisica(idValue);
                    return true;
                }
            }

            return false;
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

        #endregion

    }
}
