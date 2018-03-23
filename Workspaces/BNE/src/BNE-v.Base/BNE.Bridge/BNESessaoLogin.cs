using BNE.BLL;
using BNE.Bridge;
using BNE.Common.Enumeradores;
using BNE.Common.Session;
using BNE.EL;
using Microsoft.IdentityModel.Claims;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace BNE.Bridge
{
    public static class BNESessaoLogin
    {
        public static BNESessaoLoginModelResult PreencherDadosSessao(System.Web.HttpContext context, ClaimsIdentity identity)
        {
            return PreencherDadosSessao(new HttpContextWrapper(context), identity);
        }

        public static BNESessaoLoginModelResult PreencherDadosSessao(System.Web.HttpContextBase context, ClaimsIdentity identity)
        {
            if (identity == null)
                return new BNESessaoLoginModelResult(BNESessaoLoginResultType.NOK_KEEP_STATE);

            if (!identity.IsAuthenticated)
                return new BNESessaoLoginModelResult(BNESessaoLoginResultType.NOK_KEEP_STATE);

            if (identity.Claims == null || identity.Claims.Count <= 0)
                return new BNESessaoLoginModelResult(BNESessaoLoginResultType.NOK_KEEP_STATE);

            var cpfClaim = identity.Claims.FirstOrDefault(a => a.ClaimType == BNE.Auth.BNEClaimTypes.CPF);

            decimal cpfValue;
            if (cpfClaim != null && decimal.TryParse(cpfClaim.Value, out cpfValue))
            {
                var res = UsuarioLogadoPreencherDadosSessao(context, cpfValue);
                return res;
            }

            var idClaim = identity.Claims.FirstOrDefault(a => a.ClaimType == BNE.Auth.BNEClaimTypes.PessoaFisicaId);

            int idValue;
            if (idClaim != null && Int32.TryParse(idClaim.Value, out idValue))
            {
                try
                {
                    var pf = PessoaFisica.LoadObject(idValue);
                    return UsuarioLogadoPreencherDadosSessao(context, pf, false);
                }
                catch (RecordNotFoundException)
                {

                }

            }

            BNE.Auth.BNEAutenticacao.DeslogarPadrao(context);
            return new BNESessaoLoginModelResult(BNESessaoLoginResultType.NOK_LOGOUT);
        }

        public static BNESessaoLoginModelResult UsuarioLogadoPreencherDadosSessao(System.Web.HttpContext context, decimal cpf, bool checkIdentity = true)
        {
            return UsuarioLogadoPreencherDadosSessao(new HttpContextWrapper(context), cpf, checkIdentity);
        }
        public static BNESessaoLoginModelResult UsuarioLogadoPreencherDadosSessao(System.Web.HttpContextBase context, decimal cpf, bool checkIdentity = true)
        {
            if (checkIdentity)
            {
                if (context.User == null || context.User.Identity == null || !context.User.Identity.IsAuthenticated)
                {
                    return new BNESessaoLoginModelResult(BNESessaoLoginResultType.NOK_KEEP_STATE);
                }
            }

            PessoaFisica pf;
            try
            {
                pf = PessoaFisica.CarregarPorCPF(cpf);
            }
            catch (RecordNotFoundException)
            {
                BNE.Auth.BNEAutenticacao.DeslogarPadrao(context);
                return new BNESessaoLoginModelResult(BNESessaoLoginResultType.NOK_LOGOUT);
            }

            return UsuarioLogadoPreencherDadosSessao(context, pf, false);

        }

        public static BNESessaoLoginModelResult UsuarioLogadoPreencherDadosSessao(System.Web.HttpContext context, PessoaFisica objPessoaFisica, bool checkIdentity = true)
        {
            return UsuarioLogadoPreencherDadosSessao(new HttpContextWrapper(context), objPessoaFisica, checkIdentity);
        }
        public static BNESessaoLoginModelResult UsuarioLogadoPreencherDadosSessao(System.Web.HttpContextBase context, PessoaFisica objPessoaFisica, bool checkIdentity = true)
        {
            if (checkIdentity)
            {
                if (context.User == null || context.User.Identity == null || !context.User.Identity.IsAuthenticated)
                {
                    return new BNESessaoLoginModelResult(BNESessaoLoginResultType.NOK_KEEP_STATE);
                }
            }

            var oldContext = HttpContext.Current;
            try
            {
                var replaceContext = ((HttpApplication)context.GetService(typeof(HttpApplication))).Context;
                if (replaceContext != null)
                    HttpContext.Current = replaceContext;

                var handler = ProcessoGravarSessao(objPessoaFisica);

                if (handler.Result == BNESessaoLoginResultType.UNKNOWN)
                {
                    return new BNESessaoLoginModelResult(handler.Result, handler.Profile).WrapDetails(handler);
                }

                Delegate[] invocation;
                while ((invocation = GetCompleteAction(handler)).Length > 0) // because of possible trampoline
                {
                    foreach (Action item in invocation)
                    {
                        item();
                        handler.Remove(item);
                    }
                }

                return new BNESessaoLoginModelResult(handler.Result, handler.Profile).WrapDetails(handler);
            }
            finally
            {
                HttpContext.Current = oldContext;
            }
        }

        private static Delegate[] GetCompleteAction(BNESessaoLoginProcessModel handler)
        {
            return handler.OnCompleteAction == null ? new Delegate[0] : handler.OnCompleteAction.GetInvocationList();
        }

        private static BNESessaoLoginProcessModel ProcessoGravarSessao(PessoaFisica objPessoaFisica)
        {
            if (objPessoaFisica == null || objPessoaFisica.FlagInativo.HasValue && objPessoaFisica.FlagInativo.Value)
            {
                return CancelProcess(new BNESessaoLoginProcessModel(), BNESessaoLoginResultType.NOK_LOGOUT);
            }

            UsuarioFilialPerfil objPerfisPessoa;
            if (!UsuarioFilialPerfil.CarregarPorPessoaFisica(objPessoaFisica.IdPessoaFisica, out objPerfisPessoa))
            {
                return CancelProcess(new BNESessaoLoginProcessModel(), BNESessaoLoginResultType.NOK_LOGOUT);
            }

            var processModel = new BNESessaoLoginProcessModel();

            int idUsuarioFilialPerfil, idPerfil;
            if (PessoaFisica.VerificaPessoaFisicaUsuarioInterno(objPessoaFisica.IdPessoaFisica, out idUsuarioFilialPerfil, out idPerfil))
            {
                processModel.Profile = BNESessaoProfileType.INTERNO;

                if (PopularUsuarioInterno(processModel, objPessoaFisica, idUsuarioFilialPerfil, idPerfil))
                {
                    processModel.Result = BNESessaoLoginResultType.OK;
                    return processModel;
                }

                return CancelProcess(processModel, BNESessaoLoginResultType.NOK_LOGOUT);
            }


            var quantidadeEmpresa = UsuarioFilialPerfil.QuantidadeUsuarioEmpresa(objPessoaFisica.IdPessoaFisica);
            if (quantidadeEmpresa != 0)
            {
                return VerificaUsuarioEmpresa(processModel, objPessoaFisica, quantidadeEmpresa);
            }

            processModel.Profile = BNESessaoProfileType.CANDIDATO;
            return VerificaUsuarioCandidato(processModel, objPessoaFisica);
        }

        private static BNESessaoLoginProcessModel VerificaUsuarioEmpresa(BNESessaoLoginProcessModel processModel, PessoaFisica objPessoaFisica, int quantidade)
        {
            if (!new SessionVariable<bool>(Chave.Permanente.STC.ToString()).ValueOrDefault)
                return CarregaUsuarioEmpresa(processModel, objPessoaFisica, quantidade);

            processModel.ExtraInfo.Add("UsaSTC", true);

            processModel.Profile = BNESessaoProfileType.EMPRESA;

            var origem = new SessionVariable<int>(Chave.Permanente.IdOrigem.ToString()).ValueOrDefault;

            OrigemFilial objOrigemFilial;
            if (origem <= 0 || !OrigemFilial.CarregarPorOrigem(origem, out objOrigemFilial))
                return CancelProcess(processModel, BNESessaoLoginResultType.NOK_LOGOUT);

            UsuarioFilialPerfil objUsuarioFilialPerfil;
            if (!UsuarioFilialPerfil.CarregarPorPessoaFisicaFilial(objPessoaFisica.IdPessoaFisica, objOrigemFilial.Filial.IdFilial, out objUsuarioFilialPerfil))
                return CancelProcess(processModel, BNESessaoLoginResultType.NOK_LOGOUT);

            if (objUsuarioFilialPerfil.FlagInativo)
                return CancelProcess(processModel, BNESessaoLoginResultType.NOK_LOGOUT);

            if (!PopularPessoaFisica(processModel, objPessoaFisica))
                return CancelProcess(processModel, BNESessaoLoginResultType.NOK_LOGOUT);

            processModel.Concat(() => new SessionVariable<int>(Chave.Permanente.IdFilial.ToString()).Value = objUsuarioFilialPerfil.Filial.IdFilial);
            processModel.Concat(() => new SessionVariable<int>(Chave.Permanente.IdUsuarioFilialPerfilLogadoEmpresa.ToString()).Value = objUsuarioFilialPerfil.IdUsuarioFilialPerfil);
            processModel.Result = BNESessaoLoginResultType.OK;
            return processModel;
        }

        private static BNESessaoLoginProcessModel CarregaUsuarioEmpresa(BNESessaoLoginProcessModel processModel, PessoaFisica objPessoaFisica, int quantidade)
        {
            processModel.ExtraInfo.Add("QuantidadeEmpresas", quantidade);

            Curriculo objCurriculo;
            if (Curriculo.CarregarPorPessoaFisica(objPessoaFisica.IdPessoaFisica, out objCurriculo))
            {
                processModel.Profile = BNESessaoProfileType.CANDIDATO;

                if (objCurriculo.SituacaoCurriculo.IdSituacaoCurriculo.Equals((int)BNE.BLL.Enumeradores.SituacaoCurriculo.Bloqueado) || objCurriculo.FlagInativo)
                {
                    return CancelProcess(processModel, BNESessaoLoginResultType.NOK_LOGOUT);
                }

                return CarregaPessoaFisica(processModel, objPessoaFisica, objCurriculo);
            }

            processModel.Profile = BNESessaoProfileType.EMPRESA;
            if (quantidade <= 1)
            {
                UsuarioFilialPerfil objUsuarioFilialPerfil;
                if (!UsuarioFilialPerfil.CarregarUsuarioEmpresaPorPessoaFisica(objPessoaFisica.IdPessoaFisica, out objUsuarioFilialPerfil))
                {
                    processModel.Result = BNESessaoLoginResultType.NOK_KEEP_STATE;
                    return processModel;
                }

                if (!Usuario.UsuarioPodeLogarFilial(new Filial(objUsuarioFilialPerfil.Filial.IdFilial), new PessoaFisica(objPessoaFisica.IdPessoaFisica)))
                {
                    return CancelProcess(processModel, BNESessaoLoginResultType.NOK_LOGOUT);
                }

                if (!PopularPessoaFisica(processModel, objPessoaFisica))
                {
                    return CancelProcess(processModel, BNESessaoLoginResultType.NOK_LOGOUT);
                }

                processModel.Concat(() => new SessionVariable<int>(Chave.Permanente.IdFilial.ToString()).Value = objUsuarioFilialPerfil.Filial.IdFilial);
                processModel.Concat(() => new SessionVariable<int>(Chave.Permanente.IdUsuarioFilialPerfilLogadoEmpresa.ToString()).Value = objUsuarioFilialPerfil.IdUsuarioFilialPerfil);

                processModel.Result = BNESessaoLoginResultType.OK;
                return processModel;
            }

            Usuario objUsuario;
            if (!Usuario.CarregarPorPessoaFisica(objPessoaFisica.IdPessoaFisica, out objUsuario))
            {
                processModel.Result = BNESessaoLoginResultType.NOK_KEEP_STATE;
                return processModel;
            }

            if (!PopularPessoaFisica(processModel, objPessoaFisica))
            {
                return CancelProcess(processModel, BNESessaoLoginResultType.NOK_LOGOUT);
            }

            if (objUsuario.UltimaFilialLogada == null)
            {
                processModel.Result = BNESessaoLoginResultType.NOK_KEEP_STATE;
                return processModel;
            }

            if (!UsuarioFilialPerfil.VerificaUsuarioFilialPorUltimaEmpresaLogada(objPessoaFisica.IdPessoaFisica, objUsuario.UltimaFilialLogada.IdFilial))
            {
                processModel.Result = BNESessaoLoginResultType.NOK_KEEP_STATE;
                return processModel;
            }

            if (!Usuario.UsuarioPodeLogarFilial(new Filial(objUsuario.UltimaFilialLogada.IdFilial), new PessoaFisica(objPessoaFisica.IdPessoaFisica)))
            {
                return CancelProcess(processModel, BNESessaoLoginResultType.NOK_LOGOUT);
            }

            processModel.Concat(() => new SessionVariable<int>(Chave.Permanente.IdFilial.ToString()).Value = objUsuario.UltimaFilialLogada.IdFilial);

            UsuarioFilialPerfil objUsuFiPerf;
            if (!UsuarioFilialPerfil.CarregarPorPessoaFisicaFilial(objPessoaFisica.IdPessoaFisica, objUsuario.UltimaFilialLogada.IdFilial, out objUsuFiPerf))
            {
                processModel.Result = BNESessaoLoginResultType.NOK_KEEP_STATE;
                return processModel;
            }

            processModel.Concat(() => new SessionVariable<int>(Chave.Permanente.IdUsuarioFilialPerfilLogadoEmpresa.ToString()).Value = objUsuFiPerf.IdUsuarioFilialPerfil);
            processModel.Result = BNESessaoLoginResultType.OK;
            return processModel;
        }

        private static BNESessaoLoginProcessModel VerificaUsuarioCandidato(BNESessaoLoginProcessModel process, PessoaFisica objPessoaFisica)
        {
            Curriculo objCurriculo;
            if (!Curriculo.CarregarPorPessoaFisica(objPessoaFisica.IdPessoaFisica, out objCurriculo))
            {
                return CancelProcess(process, BNESessaoLoginResultType.NOK_LOGOUT);
            }

            if (objCurriculo.SituacaoCurriculo.IdSituacaoCurriculo.Equals((int)BNE.BLL.Enumeradores.SituacaoCurriculo.Bloqueado) || objCurriculo.FlagInativo)
            {
                return CancelProcess(process, BNESessaoLoginResultType.NOK_LOGOUT);
            }

            return CarregaPessoaFisica(process, objPessoaFisica, objCurriculo);
        }

        private static BNESessaoLoginProcessModel CarregaPessoaFisica(BNESessaoLoginProcessModel processModel, PessoaFisica objPessoaFisica, Curriculo objCurriculo)
        {
            processModel.ExtraInfo.Add("Curriculo", objCurriculo);

            processModel.Concat(() => new SessionVariable<int>(Chave.Permanente.IdCurriculo.ToString()).Value = objCurriculo.IdCurriculo);
            if (!PopularPessoaFisica(processModel, objPessoaFisica))
            {
                return CancelProcess(processModel, BNESessaoLoginResultType.NOK_LOGOUT);
            }

            UsuarioFilialPerfil objUsuarioFilialPerfil;
            if (!UsuarioFilialPerfil.CarregarPorPessoaFisica(objPessoaFisica.IdPessoaFisica, out objUsuarioFilialPerfil))
            {
                return CancelProcess(processModel, BNESessaoLoginResultType.NOK_LOGOUT);
            }

            processModel.Concat(() => new SessionVariable<int>(Chave.Permanente.IdUsuarioFilialPerfilLogadoCandidato.ToString()).Value = objUsuarioFilialPerfil.IdUsuarioFilialPerfil);

            var origemValue = new SessionVariable<int>(Chave.Permanente.IdOrigem.ToString()).ValueOrDefault;
            if (origemValue <= 0 || !CurriculoOrigem.ExisteCurriculoNaOrigem(objCurriculo, new Origem(origemValue)))
            {
                processModel.Concat(() => HttpContext.Current.Session.Add("Variavel1", objPessoaFisica.IdPessoaFisica)); // temporáraria
                processModel.Result = BNESessaoLoginResultType.NOK_KEEP_STATE;
                return processModel;
            }
            processModel.ExtraInfo.Add("ExisteCurriculoNaOrigem", true);
            processModel.Result = BNESessaoLoginResultType.OK;
            return processModel;
        }

        private static BNESessaoLoginProcessModel CancelProcess(BNESessaoLoginProcessModel processModel, BNESessaoLoginResultType result)
        {
            processModel.Result = result;

            if (result == BNESessaoLoginResultType.NOK_LOGOUT)
                processModel.Override(() => BNE.Auth.BNEAutenticacao.DeslogarPadrao(BNE.Auth.LogoffType.UNAUTHORIZED));
            else
                processModel.Override(null);
            return processModel;
        }

        private static bool PopularUsuarioInterno(BNESessaoLoginProcessModel process, PessoaFisica objPessoaFisica, int idUsuarioFilialPerfil, int idPerfil)
        {
            Action action = () =>
                {
                    new SessionVariable<int>(Chave.Permanente.IdUsuarioFilialPerfilLogadoUsuarioInterno.ToString()).Value = idUsuarioFilialPerfil;
                    new SessionVariable<int>(Chave.Permanente.IdPerfil.ToString()).Value = idPerfil;
                };

            process.Concat(action);
            return PopularPessoaFisica(process, objPessoaFisica);
        }

        private static bool PopularPessoaFisica(BNESessaoLoginProcessModel process, PessoaFisica objPessoaFisica)
        {
            process.Concat(() => new SessionVariable<int>(Chave.Permanente.IdPessoaFisicaLogada.ToString()).Value = objPessoaFisica.IdPessoaFisica);
            return true;
        }

    }
}
