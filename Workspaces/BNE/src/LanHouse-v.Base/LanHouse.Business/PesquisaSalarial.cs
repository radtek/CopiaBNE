using LanHouse.Entities.BNE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanHouse.Business
{
    public class PesquisaSalarial
    {
        #region CarregarMedialSalarialFuncao
        public static bool CarregarMedialSalarialFuncao(int idFuncao, int idEstado, out TAB_Pesquisa_Salarial objPesquisaSalarial)
        {
            using (var entity = new LanEntities())
            {
                objPesquisaSalarial =
                    entity.TAB_Pesquisa_Salarial
                    .FirstOrDefault(p => p.Idf_Estado == idEstado && p.Idf_Funcao == idFuncao);

                return objPesquisaSalarial != null;
            }
        }
        #endregion
    }
}
