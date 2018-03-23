//-- Data: 13/07/2015 17:00
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BNE.BLL
{
	public partial class CampanhaRecrutamentoCurriculo // Tabela: BNE_Campanha_Recrutamento_Curriculo
    {

        #region Consultas

        #region Spquantidadecurriculosenviados
        private const string Spquantidadecurriculosenviados = @"
        SELECT  COUNT(DISTINCT(Idf_Curriculo))
        FROM    BNE_Campanha_Recrutamento_Curriculo
        WHERE   Idf_Campanha_Recrutamento = @Idf_Campanha_Recrutamento
        ";
        #endregion

        #region Spquantidadecurriculosenviadostipoenvio
        private const string Spquantidadecurriculosenviadostipoenvio = @"
        SELECT  COUNT(DISTINCT(Idf_Curriculo))
        FROM    BNE_Campanha_Recrutamento_Curriculo
        WHERE   Idf_Campanha_Recrutamento = @Idf_Campanha_Recrutamento
                AND Idf_Tipo_Mensagem_CS = @Idf_Tipo_Mensagem_CS
        ";
        #endregion
        

        #region Spselectcurriculosenviados
        private const string Spselectcurriculosenviados = @"
        SELECT  Idf_Curriculo
        FROM    BNE_Campanha_Recrutamento_Curriculo
        WHERE   Idf_Campanha_Recrutamento = @Idf_Campanha_Recrutamento
        GROUP BY Idf_Curriculo 
        ";
        #endregion

        #region SpSelectCurriculosEnviadosPesquisaCVByFuncao
        private const string SpSelectCurriculosEnviadosPesquisaCVByFuncao = @"
        SELECT crc.Idf_Curriculo 
        FROM   BNE.BNE_Campanha_Recrutamento_Curriculo crc
            JOIN BNE.BNE_Campanha_Recrutamento cr ON crc.Idf_Campanha_Recrutamento = cr.Idf_Campanha_Recrutamento
            JOIN BNE.TAB_Pesquisa_Curriculo pc ON cr.Idf_Pesquisa_Curriculo = pc.Idf_Pesquisa_Curriculo
        WHERE  crc.Dta_Cadastro > @Dta_Cadastro_Limite
        GROUP  BY crc.Idf_Curriculo
        ";
        #endregion

        #region spVerificaCurriculoCandidatouCampanha

        private const string spVerificaCurriculoCandidatouCampanha = @"select distinct idf_curriculo from BNE_Campanha_Recrutamento_Curriculo crc with(nolocK)
                                join bne.BNE_Campanha_Recrutamento cr with(nolock) on cr.Idf_Campanha_Recrutamento = crc.Idf_Campanha_Recrutamento
                                where cr.Idf_Vaga = @Idf_Vaga and Idf_Curriculo = @Idf_Curriculo";
        #endregion

        #endregion

        #region Métodos

        #region RecuperarQuantidadeCurriculosEnviados
        /// <summary>
        /// Recuperar a quantidade de currículos já enviados
        /// </summary>
        /// <param name="objCampanhaRecrutamento"></param>
        /// <returns></returns>
        public static int RecuperarQuantidadeCurriculosEnviados(CampanhaRecrutamento objCampanhaRecrutamento)
	    {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Campanha_Recrutamento", SqlDbType = SqlDbType.Int, Size = 4, Value = objCampanhaRecrutamento.IdCampanhaRecrutamento }
                };

            return Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, Spquantidadecurriculosenviados, parms));
	    }
        #endregion

        #region RecuperarQuantidadeCurriculosEnviadosSMS
        /// <summary>
        /// Recuperar a quantidade de currículos já enviados
        /// </summary>
        /// <param name="objCampanhaRecrutamento"></param>
        /// <returns></returns>
        public static int RecuperarQuantidadeCurriculosEnviadosSMS(CampanhaRecrutamento objCampanhaRecrutamento)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Campanha_Recrutamento", SqlDbType = SqlDbType.Int, Size = 4, Value = objCampanhaRecrutamento.IdCampanhaRecrutamento },
                    new SqlParameter { ParameterName = "@Idf_Tipo_Mensagem_CS", SqlDbType = SqlDbType.Int, Size = 4, Value = (int)Enumeradores.TipoMensagem.SMS }
                    
                };

            return Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, Spquantidadecurriculosenviadostipoenvio, parms));
        }
        #endregion

        #region RecuperarQuantidadeCurriculosEnviadosEmail
        /// <summary>
        /// Recuperar a quantidade de currículos já enviados
        /// </summary>
        /// <param name="objCampanhaRecrutamento"></param>
        /// <returns></returns>
        public static int RecuperarQuantidadeCurriculosEnviadosEmail(CampanhaRecrutamento objCampanhaRecrutamento)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Campanha_Recrutamento", SqlDbType = SqlDbType.Int, Size = 4, Value = objCampanhaRecrutamento.IdCampanhaRecrutamento },
                    new SqlParameter { ParameterName = "@Idf_Tipo_Mensagem_CS", SqlDbType = SqlDbType.Int, Size = 4, Value = (int)Enumeradores.TipoMensagem.Email }
                };

            return Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, Spquantidadecurriculosenviadostipoenvio, parms));
        }
        #endregion

        #region ListaIdsCurriculosEnviados
        /// <summary>
        /// Retorna uma lista com os ids dos currículos já enviados
        /// </summary>
        /// <param name="objCampanhaRecrutamento"></param>
        /// <returns></returns>
        public static List<int> ListaIdsCurriculosEnviados(CampanhaRecrutamento objCampanhaRecrutamento)
        {
            var lista = new List<int>();

            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Campanha_Recrutamento", SqlDbType = SqlDbType.Int, Size = 4, Value = objCampanhaRecrutamento.IdCampanhaRecrutamento }
                };

            using (var dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spselectcurriculosenviados, parms))
            {
                while(dr.Read())
                    lista.Add(Convert.ToInt32(dr["Idf_Curriculo"]));
            }

            return lista;
        }
        #endregion

        #region ListaIdsCurriculosEnviadosCampanhaPesquisaCV
        /// <summary>
        /// Retorna uma lista com os ids dos currículos já enviados na campanha de envio de CVs
        /// </summary>
        /// <param name="objCampanhaRecrutamento"></param>
        /// <returns></returns>
        public static List<int> ListaIdsCurriculosEnviadosCampanhaPesquisaCV(CampanhaRecrutamento objCampanhaRecrutamento)
        {
            var lista = new List<int>();

            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Dta_Cadastro_Limite", SqlDbType = SqlDbType.DateTime, Value = DateTime.Now.ToShortDateString() }
                };

            using (var dr = DataAccessLayer.ExecuteReader(CommandType.Text, SpSelectCurriculosEnviadosPesquisaCVByFuncao, parms))
            {
                while (dr.Read())
                    lista.Add(Convert.ToInt32(dr["Idf_Curriculo"]));
            }

            return lista;
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
            DataAccessLayer.SaveBulkTable(dt, "BNE_Campanha_Recrutamento_Curriculo", trans);
        }
        #endregion

        #region VerificaCurriculoCandidatouCampanha
          /// <summary>
          /// Verifica se o curriculo candidato a vaga recebeu a campanha
          /// </summary>
          /// <param name="IdCurriculo"></param>
          /// <returns></returns>
        public static bool VerificaCurriculoCandidatouCampanha(int IdVaga, int IdCurriculo, SqlTransaction trans)
        {
            var retorno = false;
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Curriculo", SqlDbType = SqlDbType.Int, Size = 4, Value = IdCurriculo },
                    new SqlParameter { ParameterName = "@Idf_Vaga", SqlDbType = SqlDbType.Int, Size = 4, Value = IdVaga }
                };

            using (var dr = DataAccessLayer.ExecuteReader(trans, CommandType.Text, spVerificaCurriculoCandidatouCampanha, parms))
            {
                if (dr.Read())
                    retorno= true;
            }

            return retorno;
        }
        #endregion
        #endregion

    }
}