using LanHouse.Entities.BNE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanHouse.Business
{
    public class CartaSMS
    {
        #region RecuperaValorConteudo
        /// <summary>
        /// Método que recupera o valor de um conteúdo a partir do id.
        /// </summary>
        /// <param name="carta">Identificador do conteúdo.</param>
        /// <param name="context">Transação com o banco de dados.</param>
        /// <returns>Valor do conteúdo.</returns>
        public static string RecuperaValorConteudo(int idCarta, LanEntities context)
        {

            var conteudo = (from carta in context.BNE_Carta_SMS
                             where carta.Idf_Carta_SMS == idCarta
                             select carta.Vlr_Carta_SMS).FirstOrDefault();

            if (conteudo != null)
                return conteudo;

            return "";
        }
        #endregion
    }
}
