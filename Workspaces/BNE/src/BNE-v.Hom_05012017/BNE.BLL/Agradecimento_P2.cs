//-- Data: 17/11/2010 11:08
//-- Autor: Bruno Flammarion Chervisnki Boscolo

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
namespace BNE.BLL
{
    public partial class Agradecimento // Tabela: BNE_Agradecimento
    {

        #region Consultas

        #region SPSELECTAGRADECIMENTO
        
        private const string SPSELECTAGRADECIMENTO = @" DECLARE @Pagina VARCHAR(8000)
                                                        DECLARE @iSelect VARCHAR(8000)

                                                        DECLARE @LastRec INT
                                                        DECLARE @FirstRec INT
                                                        SET @LastRec = ( @CurrentPage * @QtdItens + 1 )
                                                        SET @FirstRec =  ( @CurrentPage - 1 ) * @QtdItens

                                                        SET @iSelect = 'SELECT  ROW_NUMBER() OVER ( ORDER BY A.Dta_Cadastro DESC ) AS Row_ID,A.* FROM    BNE_Agradecimento A'
                                                        SET @Pagina = 'SELECT * FROM (' + @iSelect + ') AS ISelect WHERE Row_ID > ' + CONVERT(VARCHAR, @FirstRec) + ' AND ROW_ID < ' + CONVERT(VARCHAR, @LastRec)
                                                        
                                                        EXEC(@Pagina)";
        #endregion

        #region SPCARREGARULTIMOROWID

        private const string SPCARREGARULTIMOROWID = @" DECLARE @Pagina VARCHAR(8000)
                                                        DECLARE @iSelect VARCHAR(8000)
                                                        DECLARE @UltimoRowId VARCHAR(8000)
                                                       
                                                        SET @iSelect = 'SELECT  ROW_NUMBER() OVER ( ORDER BY A.Dta_Cadastro DESC ) AS Row_ID, A.* FROM    BNE_Agradecimento A'
            
                                                        SET @UltimoRowId  = 'SELECT TOP 1 Row_ID FROM ('+ @iSelect +')AS UltimoRegistro ORDER BY Row_ID DESC'
                                                        EXEC(@UltimoRowId)";
        #endregion

        #region SPRECUPERARIDMAIORAGRADECIMENTO

        private const string SPRECUPERARIDMAIORAGRADECIMENTO = @"
            SELECT TOP 1
                    Idf_Agradecimento
            FROM    ( SELECT TOP 2
                                Idf_Agradecimento ,
                                Dta_Cadastro
                      FROM      BNE.BNE_Agradecimento
                      WHERE     Flg_Auditado = 1
                                AND Flg_Inativo = 0
                      ORDER BY  Dta_Cadastro DESC
                    ) AS temp
            ORDER BY temp.Dta_Cadastro ASC";

        #endregion

        #region SPRECUPERARAGRADECIMENTO

