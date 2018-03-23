//-- Data: 09/03/2010 17:06
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;


namespace BNE.BLL
{
	public partial class PessoaFisicaVeiculo // Tabela: Tab_Pessoa_Fisica_Veiculo
    {

        #region Consultas
        private const string SPSELECTPORPESSOAFISICA = @"
        SELECT 
                * 
        FROM    Tab_Pessoa_Fisica_Veiculo PFV WITH(NOLOCK)
                INNER JOIN plataforma.TAB_Tipo_Veiculo TV WITH(NOLOCK) ON PFV.Idf_Tipo_Veiculo = TV.Idf_Tipo_Veiculo
        WHERE 
                Idf_Pessoa_Fisica = @Idf_Pessoa_Fisica AND
                PFV.Flg_Inativo = 0";

        private const string SPSELECTCOUNTPORPESSOAFISICA = @"  
        SELECT 
            COUNT(Idf_Veiculo) 
        FROM Tab_Pessoa_Fisica_Veiculo PFV WITH(NOLOCK)
        WHERE Idf_Pessoa_Fisica = @Idf_Pessoa_Fisica";

        private const string SPSELECTCOUNTPORPESSOAFISICATIPOVEICULO = @"  
        SELECT 
                COUNT(Idf_Veiculo)
        FROM    Tab_Pessoa_Fisica_Veiculo PFV WITH(NOLOCK)
        WHERE   Idf_Pessoa_Fisica = @Idf_Pessoa_Fisica 
                AND Idf_Tipo_Veiculo = @Idf_Tipo_Veiculo";
        
        #endregion

        #region ListarPessoaFisicaVeiculo
        /// <summary>
        /// M�todo respons�vel por retornar uma list com todas as inst�ncias de PessoaFisicaVeiculo 
        /// </summary>
        /// <param name="idPessoaFisica">C�digo identificador de uma pessoa f�sica</param>
        /// <returns></returns>
        public static List<PessoaFisicaVeiculo> ListarPessoaFisicaVeiculo(int idPessoaFisica)
        {
            List<PessoaFisicaVeiculo> listPessoaFisicaVeiculo = new List<PessoaFisicaVeiculo>();

            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Int, 4));
            parms[0].Value = idPessoaFisica;

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPORPESSOAFISICA, parms))
            {
                while (dr.Read())
                    listPessoaFisicaVeiculo.Add(PessoaFisicaVeiculo.LoadObject(Convert.ToInt32(dr["Idf_Veiculo"])));

                if (!dr.IsClosed)
                    dr.Close();
            }

            return listPessoaFisicaVeiculo;
        }
        #endregion

        #region ListarVeiculosDT
        /// <summary>
        /// M�todo respons�vel por retornar uma IDataReader com todas as inst�ncias de PessoaFisicaVeiculo 
        /// </summary>
        /// <param name="idPessoaFisica">C�digo identificador de uma pessoa f�sica</param>
        /// <returns></returns>
        public static DataTable ListarVeiculosDT(int idPessoaFisica)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Int, 4));
            parms[0].Value = idPessoaFisica;

            return DataAccessLayer.ExecuteReaderDs(CommandType.Text, SPSELECTPORPESSOAFISICA, parms).Tables[0];
        }
        #endregion

        #region ListarVeiculos
        /// <summary>
        /// M�todo respons�vel por retornar uma IDataReader com todas as inst�ncias de PessoaFisicaVeiculo 
        /// </summary>
        /// <param name="idPessoaFisica">C�digo identificador de uma pessoa f�sica</param>
        /// <returns></returns>
        public static IDataReader ListarVeiculos(int idPessoaFisica)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Int, 4));
            parms[0].Value = idPessoaFisica;

            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPORPESSOAFISICA, parms);
        }
        #endregion

        #region ExisteVeiculo
        /// <summary>
        /// M�todo utilizado para identificar se uma pessoa fisica possui um veiculo.
        /// </summary>
        /// <param name="idPessoaFisica">C�digo identificador de uma pessoa f�sica</param>
        /// <returns></returns>
        public static bool ExisteVeiculo(int idPessoaFisica)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Int, 4));
            parms[0].Value = idPessoaFisica;

            return Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNTPORPESSOAFISICA, parms)) > 0;
        }
        #endregion

        #region ExisteVeiculoPorTipo
        /// <summary>
        /// M�todo utilizado para identificar se uma pessoa fisica possui um veiculo.
        /// </summary>
        /// <param name="idPessoaFisica">C�digo identificador de uma pessoa f�sica</param>
        /// <returns></returns>
        public static bool ExisteVeiculoPorTipo(int idPessoaFisica, int idTipoVeiculo)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Tipo_Veiculo", SqlDbType.Int, 4));
            parms[0].Value = idPessoaFisica;
            parms[1].Value = idTipoVeiculo;

            return Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNTPORPESSOAFISICATIPOVEICULO, parms)) > 0;
        }
        #endregion

    }
}