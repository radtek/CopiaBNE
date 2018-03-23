using BNE.Common.Enumeradores;
using BNE.Common.Session;
using Microsoft.IdentityModel.Claims;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BNE.Auth
{

    public class BNECleanDefaultSession : LoginBehaviorBase
    {
        //não funciona
        //private List<KeyValuePair<string, object>> _saveLogoffState; a
        protected override void OnBeforeLogoffSafe(Func<System.Web.HttpContextBase> contextAccessor, BNE.Auth.LoginBehaviorBase.LogOffInfo info)
        {
            var context = contextAccessor();
            if (context == null)
                return;

            foreach (var item in SessionVariablesToClean())
            {
                item();
            }          

            // não funciona
            //  _saveLogoffState = new List<KeyValuePair<string, object>>();
            //foreach (var item in context.Session.Keys)  // mantém outras informações
            //{
            //    var key = (string)item;
            //    _saveLogoffState.Add(new KeyValuePair<string, object>(key, context.Session[key]));
            //}
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
        }

        public override void OnAfterLogoff(System.Web.HttpContextBase context, LoginBehaviorBase.LogOffInfo info)
        {
            // não funciona
            //if (_saveLogoffState == null
            //    || context == null)
            //    return;

            //foreach (var pair in _saveLogoffState) // mantém outras informações
            //{
            //    context.Session[pair.Key] = pair.Value;
            //}

            //_saveLogoffState.Clear();
        }


    }
}