        private const string SPRECUPERARAGRADECIMENTO = @"
                            WITH    CTE
                                  AS ( SELECT   Row_ID = ROW_NUMBER() OVER ( ORDER BY Dta_Cadastro DESC ) ,
                                                *
                                       FROM     BNE.BNE_Agradecimento A
                                       WHERE    A.Flg_Auditado = 1
                                                AND A.Flg_Inativo = 0
                                     )
                            SELECT  ISNULL([Previous Row].[Idf_Agradecimento],
                                           ( SELECT TOP 1
                                                    [Idf_Agradecimento]
                                             FROM   CTE
                                             ORDER BY Row_ID DESC
                                           )) AS 'Idf_Agradecimento Anterior' ,
                                    ISNULL([Previous Row].[Nme_Pessoa], ( SELECT TOP 1
                                                                                    [Nme_Pessoa]
                                                                          FROM      CTE
                                                                          ORDER BY  Row_ID DESC
                                                                        )) AS 'Nme_Pessoa Anterior' ,
                                    ISNULL([Previous Row Cidade].[Nme_Cidade] + '/'
                                           + [Previous Row Cidade].[Sig_Estado],
                                           ( SELECT TOP 1
                                                    [Nme_Cidade] + '/' + [Sig_Estado]
                                             FROM   CTE
                                                    LEFT JOIN plataforma.TAB_Cidade CTECid ON CTE.Idf_Cidade = CTECid.Idf_Cidade
                                             ORDER BY Row_ID DESC
                                           )) AS 'Des_Cidade Anterior' ,
                                    ISNULL([Previous Row].[Des_Mensagem], ( SELECT TOP 1
                                                                                    [Des_Mensagem]
                                                                            FROM    CTE
                                                                            ORDER BY Row_ID DESC
                                                                          )) AS 'Des_Mensagem Anterior' ,
                                    [Current Row].[Idf_Agradecimento] AS 'Idf_Agradecimento Atual' ,
                                    [Current Row].[Des_Mensagem] AS 'Des_Mensagem Atual' ,
                                    [Current Row].[Nme_Pessoa] AS 'Nme_Pessoa Atual' ,
                                    [Current Row Cidade].[Nme_Cidade] + '/'
                                    + [Current Row Cidade].[Sig_Estado] AS 'Des_Cidade Atual' ,
                                    ISNULL([Next Row].[Idf_Agradecimento],
                                           ( SELECT [Idf_Agradecimento]
                                             FROM   CTE
                                             WHERE  Row_ID = 1
                                           )) AS 'Idf_Agradecimento Proximo' ,
                                    ISNULL([Next Row].[Nme_Pessoa], ( SELECT    [Nme_Pessoa]
                                                                      FROM      CTE
                                                                      WHERE     Row_ID = 1
                                                                    )) AS 'Nme_Pessoa Proximo' ,
                                    ISNULL([Next Row Cidade].[Nme_Cidade] + '/'
                                           + [Next Row Cidade].[Sig_Estado],
                                           ( SELECT [Nme_Cidade] + '/' + [Sig_Estado]
                                             FROM   CTE
                                                    LEFT JOIN plataforma.TAB_Cidade CTECid ON CTE.Idf_Cidade = CTECid.Idf_Cidade
                                             WHERE  Row_ID = 1
                                           )) AS 'Des_Cidade Proximo' ,
                                    ISNULL([Next Row].[Des_Mensagem], ( SELECT  [Des_Mensagem]
                                                                        FROM    CTE
                                                                        WHERE   Row_ID = 1
                                                                      )) AS 'Des_Mensagem Proximo'
                            FROM    CTE [Current Row]
                                    LEFT JOIN CTE [Previous Row] ON [Previous Row].Row_ID = [Current Row].Row_ID
                                                                    - 1
                                    LEFT JOIN CTE [Next Row] ON [Next Row].Row_ID = [Current Row].Row_ID
                                                                + 1
                                    LEFT JOIN plataforma.TAB_Cidade [Current Row Cidade] ON [Current Row].Idf_Cidade = [Current Row Cidade].Idf_Cidade
                                    LEFT JOIN plataforma.TAB_Cidade [Next Row Cidade] ON [Next Row].Idf_Cidade = [Next Row Cidade].Idf_Cidade
                                    LEFT JOIN plataforma.TAB_Cidade [Previous Row Cidade] ON [Previous Row].Idf_Cidade = [Previous Row Cidade].Idf_Cidade
                            WHERE   [Current Row].Idf_Agradecimento = @Idf_Agradecimento";

        #endregion

        #region SPSELECTAGRADECIMENTOS

        private const string SPSELECTAGRADECIMENTOS = @"
        DECLARE @FirstRec INT
        DECLARE @LastRec INT
        DECLARE @iSelect VARCHAR(8000)
        DECLARE @iSelectCount VARCHAR(8000)
        DECLARE @iSelectPag VARCHAR(8000)
        SET @FirstRec = ( @CurrentPage - 1 ) * @PageSize
        SET @LastRec = ( @CurrentPage * @PageSize + 1 )
        SET @iSelect = '
	        SELECT  ROW_NUMBER() OVER (ORDER BY Dta_Cadastro DESC) AS RowID ,
		        * FROM  (    SELECT	A.Idf_Agradecimento, 
									  A.Nme_Pessoa
									  ,A.Eml_Pessoa
									  ,C.Nme_Cidade
									 ,Substring(A.Des_Mensagem, 0, 20) + '' ...'' as Des_Mensagem
									  ,A.Dta_Cadastro
									  , Case when A.Flg_Auditado = 1 then ''Sim''
											 else ''Não'' End as Flg_Auditado
							FROM BNE_Agradecimento A
							inner join plataforma.Tab_cidade C on C.idf_cidade = A.idf_cidade ) as temp '

