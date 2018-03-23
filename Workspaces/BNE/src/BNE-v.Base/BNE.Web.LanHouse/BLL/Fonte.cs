using System.Collections.Generic;
using System.Linq;
using BNE.Web.LanHouse.EntityFramework;

namespace BNE.Web.LanHouse.BLL
{
    public static class Fonte
    {
        private static LanEntities entity = new LanEntities();

        public static bool CarregarPorDescricaoOuSigla(string descricao, out IEnumerable<TAB_Fonte> objFontes)
        {
            objFontes =
                entity
                    .TAB_Fonte
                    .Where(f => f.Sig_Fonte.Contains(descricao) || f.Nme_Fonte.StartsWith(descricao))
                    .OrderBy(f => f.Nme_Fonte)
                    .ToList();

            return objFontes.Any();
        }

        public static bool CarregarPorId(int id, out TAB_Fonte objFonte)
        {
            objFonte =
                entity
                    .TAB_Fonte
                    .FirstOrDefault(f => f.Idf_Fonte == id);

            return objFonte != null;
        }
    }
}