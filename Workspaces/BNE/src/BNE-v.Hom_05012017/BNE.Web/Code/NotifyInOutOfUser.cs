using System;
using System.Web;
using BNE.Auth.Helper;
using BNE.Services.Base.ProcessosAssincronos;

namespace BNE.Web.Code
{
    public class NotifyInOutOfCandidate
    {
        internal void LoginProcess(Microsoft.IdentityModel.Claims.ClaimsIdentity claimsIdentity, bool existentAuthentication)
        {
            var tipoCandidato = claimsIdentity.TipoCandidato();
            if (!tipoCandidato ?? false)
                return;

            var pfId = claimsIdentity.GetPessoaFisicaId();
            if (!pfId.HasValue)
                return;

            var curId = claimsIdentity.GetCurriculoId();
            if (!curId.HasValue)
                return;

            LoginTrigger(pfId.Value, curId.Value, existentAuthentication);
        }

        private void LoginTrigger(int pessoaFisicaId, int curriculoId, bool existentAuthentication)
        {
            var parametros = new ParametroExecucaoCollection()
                        {
                            new ParametroExecucao
                            {
                                Parametro = "IdTipoGatilho",
                                DesParametro = "IdTipoGatilho",
                                Valor = ((int)BLL.Enumeradores.TipoGatilho.LoginCandidato).ToString()
                            },
                            new ParametroExecucao
                            {
                                Parametro = "IdCurriculo",
                                DesParametro = "IdCurriculo",
                                Valor = curriculoId.ToString()    
                            },
                            new ParametroExecucao
                            {
                                Parametro = "IdPessoaFisica",
                                DesParametro = "IdPessoaFisica",
                                Valor = pessoaFisicaId.ToString()
                            },
                            new ParametroExecucao
                            {
                                Parametro = "ExitentAuthentication",
                                DesParametro = "ExitentAuthentication",
                                Valor = existentAuthentication.ToString()
                            }
                        };
            try
            {
                ProcessoAssincrono.IniciarAtividade(BLL.AsyncServices.Enumeradores.TipoAtividade.GatilhosBNE, parametros);
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
            }

        }

        internal void LogoutProcess(Microsoft.IdentityModel.Claims.ClaimsIdentity claimsIdentity, HttpSessionStateBase currentSession, bool byTimeout)
        {
            var tipoCandidato = TipoCandidato(claimsIdentity, currentSession);

            if (!tipoCandidato ?? false)
                return;

            var pfId = PegarPessoaFisicaId(claimsIdentity, currentSession);
            if (!pfId.HasValue)
                return;

            var curId = PegarCurriculoId(claimsIdentity, currentSession);
            if (!curId.HasValue)
                return;

            LogoutTrigger(pfId.Value, curId.Value, byTimeout);
        }

        private void LogoutTrigger(int pessoaFisicaId, int curriculoId, bool byTimeout)
        {
            var parametros = new ParametroExecucaoCollection()
                        {
                            new ParametroExecucao
                            {
                                Parametro = "IdTipoGatilho",
                                DesParametro = "IdTipoGatilho",
                                Valor = ((int)BLL.Enumeradores.TipoGatilho.LogoutCandidato).ToString()
                            },
                            new ParametroExecucao
                            {
                                Parametro = "IdCurriculo",
                                DesParametro = "IdCurriculo",
                                Valor = curriculoId.ToString()    
                            },
                            new ParametroExecucao
                            {
                                Parametro = "IdPessoaFisica",
                                DesParametro = "IdPessoaFisica",
                                Valor = pessoaFisicaId.ToString()
                            },
                            new ParametroExecucao
                            {
                                Parametro = "SessionEndByTimeout",
                                DesParametro = "SessionEndByTimeout",
                                Valor = byTimeout.ToString()
                            }
                        };
            try
            {

                ProcessoAssincrono.IniciarAtividade(BLL.AsyncServices.Enumeradores.TipoAtividade.GatilhosBNE, parametros);
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
            }

        }

        private int? PegarCurriculoId(Microsoft.IdentityModel.Claims.ClaimsIdentity claimsIdentity, HttpSessionStateBase currentSession)
        {
            if (claimsIdentity != null)
            {
                var res = claimsIdentity.GetCurriculoId();
                if (res.HasValue)
                    return res.Value;
            }

            if (currentSession == null)
                return null;

            var cur = new Common.Session.WrapperSessionVariable<int>(currentSession, BNE.Web.Code.Enumeradores.Chave.Permanente.IdCurriculo.ToString());
            if (!cur.HasValue)
                return null;

            return cur.Value;
        }

        private int? PegarPessoaFisicaId(Microsoft.IdentityModel.Claims.ClaimsIdentity claimsIdentity, HttpSessionStateBase currentSession)
        {
            if (claimsIdentity != null)
            {
                var res = claimsIdentity.GetPessoaFisicaId();
                if (res.HasValue)
                    return res.Value;
            }

            if (currentSession == null)
                return null;

            var cur = new Common.Session.WrapperSessionVariable<int>(currentSession, BNE.Web.Code.Enumeradores.Chave.Permanente.IdPessoaFisicaLogada.ToString());
            if (!cur.HasValue)
                return null;

            return cur.Value;

        }

        private bool? TipoCandidato(Microsoft.IdentityModel.Claims.ClaimsIdentity claimsIdentity, HttpSessionStateBase currentSession)
        {
            if (claimsIdentity != null)
            {
                var res = claimsIdentity.TipoCandidato();
                if (res.HasValue)
                    return res.Value;
            }

            if (currentSession == null)
                return null;

            return true;
        }
    }
}