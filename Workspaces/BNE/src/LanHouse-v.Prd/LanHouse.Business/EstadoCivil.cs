using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanHouse.Entities.BNE;
using System.Collections;

namespace LanHouse.Business
{
    public class EstadoCivil
    {
        #region CarregarEstadoCivilporId
        /// <summary>
        /// Carregar um estado civil pelo Id
        /// </summary>
        /// <param name="idEstadoCivil"></param>
        /// <returns></returns>
        public static TAB_Estado_Civil CarregarEstadoCivilporId(int? idEstadoCivil)
        {
            using (var entity = new LanEntities())
            {
                var query = (
                    from estCiv in entity.TAB_Estado_Civil
                    where estCiv.Idf_Estado_Civil == idEstadoCivil
                    select estCiv).SingleOrDefault();

                return query;
            }
        }
        #endregion

        #region ListarEstadoCivil
        /// <summary>
        /// Listar todos os estados cívis
        /// </summary>
        /// <returns></returns>
        public static IList ListarEstadoCivil()
        {
            using (var entity = new LanEntities())
            {
                var query = (
                    from estCiv in entity.TAB_Estado_Civil
                    where estCiv.Flg_Inativo == false
                    orderby estCiv.Des_Estado_Civil ascending
                    select new { id = estCiv.Idf_Estado_Civil, text = estCiv.Des_Estado_Civil });

                return query.ToList();
            }
        }
        #endregion
    }
}
