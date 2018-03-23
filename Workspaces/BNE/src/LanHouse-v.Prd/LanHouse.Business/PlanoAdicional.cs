using LanHouse.Entities.BNE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanHouse.Business
{
    public class PlanoAdicional
    {
        #region LiberarPlanoAdicional
        /// <summary>
        /// Libera plano adicional e, caso seja de SMS, recarrega SMS
        /// </summary>
        public static void LiberarPlanoAdicional(int idPlanoAdicional, LanEntities context)
        {
            BNE_Adicional_Plano objPlanoAdicional;

            objPlanoAdicional = (from planoAdc in context.BNE_Adicional_Plano
                                 where planoAdc.Idf_Adicional_Plano == idPlanoAdicional
                                 select planoAdc).FirstOrDefault();

            if (objPlanoAdicional.Idf_Adicional_Plano.Equals((int)Enumeradores.TipoAdicional.SMSAdicional))
                PlanoQuantidade.RecarregarSMS(objPlanoAdicional);

            objPlanoAdicional.Idf_Adicional_Plano_Situacao = (int)Enumeradores.AdicionalPlanoSituacao.Liberado;
            context.SaveChanges();
        }
        #endregion
    }
}
