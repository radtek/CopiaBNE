using LanHouse.Entities.BNE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanHouse.Business
{
    public class Plano
    {
        #region CarregarPorTipoCodigoDesconto
        public static bool CarregarPorTipoCodigoDesconto(int idTipoCodigoDesconto, out List<BNE_Plano> lstCodigoDescontoPlano)
        {
            using (var entity = new LanEntities())
            {
                lstCodigoDescontoPlano = (from planos in entity.BNE_Plano.Include("BNE_Tipo_Codigo_Desconto")
                                          join codDescPlano in entity.BNE_Codigo_Desconto_Plano on planos.Idf_Plano equals codDescPlano.Idf_Plano
                                          where codDescPlano.Idf_Tipo_Codigo_Desconto == idTipoCodigoDesconto
                                          select planos).ToList();

                return lstCodigoDescontoPlano != null;
            }

        }
        #endregion
    }
}
