//-- Data: 09/03/2010 17:06
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
namespace BNE.BLL
{
    public partial class PessoafisicaIdioma // Tabela: Tab_Pessoa_fisica_Idioma
    {
        private const string SPSELECTPORPESSOAFISICA = @"
                                                            SELECT 
                                                                Idf_Pessoa_Fisica_Idioma,
                                                                Des_Idioma,
                                                                Des_Nivel_Idioma,
																Idf_Pessoa_Fisica,
																I.Idf_Idioma,
																NI.Idf_Nivel_Idioma,
																PFI.Dta_Cadastro,
																PFI.Flg_Inativo
                                                            FROM TAB_Pessoa_fisica_Idioma PFI
                                                                INNER JOIN TAB_Idioma I ON PFI.Idf_Idioma = I.Idf_Idioma
                                                                INNER JOIN TAB_Nivel_Idioma NI ON PFI.Idf_Nivel_Idioma = NI.Idf_Nivel_Idioma
                                                            WHERE PFI.Idf_Pessoa_Fisica = @Idf_Pessoa_Fisica AND
                                                                PFI.Flg_Inativo = 0";

        private const string SPSELECTPORPESSOAFISICAIDIOMA = @"
                                                                SELECT * FROM Tab_Pessoa_fisica_Idioma WHERE Idf_Pessoa_Fisica = @Idf_Pessoa_Fisica AND Idf_Idioma = @Idf_Idioma";

        private const string SP_DELETE_POR_PESSOA_FISICA = @"
            DELETE FROM BNE.TAB_Pessoa_Fisica_Idioma
            WHERE Idf_Pessoa_Fisica = @Idf_Pessoa_Fisica;";

        #region [spInativarTodosIdiomasPessoaFisica]
        private const string spInativarTodosIdiomasPessoaFisica = @"update TAB_Pessoa_fisica_Idioma set flg_inativo = 1 where idf_pessoa_fisica = @Idf_Pessoa_Fisica";
        #endregion

        #region ListarPorPessoaFisicaList
        /// <summary>
        /// Método responsável por retornar uma IDataReader com todas as instâncias de Formacao 
        /// </summary>
        /// <param name="idPessoaFisica">Código identificador de uma pessoa física</param>
        /// <returns></returns>
        public static IDataReader ListarPorPessoaFisica(int idPessoaFisica)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Int, 4));
            parms[0].Value = idPessoaFisica;

            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPORPESSOAFISICA, parms);
        }
        /// <summary>
        /// Método responsável por retornar uma IDataReader com todas as instâncias de Formacao 
        /// </summary>
        /// <param name="idPessoaFisica">Código identificador de uma pessoa física</param>
        /// <returns></returns>
        public static DataTable ListarPorPessoaFisicaDT(int idPessoaFisica)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Int, 4));
            parms[0].Value = idPessoaFisica;

            return DataAccessLayer.ExecuteReaderDs(CommandType.Text, SPSELECTPORPESSOAFISICA, parms).Tables[0];
        }
        #endregion

        #region CarregarPorPessoaFisicaIdioma
        public static bool CarregarPorPessoaFisicaIdioma(int idPessoaFisica, int idIdioma, out PessoafisicaIdioma objPessoafisicaIdioma)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Idioma", SqlDbType.Int, 4));
            parms[0].Value = idPessoaFisica;
            parms[1].Value = idIdioma;

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPORPESSOAFISICAIDIOMA, parms))
            {
                objPessoafisicaIdioma = new PessoafisicaIdioma();
                if (SetInstance(dr, objPessoafisicaIdioma))
                    return true;

                if (!dr.IsClosed)
                    dr.Close();
            }
            objPessoafisicaIdioma = null;
            return false;
        }
        #endregion

        #region Listar
        public static List<PessoafisicaIdioma> Listar(int idPessoaFisica)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Int, 4));
            parms[0].Value = idPessoaFisica;


            var lstPessoaFisicaIdioma = new List<PessoafisicaIdioma>();
            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPORPESSOAFISICA, parms))
            {
                var objPessoafisicaIdioma = new PessoafisicaIdioma();
                while (SetInstance(dr, objPessoafisicaIdioma))
                {
                    lstPessoaFisicaIdioma.Add(objPessoafisicaIdioma);
                    objPessoafisicaIdioma = new PessoafisicaIdioma();
                }

                if (!dr.IsClosed)
                    dr.Close();
            }
            return lstPessoaFisicaIdioma;
        }
        #endregion

        #region DeletePorPessoaFisica
        /// <summary>
        /// Apaga todas as formações da pessoa fisica que NAO sejam de aperfeicoamento/complementares
        /// </summary>
        /// <param name="idPessoaFisica"></param>
        public static void DeletePorPessoaFisica(int idPessoaFisica)
        {
            using (SqlConnection conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    DeletePorPessoaFisica(idPessoaFisica, trans);
                }
            }
        }

        /// <summary>
        /// Apaga todas as formações da pessoa fisica que NAO sejam de aperfeicoamento/complementares
        /// </summary>
        /// <param name="idPessoaFisica"></param>
        public static void DeletePorPessoaFisica(int idPessoaFisica, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Int, 4));
            parms[0].Value = idPessoaFisica;

            DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SP_DELETE_POR_PESSOA_FISICA, parms);
        }
        #endregion DeletePorPessoaFisica

        #region [InativarTodosIdiomasPessoaFisica]
        public static void InativarTodosIdiomasPessoaFisica(int Idf_Pessoa_Fisica, SqlTransaction trans = null)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Pessoa_Fisica", SqlDbType = SqlDbType.Int, Size = 4, Value = Idf_Pessoa_Fisica}
                };

            if (trans != null)
                DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, spInativarTodosIdiomasPessoaFisica, parms);
            else
                DataAccessLayer.ExecuteNonQuery(CommandType.Text, spInativarTodosIdiomasPessoaFisica, parms);
        }
        #endregion

        #region [ Migration Mapping ]
        /// <summary>
        /// Médodos e atributos auxiliares à migração de dados para o novo
        /// domínio.
        /// </summary>
        public int MigrationId
        {
            set
            {
                this._idPessoaFisicaIdioma = value;
            }
            get { return this._idPessoaFisicaIdioma; }
        }

        public DateTime MigrationDataCadastro
        {
            set
            {
                this._dataCadastro = value;
            }
            get { return this._dataCadastro; }
        }
        #endregion

    }
}