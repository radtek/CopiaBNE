using APIGateway.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIGateway.Domain
{
    public class UsuarioPessoaFisica
    {
        private static string GetCacheKey(decimal CPF)
        {
            return String.Format("UsuarioPessoaFisica:{0}", CPF.ToString());
        }

        public static Model.UsuarioPessoaFisica Obter(decimal CPF)
        {
            string cacheKey = GetCacheKey(CPF);
            Model.UsuarioPessoaFisica upj = (Model.UsuarioPessoaFisica)Cache.CacheManager.GetCached(cacheKey);

            if (upj != null)
                return upj;

            using (var _context = new APIGatewayContext())
            {
                upj = (from pj in _context.UsuarioPessoaFisica
                       where pj.CPF == CPF
                       select pj).FirstOrDefault();
            }

            if (upj == null)
                upj = Add(CPF);

            Cache.CacheManager.Cache(cacheKey, upj);

            return upj;
        }

        public static Model.UsuarioPessoaFisica Add(decimal CPF)
        {
            using (var _context = new APIGatewayContext())
            {
                Model.UsuarioPessoaFisica upj = _context.UsuarioPessoaFisica.Add(new Model.UsuarioPessoaFisica(CPF));
                _context.Commit();
                return upj;
            }
        }
    }
}
