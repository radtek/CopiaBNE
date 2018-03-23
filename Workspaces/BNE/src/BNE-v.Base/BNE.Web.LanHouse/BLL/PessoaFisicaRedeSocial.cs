using System.Linq;
using BNE.Web.LanHouse.EntityFramework;

namespace BNE.Web.LanHouse.BLL
{
    public static class PessoaFisicaRedeSocial
    {

        #region Campos
        private static LanEntities entity = new LanEntities();
        #endregion Campos

        #region CarregarPorFacebook
        public static bool CarregarPorFacebook(string id, out TAB_Pessoa_Fisica_Rede_Social objPessoaFisicaRedeSocial)
        {
            objPessoaFisicaRedeSocial =
                entity
                    .TAB_Pessoa_Fisica_Rede_Social
                    .FirstOrDefault(pfrs => pfrs.Cod_Interno_Rede_Social == id && pfrs.Idf_Rede_Social_CS == 4 /*Facebook*/);

            return objPessoaFisicaRedeSocial != null;
        }
        #endregion

    }
}