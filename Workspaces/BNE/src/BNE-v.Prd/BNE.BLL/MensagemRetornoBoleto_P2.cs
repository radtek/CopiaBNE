//-- Data: 28/04/2014 09:56
//-- Autor: Francisco Ribas

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
namespace BNE.BLL
{
	public partial class MensagemRetornoBoleto // Tabela: GLO_Mensagem_Retorno_Boleto
    {
        #region Consultas
        private const string SP_CODIGO_MENSAGEM = @"SELECT * FROM BNE.GLO_Mensagem_Retorno_Boleto
                                                    WHERE Cod_Status = @Cod_Status;";
        #endregion

        #region Metodos
        public static bool RecuperarPorCodigo(string codigoMensagem, out MensagemRetornoBoleto objMensagemRetornoBoleto)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Cod_Status", SqlDbType.VarChar, 10));
            parms[0].Value = codigoMensagem;
            objMensagemRetornoBoleto = new MensagemRetornoBoleto();

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SP_CODIGO_MENSAGEM, parms))
            {
                if (SetInstance(dr, objMensagemRetornoBoleto))
                {
                    return true;
                }
            }

            return false;
        }
        #endregion
    }
}