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
                return ConfigurationManager.ConnectionStrings["CONN_ATIVIDADE"].ToString();
            }
        }
        #endregion

        #region PrepareCommand
        /// <summary>
        ///     Método utilizado para preparar os comandos SQL que serão executados no banco.
        /// </summary>
        /// <param name="cmd">Comando SQL.</param>
        /// <param name="conn">Conexão que será utilizada.</param>
        /// <param name="trans">Transação corrente.</param>
        /// <param name="cmdType">Tipo do comando.</param>
        /// <param name="cmdText">Query ou nome da stored procedure.</param>
        /// <param name="cmdParms">Coleção de parâmetros SQL.</param>
        private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, CommandType cmdType, string cmdText, List<SqlParameter> cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();

            cmd.Connection = conn;
            cmd.CommandText = cmdText;

            var timeout = 0;
            if (int.TryParse(ConfigurationManager.AppSettings["CommandTimeout"], out timeout))
                cmd.CommandTimeout = timeout;

            if (trans != null)
                cmd.Transaction = trans;

            cmd.CommandType = cmdType;

            if (cmdParms != null)
            {
                foreach (var parm in cmdParms)
                {
                    if (cmd.Parameters.Contains(parm))
                        cmd.Parameters[parm.ParameterName] = parm;
                    else
                        cmd.Parameters.Add(parm);
                }
            }
        }
        #endregion

        #region ExecuteNonQuery
        /// <summary>
        ///     Método utilizado para executar um comando de escrita no banco de dados.
        /// </summary>
        /// <param name="cmdType">Tipo do comando.</param>
        /// <param name="cmdText">Query ou nome da Stored Procedure.</param>
        /// <param name="cmdParms">Coleção de parâmetros SQL.</param>
        /// <returns>Inteiro com o resultado do método original.</returns>
        public static int ExecuteNonQuery(CommandType cmdType, string cmdText, List<SqlParameter> cmdParms)
        {
            using (var conn = new SqlConnection(CONN_STRING))
            {
                try
                {
                    using (var cmd = new SqlCommand())
                    {
                        PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
                        var val = cmd.ExecuteNonQuery();
                        //int val = ExecuteNonQueryReTry(cmd);
                        cmd.Parameters.Clear();
                        return val;
                    }
                }
                catch
                {
                    conn.Close();
                    conn.Dispose();
                    throw;
                }
            }
        }

        /// <summary>
        ///     Método utilizado para executar um comando de escrita no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação corrente.</param>
        /// <param name="cmdType">Tipo do comando.</param>
        /// <param name="cmdText">Query ou nome da Stored Procedure.</param>
        /// <param name="cmdParms">Coleção de parâmetros SQL.</param>
        /// <returns>Inteiro com o resultado do método original.</returns>
        public static int ExecuteNonQuery(SqlTransaction trans, CommandType cmdType, string cmdText, List<SqlParameter> cmdParms)
        {
            var cmd = new SqlCommand();
            var conn = new SqlConnection(CONN_STRING);

            if (trans == null)
                PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
            else
                PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, cmdParms);

            var val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return val;
        }
        #endregion

        #region ExecuteNonQueryCmd
        /// <summary>
        ///     Método utilizado para executar um comando de escrita no banco de dados.
        /// </summary>
        /// <param name="cmdType">Tipo do comando.</param>
        /// <param name="cmdText">Query ou nome da Stored Procedure.</param>
        /// <param name="cmdParms">Coleção de parâmetros SQL.</param>
        /// <returns>Comando SQL executado.</returns>
        public static SqlCommand ExecuteNonQueryCmd(CommandType cmdType, string cmdText, List<SqlParameter> cmdParms)
        {
            var cmd = new SqlCommand();
            using (var conn = new SqlConnection(CONN_STRING))
            {
                try
                {
                    PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
                    cmd.ExecuteNonQuery();
                }
                catch
                {
                    conn.Close();
                    conn.Dispose();
                    throw;
                }
            }
            return cmd;
        }

        /// <summary>
        ///     Método utilizado para executar um comando de escrita no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação corrente.</param>
        /// <param name="cmdType">Tipo do comando.</param>
        /// <param name="cmdText">Query ou nome da Stored Procedure.</param>
        /// <param name="cmdParms">Coleção de parâmetros SQL.</param>
        /// <returns>Comando SQL executado.</returns>
        public static SqlCommand ExecuteNonQueryCmd(SqlTransaction trans, CommandType cmdType, string cmdText, List<SqlParameter> cmdParms)
        {
            var cmd = new SqlCommand();
            PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, cmdParms);
            cmd.ExecuteNonQuery();
            return cmd;
        }
        #endregion

        #region ExecuteReader
        /// <summary>
        ///     Método utilizado para executar um comando de leitura no banco de dados.
        /// </summary>
        /// <param name="cmdType">Tipo do comando.</param>
        /// <param name="cmdText">Query ou nome da Stored Procedure.</param>
        /// <param name="cmdParms">Coleção de parâmetros SQL.</param>
        /// <param name="connStringKey">Connection string que deve ser usada.</param>
        /// <returns>SqlDataReader de retorno da consulta.</returns>
        public static SqlDataReader ExecuteReader(CommandType cmdType, string cmdText, List<SqlParameter> cmdParms, string connStringKey)
        {
            var connString = connStringKey;
            //string connString = ConfigurationManager.ConnectionStrings[connStringKey].ConnectionString;
            SqlDataReader dr = null;
            var conn = new SqlConnection(connString);
            var cmd = new SqlCommand();
            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("TCP Provider, error: 0") || ex.Message.Contains("Provedor TCP, error: 0"))
                {
                    try
                    {
                        conn.Close();
                        SqlConnection.ClearAllPools();
                        //SqlCommand cmd = new SqlCommand();
                        //PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
                        dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    }
                    catch
                    {
                        conn.Close();
                        conn.Dispose();
                    }
                }
                else
                {
                    throw;
                }
            }
            catch
            {
                conn.Close();
                conn.Dispose();
                throw;
            }
            return dr;
        }

        /// <summary>
        ///     Método utilizado para executar um comando de leitura no banco de dados.
        /// </summary>
        /// <param name="cmdType">Tipo do comando.</param>
        /// <param name="cmdText">Query ou nome da Stored Procedure.</param>
        /// <param name="cmdParms">Coleção de parâmetros SQL.</param>
        /// <returns>SqlDataReader de retorno da consulta.</returns>
        public static SqlDataReader ExecuteReader(CommandType cmdType, string cmdText, List<SqlParameter> cmdParms)
        {
            //SqlDataReader dr = null;
            var conn = new SqlConnection(CONN_STRING);
            var cmd = new SqlCommand();
            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
                return cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("TCP Provider, error: 0") || ex.Message.Contains("Provedor TCP, error: 0"))
                {
                    try
                    {
                        conn.Close();
                        SqlConnection.ClearAllPools();
                        conn.Open();
                        //SqlCommand cmd = new SqlCommand();
                        //PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
                        return cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    }
                    catch
                    {
                        conn.Close();
                        conn.Dispose();
                        throw;
                    }
                }
                throw;
            }
            catch
            {
                conn.Close();
                conn.Dispose();
                throw;
            }
        }

        /// <summary>
        ///     Método utilizado para executar um comando de leitura no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação corrente.</param>
        /// <param name="cmdType">Tipo do comando.</param>
        /// <param name="cmdText">Query ou nome da Stored Procedure.</param>
        /// <param name="cmdParms">Coleção de parâmetros SQL.</param>
        /// <returns>SqlDataReader de retorno da consulta.</returns>
        public static SqlDataReader ExecuteReader(SqlTransaction trans, CommandType cmdType, string cmdText, List<SqlParameter> cmdParms)
        {
            SqlDataReader dr = null;
            var cmd = new SqlCommand();
            var conn = new SqlConnection(CONN_STRING);

            if (trans == null)
                PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
            else
                PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, cmdParms);

            dr = cmd.ExecuteReader();
            return dr;
        }
        #endregion

        #region ExecuteReaderDs
        /// <summary>
        ///     Método utilizado para executar um comando de leitura no banco de dados.
        /// </summary>
        /// <param name="cmdType">Tipo do comando.</param>
        /// <param name="cmdText">Query ou nome da Stored Procedure.</param>
        /// <param name="cmdParms">Coleção de parâmetros SQL.</param>
        /// <param name="connStringKey">Connection string que deve ser utilizada.</param>
        /// <returns>DataSet de retorno da consulta.</returns>
        public static DataSet ExecuteReaderDs(CommandType cmdType, string cmdText, List<SqlParameter> cmdParms, string connStringKey)
        {
            var connString = ConfigurationManager.ConnectionStrings[connStringKey].ConnectionString;
            var ds = new DataSet();
            using (var conn = new SqlConnection(connString))
            {
                try
                {
                    using (var cmd = new SqlCommand())
                    {
                        PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
                        var da = new SqlDataAdapter(cmd);
                        da.Fill(ds);
                        cmd.Parameters.Clear();
                    }
                }
                catch
                {
                    conn.Close();
                    conn.Dispose();
                    throw;
                }
            }
            return ds;
        }

        /// <summary>
        ///     Método utilizado para executar um comando de leitura no banco de dados.
        /// </summary>
        /// <param name="cmdType">Tipo do comando.</param>
        /// <param name="cmdText">Query ou nome da Stored Procedure.</param>
        /// <param name="cmdParms">Coleção de parâmetros SQL.</param>
        /// <returns>DataSet de retorno da consulta.</returns>
        public static DataSet ExecuteReaderDs(CommandType cmdType, string cmdText, List<SqlParameter> cmdParms)
        {
            var ds = new DataSet();
            using (var conn = new SqlConnection(CONN_STRING))
            {
                try
                {
                    using (var cmd = new SqlCommand())
                    {
                        PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
                        var da = new SqlDataAdapter(cmd);
                        da.Fill(ds);
                        cmd.Parameters.Clear();
                    }
                }
                catch
                {
                    conn.Close();
                    conn.Dispose();
                    throw;
                }
            }
            return ds;
        }

        /// <summary>
        ///     Método utilizado para executar um comando de leitura no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação corrente.</param>
        /// <param name="cmdType">Tipo do comando.</param>
        /// <param name="cmdText">Query ou nome da Stored Procedure.</param>
        /// <param name="cmdParms">Coleção de parâmetros SQL.</param>
        /// <returns>DataSet de retorno da consulta.</returns>
        public static DataSet ExecuteReaderDs(SqlTransaction trans, CommandType cmdType, string cmdText, List<SqlParameter> cmdParms)
        {
            var ds = new DataSet();
            var cmd = new SqlCommand();
            PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, cmdParms);
            var da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            cmd.Parameters.Clear();
            return ds;
        }
        #endregion

        #region ExecuteScalar
        /// <summary>
        ///     Método utilizado para executar um comando de leitura no banco de dados.
        /// </summary>
        /// <param name="cmdType">Tipo do comando.</param>
        /// <param name="cmdText">Query ou nome da Stored Procedure.</param>
        /// <param name="cmdParms">Coleção de parâmetros SQL.</param>
        /// <returns>Objeto de retorno da consulta.</returns>
        public static object ExecuteScalar(CommandType cmdType, string cmdText, List<SqlParameter> cmdParms)
        {
            var obj = new object();
            using (var conn = new SqlConnection(CONN_STRING))
            {
                try
                {
                    using (var cmd = new SqlCommand())
                    {
                        PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
                        obj = cmd.ExecuteScalar();
                        cmd.Parameters.Clear();
                    }
                }
                catch
                {
                    conn.Close();
                    conn.Dispose();
                    throw;
                }
            }
            return obj;
        }

        /// <summary>
        ///     Método utilizado para executar um comando de leitura no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação corrente.</param>
        /// <param name="cmdType">Tipo do comando.</param>
        /// <param name="cmdText">Query ou nome da Stored Procedure.</param>
        /// <param name="cmdParms">Coleção de parâmetros SQL.</param>
        /// <returns>Objeto de retorno da consulta.</returns>
        public static object ExecuteScalar(SqlTransaction trans, CommandType cmdType, string cmdText, List<SqlParameter> cmdParms)
        {
            var cmd = new SqlCommand();
            var conn = new SqlConnection(CONN_STRING);

            if (trans == null)
                PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
            else
                PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, cmdParms);

            var obj = cmd.ExecuteScalar();
            cmd.Parameters.Clear();
            return obj;
        }
        #endregion

        #region Inserção em Massa
        /// <summary>
        ///     Cria uma tabela para inserir em massa.
        ///     Passe um DataTable nulo para e a classe em populada.  Ex: MensagemCS
        ///     Ela irá instanciar um novo DataTable já com colunas definidas apartir dos parâmetros sql definidos na classe.
        ///     Os valores setados nas propriedades são transformados em uma linha na tabela.
        /// </summary>
        /// <param name="tbMsg"></param>
        /// <param name="ObjDaata">Objeto de classe. A classe tem que ter os métodos GetParameters e  SetParameters</param>
        public static void AddBulkTable(ref DataTable tbMsg, object ObjDaata)
        {
            var parms = (List<SqlParameter>)
                ObjDaata.GetType().GetMethod("GetParameters", BindingFlags.NonPublic | BindingFlags.Instance).
                    Invoke(ObjDaata, null);
            ObjDaata.GetType().GetMethod("SetParameters", BindingFlags.NonPublic | BindingFlags.Instance).
                Invoke(ObjDaata, new object[] { parms });

            if (tbMsg == null)
            {
                tbMsg = new DataTable();
                foreach (var par in parms)
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

                    tbMsg.Columns.Add(par.ParameterName.Replace("@", ""), tyColummn);
                }
            }

            var linha = tbMsg.NewRow();
            foreach (var par in parms)
                linha[par.ParameterName.Replace("@", "")] = par.Value;

            tbMsg.Rows.Add(linha);
        }

        /// <summary>
        ///     Realiza inserção em massa.
        /// </summary>
        /// <param name="tbMsg">Tabela criada pelo método AddBulkTable</param>
        /// <param name="DestinationTableName">Tabela destino</param>
        public static void SaveBulkTable(DataTable tbMsg, string DestinationTableName, SqlTransaction tran = null)
        {
            if (tbMsg == null)
                return;

            SqlBulkCopy bulkCopy = null;
            if (tran == null)
                bulkCopy = new SqlBulkCopy(CONN_STRING, SqlBulkCopyOptions.FireTriggers & SqlBulkCopyOptions.KeepIdentity & SqlBulkCopyOptions.KeepNulls);
            else
                bulkCopy = new SqlBulkCopy(tran.Connection, SqlBulkCopyOptions.FireTriggers & SqlBulkCopyOptions.KeepIdentity & SqlBulkCopyOptions.KeepNulls, tran);
            using (bulkCopy)
            {
                foreach (DataColumn column in tbMsg.Columns)
                {
                    bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping(column.ColumnName, column.ColumnName));
                }

                bulkCopy.DestinationTableName = DestinationTableName;

                var arlinha = new DataRow[tbMsg.Rows.Count];
                var i = 0;
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
}