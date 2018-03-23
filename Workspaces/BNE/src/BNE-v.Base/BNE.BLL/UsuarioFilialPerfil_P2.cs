//-- Data: 25/03/2010 17:04
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;
using System.Linq;
using BNE.BLL.Custom;
using BNE.BLL.Enumeradores;

namespace BNE.BLL
{
    public partial class UsuarioFilialPerfil // Tabela: TAB_Usuario_Filial_Perfil
    {

        #region Consultas

        #region SPSELECTFILIALPORPESSOAFISICAPAGINACAO
        private const string SPSELECTFILIALPORPESSOAFISICAPAGINACAO = @"
        DECLARE @FirstRec INT
        DECLARE @LastRec INT
        DECLARE @iSelect VARCHAR(8000)
        DECLARE @iSelectCount VARCHAR(8000)
        DECLARE @iSelectPag VARCHAR(8000)
        
        SET @FirstRec = ( @CurrentPage - 1 ) * @PageSize
        SET @LastRec = ( @CurrentPage * @PageSize + 1 )
        
        SET @iSelect = '
        SELECT  ROW_NUMBER() OVER (ORDER BY F.Dta_Cadastro DESC) AS RowID,
			    UFP.Idf_Filial,
                UFP.Idf_Usuario_Filial_Perfil,
			    F.Raz_Social,
                F.Num_CNPJ
		FROM    TAB_Usuario_Filial_Perfil UFP WITH(NOLOCK)
			    INNER JOIN TAB_Filial F WITH(NOLOCK) ON UFP.Idf_Filial = F.Idf_Filial
	    WHERE   Idf_Pessoa_Fisica = ' + CONVERT(VARCHAR, @Idf_Pessoa_Fisica) + '
                AND UFP.Flg_Inativo = 0'
                
        IF(@Des_Filtro IS NOT NULL)
        BEGIN
            SET @iSelect = @iSelect + ' AND (F.Raz_Social LIKE ''%'+ CONVERT(VARCHAR,@Des_Filtro) + '%'' OR F.Num_CNPJ LIKE ''%'+ CONVERT(VARCHAR,@Des_Filtro) + '%'')'
	    END
        		
        SET @iSelectCount = 'Select Count(*) From ( ' + @iSelect + ' ) As TblTempCount'
        SET @iSelectPag = 'Select * From ( ' + @iSelect + ' ) As TblTempPag	Where RowID > ' + CONVERT(VARCHAR, @FirstRec) + ' And RowID < ' + CONVERT(VARCHAR, @LastRec)
        
        EXECUTE (@iSelectCount)
        EXECUTE (@iSelectPag)";
        #endregion

        #region SPSELECTPORPESSOAFISICAFILIAL
        private const string SPSELECTPORPESSOAFISICAFILIAL = @"  
        SELECT  *
        FROM    TAB_Usuario_Filial_Perfil WITH (NOLOCK)
        WHERE   Idf_Pessoa_Fisica = @Idf_Pessoa_Fisica 
                AND Idf_Filial = @Idf_Filial";
        #endregion

        #region SPSELECTPORPESSOAFISICA
        private const string SPSELECTPORPESSOAFISICA = @"
        SELECT  *
        FROM    TAB_Usuario_Filial_Perfil WITH(NOLOCK)
        WHERE   Idf_Pessoa_Fisica = @Idf_Pessoa_Fisica 
                AND Flg_Inativo = 0";
        #endregion

        #region SPSELECTPORPESSOAFISICACONSIDERARUSUARIOINATIVO
        private const string SPSELECTPORPESSOAFISICACONSIDERARUSUARIOINATIVO = @"
        SELECT  *
        FROM    TAB_Usuario_Filial_Perfil WITH(NOLOCK)
        WHERE   Idf_Pessoa_Fisica = @Idf_Pessoa_Fisica";
        #endregion

        #region SPSELECTUSUARIOEMPRESAPORPESSOAFISICA
        private const string SPSELECTUSUARIOEMPRESAPORPESSOAFISICA = @"  
        SELECT  *
        FROM    TAB_Usuario_Filial_Perfil WITH(NOLOCK)
        WHERE   Idf_Pessoa_Fisica = @Idf_Pessoa_Fisica
                AND Idf_Filial IS NOT NULL
                AND Flg_Inativo = 0";
        #endregion

        #region SPSELECTUSUARIOEMPRESAPORFILIAL
        private const string SPSELECTUSUARIOEMPRESAPORFILIAL = @"  
        SELECT  TOP 1 *
        FROM    TAB_Usuario_Filial_Perfil WITH(NOLOCK)
        WHERE   Idf_Filial = @Idf_Filial
                AND Flg_Inativo = 0";
        #endregion

        #region SPSELECTPOREMPRESAPESSOAFISICA
        private const string SPSELECTPOREMPRESAPESSOAFISICA = @"
        SELECT  COUNT(Idf_Usuario_Filial_Perfil) 
        FROM    TAB_Usuario_Filial_Perfil WITH(NOLOCK)
        WHERE   Idf_Pessoa_Fisica = @Idf_Pessoa_Fisica
                AND Idf_Filial IS NOT NULL
                AND Flg_Inativo = 0";
        #endregion

        #region SPSELECTPOREMPRESAUSUARIOMASTER
        private const string SPSELECTPOREMPRESAUSUARIOMASTER = @" 
        SELECT  COUNT(UFP.Idf_Usuario_Filial_Perfil) 
        FROM    TAB_Usuario_Filial_Perfil UFP WITH(NOLOCK)
        WHERE   UFP.Idf_Filial = @Idf_Filial
                AND UFP.Idf_Perfil = @Idf_Perfil
                AND UFP.Flg_Inativo = 0";
        #endregion

        #region SPSELECTPORPERFILFILIAL
        private const string SPSELECTPORPERFILFILIAL = @"   
        SELECT  *
        FROM    TAB_Usuario_Filial_Perfil UFP WITH (NOLOCK)
        WHERE   UFP.Idf_Filial = @Idf_Filial 
                AND UFP.Flg_Inativo = 0";
        #endregion

        #region SPSELECTPORPERFILFILIALPESSOAFISICA
        private const string SPSELECTPORPERFILFILIALPESSOAFISICA = @"   
        SELECT  *
        FROM    TAB_Usuario_Filial_Perfil UFP WITH (NOLOCK)
        WHERE   UFP.Idf_Perfil = @Idf_Perfil
                AND UFP.Idf_Filial = @Idf_Filial 
                AND UFP.Idf_Pessoa_Fisica = @Idf_Pessoa_Fisica
                AND UFP.Flg_Inativo = 0
        ORDER BY Idf_Usuario_Filial_Perfil Desc";
        #endregion

        #region SPSELECTUSUARIONAOMASTERPORFILIAL
        private const string SPSELECTUSUARIONAOMASTERPORFILIAL = @"    
        SELECT  Nme_Pessoa ,
                Num_Cpf ,
                Dta_Nascimento ,
                Des_Sexo ,
                Des_Funcao ,
                Eml_Comercial ,
                Num_DDD_Celular ,
                Num_Celular ,
                Num_DDD_Comercial ,
                Num_Comercial ,
                REPLACE(REPLACE(REPLACE(xmlOut, '</row><row>', ','), '<row>', ''), '</row>', '') AS Des_Perfil
        FROM    ( SELECT  DISTINCT
                            PF.Nme_Pessoa ,
                            PF.Num_Cpf ,
                            PF.Dta_Nascimento ,
                            S.Des_Sexo ,
                            UFP.Eml_Comercial ,
                            ISNULL(F.Des_Funcao, UFP.Des_Funcao) AS Des_Funcao ,
                            PF.Num_Celular ,
                            PF.Num_DDD_Celular ,
                            UFP.Num_DDD_Comercial ,
                            UFP.Num_Comercial ,
                            ( SELECT    Ps.Des_Perfil AS [text()]
                                FROM      TAB_Perfil_Usuario PUs
                                        INNER JOIN TAB_Perfil Ps ON PUs.Idf_Perfil = Ps.Idf_Perfil
                                WHERE     PUs.Idf_Usuario_Filial_Perfil = PU.Idf_Usuario_Filial_Perfil
                            FOR
                                XML PATH /*, TYPE*/
                            ) AS xmlOut
                    FROM      TAB_Usuario_Filial_Perfil UFP
                            INNER JOIN TAB_Perfil_Usuario PU ON PU.Idf_Usuario_Filial_Perfil = UFP.Idf_Usuario_Filial_Perfil
                            INNER JOIN BNE_Usuario U ON UFP.Idf_Usuario = U.Idf_Usuario
                            INNER JOIN TAB_Pessoa_Fisica PF ON U.Idf_Pessoa_Fisica = PF.Idf_Pessoa_Fisica
                            INNER JOIN plataforma.TAB_Sexo S ON S.Idf_Sexo = PF.Idf_Sexo
                            LEFT JOIN plataforma.TAB_Funcao F ON F.Idf_Funcao = UFP.Idf_Funcao
                    WHERE     UFP.Idf_Filial = @Idf_Filial
                            AND UFP.Flg_Inativo = 0
                            AND 
		                    --PU.Idf_Perfil <> 6 -- Diferente de usuario master
                            PU.Flg_Inativo = 0
                ) AS temp";
        #endregion

