using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanHouse.Entities.BNE;
using System.Collections;

namespace LanHouse.Business
{
    public class Idioma
    {
        #region ListarIdiomas
        /// <summary>
        /// Listar todos os idiomas
        /// </summary>
        /// <returns></returns>
        public static IList ListarIdiomas()
        {
            using (var entity = new LanEntities())
            {
                var query = (
                    from idi in entity.TAB_Idioma
                    where idi.Flg_Inativo == false
                    orderby idi.Des_Idioma ascending
                    select new { idIdioma = idi.Idf_Idioma, text = idi.Des_Idioma });

                return query.ToList();
            }
        }
        #endregion
    }
}
