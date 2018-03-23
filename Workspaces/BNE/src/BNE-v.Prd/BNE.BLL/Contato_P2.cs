//-- Data: 09/03/2010 17:06
//-- Autor: Gieyson Stelmak

using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System;
using BNE.BLL.DTO;
using BNE.BLL.DTO.OperadoraCelular;

namespace BNE.BLL
{
	public partial class Contato // Tabela: TAB_Contato
    {

        #region Consultas
        private const string SPSELECTPORPESSOAFISICATIPOCONTATO = "SELECT * FROM TAB_Contato WHERE Idf_Pessoa_Fisica = @Idf_Pessoa_Fisica AND Idf_Tipo_Contato = @Idf_Tipo_Contato";
        private const string SPSELECTPORPESSOAFISICA = "SELECT * FROM TAB_Contato WHERE Idf_Pessoa_Fisica = @Idf_Pessoa_Fisica";
        private const string SpSelectOperadoraCelular = @"
        SELECT Con.Idf_Contato, LTRIM(RTRIM(Con.Num_DDD_Celular)) AS DDD, LTRIM(RTRIM(Con.Num_Celular)) AS Numero, Con.Idf_Operadora_Celular, C.Idf_Curriculo
        FROM BNE.TAB_Contato Con
        LEFT JOIN BNE.BNE_Curriculo C ON Con.Idf_Pessoa_Fisica = C.Idf_Pessoa_Fisica
        WHERE 1 = 1
        AND   Con.Num_DDD_Celular   IS NOT NULL
        AND   Con.Num_Celular       IS NOT NULL
        AND   Con.Idf_Contato       BETWEEN @Idf_Contato_Inicio AND @Idf_Contato_Fim
";
        private const string SpAtualizarOperadoraCelular = @"
        UPDATE BNE.TAB_Contato
        SET Idf_Operadora_Celular = {0}
        WHERE Idf_Contato IN ({1})
";
        private const string SpCarregarIdCadastradoEm = @"
        SELECT TOP (1) Idf_Contato
        FROM BNE.TAB_Contato
        WHERE Dta_Cadastro BETWEEN @Dta_Cadastro AND GETDATE()
        ORDER BY Idf_Contato
";

        #region [spDeletePorPessoaFisica]
        private const string spDeletePorPessoaFisica = "delete tab_contato where Idf_Pessoa_Fisica = @Idf_Pessoa_Fisica";
        #endregion

        #endregion

        #region CarregarPorPessoaFisicaTipoContato
        /// <summary>
        /// Método responsável por carregar uma instancia de contato através do
        /// identificar de uma pessoa física e do tipo do contato.
        /// </summary>
        /// <param name="idPessoaFisica">Identificador da Pessoa Física</param>
        /// <returns>Boolean</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static bool CarregarPorPessoaFisicaTipoContato(int idPessoaFisica, int idTipoContato, out Contato objContato, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Tipo_Contato", SqlDbType.Int, 4));
            parms[0].Value = idPessoaFisica;
            parms[1].Value = idTipoContato;

            IDataReader dr;

            if (trans != null)
                dr = DataAccessLayer.ExecuteReader(trans,CommandType.Text, SPSELECTPORPESSOAFISICATIPOCONTATO, parms);
            else
                dr = DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPORPESSOAFISICATIPOCONTATO, parms);

            objContato = new Contato();
            if (SetInstance(dr, objContato))
                return true;

            if (!dr.IsClosed)
                dr.Close();

            dr.Dispose();
            
            objContato = null;
            return false;
        }
        #endregion

        #region CarregarPorPessoaFisica
        /// <summary>
        /// Método responsável por carregar uma instancia de contato através do
        /// identificar de uma pessoa física
        /// </summary>
        /// <param name="idPessoaFisica">Identificador da Pessoa Física</param>
        /// <returns>Boolean</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static bool CarregarPorPessoaFisica(int idPessoaFisica, out Contato objContato)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Int, 4));
            parms[0].Value = idPessoaFisica;

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPORPESSOAFISICA, parms))
            {
                objContato = new Contato();
                if (SetInstance(dr, objContato))
                    return true;

                if (!dr.IsClosed)
                    dr.Close();
            }
            objContato = null;
            return false;
        }
        #endregion

        #region InsertIntegracao
        public void InsertIntegracao(DateTime dtaAlteracao)
        {
            using (SqlConnection conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        List<SqlParameter> parms = GetParameters();
                        SetParameters(parms);

                        //sincroniza a Data de Alteração do Registro para evitar trashing de sincronismo.
                        parms.Find(ParametroDataAlteracao).Value = dtaAlteracao;

                        SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
                        cmd.Parameters.Clear();
                        this._persisted = true;
                        this._modified = false;

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

        #region ParametroDataAlteracao
        private static bool ParametroDataAlteracao(SqlParameter parm)
        {
            if (parm.ParameterName == "@Dta_Alteracao")
                return true;

            return false;
        }
        #endregion

        #region CarregarIdCadastradoEm
        public static int CarregarIdCadastradoEm(DateTime dataCadastro)
        {
            var parms = new List<SqlParameter>
				{
					new SqlParameter { ParameterName = "@Dta_Cadastro", SqlDbType = SqlDbType.DateTime, Value = dataCadastro }
				};

            int retorno = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SpCarregarIdCadastradoEm, parms));

            return retorno;
        }
        #endregion CarregarIdCadastradoEm

        #region CarregarOperadoraCelular
        public static List<ContatoOperadoraCelular> CarregarOperadoraCelular(int idContatoInicial, int idContatoFinal)
        {
            var lista = new List<ContatoOperadoraCelular>();

            List<SqlParameter> parms = new List<SqlParameter>()
            {
                new SqlParameter { ParameterName = "@Idf_Contato_Inicio", SqlDbType = SqlDbType.Int, Size = 4, Value = idContatoInicial },
                new SqlParameter { ParameterName = "@Idf_Contato_Fim", SqlDbType = SqlDbType.Int, Size = 4, Value = idContatoFinal }
            };

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SpSelectOperadoraCelular, parms))
            {
                while (dr.Read())
                {
                    lista.Add(new ContatoOperadoraCelular
                    {
                        IdContato = Convert.ToInt32(dr["Idf_Contato"]),
                        IdOperadoraCelular = dr["Idf_Operadora_Celular"] != DBNull.Value ? Convert.ToInt32(dr["Idf_Operadora_Celular"]) : (int?)null,
                        DDD = dr["DDD"].ToString(),
                        Numero = dr["Numero"].ToString(),
                        IdCurriculo = dr["Idf_Curriculo"] != DBNull.Value ? Convert.ToInt32(dr["Idf_Curriculo"]) : (int?)null
                    });
                }
            }

            return lista;
        }
        #endregion

        #region AtualizarOperadoraCelular
        public static void AtualizarOperadoraCelular(int idOperadoraCelular, List<int> lista)
        {
            string listaIds = String.Join(",", lista);

            string sql =
                String.Format(SpAtualizarOperadoraCelular,
                    idOperadoraCelular == 0 ? "NULL" : idOperadoraCelular.ToString(),
                    listaIds);

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, sql, null);
        }
        #endregion

        #region [DeletePorPessoaFisica]
        /// <summary>
        /// Deleta todos contatos.
        /// </summary>
        /// <param name="idPessoaFisica"></param>
        /// <param name="trans"></param>
        public static void DeletePorPessoaFisica(int idPessoaFisica, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Int, 4));
            parms[0].Value = idPessoaFisica;

            DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, spDeletePorPessoaFisica, parms);
        }

        #endregion
    }
}