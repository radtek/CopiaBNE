using Employer.DAL;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace BNE.BLL.Security
{
    public class DataAccessLayer : Employer.BNE.DAL.DataAccessLayer
    {
        #region ConnectionString
        public static string CONN_STRING
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["plataforma"].ToString();
            }
        }
        #endregion

        static BNE.BLL.Security.ConfigAccessLayer _dal = new BNE.BLL.Security.ConfigAccessLayer();

        #region ExecuteNonQuery
        public static int ExecuteNonQuery(CommandType cmdType, string cmdText, IList<SqlParameter> cmdParms)
        {
            return ExecuteNonQueryComplet(_dal, null, cmdType, cmdText, cmdParms);
        }

        /// <summary>
        /// Método utilizado para executar um comando de escrita no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação corrente.</param>
        /// <param name="cmdType">Tipo do comando.</param>
        /// <param name="cmdText">Query ou nome da Stored Procedure.</param>
        /// <param name="cmdParms">Coleção de parâmetros SQL.</param>
        /// <returns>Inteiro com o resultado do método original.</returns>
        public static int ExecuteNonQuery(SqlTransaction trans, CommandType cmdType, string cmdText, List<SqlParameter> cmdParms)
        {
            return ExecuteNonQueryComplet(_dal, trans, cmdType, cmdText, cmdParms);
        }
        #endregion

        #region ExecuteNonQueryCmd
        public static SqlCommand ExecuteNonQueryCmd(CommandType cmdType, string cmdText, List<SqlParameter> cmdParms)
        {
            return ExecuteNonQueryCmdComplet(_dal, null, cmdType, cmdText, cmdParms);
        }


        public static SqlCommand ExecuteNonQueryCmd(SqlTransaction trans, CommandType cmdType, string cmdText, List<SqlParameter> cmdParms)
        {
            return ExecuteNonQueryCmdComplet(_dal, trans, cmdType, cmdText, cmdParms);
        }
        #endregion

        #region ExecuteReader
        /// <summary>
        /// Método utilizado para executar um comando de leitura no banco de dados.
        /// </summary>
        /// <param name="cmdType">Tipo do comando.</param>
        /// <param name="cmdText">Query ou nome da Stored Procedure.</param>
        /// <param name="cmdParms">Coleção de parâmetros SQL.</param>
        /// <param name="connStringKey">Connection string que deve ser usada.</param>
        /// <returns>SqlDataReader de retorno da consulta.</returns>
        public static SqlDataReader ExecuteReader(CommandType cmdType, string cmdText, List<SqlParameter> cmdParms)
        {
            return ExecuteReaderComplet(_dal, null, cmdType, cmdText, cmdParms, null);
        }
        /// <summary>
        /// Método utilizado para executar um comando de leitura no banco de dados.
        /// </summary>
        /// <param name="cmdType">Tipo do comando.</param>
        /// <param name="cmdText">Query ou nome da Stored Procedure.</param>
        /// <param name="cmdParms">Coleção de parâmetros SQL.</param>
        /// <param name="connStringKey">Connection string que deve ser usada.</param>
        /// <returns>SqlDataReader de retorno da consulta.</returns>
        public static SqlDataReader ExecuteReader(CommandType cmdType, string cmdText, List<SqlParameter> cmdParms, string connStringKey)
        {
            return ExecuteReaderComplet(_dal, null, cmdType, cmdText, cmdParms, connStringKey);
        }
        public static SqlDataReader ExecuteReader(SqlTransaction trans, CommandType cmdType, string cmdText, List<SqlParameter> cmdParms, string connStringKey)
        {
            return ExecuteReaderComplet(_dal, trans, cmdType, cmdText, cmdParms, connStringKey);
        }

        /// <summary>
        /// Método utilizado para executar um comando de leitura no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação corrente.</param>
        /// <param name="cmdType">Tipo do comando.</param>
        /// <param name="cmdText">Query ou nome da Stored Procedure.</param>
        /// <param name="cmdParms">Coleção de parâmetros SQL.</param>
        /// <returns>SqlDataReader de retorno da consulta.</returns>
        public static SqlDataReader ExecuteReader(SqlTransaction trans, CommandType cmdType, string cmdText, List<SqlParameter> cmdParms)
        {
            return ExecuteReaderComplet(_dal, trans, cmdType, cmdText, cmdParms, null);
        }
        #endregion

        #region ExecuteScalar
        /// <summary>
        /// Método utilizado para executar um comando de leitura no banco de dados.
        /// </summary>
        /// <param name="cmdType">Tipo do comando.</param>
        /// <param name="cmdText">Query ou nome da Stored Procedure.</param>
        /// <param name="cmdParms">Coleção de parâmetros SQL.</param>
        /// <returns>Objeto de retorno da consulta.</returns>
        public static object ExecuteScalar(CommandType cmdType, string cmdText, List<SqlParameter> cmdParms)
        {
            return ExecuteScalarComplet(_dal, null, cmdType, cmdText, cmdParms);
        }
        /// <summary>
        /// Método utilizado para executar um comando de leitura no banco de dados.
        /// </summary>
        /// <param name="cmdType">Tipo do comando.</param>
        /// <param name="cmdText">Query ou nome da Stored Procedure.</param>
        /// <param name="cmdParms">Coleção de parâmetros SQL.</param>
        /// <returns>Objeto de retorno da consulta.</returns>
        public static object ExecuteScalar(Employer.BNE.DAL.AbstractConfigAccessLayer _dal, CommandType cmdType, string cmdText, List<SqlParameter> cmdParms)
        {
            return ExecuteScalarComplet(_dal, null, cmdType, cmdText, cmdParms);
        }
        /// <summary>
        /// Método utilizado para executar um comando de leitura no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação corrente.</param>
        /// <param name="cmdType">Tipo do comando.</param>
        /// <param name="cmdText">Query ou nome da Stored Procedure.</param>
        /// <param name="cmdParms">Coleção de parâmetros SQL.</param>
        /// <returns>Objeto de retorno da consulta.</returns>
        public static object ExecuteScalar(SqlTransaction trans, CommandType cmdType, string cmdText, List<SqlParameter> cmdParms)
        {
            return ExecuteScalarComplet(_dal, trans, cmdType, cmdText, cmdParms);
        }
        #endregion

        #region ExecuteReaderDs

        /// <summary>
        /// Método utilizado para executar um comando de leitura no banco de dados.
        /// </summary>
        /// <param name="cmdType">Tipo do comando.</param>
        /// <param name="cmdText">Query ou nome da Stored Procedure.</param>
        /// <param name="cmdParms">Coleção de parâmetros SQL.</param>
        /// <param name="connStringKey">Connection string que deve ser utilizada.</param>
        /// <returns>DataSet de retorno da consulta.</returns>
        public static DataSet ExecuteReaderDs(CommandType cmdType, string cmdText, List<SqlParameter> cmdParms, string connStringKey)
        {
            return ExecuteReaderDsComplet(_dal, null, cmdType, cmdText, cmdParms, connStringKey);
        }
        /// <summary>
        /// Método utilizado para executar um comando de leitura no banco de dados.
        /// </summary>
        /// <param name="cmdType">Tipo do comando.</param>
        /// <param name="cmdText">Query ou nome da Stored Procedure.</param>
        /// <param name="cmdParms">Coleção de parâmetros SQL.</param>
        /// <returns>DataSet de retorno da consulta.</returns>
        public static DataSet ExecuteReaderDs(CommandType cmdType, string cmdText, List<SqlParameter> cmdParms)
        {
            return ExecuteReaderDsComplet(_dal, null, cmdType, cmdText, cmdParms, null);
        }
        /// <summary>
        /// Método utilizado para executar um comando de leitura no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação corrente.</param>
        /// <param name="cmdType">Tipo do comando.</param>
        /// <param name="cmdText">Query ou nome da Stored Procedure.</param>
        /// <param name="cmdParms">Coleção de parâmetros SQL.</param>
        /// <returns>DataSet de retorno da consulta.</returns>
        public static DataSet ExecuteReaderDs(SqlTransaction trans, CommandType cmdType, string cmdText, List<SqlParameter> cmdParms)
        {
            return ExecuteReaderDsComplet(_dal, trans, cmdType, cmdText, cmdParms, null);
        }
        #endregion
    }

    public class ConfigAccessLayer : Employer.BNE.DAL.AbstractConfigAccessLayer
    {
        public override string CONNSTRING
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["plataforma"].ToString();
            }
        }
    }
    public class ConfigAccessLayerBNE : Employer.BNE.DAL.AbstractConfigAccessLayer
    {
        public override string CONNSTRING
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["CONN_BNE"].ToString();
            }
        }
    }
}