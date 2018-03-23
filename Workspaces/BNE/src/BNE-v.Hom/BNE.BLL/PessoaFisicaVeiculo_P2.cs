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

        private const string SPDELETEPORPESSOAFISICA = @"
        DELETE FROM BNE.TAB_Pessoa_Fisica_Veiculo
        WHERE Idf_Pessoa_Fisica = @Idf_Pessoa_Fisica";

        #region [spDeleteVeiculoPorPessoaFisica]
        private const string spDeleteVeiculoPorPessoaFisica = "delete Tab_Pessoa_Fisica_Veiculo where idf_pessoa_fisica = @Idf_Pessoa_Fisica ";
        #endregion


        #endregion

        #region ListarPessoaFisicaVeiculo
        /// <summary>
        /// Método responsável por retornar uma list com todas as instâncias de PessoaFisicaVeiculo 
        /// </summary>
        /// <param name="idPessoaFisica">Código identificador de uma pessoa física</param>
        /// <returns></returns>
        public static List<PessoaFisicaVeiculo> ListarPessoaFisicaVeiculo(int idPessoaFisica)
        {
            List<PessoaFisicaVeiculo> listPessoaFisicaVeiculo = new List<PessoaFisicaVeiculo>();

            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Int, 4));
            parms[0].Value = idPessoaFisica;

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPORPESSOAFISICA, parms))
            {
                var objPessoaFisicaVeiculo = new PessoaFisicaVeiculo();
                while (dr.Read() &&
                        PessoaFisicaVeiculo.SetInstance_NotDispose(dr, objPessoaFisicaVeiculo) &&
                        BLL.TipoVeiculo.SetInstance_NotDispose(dr, objPessoaFisicaVeiculo.TipoVeiculo))
                {
                    listPessoaFisicaVeiculo.Add(objPessoaFisicaVeiculo);
                }

                if (!dr.IsClosed)
                    dr.Close();
            }

            return listPessoaFisicaVeiculo;
        }
        #endregion

        #region ListarVeiculosDT
        /// <summary>
        /// Método responsável por retornar uma IDataReader com todas as instâncias de PessoaFisicaVeiculo 
        /// </summary>
        /// <param name="idPessoaFisica">Código identificador de uma pessoa física</param>
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
        /// Método responsável por retornar uma IDataReader com todas as instâncias de PessoaFisicaVeiculo 
        /// </summary>
        /// <param name="idPessoaFisica">Código identificador de uma pessoa física</param>
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
        /// Método utilizado para identificar se uma pessoa fisica possui um veiculo.
        /// </summary>
        /// <param name="idPessoaFisica">Código identificador de uma pessoa física</param>
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
        /// Método utilizado para identificar se uma pessoa fisica possui um veiculo.
        /// </summary>
        /// <param name="idPessoaFisica">Código identificador de uma pessoa física</param>
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

        public static void DeletarPorPessoaFisica(int idPessoaFisica, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Int, 4));
            parms[0].Value = idPessoaFisica;

            DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETEPORPESSOAFISICA, parms);
        }

        #region SetInstance_NotDispose
        /// <summary>
        /// Método auxiliar utilizado pelos métodos LoadObject e CompleteObject para percorrer um IDataReader e vincular as colunas com os atributos da classe.
        /// </summary>
        /// <param name="dr">Cursor de leitura do banco de dados.</param>
        /// <param name="objPessoaFisicaVeiculo">Instância a ser manipulada.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static bool SetInstance_NotDispose(IDataReader dr, PessoaFisicaVeiculo objPessoaFisicaVeiculo)
        {
            try
            {
                objPessoaFisicaVeiculo._idVeiculo = Convert.ToInt32(dr["Idf_Veiculo"]);
                objPessoaFisicaVeiculo._pessoaFisica = new PessoaFisica(Convert.ToInt32(dr["Idf_Pessoa_Fisica"]));
                objPessoaFisicaVeiculo._tipoVeiculo = new TipoVeiculo(Convert.ToInt32(dr["Idf_Tipo_Veiculo"]));
                if (dr["Des_Modelo"] != DBNull.Value)
                    objPessoaFisicaVeiculo._descricaoModelo = Convert.ToString(dr["Des_Modelo"]);
                objPessoaFisicaVeiculo._anoVeiculo = Convert.ToInt16(dr["Ano_Veiculo"]);
                objPessoaFisicaVeiculo._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
                objPessoaFisicaVeiculo._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
                if (dr["Flg_Veiculo_Proprio"] != DBNull.Value)
                    objPessoaFisicaVeiculo._flagVeiculoProprio = Convert.ToBoolean(dr["Flg_Veiculo_Proprio"]);
                if (dr["Des_placa_Veiculo"] != DBNull.Value)
                    objPessoaFisicaVeiculo._descricaoplacaVeiculo = Convert.ToString(dr["Des_placa_Veiculo"]);
                if (dr["Num_Renavam"] != DBNull.Value)
                    objPessoaFisicaVeiculo._numeroRenavam = Convert.ToString(dr["Num_Renavam"]);
                if (dr["Flg_Seguro_Veiculo"] != DBNull.Value)
                    objPessoaFisicaVeiculo._flagSeguroVeiculo = Convert.ToBoolean(dr["Flg_Seguro_Veiculo"]);
                if (dr["Dta_Vencimento_Seguro"] != DBNull.Value)
                    objPessoaFisicaVeiculo._dataVencimentoSeguro = Convert.ToDateTime(dr["Dta_Vencimento_Seguro"]);

                objPessoaFisicaVeiculo._persisted = true;
                objPessoaFisicaVeiculo._modified = false;

                return true;
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region [DeleteVeiculoPorPessoaFisica]
        /// <summary>
        /// Deleta todos os veiculos da pessoa fisica
        /// </summary>
        /// <param name="idPessoaFisica"></param>
        public static void DeleteVeiculoPorPessoaFisica(int idPessoaFisica, SqlTransaction trans)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Int, 4)
                };
            parms[0].Value = idPessoaFisica;

            DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, spDeleteVeiculoPorPessoaFisica, parms);
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
                this._idVeiculo = value;
            }
            get { return this._idVeiculo; }
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