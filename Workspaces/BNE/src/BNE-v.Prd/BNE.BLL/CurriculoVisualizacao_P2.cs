//-- Data: 02/03/2010 09:20
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.Domain.Events.CrossDomainEvents;
using BNE.Domain.Events.Handler;

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
        private const string Spfilialvisualizoucurriculo = @"
        SELECT  count(*)
        FROM    BNE_Curriculo_Visualizacao WITH(NOLOCK)
        WHERE   Idf_Curriculo = @Idf_Curriculo
                AND Idf_Filial = @Idf_Filial
                AND CONVERT( DATETIME, CONVERT(VARCHAR, GETDATE(), 103), 103)  BETWEEN CONVERT( DATETIME, CONVERT(VARCHAR, Dta_Visualizacao, 103), 103)
                AND CONVERT( DATETIME, CONVERT(VARCHAR, Dta_Visualizacao + @Dias_ReVisualizacao, 103), 103)
        ";
        #endregion

        #region Spselectcurriculosvisualizados
        private const string Spselectcurriculosvisualizados = @"
        SELECT  Qtd_Visualizacao_Utilizado
        FROM    BNE.BNE_Plano_Adquirido PA
                JOIN BNE.BNE_Plano_Quantidade PQ ON PQ.Idf_Plano_Adquirido = PA.Idf_Plano_Adquirido
        WHERE   Idf_Filial = @Idf_Filial
                AND PA.Idf_Plano_Situacao = 1
        ";
        #endregion

        #region [spQuantidadeCurriculosNaoVisuazados]
        private const string spQuantidadeCurriculosNaoVisuazados = @"SELECT  COUNT(*) qtd
FROM  BNE.BNE_Vaga_Candidato VCSub WITH ( NOLOCK )
    JOIN BNE.BNE_Vaga v WITH ( NOLOCK ) ON VCSub.Idf_Vaga = v.Idf_Vaga
