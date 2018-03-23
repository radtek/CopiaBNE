using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BNE.BLL
{
    public class ValidaCelular
    {
        #region Attributes
        public int IdValidaCelular { get; set; }
        public int IdPessoaFisica { get; set; }
        public DateTime? DataEnvio { get; set; }
        public string DDD { get; set; }
        public string Numero { get; set; }

        private bool _persisted;
        #endregion

        #region Queries

        #region INSERT
        private const string INSERT = @"INSERT INTO BNE.BNE_Valida_Celular (Idf_Pessoa_Fisica, Dta_Envio, DDD_Celular, Num_Celular) VALUES (@Idf_Pessoa_Fisica, @Dta_Envio, @DDD_Celular,  @Num_Celular); SET @Idf_Valida_Celular = SCOPE_IDENTITY();";
        #endregion

        #region CARREGAR
        private const string CARREGAR = @"SELECT TOP 1 Idf_Valida_Celular, Dta_Envio FROM BNE_Valida_Celular WITH(NOLOCK) WHERE Idf_Pessoa_Fisica = @Idf_Pessoa_Fisica AND DDD_Celular = @DDD_Celular AND Num_Celular = @Num_Celular ORDER BY Dta_Envio DESC;";
        #endregion

        #endregion

        #region Methods
        public static ValidaCelular Carregar(int IdPessoaFisicia, string DDD, string Celular)
        {
            ValidaCelular result = null;
            var parms = new List<SqlParameter>
            {
                new SqlParameter {ParameterName = "@Idf_Pessoa_Fisica", SqlDbType = SqlDbType.Int, Value = IdPessoaFisicia},
                new SqlParameter {ParameterName = "@DDD_Celular", SqlDbType = SqlDbType.VarChar, Value = DDD},
                new SqlParameter {ParameterName = "@Num_Celular", SqlDbType = SqlDbType.VarChar, Value = Celular}
            };

            using (var dr = DataAccessLayer.ExecuteReader(CommandType.Text, CARREGAR, parms))
            {
                if (dr.Read())
                {
                    result = new ValidaCelular();
                    result.IdValidaCelular = dr.GetInt32(dr.GetOrdinal("Idf_Valida_Celular"));
                    result.IdPessoaFisica = IdPessoaFisicia;
                    result.DataEnvio = dr.GetDateTime(dr.GetOrdinal("Dta_Envio"));
                    result.DDD = DDD;
                    result.Numero = Celular;
                }
            }

            return result;
        }


        private List<SqlParameter> getParameters()
        {
            var parms = new List<SqlParameter>
            {
                new SqlParameter {ParameterName = "@Idf_Valida_Celular", SqlDbType = SqlDbType.Int},
                new SqlParameter {ParameterName = "@Idf_Pessoa_Fisica", SqlDbType = SqlDbType.Int},
                new SqlParameter {ParameterName = "@Dta_Envio", SqlDbType = SqlDbType.DateTime},
                new SqlParameter {ParameterName = "@DDD_Celular", SqlDbType = SqlDbType.VarChar},
                new SqlParameter {ParameterName = "@Num_Celular", SqlDbType = SqlDbType.VarChar}
            };

            return parms;
        }

        public List<SqlParameter> SetParameters(List<SqlParameter> parms)
        {
            parms[0].Value = IdValidaCelular;
            parms[1].Value = IdPessoaFisica;
            parms[2].Value = DataEnvio;
            parms[3].Value = DDD;
            parms[4].Value = Numero;

            return parms;
        }

        public void Save()
        {
            using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();
                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        var cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, INSERT, SetParameters(getParameters()));
                        IdValidaCelular = Convert.ToInt32(cmd.Parameters["@Idf_Valida_Celular"].Value);
                        cmd.Parameters.Clear();
                        _persisted = true;
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }
        #endregion
    }
}