        SET @iSelectCount = 'Select Count(1) From ( ' + @iSelect + ' ) As TblTempCount'
        SET @iSelectPag = 'Select * From ( ' + @iSelect  + ' ) As TblTempPag	Where RowID > ' + CONVERT(VARCHAR, @FirstRec) + ' And RowID < ' + CONVERT(VARCHAR, @LastRec)
        EXECUTE (@iSelectCount)
        EXECUTE (@iSelectPag)";
        
        #endregion

        #region SPSELECTAGRADECIMENTOSFILTRO

        private const string SPSELECTAGRADECIMENTOSFILTRO = @"
        DECLARE @FirstRec INT
        DECLARE @LastRec INT
        DECLARE @iSelect VARCHAR(8000)
        DECLARE @iSelectCount VARCHAR(8000)
        DECLARE @iSelectPag VARCHAR(8000)
        SET @FirstRec = ( @CurrentPage - 1 ) * @PageSize
        SET @LastRec = ( @CurrentPage * @PageSize + 1 )
        SET @iSelect = '
	        SELECT  ROW_NUMBER() OVER (ORDER BY Dta_Cadastro DESC) AS RowID ,
		        * FROM  (    SELECT	A.Idf_Agradecimento, 
									  A.Nme_Pessoa
									  ,A.Eml_Pessoa
									  ,C.Nme_Cidade
									 ,Substring(A.Des_Mensagem, 0, 20) + '' ...'' as Des_Mensagem
									  ,A.Dta_Cadastro
									  , Case when A.Flg_Auditado = 1 then ''Sim''
											 else ''Não'' End as Flg_Auditado
							FROM BNE_Agradecimento A
							inner join plataforma.Tab_cidade C on C.idf_cidade = A.idf_cidade 
                               where A.Nme_Pessoa like ''%'+ @Filtro +'%''
								or A.Eml_Pessoa like ''%'+ @Filtro + '%''
								or A.Des_Mensagem like ''%'+ @Filtro + '%'') as temp '

        SET @iSelectCount = 'Select Count(1) From ( ' + @iSelect + ' ) As TblTempCount'
        SET @iSelectPag = 'Select * From ( ' + @iSelect  + ' ) As TblTempPag	Where RowID > ' + CONVERT(VARCHAR, @FirstRec) + ' And RowID < ' + CONVERT(VARCHAR, @LastRec)
        EXECUTE (@iSelectCount)
        EXECUTE (@iSelectPag)";

        #endregion

        #endregion

        #region Metodos
       
