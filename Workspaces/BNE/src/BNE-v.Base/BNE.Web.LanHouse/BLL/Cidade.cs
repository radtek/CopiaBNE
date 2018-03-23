using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using BNE.Web.LanHouse.EntityFramework;

namespace BNE.Web.LanHouse.BLL
{
    public static class Cidade
    {
        private static LanEntities entity = new LanEntities();

        public static bool CarregarPorDescricao(string descricao, out IEnumerable<TAB_Cidade> objCidades)
        {
            if (descricao == null)
                descricao = string.Empty;

            string uf = null;
            if (descricao.IndexOf('/') > 0 || descricao.IndexOf('\\') > 0)
            {
                string[] partes = descricao.Split('/', '\\');

                if (partes.Length != 2)
                    throw new ArgumentException("Descrição não pode ter mais que uma barra para separar cidade da UF", "descricao");

                descricao = partes[0];
                uf = partes[1].ToUpper();
            }

            Expression<Func<TAB_Cidade, bool>> predicate;
            if (!String.IsNullOrEmpty(uf))
                predicate = c => c.Nme_Cidade.StartsWith(descricao) && c.Sig_Estado.Equals(uf);
            else
                predicate = c => c.Nme_Cidade.StartsWith(descricao);

            objCidades =
                entity
                    .TAB_Cidade
                    .Where(predicate)
                    .ToList();

            return objCidades.Any();
        }

        public static bool CarregarPorId(int id, out TAB_Cidade objCidade)
        {
            objCidade =
                entity
                    .TAB_Cidade
                    .FirstOrDefault(c => c.Idf_Cidade == id);

            return objCidade != null;
        }
    }
}