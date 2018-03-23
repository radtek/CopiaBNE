//-- Data: 18/02/2016 17:07
//-- Autor: Mailson

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.Caching;
using System.Text;
namespace BNE.BLL
{
	public partial class ParametroVaga // Tabela: BNE_Parametro_Vaga
	{

        #region Consultas
        #region spPremium
        private const string spPremium = @"
        SELECT v.idf_vaga FROM BNE_Parametro_Vaga pv WITH(NOLOCK)
	        join bne_vaga v on v.idf_vaga = pv.idf_vaga
	         WHERE pv.Idf_Parametro = @Idf_Parametro AND v.Idf_Vaga = @Idf_Vaga
	         and v.Flg_vaga_Arquivada = 0 -- vaga ativa";

        #endregion

        #region spDeleteParametro
        private const string spDeleteParametro = @"SET NOCOUNT ON
                            SELECT pv.Idf_Vaga, Idf_Parametro
                            INTO #temp
                            FROM  BNE.BNE_Parametro_Vaga pv WITH (NOLOCK)
                                JOIN BNE_Vaga v ON v.Idf_Vaga = pv.Idf_Vaga
                            WHERE  v.Idf_Filial = @Idf_Filial
                                AND pv.Idf_Parametro = @Idf_Parametro

                            DELETE  BNE_Parametro_Vaga
                            FROM  BNE.BNE_Parametro_Vaga pv 
                                JOIN #temp t ON pv.Idf_Parametro = t.Idf_Parametro AND pv.Idf_Vaga = t.Idf_Vaga
                            WHERE  pv.Idf_Parametro = @Idf_Parametro

                            SELECT Idf_Vaga FROM #temp

                            DROP TABLE #temp";
        #endregion

        #region spFilialTransacao
        private const string spFilialTransacao = @"select top 40 pa.Idf_Filial from  BNE_Linha_Arquivo la
            join BNE_Transacao t on t.Idf_Transacao = la.Idf_Transacao
            join BNE_Plano_Adquirido pa on pa.Idf_Plano_Adquirido = t.Idf_Plano_Adquirido
             where pa.Idf_Filial is not null and t.idf_transacao = @Idf_Transacao ";
	    #endregion

        #endregion

        #region Métodos

        #region Premium
        /// <summary>
        /// Retorna se a vaga é premium
        /// </summary>
        /// <param name="idParametro"></param>
        /// <param name="idVaga"></param>
        /// <returns></returns>
        public static bool Premium(int idParametro, int idVaga)
        {
            return false;

            //RETIRADA VAGA PREMIUM TASK: 41857

            //string cacheKey = String.Format("VagaPremium:v:{0}", idVaga);
            //if (MemoryCache.Default[cacheKey] != null)
            //{
            //    return (bool)MemoryCache.Default.Get(cacheKey);
            //}
            //bool valor = false;
            //var parms = new List<SqlParameter>
            //    {
            //        new SqlParameter { ParameterName = "@idf_Parametro", SqlDbType = SqlDbType.Int, Size = 4, Value = idParametro},
            //        new SqlParameter { ParameterName = "@idf_Vaga", SqlDbType= SqlDbType.Int, Size = 4, Value = idVaga}
            //    };
            //using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, spPremium, parms))
            //{
             
            //    if (dr.Read())
            //        valor= true;
            //}

            //   CacheItemPolicy policy = new CacheItemPolicy();
            //   policy.AbsoluteExpiration = DateTime.Now.AddMinutes(Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.MinutosLimpezaCache)));
            //   MemoryCache.Default.Add(cacheKey,valor,policy);
            //return valor;

        }
        #endregion

        #region [Apagar parametro premium das vagas]
        /// <summary>
        /// Apagar todos os parametro de vaga premium de uma empresa
        /// </summary>
        /// <param name="idFilial"></param>
        /// 
        public static void ApagarParametroVagaPremium(int idFilial)
        {
            try
            {
               var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Parametro", SqlDbType = SqlDbType.Int, Size = 4, Value = (int)Enumeradores.Parametro.VagaPremium},
                    new SqlParameter { ParameterName = "@Idf_Filial", SqlDbType= SqlDbType.Int, Size = 4, Value = idFilial}
                };
                 using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, spDeleteParametro, parms)){
                     while(dr.Read()){
                         Vaga objVaga = new Vaga(Convert.ToInt32(dr["Idf_Vaga"]));
                          BNE.BLL.Custom.BufferAtualizacaoVaga.UpdateVaga(objVaga);
                     }
                 }
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex, "Erro - Apagara parametro vaga premium, metodo ApagarParametroVagaPremium");
            }
           
        }

        public static void ApagarParametroVagaPremium(SqlTransaction trans, int idFilial)
        {
            try
            {
                //Pegar as vaga para atualizar no solr

                var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Parametro", SqlDbType = SqlDbType.Int, Size = 4, Value = (int)Enumeradores.Parametro.VagaPremium},
                    new SqlParameter { ParameterName = "@Idf_Filial", SqlDbType= SqlDbType.Int, Size = 4, Value = idFilial}
                };
                using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, spDeleteParametro, parms))
                {
                    while (dr.Read())
                    {
                        Vaga objVaga = new Vaga(Convert.ToInt32(dr["Idf_Vaga"]));
                        BNE.BLL.Custom.BufferAtualizacaoVaga.UpdateVaga(objVaga);
                    }
                }
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex, "Erro - Apagara parametro vaga premium, metodo ApagarParametroVagaPremium");
            }

        }
        #endregion

        #region ApagarParametroVaga
        /// <summary>
        /// Apagar lista de parametro vaga
        /// </summary>
        /// <param name="ListIdVaga"></param>
        /// <param name="idParametro"></param>
        public static void ApagarParametroVaga(Dictionary<int, DateTime> ListIdVaga, int idFilial, Enumeradores.Parametro idParametro)
        {
            try
            {
                var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Parametro", SqlDbType = SqlDbType.Int, Size = 4, Value = (int)idParametro},
                    new SqlParameter { ParameterName = "@Idf_Filial", SqlDbType= SqlDbType.Int,Size= 4, Value = idFilial}
                };
                //DELETAR OS PARAMETROS
                DataAccessLayer.ExecuteNonQuery(CommandType.Text, spDeleteParametro, parms);
                foreach (var vaga in ListIdVaga)
                {
                    if (vaga.Value > DateTime.Now.AddMonths(Convert.ToInt32(BLL.Parametro.RecuperaValorParametro(Enumeradores.Parametro.MesesParaAtivarVagaEmpresaDesbloqueada))))
                        BNE.BLL.Custom.BufferAtualizacaoVaga.UpdateVaga(new Vaga(vaga.Key));
                    else
                        Vaga.InativarVaga(vaga.Key, null, Enumeradores.VagaLog.PluginRemoverVagaEmpresaBloqueada);
                }
                        
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex, "Erro - Apagara parametro vaga");
            }

        }
        #endregion
       
        #endregion
	}
}