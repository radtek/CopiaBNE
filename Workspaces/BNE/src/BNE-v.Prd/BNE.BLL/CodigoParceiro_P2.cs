//-- Data: 19/01/2012 13:14
//-- Autor: Gieyson Stelmak

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Reporting.WebForms;
using System.Web.UI.WebControls;

namespace BNE.BLL
{
    public partial class CodigoParceiro // Tabela: BNE_Codigo_Parceiro
    {

        #region SPPESQUISARCODIGOSPARCEIRO
        private const string SPPESQUISARCODIGOSPARCEIRO = @"
        DECLARE @FirstRec INT
        DECLARE @LastRec INT
                                                                    
        SET @FirstRec = ( @CurrentPage * @PageSize + 1 )
        SET @LastRec = ( @CurrentPage * @PageSize ) + @PageSize
		
		IF EXISTS ( SELECT * FROM TEMPDB.DBO.SYSOBJECTS WHERE NAME = '#temp_results_codigo%' ) 
			DROP TABLE #temp_results_codigo
         
        CREATE TABLE #temp_results_codigo 
		(
			RowID INT NOT NULL,
            Num_CNPJ VARCHAR (14) ,
            Raz_Social NVARCHAR(100) ,
            Des_Status_Codigo_Parceiro NVARCHAR(100) ,
            Idf_Codigo_Parceiro INT ,
            Des_Codigo_Parceiro NVARCHAR(10) ,
            Des_Codigo_Seguranca_Parceiro NVARCHAR(10) ,
            Dta_Utilizacao DATETIME ,
            Num_CPF VARCHAR (11) ,
            Nme_Pessoa NVARCHAR(100) 
		)
       
		INSERT INTO #temp_results_codigo
		SELECT  
                ROW_NUMBER() OVER (ORDER BY CP.Dta_Cadastro ASC) AS RowID,  
                F.Num_CNPJ ,
                F.Raz_Social ,
                SCP.Des_Status_Codigo_Parceiro ,
                CP.Idf_Codigo_Parceiro ,
                CP.Des_Codigo_Parceiro ,
                CP.Des_Codigo_Seguranca_Parceiro ,
                CP.Dta_Utilizacao ,
                PF.Num_CPF ,
                PF.Nme_Pessoa 
        FROM    BNE.BNE_Codigo_Parceiro CP WITH(NOLOCK)
                INNER JOIN BNE.BNE_Parceiro P WITH(NOLOCK) ON CP.Idf_Parceiro = P.Idf_Parceiro
                INNER JOIN BNE.TAB_Filial F WITH(NOLOCK) ON P.Idf_Filial = F.Idf_Filial
                INNER JOIN BNE.BNE_Status_Codigo_Parceiro SCP WITH(NOLOCK) ON CP.Idf_Status_Codigo_Parceiro = SCP.Idf_Status_Codigo_Parceiro
                LEFT JOIN BNE.TAB_Usuario_Filial_Perfil UFP WITH(NOLOCK) ON CP.Idf_Utilizador = UFP.Idf_Usuario_Filial_Perfil
                LEFT JOIN BNE.TAB_Pessoa_Fisica PF WITH(NOLOCK) ON UFP.Idf_Pessoa_Fisica = PF.Idf_Pessoa_Fisica
        WHERE   (@CNPJRAZAOCODIGO IS NULL OR F.Num_CNPJ LIKE '%' + @CNPJRAZAOCODIGO + '%' OR F.Raz_Social LIKE '%' + @CNPJRAZAOCODIGO + '%' OR CP.Des_Codigo_Parceiro LIKE '%' + @CNPJRAZAOCODIGO + '%' )
                AND
                (@Dta_Inicial IS NULL OR CONVERT(VARCHAR, CP.Dta_Utilizacao, 112) >= CONVERT(VARCHAR, @Dta_Inicial, 112) )
                AND
                (@Dta_Final IS NULL OR CONVERT(VARCHAR, CP.Dta_Utilizacao, 112) <= CONVERT(VARCHAR, @Dta_Final, 112) )
                AND
                (@Idf_Status_Codigo_Parceiro IS NULL OR CP.Idf_Status_Codigo_Parceiro = @Idf_Status_Codigo_Parceiro)
                
