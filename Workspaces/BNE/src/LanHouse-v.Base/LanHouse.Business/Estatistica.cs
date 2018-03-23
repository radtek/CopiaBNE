using LanHouse.Entities.BNE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanHouse.Business
{
    public class Estatistica
    {
        public static bool CarregarEstatistica(out BNE_Estatistica objEstatistica)
        {
            using(var entity = new LanEntities())
            {
                objEstatistica = entity.BNE_Estatistica.OrderByDescending(item => item.Dta_Cadastro).FirstOrDefault();
                return objEstatistica != null;
            }
        }
    }
}
