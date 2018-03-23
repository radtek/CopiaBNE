using BNE.Common.Enumeradores;
using BNE.Common.Session;
using System;
using System.Collections.Generic;
using BNE.Auth.Core;

namespace BNE.Auth
{

    public class BNECleanDefaultSession : LoginBehaviorBase
    {
        //não funciona
        protected override void OnBeforeLogoffSafe(Func<System.Web.HttpContextBase> contextAccessor, LogOffInfo info)
        {
            var context = contextAccessor();
            if (context == null)
                return;

            foreach (var item in SessionVariablesToClean())
            {
                item();
            }          
        }

        protected IEnumerable<Action> SessionVariablesToClean()
        {
            yield return () => new SessionVariable<int>(Chave.Permanente.IdPessoaFisicaLogada.ToString()).Clear();
            yield return () => new SessionVariable<int>(Chave.Permanente.IdCurriculo.ToString()).Clear();
            yield return () => new SessionVariable<int>(Chave.Permanente.IdFilial.ToString()).Clear();
            yield return () => new SessionVariable<int>(Chave.Permanente.IdUsuarioLogado.ToString()).Clear();
            yield return () => new SessionVariable<int>(Chave.Permanente.IdUsuarioFilialPerfilLogadoCandidato.ToString()).Clear();
            yield return () => new SessionVariable<int>(Chave.Permanente.IdUsuarioFilialPerfilLogadoUsuarioInterno.ToString()).Clear();
            yield return () => new SessionVariable<int>(Chave.Permanente.IdUsuarioFilialPerfilLogadoEmpresa.ToString()).Clear();
            yield return () => new SessionVariable<int>(Chave.Permanente.IdPerfil.ToString()).Clear();
            yield return () => new SessionVariable<string>(Chave.Permanente.FuncaoMaster.ToString()).Clear();
            yield return () => new SessionVariable<string>(Chave.Permanente.CidadeMaster.ToString()).Clear();
            yield return () => new SessionVariable<string>(Chave.Permanente.PalavraChaveMaster.ToString()).Clear();
            yield return () => new SessionVariable<string>(Chave.Permanente.AutenticacaoVerificadaNaSessao.ToString()).Clear();
            yield return () => new SessionVariable<bool>(Chave.Permanente.IsSTCMaster.ToString()).Clear();
        }

    }
}