        #region SPSELECTPERMISSOESUSUARIOFILIALPERFIL
        private const string SPSELECTPERMISSOESUSUARIOFILIALPERFIL =
                                 @" SELECT  Perm.Idf_Permissao
                                    FROM    plataforma.TAB_Permissao Perm
                                            INNER JOIN BNE.TAB_Perfil_Permissao PP ON Perm.Idf_Permissao = PP.Idf_Permissao
                                            INNER JOIN BNE.TAB_Perfil Perf ON Perf.Idf_Perfil = PP.Idf_Perfil
                                            INNER JOIN BNE.TAB_Usuario_Filial_Perfil UFP ON Perf.Idf_Perfil = UFP.Idf_Perfil
                                    WHERE   UFP.Idf_Usuario_Filial_Perfil = @Idf_Usuario_Filial_Perfil
                                            AND Perm.Idf_Categoria_Permissao = @Idf_Categoria_Permissao";
        #endregion

        #region SPVALIDARTIPOPERFILUSUARIO
        private const string SPVALIDARTIPOPERFILUSUARIO = @"SELECT  COUNT(1)
                                                            FROM    BNE.TAB_Usuario_Filial_Perfil UFP
                                                                    INNER JOIN BNE.TAB_Perfil P ON UFP.Idf_Perfil = P.Idf_Perfil
                                                            WHERE   UFP.Idf_Usuario_Filial_Perfil = @Idf_Usuario_Filial_Perfil
                                                                    AND P.Idf_Tipo_Perfil = @Idf_Tipo_Perfil";
        #endregion

        #region SPSELECTUSUARIOSCADASTRADOSPORFILIAL
        private const string SPSELECTUSUARIOSCADASTRADOSPORFILIAL = @"
        DECLARE @FirstRec INT
        DECLARE @LastRec INT
        DECLARE @iSelect VARCHAR(8000)
        DECLARE @iSelectCount VARCHAR(8000)
        DECLARE @iSelectPag VARCHAR(8000)
        SET @FirstRec = ( @CurrentPage - 1 ) * @PageSize
        SET @LastRec = ( @CurrentPage * @PageSize + 1 )
 
        SET @iSelect = '
		    SELECT 
            ROW_NUMBER() OVER (ORDER BY CONVERT(VARCHAR, PF.Dta_Cadastro) DESC) AS RowID,
            UFP.Idf_Usuario_Filial_Perfil ,
            CONVERT(VARCHAR, PF.Dta_Nascimento, 103) AS Dta_Nascimento ,
                CASE WHEN ( UF.Des_Funcao IS NULL )
                    THEN (  SELECT  Des_Funcao
                            FROM    plataforma.TAB_Funcao WITH(NOLOCK)
                            WHERE   Idf_Funcao = UF.Idf_Funcao
                        )
                    ELSE UF.Des_Funcao
                END AS Des_Funcao ,
            PF.Nme_Pessoa ,
            PF.Num_CPF ,
            ''('' + PF.Num_DDD_Celular + '') '' + SUBSTRING(PF.Num_Celular, 0, 5)
            + '' - '' + SUBSTRING(PF.Num_Celular, 5, 5) AS Num_Celular ,
            CASE WHEN (UFP.Idf_Perfil = ' + CONVERT(VARCHAR, @Idf_Perfil_Master) + ') THEN ''True''
                    ELSE ''False''
            END AS PerfilMaster,
            CASE WHEN (' + CONVERT(varchar, @BotaoExcluirVisible) + ' = 1) THEN ''True''
                            ELSE ''False''
                    END AS BotaoExcluirVisible ,
            UFP.Flg_Inativo
        FROM    BNE.TAB_Usuario_Filial_Perfil UFP WITH(NOLOCK)
                LEFT JOIN BNE.BNE_Usuario_Filial UF WITH(NOLOCK) ON UF.Idf_Usuario_Filial_Perfil = UFP.Idf_Usuario_Filial_Perfil
                JOIN BNE.TAB_Pessoa_Fisica PF WITH(NOLOCK) ON UFP.Idf_Pessoa_Fisica = PF.Idf_Pessoa_Fisica
                JOIN BNE.TAB_Perfil P WITH(NOLOCK) ON UFP.Idf_Perfil = P.Idf_Perfil
        WHERE   UFP.Idf_Pessoa_Fisica <> 5894 
                AND UFP.Idf_Filial =' + CONVERT(VARCHAR, @Idf_Filial)
 
        IF(@Idf_Perfil <> @Idf_Perfil_Master)
	        SET @iSelect = @iSelect + ' AND UFP.Idf_Usuario_Filial_Perfil =' + CONVERT(VARCHAR,@Idf_Usuario_Filial_Perfil)
        
        IF(@Flg_Administrador = 0)
	        SET @iSelect = @iSelect + ' AND UFP.Flg_Inativo = 0 '
                                                                    		
        SET @iSelectCount = 'Select Count(*) From ( ' + @iSelect + ' ) As TblTempCount'
        SET @iSelectPag = 'Select * From ( ' + @iSelect + ' ) As TblTempPag	Where RowID > ' + CONVERT(VARCHAR, @FirstRec) + ' And RowID < ' + CONVERT(VARCHAR, @LastRec)
        
        EXECUTE (@iSelectCount)
        EXECUTE (@iSelectPag)";
        #endregion

        #region SPVERIFICAUSUARIOPORULTIMAEMPRESALOGADA
        private const string SPVERIFICAUSUARIOPORULTIMAEMPRESALOGADA = @"
        SELECT  COUNT(*)
        FROM    BNE.TAB_Usuario_Filial_Perfil UFP WITH (NOLOCK)
                INNER JOIN BNE.BNE_Usuario U WITH (NOLOCK) ON UFP.Idf_Pessoa_Fisica = U.Idf_Pessoa_Fisica
        WHERE   UFP.Idf_Pessoa_Fisica = @Idf_Pessoa_Fisica
                AND U.Idf_Ultima_Filial_Logada = @Idf_Ultima_Filial_Logada
                AND UFP.Idf_Filial = @Idf_Ultima_Filial_Logada
                AND UFP.Flg_Inativo = 0";
        #endregion

        #region SPCARREGARUFPPORTIPOPERFILATIVOSEINATIVOS
        private const string SPCARREGARUFPPORTIPOPERFILATIVOSEINATIVOS = @"
        SELECT  UFP.* 
        FROM    TAB_Usuario_Filial_Perfil UFP WITH (NOLOCK)
                JOIN TAB_Perfil P WITH (NOLOCK) ON UFP.Idf_Perfil = P.Idf_Perfil 
        WHERE   P.Idf_Tipo_Perfil = @Idf_Tipo_Perfil 
                AND UFP.Idf_Pessoa_Fisica = @Idf_Pessoa_Fisica";
        #endregion

        #region SPCARREGARUFPPORTIPOPERFILAPENASATIVOS
        private const string SPCARREGARUFPPORTIPOPERFILAPENASATIVOS = @"
        SELECT  UFP.* 
        FROM    TAB_Usuario_Filial_Perfil UFP WITH (NOLOCK)
                JOIN TAB_Perfil P WITH (NOLOCK) ON UFP.Idf_Perfil = P.Idf_Perfil 
        WHERE   P.Idf_Tipo_Perfil = @Idf_Tipo_Perfil 
                AND UFP.Idf_Pessoa_Fisica = @Idf_Pessoa_Fisica
                AND UFP.Flg_Inativo = 0";
        #endregion

        #region Splistarusuariosadministrador
        private const string Splistarusuariosadministrador = @"
        DECLARE @FirstRec INT
        DECLARE @LastRec INT
        DECLARE @iSelect NVARCHAR(MAX)
        DECLARE @iSelectCount NVARCHAR(MAX)
        DECLARE @iSelectPag NVARCHAR(MAX)
        
        SET @FirstRec = @CurrentPage * @PageSize + 1 
        SET @LastRec = @CurrentPage * @PageSize + @PageSize
        
        DECLARE @ParmDefinition NVARCHAR(1000)
        SET @ParmDefinition = N'@Idf_Perfil VARCHAR(200)'

        SET @iSelect = '
        SELECT	ROW_NUMBER() OVER (ORDER BY CONVERT(VARCHAR, PF.Nme_Pessoa) ASC) AS RowID,
		        PF.Nme_Pessoa, PF.Dta_Nascimento, PF.Num_CPF, UFP.Flg_Inativo, P.Des_Perfil, UFP.Idf_Usuario_Filial_Perfil
        FROM	TAB_Usuario_Filial_Perfil UFP WITH(NOLOCK)
		        INNER JOIN TAB_Pessoa_Fisica PF WITH(NOLOCK) ON UFP.Idf_Pessoa_Fisica = PF.Idf_Pessoa_Fisica
		        INNER JOIN TAB_Perfil P WITH(NOLOCK) ON UFP.Idf_Perfil = P.Idf_Perfil
        WHERE	P.Idf_Perfil IN ('+ CONVERT(VARCHAR(200), @Idf_Perfil) + ')
                AND UFP.Flg_Inativo = 0'
                                                            		
