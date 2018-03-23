//-- Data: 24/07/2012 09:46
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
namespace BNE.BLL
{
    public partial class CurriculoDisponibilidadeCidade // Tabela: BNE_Curriculo_Disponibilidade_Cidade
    {

        #region Querys

        private const string Spselectporcurriculo = @"
        SELECT  CI.Nme_Cidade + '/' + CI.Sig_Estado as 'Nme_Cidade', Idf_Curriculo_Disponibilidade_Cidade, CDC.Flg_Inativo 
        FROM    BNE_Curriculo_Disponibilidade_Cidade AS CDC WITH(NOLOCK)
                INNER JOIN plataforma.TAB_Cidade AS CI WITH(NOLOCK) ON CDC.Idf_Cidade = CI.Idf_Cidade 
        WHERE   Idf_Curriculo = @Idf_Curriculo 
                AND CDC.Flg_Inativo = 0";

        private const string Spselectporcurriculocidade = @"
        SELECT  *
        FROM    BNE_Curriculo_Disponibilidade_Cidade AS CDC WITH(NOLOCK)
        WHERE   Idf_Curriculo = @Idf_Curriculo 
                AND Idf_Cidade = @Idf_Cidade";


        #region [spDeleteDisponibilidadePorCurriculo]
        private const string spDeleteDisponibilidadePorCurriculo = "delete BNE_Curriculo_Disponibilidade_Cidade where Idf_Curriculo = @Idf_Curriculo";
        #endregion
        #endregion

        #region Métodos

        #region ListarCidades

        /// <summary>
        /// Método que Lista as Cidades Disponiveis por Curriculo
        /// </summary>
        /// <param name="idCurriculo">Idf do Curriculo Selecionado</param>
        /// <returns>Retornar um DataTable contendo uma coluna com as cidades do curriculo informado</returns>
        public static DataTable ListarCidadesPorCurriculo(int idCurriculo)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4)
                };
            parms[0].Value = idCurriculo;

            return DataAccessLayer.ExecuteReaderDs(CommandType.Text, Spselectporcurriculo, parms).Tables[0];
        }

        /// <summary>
        /// Lista as Cidades pelo Id do Curriculo
        /// </summary>
        /// <param name="idCurriculo">Idf dp Curriculo Selecionado</param>
        /// <returns>Retornar uma lista de Cidades com o nome da Cidade</returns>
        public static List<String> ListarCidade(int idCurriculo)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4)
                };
            parms[0].Value = idCurriculo;

            var listString = new List<String>();
            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spselectporcurriculo, parms))
            {
                while (dr.Read())
                    listString.Add(dr["Nme_Cidade"].ToString());

                if (!dr.IsClosed)
                    dr.Close();
            }

            return listString;
        }

        #endregion

        #region CarregarPorCurriculoCidade
        /// <summary>
        /// Método responsável por carregar uma instancia de Curriculo Disponbilidade Cidade dado determinado Curriculo e uma Cidade
        /// </summary>
        /// <param name="objCurriculo">Curriculo em questão</param>
        /// <param name="objCidade">Instância de Cidade </param>
        /// <param name="objCurriculoDisponibilidadeCidade">Curriculo Disponbilidade Cidade</param>
        /// <returns>True caso exista</returns>
        public static bool CarregarPorCurriculoCidade(Curriculo objCurriculo, Cidade objCidade, out CurriculoDisponibilidadeCidade objCurriculoDisponibilidadeCidade)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Curriculo", SqlDbType = SqlDbType.Int, Size = 4, Value = objCurriculo.IdCurriculo} ,
                    new SqlParameter { ParameterName = "@Idf_Cidade", SqlDbType = SqlDbType.Int, Size = 4, Value = objCidade.IdCidade}
                };

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spselectporcurriculocidade, parms))
            {
                objCurriculoDisponibilidadeCidade = new CurriculoDisponibilidadeCidade();
                if (SetInstance(dr, objCurriculoDisponibilidadeCidade))
                    return true;
                dr.Close();
            }
            objCurriculoDisponibilidadeCidade = null;
            return false;
        }
        #endregion

        #region Save
        /// <summary>
        /// Método sobrescrito utilizado para salvar uma instância de CurriculoDisponibilidadeCidade no banco de dados.
        /// </summary>
        /// <remarks>Luan Fernandes</remarks>
        public void Save()
        {
            if (!this._persisted)
                this.Insert();
            else
                this.Update();

            //AlertaCurriculos.OnAlterarCurriculo(this.Curriculo);
        }
        /// <summary>
        /// Método sobrescrito utilizado para salvar uma instância de CurriculoDisponibilidadeCidade no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Luan Fernandes</remarks>
        public void Save(SqlTransaction trans)
        {
            if (!this._persisted)
                this.Insert(trans);
            else
                this.Update(trans);

            //AlertaCurriculos.OnAlterarCurriculo(this.Curriculo);
        }
        #endregion


        #region [DeleteDisponibilidadePorCurriculo]
        /// <summary>
        /// Deleta todas as disponibilidade cidade do curriculo
        /// </summary>
        /// <param name="idCurriculo"></param>
        public static void DeleteDisponibilidadePorCurriculo(int idCurriculo, SqlTransaction trans)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4)
                };
            parms[0].Value = idCurriculo;

            DataAccessLayer.ExecuteNonQuery(trans,CommandType.Text, spDeleteDisponibilidadePorCurriculo, parms);
        }
        #endregion
        #endregion

    }
}