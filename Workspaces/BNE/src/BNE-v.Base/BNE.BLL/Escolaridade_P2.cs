//-- Data: 02/03/2010 09:20
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using BNE.BLL.Custom;
using BNE.Cache;
using BNE.EL;

namespace BNE.BLL
{
    public partial class Escolaridade // Tabela: TAB_Escolaridade
    {

        #region Configuração de cache

        static readonly ICachingService Cache = CachingServiceProvider.Instance;

        #region Parametros
        private const string ObjectKey = "plataforma.TAB_Escolaridade";
        private const double SlidingExpiration = 1440;
        private static readonly bool HabilitaCache = ConfigurationManager.AppSettings["HabilitaCache"] != null && Convert.ToBoolean(ConfigurationManager.AppSettings["HabilitaCache"]);
        #endregion

        #region Cached Objects

        #region Escolaridades
        private static List<EscolaridadeCache> Escolaridades
        {
            get
            {
                return Cache.GetItem(ObjectKey, ListarEscolaridadesCACHE, SlidingExpiration);
            }
        }
        #endregion

        #endregion

        #region Métodos

        #region ListarEscolaridadesCACHE
        /// <summary>
        /// Método que retorna uma lista de itens.
        /// </summary>
        /// <returns>Dicionário com valores recuperados do banco.</returns>
        private static List<EscolaridadeCache> ListarEscolaridadesCACHE()
        {
            var listaEscolaridades = new List<EscolaridadeCache>();

            const string selectescolaridades = @"
            SELECT  E.Idf_Escolaridade,
                    E.Des_Bne,
                    E.Flg_BNE,
                    E.Seq_BNE,
                    E.Seq_Peso
            FROM    plataforma.TAB_Escolaridade E WITH(NOLOCK) 
            WHERE   E.Flg_Inativo = 0
            ";

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, selectescolaridades, null))
            {
                while (dr.Read())
                {
                    listaEscolaridades.Add(new EscolaridadeCache
                    {
                        Identificador = Convert.ToInt32(dr["Idf_Escolaridade"]),
                        DescricaoBNE = dr["Des_Bne"].ToString(),
                        FlagBNE = Convert.ToBoolean(dr["Flg_BNE"]),
                        SequenciaBNE = dr["Seq_BNE"] != DBNull.Value ? Convert.ToInt32(dr["Seq_BNE"]) : (int?)null,
                        SequenciaPeso = dr["Seq_Peso"] != DBNull.Value ? Convert.ToInt16(dr["Seq_Peso"]) : (short?)null
                    });
                }
                if (!dr.IsClosed)
                    dr.Close();
            }

