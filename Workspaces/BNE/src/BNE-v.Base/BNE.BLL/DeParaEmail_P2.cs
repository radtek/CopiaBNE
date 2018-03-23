//-- Data: 27/06/2014 11:37
//-- Autor: Gieyson Stelmak

using BNE.Cache;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace BNE.BLL
{
    public partial class DeParaEmail // Tabela: plataforma.TAB_DePara_Email
    {

        #region Configuração de cache

        static readonly ICachingService Cache = CachingServiceProvider.Instance;

        #region Parametros
        private const string ObjectKey = "plataforma.TAB_DePara_Email";
        private const double SlidingExpiration = 1440;
        private static readonly bool HabilitaCache = ConfigurationManager.AppSettings["HabilitaCache"] != null && Convert.ToBoolean(ConfigurationManager.AppSettings["HabilitaCache"]);
        #endregion

        #region Cached Objects

        #region DeParaEmail
        private static List<DeParaEmail> DeParaEmailCache
        {
            get
            {
                return Cache.GetItem(ObjectKey, ListarDeParaEmail, SlidingExpiration);
            }
        }
        #endregion

        #endregion

        #region Métodos

        #region ListarDeParaEmail
        /// <summary>
        /// Método que retorna uma lista de itens do dicionário de parâmetros do sistema.
        /// </summary>
        /// <returns>Lista com valores recuperados do banco.</returns>
        private static List<DeParaEmail> ListarDeParaEmail()
        {
            var lista = new List<DeParaEmail>();

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, spselectall, null))
            {
                while (dr.Read())
                {
                    var objDePara = new DeParaEmail();
                    if (SetInstance_NonDispose(dr, objDePara))
                        lista.Add(objDePara);
                }
            }

            return lista;
        }
        #endregion

        #region SetInstance
        /// <summary>
        /// Método auxiliar utilizado pelos métodos LoadObject e CompleteObject para percorrer um IDataReader e vincular as colunas com os atributos da classe.
        /// </summary>
        /// <param name="dr">Cursor de leitura do banco de dados.</param>
        /// <param name="objDeParaEmail">Instância a ser manipulada.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static bool SetInstance_NonDispose(IDataReader dr, DeParaEmail objDeParaEmail)
        {
            objDeParaEmail._idDeParaEmail = Convert.ToInt32(dr["Idf_DePara_Email"]);
            objDeParaEmail._descricaoEmailErro = Convert.ToString(dr["Des_Email_Erro"]);
            objDeParaEmail._descricaoEmailCorreto = Convert.ToString(dr["Des_Email_Correto"]);
            objDeParaEmail._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);

            objDeParaEmail._persisted = true;
            objDeParaEmail._modified = false;

            return true;
        }
        #endregion

        #endregion

        #endregion

        #region Consultas

        #region spselectall
        private const string spselectbydomainwithat = "SELECT COUNT(*) FROM plataforma.TAB_DePara_Email WHERE Des_Email_Correto LIKE @DomainWithAt";
        private const string spselectall = "SELECT [Idf_DePara_Email], [Des_Email_Erro], [Des_Email_Correto], [Dta_Cadastro] FROM plataforma.TAB_DePara_Email";
        #endregion

        #endregion

        #region Métodos

        #region RecuperarAutocomplete
        /// <summary>
        /// Método que retorna uma lista de email
        /// </summary>
        /// <param name="nomeParcial">Parte do nome da funcao.</param>
        /// <param name="numeroRegistros">Quantidade de registros a serem retornados.</param>
        /// <returns>Lista de nomes de funcoes.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static string[] RecuperarAutocomplete(string nomeParcial, int numeroRegistros)
        {
            #region Cache
            if (HabilitaCache)
            {
                if (nomeParcial.Contains("@"))
                {
                    string sufixo = nomeParcial.Substring(nomeParcial.IndexOf('@'));

                    nomeParcial = nomeParcial.Replace(sufixo, string.Empty);

                    return DeParaEmailCache.Where(dp => dp.DescricaoEmailCorreto.StartsWith(sufixo)).Select(dp => nomeParcial + dp.DescricaoEmailCorreto).Distinct().Take(numeroRegistros).ToArray();
                }
            }
            #endregion

            return null;
        }
        #endregion

        #region ExistsDomainWithAt
        public static bool ExistsDomainWithAt(string domain)
        {
            #region Cache
            if (HabilitaCache)
            {
                if (domain.Contains("@"))
                    return DeParaEmailCache.Any(dp => dp.DescricaoEmailCorreto == domain);
                return false;
            }
            #endregion

            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@DomainWithAt", SqlDbType = SqlDbType.VarChar, Size = 100, Value = domain }
                };

            return Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, spselectbydomainwithat, parms)) > 0;
        }
        #endregion

        #endregion

    }
}