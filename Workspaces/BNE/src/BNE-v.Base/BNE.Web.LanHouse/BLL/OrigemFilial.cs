using System.Linq;
using BNE.Web.LanHouse.EntityFramework;

namespace BNE.Web.LanHouse.BLL
{
    public static class OrigemFilial
    {
        private static LanEntities entity = new LanEntities();

        public static bool CarregarPorDiretorio(string diretorio, out TAB_Origem_Filial objOrigemFilial)
        {
            objOrigemFilial =
                entity
                    .TAB_Origem_Filial
                    .Include("TAB_Filial").FirstOrDefault(of => of.Des_Diretorio.Equals(diretorio));

            bool retorno = objOrigemFilial != null;

            return retorno;
        }

        public static bool CarregarPorFilial(TAB_Filial objFilial, out TAB_Origem_Filial objOrigemFilial)
        {
            objOrigemFilial =
                entity
                    .TAB_Origem_Filial
                    .Include("TAB_Filial").FirstOrDefault(of => of.Idf_Filial == objFilial.Idf_Filial);

            bool retorno = objOrigemFilial != null;

            return retorno;
        }

    }
}