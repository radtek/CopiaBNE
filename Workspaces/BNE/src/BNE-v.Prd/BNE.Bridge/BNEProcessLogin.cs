using BNE.Auth.EventArgs;
using BNE.Auth.Helper;
using BNE.BLL;
using BNE.Common.Enumeradores;
using BNE.Common.Session;
using System;
using System.Globalization;
using Microsoft.IdentityModel.Claims;

namespace BNE.Bridge
{
    public static class BNELoginProcess
    {
        public static bool SalvarNovaSessaoBanco(BNEAuthEventArgs authArgs, BNESessaoLoginModelResult modelResult, BLL.Filial filial = null)
        {
            if (authArgs == null || authArgs.Context == null || authArgs.Context.Session == null || modelResult.Value != BNESessaoLoginResultType.OK)
                return false;

            var pfId = PegarPFId(authArgs);
            if (pfId <= 0)
                return false;

            BNE.BLL.Usuario objUsuario;
            if (BNE.BLL.Usuario.CarregarPorPessoaFisica(pfId, out objUsuario))
            {
                objUsuario.DescricaoSessionID = authArgs.Context.Session.SessionID;
                if (filial != null)
                {
                    objUsuario.UltimaFilialLogada = filial;
                }
            }
            else
            {
                objUsuario = new BNE.BLL.Usuario
                {
                    PessoaFisica = new BNE.BLL.PessoaFisica(PegarPFId(authArgs)),
                    DescricaoSessionID = authArgs.Context.Session.SessionID,
                    SenhaUsuario = new Guid().ToString().Substring(0, 8),
                    UltimaFilialLogada = filial
                };
            }

            objUsuario.Save();
            return true;
        }

        private static int PegarPFId(BNEAuthEventArgs authArgs)
        {
            var value = new WrapperSessionVariable<int>(authArgs.Context, Chave.Permanente.IdPessoaFisicaLogada.ToString()).ValueOrDefault;

            if (value > 0)
                return value;

            var principal = authArgs.Context.User as ClaimsPrincipal;

            if (principal == null)
                return 0;

            var identity = principal.Identity as ClaimsIdentity;

            if (identity == null)
                return 0;

            return identity.GetPessoaFisicaId().GetValueOrDefault();
        }

        public static bool RegistrarBLLProcess(BNEAuthEventArgs authArgs, BNESessaoLoginModelResult modelResult, Filial filial = null)
        {
            if (authArgs == null || authArgs.Context == null || authArgs.Context.Session == null || modelResult.Value != BNESessaoLoginResultType.OK)
                return false;

            switch (modelResult.Profile)
            {
                case BNESessaoProfileType.CANDIDATO:
                    return RegistrarBLLProcessCandidato(authArgs, modelResult);
                case BNESessaoProfileType.EMPRESA:
                    return RegistrarBLLProcessEmpresa(authArgs, modelResult, filial);
                case BNESessaoProfileType.INTERNO:
                    return RegistrarBLLProcessInterno(authArgs, modelResult);
                default:
                    return false;
            }
        }

        private static bool RegistrarBLLProcessInterno(BNEAuthEventArgs authArgs, BNESessaoLoginModelResult modelResult)
        {
            return true;
        }

        private static bool RegistrarBLLProcessEmpresa(BNEAuthEventArgs authArgs, BNESessaoLoginModelResult modelResult, Filial filial = null)
        {
            return true;
        }

        private static bool RegistrarBLLProcessCandidato(BNEAuthEventArgs authArgs, BNESessaoLoginModelResult modelResult)
        {
            // As regras estão definidas em Login.ascx (aqui é só uma cópia)
            if (modelResult.DataProcessInfo.QuantidadeEmpresas > 0)
                return true;

            if (modelResult.DataProcessInfo.UsaSTC)
                return true;

            if (!modelResult.DataProcessInfo.ExisteCurriculoNaOrigem)
                return true;

            if (modelResult.DataProcessInfo.Curriculo == null)
                return false;

            int diaslimite = Convert.ToInt32(BNE.BLL.Parametro.RecuperaValorParametro(BNE.BLL.Enumeradores.Parametro.DiasLimiteAtualizacaoCurriculo));
            if (modelResult.DataProcessInfo.Curriculo.DataAtualizacao < DateTime.Now.AddDays(-1 * diaslimite))
                return true;

            try
            {
                modelResult.DataProcessInfo.Curriculo.DataAtualizacao = DateTime.Now; //task 7367
                modelResult.DataProcessInfo.Curriculo.Salvar();

                var enfileiraPublicacao = Convert.ToBoolean(BNE.BLL.Parametro.RecuperaValorParametro(BNE.BLL.Enumeradores.Parametro.EnfileiraPublicacaoAutomaticaCurriculo));
                if (enfileiraPublicacao)
                {
                    var parametros = new BNE.Services.Base.ProcessosAssincronos.ParametroExecucaoCollection
                                    {
                                        {"idCurriculo", "Curriculo", modelResult.DataProcessInfo.Curriculo.IdCurriculo.ToString(CultureInfo.InvariantCulture),
                                                                     modelResult.DataProcessInfo.Curriculo.IdCurriculo.ToString(CultureInfo.InvariantCulture)}
                                    };

                    Services.Base.ProcessosAssincronos.ProcessoAssincrono.IniciarAtividade(BLL.AsyncServices.Enumeradores.TipoAtividade.PublicacaoCurriculo, parametros);
                }
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
                return false;
            }

            return true;
        }
    }
}
