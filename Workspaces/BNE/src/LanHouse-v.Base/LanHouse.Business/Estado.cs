using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanHouse.Entities.BNE;

namespace LanHouse.Business
{
    public class Estado
    {
        #region CarregarEstadoporSigla
        public static bool CarregarEstadoporSigla(string siglaEstado, out TAB_Estado objEstado)
        {
            using (var entity = new LanEntities())
            {
                objEstado = entity.TAB_Estado.FirstOrDefault(e => e.Sig_Estado == siglaEstado);
                return objEstado != null;
            }
        }
        #endregion
    }
}
