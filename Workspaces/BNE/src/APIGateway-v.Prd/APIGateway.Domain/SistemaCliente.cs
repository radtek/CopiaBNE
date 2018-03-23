using APIGateway.Data;
using APIGateway.Domain.Code;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIGateway.Domain
{
    public class SistemaCliente
    {
        /// <summary>
        /// Lista os sistemas cadastrados
        /// </summary>
        /// <returns>List com os objetos SistemaCliente</returns>
        public static List<Model.SistemaCliente> List()
        {
            using (var _context = new APIGatewayContext())
            {
                return (from s in _context.SistemaCliente.Include("Apis")
                        select s).ToList();
            }
        }

        /// <summary>
        /// Lista os sistemas cadastrados
        /// </summary>
        /// <param name="ApiUrlSuffix">Api a ser considerada na busca</param>
        /// <param name="reverse">Se false, Listará os sistemas com permissão na Api. Se false, listará os sistemas sem acesso à Api.</param>
        /// <returns>List com os objetos SistemaCliente</returns>
        public static List<Model.SistemaCliente> List(String ApiUrlSuffix, bool reverse = false)
        {
            if (String.IsNullOrEmpty(ApiUrlSuffix))
                throw new ArgumentException("ApiUrlSuffix não pode ser null ou vazio");

            using (var _context = new APIGatewayContext())
            {
                if (reverse)
                {
                    return (from s in _context.SistemaCliente
                            where !s.Apis.Any(a => a.UrlSuffix == ApiUrlSuffix)
                            select s).ToList();
                }
                else
                {
                    return (from s in _context.SistemaCliente
                            where s.Apis.Any(a => a.UrlSuffix == ApiUrlSuffix)
                            select s).ToList();
                }
            }
        }

        /// <summary>
        /// Metodo para pesquisa de sistemas
        /// </summary>
        /// <param name="chave">Chave do sistema pesquisado</param>
        /// <returns>Objeto do sistema solicitado</returns>
        public static Model.SistemaCliente Get(Guid chave)
        {
            string cacheKey = "SistemaCliente:" + chave.ToString();
            Model.SistemaCliente sistema = (Model.SistemaCliente)Cache.CacheManager.GetCached(cacheKey);

            if (sistema != null)
                return sistema;

            using (var _context = new APIGatewayContext())
            {
                sistema = (from s in _context.SistemaCliente
                           where s.Chave == chave
                           select s).FirstOrDefault();
            }

            Cache.CacheManager.Cache(cacheKey, sistema);

            return sistema;
        }

        /// <summary>
        /// Metodo para pesquisa de sistemas
        /// </summary>
        /// <param name="nome">Nome do sistema pesquisado</param>
        /// <returns>Objeto do sistema solicitado</returns>
        public static Model.SistemaCliente Get(String nome)
        {
            using (var _ctx = new Data.APIGatewayContext())
            {
                return (from s in _ctx.SistemaCliente
                        where s.Nome == nome
                        select s).FirstOrDefault();

            }
        }

        /// <summary>
        /// Adiciona novo sistema
        /// </summary>
        /// <param name="NomeSistema">Nome do novo sistema a ser incluso</param>
        /// <returns></returns>
        public static Model.SistemaCliente Add(String NomeSistema)
        {
            try
            {
                if (String.IsNullOrEmpty(NomeSistema))
                    throw new ArgumentException("Nome Sistema não pode ser null ou vazio");

                Model.SistemaCliente objSistema = new Model.SistemaCliente()
                {
                    Chave = Guid.NewGuid(),
                    Nome = NomeSistema
                };

                using (var _ctx = new Data.APIGatewayContext())
                {
                    //if (_ctx.SistemaCliente.Count(s => s.Nome == NomeSistema) > 0)
                    //     throw new Exception("Já existe um Sistema com o nome " + NomeSistema);

                    _ctx.SistemaCliente.Add(objSistema);
                    _ctx.Commit();
                }

                UsuarioSistemaCliente.Create(objSistema);

                return objSistema;
            }
            catch (Exception ex)
            {
                throw BNE.Core.Exceptions.SqlExceptionsHandler.TratarExcecao(ex);
            }
        }

        /// <summary>
        /// Atualiza sistema
        /// </summary>
        /// <param name="chave">Chave do sistema a ser atualizado</param>
        /// <param name="nomeSistema">Nome do novo sistema a ser incluso</param>
        /// <returns></returns>
        public static Model.SistemaCliente Update(Guid chave, String nomeSistema)
        {
            if (String.IsNullOrEmpty(nomeSistema))
                throw new ArgumentException("Nome Sistema não pode ser null ou vazio");

            using (var _ctx = new Data.APIGatewayContext())
            {
                Model.SistemaCliente objSistema = _ctx.SistemaCliente.FirstOrDefault(s => s.Chave == chave);

                if (objSistema == null)
                    throw new ObjectNotFoundException(String.Format("Sistema com chave {0} não encontrado", chave));

                _ctx.Commit();

                return objSistema;
            }
        }

        /// <summary>
        /// Deleta sistema
        /// </summary>
        /// <param name="chave">Chave do sistema a ser atualizado</param>
        /// <returns></returns>
        public static void Delete(Guid chave)
        {
            using (var _ctx = new Data.APIGatewayContext())
            {
                Model.SistemaCliente objSistema = _ctx.SistemaCliente.FirstOrDefault(s => s.Chave == chave);

                try
                {
                    if (objSistema == null)
                        throw new ObjectNotFoundException(String.Format("Sistema com chave {0} não encontrado", chave));

                    _ctx.SistemaCliente.Remove(objSistema);
                    _ctx.Commit();
                }
                catch (Exception ex)
                {
                    throw BNE.Core.Exceptions.SqlExceptionsHandler.TratarExcecao(ex);
                }

            }

        }
    }
}
