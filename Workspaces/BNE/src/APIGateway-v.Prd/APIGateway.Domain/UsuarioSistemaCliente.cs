using APIGateway.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIGateway.Domain
{
    public class UsuarioSistemaCliente
    {
        private static string GetCacheKey(Guid chave)
        {
            return String.Format("UsuarioSistema:{0}", chave);
        }

        public static Model.UsuarioSistemaCliente Obter(Guid chave)
        {
            string cacheKey = GetCacheKey(chave);
            Model.UsuarioSistemaCliente usc = (Model.UsuarioSistemaCliente)Cache.CacheManager.GetCached(cacheKey);

            if (usc != null)
                return usc;

            using (var _context = new APIGatewayContext())
            {
                usc = (from u in _context.UsuarioSistemaCliente
                       .Include("Headers")
                       .Include("SistemaCliente")
                       where u.SistemaCliente.Chave.Equals(chave)
                       select u).FirstOrDefault();
            }

            if (usc == null)
                return null;

            Cache.CacheManager.Cache(cacheKey, usc);

            return usc;
        }

        /// <summary>
        /// Cria um usuário para o sistema se ainda não foi registrado
        /// </summary>
        /// <param name="objSistemaCliente">Sistema cliente para qual o usuário deve ser criado</param>
        public static void Create(Model.SistemaCliente objSistemaCliente, APIGatewayContext _ctx)
        {
            if (!_ctx.UsuarioSistemaCliente.Any(u => u.SistemaCliente.Chave == objSistemaCliente.Chave))
            {
                Model.UsuarioSistemaCliente objUsuario = new Model.UsuarioSistemaCliente()
                {
                    SistemaCliente = objSistemaCliente
                };

                _ctx.UsuarioSistemaCliente.Add(objUsuario);

                _ctx.SaveChanges();
            }
        }

        /// <summary>
        /// Cria um usuário para o sistema se ainda não foi registrado
        /// </summary>
        /// <param name="objSistemaCliente">Sistema cliente para qual o usuário deve ser criado</param>
        public static void Create(Model.SistemaCliente objSistemaCliente)
        {
            using (var _ctx = new APIGatewayContext())
            {
                Create(objSistemaCliente, _ctx);
            }
        }
    }
}
