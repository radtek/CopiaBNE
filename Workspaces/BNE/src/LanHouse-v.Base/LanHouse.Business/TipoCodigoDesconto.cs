using LanHouse.Entities.BNE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanHouse.Business
{
    public class TipoCodigoDesconto
    {
        #region CarregarPorCodigo
        public static bool CarregarPorCodigo(int id, out BNE_Tipo_Codigo_Desconto objTipoCodigoDesconto)
        {
            using (var entity = new LanEntities())
            {
                objTipoCodigoDesconto = (from tipoCodDesc in entity.BNE_Tipo_Codigo_Desconto
                                     where tipoCodDesc.Idf_Tipo_Codigo_Desconto == id
                                     select tipoCodDesc).FirstOrDefault();

                return objTipoCodigoDesconto != null;
            }
        }
        #endregion
    }
}