            return listaEscolaridades;
        }
        #endregion

        #endregion

        #region CacheObjects

        #region EscolaridadeCache
        private class EscolaridadeCache
        {
            public int Identificador { get; set; }
            public string DescricaoBNE { get; set; }
            public bool FlagBNE { get; set; }
            public int? SequenciaBNE { get; set; }
            public short? SequenciaPeso { get; set; }
        }
        #endregion

        #endregion

        #endregion

        #region Consultas
        private const string Selectnivel = @"   
        SELECT  Idf_Escolaridade,
                Des_Bne
        FROM    plataforma.Tab_Escolaridade
        WHERE   Flg_BNE = 1 AND
                Flg_Inativo = 0 AND
	            Idf_Escolaridade != 18 --Diferente de outros cursos
        ORDER BY Seq_BNE ASC";

        private const string Selectnivelpornome = @"   
        SELECT  *
        FROM    plataforma.Tab_Escolaridade
        WHERE   Flg_BNE = 1 AND
                Flg_Inativo = 0 AND
	            Idf_Escolaridade != 18 AND --Diferente de outros cursos 
                des_bne = @des_bne
        ORDER BY Seq_BNE ASC";

        private const string SPSELECTNIVELESCOLARIDADEPESSOAFISICA = @"
        DECLARE @sql VARCHAR(MAX)

        SET @sql = '
        IF ( SELECT COUNT(E.Idf_Escolaridade)
                FROM   plataforma.TAB_Escolaridade E
                    INNER JOIN BNE_Formacao F ON E.Idf_Escolaridade = F.Idf_Escolaridade
                WHERE  E.Flg_BNE = 1
                    AND E.Flg_Inativo = 0
                    AND E.Idf_Escolaridade != 18 /* Diferente de outros cursos */
                    '
        IF (@Flg_Graduacao = 1)
	        BEGIN
		        SET @sql = @sql + ' AND (F.Idf_Escolaridade >= 4 AND F.Idf_Escolaridade <= 13) '
	        END         
        ELSE
	        BEGIN
		        SET @sql = @sql + ' AND (F.Idf_Escolaridade >= 14 AND F.Idf_Escolaridade <= 17) '
	        END   
                                	
        SET @sql = @sql + '            
                    AND F.Flg_Inativo = 0
                    AND F.Idf_Pessoa_Fisica = ' + CONVERT(VARCHAR, @Idf_Pessoa_Fisica) + '
            ) > 0 --Se tiver escolaridade informada.
            SELECT  Idf_Escolaridade ,
                    Des_BNE
            FROM    plataforma.Tab_Escolaridade
            WHERE   Flg_BNE = 1
                    AND Flg_Inativo = 0
                    AND Idf_Escolaridade != 18 /* Diferente de outros cursos */ 
                        '
        IF (@Flg_Graduacao = 1)
	        BEGIN
		        SET @sql = @sql + ' AND (Idf_Escolaridade >= 4 AND Idf_Escolaridade <= 13) '
	        END         
        ELSE
	        BEGIN
		        SET @sql = @sql + ' AND (Idf_Escolaridade >= 14 AND Idf_Escolaridade <= 17) '
	        END   
                                	
        SET @sql = @sql + ' 
                    AND Idf_Escolaridade > ( -- Retirando ensino médio e fundamental
                                                SELECT CASE MAX(E.Idf_Escolaridade)
                                                        WHEN 4 THEN 5
                                                        WHEN 5 THEN 5
                                                        WHEN 6 THEN 7
                                                        WHEN 7 THEN 7
                                                        ELSE 3
                                                    END AS Idf_Escolaridade
                                                FROM   plataforma.TAB_Escolaridade E
                                                    INNER JOIN BNE_Formacao F ON E.Idf_Escolaridade = F.Idf_Escolaridade
                                                WHERE  E.Flg_BNE = 1
                                                    AND E.Flg_Inativo = 0
                                                    AND E.Idf_Escolaridade != 18 /* Diferente de outros cursos */
                                                        '
        IF (@Flg_Graduacao = 1)
	        BEGIN
		        SET @sql = @sql + ' AND (F.Idf_Escolaridade >= 4 AND F.Idf_Escolaridade <= 13) '
	        END         
        ELSE
	        BEGIN
		        SET @sql = @sql + ' AND (F.Idf_Escolaridade >= 14 AND F.Idf_Escolaridade <= 17) '
	        END   
                                	
        SET @sql = @sql + ' 
                    AND F.Flg_Inativo = 0
                    AND F.Idf_Pessoa_Fisica = ' + CONVERT(VARCHAR, @Idf_Pessoa_Fisica) + '
                    AND E.Idf_Escolaridade IN ( 4, 5, 6, 7 ) -- EFI EFC EMI EMC
                                                                     
            )
        ELSE 
            SELECT  E.Idf_Escolaridade ,
                    Des_BNE
            FROM    plataforma.TAB_Escolaridade E
            WHERE   E.Flg_BNE = 1
                    AND E.Idf_Escolaridade != 18 /* Diferente de outros cursos */
                    AND E.Flg_Inativo = 0
                    '
        IF (@Flg_Graduacao = 1)
	        BEGIN
		        SET @sql = @sql + ' AND (Idf_Escolaridade >= 4 AND Idf_Escolaridade <= 13) '
	        END         
        ELSE
	        BEGIN
		        SET @sql = @sql + ' AND (Idf_Escolaridade >= 14 AND Idf_Escolaridade <= 17) '
	        END   
                                            
        EXECUTE (@sql)";
        #endregion

        #region Métodos

        #region Listar
        /// <summary>
        /// Método retorna Consulta para carregar DropDownList
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> Listar()
        {
            #region Cache
            if (HabilitaCache)
                return Escolaridades.OrderBy(e => e.SequenciaBNE).Where(e => e.FlagBNE && e.Identificador != 18).ToDictionary(e => e.Identificador.ToString(), e => e.DescricaoBNE);
            #endregion

            var dicionarioNivel = new Dictionary<string, string>();

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Selectnivel, null))
            {
                while (dr.Read())
                {
                    dicionarioNivel.Add(dr["Idf_Escolaridade"].ToString(), dr["Des_Bne"].ToString());
                }
                if (!dr.IsClosed)
                    dr.Close();
            }

            return dicionarioNivel;
        }
        #endregion

        #region CarregarPorNome
        public static bool CarregarPorNome(string nome, out Escolaridade objEscolaridade)
        {
            var retorno = false;
            objEscolaridade = new Escolaridade();

            #region Cache
            if (HabilitaCache)
            {
                var escolaridade = Escolaridades.FirstOrDefault(e => e.FlagBNE && e.Identificador != 18 && e.DescricaoBNE.NormalizarStringLINQ().Equals(nome.NormalizarStringLINQ()));
                if (escolaridade != null)
                {
                    objEscolaridade = new Escolaridade { IdEscolaridade = escolaridade.Identificador, DescricaoBNE = escolaridade.DescricaoBNE, FlagBNE = escolaridade.FlagBNE, SequenciaBNE = escolaridade.SequenciaBNE, SequenciaPeso = escolaridade.SequenciaPeso };
                    retorno = true;
                }
                else
                    objEscolaridade = null;

                return retorno;
            }
            #endregion

            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Des_bne", SqlDbType = SqlDbType.VarChar, Size = 80, Value = nome }
                };

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Selectnivelpornome, parms))
            {
                if (SetInstance(dr, objEscolaridade))
                    retorno = true;
                else
                    objEscolaridade = null;

                if (!dr.IsClosed)
                    dr.Close();
            }

            return retorno;
        }
        #endregion

        #region ListaNivelEducacao
        /// <summary>
        /// Método responsável por carregar uma instância de Curso pelo id pessoa fisica
        /// </summary>
        /// <returns>Objeto Pessoa Fisica</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static IDataReader ListaNivelEducacao(int idPessoaFisica, bool graduacao)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Flg_Graduacao", SqlDbType.Bit));

            parms[0].Value = idPessoaFisica;
            parms[1].Value = graduacao;

            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTNIVELESCOLARIDADEPESSOAFISICA, parms);
        }
        #endregion

        #region LoadObject
        /// <summary>
        /// Método utilizado para retornar uma instância de Escolaridade a partir do banco de dados.
        /// </summary>
        /// <param name="idEscolaridade">Chave do registro.</param>
        /// <returns>Instância de Escolaridade.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static Escolaridade LoadObject(int idEscolaridade)
        {
            #region Cache
            if (HabilitaCache)
            {
                var escolaridade = Escolaridades.FirstOrDefault(e => e.Identificador == idEscolaridade);

                if (escolaridade != null)
                    return new Escolaridade { IdEscolaridade = escolaridade.Identificador, DescricaoBNE = escolaridade.DescricaoBNE, FlagBNE = escolaridade.FlagBNE, SequenciaBNE = escolaridade.SequenciaBNE, SequenciaPeso = escolaridade.SequenciaPeso };

                throw (new RecordNotFoundException(typeof(Escolaridade)));
            }
            #endregion

            using (IDataReader dr = LoadDataReader(idEscolaridade))
            {
                Escolaridade objEscolaridade = new Escolaridade();
                if (SetInstance(dr, objEscolaridade))
                    return objEscolaridade;
            }
            throw (new RecordNotFoundException(typeof(Escolaridade)));
        }
        /// <summary>
        /// Método utilizado para retornar uma instância de Escolaridade a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idEscolaridade">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Instância de Escolaridade.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static Escolaridade LoadObject(int idEscolaridade, SqlTransaction trans)
        {
            #region Cache
            if (HabilitaCache)
            {
                var escolaridade = Escolaridades.FirstOrDefault(e => e.Identificador == idEscolaridade);

                if (escolaridade != null)
                    return new Escolaridade { IdEscolaridade = escolaridade.Identificador, DescricaoBNE = escolaridade.DescricaoBNE, FlagBNE = escolaridade.FlagBNE, SequenciaBNE = escolaridade.SequenciaBNE, SequenciaPeso = escolaridade.SequenciaPeso };

                throw (new RecordNotFoundException(typeof(Escolaridade)));
            }
            #endregion

            using (IDataReader dr = LoadDataReader(idEscolaridade, trans))
            {
                Escolaridade objEscolaridade = new Escolaridade();
                if (SetInstance(dr, objEscolaridade))
                    return objEscolaridade;
            }
            throw (new RecordNotFoundException(typeof(Escolaridade)));
        }
        #endregion

        #region CompleteObject
        /// <summary>
        /// Método utilizado para completar uma instância de Escolaridade a partir do banco de dados.
        /// </summary>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public bool CompleteObject()
        {
            #region Cache
            if (HabilitaCache)
            {
                var escolaridade = Escolaridades.FirstOrDefault(e => e.Identificador == this._idEscolaridade);

                if (escolaridade != null)
                {
                    this.DescricaoBNE = escolaridade.DescricaoBNE;
                    this.FlagBNE = escolaridade.FlagBNE;
                    this.SequenciaBNE = escolaridade.SequenciaBNE;
                    this.SequenciaPeso = escolaridade.SequenciaPeso;
                    return true;
                }

                return false;
            }
            #endregion

            using (IDataReader dr = LoadDataReader(this._idEscolaridade))
            {
                return SetInstance(dr, this);
            }
        }
        /// <summary>
        /// Método utilizado para completar uma instância de Escolaridade a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public bool CompleteObject(SqlTransaction trans)
        {
            #region Cache
            if (HabilitaCache)
            {
                var escolaridade = Escolaridades.FirstOrDefault(e => e.Identificador == this._idEscolaridade);

                if (escolaridade != null)
                {
                    this.DescricaoBNE = escolaridade.DescricaoBNE;
                    this.FlagBNE = escolaridade.FlagBNE;
                    this.SequenciaBNE = escolaridade.SequenciaBNE;
                    this.SequenciaPeso = escolaridade.SequenciaPeso;
                    return true;
                }

                return false;
            }
            #endregion

            using (IDataReader dr = LoadDataReader(this._idEscolaridade, trans))
            {
                return SetInstance(dr, this);
            }
        }
        #endregion

        #endregion

    }
}