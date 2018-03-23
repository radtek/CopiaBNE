using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System;
using System.Reflection;

namespace BNE.BLL.AsyncServices
{
    public class DataAccessLayer : Employer.BNE.DAL.DataAccessLayer
    {
        #region ConnectionString
        public static string CONN_STRING 
        {
            get 
            {
                return ConfigurationManager.ConnectionStrings["CONN_BNE"].ToString();
            }
        }

        public static string CONN_NOTIFICACAO
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["CONN_NOTIFICACAO"].ToString();
            }
        }
        #endregion

        static BNE.BLL.AsyncServices.ConfigAccessLayer _dal = new BNE.BLL.AsyncServices.ConfigAccessLayer();

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

        #region Inserção em Massa
        /// <summary>
        /// Cria uma tabela para inserir em massa.
        /// Passe um DataTable nulo para e a classe em populada.  Ex: MensagemCS
        /// Ela irá instanciar um novo DataTable já com colunas definidas apartir dos parâmetros sql definidos na classe.
        /// Os valores setados nas propriedades são transformados em uma linha na tabela.
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="ObjDaata">Objeto de classe. A classe tem que ter os métodos GetParameters e  SetParameters</param>
        public static void AddBulkTable(ref DataTable dt,object obj)
        {
            var parms = (List<SqlParameter>)obj.GetType().GetMethod("GetParameters", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(obj, null);

            foreach (var parm in parms)
            {
                parm.ParameterName = parm.ParameterName.Replace("@", string.Empty);
            }

            obj.GetType().GetMethod("SetParameters", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(obj, new object[] { parms });

            if (dt == null)
            {
                dt = new DataTable();
                foreach (SqlParameter par in parms)
                {
                    Type tyColummn = null;
                    if (par.SqlDbType == SqlDbType.VarChar ||
                        par.SqlDbType == SqlDbType.Text ||
                        par.SqlDbType == SqlDbType.NVarChar ||
                        par.SqlDbType == SqlDbType.NText ||
                        par.SqlDbType == SqlDbType.NChar ||
                        par.SqlDbType == SqlDbType.Char ||
                        par.SqlDbType == SqlDbType.Xml)
                        tyColummn = typeof(string);
                    else if (par.SqlDbType == SqlDbType.Int)
                        tyColummn = typeof(int);
                    else if (par.SqlDbType == SqlDbType.SmallInt)
                        tyColummn = typeof(short);
                    else if (par.SqlDbType == SqlDbType.Decimal ||
                        par.SqlDbType == SqlDbType.Money ||
                        par.SqlDbType == SqlDbType.Float)
                        tyColummn = typeof(decimal);
                    else if (par.SqlDbType == SqlDbType.Date ||
                        par.SqlDbType == SqlDbType.DateTime ||
                        par.SqlDbType == SqlDbType.DateTime2)
                        tyColummn = typeof(DateTime);
                    else if (par.SqlDbType == SqlDbType.Bit)
                        tyColummn = typeof(bool);
                    else if (par.SqlDbType == SqlDbType.VarBinary)
                        tyColummn = typeof(byte[]);
                    else
                        tyColummn = typeof(object);

                    dt.Columns.Add(par.ParameterName, tyColummn);
                }
            }

            DataRow linha = dt.NewRow();
            foreach (SqlParameter par in parms)
                linha[par.ParameterName] = par.Value;

            dt.Rows.Add(linha);
        }

        /// <summary>
        /// Realiza inserção em massa.
        /// </summary>
        /// <param name="tbMsg">Tabela criada pelo método AddBulkTable</param>
        /// <param name="DestinationTableName">Tabela destino</param>
		/// 
        public static void SaveBulkTable(DataTable tbMsg, string DestinationTableName, SqlTransaction tran = null)
        {
            if (tbMsg == null)
                return;

            SqlBulkCopy bulkCopy;
            if (tran == null)
                bulkCopy = new SqlBulkCopy(DataAccessLayer.CONN_STRING, SqlBulkCopyOptions.FireTriggers);
            else
                bulkCopy = new SqlBulkCopy(tran.Connection, SqlBulkCopyOptions.FireTriggers, tran);

            using (bulkCopy)
            {
                foreach (DataColumn column in tbMsg.Columns)
                {
                    bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping(column.ColumnName, column.ColumnName));
                }

                bulkCopy.DestinationTableName = DestinationTableName;

                var arlinha = new DataRow[tbMsg.Rows.Count];
                int i = 0;
                foreach (DataRow linhaAdd in tbMsg.Rows)
                {
                    arlinha[i] = linhaAdd;
                    i++;
                }

                bulkCopy.WriteToServer(arlinha);
            }
        }
        #endregion
    }

    public class ConfigAccessLayer : Employer.BNE.DAL.AbstractConfigAccessLayer
    {


    }
}