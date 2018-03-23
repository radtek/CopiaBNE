using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BNE.BLL;
using BNE.Chat.Core.Interface;
using BNE.Chat.DTO.Log;
using BNE.Chat.Helper;
using BNE.Common.Session;
using BNE.Web.Code.Enumeradores;

namespace BNE.Web
{
    public class ChatSecuritySelecionador : IClientSimpleSecurity
    {
        private readonly ConcurrentDictionary<int, Tuple<DateTime, bool>> _cachePermissionSecurity;

        public static readonly SetValueOrDefaultFact<HardConfig<int>, int> RefreshSecurityCache =
            new HardConfig<int>("chat_verificacao_seguranca_usuario_em_minutos", 5).Wrap(a => a.Value);

        public ChatSecuritySelecionador()
        {
            _cachePermissionSecurity = new ConcurrentDictionary<int, Tuple<DateTime, bool>>();
            Permissions = BuildPermissions();
        }

        public bool Evaluate(HttpContext context)
        {
            if (context == null)
                if (!Permissions.Any())
                    return true;
                else
                    return false;

            foreach (var item in Permissions)
            {
                if (!item(context))
                    return false;
            }
            return true;
        }

        public IEnumerable<Func<HttpContext, bool>> Permissions { get; private set; }

        private IEnumerable<Func<HttpContext, bool>> BuildPermissions()
        {
            foreach (var permission in GetBasicPermissions())
            {
                yield return permission;
            }

            foreach (var permission in GetCachedPermissions())
            {
                yield return permission;
            }
        }

        private IEnumerable<Func<HttpContext, bool>> GetBasicPermissions()
        {
            yield return context => context.Session != null;
            yield return context => context.Session[typeof(SessionVariable<int>) + Chave.Permanente.IdUsuarioFilialPerfilLogadoEmpresa.ToString()] != null;
            yield return context => context.Session[typeof(SessionVariable<int>) + Chave.Permanente.IdFilial.ToString()] != null;
        }

        private IEnumerable<Func<HttpContext, bool>> GetCachedPermissions()
        {
            yield return context =>
                {
                    int aux;
                    return
                        Int32.TryParse(
                            context.Session[
                                typeof(SessionVariable<int>) +
                                Chave.Permanente.IdUsuarioFilialPerfilLogadoEmpresa.ToString()].ToString(), out aux);
                };

            yield return context =>
                {
                    int filialPerfilId = Convert.ToInt32(context.Session[
                        typeof(SessionVariable<int>) +
                        Chave.Permanente.IdUsuarioFilialPerfilLogadoEmpresa.ToString()].ToString());

                    if (filialPerfilId <= 0)
                        return false;

                    var permissionFactory =
                        new Func<int, bool>(
                            id => CelularSelecionador.VerificaCelularEstaLiberadoParaTanque(new UsuarioFilialPerfil(id)));

                    LimparCacheSeNecessario();

                    try
                    {
                        return CriarOuAtualizarCache(filialPerfilId, DateTime.Now, permissionFactory);
                    }
                    catch (Exception ex)
                    {
                        EL.GerenciadorException.GravarExcecao(ex, "#Chat Security");
                        return false;
                    }
                };
        }

        private bool CriarOuAtualizarCache(int filialPerfilId, DateTime now, Func<int, bool> permissionFactory)
        {
            var result = _cachePermissionSecurity.GetOrAdd(filialPerfilId, fact => new Tuple<DateTime, bool>(DateTime.Now, permissionFactory(fact)));
            return result.Item2;
        }

        private void LimparCacheSeNecessario()
        {
            foreach (var item in _cachePermissionSecurity)
            {
                if (DateTime.Now - item.Value.Item1 >= TimeSpan.FromMinutes(RefreshSecurityCache.Value))
                {
                    Tuple<DateTime, bool> aux;
                    _cachePermissionSecurity.TryRemove(item.Key, out aux);
                }
            }
        }


    }
}