        #region ListarAgradecimento
        public static string CarregarAgradecimento(int paginaAtual, int quantidadeitens)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@CurrentPage", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@QtdItens", SqlDbType.Int, 4));
            parms[0].Value = paginaAtual;
            parms[1].Value = quantidadeitens;

            string agradecimento = string.Empty;
            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTAGRADECIMENTO, parms))
            {
                if (dr.Read())
                    agradecimento = dr["Des_Mensagem"].ToString();
            }

            return agradecimento;
        }
        #endregion

        #region RetornarUltimoRowId
        public static string RetornarUltimoRowId()
        {
            return DataAccessLayer.ExecuteScalar(CommandType.Text, SPCARREGARULTIMOROWID, null).ToString();
        }
        #endregion

        #region RetornarMaiorCodigoAgradecimento
        public static int RetornarMaiorCodigoAgradecimento()
        {
            return Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPRECUPERARIDMAIORAGRADECIMENTO, null));
        }
        #endregion

        #region ListarAgradecimentos
        public static Dictionary<int, MensagemAgradecimento> ListarAgradecimentos(int agradecimento)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Agradecimento", SqlDbType.Int, 4));
            parms[0].Value = agradecimento;

            Dictionary<int, MensagemAgradecimento> dicionario = new Dictionary<int, MensagemAgradecimento>();

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SPRECUPERARAGRADECIMENTO, parms))
            {
                if (dr.Read())
                {
                    dicionario.Add(Convert.ToInt32(dr["Idf_Agradecimento Anterior"]), new MensagemAgradecimento(Convert.ToInt32(dr["Idf_Agradecimento Anterior"]), dr["Des_Mensagem Anterior"].ToString(), dr["Nme_Pessoa Anterior"].ToString(), dr["Des_Cidade Anterior"].ToString()));
                    dicionario.Add(Convert.ToInt32(dr["Idf_Agradecimento Atual"]), new MensagemAgradecimento(Convert.ToInt32(dr["Idf_Agradecimento Atual"]), dr["Des_Mensagem Atual"].ToString(), dr["Nme_Pessoa Atual"].ToString(), dr["Des_Cidade Atual"].ToString()));
                    dicionario.Add(Convert.ToInt32(dr["Idf_Agradecimento Proximo"]), new MensagemAgradecimento(Convert.ToInt32(dr["Idf_Agradecimento Proximo"]), dr["Des_Mensagem Proximo"].ToString(), dr["Nme_Pessoa Proximo"].ToString(), dr["Des_Cidade Proximo"].ToString()));
                }
            }

            return dicionario;
        }
        #endregion

        #region SalvarAgradecimento
        public static bool SalvarAgradecimento(SqlTransaction trans, string nome, string email, Cidade cidade, string mensagem)
        {
            Agradecimento objAgradecimento = new Agradecimento();
            objAgradecimento.NomePessoa = nome;
            objAgradecimento.EmailPessoa = email;
            objAgradecimento.Cidade = cidade;
            objAgradecimento.DescricaoMensagem = mensagem;
            objAgradecimento.FlagAuditado = false;

            if (trans != null)
                objAgradecimento.Save(trans);
            else
                objAgradecimento.Save();

            return true;
        }
        #endregion

        #region ListarAgradecimentosDT

        public static DataTable ListarAgradecimentosDT(int paginaCorrente, int tamanhoPagina, out int totalRegistros)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@CurrentPage", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@PageSize", SqlDbType.Int, 4));

            parms[0].Value = paginaCorrente;
            parms[1].Value = tamanhoPagina;

            totalRegistros = 0;
            DataTable dt = null;
            try
            {
                using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTAGRADECIMENTOS, parms))
                {
                    if (dr.Read())
                        totalRegistros = Convert.ToInt32(dr[0]);

                    dr.NextResult();
                    dt = new DataTable();
                    dt.Load(dr);

                    if (!dr.IsClosed)
                        dr.Close();
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

        #region ListarAgradecimentosDT

        public static DataTable ListarAgradecimentosDT(string filtro, int paginaCorrente, int tamanhoPagina, out int totalRegistros)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@CurrentPage", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@PageSize", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Filtro", SqlDbType.VarChar, 50));

            parms[0].Value = paginaCorrente;
            parms[1].Value = tamanhoPagina;
            parms[2].Value = filtro;

            totalRegistros = 0;
            DataTable dt = null;
            try
            {
                using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTAGRADECIMENTOSFILTRO, parms))
                {
                    if (dr.Read())
                        totalRegistros = Convert.ToInt32(dr[0]);

                    dr.NextResult();
                    dt = new DataTable();
                    dt.Load(dr);

                    if (!dr.IsClosed)
                        dr.Close();
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

    public class MensagemAgradecimento
    {
        public int Idf_Agradecimento { get; private set; }
        public string Des_Agradecimento { get; private set; }
        public string Des_Usuario_Agradecimento { get; private set; }
        public string Des_Cidade_Agradecimento { get; private set; }

        public MensagemAgradecimento(int idAgradecimento, string desAgradecimento, string desUsuario, string desCidade)
        {
            this.Idf_Agradecimento = idAgradecimento;
            this.Des_Agradecimento = desAgradecimento;
            this.Des_Usuario_Agradecimento = desUsuario;
            this.Des_Cidade_Agradecimento = desCidade;
        }
    }
}