//-- Data: 14/01/2011 14:16
//-- Autor: Vinicius Maciel

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;
namespace BNE.BLL
{
	public partial class Noticia // Tabela: BNE_Noticia
    {

        #region Atributos
        private int? _idNoticiaAnterior;
        private int? _idNoticiaProxima;

        #endregion

        #region Propriedades

        #region IdNoticiaAnterior
        public int? IdNoticiaAnterior
        {
            get
            {
                return this._idNoticiaAnterior;
            }
            set
            {
                this._idNoticiaAnterior = value;
                this._modified = true;
            }
        }
        #endregion 

        #region IdNoticiaProxima
        public int? IdNoticiaProxima
        {
            get
            {
                return this._idNoticiaProxima;
            }
            set
            {
                this._idNoticiaProxima = value;
                this._modified = true;
            }
        }
        #endregion

        #endregion

        #region Consultas

       private const string SPSELECTNOTICIASPORDATA = @"
        SELECT TOP (@Quantidade) Idf_Noticia, Nme_Titulo_Noticia, Nme_Link_Noticia, Flg_Noticia_BNE 
        FROM BNE_Noticia 
        WHERE
            Flg_Inativo = 0 AND Flg_Exibicao = 1 AND
            CONVERT(VARCHAR, Dta_Cadastro, 112) =  CONVERT(VARCHAR, @Dta_Cadastro, 112) ORDER BY Dta_Cadastro DESC ";

        private const string SPSELECTMAISNOTICIASPORDATA = @"
        SELECT  Idf_Noticia ,
                Nme_Titulo_Noticia ,
                Nme_Link_Noticia, 
                Flg_Noticia_BNE 
        FROM    BNE_Noticia
        WHERE   
                Flg_Inativo = 0 AND
                CONVERT(VARCHAR, Dta_Cadastro, 112) = CONVERT(VARCHAR, @Dta_Cadastro, 112)
                AND Idf_Noticia NOT IN (
                SELECT TOP (@Quantidade)
                        Idf_Noticia
                FROM    BNE_Noticia
                WHERE   
                    Flg_Inativo = 0 AND
                    CONVERT(VARCHAR, Dta_Cadastro, 112) = CONVERT(VARCHAR, @Dta_Cadastro, 112)
                ORDER BY Dta_Cadastro DESC )
        ORDER BY Dta_Cadastro DESC ";

        private const string SPSELECTULTIMASDATAS = @"
        SELECT 
			CONVERT(DATETIME, Dta_Cadastro) AS Dta_Cadastro, 
			PossuiMaisNoticias 
		FROM  (
			SELECT 
				TOP 3 CONVERT(VARCHAR, Dta_Cadastro, 112) AS Dta_Cadastro,
				CASE WHEN ( SELECT COUNT(Idf_Noticia) FROM BNE.BNE_Noticia NSub WHERE Flg_Inativo = 0 AND (CONVERT(VARCHAR, NSub.Dta_Cadastro, 112) = CONVERT(VARCHAR, N.Dta_Cadastro, 112)) GROUP BY CONVERT(VARCHAR, NSub.Dta_Cadastro, 112) ) > @Quantidade 
					THEN 'True'
					ELSE 'False'
				END AS PossuiMaisNoticias
			FROM BNE.BNE_Noticia N 
			WHERE Flg_Inativo = 0 
            AND Flg_Exibicao = 1
			GROUP BY CONVERT(VARCHAR, Dta_Cadastro, 112)
			ORDER BY CONVERT(VARCHAR, Dta_Cadastro, 112) DESC 
			) as temp";

        #region  SPNOTICIASLISTARMAISVISUALIZADAS
        private const string SPNOTICIASLISTARMAISVISUALIZADAS = @"
        SELECT TOP 3
                NV.Idf_Noticia,
                Nme_titulo_Noticia,
                Nme_Link_Noticia,
                Flg_Noticia_BNE
        FROM    BNE.BNE_Noticia_Visualizacao (NOLOCK) NV
                INNER JOIN BNE.BNE_Noticia (NOLOCK) N ON NV.Idf_Noticia = N.Idf_Noticia 
        WHERE  N.Flg_Inativo = 0
        AND    N.Flg_Exibicao = 1        
        GROUP BY NV.Idf_Noticia,
                Nme_titulo_Noticia,
                Nme_Link_Noticia,
                Flg_Noticia_BNE
        ORDER BY COUNT(NV.Idf_Noticia) DESC";

        #endregion

