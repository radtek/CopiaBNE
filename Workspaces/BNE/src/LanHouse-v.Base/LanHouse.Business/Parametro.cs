using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanHouse.Entities.BNE;
using System.Collections;
using LanHouse.Business.EL;
using LanHouse.Entities.BNE.Repositories;
using LanHouse.Entities.BNE.Infrastructure;

namespace LanHouse.Business
{
    public class Parametro
    {
        private static DatabaseFactory dbFactory = new DatabaseFactory();

        #region GetById
        public Entities.BNE.TAB_Parametro GetById(int id)
        {
            Entities.BNE.TAB_Parametro objParametro;

            using (var entity = new LanEntities())
            {
               objParametro = (from par in entity.TAB_Parametro
                                 where par.Idf_Parametro == id
                                 select par).SingleOrDefault();

                if (objParametro == null)
                {
                    throw new RecordNotFoundException(typeof(Entities.BNE.TAB_Parametro));
                }

                return objParametro;
            }
        }
        #endregion

        #region ListarParametros
        public static Dictionary<Business.Enumeradores.Parametro, string> ListarParametros(List<Int32> idsParametros)
        {
            using (var entity = new LanEntities())
            {
                return entity.TAB_Parametro
                    .Where(par => idsParametros
                        .Contains(par.Idf_Parametro))
                        .Select(t => new { t.Idf_Parametro, t.Vlr_Parametro })
                        .ToDictionary(t => (Business.Enumeradores.Parametro)Enum.Parse(typeof(Business.Enumeradores.Parametro), t.Idf_Parametro.ToString()), t => t.Vlr_Parametro);
            }
        }
        #endregion
    }
}