WHERE  Dta_Visualizacao IS NULL
    AND VCSub.Flg_Inativo = 0
	AND v.flg_inativo = 0
    AND Idf_Filial = @Idf_Filial ";
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
        /// <param name="objCurriculoVisualizacao"> </param>
        /// <returns>Boolean</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static bool CarregarPorCurriculoFilial(Curriculo objCurriculo, Filial objFilial, out CurriculoVisualizacao objCurriculoVisualizacao)
        {
            return CarregarPorCurriculoFilial(objCurriculo, objFilial, null, out objCurriculoVisualizacao);
        }
        public static bool CarregarPorCurriculoFilial(Curriculo objCurriculo, Filial objFilial, SqlTransaction trans, out CurriculoVisualizacao objCurriculoVisualizacao)
        {
            int diasRevisualizacaoCV = Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.PeriodoReVisualizacaoCurriculo));

            var parms = new List<SqlParameter>
                {
                    new SqlParameter{ ParameterName = "@Idf_Curriculo", SqlDbType = SqlDbType.Int, Size = 4, Value = objCurriculo.IdCurriculo},
                    new SqlParameter{ ParameterName = "@Idf_Filial", SqlDbType = SqlDbType.Int, Size = 4, Value = objFilial.IdFilial},
                    new SqlParameter{ ParameterName = "@Dias_ReVisualizacao", SqlDbType = SqlDbType.Int, Size = 4, Value = diasRevisualizacaoCV}
                };

            using (IDataReader dr = trans != null ?
                DataAccessLayer.ExecuteReader(trans, CommandType.Text, Spselectcurriculofilial, parms) :
                DataAccessLayer.ExecuteReader(CommandType.Text, Spselectcurriculofilial, parms))
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

        #region VerificarVisualizacaoCV
        public static bool VerificarVisualizacaoCV(Curriculo curriculo, Filial filial, SqlTransaction trans = null)
        {
            int diasRevisualizacaoCV = Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.PeriodoReVisualizacaoCurriculo));

            var parms = new List<SqlParameter>
                {
                    new SqlParameter{ ParameterName = "@Idf_Curriculo", SqlDbType = SqlDbType.Int, Size = 4, Value = curriculo.IdCurriculo},
                    new SqlParameter{ ParameterName = "@Idf_Filial", SqlDbType = SqlDbType.Int, Size = 4, Value = filial.IdFilial},
                    new SqlParameter{ ParameterName = "@Dias_ReVisualizacao", SqlDbType = SqlDbType.Int, Size = 4, Value = diasRevisualizacaoCV}
                };

            if (trans != null)
            {
                return Convert.ToInt32(DataAccessLayer.ExecuteScalar(trans, CommandType.Text, Spfilialvisualizoucurriculo, parms)) > 0;
            }

            return Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, Spfilialvisualizoucurriculo, parms)) > 0;
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
        public static bool FilialPodeVerDadosCurriculo(Filial objFilial, Curriculo objCurriculo, bool avalDeVisualizacaoPorWebEstagios, SqlTransaction trans)
        {
            try
            {
                bool podeVerDados;

                if (VerificarVisualizacaoCV(objCurriculo, objFilial, trans))
                    podeVerDados = true;
                else
                {
                    PlanoAdquirido objPlanoAdquirido;
                    if (PlanoAdquirido.CarregarPlanoAdquiridoPorSituacao(objFilial, (int)Enumeradores.PlanoSituacao.Liberado, out objPlanoAdquirido, trans))
                    {
                        if (objPlanoAdquirido.SaldoVisualizacao(trans) > 0)
                        {
                            objPlanoAdquirido.DescontarVisualizacao(trans);

                            var objCurriculoVisualizacao = new CurriculoVisualizacao
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
                            DomainEventsHandler.Handle(new OnVisualizacaoCurriculoSemSaldo(objCurriculo.IdCurriculo, objFilial.NumeroCNPJ.Value));
                            podeVerDados = avalDeVisualizacaoPorWebEstagios;
                        }
                    }
                    else
                    {
                        podeVerDados = avalDeVisualizacaoPorWebEstagios;
                    }

                }
                return podeVerDados;
            }
            catch
            {
                return false;
            }
        }
        public static bool FilialPodeVerDadosCurriculo(Filial objFilial, Curriculo objCurriculo, bool avalDeVisualizacaoPorWebEstagios)
        {
            using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();

                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        var podeVerDados = FilialPodeVerDadosCurriculo(objFilial, objCurriculo, avalDeVisualizacaoPorWebEstagios, trans);
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

        #region VisualizacaoDeCurriculo
        /// <summary>
        /// Lógica para descontar saldo e salvar visualizacao de currículo para um plano adquirido específico
        /// </summary>
        /// <param name="objPlanoAdquirido"></param>
        /// <param name="objCurriculo"></param>
        public static bool VisualizacaoDeCurriculo(PlanoAdquirido objPlanoAdquirido, Curriculo objCurriculo)
        {
            bool podeVerDados = false;

            using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();

                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        if (VerificarVisualizacaoCV(objCurriculo, objPlanoAdquirido.Filial, trans))
                            podeVerDados = true;
                        else
                        {
                            if (objPlanoAdquirido.SaldoVisualizacao(trans) > 0)
                            {
                                objPlanoAdquirido.DescontarVisualizacao(trans);

                                var objCurriculoVisualizacao = new CurriculoVisualizacao
                                {
                                    Filial = objPlanoAdquirido.Filial,
                                    Curriculo = objCurriculo,
                                    DataVisualizacao = DateTime.Now
                                };

                                objCurriculoVisualizacao.Save(trans);
                                podeVerDados = true;
                            }
                        }

                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        EL.GerenciadorException.GravarExcecao(ex);
                    }
                }
            }
            return podeVerDados;
        }
        #endregion

        #region [QuantidadeCurriculosNaoVisuazados]
        /// <summary>
        /// Quantidade de curriculos candidatos as vagada da filial que não foram visualizados.
        /// </summary>
        /// <param name="idFilial"></param>
        /// <returns></returns>
        public static int QuantidadeCurriculosNaoVisuazados(int idFilial)
        {

            var parms = new List<SqlParameter>
                {
                    new SqlParameter{ ParameterName = "@Idf_Filial", SqlDbType = SqlDbType.Int, Size = 4, Value = idFilial},
                };

            int total = 0;
            try
            {
                using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, spQuantidadeCurriculosNaoVisuazados, parms))
                {
                    if (dr.Read())
                        total = Convert.ToInt32(dr["qtd"]);
                }
            }
            catch
            {

            }
            return total;
        }
        #endregion

        #region [ListaCandidatosNaoVisualizados]
        /// <summary>
        /// Lista ate 6 curriculos que nao foram visualizados pela filial, com perfil da sua ultima vaga anunciada.
        /// </summary>
        /// <param name="idFilial"></param>
        /// <returns></returns>
        public static DataTable ListaCandidatosNaoVisualizados(int idFilial, int? idPesquisaCurriculo, out int Total)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Filial", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Pesquisa_Curriculo", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Qtd_curriculos", SqlDbType.Int, 4) { Direction = ParameterDirection.Output });

            parms[0].Value = idFilial;
            if (idPesquisaCurriculo.HasValue)
                parms[1].Value = idPesquisaCurriculo;
            else
                parms[1].Value = DBNull.Value;
            parms[2].Value = 0;
            Total = 0;

            DataTable dt = null;
            try
            {
                using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.StoredProcedure, "BNE.SP_Curriculos_Ultima_Vaga", parms))
                {
                    dt = new DataTable();
                    dt.Columns.Add("Nome", typeof(string));
                    dt.Columns.Add("Idade", typeof(int));
                    dt.Columns.Add("Funcao", typeof(string));
                    dt.Columns.Add("Cidade", typeof(string));
                    dt.Columns.Add("Descricao", typeof(string));
                    dt.Columns.Add("URL", typeof(string));
                    while (dr.Read())
                    {
                        dt.Rows.Add(new Object[] {dr["Nome"].ToString(),
                        Custom.Helper.CalcularIdade(Convert.ToDateTime(dr["Dta_Nascimento"])),
                        dr["Funcao"].ToString(),
                        dr["Cidade"].ToString(),
                        dr["Descricao"].ToString(),
                        dr["URL"].ToString()
                        });
                    }
                }
                Total = Convert.ToInt32(parms[2].Value);
            }
            finally
            {
                if (dt != null)
                    dt.Dispose();
            }

            return dt;
        }
        #endregion

        #endregion

    }
}