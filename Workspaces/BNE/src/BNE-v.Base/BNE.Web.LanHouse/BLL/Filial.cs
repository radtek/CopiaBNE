using BNE.Web.LanHouse.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BNE.Web.LanHouse.BLL
{
    public class Filial
    {

        private static LanEntities entity = new LanEntities();

        #region SelectByID
        public static TAB_Filial SelectByID(int id)
        {
            return entity.TAB_Filial.FirstOrDefault(f => f.Idf_Filial == id);
        }
        #endregion


    }
}