        SET @iSelectCount = 'Select Count(*) From ( ' + @iSelect + ' ) As TblTempCount'
        SET @iSelectPag = 'Select * From ( ' + @iSelect + ' ) As TblTempPag	Where RowID >= ' + CONVERT(VARCHAR, @FirstRec) + ' And RowID <= ' + CONVERT(VARCHAR, @LastRec)

        EXEC sp_executesql @iSelectCount, @ParmDefinition, @Idf_Perfil
        EXEC sp_executesql @iSelectPag, @ParmDefinition, @Idf_Perfil
        ";
        #endregion

        #region SPSELECTPORPERFILPESSOAFISICA
        private const string SPSELECTPORPERFILPESSOAFISICA = @"   
        SELECT  *
        FROM    TAB_Usuario_Filial_Perfil UFP WITH (NOLOCK)
        WHERE   UFP.Idf_Perfil = @Idf_Perfil
                AND UFP.Idf_Pessoa_Fisica = @Idf_Pessoa_Fisica
        ";
        #endregion

        #region Spverificarusuariofilialperfil
        private const string Spverificarusuariofilialperfil = @" 
        SELECT  COUNT(UFP.Idf_Usuario_Filial_Perfil) 
        FROM    TAB_Usuario_Filial_Perfil UFP WITH(NOLOCK)
        WHERE   UFP.Idf_Filial = @Idf_Filial
                AND UFP.Idf_Perfil = @Idf_Perfil
                AND UFP.Idf_Pessoa_Fisica = @Idf_Pessoa_Fisica
                AND UFP.Flg_Inativo = 0";
        #endregion


        #region SpBuscaVinculoUsuarioComPlanoSelecionadora
        private const string SpBuscaVinculoUsuarioComPlanoSelecionadora = @"
        SELECT
            UFP.Idf_Usuario_Filial_Perfil,
            PF.Nme_Pessoa,
			celSelec.Idf_Celular_Selecionador,
			celSelec.Idf_Celular,
			celSelec.Dta_Fim_Utilizacao
        FROM TAB_Usuario_Filial_Perfil UFP WITH(NOLOCK)
        JOIN BNE.TAB_Pessoa_Fisica PF WITH(NOLOCK) ON UFP.Idf_Pessoa_Fisica = PF.Idf_Pessoa_Fisica
		JOIN BNE.BNE_Celular_Selecionador celSelec WITH(NOLOCK) ON UFP.Idf_Usuario_Filial_Perfil = celSelec.Idf_Usuario_Filial_Perfil
        WHERE   UFP.idf_Filial = @Idf_Filial 
                AND UFP.Flg_Inativo = 0
                AND celSelec.Dta_Fim_Utilizacao IS NULL";
        #endregion

        #region SpSelectUsuariosAtivosPorFilialPerfil
        private const string SpSelectUsuariosAtivosPorFilialPerfil = @"
        SELECT
            UFP.Idf_Perfil,
            UFP.Idf_Usuario_Filial_Perfil,
            PF.Nme_Pessoa,
            PF.Num_DDD_Celular,
            PF.Num_Celular
        FROM TAB_Usuario_Filial_Perfil UFP WITH(NOLOCK)
        JOIN BNE.TAB_Pessoa_Fisica PF WITH(NOLOCK) ON UFP.Idf_Pessoa_Fisica = PF.Idf_Pessoa_Fisica
        WHERE   UFP.idf_Filial = @Idf_Filial 
                AND UFP.Flg_Inativo = 0
                AND PF.Num_DDD_Celular IS NOT NULL
				AND PF.Num_Celular IS NOT NULL";
        #endregion

        #region SpSelectNomeUsuarioAutoComplete
        private const string SpSelectNomeUsuarioAutoComplete = @"
        SELECT 
            PF.Nme_Pessoa
        FROM tab_usuario_filial_perfil UFP WITH(NOLOCK) 
        INNER JOIN BNE.TAB_Pessoa_Fisica PF WITH(NOLOCK) ON PF.Idf_Pessoa_Fisica = UFP.Idf_Pessoa_Fisica
        WHERE   PF.Nme_Pessoa LIKE + @Nme_Pessoa + '%'
                AND UFP.Flg_Inativo = ISNULL(@Flg_Inativo,  UFP.Flg_Inativo)
        ";
        #endregion

        #region SpSelectUsuariosAdministradorFiltros
        private const string SpSelectUsuariosAdministradorFiltros = @"
        DECLARE @FirstRec INT
        DECLARE @LastRec INT
        DECLARE @iSelect NVARCHAR(MAX)
        DECLARE @iSelectCount NVARCHAR(MAX)
        DECLARE @iSelectPag NVARCHAR(MAX)
        
        SET @FirstRec = @CurrentPage * @PageSize + 1 
        SET @LastRec = @CurrentPage * @PageSize + @PageSize
        
        DECLARE @ParmDefinition NVARCHAR(1000)
        SET @ParmDefinition = N'@Idf_Perfil VARCHAR(200)'

        SET @iSelect = '
        SELECT	ROW_NUMBER() OVER (ORDER BY CONVERT(VARCHAR, PF.Nme_Pessoa) ASC) AS RowID,
		        PF.Nme_Pessoa, PF.Dta_Nascimento, PF.Num_CPF, UFP.Flg_Inativo, P.Des_Perfil, UFP.Idf_Usuario_Filial_Perfil
        FROM	TAB_Usuario_Filial_Perfil UFP WITH(NOLOCK)
		        INNER JOIN TAB_Pessoa_Fisica PF WITH(NOLOCK) ON UFP.Idf_Pessoa_Fisica = PF.Idf_Pessoa_Fisica
		        INNER JOIN TAB_Perfil P WITH(NOLOCK) ON UFP.Idf_Perfil = P.Idf_Perfil
        WHERE	P.Idf_Perfil IN ('+ CONVERT(VARCHAR(200), @Idf_Perfil) + ')
		'
        IF(@Flg_Inativo IS NOT NULL)
            SET @iSelect = @iSelect + ' AND UFP.Flg_Inativo = ' + CONVERT(VARCHAR(1),@Flg_Inativo)	

		IF(@Num_CPF IS NOT NULL)
			SET @iSelect = @iSelect + ' AND PF.Num_CPF =' + CONVERT(VARCHAR(11),@Num_CPF)        
                 
		IF(@Nme_Pessoa IS NOT NULL)
			SET @iSelect = @iSelect + ' AND PF.Nme_Pessoa LIKE''' + '%' +  @Nme_Pessoa + '%'''	
				                                            		
        SET @iSelectCount = 'Select Count(*) From ( ' + @iSelect + ' ) As TblTempCount'
        SET @iSelectPag = 'Select * From ( ' + @iSelect + ' ) As TblTempPag	Where RowID >= ' + CONVERT(VARCHAR, @FirstRec) + ' And RowID <= ' + CONVERT(VARCHAR, @LastRec)

        EXEC sp_executesql @iSelectCount, @ParmDefinition, @Idf_Perfil
        EXEC sp_executesql @iSelectPag, @ParmDefinition, @Idf_Perfil
        ";
        #endregion
        
        #endregion

        #region Métodos

        #region RetornarUsuarioFilialPerfil
        /// <summary>
        /// Retorna um DataTable e a paginação como todos as Instancia de UsuarioFilalPerfil de um determinado Usuario.
        /// </summary>
        /// <param name="idPessoaFisica"></param>
        /// <param name="paginaCorrente"></param>
        /// <param name="tamanhoPagina"></param>
        /// <param name="totalRegistros"></param>
        /// <returns></returns>
        public static DataTable RetornarUsuarioFilialPerfil(int idPessoaFisica, int paginaCorrente, int tamanhoPagina, string desPesquisa, out int totalRegistros)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@CurrentPage", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@PageSize", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Des_Filtro", SqlDbType.VarChar, 1000));

            parms[0].Value = idPessoaFisica;
            parms[1].Value = paginaCorrente;
            parms[2].Value = tamanhoPagina;


            if (!string.IsNullOrEmpty(desPesquisa))
                parms[3].Value = desPesquisa;
            else
                parms[3].Value = DBNull.Value;


            totalRegistros = 0;
            DataTable dt = null;
            try
            {
                using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTFILIALPORPESSOAFISICAPAGINACAO, parms))
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

