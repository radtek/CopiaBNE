using LanHouse.Business.Custom;
using LanHouse.Entities.BNE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanHouse.Business
{
    public class UsuarioFilial
    {
        #region LiberarCIA
        public static bool LiberarCIA(BNE_Plano_Adquirido objPlanoAdquirido, BNE_Plano_Parcela objPlanoParcela, out BNE_Usuario_Filial objUsuarioFilial, LanEntities context)
        {
            objUsuarioFilial = null;
            return true;

            if (CarregarUsuarioFilialPorUsuarioFilialPerfil(objPlanoAdquirido.Idf_Usuario_Filial_Perfil, out objUsuarioFilial))
            {
                if (!String.IsNullOrEmpty(objUsuarioFilial.Eml_Comercial)) //Só envia mensagem caso o usuário possua e-mail
                {
                    if (PlanoParcela.NumeroParcela(objPlanoAdquirido.Idf_Plano_Adquirido, objPlanoParcela.Idf_Plano_Parcela, context) == 1)
                    {
                        string emailRemetente = new Parametro().GetById(Convert.ToInt32(Enumeradores.Parametro.EmailMensagens)).Vlr_Parametro;

                        string assunto;
                        string template = CartaEmail.RetornarConteudoBNE(Enumeradores.CartaEmail.ConfirmacaoPagamentoCIA, out assunto);

                        var parametros = new
                        {
                            Nome = objPlanoAdquirido.TAB_Filial.Raz_Social,
                            DescricaoPlano = objPlanoAdquirido.BNE_Plano.Des_Plano
                        };
                        string mensagem = parametros.ToString(template);

                        MailController.Send(objUsuarioFilial.Eml_Comercial, emailRemetente, assunto, mensagem);
                    }


                }

                return true;
            }

            return false;
        }
        #endregion

        #region CarregarUsuarioFilialPorUsuarioFilialPerfil
        public static bool CarregarUsuarioFilialPorUsuarioFilialPerfil(int idUsuarioFilialPerfil, out BNE_Usuario_Filial objUsuarioFilial)
        {
            using (var entity = new LanEntities())
            {
                objUsuarioFilial = (from uf in entity.BNE_Usuario_Filial
                                    where uf.Idf_Usuario_Filial_Perfil == idUsuarioFilialPerfil
                                    select uf).FirstOrDefault();

                return objUsuarioFilial != null;
            }
        }
        #endregion
    }
}