        SELECT COUNT (rowid) FROM #temp_results_codigo
		SELECT * FROM #temp_results_codigo WHERE RowID >= @FirstRec AND RowID <= @LastRec
        ";
        #endregion

        #region Inserção em Massa
        /// <summary>
        /// Cria uma tabela para inserir em massa.
        /// Passe um DataTable nulo para e a classe em populada.  Ex: MensagemCS
        /// Ela irá instanciar um novo DataTable já com colunas definidas apartir dos parâmetros sql definidos na classe.
        /// Os valores setados nas propriedades são transformados em uma linha na tabela.
        /// </summary>
        /// <param name="tbMsg"></param>
        public void AddBulkTable(ref DataTable tbMsg)
        {
            DataAccessLayer.AddBulkTable(ref tbMsg, this);
        }
        /// <summary>
        /// Realiza inserção em massa.
        /// </summary>
        /// <param name="tbMsg">Tabela criada pelo método AddBulkTable</param>
        public static void SaveBulkTable(DataTable tbMsg)
        {
            DataAccessLayer.SaveBulkTable(tbMsg, "BNE_Codigo_Parceiro");
        }
        #endregion

        #region GerarArquivoCodigosGerados
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objFilial"></param>
        /// <param name="dataTable">DataTable com os códigos gerados em uma execução de geração de codigos. Pode ser NULL.</param>
        /// <returns></returns>
        public static ReportViewer GerarArquivoCodigosGerados(Filial objFilial, DataTable dataTable)
        {
            #region Variáveis
            ReportViewer rpv = new ReportViewer();
            ReportParameter[] parmsReport;
            #endregion

            #region DataTable
            DataTable dt = dataTable;
            #endregion

            #region ReportViewer
            rpv.Width = Unit.Percentage(100);
            rpv.Height = Unit.Percentage(100);
            rpv.AsyncRendering = false;
            rpv.ShowToolBar = false;
            rpv.LocalReport.ReportPath = "Reports/CodigosParceiro.rdlc";
            rpv.LocalReport.DataSources.Clear();
            rpv.LocalReport.DataSources.Add(new ReportDataSource("Codigos_Parceiro", dt));

            parmsReport = new ReportParameter[] { new ReportParameter("usuario", Custom.Relatorio.RecuperarNomeUsuarioLogado()) };

            rpv.LocalReport.SetParameters(parmsReport);
            #endregion

            return rpv;
        }
        #endregion

        #region PesquisarCodigosParceiro
        public static DataTable PesquisarCodigosParceiro(string cnpjRazaoCodigo, DateTime? dataInicial, DateTime? dataFinal, int? statusCodigoParceiro, int paginaAtual, int tamanhoPaginacao, out int totalRegistros)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@CurrentPage", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@PageSize", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@CNPJRAZAOCODIGO", SqlDbType.VarChar, 40));
            parms.Add(new SqlParameter("@Dta_Inicial", SqlDbType.DateTime));
            parms.Add(new SqlParameter("@Dta_Final", SqlDbType.DateTime, 40));
            parms.Add(new SqlParameter("@Idf_Status_Codigo_Parceiro", SqlDbType.Int, 2));

            parms[0].Value = paginaAtual;
            parms[1].Value = tamanhoPaginacao;

            if (!string.IsNullOrEmpty(cnpjRazaoCodigo))
                parms[2].Value = cnpjRazaoCodigo;
            else
                parms[2].Value = DBNull.Value;

            if (dataInicial.HasValue)
                parms[3].Value = dataInicial;
            else
                parms[3].Value = DBNull.Value;

            if (dataFinal.HasValue)
                parms[4].Value = dataFinal;
            else
                parms[4].Value = DBNull.Value;

            if (statusCodigoParceiro.HasValue)
                parms[5].Value = (int)statusCodigoParceiro;
            else
                parms[5].Value = DBNull.Value;

            totalRegistros = 0;
            DataTable dt = null;
            try
            {
                using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SPPESQUISARCODIGOSPARCEIRO, parms))
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

    }
}
