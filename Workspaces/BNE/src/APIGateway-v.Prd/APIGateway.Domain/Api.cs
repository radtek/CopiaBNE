using APIGateway.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIGateway.Domain
{
    public class Api
    {
        private static string GetCacheKey(String UrlSuffix)
        {
            return String.Format("Api:{0}", UrlSuffix);
        }

        #region Listar
        /// <summary>
        /// Lista todas as Apis presentes no banco, com todas as suas relações.
        /// </summary>
        public static List<Model.Api> Listar()
        {
            using (var _context = new APIGatewayContext())
            {
                return (from a in _context.Api
                                      .Include("AuthenticationType")
                                      .Include("AuthenticationType.OAuthConfig")
                                      .Include("Sistemas")
                                      .Include("Sistemas.Headers")
                                      .Include("Endpoints")
                                      .Include("SwaggerConfig")
                        select a).ToList();
            }
        }
        #endregion

        #region ObterCompleto
        /// <summary>
        /// Lista todas as Apis presentes no banco, com todas as suas relações.
        /// </summary>
        private static Model.Api ObterCompleto(string UrlSuffix)
        {
            using (var _context = new APIGatewayContext())
            {
                return (from a in _context.Api
                                      .Include("AuthenticationType")
                                      .Include("AuthenticationType.OAuthConfig")
                                      .Include("Sistemas")
                                      .Include("Sistemas.Headers")
                                      .Include("Endpoints")
                                      .Include("SwaggerConfig")
                        where a.UrlSuffix == UrlSuffix
                        select a).FirstOrDefault();
            }
        }

        /// <summary>
        /// Lista todas as Apis presentes no banco, com todas as suas relações.
        /// </summary>
        private static Model.Api ObterCompleto(string UrlSuffix, APIGatewayContext _context)
        {
            return (from a in _context.Api
                                  .Include("AuthenticationType")
                                  .Include("AuthenticationType.OAuthConfig")
                                  .Include("Sistemas")
                                  .Include("Sistemas.Headers")
                                  .Include("Endpoints")
                                  .Include("SwaggerConfig")
                    where a.UrlSuffix == UrlSuffix
                    select a).FirstOrDefault();
        }
        #endregion

        #region CarregarPorSuffix
        /// <summary>
        /// Carrega a api pelo sufixo.
        /// </summary>
        /// <param name="numeroCNPJ"></param>
        /// <returns></returns>
        public static Model.Api CarregarPorSuffix(string UrlSuffix)
        {
            string cacheKey = GetCacheKey(UrlSuffix);
            Model.Api api = (Model.Api)Cache.CacheManager.GetCached(cacheKey);

            if (api != null)
                return api;

            using (var _ctx = new APIGatewayContext())
            {
                api = ObterCompleto(UrlSuffix);
            }

            Cache.CacheManager.Cache(GetCacheKey(UrlSuffix), api);

            return api;
        }
        #endregion

        /// <summary>
        /// Concede o acesso à API a determinado sistema
        /// </summary>
        /// <param name="UrlSuffix">Sufixo da api a conceder acesso</param>
        /// <param name="chaveSistema">Chave do sistema a conceder acesso</param>
        public static void ConcederAcesso(string UrlSuffix, Guid chaveSistema)
        {

            using (var _ctx = new APIGatewayContext())
            {
                Model.Api objApi = ObterCompleto(UrlSuffix, _ctx);
                if (objApi == null) throw new Exception(String.Format("Api com sufixo {0} não existe", UrlSuffix));

                Model.SistemaCliente objSistema = _ctx.SistemaCliente.FirstOrDefault(s => s.Chave == chaveSistema);
                if (objSistema == null) throw new Exception(String.Format("Sistema com chave {0} não existe", chaveSistema));

                if (!objApi.Sistemas.Any(s => s.Chave == chaveSistema))
                {
                    objApi.Sistemas.Add(objSistema);

                    _ctx.SaveChanges();

                    Cache.CacheManager.Cache(GetCacheKey(UrlSuffix), objApi);
                }

                UsuarioSistemaCliente.Create(objSistema, _ctx);
            }

        }

        /// <summary>
        /// Retira o acesso à API a determinado sistema
        /// </summary>
        /// <param name="UrlSuffix">Sufixo da api a retirar acesso</param>
        /// <param name="chaveSistema">Chave do sistema a retirar acesso</param>
        public static void RetirarAcesso(string UrlSuffix, Guid chaveSistema)
        {
            using (var _ctx = new APIGatewayContext())
            {
                Model.Api objApi = ObterCompleto(UrlSuffix, _ctx);
                if (objApi == null) throw new Exception(String.Format("Api com sufixo {0} não existe", UrlSuffix));

                Model.SistemaCliente objSistema = _ctx.SistemaCliente.FirstOrDefault(s => s.Chave == chaveSistema);
                if (objSistema == null) throw new Exception(String.Format("Sistema com chave {0} não existe", chaveSistema));

                if (objApi.Sistemas.Any(s => s.Chave == chaveSistema))
                {
                    objApi.Sistemas.Remove(objApi.Sistemas.First(s => s.Chave == chaveSistema));

                    _ctx.SaveChanges();

                    Cache.CacheManager.Cache(GetCacheKey(UrlSuffix), objApi);
                }

                _ctx.SaveChanges();
            }
        }
    }
}
