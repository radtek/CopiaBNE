//-- Data: 27/04/2010 11:34
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net;

namespace BNE.BLL
{
    public partial class CurriculoClassificacao // Tabela: BNE_Curriculo_Classificacao
    {

        #region Consultas
        
        #region Spselectdescricao
        private const string Spselectdescricao = @"
        SELECT  Des_Observacao
        FROM    BNE_Curriculo_Classificacao WITH(NOLOCK)
        WHERE   Idf_Curriculo_Observacao = @Idf_Curriculo_Observacao
        ";
        #endregion

        #region SpSelectCurriculoClassificao
        private const string SpSelectCurriculoClassificao = @"
        DECLARE @FirstRec INT
        DECLARE @LastRec INT
        DECLARE @iSelect VARCHAR(8000)
        DECLARE @iSelectCount NVARCHAR(4000)
        DECLARE @iSelectPag NVARCHAR(4000)
        SET @FirstRec = ( @CurrentPage * @PageSize ) + 1
        SET @LastRec = ( (@CurrentPage + 1) * @PageSize  )

        SET @iSelect = '
            SELECT  
                ROW_NUMBER() OVER (ORDER BY CC.Dta_Cadastro DESC) AS RowID,  
                CC.Dta_Cadastro ,
                CC.Des_Observacao ,
                PF.Nme_Pessoa ,
                CC.Idf_Curriculo_Classificacao ,
                CC.Idf_Usuario_Filial_Perfil ,
                CC.Idf_Avaliacao
            FROM    BNE.BNE_Curriculo_Classificacao CC WITH(NOLOCK)
		            LEFT JOIN BNE.TAB_Usuario_Filial_Perfil UFP WITH(NOLOCK) ON UFP.Idf_Usuario_Filial_Perfil = CC.Idf_Usuario_Filial_Perfil
		            LEFT JOIN BNE.TAB_Pessoa_Fisica PF WITH(NOLOCK) ON UFP.Idf_Pessoa_Fisica = PF.Idf_Pessoa_Fisica
            WHERE   CC.Idf_Filial = @Idf_Filial
                    AND CC.Idf_Curriculo = @Idf_Curriculo
                    AND CC.Flg_Inativo = 0
                    AND CC.Idf_Usuario_Filial_Perfil IS NOT NULL /* Retirando classificações importadas */'

        IF (@Idf_Usuario_Filial_Perfil IS NOT NULL)
            BEGIN
                SET @iSelect = @iSelect + ' AND CC.Idf_Usuario_Filial_Perfil = @Idf_Usuario_Filial_Perfil '
            END

        SET @iSelectCount = 'Select Count(1) From ( ' + @iSelect + ' ) As TblTempCount'
        SET @iSelectPag = 'Select * From ( ' + @iSelect  + ' ) As TblTempPag	Where RowID >= ' + CONVERT(VARCHAR, @FirstRec) + ' And RowID <= ' + CONVERT(VARCHAR, @LastRec)

        DECLARE @ParmDefinition NVARCHAR(1000)
        SET @ParmDefinition = N'@Idf_Filial INT, @Idf_Usuario_Filial_Perfil INT, @Idf_Curriculo INT';
  
        EXEC sp_executesql @iSelectCount, @ParmDefinition, @Idf_Filial = @Idf_Filial, @Idf_Usuario_Filial_Perfil = @Idf_Usuario_Filial_Perfil, @Idf_Curriculo = @Idf_Curriculo
        EXEC sp_executesql @iSelectPag, @ParmDefinition, @Idf_Filial = @Idf_Filial, @Idf_Usuario_Filial_Perfil = @Idf_Usuario_Filial_Perfil, @Idf_Curriculo = @Idf_Curriculo
        ";
        #endregion

        #region SpInativarCurriculoClassificacao
        private const string SpInativarCurriculoClassificacao = @"UPDATE BNE_Curriculo_Classificacao SET Flg_Inativo = 1 WHERE Idf_Curriculo_Classificacao = @Idf_Curriculo_Classificacao";
        #endregion

        #endregion

        #region Métodos

