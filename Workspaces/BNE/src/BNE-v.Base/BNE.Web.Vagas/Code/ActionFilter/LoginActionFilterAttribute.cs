using System;
using System.Web;
using System.Web.Mvc;
using BNE.BLL;
using BNE.BLL.Custom;
using BNE.Common.Enumeradores;
using BNE.Common.Session;
using Parametro = BNE.BLL.Parametro;
using System.Threading;
using System.Threading.Tasks;

namespace BNE.Web.Vagas.Code.ActionFilter
{

    [Obsolete("UtilizarHttpModules")]
    public class LoginActionFilterAttribute : ActionFilterAttribute, System.Web.Http.Filters.IActionFilter
    {
        public SessionVariable<int> IdCurriculo = new SessionVariable<int>(Chave.Permanente.IdCurriculo.ToString());
        public SessionVariable<int> IdPessoaFisicaLogada = new SessionVariable<int>(Chave.Permanente.IdPessoaFisicaLogada.ToString());
        public SessionVariable<int> IdUsuarioFilialPerfilLogadoCandidato = new SessionVariable<int>(Chave.Permanente.IdUsuarioFilialPerfilLogadoCandidato.ToString());
        public SessionVariable<int> IdUsuarioFilialPerfilLogadoUsuarioInterno = new SessionVariable<int>(Chave.Permanente.IdUsuarioFilialPerfilLogadoUsuarioInterno.ToString());
        public SessionVariable<int> IdUsuarioFilialPerfilLogadoEmpresa = new SessionVariable<int>(Chave.Permanente.IdUsuarioFilialPerfilLogadoEmpresa.ToString());
        public SessionVariable<int> IdFilial = new SessionVariable<int>(Chave.Permanente.IdFilial.ToString());
        public SessionVariable<int> IdPerfil = new SessionVariable<int>(Chave.Permanente.IdPerfil.ToString());
        public SessionVariable<int> IdOrigem = new SessionVariable<int>(Chave.Permanente.IdOrigem.ToString());

        /// <summary>
        /// To USE with ASP.NET Web API
        /// </summary>
        /// <param name="actionContext"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="continuation"></param>
        /// <returns></returns>
        public Task<System.Net.Http.HttpResponseMessage> ExecuteActionFilterAsync(System.Web.Http.Controllers.HttpActionContext actionContext, CancellationToken cancellationToken, Func<Task<System.Net.Http.HttpResponseMessage>> continuation)
        {
            LogarCandidato(new HttpContextWrapper(HttpContext.Current));
            return continuation();
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            LogarCandidato(context.HttpContext);
        }

        private void LogarCandidato(HttpContextBase context)
        {
            if (context.User != null
                && context.User.Identity != null
                && context.User.Identity.IsAuthenticated)
                return;

            var cookie = RecuperarCookieLoginVagas(context);
            if (cookie != null)
            {
                var hashAcesso = cookie["HashAcesso"];

                if (string.IsNullOrWhiteSpace(hashAcesso))
                    hashAcesso = RecuperarHashAcessoREST(context);

                var objLoginAutomatico = LoginAutomatico.RecuperarInformacaoHash(hashAcesso);
                if (objLoginAutomatico != null)
                {
                    var objPessoaFisica = PessoaFisica.CarregarPorCPF(objLoginAutomatico.NumeroCPF);
                    if (objPessoaFisica.DataNascimento.Equals(objLoginAutomatico.DataNascimento))
                        ValidarPessoaFisica(objPessoaFisica, objLoginAutomatico.Url);
                }

                LimparCookieLoginVagas(context);
            }
        }

        private string RecuperarHashAcessoREST(HttpContextBase context)
        {
            var hashLogin = context.Request.QueryString["HashAcesso"];
            return hashLogin;
        }

