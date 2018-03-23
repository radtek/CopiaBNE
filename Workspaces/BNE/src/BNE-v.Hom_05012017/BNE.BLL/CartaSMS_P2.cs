//-- Data: 02/04/2013 15:45
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BNE.BLL
{
    public partial class CartaSMS // Tabela: BNE_Carta_SMS
    {

        #region Consultas

        #region Spselectvalor
        private const string Spselectvalor = "SELECT Vlr_Carta_SMS FROM BNE_Carta_SMS WITH(NOLOCK) WHERE Idf_Carta_SMS = @Idf_Carta_SMS";
        #endregion

        #endregion

        #region RecuperaValorConteudo
        /// <summary>
        /// Método que recupera o valor de um conteúdo a partir do id.
        /// </summary>
        /// <param name="carta">Identificador do conteúdo.</param>
        /// <param name="trans">Transação com o banco de dados.</param>
        /// <returns>Valor do conteúdo.</returns>
        public static string RecuperaValorConteudo(Enumeradores.CartaSMS carta, SqlTransaction trans)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Carta_SMS", SqlDbType = SqlDbType.Int, Size = 4, Value = (int)carta }
                };

            IDataReader dr;
            if (trans != null)
                dr = DataAccessLayer.ExecuteReader(trans, CommandType.Text, Spselectvalor, parms);
            else
                dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spselectvalor, parms);

            string valorConteudo = String.Empty;

            if (dr.Read())
                valorConteudo = Convert.ToString(dr["Vlr_Carta_SMS"]);

            //Fechando o IDataReader caso esteja aberto.
            if (!dr.IsClosed)
                dr.Close();

            //Disposing o IDataReader.
            dr.Dispose();

            return valorConteudo;
        }
        public static string RecuperaValorConteudo(Enumeradores.CartaSMS carta)
        {
            return RecuperaValorConteudo(carta, null);
        }
        #endregion

    }
}