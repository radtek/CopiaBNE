using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BNE.Web.LanHouse.EntityFramework;

namespace BNE.Web.LanHouse.BLL
{
    public static class Endereco
    {
        private static LanEntities entity = new LanEntities();

        public static bool CarregarCidadePorId(int idEndereco, out TAB_Cidade objCidade)
        {
            bool retorno = false;
            objCidade = null;

            TAB_Endereco objEndereco =
                entity
                    .TAB_Endereco
                    .Include("TAB_Cidade")
                    .FirstOrDefault(e => e.Idf_Endereco == idEndereco);

            if (objEndereco != null && objEndereco.TAB_Cidade != null)
            {
                objCidade = objEndereco.TAB_Cidade;
                retorno = true;
            }

            return retorno;
        }
    }
}