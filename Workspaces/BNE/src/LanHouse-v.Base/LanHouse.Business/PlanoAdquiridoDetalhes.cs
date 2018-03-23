using LanHouse.Entities.BNE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanHouse.Business
{
    public class PlanoAdquiridoDetalhes
    {
        public static bool InserirPlanoAdquiridoDetalhes(BNE_Plano objPlano, BNE_Plano_Adquirido objPlanoAdquirido, BNE_Usuario_Filial objUsuarioFilial, TAB_Usuario_Filial_Perfil objUsuarioFilialPerfil)
        {
            try
            {
                using (var entity = new LanEntities())
                {
                    entity.BNE_Plano_Adquirido_Detalhes.Add(new BNE_Plano_Adquirido_Detalhes()
                    {
                        Eml_Envio_Boleto = objUsuarioFilial.Eml_Comercial,
                        Flg_Nota_Fiscal = true,
                        Nme_Res_Plano_Adquirido = objUsuarioFilialPerfil.TAB_Pessoa_Fisica.Nme_Pessoa,
                        Num_Res_DDD_Telefone = objUsuarioFilial.Num_DDD_Comercial,
                        Num_Res_Telefone = objUsuarioFilial.Num_Comercial,
                        Idf_Funcao = objUsuarioFilial.Idf_Funcao,
                        Idf_Plano_Adquirido = objPlanoAdquirido.Idf_Plano_Adquirido,
                        Idf_Filial_Gestora = Convert.ToInt32(new Business.Parametro().GetById(Convert.ToInt32(Enumeradores.Parametro.FilialGestoraPadraoDoPlano)).Vlr_Parametro)
                    });
                    entity.SaveChanges();

                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
