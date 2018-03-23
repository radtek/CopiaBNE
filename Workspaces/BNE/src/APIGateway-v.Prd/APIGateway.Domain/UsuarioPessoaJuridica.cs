using APIGateway.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIGateway.Domain
{
    public class UsuarioPessoaJuridica
    {
        private static string GetCacheKey(decimal CPF, decimal CNPJ)
        {
            return String.Format("UsuarioPessoaJuridica:{0}:{1}", CPF.ToString(), CNPJ.ToString());
        }

        public static Model.UsuarioPessoaJuridica Obter(decimal CPF, decimal CNPJ)
        {
            string cacheKey = GetCacheKey(CPF, CNPJ);
            Model.UsuarioPessoaJuridica upj = (Model.UsuarioPessoaJuridica)Cache.CacheManager.GetCached(cacheKey);

            if (upj != null)
                return upj;

            using (var _context = new APIGatewayContext())
            {
                upj = (from pj in _context.UsuarioPessoaJuridica
                       where pj.CPF == CPF && pj.CNPJ == CNPJ
                       select pj).FirstOrDefault();
            }

            if (upj == null)
                upj = Add(CPF, CNPJ);

            Cache.CacheManager.Cache(cacheKey, upj);

            return upj;
        }

        public static Model.UsuarioPessoaJuridica Add(decimal CPF, decimal CNPJ)
        {
            using (var _context = new APIGatewayContext())
            {
                Model.UsuarioPessoaJuridica upj = _context.UsuarioPessoaJuridica.Add(new Model.UsuarioPessoaJuridica(CPF, CNPJ));
                _context.Commit();
                return upj;
            }
        }
    }
}