        private const string SPSELECTNOTICIAANTERIORPROXIMACTE = @"
        WITH CTE AS (
			        SELECT 
				        ROW_NUMBER() OVER (ORDER BY Dta_Cadastro DESC) AS Row_ID ,
				        N.*
			        FROM BNE.BNE_Noticia N
			        WHERE 
				        N.Flg_Inativo = 0
			        )
        SELECT 
            RowAtual.*,
            RowAnterior.Idf_Noticia AS Idf_Anterior ,
            RowProxima.Idf_Noticia AS Idf_Proxima 
        FROM CTE AS RowAtual
	        LEFT JOIN CTE AS RowAnterior ON RowAnterior.Row_ID = RowAtual.Row_ID - 1
	        LEFT JOIN CTE AS RowProxima ON RowProxima.Row_ID = RowAtual.Row_ID + 1
        WHERE RowAtual.Idf_Noticia = @Idf_Noticia";

        private const string SPSELECTNOTICIAS = @"
        --Ajustando os parâmetros para a query
        DECLARE @ParmDefinition NVARCHAR(1000)
        SET @ParmDefinition = N'@Flg_Inativo BIT, @Des_Titulo VARCHAR(100), @Dta_Publicacao DATETIME' ;

        DECLARE @FirstRec INT
        DECLARE @LastRec INT
        DECLARE @iSelect VARCHAR(8000)
        DECLARE @iSelectCount NVARCHAR(MAX)
        DECLARE @iSelectPag NVARCHAR(MAX)
        SET @FirstRec = ( @CurrentPage - 1 ) * @PageSize
        SET @LastRec = ( @CurrentPage * @PageSize + 1 )
        SET @iSelect = '
            SELECT  ROW_NUMBER() OVER (ORDER BY Dta_Cadastro DESC, Nme_Titulo_Noticia ASC) AS RowID,
                Idf_Noticia ,
                Nme_Titulo_Noticia ,
                Dta_Publicacao ,
                CASE Flg_Exibicao
                    WHEN 0 THEN ''Não''                
                    WHEN 1 THEN ''Sim''
                END AS Des_Exibicao       
            FROM BNE_Noticia (NOLOCK) P
            WHERE Flg_Inativo = @Flg_Inativo'
        
        IF (@Des_Titulo IS NOT NULL)
            SET @iSelect = @iSelect + ' AND Nme_Titulo_Noticia LIKE ''%' + @Des_Titulo + '%''';

        IF (@Dta_Publicacao IS NOT NULL)
            SET @iSelect = @iSelect + ' AND CONVERT(VARCHAR, Dta_Publicacao, 112) = CONVERT(VARCHAR, @Dta_Publicacao, 112)';
                              		
        SET @iSelectCount = 'Select Count(*) From ( ' + @iSelect + ' ) As TblTempCount'
        SET @iSelectPag = 'Select * From ( ' + @iSelect + ' ) As TblTempPag	Where RowID > ' + CONVERT(VARCHAR, @FirstRec) + ' And RowID < ' + CONVERT(VARCHAR, @LastRec)

        EXEC sp_executesql @iSelectCount, @ParmDefinition, @Flg_Inativo = @Flg_Inativo, @Des_Titulo = @Des_Titulo, @Dta_Publicacao = @Dta_Publicacao
        EXEC sp_executesql @iSelectPag, @ParmDefinition, @Flg_Inativo = @Flg_Inativo, @Des_Titulo = @Des_Titulo, @Dta_Publicacao = @Dta_Publicacao";

        #endregion

        #region Metodos

