//-- Data: 23/07/2013 15:25
//-- Autor: Luan Fernandes

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL.Notificacao
{
    public partial class AlertaCurriculos // Tabela: alerta.Tab_Alerta_Curriculos
    {
        #region Consultas

        #region SPDELETEALERTAS
        private const string SPDELETEALERTAS = @"SP_DELETE_ALERTAS";
        #endregion

        #region SPBUSCACVSALERTA
        private const string SPBUSCACVSALERTA = @" SELECT AC.Idf_Curriculo, AC.Nme_Pessoa, Ac.Eml_Pessoa, Ac.Flg_VIP 
               FROM alerta.Tab_Alerta_Curriculos AC WITH(NOLOCK)
         INNER JOIN alerta.Tab_Alerta_Cidades ACD WITH(NOLOCK) ON ACD.Idf_Curriculo = AC.Idf_Curriculo
         INNER JOIN alerta.Tab_Alerta_Funcoes AF WITH(NOLOCK)  ON AF.Idf_Curriculo = AC.Idf_Curriculo 
              WHERE ACD.Idf_Cidade = @Idf_Cidade
                AND AF.Idf_Funcao  = @Idf_Funcao
		   GROUP BY AC.Idf_Curriculo, AC.Nme_Pessoa, Ac.Eml_Pessoa, Ac.Flg_VIP";

        private const string SPBUSCATODOSCVSALERTA = @" SELECT AC.Idf_Curriculo, AC.Nme_Pessoa, Ac.Eml_Pessoa, Ac.Flg_VIP 
               FROM alerta.Tab_Alerta_Curriculos AC WITH(NOLOCK)
		   GROUP BY AC.Idf_Curriculo, AC.Nme_Pessoa, Ac.Eml_Pessoa, Ac.Flg_VIP";
        #endregion

        #region SPAJUSTARALERTACVS
        /// <summary>
        ///     Ajusta os alertas para os CVS que foram atualizados ou excluidos...
        /// </summary>
        private const string SPAJUSTARALERTACVS = @"SP_AJUSTAR_ALERTACVS";
        #endregion

        #endregion

        #region Métodos

        #region Delete
        /// <summary>
        ///     Método utilizado para excluir uma instância de AlertaCurriculos no banco de dados.
        /// </summary>
        /// <param name="idCurriculo">Chave do registro.</param>
        /// <remarks>Luan Fernandes</remarks>
        public static void Delete(int idCurriculo)
        {
            var parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4));

            parms[0].Value = idCurriculo;

            using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();
                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        DataAccessLayer.ExecuteNonQuery(trans, CommandType.StoredProcedure, SPDELETEALERTAS, parms);
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

        #region ListarCv'sAlertas
        public static List<AlertaCurriculos> ListarCvsAlertas(int IdCidade, int IdFuncao)
        {
            var parms = new List<SqlParameter>();

            parms.Add(new SqlParameter("@Idf_Cidade", SqlDbType.Int, 8));
            parms.Add(new SqlParameter("@Idf_Funcao", SqlDbType.Int, 8));

            parms[0].Value = IdCidade;
            parms[1].Value = IdFuncao;

            var lista = new List<AlertaCurriculos>();

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SPBUSCACVSALERTA, parms, DataAccessLayer.CONN_STRING))
            {
                while (dr.Read())
                {
                    lista.Add(new AlertaCurriculos
                    {
                        IdCurriculo = Convert.ToInt32(dr["Idf_Curriculo"]),
                        NomePessoa = dr["Nme_Pessoa"].ToString(),
                        EmailPessoa = dr["Eml_Pessoa"].ToString(),
                        FlagVIP = Convert.ToBoolean(dr["Flg_VIP"])
                    });
                }
            }
            return lista;
        }

        public static List<AlertaCurriculos> ListarCvsAlertas(SqlTransaction trans)
        {
            var lista = new List<AlertaCurriculos>();

            using (IDataReader dr = DataAccessLayer.ExecuteReader(trans, CommandType.Text, SPBUSCATODOSCVSALERTA, null))
            {
                while (dr.Read())
                {
                    lista.Add(new AlertaCurriculos
                    {
                        IdCurriculo = Convert.ToInt32(dr["Idf_Curriculo"]),
                        NomePessoa = dr["Nme_Pessoa"].ToString(),
                        EmailPessoa = dr["Eml_Pessoa"].ToString(),
                        FlagVIP = Convert.ToBoolean(dr["Flg_VIP"])
                    });
                }
            }
            return lista;
        }
        #endregion

        #region AjustarAlertasCVs
        /// <summary>
        ///     Ajusta os alertas para novos Cvs, Vips adquiridos ou CVs bloqueados, invativados
        /// </summary>
        public static void AjustarAlertasCVs()
        {
            using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();

                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        DataAccessLayer.ExecuteNonQuery(trans, CommandType.StoredProcedure, SPAJUSTARALERTACVS, null);
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        GerenciadorException.GravarExcecao(ex);
                        trans.Rollback();
                    }
                    trans.Dispose();
                }
                conn.Close();
                conn.Dispose();
            }
        }
        #endregion

        #endregion

        public static void Atualizar(List<int> lista)
        {
            var parms = new List<SqlParameter>
            {
                new SqlParameter {ParameterName = "@idfs_curriculos", SqlDbType = SqlDbType.VarChar, Size = -1, Value = string.Join(",", lista)},
                new SqlParameter {ParameterName = "@Data", SqlDbType = SqlDbType.DateTime, Value = DateTime.Now}
            };
            DataAccessLayer.ExecuteNonQuery(CommandType.StoredProcedure, "JornalVaga_AtualizarAlertaCurriculo", parms);
        }

        public static int QuantidadeEnviadaHoje()
        {
            return Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.StoredProcedure, "JornalVaga_QuantidadeAlertaCurriculoEnviadoHoje", null));
        }
    }
}