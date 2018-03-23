using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanHouse.Entities.BNE;
using System.Collections;
using LanHouse.Entities.BNE.Repositories;

namespace LanHouse.Business
{
    public class AreaBNE
    {
        #region ListarAreasBNE
        public static IList ListarAreasBNE()
        {
            using (var entity = new LanEntities())
            {
                var query = (
                    from area in entity.TAB_Area_BNE
                    where area.Flg_Inativo == false
                    orderby area.Des_Area_BNE ascending
                    select new { id = area.Idf_Area_BNE, text = area.Des_Area_BNE });

                return query.ToList();
            }
        }
        #endregion
    }
}