        #region Inativar
        /// <summary>
        /// Método utilizado para excluir uma instância de Curriculo Classificacao no banco de dados.
        /// </summary>
        /// <param name="idFilialObservacao">Chave do registro.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        public static void Inativar(int idFilialObservacao)
        {
            Inativar(idFilialObservacao, null);
        }
        /// <summary>
        /// Método utilizado para excluir uma instância de Curriculo Classificacao no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idCurriculoClassificacao">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        public static void Inativar(int idCurriculoClassificacao, SqlTransaction trans)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Curriculo_Classificacao", SqlDbType = SqlDbType.Int, Size = 4, Value = idCurriculoClassificacao }
                };

            if (trans != null)
                DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SpInativarCurriculoClassificacao, parms);
            else
                DataAccessLayer.ExecuteNonQuery(CommandType.Text, SpInativarCurriculoClassificacao, parms);
        }
        #endregion

        #region SalvarAvaliacao
        public static void SalvarAvaliacao(Filial objFilial, UsuarioFilialPerfil objUsuarioFilialPerfil, Curriculo objCurriculo, int? idClassificacao, string descricaoObservacao, Enumeradores.CurriculoClassificacao classificacao)
        {
            CurriculoClassificacao objCurriculoClassificacao;
            if (idClassificacao.HasValue)
                objCurriculoClassificacao = BLL.CurriculoClassificacao.LoadObject(idClassificacao.Value);
            else
            {
                objCurriculoClassificacao = new CurriculoClassificacao
                {
                    Curriculo = objCurriculo,
                    Filial = objFilial,
                    UsuarioFilialPerfil = objUsuarioFilialPerfil
                };
            }

            objCurriculoClassificacao.DescricaoObservacao = descricaoObservacao;
            objCurriculoClassificacao.Avaliacao = new Avaliacao((int)classificacao);
            objCurriculoClassificacao.Save();
        }
        #endregion

        #region EnviarAvaliacaoSolr
        public static void EnviarAvaliacaoSolr(Curriculo objCurriculo)
        {
            string urlSLOR = Parametro.RecuperaValorParametro(Enumeradores.Parametro.URLSolrAtualizaClassificacao);

            if (!String.IsNullOrEmpty(urlSLOR))
            {
                string url = urlSLOR + objCurriculo.IdCurriculo;
                WebRequest request = WebRequest.Create(url);
                using (var response = (HttpWebResponse)request.GetResponse())
                {

                }
            }
        }
        #endregion

        #region RecuperarDescricao
        /// <summary>
        /// Recupera a descrição da Observação
        /// </summary>
        /// <returns></returns>
        public string RecuperarDescricao()
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Curriculo_Observacao", SqlDbType = SqlDbType.Int, Size = 4, Value = _idCurriculoClassificacao }
                };

            return Convert.ToString(DataAccessLayer.ExecuteScalar(CommandType.Text, Spselectdescricao, parms));
        }
        #endregion

        #region ListarObservacoes
        /// <summary>
        /// Recupera um DataTable paginado com as observações ativas de uma filial
        /// </summary>
        /// <param name="objFilial"></param>
        /// <param name="paginaAtual"></param>
        /// <param name="tamanhoPagina"></param>
        /// <param name="totalRegistros"></param>
        /// <returns></returns>
        public static DataTable ListarObservacoes(Filial objFilial, UsuarioFilialPerfil objUsuarioFilialPerfil, Curriculo objCurriculo, int paginaAtual, int tamanhoPagina, out int totalRegistros)
        {
            object valueUsuarioFilialPerfil = DBNull.Value;

            if (objUsuarioFilialPerfil != null)
                valueUsuarioFilialPerfil = objUsuarioFilialPerfil.IdUsuarioFilialPerfil;

            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@CurrentPage", SqlDbType = SqlDbType.Int, Size = 4, Value = paginaAtual },
                    new SqlParameter { ParameterName = "@PageSize", SqlDbType = SqlDbType.Int, Size = 4, Value = tamanhoPagina },
                    new SqlParameter { ParameterName = "@Idf_Curriculo", SqlDbType = SqlDbType.Int, Size = 4, Value = objCurriculo.IdCurriculo },
                    new SqlParameter { ParameterName = "@Idf_Filial", SqlDbType = SqlDbType.Int, Size = 4, Value = objFilial.IdFilial },
                    new SqlParameter { ParameterName = "@Idf_Usuario_Filial_Perfil", SqlDbType = SqlDbType.Int, Size = 4, Value = valueUsuarioFilialPerfil }
                };

            totalRegistros = 0;
            DataTable dt = null;
            try
            {
                using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SpSelectCurriculoClassificao, parms))
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