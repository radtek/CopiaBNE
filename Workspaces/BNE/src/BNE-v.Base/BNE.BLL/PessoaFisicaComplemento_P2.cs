//-- Data: 09/03/2010 17:06
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
namespace BNE.BLL
{
	public partial class PessoaFisicaComplemento // Tabela: TAB_Pessoa_Fisica_Complemento
    {

        #region Métodos

        #region CarregarPorPessoaFisica
        /// <summary>
        /// Método utilizado para retornar uma instância de Pessoa Fisica Completo a partir do banco de dados.
        /// </summary>
        /// <param name="codigo">identificador da pessoa fisica</param>
        /// <param name="objPessoaFisicaComplemento">objeto de saída </param>
        /// <returns></returns>
        public static bool CarregarPorPessoaFisica(int idPessoaFisica, out PessoaFisicaComplemento objPessoaFisicaComplemento)
        {
            using (IDataReader dr = LoadDataReader(idPessoaFisica))
            {
                objPessoaFisicaComplemento = new PessoaFisicaComplemento();
                if (SetInstance(dr, objPessoaFisicaComplemento))
                    return true;

                if (!dr.IsClosed)
                    dr.Close();
            }
            objPessoaFisicaComplemento = null;
            return false;
        }
        #endregion

        #region SalvarIntegracao
        public void SalvarIntegracao(DateTime dtaAlteracao)
        {
            if (!this._persisted)
                InsertIntegracao(dtaAlteracao);
            else
                UpdateIntegracao(dtaAlteracao);
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

        #region UpdateIntegracao
        public void UpdateIntegracao(DateTime dtaAlteracao)
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

                        DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPUPDATE, parms);
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

        #endregion

	}
}