        #region ListarUsuariosNaoMasterPorFilial
        /// <summary>
        /// Retorna uma list como todos as Instancia de UsuarioFilalPerfil de um determinado Usuario.
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        public static IDataReader ListarUsuariosNaoMasterPorFilial(int idFilial)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Filial", SqlDbType.Int, 4));
            parms[0].Value = idFilial;

            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTUSUARIONAOMASTERPORFILIAL, parms);
        }
        #endregion

        #region QuantidadeUsuarioEmpresa
        /// <summary>
        /// Retorna um int que informa em quantas empresas a pessoa fisica possui
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        public static int QuantidadeUsuarioEmpresa(int idPessoaFisica)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Int, 4));
            parms[0].Value = idPessoaFisica;

            return Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTPOREMPRESAPESSOAFISICA, parms));
        }
        #endregion

        #region CarregarPorPessoaFisicaFilial
        /// <summary>
        /// Método responsável por carregar uma instancia de Usuario Filial Perfil através do
        /// identificar de um usuario e uma filial
        /// </summary>
        /// <param name="idUsuario">Identificador de um Usuário</param>
        /// <param name="idFilial">Identificador de uma Filial</param>
        /// <returns>Boolean</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static bool CarregarPorPessoaFisicaFilial(int idPessoaFisica, int idFilial, out UsuarioFilialPerfil objUsuarioFilialPerfil)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Filial", SqlDbType.Int, 4));
            parms[0].Value = idPessoaFisica;
            parms[1].Value = idFilial;

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPORPESSOAFISICAFILIAL, parms))
            {
                objUsuarioFilialPerfil = new UsuarioFilialPerfil();
                if (SetInstance(dr, objUsuarioFilialPerfil))
                    return true;

                if (!dr.IsClosed)
                    dr.Close();
            }
            objUsuarioFilialPerfil = null;
            return false;
        }
        #endregion

        #region CarregarUsuarioEmpresaPorPessoaFisica
        /// <summary>
        /// Método responsável por carregar uma instancia de Usuario Filial Perfil através do
        /// identificar de um usuario e uma filial
        /// </summary>
        /// <param name="idUsuario">Identificador de um Usuário</param>
        /// <param name="idFilial">Identificador de uma Filial</param>
        /// <returns>Boolean</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static bool CarregarUsuarioEmpresaPorPessoaFisica(int idPessoaFisica, out UsuarioFilialPerfil objUsuarioFilialPerfil)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Int, 4));
            parms[0].Value = idPessoaFisica;

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTUSUARIOEMPRESAPORPESSOAFISICA, parms))
            {
                objUsuarioFilialPerfil = new UsuarioFilialPerfil();
                if (SetInstance(dr, objUsuarioFilialPerfil))
                    return true;

                if (!dr.IsClosed)
                    dr.Close();
            }
            objUsuarioFilialPerfil = null;
            return false;
        }

        #endregion

        #region CarregarUsuarioEmpresaPorFilial
        /// <summary>
        /// Método responsável por carregar uma instancia de Usuario Filial Perfil através do
        /// identificador de uma filial. Carrega o primeiro usuario, independente de perfil.
        /// </summary>
        /// <param name="idFilial">Identificador de uma Filial</param>
        /// <returns>Boolean</returns>
        /// <remarks>Francisco Ribas</remarks>
        public static bool CarregarUsuarioEmpresaPorFilial(int idFilial, out UsuarioFilialPerfil objUsuarioFilialPerfil)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Filial", SqlDbType.Int, 4));
            parms[0].Value = idFilial;

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTUSUARIOEMPRESAPORFILIAL, parms))
            {
                objUsuarioFilialPerfil = new UsuarioFilialPerfil();
                if (SetInstance(dr, objUsuarioFilialPerfil))
                    return true;

                if (!dr.IsClosed)
                    dr.Close();
            }
            objUsuarioFilialPerfil = null;
            return false;
        }

        #endregion

        #region CarregarPorPessoaFisica

        public static bool CarregarPorPessoaFisica(int idPessoaFisica, out UsuarioFilialPerfil objUsuarioFilialPerfil, SqlTransaction trans = null)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Int, 4));
            parms[0].Value = idPessoaFisica;

            IDataReader dr;
            if (trans != null)
                dr = DataAccessLayer.ExecuteReader(trans, CommandType.Text, SPSELECTPORPESSOAFISICA, parms);
            else
                dr = DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPORPESSOAFISICA, parms);

            objUsuarioFilialPerfil = new UsuarioFilialPerfil();
            if (SetInstance(dr, objUsuarioFilialPerfil))
                return true;

            if (!dr.IsClosed)
                dr.Close();

            objUsuarioFilialPerfil = null;
            return false;
        }

        /// <summary>
        /// Carrega Usuario Filial Perfil atraves do IdPessoaFisica
        /// </summary>
        /// <param name="idPessoaFisica">Id da Pessoa Fisica</param>
        /// <param name="considerarUsuariosInativos">Se for setado como true não faz o filtro flg_inativo = 0 trazendo os usuarioFilialPerfil Inativos e Ativos</param>
        /// <returns></returns>
        public static UsuarioFilialPerfil CarregarPorPessoaFisica(int idPessoaFisica, bool considerarUsuariosInativos = false)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Int, 4));
            parms[0].Value = idPessoaFisica;

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, considerarUsuariosInativos ? SPSELECTPORPESSOAFISICACONSIDERARUSUARIOINATIVO : SPSELECTPORPESSOAFISICA, parms))
            {
                UsuarioFilialPerfil objUsuarioFilialPerfil = new UsuarioFilialPerfil();
                if (SetInstance(dr, objUsuarioFilialPerfil))
                    return objUsuarioFilialPerfil;
                else
                    return null;
            }
            throw (new RecordNotFoundException(typeof(UsuarioFilialPerfil)));
        }
        #endregion

        #region CarregarPorPerfilFilial
        /// <summary>
        /// Método responsável por carregar uma instancia de Usuario Filial Perfil através do
        /// identificar de um perfil e uma filial
        /// </summary>
        /// <param name="idUsuario">Identificador de um Usuário</param>
        /// <param name="idFilial">Identificador de uma Filial</param>
        /// <returns>Boolean</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static bool CarregarPorPerfilFilial(int idPerfil, int idFilial, out UsuarioFilialPerfil objUsuarioFilialPerfil, SqlTransaction trans = null)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Perfil", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Filial", SqlDbType.Int, 4));
            parms[0].Value = idPerfil;
            parms[1].Value = idFilial;

            IDataReader dr = null;

            if (trans != null)
                dr = DataAccessLayer.ExecuteReader(trans, CommandType.Text, SPSELECTPORPERFILFILIAL, parms);
            else
                dr = DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPORPERFILFILIAL, parms);

            objUsuarioFilialPerfil = new UsuarioFilialPerfil();
            if (SetInstance(dr, objUsuarioFilialPerfil))
                return true;
            objUsuarioFilialPerfil = null;
            return false;
        }
        /// <summary>
        /// Método responsável por carregar uma instancia de Usuario Filial Perfil através do
        /// identificar de um perfil e uma filial
        /// </summary>
        /// <param name="idUsuario">Identificador de um Usuário</param>
        /// <param name="idFilial">Identificador de uma Filial</param>
        /// <returns>Boolean</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static bool CarregarPorPerfilFilialPessoaFisica(int idPerfil, int idFilial, int idPessoaFisica, out UsuarioFilialPerfil objUsuarioFilialPerfil)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Perfil", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Filial", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Int, 4));
            parms[0].Value = idPerfil;
            parms[1].Value = idFilial;
            parms[2].Value = idPessoaFisica;

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPORPERFILFILIALPESSOAFISICA, parms))
            {
                objUsuarioFilialPerfil = new UsuarioFilialPerfil();
                if (SetInstance(dr, objUsuarioFilialPerfil))
                    return true;

                if (!dr.IsClosed)
                    dr.Close();
            }
            objUsuarioFilialPerfil = null;
            return false;
        }
        #endregion

        #region ExisteUsuarioLigadoFilialcomPerfil
        /// <summary>
        /// 
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        public static bool ExisteUsuarioLigadoFilialcomPerfil(int idFilial, int idPerfil)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Filial", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Perfil", SqlDbType.Int, 4));
            parms[0].Value = idFilial;
            parms[1].Value = idPerfil;

            return Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTPOREMPRESAUSUARIOMASTER, parms)) > 0;
        }
        #endregion

        #region PessoaFisicaPossuiPerfilNaFilial
        /// <summary>
        /// Retorna um boleano para identificar se determinada pessoa física possui este perfil na filial.
        /// </summary>
        /// <param name="objFilial"></param>
        /// <param name="objPessoaFisica"></param>
        /// <param name="objPerfil"></param>
        /// <returns></returns>
        public static bool PessoaFisicaPossuiPerfilNaFilial(Filial objFilial, PessoaFisica objPessoaFisica, Perfil objPerfil)
        {
            var parms = new List<SqlParameter>
			    {
			        new SqlParameter { ParameterName = "@Idf_Pessoa_Fisica", SqlDbType = SqlDbType.Int, Size = 4, Value = objPessoaFisica.IdPessoaFisica } ,
			        new SqlParameter { ParameterName = "@Idf_Perfil", SqlDbType = SqlDbType.Int, Size = 4, Value = objPerfil.IdPerfil },
			        new SqlParameter { ParameterName = "@Idf_Filial", SqlDbType = SqlDbType.Int, Size = 4, Value = objFilial.IdFilial }
			    };

            return Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, Spverificarusuariofilialperfil, parms)) > 0;
        }
        #endregion

        #region CarregarPermissoes
        /// <summary>
        /// Método responsável por retornar as permissões de um usuário filial perfil ligadas a uma determinada categoria de permissão.
        /// </summary>
        /// <param name="usuario">Usuário o qual devem ser carregadas as permissões.</param>
        /// <param name="categoria">Categoria que deve ser analisada para retornar as permissões.</param>
        /// <returns>Lista de ids das permissões que o usuário contém na categoria especificada.</returns>
        public static List<int> CarregarPermissoes(int idUsuarioFilialPerfil, Enumeradores.CategoriaPermissao enumCategoriaPermissao)
        {
            List<int> listaPermissoes = new List<int>();
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Categoria_Permissao", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Usuario_Filial_Perfil", SqlDbType.Int, 4));

            parms[0].Value = (int)enumCategoriaPermissao;
            parms[1].Value = idUsuarioFilialPerfil;

            IDataReader dr = null;
            try
            {
                dr = DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPERMISSOESUSUARIOFILIALPERFIL, parms);
                while (dr.Read())
                    listaPermissoes.Add((int)dr["Idf_Permissao"]);
            }
            finally
            {
                if (!dr.IsClosed)
                    dr.Close();

                dr.Dispose();
            }
            return listaPermissoes;
        }
        #endregion

        #region ValidarTipoPerfil
        /// <summary>
        /// Método responsável por verificar se o usuário logado possui o Tipo de Perfil informado
        /// </summary>
        /// <param name="idUsuarioFilialPerfil"></param>
        /// <returns></returns>
        public static bool ValidarTipoPerfil(int idUsuarioFilialPerfil, int idTipoPerfil)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Usuario_Filial_Perfil", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Tipo_Perfil", SqlDbType.Int, 4));

            parms[0].Value = idUsuarioFilialPerfil;
            parms[1].Value = idTipoPerfil;

            return (int)DataAccessLayer.ExecuteScalar(CommandType.Text, SPVALIDARTIPOPERFILUSUARIO, parms) > 0;
        }
        #endregion

        #region CarregarUsuariosCadastradosPorFilial
        /// <summary>
        /// Carrega os usuarios cadastrados pela filial informada
        /// </summary>
        /// <returns></returns>
        public static DataTable CarregarUsuariosCadastradosPorFilial(int paginaCorrente, int tamanhoPagina, int idFilial, int idPerfilUsuarioLogado, int idUsuarioFilialPerfilLogado, bool flagAdministrador, out int totalRegistros, SqlTransaction trans = null)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter("@Idf_Filial", SqlDbType.Int, 4),
                    new SqlParameter("@Idf_Perfil", SqlDbType.Int, 4),
                    new SqlParameter("@CurrentPage", SqlDbType.Int, 4),
                    new SqlParameter("@PageSize", SqlDbType.Int, 4),
                    new SqlParameter("@Idf_Perfil_Master", SqlDbType.Int, 4),
                    new SqlParameter("@Idf_Usuario_Filial_Perfil", SqlDbType.Int, 4),
                    new SqlParameter("@BotaoExcluirVisible", SqlDbType.Bit, 1),
                    new SqlParameter("@Flg_Administrador", SqlDbType.Bit, 1)
                };

            parms[0].Value = idFilial;
            parms[1].Value = idPerfilUsuarioLogado;
            parms[2].Value = paginaCorrente;
            parms[3].Value = tamanhoPagina;
            parms[4].Value = (int)Enumeradores.Perfil.AcessoEmpresaMaster;
            parms[5].Value = idUsuarioFilialPerfilLogado;

            if (idPerfilUsuarioLogado.Equals((int)Enumeradores.Perfil.AcessoEmpresaMaster))
                parms[6].Value = true;
            else
                parms[6].Value = false;

            parms[7].Value = flagAdministrador;

            totalRegistros = 0;
            DataTable dt = null;
            try
            {
                using (IDataReader dr = trans == null ? DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTUSUARIOSCADASTRADOSPORFILIAL, parms)
                                                        : DataAccessLayer.ExecuteReader(trans, CommandType.Text, SPSELECTUSUARIOSCADASTRADOSPORFILIAL, parms))
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

        #region VerificaUsuarioFilialPorUltimaEmpresaLogada
        /// <summary>
        /// Verifica se o usuário está anativo para aquela ultima empresa logada
        /// </summary>
        /// <param name="idFilial"></param>
        /// <returns></returns>
        public static bool VerificaUsuarioFilialPorUltimaEmpresaLogada(int idPessoaFisica, int idUltimaFilialLogada)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Ultima_Filial_Logada", SqlDbType.Int, 4));

            parms[0].Value = idPessoaFisica;
            parms[1].Value = idUltimaFilialLogada;

            return Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPVERIFICAUSUARIOPORULTIMAEMPRESALOGADA, parms)) > 0;
        }
        #endregion

        #region CarregarUsuarioFilialPerfilCandidatoAtivoEInativo
        /// <summary>
        /// Metodo responsável por carregar o usuario filial perfil por pessoa e tipo perfil candidato
        /// </summary>
        /// <param name="objPessoaFisica"></param>
        /// <param name="objUsuarioFilialPerfil"></param>
        /// <returns></returns>
        public static bool CarregarUsuarioFilialPerfilCandidatoAtivoEInativo(PessoaFisica objPessoaFisica, out UsuarioFilialPerfil objUsuarioFilialPerfil)
        {
            return CarregarUsuarioFilialPerfilCandidatoAtivoEInativo(objPessoaFisica, null, out objUsuarioFilialPerfil);
        }
        public static bool CarregarUsuarioFilialPerfilCandidatoAtivoEInativo(PessoaFisica objPessoaFisica, SqlTransaction trans, out UsuarioFilialPerfil objUsuarioFilialPerfil)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Tipo_Perfil", SqlDbType.Int, 4));

            parms[0].Value = objPessoaFisica.IdPessoaFisica;
            parms[1].Value = (int)BLL.Enumeradores.TipoPerfil.Candidato;

            IDataReader dr;

            if (trans != null)
                dr = DataAccessLayer.ExecuteReader(trans, CommandType.Text, SPCARREGARUFPPORTIPOPERFILATIVOSEINATIVOS, parms);
            else
                dr = DataAccessLayer.ExecuteReader(CommandType.Text, SPCARREGARUFPPORTIPOPERFILATIVOSEINATIVOS, parms);

            objUsuarioFilialPerfil = new UsuarioFilialPerfil();

            bool retorno = false;
            if (SetInstance(dr, objUsuarioFilialPerfil))
                retorno = true;
            else
                objUsuarioFilialPerfil = null;

            if (!dr.IsClosed)
                dr.Close();

            dr.Dispose();

            return retorno;
        }

        #endregion

        #region CarregarUsuarioFilialPerfilCandidatoAtivo
        /// <summary>
        /// Metodo responsável por carregar o usuario filial perfil ATIVO por pessoa e tipo perfil candidato
        /// </summary>
        /// <param name="idPessoaFisica"></param>
        /// <param name="objUsuarioFilialPerfil"></param>
        /// <returns></returns>
        public static bool CarregarUsuarioFilialPerfilCandidatoAtivo(PessoaFisica objPessoaFisica, out UsuarioFilialPerfil objUsuarioFilialPerfil)
        {
            return CarregarUsuarioFilialPerfilCandidatoAtivo(objPessoaFisica, null, out objUsuarioFilialPerfil);
        }
        public static bool CarregarUsuarioFilialPerfilCandidatoAtivo(PessoaFisica objPessoaFisica, SqlTransaction trans, out UsuarioFilialPerfil objUsuarioFilialPerfil)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Tipo_Perfil", SqlDbType.Int, 4));

            parms[0].Value = objPessoaFisica.IdPessoaFisica;
            parms[1].Value = (int)BLL.Enumeradores.TipoPerfil.Candidato;

            IDataReader dr;

            if (trans != null)
                dr = DataAccessLayer.ExecuteReader(trans, CommandType.Text, SPCARREGARUFPPORTIPOPERFILAPENASATIVOS, parms);
            else
                dr = DataAccessLayer.ExecuteReader(CommandType.Text, SPCARREGARUFPPORTIPOPERFILAPENASATIVOS, parms);

            objUsuarioFilialPerfil = new UsuarioFilialPerfil();

            bool retorno = false;
            if (SetInstance(dr, objUsuarioFilialPerfil))
                retorno = true;
            else
                objUsuarioFilialPerfil = null;

            if (!dr.IsClosed)
                dr.Close();

            dr.Dispose();

            return retorno;
        }
        #endregion

        #region CarregarUsuarioFilialPerfilPessoaFisicaPerfil
        /// <summary>
        /// Metodo responsável por carregar o usuario filial perfil ativo ou inativo por pessoa e perfil
        /// </summary>
        /// <param name="objPessoaFisica"></param>
        /// <param name="objPerfil"> </param>
        /// <param name="objUsuarioFilialPerfil"></param>
        /// <returns></returns>
        public static bool CarregarUsuarioFilialPerfilPessoaFisicaPerfil(PessoaFisica objPessoaFisica, Perfil objPerfil, out UsuarioFilialPerfil objUsuarioFilialPerfil, SqlTransaction trans = null)
        {
            var parms = new List<SqlParameter>
			    {
			        new SqlParameter { ParameterName = "@Idf_Pessoa_Fisica", SqlDbType = SqlDbType.Int, Size = 4, Value = objPessoaFisica.IdPessoaFisica } ,
			        new SqlParameter { ParameterName = "@Idf_Perfil", SqlDbType = SqlDbType.Int, Size = 4, Value = objPerfil.IdPerfil }
			    };

            IDataReader dr;

            if (trans != null)
                dr = DataAccessLayer.ExecuteReader(trans, CommandType.Text, SPSELECTPORPERFILPESSOAFISICA, parms);
            else
                dr = DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPORPERFILPESSOAFISICA, parms);

            objUsuarioFilialPerfil = new UsuarioFilialPerfil();

            bool retorno = false;
            if (SetInstance(dr, objUsuarioFilialPerfil))
                retorno = true;
            else
                objUsuarioFilialPerfil = null;

            if (!dr.IsClosed)
                dr.Close();

            dr.Dispose();

            return retorno;
        }
        #endregion

        #region ListarUsuariosAdministrador
        /// <summary>
        /// Retorna um DataTable paginado com todos os Usuários Administrador do BNE.
        /// </summary>
        /// <param name="idPerfil"></param>
        /// <param name="paginaCorrente"></param>
        /// <param name="tamanhoPagina"></param>
        /// <param name="totalRegistros"></param>
        /// <returns></returns>
        public static DataTable ListarUsuariosAdministrador(string idsPerfil, int paginaCorrente, int tamanhoPagina, out int totalRegistros)
        {

            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@CurrentPage", SqlDbType = SqlDbType.Int, Value = paginaCorrente },
                    new SqlParameter { ParameterName = "@PageSize", SqlDbType = SqlDbType.Int, Value = tamanhoPagina } ,
                    new SqlParameter { ParameterName = "@Idf_Perfil", SqlDbType = SqlDbType.VarChar, Value = idsPerfil }
                };

            totalRegistros = 0;
            DataTable dt = null;
            try
            {
                using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Splistarusuariosadministrador, parms))
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

        #region ListarUsuariosAdministradorPorObj
        /// <summary>
        /// Retorna um DataTable paginado com todos os Usuários Administrador do BNE.
        /// </summary>
        /// <param name="idPerfil"></param>
        /// <param name="paginaCorrente"></param>
        /// <param name="tamanhoPagina"></param>
        /// <param name="totalRegistros"></param>
        /// <returns></returns>
        public static DataTable ListarUsuariosAdministradorPorFiltros(string idPerfil, int? flgInativo, string cpf, string nome, int paginaCorrente, int tamanhoPagina, out int totalRegistros)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@CurrentPage", SqlDbType = SqlDbType.Int, Value = paginaCorrente },
                    new SqlParameter { ParameterName = "@PageSize", SqlDbType = SqlDbType.Int, Value = tamanhoPagina } ,
                    new SqlParameter { ParameterName = "@Idf_Perfil", SqlDbType = SqlDbType.VarChar, Value = idPerfil },
                    new SqlParameter { ParameterName = "@Flg_Inativo", SqlDbType = SqlDbType.Bit, Value = flgInativo },
                    new SqlParameter { ParameterName = "@Num_CPF", SqlDbType = SqlDbType.VarChar, Value = cpf},
                    new SqlParameter { ParameterName = "@Nme_Pessoa", SqlDbType = SqlDbType.VarChar, Value = nome}
                };

            if (flgInativo == null)
                parms[3].Value = DBNull.Value;

            if (cpf == "")
                parms[4].Value = DBNull.Value;

            if (nome == "")
                parms[5].Value = DBNull.Value;

            totalRegistros = 0;
            DataTable dt = null;
            try
            {
                using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SpSelectUsuariosAdministradorFiltros, parms))
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

        #region Salvar
        public void Salvar(Usuario objUsuario)
        {
            using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        Save(trans);
                        objUsuario.Save(trans);

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

        #region ListarUsuariosAtivosPorFilialPerfil
        /// <summary>
        /// Carrega os números de celular dos usuários ativos por filial, e que tem número de celular.
        /// </summary>
        /// <param name="idFilial"></param>
        /// <returns></returns>
        public static List<BLL.DTO.PessoaFisicaEnvioSMSTanque> CarregarEnvioUsuariosAtivosPorFilialPerfil_SaldoZerou(int idFilial)
        {
            List<BLL.DTO.PessoaFisicaEnvioSMSTanque> listaDestinatarios = new List<DTO.PessoaFisicaEnvioSMSTanque>();

            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Filial", SqlDbType = SqlDbType.Int, Size = 4, Value = idFilial }
                };

            var listCelulares = new List<int>();
            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SpSelectUsuariosAtivosPorFilialPerfil, parms))
            {
                var carta = CartaSMS.RecuperaValorConteudo(Enumeradores.CartaSMS.SMSAviso_SaldoZerou);

                while (dr.Read())
                {
                    string nomeCompleto = Convert.ToString(dr["Nme_Pessoa"]);
                    int AtePosicao = nomeCompleto.IndexOf(" ");
                    string primeiroNome = nomeCompleto.Substring(0, AtePosicao);

                    var objDestinatario = new BLL.DTO.PessoaFisicaEnvioSMSTanque
                    {
                        idDestinatario = Convert.ToInt32(dr["Idf_Usuario_Filial_Perfil"]),
                        dddCelular = Convert.ToString(dr["Num_DDD_Celular"]),
                        numeroCelular = Convert.ToString(dr["Num_Celular"]),
                        nomePessoa = Convert.ToString(dr["Nme_Pessoa"]),
                        mensagem = string.Format(carta, primeiroNome)
                    };

                    listaDestinatarios.Add(objDestinatario);
                }
            }

            return listaDestinatarios;
        }

        /// <summary>
        /// Método que verifica se algum usuário vinculado ao Idf_Filial tem Plano Celular Selecionadora
        /// </summary>    
        /// <param name="idFilial"></param>
        /// <returns>Retornar Verdadeiro se algum usuário tiver plano selecionadora</returns>
        public static bool PossuiPlanoSelecionadoraAtivo(int idFilial, SqlTransaction trans)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter{ ParameterName = "@Idf_Filial", SqlDbType = SqlDbType.Int, Size = 4, Value = idFilial }
                };

            return Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SpBuscaVinculoUsuarioComPlanoSelecionadora, parms)) > 0;
        }

        /// <summary>
        /// Carrega lista de destinatários (objDestinatario) dos usuários vinculados a empresa (idf_filial)
        /// </summary>
        /// <param name="idFilial"></param>
        /// <param name="qtdSMS"></param>
        /// <param name="qtdSMSUtilizado"></param>
        /// <returns></returns>
        public static List<BLL.DTO.PessoaFisicaEnvioSMSTanque> CarregarEnvioUsuariosAtivosPorFilialPerfil_SaldoDisponivel(int idFilial, int qtdSMS, int qtdSMSUtilizado)
        {
            List<BLL.DTO.PessoaFisicaEnvioSMSTanque> listaDestinatarios = new List<DTO.PessoaFisicaEnvioSMSTanque>();

            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Filial", SqlDbType = SqlDbType.Int, Size = 4, Value = idFilial }
                };

            var qtdSMSDisponivel = qtdSMS - qtdSMSUtilizado;
            var conteudoCarta = "";
            var idCarta = 0;

            #region CarregaObjetosParametrosFilial
            ParametroFilial objParametroFilialCarta;
            try
            {
                objParametroFilialCarta = ParametroFilial.LoadObject((int)Enumeradores.Parametro.EnviaCartaAvisoSMSSaldoEmpresa, idFilial);
            }
            catch (RecordNotFoundException ex) { objParametroFilialCarta = null; }

            ParametroFilial objParametroFilialDtUltimoEnvio;
            try
            {
                objParametroFilialDtUltimoEnvio = ParametroFilial.LoadObject((int)Enumeradores.Parametro.DataUltimoEnvioCartaAvisoSMSSaldoEmpresa, idFilial);
            }
            catch (RecordNotFoundException ex) { objParametroFilialDtUltimoEnvio = null; }
            #endregion

            if (objParametroFilialCarta != null && objParametroFilialDtUltimoEnvio != null)
            {
                DateTime dataUltimoEnvio = (Convert.ToDateTime(objParametroFilialDtUltimoEnvio.ValorParametro));
                DateTime dataHoje = DateTime.Now;
                int daysDiff = ((TimeSpan)(dataHoje - dataUltimoEnvio)).Days;

                if (daysDiff < 30)
                    return null; //se empresa já recebeu um aviso de saldo de sms em menos de 30 dias, Não enviar outro  

                idCarta = AtualizaParametrosAvisoSaldoSMS(objParametroFilialCarta, objParametroFilialDtUltimoEnvio);
            }
            else
            {
                if (objParametroFilialCarta == null && objParametroFilialDtUltimoEnvio == null) //primeiro envio para a empresa
                    idCarta = SalvaParametrosAvisoSaldoSMS(idFilial);
                else if (objParametroFilialCarta == null) //Tem data do ultimo envio, mas Não tem parametro idCarta
                    idCarta = SalvaIdCarta_AtualizaData(objParametroFilialDtUltimoEnvio);
                else if (objParametroFilialDtUltimoEnvio == null) //Tem parametro idCarta, mas Não tem parametro data do ultimo envio
                    idCarta = SalvaData_AtualizaIdCarta(objParametroFilialCarta);
            }
            conteudoCarta = CartaSMS.LoadObject(idCarta).ValorCartaSMS;

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SpSelectUsuariosAtivosPorFilialPerfil, parms))
            {
                List<BNE.BLL.DTO.UsuarioFilialPerfiEnvioAvisoSaldoSMS> listaUsuariosCompleta = new List<DTO.UsuarioFilialPerfiEnvioAvisoSaldoSMS>();
                while (dr.Read())
                {
                    var objUsuarioEnvioSMS = new BNE.BLL.DTO.UsuarioFilialPerfiEnvioAvisoSaldoSMS
                    {
                        idPerfil = Convert.ToInt32(dr["Idf_Perfil"]),
                        idUsuarioFilialPerfil = Convert.ToInt32(dr["Idf_Usuario_Filial_Perfil"]), //idDestinatario
                        dddCelular = Convert.ToString(dr["Num_DDD_Celular"]),
                        numeroCelular = Convert.ToString(dr["Num_Celular"]),
                        nomePessoa = Convert.ToString(dr["Nme_Pessoa"]),
                        mensagem = string.Format(conteudoCarta, Convert.ToString(qtdSMSDisponivel))
                    };

                    listaUsuariosCompleta.Add(objUsuarioEnvioSMS);
                }

                //Refinar lista de destinatários Agrupando por números de celular para enviar somente um SMS para o mesmo número de celular
                //Caso existam números iguais, verificar se algum deles é perfil master, senão envia para o primeiro ordenando por idUsuarioFilialPerfil
                var dicUsuarios = listaUsuariosCompleta.GroupBy(p => p.dddCelular.ToString() + p.numeroCelular.ToString(), p => p)
                                        .ToDictionary(a => a.Key, b => b.OrderByDescending(a => a.idPerfil == (int)BLL.Enumeradores.Perfil.AcessoEmpresaMaster ? 1 : -1)
                                                                        .ThenBy(a => a.idUsuarioFilialPerfil)
                                                                        .First());

                foreach (var usuario in dicUsuarios)
                {

                    var objDestinatario = new BLL.DTO.PessoaFisicaEnvioSMSTanque
                    {
                        idDestinatario = Convert.ToInt32(usuario.Value.idUsuarioFilialPerfil),
                        dddCelular = Convert.ToString(usuario.Value.dddCelular),
                        numeroCelular = Convert.ToString(usuario.Value.numeroCelular),
                        nomePessoa = Convert.ToString(usuario.Value.nomePessoa),
                        mensagem = string.Format(conteudoCarta, Convert.ToString(qtdSMSDisponivel))
                    };

                    listaDestinatarios.Add(objDestinatario);
                }
            }

            return listaDestinatarios;
        }
        private static int AtualizaParametrosAvisoSaldoSMS(ParametroFilial objParametroFilial, ParametroFilial objParametroFilialDtUltimoEnvio)
        {
            int idCarta = 0;
            if (objParametroFilial.ValorParametro == ((int)Enumeradores.CartaSMS.SMSAviso_SaldoDisponivelMod1).ToString())
                idCarta = (int)Enumeradores.CartaSMS.SMSAviso_SaldoDisponivelMod2;
            else if (objParametroFilial.ValorParametro == ((int)Enumeradores.CartaSMS.SMSAviso_SaldoDisponivelMod2).ToString())
                idCarta = (int)Enumeradores.CartaSMS.SMSAviso_SaldoDisponivelMod3;
            else
                idCarta = (int)Enumeradores.CartaSMS.SMSAviso_SaldoDisponivelMod1;

            objParametroFilial.ValorParametro = idCarta.ToString();
            objParametroFilial.Save(); //atualiza vlr_parametro (idf_Carta_SMS)   

            objParametroFilialDtUltimoEnvio.ValorParametro = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            objParametroFilialDtUltimoEnvio.Save(); //atualiza  Vlr_Parametro (data do último envio)

            return idCarta;
        }
        private static int SalvaParametrosAvisoSaldoSMS(int idFilial)
        {
            //Salva Vlr_Parametro (idf_Carta_SMS)
            ParametroFilial objParametroFilial;
            objParametroFilial = new ParametroFilial
            {
                IdParametro = (int)Enumeradores.Parametro.EnviaCartaAvisoSMSSaldoEmpresa,
                IdFilial = idFilial,
                ValorParametro = ((int)Enumeradores.CartaSMS.SMSAviso_SaldoDisponivelMod1).ToString(),
                FlagInativo = false
            };
            objParametroFilial.Save();

            //Salva Vlr_Parametro (data do último envio) 
            objParametroFilial = new ParametroFilial
            {
                IdParametro = (int)Enumeradores.Parametro.DataUltimoEnvioCartaAvisoSMSSaldoEmpresa,
                IdFilial = idFilial,
                ValorParametro = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                FlagInativo = false
            };
            objParametroFilial.Save();

            var idCarta = (int)Enumeradores.CartaSMS.SMSAviso_SaldoDisponivelMod1;
            return idCarta;
        }
        private static int SalvaIdCarta_AtualizaData(ParametroFilial objParametroFilialDtUltimoEnvio)
        {
            //Salva Vlr_Parametro (idf_Carta_SMS)
            ParametroFilial objParametroFilial;
            objParametroFilial = new ParametroFilial
            {
                IdParametro = (int)Enumeradores.Parametro.EnviaCartaAvisoSMSSaldoEmpresa,
                IdFilial = objParametroFilialDtUltimoEnvio.IdFilial,
                ValorParametro = ((int)Enumeradores.CartaSMS.SMSAviso_SaldoDisponivelMod1).ToString(),
                FlagInativo = false
            };
            objParametroFilial.Save();

            //Atualiza Vlr_Parametro (data do último envio) 
            objParametroFilialDtUltimoEnvio.ValorParametro = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            objParametroFilialDtUltimoEnvio.Save();

            var idCarta = (int)Enumeradores.CartaSMS.SMSAviso_SaldoDisponivelMod2;
            return idCarta;
        }
        private static int SalvaData_AtualizaIdCarta(ParametroFilial objParametroFilial)
        {
            //Atualiza Vlr_Parametro (idf_Carta_SMS)
            int idCarta = 0;
            if (objParametroFilial.ValorParametro == ((int)Enumeradores.CartaSMS.SMSAviso_SaldoDisponivelMod1).ToString())
                idCarta = (int)Enumeradores.CartaSMS.SMSAviso_SaldoDisponivelMod2;
            else if (objParametroFilial.ValorParametro == ((int)Enumeradores.CartaSMS.SMSAviso_SaldoDisponivelMod2).ToString())
                idCarta = (int)Enumeradores.CartaSMS.SMSAviso_SaldoDisponivelMod3;
            else
                idCarta = (int)Enumeradores.CartaSMS.SMSAviso_SaldoDisponivelMod1;

            objParametroFilial.ValorParametro = idCarta.ToString();
            objParametroFilial.Save();

            //Salva Vlr_Parametro (data do último envio)
            objParametroFilial = new ParametroFilial
            {
                IdParametro = (int)Enumeradores.Parametro.DataUltimoEnvioCartaAvisoSMSSaldoEmpresa,
                IdFilial = objParametroFilial.IdFilial,
                ValorParametro = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                FlagInativo = false
            };
            objParametroFilial.Save();

            return idCarta;
        }

        #endregion

        #region ListarUsuariosAdministrador
        /// <summary>
        /// Retorna um DataTable paginado com todos os Usuários Administrador do BNE.
        /// </summary>
        /// <param name="idPerfil"></param>
        /// <param name="paginaCorrente"></param>
        /// <param name="tamanhoPagina"></param>
        /// <param name="totalRegistros"></param>
        /// <returns></returns>
        public static DataTable ListarUsuariosAdministradorPorNome(string idPerfil, string nomeParcial, int paginaCorrente, int tamanhoPagina, out int totalRegistros)
        {

            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@CurrentPage", SqlDbType = SqlDbType.Int, Value = paginaCorrente },
                    new SqlParameter { ParameterName = "@PageSize", SqlDbType = SqlDbType.Int, Value = tamanhoPagina } ,
                    new SqlParameter { ParameterName = "@Idf_Perfil", SqlDbType = SqlDbType.VarChar, Value = idPerfil }
                };

            totalRegistros = 0;
            DataTable dt = null;
            try
            {
                using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Splistarusuariosadministrador, parms))
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

        #region RecuperarFuncoes
        /// <summary>
        /// Método que retorna uma lista de funcoes
        /// </summary>
        /// <param name="nomeParcial">Parte do nome da funcao.</param>
        /// <param name="numeroRegistros">Quantidade de registros a serem retornados.</param>
        /// <returns>Lista de nomes de funcoes.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static string[] RecuperarUsuariosPorNome(int? idPerfil, string nomeParcial, int numeroRegistros, int? idOrigem)
        {
            List<string> funcoes = new List<string>();

            //var status = Session["Usuarios_SalaAdmConfiguracoes"];

            //recuperar parametros da tela de Usuários  httpcontext.current.session

            using (IDataReader dr = UsuarioFilialPerfil.ListarPorNomeParcial(idPerfil, nomeParcial, null, numeroRegistros))
            {
                while (dr.Read())
                    funcoes.Add(dr["Nme_Pessoa"].ToString());

                if (!dr.IsClosed)
                    dr.Close();
            }

            return funcoes.ToArray();
        }
        #endregion

        #region ListarPorNomeParcial
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados.
        /// </summary>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Valéria Neves</remarks>
        private static IDataReader ListarPorNomeParcial(int? idPerfil, string nome, int? flgInativo, int numeroRegistros)
        {
            var parms = new List<SqlParameter>
                            {
                                new SqlParameter("@idPerfil", SqlDbType.Int, 4),
                                new SqlParameter("@Nme_Pessoa", SqlDbType.VarChar, 80),
                                new SqlParameter("@Flg_Inativo", SqlDbType.Bit, 1),
                                new SqlParameter("@Count", SqlDbType.Int, 4)
                            };
            if (idPerfil != null)
                parms[0].Value = idPerfil;
            else
                parms[0].Value = DBNull.Value;

            parms[1].Value = nome.Replace(",", string.Empty); //Removendo a vírgula para evitar erro na fulltext

            if (flgInativo != null)
                parms[2].Value = flgInativo;
            else
                parms[2].Value = DBNull.Value;

            parms[3].Value = numeroRegistros;

            return DataAccessLayer.ExecuteReader(CommandType.Text, SpSelectNomeUsuarioAutoComplete, parms);
        }
        #endregion

        #region SetInstanceNotDipose
        internal static bool SetInstanceNotDipose(IDataReader dr, UsuarioFilialPerfil objUsuarioFilialPerfil)
        {
            try
            {
                objUsuarioFilialPerfil._idUsuarioFilialPerfil = Convert.ToInt32(dr["Idf_Usuario_Filial_Perfil"]);
                if (dr["Idf_Filial"] != DBNull.Value)
                    objUsuarioFilialPerfil._filial = new Filial(Convert.ToInt32(dr["Idf_Filial"]));
                if (dr["Des_IP"] != DBNull.Value)
                    objUsuarioFilialPerfil._descricaoIP = Convert.ToString(dr["Des_IP"]);
                objUsuarioFilialPerfil._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
                objUsuarioFilialPerfil._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
                objUsuarioFilialPerfil._pessoaFisica = new PessoaFisica(Convert.ToInt32(dr["Idf_Pessoa_Fisica"]));
                if (dr["Idf_Perfil"] != DBNull.Value)
                    objUsuarioFilialPerfil._perfil = new Perfil(Convert.ToInt32(dr["Idf_Perfil"]));
                objUsuarioFilialPerfil._senhaUsuarioFilialPerfil = Convert.ToString(dr["Sen_Usuario_Filial_Perfil"]);
                objUsuarioFilialPerfil._dataAlteracao = Convert.ToDateTime(dr["Dta_Alteracao"]);

                objUsuarioFilialPerfil._persisted = true;
                objUsuarioFilialPerfil._modified = false;

                return true;
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region RetornaHashLogarUsuarioEmpresa_CvsNaoVistosVaga
        /// <summary>
        /// Retorna Hash com a urlDestino (rota para PesquisaCurriculo.aspx), mais o idVaga cadastrada pelo usuário, 
        /// com objetivo de carregar a grid com currículos Não lidos da vaga
        /// </summary>
        /// <param name="idPessoaFisica"></param>
        /// <param name="idVaga"></param>
        /// <param name="linkPesquisaCurriculosDaVaga"></param>
        public static void RetornarHashLogarUsuarioEmpresa_CvsNaoVistosVaga(int idPessoaFisica, int idVaga,
                                                                            out string linkPesquisaCurriculosNaoVistosDaVaga,
                                                                            out string linkUrlHomeBNE,
                                                                            out string linkPaginaUsuario,
                                                                            out string linkAnunciarVagas,
                                                                            out string linkSalaSelecionadora,
                                                                            out string linkCompreCurriculos,
                                                                            out string linkAtualizarEmpresa,
                                                                            out string linkCvsRecebidos)
        {
            var objPessoaFisica = PessoaFisica.LoadObject(idPessoaFisica);
            
            string urlPadrao = string.Concat("http://", Helper.RecuperarURLAmbiente(), "/logar/");              

            string idVaga_cripto = Helper.ToBase64(idVaga.ToString()).ToString();
            string urlDestinoCvsNaoVistos = "~/" + Rota.RecuperarURLRota(RouteCollection.PesquisaCurriculosNaoVistosPorVaga); // ~/PesquisaCurriculo.aspx
            urlDestinoCvsNaoVistos = urlDestinoCvsNaoVistos.Replace("{VagaCvsNaoVistos}", idVaga_cripto);
            linkPesquisaCurriculosNaoVistosDaVaga = string.Concat(urlPadrao, LoginAutomatico.GerarHashAcessoLogin(objPessoaFisica, urlDestinoCvsNaoVistos));

            linkUrlHomeBNE = "http://www.bne.com.br";         

            string urlPaginaUsuario = "~/" + Rota.RecuperarURLRota(RouteCollection.SalaSelecionador); // ~/SalaSelecionador.aspx
            linkPaginaUsuario = string.Concat(urlPadrao, LoginAutomatico.GerarHashAcessoLogin(objPessoaFisica, urlPaginaUsuario));

            string urlDestinoAnunciarVagas = "~/" + Rota.RecuperarURLRota(RouteCollection.AnunciarVaga); // ~/AnunciarVaga.aspx
            linkAnunciarVagas = string.Concat(urlPadrao, LoginAutomatico.GerarHashAcessoLogin(objPessoaFisica, urlDestinoAnunciarVagas));

            string urlDestinoSalaSelecionadora = "~/" + Rota.RecuperarURLRota(RouteCollection.SalaSelecionador); // ~/SalaSelecionador.aspx
            linkSalaSelecionadora = string.Concat(urlPadrao, LoginAutomatico.GerarHashAcessoLogin(objPessoaFisica, urlDestinoSalaSelecionadora));

            string urlDestinoCompreCurriculos = "~/" + Rota.RecuperarURLRota(RouteCollection.ProdutoCIA); // ~/CIAProduto.aspx
            linkCompreCurriculos = string.Concat(urlPadrao, LoginAutomatico.GerarHashAcessoLogin(objPessoaFisica, urlDestinoCompreCurriculos));

            string urlDestinoAtualizarEmpresa = "~/" + Rota.RecuperarURLRota(RouteCollection.CadastroEmpresaDados); //~/CadastroEmpresaDados.aspx
            linkAtualizarEmpresa = string.Concat(urlPadrao, LoginAutomatico.GerarHashAcessoLogin(objPessoaFisica, urlDestinoAtualizarEmpresa));

            string urlDestinoCvsRecebidos = "~/" + Rota.RecuperarURLRota(RouteCollection.VagasAnunciadas); // ~/SalaSelecionadorVagasAnunciadas.aspx
            linkCvsRecebidos = string.Concat(urlPadrao, LoginAutomatico.GerarHashAcessoLogin(objPessoaFisica, urlDestinoCvsRecebidos));
        }
        #endregion RetornaHashLogarUsuarioEmpresa_CvsNaoVistosVaga


        #region ValidaExibicaoBannerSalaSelecionadora
        /// <summary>
        /// Valida no banco de dados a exibição da modal da tela sala da selecionadora.
        /// </summary>
        /// <param name="idUsuarioFilialPerfil"></param>
        /// <returns></returns>
        public static bool ValidaExibicaoBannerSalaSelecionadora(int idUsuarioFilialPerfil)
        {
            try
            { 
                string sql = "SELECT bne.sf_alerta_login(" + idUsuarioFilialPerfil + ")";
                var retorno = DataAccessLayer.ExecuteScalar(CommandType.Text, sql, null);
            
                return Convert.ToBoolean(retorno);
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #endregion

    }
}