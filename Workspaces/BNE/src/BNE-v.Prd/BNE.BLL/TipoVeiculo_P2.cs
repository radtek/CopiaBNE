//-- Data: 02/03/2010 09:17
//-- Autor: Gieyson Stelmak

using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;
using System;

namespace BNE.BLL
{
    public partial class TipoVeiculo // Tabela: TAB_Tipo_Veiculo
    {
        #region Consulta 
        private const string SELECTTIPOVEICULO = @"SELECT 
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
        /// M�todo retorna os registros da tabela tipo veiculo
        /// </summary>
        /// <returns></returns>
        public static IDataReader Listar()
        {
            return DataAccessLayer.ExecuteReader(CommandType.Text, SELECTTIPOVEICULO, null);
        }
        #endregion

        #region CarregarPorDescricao
        /// <summary>
        /// M�todo respons�vel por carregar uma instancia de TipoVeiculo atrav�s da sua descricao
        /// </summary>
        /// <param name="idPessoaFisica">Identificador da Pessoa F�sica</param>
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

        #region SetInstance_NotDispose
        /// <summary>
        /// M�todo auxiliar utilizado pelos m�todos LoadObject e CompleteObject para percorrer um IDataReader e vincular as colunas com os atributos da classe.
        /// </summary>
        /// <param name="dr">Cursor de leitura do banco de dados.</param>
        /// <param name="objTipoVeiculo">Inst�ncia a ser manipulada.</param>
        /// <returns>Verdadeiro ou falso informando se a opera��o foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static bool SetInstance_NotDispose(IDataReader dr, TipoVeiculo objTipoVeiculo)
        {
            try
            {
                objTipoVeiculo._idTipoVeiculo = Convert.ToInt32(dr["Idf_Tipo_Veiculo"]);
                objTipoVeiculo._descricaoTipoVeiculo = Convert.ToString(dr["Des_Tipo_Veiculo"]);
                objTipoVeiculo._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
                objTipoVeiculo._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);

                objTipoVeiculo._persisted = true;
                objTipoVeiculo._modified = false;

                return true;
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region [ Migration Mapping ]
        /// <summary>
        /// M�dodos e atributos auxiliares � migra��o de dados para o novo
        /// dom�nio.
        /// </summary>
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