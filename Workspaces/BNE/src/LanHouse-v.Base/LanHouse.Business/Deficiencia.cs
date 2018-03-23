using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanHouse.Entities.BNE;
using System.Collections;

namespace LanHouse.Business
{
    public class Deficiencia
    {

        #region CarregarDescricaoDeficieciaporId

        public static string CarregarDescricaoDeficieciaporId(int? idDeficiencia)
        {
            using (var entity = new LanEntities())
            {
                var query = (
                    from def in entity.TAB_Deficiencia
                    where def.Idf_Deficiencia == idDeficiencia
                    select def.Des_Deficiencia).FirstOrDefault();

                return query;
            }
        }

        #endregion

        #region ListarDeficiencias
        public static IList ListarDeficiencias()
        {
            using (var entity = new LanEntities())
            {
                var query = (
                    from def in entity.TAB_Deficiencia
                    where def.Flg_Inativo == false //&& def.Idf_Deficiencia > 0
                    orderby def.Des_Deficiencia ascending
                    select new { id = def.Idf_Deficiencia, text = def.Des_Deficiencia });

                return query.ToList();
            }
        }
        #endregion
    }
}