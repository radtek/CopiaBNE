using LanHouse.Entities.BNE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanHouse.Business
{
    public class CodigoDescontoPlano
    {
        #region CarregarPorTipoCodigoDesconto
        public static bool CarregarPorTipoCodigoDesconto(int idTipoCodigoDesconto, out List<BNE_Codigo_Desconto_Plano> lstCodigoDescontoPlano)
        {
            using (var entity = new LanEntities())
            {
                lstCodigoDescontoPlano = (from codDescPlano in entity.BNE_Codigo_Desconto_Plano
                                          where codDescPlano.Idf_Tipo_Codigo_Desconto == idTipoCodigoDesconto
                                     select codDescPlano).ToList();

                return lstCodigoDescontoPlano != null;

            }
        }
        #endregion

        #region CarregarPorTipoCodigoDesconto
        public static BNE_Codigo_Desconto_Plano CarregarPorPlanoETipoCodigoDesconto(int idTipoCodigoDesconto, int idPlano)
        {
            using (var entity = new LanEntities())
            {
                BNE_Codigo_Desconto_Plano objCodigoDescontoPlano;

                objCodigoDescontoPlano = (from codDescPlano in entity.BNE_Codigo_Desconto_Plano
                                          where codDescPlano.Idf_Tipo_Codigo_Desconto == idTipoCodigoDesconto
                                          && codDescPlano.Idf_Plano == idPlano
                                          select codDescPlano).FirstOrDefault();

                return objCodigoDescontoPlano;
            }

        }
        #endregion
    }
}
