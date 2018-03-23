using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace BNE.Sistmars.BLL
{
    public abstract class DataAccessLayer
    {
        #region ConnectionString
        public static string CONN_STRING = ConfigurationManager.ConnectionStrings["CONN_SISTMARS"].ToString();
        #endregion

        #region ExecuteNonQuery
        /// <summary>
        /// Método utilizado para executar um comando de escrita no banco de dados.
        /// </summary>
        /// <param name="cmdType">Tipo do comando.</param>
        /// <param name="cmdText">Query ou nome da Stored Procedure.</param>
        /// <param name="cmdParms">Coleção de parâmetros SQL.</param>
        /// <returns>Inteiro com o resultado do método original.</returns>
        public static int ExecuteNonQuery(CommandType cmdType, string cmdText, List<SqlParameter> cmdParms)
        {
            SqlCommand cmd = null;
            try
            {
                using (SqlConnection conn = new SqlConnection(CONN_STRING))
                {
                    try
                    {
                        cmd = new SqlCommand();
                        PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
                        int val = cmd.ExecuteNonQuery();
                        //int val = ExecuteNonQueryReTry(cmd);
                        cmd.Parameters.Clear();
                        return val;
                    }
                    catch (SqlException ex)
                    {
                        if (ex.Message.Contains("TCP Provider, error: 0") || ex.Message.Contains("Provedor TCP, error: 0"))
                        {
                            try
                            {
                                conn.Close();
                                SqlConnection.ClearAllPools();
                                //cmd = new SqlCommand();
                                //PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
                                int val = cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                                return val;
                            }
                            catch
                            {
                                conn.Close();
                                conn.Dispose();
                                throw;
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
                }
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
            }
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
            SqlCommand cmd = null;
            int val;
            try
            {
                cmd = new SqlCommand();
                PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, cmdParms);
                val = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
            }
            return val;
        }
        #endregion

        #region ExecuteNonQueryCmd
        /// <summary>
        /// Método utilizado para executar um comando de escrita no banco de dados.
        /// </summary>
        /// <param name="cmdType">Tipo do comando.</param>
        /// <param name="cmdText">Query ou nome da Stored Procedure.</param>
        /// <param name="cmdParms">Coleção de parâmetros SQL.</param>
        /// <returns>Comando SQL executado.</returns>
        public static SqlCommand ExecuteNonQueryCmd(CommandType cmdType, string cmdText, List<SqlParameter> cmdParms)
        {
            SqlCommand cmd = null;
            try
            {
                using (SqlConnection conn = new SqlConnection(CONN_STRING))
                {
                    try
                    {
                        cmd = new SqlCommand();
                        PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
                        cmd.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        if (ex.Message.Contains("TCP Provider, error: 0") || ex.Message.Contains("Provedor TCP, error: 0"))
                        {
                            try
                            {
                                conn.Close();
                                SqlConnection.ClearAllPools();
                                //PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
                                cmd.ExecuteNonQuery();
                            }
                            catch
                            {
                                conn.Close();
                                conn.Dispose();
                                throw;
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
                }
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
            }
           
            return cmd;
        }
        /// <summary>
        /// Método utilizado para executar um comando de escrita no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação corrente.</param>
        /// <param name="cmdType">Tipo do comando.</param>
        /// <param name="cmdText">Query ou nome da Stored Procedure.</param>
        /// <param name="cmdParms">Coleção de parâmetros SQL.</param>
        /// <returns>Comando SQL executado.</returns>
        public static SqlCommand ExecuteNonQueryCmd(SqlTransaction trans, CommandType cmdType, string cmdText, List<SqlParameter> cmdParms)
        {
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand();
                PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, cmdParms);
                cmd.ExecuteNonQuery();
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
            }

            return cmd;
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
        public static SqlDataReader ExecuteReader(CommandType cmdType, string cmdText, List<SqlParameter> cmdParms, string connStringKey)
        {
            string connString = connStringKey;
            //string connString = ConfigurationManager.ConnectionStrings[connStringKey].ConnectionString;
            SqlDataReader dr = null;
            SqlConnection conn = null;
            SqlCommand cmd = null;

            try
            {
                conn = new SqlConnection(connString);
                cmd = new SqlCommand();
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
            finally
            {
                if (cmd != null)
                    cmd.Dispose();

                if (conn != null)
                    conn.Dispose();
            }
         
            return dr;
        }
        /// <summary>
        /// Método utilizado para executar um comando de leitura no banco de dados.
        /// </summary>
        /// <param name="cmdType">Tipo do comando.</param>
        /// <param name="cmdText">Query ou nome da Stored Procedure.</param>
        /// <param name="cmdParms">Coleção de parâmetros SQL.</param>
        /// <returns>SqlDataReader de retorno da consulta.</returns>
        public static SqlDataReader ExecuteReader(CommandType cmdType, string cmdText, List<SqlParameter> cmdParms)
        {
            //SqlDataReader dr = null;
            SqlConnection conn = null;
            SqlCommand cmd = null;
            try
            {
                conn = new SqlConnection(CONN_STRING);
                cmd = new SqlCommand();
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
            finally
            {
                if (cmd != null)
                    cmd.Dispose();

                if (conn != null)
                    conn.Dispose();
            }
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
            SqlDataReader dr = null;
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand();
                PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, cmdParms);
                dr = cmd.ExecuteReader();
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
            }

            return dr;
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
            string connString = ConfigurationManager.ConnectionStrings[connStringKey].ConnectionString;
            DataSet ds = null;
            SqlCommand cmd = null;
            SqlDataAdapter da = null;
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    try
                    {
                        cmd = new SqlCommand();
                        PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
                        da = new SqlDataAdapter(cmd);
                        ds = new DataSet();
                        da.Fill(ds);
                        cmd.Parameters.Clear();
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
                                da = new SqlDataAdapter(cmd);
                                da.Fill(ds);
                                cmd.Parameters.Clear();
                            }
                            catch
                            {
                                conn.Close();
                                conn.Dispose();

                                if (da != null)
                                    da.Dispose();

                                if (cmd != null)
                                    cmd.Dispose();

                                throw;
                            }
                        }
                        else
                        {
                            if (da != null)
                                da.Dispose();

                            throw;
                        }
                    }
                    catch
                    {
                        conn.Close();
                        conn.Dispose();

                        if (cmd != null)
                            cmd.Dispose();

                        if (da != null)
                            da.Dispose();

                        throw;
                    }
                    finally
                    {
                        if (ds != null)
                            ds.Dispose();

                        if (da != null)
                            da.Dispose();
                    }
                }
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();

                if (ds != null)
                    ds.Dispose();

                if (da != null)
                    da.Dispose();
            }
            return ds;
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
            DataSet ds = null;
            SqlCommand cmd = null;

            try
            {
                using (SqlConnection conn = new SqlConnection(CONN_STRING))
                {
                    SqlDataAdapter da = null;
                    try
                    {
                        cmd = new SqlCommand();
                        PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
                        da = new SqlDataAdapter(cmd);
                        ds = new DataSet();
                        da.Fill(ds);
                        cmd.Parameters.Clear();
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
                                da = new SqlDataAdapter(cmd);
                                da.Fill(ds);
                                cmd.Parameters.Clear();
                            }
                            catch
                            {
                                conn.Close();
                                conn.Dispose();
                                throw;
                            }
                            finally
                            {
                                if (da != null)
                                    da.Dispose();
                            }
                        }
                        else
                        {
                            if (da != null)
                                da.Dispose();
                            throw;
                        }
                    }
                    catch
                    {
                        conn.Close();
                        conn.Dispose();
                        if (da != null)
                            da.Dispose();
                        throw;
                    }
                    finally
                    {
                        if (da != null)
                            da.Dispose();
                    }
                }
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();

                if (ds != null)
                    ds.Dispose();
            }
            return ds;
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
            DataSet ds = null;
            SqlCommand cmd = null;
            SqlDataAdapter da = null;
            try
            {
                ds = new DataSet();
                cmd = new SqlCommand();
                PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, cmdParms);
                da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                cmd.Parameters.Clear();
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();

                if (ds != null)
                    ds.Dispose();

                if (da != null)
                    da.Dispose();
            }
            return ds;
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
            object obj = new object();
            SqlCommand cmd = null;
            try
            {
                using (SqlConnection conn = new SqlConnection(CONN_STRING))
                {
                    try
                    {
                        cmd = new SqlCommand();
                        PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
                        obj = cmd.ExecuteScalar();
                        cmd.Parameters.Clear();
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
                                obj = cmd.ExecuteScalar();
                                cmd.Parameters.Clear();
                            }
                            catch
                            {
                                conn.Close();
                                conn.Dispose();
                                throw;
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
                }
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
            }
            return obj;
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
            SqlCommand cmd = null;
            object obj = null;
            try
            {
                cmd = new SqlCommand();
                PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, cmdParms);
                obj = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
            }
            return obj;
        }
        #endregion

        #region PrepareCommand
        /// <summary>
        /// Método utilizado para preparar os comandos SQL que serão executados no banco.
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

            if (trans != null)
                cmd.Transaction = trans;

            cmd.CommandType = cmdType;

            if (cmdParms != null)
            {
                foreach (SqlParameter parm in cmdParms)
                {
                    if (cmd.Parameters.Contains(parm))
                        cmd.Parameters[parm.ParameterName] = parm;
                    else
                        cmd.Parameters.Add(parm);
                }
            }
        }
        #endregion
    }
}
