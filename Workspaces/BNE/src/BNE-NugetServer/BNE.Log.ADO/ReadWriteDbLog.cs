using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using BNE.Log.Base;

namespace BNE.Log.ADO
{
    public class ReadWriteDbLog : IReadWriteDbLog
    {
        #region Propriedades

        #region ConnString
        private static string ConnString
        {
            get
            {
                if (string.IsNullOrEmpty(_connString))
                {
                    lock (_connString)
                    {
                        if (string.IsNullOrEmpty(_connString))
                        {
                            var rx = new Regex("provider +connection +string=\"([^\"]+)\"", RegexOptions.IgnoreCase);
                            var con = ConfigurationManager.ConnectionStrings["BNE.Log"];
                            if (rx.IsMatch(con.ConnectionString))
                            {
                                var cmm = rx.Matches(con.ConnectionString);
                                _connString = cmm[0].Groups[1].Value;
                            }
                            else
                                _connString = con.ConnectionString;
                        }
                    }
                }

                return _connString;
            }
        }
        #endregion

        #endregion

        public void WriteList(IEnumerable<BaseMessage> ls)
        {
            foreach (var er in ls)
            {
                WriteMessage(er);
            }
        }

        private object SetParamValueDB(object obj)
        {
            if (obj == null)
                return DBNull.Value;
            return obj;
        }

        private void WriteMessage(BaseMessage er)
        {
            string insert = null;

            var errorMessage = er as ErrorMessage;
            if (errorMessage != null)
            {
                insert = InsertErro;
                if (errorMessage.InnerException != null)
                {
                    WriteMessage(errorMessage.InnerException);
                }
            }

            var warningMessage = er as WarningMessage;
            if (warningMessage != null)
            {
                insert = InsertWarning;
            }

            var informationMessage = er as InformationMessage;
            if (informationMessage != null)
            {
                insert = InsertInformation;
            }

            if (!string.IsNullOrWhiteSpace(insert))
            {
                using (var conn = new SqlConnection(ConnString))
                {
                    conn.Open();

                    var cmd = new SqlCommand
                    {
                        CommandType = CommandType.Text,
                        Connection = conn,
                        CommandText = insert
                    };

                    cmd.Parameters.AddRange(new[]
                    {
                        new SqlParameter("@Id", SqlDbType.UniqueIdentifier) {Value = SetParamValueDB(er.Id)}
                        , new SqlParameter("@DataCadastro", SqlDbType.DateTime) {Value = SetParamValueDB(er.DataCadastro)}
                        , new SqlParameter("@Aplicacao", SqlDbType.VarChar, -1) {Value = SetParamValueDB(er.Aplicacao)}
                        , new SqlParameter("@Usuario", SqlDbType.VarChar, 255) {Value = SetParamValueDB(er.Usuario)}
                        , new SqlParameter("@Session", SqlDbType.NText) {Value = SetParamValueDB(er.Session)}
                        , new SqlParameter("@URL", SqlDbType.NText) {Value = SetParamValueDB(er.URL)}
                        , new SqlParameter("@Request", SqlDbType.NText) {Value = SetParamValueDB(er.Request)}
                        , new SqlParameter("@Response", SqlDbType.NText) {Value = SetParamValueDB(er.Response)}
                        , new SqlParameter("@Message", SqlDbType.VarChar, -1) {Value = SetParamValueDB(er.Message)}
                        , new SqlParameter("@MachineName", SqlDbType.VarChar, -1) {Value = SetParamValueDB(er.MachineName)}
                        , new SqlParameter("@CustomMessage", SqlDbType.VarChar, -1) {Value = SetParamValueDB(er.CustomMessage)}
                        , new SqlParameter("@Payload", SqlDbType.VarChar, -1) {Value = SetParamValueDB(er.Payload)}
                        , new SqlParameter("@UrlReferrer", SqlDbType.NText) {Value = SetParamValueDB(er.UrlReferrer)}
                    });

                    if (errorMessage != null)
                    {
                        cmd.Parameters.AddRange(new[]
                        {
                            new SqlParameter("@InnerException", SqlDbType.UniqueIdentifier) {Value = errorMessage.InnerException != null ? (object) errorMessage.InnerException.Id : DBNull.Value}
                            , new SqlParameter("@StackTrace", SqlDbType.NText) {Value = SetParamValueDB(errorMessage.StackTrace)}
                            , new SqlParameter("@Source", SqlDbType.VarChar, -1) {Value = SetParamValueDB(errorMessage.Source)}
                        });
                    }

                    cmd.ExecuteNonQuery();
                }
            }
        }
        
        #region Atributos
        private static string _connString = string.Empty;

        #region INSERT
        private const string InsertErro = @"
        INSERT  INTO Error ( [Id], [Aplicacao], [Usuario], [Session], [URL], [Request], [Response], [InnerException], [StackTrace], [Message], [CustomMessage], [Payload], [Source], [MachineName], [UrlReferrer], [DataCadastro] )
        VALUES  ( @Id, @Aplicacao, @Usuario, @Session, @URL, @Request, @Response, @InnerException, @StackTrace, @Message, @CustomMessage, @Payload, @Source, @MachineName, @UrlReferrer, @DataCadastro );
        ";
        private const string InsertInformation = @"
        INSERT  INTO Information ( [Id], [Aplicacao], [Usuario], [Session], [URL], [Request], [Response], [Message], [CustomMessage], [Payload], [MachineName], [UrlReferrer], [DataCadastro] )
        VALUES  ( @Id, @Aplicacao, @Usuario, @Session, @URL, @Request, @Response,  @Message, @CustomMessage, @Payload, @MachineName, @UrlReferrer, @DataCadastro );
        ";
        private const string InsertWarning = @"
        INSERT  INTO Warning ( [Id], [Aplicacao], [Usuario], [Session], [URL], [Request], [Response], [Message], [CustomMessage], [Payload], [MachineName], [UrlReferrer], [DataCadastro] )
        VALUES  ( @Id, @Aplicacao, @Usuario, @Session, @URL, @Request, @Response,  @Message, @CustomMessage, @Payload, @MachineName, @UrlReferrer, @DataCadastro );
        ";
        #endregion

        #endregion
    }

    #region Extensions
    public static class Extensions
    {
        private static T GetValueDB<T>(object obj)
        {
            if (obj == DBNull.Value || obj == null)
                return (T)(object)null;

            var tipo = typeof(T);
            if (tipo.IsGenericType)
                tipo = Nullable.GetUnderlyingType(tipo);

            if (obj.GetType() == tipo)
                return (T)obj;

            if (tipo == typeof(string))
                return (T)(object)Convert.ToString(obj);
            if (tipo == typeof(int))
                return (T)(object)Convert.ToInt32(obj);
            if (tipo == typeof(long))
                return (T)(object)Convert.ToInt64(obj);
            if (tipo == typeof(DateTime))
                return (T)(object)Convert.ToDateTime(obj);
            if (tipo == typeof(decimal))
                return (T)(object)Convert.ToDecimal(obj);
            if (tipo == typeof(double))
                return (T)(object)Convert.ToDouble(obj);
            if (tipo == typeof(bool))
                return (T)(object)Convert.ToBoolean(obj);
            if (tipo == typeof(Guid))
                return (T)(object)Convert.ToBoolean(obj);

            return (T)(object)null;
        }

        internal static T GetValue<T>(this IDataReader rd, string key)
        {
            return GetValueDB<T>(rd[key]);
        }
    }
    #endregion
}