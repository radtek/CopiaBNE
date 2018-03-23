//-- Data: 02/03/2010 09:17
//-- Autor: Gieyson Stelmak

using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;
namespace BNE.BLL
{
	public partial class TipoVeiculo // Tabela: TAB_Tipo_Veiculo
    {
        #region Consulta 
        private const string SELECTTIPOVEICULO  = @"SELECT 
                                                        Idf_Tipo_Veiculo,
                                                        Des_Tipo_Veiculo
                                                    FROM
                                                    plataforma.TAB_Tipo_Veiculo";

        private const string SPSELECTPORDESCRICAO = @"  SELECT 
                                                        *
                                                        FROM plataforma.TAB_Tipo_Veiculo 
                                                        WHERE Des_Tipo_Veiculo = @Des_Tipo_Veiculo";

        #endregion

        #region Listar
        /// <summary>
        /// Método retorna os registros da tabela tipo veiculo
        /// </summary>
        /// <returns></returns>
        public static IDataReader Listar()
        {
            return DataAccessLayer.ExecuteReader(CommandType.Text,SELECTTIPOVEICULO,null); 
        }
        #endregion

        #region CarregarPorDescricao
        /// <summary>
        /// Método responsável por carregar uma instancia de TipoVeiculo através da sua descricao
        /// </summary>
        /// <param name="idPessoaFisica">Identificador da Pessoa Física</param>
        /// <returns>TipoVeiculo</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static TipoVeiculo CarregarPorDescricao(string desTipoVeiculo)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Des_Tipo_Veiculo", SqlDbType.VarChar, 100));
            parms[0].Value = desTipoVeiculo;

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPORDESCRICAO, parms))
            {
                TipoVeiculo objTipoVeiculo = new TipoVeiculo();
                if (SetInstance(dr, objTipoVeiculo))
                    return objTipoVeiculo;

                if (!dr.IsClosed)
                    dr.Close();
            }
            throw (new RecordNotFoundException(typeof(PessoaFisica)));
        }
        #endregion
    }
}