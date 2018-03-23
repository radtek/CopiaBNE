//-- Data: 02/03/2010 09:20
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BNE.BLL
{
    public partial class CurriculoVisualizacao // Tabela: BNE_Curriculo_Visualizacao
    {

        #region Consultas

        #region Spselectcurriculofilial
        private const string Spselectcurriculofilial = @"
        SELECT  *
        FROM    BNE_Curriculo_Visualizacao WITH(NOLOCK)
        WHERE   Idf_Curriculo = @Idf_Curriculo
                AND Idf_Filial = @Idf_Filial
                AND CONVERT( DATETIME, CONVERT(VARCHAR, GETDATE(), 103), 103)  BETWEEN CONVERT( DATETIME, CONVERT(VARCHAR, Dta_Visualizacao, 103), 103)
                AND CONVERT( DATETIME, CONVERT(VARCHAR, Dta_Visualizacao + @Dias_ReVisualizacao, 103), 103)
        ";
        #endregion

        #region Spselectcurriculosvisualizados
        private const string Spselectcurriculosvisualizados = @"
        SELECT  count(CV.Idf_Curriculo)
        FROM    BNE.BNE_Curriculo_Visualizacao CV WITH(NOLOCK)
                INNER JOIN BNE.BNE_Curriculo C ON CV.Idf_Curriculo = C.Idf_Curriculo
        WHERE   Idf_Filial = @Idf_Filial AND CV.Dta_Visualizacao > C.Dta_Atualizacao
        ";
        #endregion

        #endregion

        #region Métodos

        #region CarregarPorCurriculoFilial
        /// <summary>
        /// Método responsável por carregar uma instancia de BNE_Curriculo_Visualizacao através do
        /// identificar de um Curriculo e uma Filial. Levando em conta o periodo de revisualizacao
        /// </summary>
        /// <param name="objCurriculo">Identificador do Curriculo</param>
        /// <param name="objFilial">Identificador de Filial</param>
        /// <param name="qtdDiasRevisualizacao"> </param>
        /// <param name="objCurriculoVisualizacao"> </param>
        /// <returns>Boolean</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static bool CarregarPorCurriculoFilial(Curriculo objCurriculo, Filial objFilial, int qtdDiasRevisualizacao, out CurriculoVisualizacao objCurriculoVisualizacao)
        {
            return CarregarPorCurriculoFilial(objCurriculo, objFilial, qtdDiasRevisualizacao, null, out objCurriculoVisualizacao);
        }
        public static bool CarregarPorCurriculoFilial(Curriculo objCurriculo, Filial objFilial, int qtdDiasRevisualizacao, SqlTransaction trans, out CurriculoVisualizacao objCurriculoVisualizacao)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter{ ParameterName = "@Idf_Curriculo", SqlDbType = SqlDbType.Int, Size = 4, Value = objCurriculo.IdCurriculo}, 
                    new SqlParameter{ ParameterName = "@Idf_Filial", SqlDbType = SqlDbType.Int, Size = 4, Value = objFilial.IdFilial}, 
                    new SqlParameter{ ParameterName = "@Dias_ReVisualizacao", SqlDbType = SqlDbType.Int, Size = 4, Value = qtdDiasRevisualizacao}
                };

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spselectcurriculofilial, parms))
            {
                objCurriculoVisualizacao = new CurriculoVisualizacao();
                if (SetInstance(dr, objCurriculoVisualizacao))
                    return true;

                if (!dr.IsClosed)
                    dr.Close();
            }
            objCurriculoVisualizacao = null;
            return false;
        }
        #endregion

        #region ListarQuantidadeCurriculoVisualizados
        /// <summary>
        /// Método utilizado para retornar os curriculo vizualidados pela empresa, levando em conta se o currículo foi atualizado após a vizualização ou não.
        /// </summary>
        /// <param name="idFilial"></param>
        /// <returns></returns>
        public static int ListarQuantidadeCurriculoVisualizados(int idFilial)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter{ ParameterName = "@Idf_Filial", SqlDbType = SqlDbType.Int, Size = 4, Value = idFilial}, 
                };

            return Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, Spselectcurriculosvisualizados, parms));
        }
        #endregion

        #region CarregarSalaAdministradora
        /// <summary>
        /// Carrega a lista de curriculos visualizados na sala administradora
        /// </summary>
        /// <param name="idFilial">A filial ativa</param>
        /// <param name="filtro">O filtro</param>
        /// <returns>Uma DataTable contendo os registros</returns>
        public static DataTable CarregarSalaAdministradora(int idFilial, String filtro, int paginalAtual, int tamanhoPagina, out int totalRegistros)
        {
            object valueFiltro = DBNull.Value;
            if (!string.IsNullOrWhiteSpace(filtro))
                valueFiltro = filtro;

            var parms = new List<SqlParameter>
                {
                    new SqlParameter{ ParameterName = "@Idf_Filial", SqlDbType = SqlDbType.Int, Size = 4, Value = idFilial}, 
                    new SqlParameter{ ParameterName = "@Filtro", SqlDbType = SqlDbType.VarChar, Size = 50, Value = valueFiltro}, 
                    new SqlParameter{ ParameterName = "@CurrentPage", SqlDbType = SqlDbType.Int, Size = 4, Value = paginalAtual}, 
                    new SqlParameter{ ParameterName = "@PageSize", SqlDbType = SqlDbType.Int, Size = 4, Value = tamanhoPagina} 
                };

            totalRegistros = 0;
            DataTable dt = null;
            try
            {
                using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.StoredProcedure, "SP_Visualizacao_Empresa", parms))
                {
                    if (dr.Read())
                        totalRegistros = Convert.ToInt32(dr[0]);

                    dr.NextResult();
                    dt = new DataTable();
                    dt.Load(dr);
                }
            }
            finally
            {
                if (dt != null)
                    dt.Dispose();
            }

            return dt;
        }
        #endregion

        #region FilialPodeVerDadosCurriculo
        /// <summary>
        /// Metodo responsável por verificar se o usuário possui saldo de acordo com o plano atual, e efetua o debito deste saldo. 
        /// </summary>
        /// <param name="objFilial"></param>
        /// <param name="objCurriculo"></param>
        /// <param name="avalDeVisualizacaoPorWebEstagios"></param>
        public static bool FilialPodeVerDadosCurriculo(Filial objFilial, Curriculo objCurriculo, bool avalDeVisualizacaoPorWebEstagios)
        {
            string recuperaValorParametro = Parametro.RecuperaValorParametro(Enumeradores.Parametro.PeriodoReVisualizacaoCurriculo);

            int diasRevisualizacao;
            if (recuperaValorParametro != null)
                diasRevisualizacao = Int32.Parse(recuperaValorParametro);
            else
                diasRevisualizacao = 0;

            using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();

                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        bool podeVerDados;

                        CurriculoVisualizacao objCurriculoVisualizacao;
                        if (CurriculoVisualizacao.CarregarPorCurriculoFilial(objCurriculo, objFilial, diasRevisualizacao, trans, out objCurriculoVisualizacao))
                            podeVerDados = true;
                        else
                        {
                            PlanoQuantidade objPlanoQuantidade;
                            if (PlanoQuantidade.CarregarPlanoAtualVigente(trans, objFilial, out objPlanoQuantidade))
                            {
                                int saldoDisponivel = objPlanoQuantidade.QuantidadeVisualizacao - objPlanoQuantidade.QuantidadeVisualizacaoUtilizado;

                                if (saldoDisponivel > 0)
                                {
                                    objPlanoQuantidade.QuantidadeVisualizacaoUtilizado++;
                                    objPlanoQuantidade.Save(trans);

                                    objCurriculoVisualizacao = new CurriculoVisualizacao
                                        {
                                            Filial = objFilial,
                                            Curriculo = objCurriculo,
                                            DataVisualizacao = DateTime.Now
                                        };

                                    objCurriculoVisualizacao.Save(trans);
                                    podeVerDados = true;
                                }
                                else
                                {
                                    podeVerDados = avalDeVisualizacaoPorWebEstagios;
                                }
                            }
                            else
                            {
                                podeVerDados = avalDeVisualizacaoPorWebEstagios;
                            }
                        }

                        trans.Commit();
                        return podeVerDados;
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

        #endregion

    }
}