        #region RecuperarCookieLoginVagas
        public HttpCookie RecuperarCookieLoginVagas(HttpContextBase context)
        {
            const string cookie = "BNE";
            HttpCookie c;

            if (context.Request.Cookies[cookie] != null)
                c = context.Request.Cookies[cookie];
            else
            {
                var tempo = Convert.ToInt32(Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.CookieAcessoHorasExpiracao));

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

        #region ValidarPessoaFisica
        private void ValidarPessoaFisica(PessoaFisica objPessoaFisica, string paginaParaRedirecionar)
        {
            if (objPessoaFisica.FlagInativo.HasValue && objPessoaFisica.FlagInativo.Value)
                return;

            UsuarioFilialPerfil objPerfisPessoa;
            if (!UsuarioFilialPerfil.CarregarPorPessoaFisica(objPessoaFisica.IdPessoaFisica, out objPerfisPessoa))
                return;

            //Verifica se o usuário é interno para ser redirecionado para a tela de manutenção do sistema
            int idUsuarioFilialPerfil, idPerfil;
            if (PessoaFisica.VerificaPessoaFisicaUsuarioInterno(objPessoaFisica.IdPessoaFisica, out idUsuarioFilialPerfil, out idPerfil))
            {
                CarregarUsuarioInterno(idUsuarioFilialPerfil, idPerfil, objPessoaFisica);
                return;
            }

            int quantidadeEmpresa = UsuarioFilialPerfil.QuantidadeUsuarioEmpresa(objPessoaFisica.IdPessoaFisica);
            if (quantidadeEmpresa != 0) //Se o usuário tiver empresa relacionada.
            {
                CarregarUsuarioEmpresa(quantidadeEmpresa, objPessoaFisica);
            }
            else
            {
                CarregarUsuarioCandidato(objPessoaFisica);
            }
        }
        #endregion

        #region CarregarUsuarioInterno
        private void CarregarUsuarioInterno(int idUsuarioFilialPerfil, int idPerfil, PessoaFisica objPessoaFisica)
        {
            //Guarda o IdUsuarioFilialPerfil e o IdPerfilSession do Usuário interno em váriavel global
            IdUsuarioFilialPerfilLogadoUsuarioInterno.Value = idUsuarioFilialPerfil;
            IdPerfil.Value = idPerfil;
            IdPessoaFisicaLogada.Value = objPessoaFisica.IdPessoaFisica;

            BNE.Auth.BNEAutenticacao.LogarCPF(objPessoaFisica.NomeCompleto, objPessoaFisica.IdPessoaFisica, objPessoaFisica.CPF);
        }
        #endregion

        #region CarregarUsuarioEmpresa
        /// <summary>
        /// Inicializa em tela as configurações de um Usuário Empresa.
        /// </summary>
        /// <param name="quantidadeEmpresa">Quantidade de empresas ligadas ao CPF</param>
        /// <param name="objPessoaFisica">Pessoa Física</param>
        private void CarregarUsuarioEmpresa(int quantidadeEmpresa, PessoaFisica objPessoaFisica)
        {
            Curriculo objCurriculo;
            if (Curriculo.CarregarPorPessoaFisica(objPessoaFisica.IdPessoaFisica, out objCurriculo) && !objCurriculo.FlagInativo && objCurriculo.SituacaoCurriculo.IdSituacaoCurriculo != (int)BLL.Enumeradores.SituacaoCurriculo.Bloqueado)
            {
                UsuarioFilialPerfil objUsuarioFilialPerfil;
                if (UsuarioFilialPerfil.CarregarUsuarioFilialPerfilCandidatoAtivo(objPessoaFisica, out objUsuarioFilialPerfil))
                    IdUsuarioFilialPerfilLogadoCandidato.Value = objUsuarioFilialPerfil.IdUsuarioFilialPerfil;

                IdCurriculo.Value = objCurriculo.IdCurriculo;
                IdPessoaFisicaLogada.Value = objPessoaFisica.IdPessoaFisica;

                BNE.Auth.BNEAutenticacao.LogarCandidato(objPessoaFisica.NomeCompleto, objPessoaFisica.IdPessoaFisica, objPessoaFisica.CPF, objCurriculo.IdCurriculo);
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
                            IdPessoaFisicaLogada.Value = objPessoaFisica.IdPessoaFisica;
                            IdFilial.Value = objUsuario.UltimaFilialLogada.IdFilial;

                            //Carrega o usuário filial perfil pela pessoa física e filial
                            UsuarioFilialPerfil objUsuarioFilialPerfil;
                            if (UsuarioFilialPerfil.CarregarPorPessoaFisicaFilial(objPessoaFisica.IdPessoaFisica, IdFilial.Value, out objUsuarioFilialPerfil))
                                IdUsuarioFilialPerfilLogadoEmpresa.Value = objUsuarioFilialPerfil.IdUsuarioFilialPerfil;
                        }
                    }
                }
            }
            else
            {
                UsuarioFilialPerfil objUsuarioFilialPerfil;
                if (UsuarioFilialPerfil.CarregarUsuarioEmpresaPorPessoaFisica(objPessoaFisica.IdPessoaFisica, out objUsuarioFilialPerfil))
                {
                    IdPessoaFisicaLogada.Value = objPessoaFisica.IdPessoaFisica;
                    IdUsuarioFilialPerfilLogadoEmpresa.Value = objUsuarioFilialPerfil.IdUsuarioFilialPerfil;
                    IdFilial.Value = objUsuarioFilialPerfil.Filial.IdFilial;
                }
            }
        }
        #endregion

        #region CarregarUsuarioCandidato
        private void CarregarUsuarioCandidato(PessoaFisica objPessoaFisica)
        {
            Curriculo objCurriculo;
            if (Curriculo.CarregarPorPessoaFisica(objPessoaFisica.IdPessoaFisica, out objCurriculo) && !objCurriculo.FlagInativo)
            {
                BNE.Auth.BNEAutenticacao.LogarCandidato(objPessoaFisica.NomeCompleto, objPessoaFisica.IdPessoaFisica, objPessoaFisica.CPF, objCurriculo.IdCurriculo, false);

                //Verifica se o curriculo está bloqueado
                IdCurriculo.Value = objCurriculo.IdCurriculo;
                IdPessoaFisicaLogada.Value = objPessoaFisica.IdPessoaFisica;

                if (CurriculoOrigem.ExisteCurriculoNaOrigem(objCurriculo, new BLL.Origem(IdOrigem.Value)))
                {
                    //Guardar IdUsuarioFilialPerfil em variável global conforme requisito
                    UsuarioFilialPerfil objUsuarioFilialPerfil;
                    if (UsuarioFilialPerfil.CarregarPorPessoaFisica(objPessoaFisica.IdPessoaFisica, out objUsuarioFilialPerfil))
                        IdUsuarioFilialPerfilLogadoCandidato.Value = objUsuarioFilialPerfil.IdUsuarioFilialPerfil;
                }
            }
            else
            {
                BNE.Auth.BNEAutenticacao.LogarCPF(objPessoaFisica.NomeCompleto, objPessoaFisica.IdPessoaFisica, objPessoaFisica.CPF);
            }

        }
        #endregion

        #region LimparCookieLoginVagas
        public void LimparCookieLoginVagas(HttpContextBase context)
        {
            var cookie = RecuperarCookieLoginVagas(context);
            cookie.Expires = DateTime.Today.AddDays(-1);
            context.Response.Cookies.Set(cookie);
        }
        #endregion

    }
}