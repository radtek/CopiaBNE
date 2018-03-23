using LanHouse.Entities.BNE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanHouse.Business
{
    public class PlanoQuantidade
    {
        #region InserirPlanoQuantidade
        public static bool InserirPlanoQuantidade(BNE_Plano objPlano, BNE_Plano_Adquirido objPlanoAdquirido)
        {

            try
            {
                using (var entity = new LanEntities())
                {
                    entity.BNE_Plano_Quantidade.Add(new BNE_Plano_Quantidade()
                    {
                        Dta_Inicio_Quantidade = objPlanoAdquirido.Dta_Inicio_Plano,
                        Dta_Fim_Quantidade = objPlanoAdquirido.Dta_Inicio_Plano.AddDays(objPlano.Qtd_Dias_Validade),
                        Flg_Inativo = false,
                        Idf_Plano_Adquirido = objPlanoAdquirido.Idf_Plano_Adquirido,
                        Qtd_SMS = 0,
                        Qtd_SMS_Utilizado = 0,
                        Qtd_Visualizacao = objPlano.Qtd_Visualizacao,
                        Qtd_Visualizacao_Utilizado = 0
                    });
                    entity.SaveChanges();
                }
                return true;
            }
            catch
            {
                return false;
            }

        }
        #endregion

        #region CarregarPorPlano
        /// <summary>   
        /// Método responsável por carregar uma instância de PlanoQuantidade vigente por uma filial
        /// </summary>
        /// <returns>Objeto Plano Quantidade</returns>
        public static bool AtualizarQtdeSMS(int idPlanoAdquirido, int qtdeARecarregar)
        {
            try
            {
                using (var entity = new LanEntities())
                {
                    var objPlanoQuantidade = (from planoAdquirido in entity.BNE_Plano_Adquirido
                                          join planoQtde in entity.BNE_Plano_Quantidade on planoAdquirido.Idf_Plano_Adquirido equals planoQtde.Idf_Plano_Adquirido
                                          where planoAdquirido.Idf_Plano_Adquirido == idPlanoAdquirido
                                          select planoQtde).FirstOrDefault();

                    objPlanoQuantidade.Qtd_SMS += qtdeARecarregar;
                    entity.SaveChanges();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region RecarregarSMS
        /// <summary>
        /// Recarrega SMS
        /// </summary>
        /// <param name="idPlanoAdquirido">objeto PlanoAdquirido</param>
        /// <param name="qtdeARecarregar">quantidade de SMSs a recarregar</param>
        /// <param name="trans">transação SQL Server</param>
        /// <returns>true se conseguiu, false se não</returns>
        public static bool RecarregarSMS(BNE_Plano_Adquirido objPlanoAdquirido, int qtdeARecarregar)
        {
            return PlanoQuantidade.AtualizarQtdeSMS(objPlanoAdquirido.Idf_Plano_Adquirido, qtdeARecarregar);
        }

        /// <summary>
        /// Recarrega SMS
        /// </summary>
        /// <param name="objAdicionalPlano">objeto AdicionalPlano</param>
        /// <param name="trans">transação SQL Server</param>
        /// <returns>true se conseguiu, false se não</returns>
        public static bool RecarregarSMS(BNE_Adicional_Plano objAdicionalPlano)
        {
            return RecarregarSMS(objAdicionalPlano.BNE_Plano_Adquirido, objAdicionalPlano.Qtd_Adicional);
        }
        #endregion
    }
}
