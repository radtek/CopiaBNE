//-- Data: 05/10/2011 11:31
//-- Autor: Gieyson Stelmak

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
namespace BNE.BLL
{
	public partial class ListaCidade // Tabela: BNE_Lista_Cidade
    {

        #region Consultas
        private const string SPSELECTPORCIDADEGRUPO = @"
        SELECT  LC.* 
        FROM    BNE_Lista_Cidade LC WITH ( NOLOCK )
        WHERE   LC.Idf_Grupo_Cidade = @Idf_Grupo_Cidade 
                AND LC.Idf_Cidade = @Idf_Cidade";
        #endregion

        #region ListarPorCidadeGrupo
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados.
        /// </summary>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static IDataReader ListarPorCidadeGrupo(Cidade objCidade, GrupoCidade objGrupoCidade, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Cidade", SqlDbType.Int, 100));
            parms.Add(new SqlParameter("@Idf_Grupo_Cidade", SqlDbType.Int, 100));

            parms[0].Value = objCidade.IdCidade;
            parms[1].Value = objGrupoCidade.IdGrupoCidade;

            return DataAccessLayer.ExecuteReader(trans, CommandType.Text, SPSELECTPORCIDADEGRUPO, parms);
        }
        #endregion

        #region CarregarPorCidadeGrupo
        public static bool CarregarPorCidadeGrupo(Cidade objCidade, GrupoCidade objGrupoCidade, SqlTransaction trans, out ListaCidade objListaCidade)
        {
            using (IDataReader dr = ListarPorCidadeGrupo(objCidade, objGrupoCidade, trans))
            {
                objListaCidade = new ListaCidade();
                if (SetInstance(dr, objListaCidade))
                    return true;

                if (!dr.IsClosed)
                    dr.Close();
            }
            objListaCidade = null;

            return false;
        }
        #endregion

        #region Inserção em Massa
        /// <summary>
        /// Crie uma tabela para inserir em massa.
        /// Passe um DataTable nulo para e a classe em populada. 
        /// Ela irá instanciar um novo DataTable já com colunas definidas apartir dos parâmetros sql definidos na classe.
        /// Os valores setados nas propriedades são transformados em uma linha na tabela.
        /// </summary>
        /// <param name="dt"></param>
        public void AddBulkTable(ref DataTable dt)
        {
            DataAccessLayer.AddBulkTable(ref dt, this);
        }

        /// <summary>
        /// Realiza inserção em massa.
        /// </summary>
        /// <param name="dt">Tabela criada pelo método AddBulkTable</param>
        /// <param name="trans">Transação</param>
        public static void SaveBulkTable(DataTable dt, SqlTransaction trans)
        {
            DataAccessLayer.SaveBulkTable(dt, "BNE_Lista_Cidade", trans);
        }
        #endregion

    }
}