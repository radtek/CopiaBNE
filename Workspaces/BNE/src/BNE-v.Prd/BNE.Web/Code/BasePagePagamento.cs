using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BNE.BLL;
using EnumeradoresBNE = BNE.BLL.Enumeradores;

namespace BNE.Web.Code
{
    public class BasePagePagamento : BasePage
    {

        #region Propriedades

        #region StatusTransacaoCartao
        /// <summary>
        /// PropriedadeRetorna o status da transação de pagamento do tipo cartão.
        /// </summary>
        public bool? StatusTransacaoCartao
        {
            get
            {
                if (Session["StatusTrancao"] != null)
                    return Convert.ToBoolean(Session["StatusTrancao"]);
                return null;
            }
            set
            {
                Session["StatusTrancao"] = value;
            }
        }
        #endregion
        
        #endregion

        #region Métodos

        #region VerificaSeTemPlanoAdquiridoLiberadoEmVigencia
        /// <summary>
        /// Método valida se existe PlanoAdquirido Liberado para o usuario ou para filial em vigência, e retorna o objeto do plano liberado caso exista.
        /// </summary>
        /// <param name="listPlanoAdquirido"> </param>
        /// <param name="objPlanoAdquirido"></param>
        /// <returns></returns>
        public  bool VerificaSeTemPlanoAdquiridoLiberadoEmVigencia(List<PlanoAdquirido> listPlanoAdquirido, out PlanoAdquirido objPlanoAdquirido)
        {
            objPlanoAdquirido = listPlanoAdquirido.FirstOrDefault(p => p.PlanoSituacao.IdPlanoSituacao == (int)EnumeradoresBNE.PlanoSituacao.Liberado 
                                                                       &&  p.DataFimPlano.Date >= DateTime.Now.Date);
            if (objPlanoAdquirido != null)
                return true;

            return false;
        }
        #endregion

        #region VerificaSeTemPlanoAdquiridoLiberadoVencido
        /// <summary>
        /// Método valida se existe PlanoAdquirido Liberado para o usuario ou para filial, e ja retorna o objeto do plano liberado caso exista.
        /// </summary>
        /// <param name="listPlanoAdquirido"> </param>
        /// <param name="objPlanoAdquirido"></param>
        /// <returns></returns>
        public bool VerificaSeTemPlanoAdquiridoLiberadoVencido(List<PlanoAdquirido> listPlanoAdquirido, out PlanoAdquirido objPlanoAdquirido)
        {
            objPlanoAdquirido = listPlanoAdquirido.Where(p => p.PlanoSituacao.IdPlanoSituacao == (int)EnumeradoresBNE.PlanoSituacao.Liberado
                                                          && p.DataFimPlano.Date < DateTime.Now.Date)
                                                  .OrderByDescending(p => p.DataFimPlano).FirstOrDefault();
            if (objPlanoAdquirido != null)
                return true;

            return false;
        }
        #endregion

        #region VerificaSeTemPlanoAdquiridoLiberadoAindaNaoIniciado
        /// <summary>
        /// Método valida se existe PlanoAdquirido Liberado para o usuario ou para filial com a data de inicio superior a data atual, e retorna o objeto do plano liberado caso exista.
        /// </summary>
        /// <param name="listPlanoAdquirido"> </param>
        /// <param name="objPlanoAdquirido"></param>
        /// <returns></returns>
        public bool VerificaSeTemPlanoAdquiridoLiberadoAindaNaoIniciado(List<PlanoAdquirido> listPlanoAdquirido, out PlanoAdquirido objPlanoAdquirido)
        {
            objPlanoAdquirido = listPlanoAdquirido.FirstOrDefault(p => p.PlanoSituacao.IdPlanoSituacao == (int)EnumeradoresBNE.PlanoSituacao.Liberado
                                                                       && p.DataInicioPlano.Date >= DateTime.Now.Date);

            if (objPlanoAdquirido != null)
                return true;

            return false;
        }
        #endregion

        #endregion

    }
}