using LanHouse.Entities.BNE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanHouse.Business
{
    public static class Companhia
    {

        #region RecuperarIdf_Companhia
        /// <summary>
        /// Recuperar Idf_Companhia pelo id da Filial
        /// </summary>
        /// <param name="idFilial"></param>
        /// <returns></returns>
        public static int RecuperarIdf_Companhia(int idFilial)
        {
            int idCompanhia = 0;

            using(var context = new LanEntities())
            {
                idCompanhia = context.LAN_Companhia.Where(c => c.TAB_Filial.Idf_Filial == idFilial).Select(c => c.Idf_Companhia).FirstOrDefault();
            }

            return idCompanhia;
        }

        #endregion

        #region RecuperarLogo
        /// <summary>
        /// Carregar a logo da LanHouse
        /// </summary>
        /// <param name="numeroCNPJ"></param>
        /// <returns></returns>
        public static byte[] RecuperarLogo(decimal numeroCNPJ)
        {
            using (var entity = new LanEntities())
            {

                entity.Configuration.LazyLoadingEnabled = true;

                var lan = entity.LAN_Companhia.FirstOrDefault(c => c.Num_CNPJ == numeroCNPJ);

                if (lan != null && lan.Img_Logo != null)
                    return lan.Img_Logo;

                var bne = entity.LAN_Companhia.FirstOrDefault(f => f.TAB_Filial.Num_CNPJ == numeroCNPJ);

                if (bne != null && bne.TAB_Filial.TAB_Filial_Logo.Count > 0 && bne.TAB_Filial.TAB_Filial_Logo.FirstOrDefault().Flg_Inativo.Equals(false))
                    return bne.TAB_Filial.TAB_Filial_Logo.FirstOrDefault().Img_Logo;

                var filial = entity.TAB_Filial.Where(f => f.Num_CNPJ == numeroCNPJ).FirstOrDefault();

                if(filial != null)
                {
                    return entity.TAB_Filial_Logo.Where(l => l.Idf_Filial == filial.Idf_Filial && l.Flg_Inativo.Equals(false)).FirstOrDefault().Img_Logo;
                }

                return null;
            }
        }
        #endregion
    }
}
