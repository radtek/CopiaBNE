//-- Data: 06/03/2013 10:06
//-- Autor: Gieyson Stelmak

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
namespace BNE.BLL
{
	public partial class TransacaoMensagemErro // Tabela: TAB_Transacao_Mensagem_Erro
    {

        #region Consultas

        private const string Spretornarmensagempordescricaoerro = @"SELECT Des_Mensagem_Amigavel FROM TAB_Transacao_Mensagem_Erro WITH(NOLOCK) WHERE Des_Descricao_Erro LIKE @Des_Descricao_Erro";
        private const string Spretornarmensagemporcodigooerro = @"SELECT Des_Mensagem_Amigavel FROM TAB_Transacao_Mensagem_Erro WITH(NOLOCK) WHERE Des_Codigo_Erro LIKE @Des_Codigo_Erro";

        #endregion

        #region RecuperarMensagemPorDescricaoErro
        public static string RecuperarMensagemPorDescricaoErro(string descricaoErro)
        {
            var parms = new List<SqlParameter>
                {
					new SqlParameter { ParameterName = "@Des_Descricao_Erro", SqlDbType = SqlDbType.VarChar, Size = 200, Value = descricaoErro.Trim() }
                };

            return Convert.ToString(DataAccessLayer.ExecuteScalar(CommandType.Text, Spretornarmensagempordescricaoerro, parms));
        }
        #endregion

        #region RecuperarMensagemPorCodigoErro
        public static string RecuperarMensagemPorCodigoErro(string codigoErro)
        {
            var parms = new List<SqlParameter>
                {
					new SqlParameter { ParameterName = "@Des_Codigo_Erro", SqlDbType = SqlDbType.VarChar, Size = 10, Value = codigoErro.Trim() }
                };

            return Convert.ToString(DataAccessLayer.ExecuteScalar(CommandType.Text, Spretornarmensagemporcodigooerro, parms));
        }
        #endregion

    }
}