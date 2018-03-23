//-- Data: 17/05/2016 10:28
//-- Autor: Mailson

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.Caching;
using System.Linq;

namespace BNE.BLL
{
	public partial class EmailEnvioCurriculo // Tabela: BNE_Email_Envio_Curriculo
	{
        #region Consuta

        #region spRetornarTodosPorUsuarioFilial
        private const string spRetornarTodosPorUsuarioFilial = @"
            SELECT 
                *
            FROM
                BNE.bne_Email_Envio_Curriculo with(nolock)
	        WHERE idf_usuario_filial_perfil = @Idf_Usuario_Filial_Perfil
            ORDER BY Dta_Utilizacao desc
        ";
        #endregion

        #region spUpdateUltimoEnviado
        private const string spUpdateUltimoEnviado = @"
                     update BNE.bne_Email_Envio_Curriculo set dta_utilizacao = getdate()
	                 where idf_usuario_filial_perfil = @Idf_Usuario_Filial_Perfil
	                 and eml_curriculo = @Email";

        #endregion
        
        #endregion

        #region Metodos

        #region RetornarTodosPorUsuarioFilial
        private static List<EmailEnvioCurriculo> RetornarTodosPorUsuarioFilial(int idUsuarioFilialPerfil)
        {
            try
            {
                string cachekey = String.Format("bne_lista_email_envio_curriculo:{0}", idUsuarioFilialPerfil);
                if (MemoryCache.Default[cachekey] != null)
                    return (List<EmailEnvioCurriculo>)MemoryCache.Default[cachekey];

                var listEmail = new List<EmailEnvioCurriculo>();
                var parms = new List<SqlParameter>
                {
					new SqlParameter { ParameterName = "@Idf_Usuario_Filial_Perfil", SqlDbType = SqlDbType.Int, Value = idUsuarioFilialPerfil },
                };

                using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, spRetornarTodosPorUsuarioFilial, parms))
                {
                    while (dr.Read())
                        listEmail.Add(new EmailEnvioCurriculo()
                        {
                            _idEmailEnvioCurriculo = Convert.ToInt32(dr["idf_email_envio_curriculo"]),
                            _emailCurriculo = dr["eml_curriculo"].ToString()
                        });

                    if (!dr.IsClosed)
                        dr.Close();
                }

                CacheItemPolicy policy = new CacheItemPolicy();
                policy.AbsoluteExpiration = DateTime.Now.AddMinutes(Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.MinutosLimpezaCache)));
                MemoryCache.Default.Add(cachekey, listEmail, policy);

                return listEmail;
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
                return new List<EmailEnvioCurriculo>();
            }
        }
        #endregion

        #region ListarSugestEmailUsuarioFilial
        /// <summary>
        /// LIsta de email para sugest do usuarioFilialPerfil
        /// </summary>
        /// <param name="parcial"></param>
        /// <param name="numeroRegistros"></param>
        /// <param name="usuarioFilial"></param>
        /// <returns></returns>
        public static Dictionary<int, string> ListarSugestEmailUsuarioFilial(string parcial, int numeroRegistros, int usuarioFilial)
        {
            try
            {
                var listEmail = new Dictionary<int, string>();
                listEmail = RetornarTodosPorUsuarioFilial(usuarioFilial).Where(x => x.EmailCurriculo.Contains(parcial)).Take(numeroRegistros).ToDictionary(i => i.IdEmailEnvioCurriculo, i => i.EmailCurriculo);
                return listEmail;
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
                throw;
            }
        }
        #endregion

        #region ExisteNaLista
        public static void ExisteNaLista(string email, int idUsuarioFilialPerfil)
        {
            var listEmail = RetornarTodosPorUsuarioFilial(idUsuarioFilialPerfil);
            if (listEmail.Where(x => x.EmailCurriculo.Equals(email)).Count() > 0) //Email já adicionado
                return;

            //Caso não exista, cria no banco e no cache
            try
            {

                //Insere no banco
                EmailEnvioCurriculo objEmail = new EmailEnvioCurriculo();
                objEmail.EmailCurriculo = email;
                objEmail.UsuarioFilialPerfil = new UsuarioFilialPerfil(idUsuarioFilialPerfil);
                objEmail.DataUtilizacao = DateTime.Now;
                objEmail.Save();

                //Insere no cache
                listEmail.Add(objEmail);
                AtualizarCache(listEmail, idUsuarioFilialPerfil);

            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex, "Erro ao gravar EmailEnvioCurriculo");
            }
        }
        #endregion

        #region Delete
        public static void Delete(int idEmailEnvioCurriculo, int idUsuarioFilialPerfil)
        {
            try
            {
                List<SqlParameter> parms = new List<SqlParameter>();
                parms.Add(new SqlParameter("@Idf_Email_Envio_Curriculo", SqlDbType.Int, 4));
                parms.Add(new SqlParameter("@Idf_Usuario_Filial_Perfil", SqlDbType.Int, 4));

                parms[0].Value = idEmailEnvioCurriculo;
                parms[1].Value = idUsuarioFilialPerfil;

                DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);

                //Remover item do cache
                var listEmail = RetornarTodosPorUsuarioFilial(idUsuarioFilialPerfil);
                listEmail.Remove(listEmail.Where(x => x.IdEmailEnvioCurriculo.Equals(idEmailEnvioCurriculo)).FirstOrDefault());
                AtualizarCache(listEmail, idUsuarioFilialPerfil);

            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
                throw;
            }
            
        }
        #endregion

        #region UltimoEmailUtilizadoParaEnvio
        /// <summary>
        /// Carregar o ultimo email usado para destinatario do envio de curriculo
        /// </summary>
        /// <param name="idUsuarioFilialPerfill"></param>
        /// <returns></returns>
        public static string UltimoEmailUtilizadoParaEnvio(int idUsuarioFilialPerfill)
        {
            try
            {
                var listEmail = RetornarTodosPorUsuarioFilial(idUsuarioFilialPerfill);

                if (listEmail.Count == 0)
                    return string.Empty;

                return listEmail.OrderByDescending(x => x.DataUtilizacao).FirstOrDefault().EmailCurriculo;
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
                return string.Empty;
            }
        }
        #endregion

        #region UpdateUltimoEnviado
        public static void UpdateUltimoEnviado(int idUsuarioFilialperfil, string email)
        {
            var parms = new List<SqlParameter>
                {
					new SqlParameter { ParameterName = "@Idf_Usuario_Filial_Perfil", SqlDbType = SqlDbType.Int, Size = 4, Value = idUsuarioFilialperfil },
                    new SqlParameter { ParameterName = "@Email", SqlDbType = SqlDbType.VarChar,Value = email},
                };
            DataAccessLayer.ExecuteNonQuery(CommandType.Text, spUpdateUltimoEnviado, parms);

            //Atualizar Cache
            var listEmail = RetornarTodosPorUsuarioFilial(idUsuarioFilialperfil);
            listEmail.Where(x => x.EmailCurriculo.Equals(email)).FirstOrDefault().DataUtilizacao = DateTime.Now;
            AtualizarCache(listEmail, idUsuarioFilialperfil);
        }
        #endregion

        #region AtualizarCache
        protected static void AtualizarCache(List<EmailEnvioCurriculo> valorCache, int idUsuarioFilialperfil)
        {
            string cachekey = String.Format("bne_lista_email_envio_curriculo:{0}", idUsuarioFilialperfil);
            CacheItemPolicy policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = DateTime.Now.AddMinutes(Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.MinutosLimpezaCache)));
            MemoryCache.Default.Add(cachekey, valorCache, policy);
        }
        #endregion

        #endregion
    }
}