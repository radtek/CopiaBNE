using System;
using System.IO;
using System.Threading;
using System.Web;
using System.Web.UI;
using BNE.BLL;
using BNE.Common.Session;
using BNE.Web.Code.Enumeradores;
using BNE.Web.Master;

namespace BNE.Web.Code
{
    public class BaseUserControl : UserControl
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
        public SessionVariable<string> PagamentoUrlRetorno = new SessionVariable<string>(Chave.Permanente.PagamentoUrlRetorno.ToString());

        #region LimparSession
        /// <summary>
        /// Método responsável por limpar os valores necessarios na identificação de um usuário.
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
            var principal = Page.Master as Principal;
            if (principal != null)
                principal.ExibirMensagem(mensagem, tipo, aumentarTamanhoPainelMensagem);
            else
            {
                // a pagina atual pode ser a de Visualizacao de Curriculo, que tem barra de mensagem própria
                var visualizacaoCurriculo = Page as VisualizacaoCurriculo;
                if (visualizacaoCurriculo != null)
                    visualizacaoCurriculo.ExibirMensagem(mensagem, tipo, aumentarTamanhoPainelMensagem);
                else
                {
                    // a pagina atual pode ser a de Login.aspx, que tem barra de mensagem própria
                    var lPage = Page as Login;
                    if (lPage != null)
                        lPage.ExibirMensagem(mensagem, tipo, aumentarTamanhoPainelMensagem);
                }
            }
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

        #region SetarFoco
        protected void SetarFoco(Control controle)
        {
            var principal = (Principal)Page.Master;
            if (principal != null) principal.SetarFoco(controle);
        }
        #endregion

        #region ExibirMensagemConfirmacao
        protected void ExibirMensagemConfirmacao(string titulo, string mensagem, bool cliqueAqui)
        {
            var principal = (Principal)Page.Master;
            if (principal != null) principal.ExibirMensagemConfirmacao(titulo, mensagem, cliqueAqui);
        }
        #endregion

        #region ExibirMensagemErro
        public virtual void ExibirMensagemErro(Exception ex)
        {
            ExibirMensagemErro(ex, null);
        }
        public virtual void ExibirMensagemErro(Exception ex, string customMessage)
        {
            string message;
            var guidErro = EL.GerenciadorException.GravarExcecao(ex, out message, customMessage);

            if (string.IsNullOrEmpty(message))
                return;

            ExibirMensagem(string.Format("{0} - {1}", guidErro, string.IsNullOrEmpty(customMessage) ? message : customMessage), TipoMensagem.Erro);
        }
        #endregion

        #region AjustarLogin
        protected void AjustarLogin()
        {
            var principal = (Principal)Page.Master;
            if (principal != null) principal.AjustarLogin();
        }
        #endregion

        #region InicializarBarraBusca
        public void InicializarBarraBusca(TipoBuscaMaster tipoBuscaMaster, bool expandida, string strKey)
        {
            var principal = (Principal)Page.Master;
            if (principal != null) principal.InicializarBarraBusca(tipoBuscaMaster, expandida, strKey);
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
            string caminhoArquivo;
            string caminhoArquivoMapeado;

            GerarLinkDownload(nomeArquivo, out caminhoArquivo, out caminhoArquivoMapeado);

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

        public void GerarLinkDownload(string nomeArquivo, out string caminhoArquivo, out string caminhoArquivoMapeado)
        {
            string diretorioArquivo = String.Format("{0}{1}_{2}", Resources.Configuracao.PastaArquivoTemporario, DateTime.Now.Ticks, Session.SessionID);
            string diretorioArquivoMapeado = Server.MapPath(diretorioArquivo);

            if (!Directory.Exists(diretorioArquivoMapeado))
                Directory.CreateDirectory(diretorioArquivoMapeado);

            caminhoArquivo = String.Format("{0}/{1}", diretorioArquivo, nomeArquivo);
            caminhoArquivoMapeado = String.Format("{0}\\{1}", diretorioArquivoMapeado, nomeArquivo);
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
        /// <summary>
        /// Método utilizado nas grids para formatar o CPF
        /// </summary>
        /// <param name="cpf"></param>
        /// <returns></returns>
        protected string FormatarCPF(string cpf)
        {
            return UIHelper.FormatarCPF(cpf);
        }
        #endregion

        #region RetornarStatus
        protected string RetornarStatus(string inativo)
        {
            return inativo.Equals("False") ? "Ativo" : "Inativo";
        }
        #endregion

        #region VerificarLoginEmpresa
        public void VerificarLoginEmpresa(int idPessoaFisica, int idFilial)
        {
            if (!PageHelper.VerificarLoginEmpresa(idPessoaFisica, idFilial))
            {
                LimparSession();

                Session.Add(Chave.Temporaria.MensagemPermissao.ToString(), "Sua empresa excedeu a quantidade de usuários logados permitido.");
                Redirect("/");
            }
        }
        #endregion

        #region RecuperarCookieAcesso
        public HttpCookie RecuperarCookieAcesso()
        {
            var principal = (Principal)Page.Master;
            if (principal != null)
                return principal.RecuperarCookieAcesso();

            return null;
        }
        #endregion

        #region GravarCookieAcesso
        public void GravarCookieAcesso(PessoaFisica objPessoaFisica)
        {
            var principal = (Principal)Page.Master;
            if (principal != null)
                principal.GravarCookieAcesso(objPessoaFisica);
        }
        #endregion

        #region GravarCookieLoginVagas
        public void GravarCookieLoginVagas(PessoaFisica objPessoaFisica)
        {
            ((BasePage)this.Page).GravarCookieLoginVagas(objPessoaFisica);
        }
        #endregion

        #region CookieChameFacil
        private HttpCookie RecuperarCookieChameFacil()
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
        public void GravarCookieChameFacil(String key, String value)
        {
            if (string.IsNullOrEmpty(key) == false)
            {
                var cookie = RecuperarCookieChameFacil();
                cookie.Values[key] = HttpUtility.UrlEncode(value);
                Response.Cookies.Add(cookie);
            }
        }
        public string RecuperarCookieChameFacil(String key)
        {
            var cookie = RecuperarCookieChameFacil();
            if (cookie.Values[key] != null)
                return HttpUtility.UrlDecode(cookie.Values[key]);

            return string.Empty;
        }
        #endregion

        #endregion

    }
}