        #region ListarNoticiasPorData
        /// <summary>
        /// Método responsável por retornar um DataReader com todas as notícias específicas de uma data.
        /// </summary>
        /// <returns></returns>
        public static IDataReader ListarNoticiasPorData(DateTime data, int quantidadeItens)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime));
            parms.Add(new SqlParameter("@Quantidade", SqlDbType.Int, 4));
            parms[0].Value = data;
            parms[1].Value = quantidadeItens;

            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTNOTICIASPORDATA, parms);
        }
        #endregion

        #region ListarMaisNoticiasPorData
        /// <summary>
        /// Método responsável por retornar um DataReader com todas as notícias específicas de uma data.
        /// </summary>
        /// <returns></returns>
        public static IDataReader ListarMaisNoticiasPorData(DateTime data, int quantidadeItens)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime));
            parms.Add(new SqlParameter("@Quantidade", SqlDbType.Int, 4));
            parms[0].Value = data;
            parms[1].Value = quantidadeItens;

            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTMAISNOTICIASPORDATA, parms);
        }
        #endregion

        #region ListarNoticiasPorData
        /// <summary>
        /// Método responsável por retornar um DataReader com todas as notícias específicas de uma data.
        /// </summary>
        /// <returns></returns>
        private static IDataReader ListarUltimasDatas(int quantidadeItens)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Quantidade", SqlDbType.Int, 4));
            parms[0].Value = quantidadeItens;

            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTULTIMASDATAS, parms);
        }
        #endregion

        #region RecuperListaUltimasDatas
        /// <summary>
        /// Método que retorna uma lista com as datas das 3 últimas noticias cadastradas
        /// </summary>
        /// <returns></returns>
        public static Dictionary<DateTime, bool> RecuperListaUltimasDatas(int quantidadeItens)
        {
            

            Dictionary<DateTime, bool> dicDatas = new Dictionary<DateTime, bool>();
            using (IDataReader dr = ListarUltimasDatas(quantidadeItens))
            {
                while (dr.Read())
                    dicDatas.Add(Convert.ToDateTime(dr["Dta_Cadastro"]), Convert.ToBoolean(dr["PossuiMaisNoticias"]));
            }

            return dicDatas;
        }
        #endregion

        #region ListarNoticiasMaisVisualizadas
        /// <summary>
        /// Método utilizado por recuperar os top (quantidade) noticias em visualização
        /// </summary>
        /// <returns>IDataReader</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static IDataReader ListarNoticiasMaisVisualizadas(int quantidade)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Quantidade", SqlDbType.Int, 4));
            parms[0].Value = quantidade;

            return DataAccessLayer.ExecuteReader(CommandType.Text, SPNOTICIASLISTARMAISVISUALIZADAS, parms);
        }
        #endregion

        #region ListaDataReaderAnteriorProxima
        /// <summary>
        /// Método que retorna um data reader com uma instancia de noticia
        /// </summary>
        /// <param name="idNoticia"></param>
        /// <returns></returns>
        private static IDataReader ListaDataReaderAnteriorProxima(int idNoticia)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Noticia", SqlDbType.Int, 4));
            parms[0].Value = idNoticia;

            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTNOTICIAANTERIORPROXIMACTE, parms);
        }
        #endregion

        #region LoadObject
        /// <summary>
        /// Método utilizado para retornar uma instância de Noticia a partir do banco de dados.
        /// </summary>
        /// <param name="idNoticia">Chave do registro.</param>
        /// <returns>Instância de Noticia.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static Noticia LoadObjectAnteriorProximo(int idNoticia)
        {
            using (IDataReader dr = ListaDataReaderAnteriorProxima(idNoticia))
            {
                Noticia objNoticia = new Noticia();
                if (SetInstanceAnteriorProximo(dr, objNoticia))
                    return objNoticia;
            }
            throw (new RecordNotFoundException(typeof(Noticia)));
        }
        #endregion

        #region SetInstanceAnteriorProximo
        /// <summary>
        /// Método auxiliar utilizado pelos métodos LoadObject e CompleteObject para percorrer um IDataReader e vincular as colunas com os atributos da classe.
        /// </summary>
        /// <param name="dr">Cursor de leitura do banco de dados.</param>
        /// <param name="objNoticia">Instância a ser manipulada.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static bool SetInstanceAnteriorProximo(IDataReader dr, Noticia objNoticia)
        {
            try
            {
                if (dr.Read())
                {
                    objNoticia._idNoticia = Convert.ToInt32(dr["Idf_Noticia"]);
                    objNoticia._descricaoNoticia = Convert.ToString(dr["Des_Noticia"]);
                    objNoticia._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
                    objNoticia._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
                    objNoticia._nomeTituloNoticia = Convert.ToString(dr["Nme_Titulo_Noticia"]);

                    if (dr["Nme_Link_Noticia"] != DBNull.Value)
                        objNoticia._nomeLinkNoticia = Convert.ToString(dr["Nme_Link_Noticia"]);

                    if (dr["Idf_Anterior"] != DBNull.Value)
                        objNoticia._idNoticiaAnterior = Convert.ToInt32(dr["Idf_Anterior"]);

                    if (dr["Idf_Proxima"] != DBNull.Value)
                        objNoticia._idNoticiaProxima = Convert.ToInt32(dr["Idf_Proxima"]);

                    objNoticia._persisted = true;
                    objNoticia._modified = false;

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                dr.Dispose();
            }
        }
        #endregion

        #region ListarNoticiasPorDataTitulo
        public static DataTable ListarNoticiasPorDataTitulo(DateTime? data, String titulo, bool flgInativo, int paginaAtual, int tamanhoPagina, out int totalRegistros)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@CurrentPage", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@PageSize", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Flg_Inativo", SqlDbType.Bit));
            parms.Add(new SqlParameter("@Dta_Publicacao", SqlDbType.DateTime));
            parms.Add(new SqlParameter("@Des_Titulo", SqlDbType.VarChar, 100));

            parms[0].Value = paginaAtual;
            parms[1].Value = tamanhoPagina;
            parms[2].Value = flgInativo;

            if (data.HasValue)
                parms[3].Value = data.Value;
            else
                parms[3].Value = DBNull.Value;

            if (!String.IsNullOrEmpty(titulo))
                parms[4].Value = titulo;
            else
                parms[4].Value = DBNull.Value;

            DataTable dt = null;
            totalRegistros = 0;
            try
            {
                using (SqlDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTNOTICIAS, parms))
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
        
        #endregion

    }
}