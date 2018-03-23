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
using BNE.BLL.Mensagem.DTO;

namespace BNE.BLL
{
    public partial class UsuarioFilialPerfil : ICloneable // Tabela: TAB_Usuario_Filial_Perfil
    {

        #region Clone
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        #endregion

        #region Consultas

        #region SPInsertDadosOrigemAcesso
        private const string SPInsertDadosOrigemAcesso = "INSERT INTO TAB_Usuario_Filial_Perfil_Acesso (Idf_Usuario_Filial_Perfil,Dta_Acesso,Des_UTM_Source,Des_UTM_Medium,Des_UTM_Campaign,Des_UTM_Term,Des_Palavra_Chave) VALUES (@Idf_Usuario_Filial_Perfil,@Dta_Acesso,@Des_UTM_Source,@Des_UTM_Medium,@Des_UTM_Campaign,@Des_UTM_Term,@Des_Palavra_Chave);";
        #endregion

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

        #region SPSELECTUSUARIOEMPRESAPORPESSOAFISICA
        private const string SPSELECTUSUARIOEMPRESAPORPESSOAFISICA = @"  
        SELECT  *
        FROM    TAB_Usuario_Filial_Perfil WITH(NOLOCK)
        WHERE   Idf_Pessoa_Fisica = @Idf_Pessoa_Fisica
                AND Idf_Filial IS NOT NULL
                AND Flg_Inativo = 0";
        #endregion

        #region SP_SELECT_USUARIO_EMPRESA_POR_PESSOA_FISICA_CNPJ
        private const string SP_SELECT_USUARIO_EMPRESA_POR_PESSOA_FISICA_CNPJ = @"  
        SELECT  ufp.*
        FROM    TAB_Usuario_Filial_Perfil ufp WITH(NOLOCK)
        JOIN	BNE.TAB_Filial f WITH(NOLOCK) ON f.Idf_Filial = ufp.Idf_Filial
        WHERE   Idf_Pessoa_Fisica = @Idf_Pessoa_Fisica
		        AND f.Num_CNPJ = @Num_CNPJ
                AND f.Idf_Filial IS NOT NULL
                AND f.Flg_Inativo = 0 AND ufp.Flg_Inativo = 0";
        #endregion

        #region SPSELECTUSUARIOEMPRESAPORFILIAL
        private const string SPSELECTUSUARIOEMPRESAPORFILIAL = @"  
        SELECT  TOP 1 *
        FROM    TAB_Usuario_Filial_Perfil WITH(NOLOCK)
        WHERE   Idf_Filial = @Idf_Filial
                AND Flg_Inativo = 0";
        #endregion



        #region SP_CARREGAR_USUARIO_FILIAL_PERFIL_POR_CPF_E_FILIAL
        private const string SP_CARREGAR_USUARIO_FILIAL_PERFIL_POR_CPF_E_FILIAL = @"
            SELECT UFP.* 
	            FROM BNE.TAB_Usuario_Filial_Perfil UFP
	            INNER JOIN BNE.TAB_Pessoa_Fisica PF ON UFP.Idf_Pessoa_Fisica = PF.Idf_Pessoa_Fisica
            WHERE 1 = 1
	            AND PF.Num_CPF = @Num_CPF AND UFP.Idf_Filial = @Idf_Filial ";
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


        #region [SpSelectUsuarioMasterFilial]
        private const string SpSelectUsuarioMasterFilial = @"  SELECT  UFP.Idf_Usuario_Filial_Perfil ,
                PF.Nme_Pessoa
        FROM    BNE.TAB_Usuario_Filial_Perfil UFP WITH(NOLOCK )
                LEFT JOIN BNE.BNE_Usuario_Filial UF WITH(NOLOCK ) ON UF.Idf_Usuario_Filial_Perfil = UFP.Idf_Usuario_Filial_Perfil
              JOIN BNE.TAB_Pessoa_Fisica PF WITH ( NOLOCK ) ON UFP.Idf_Pessoa_Fisica = PF.Idf_Pessoa_Fisica
      WHERE   UFP.Idf_Pessoa_Fisica<> 5894
              AND ufp.flg_usuario_responsavel = 0 
              AND UFP.Idf_Filial = @Idf_Filial
              and UFP.Flg_Inativo = 0";
        #endregion

        #region SPSELECTUSUARIOSCADASTRADOSPORFILIAL
        private const string SPSELECTUSUARIOSCADASTRADOSPORFILIAL = @"
        SELECT  UFP.Idf_Usuario_Filial_Perfil ,
                UFP.Idf_Pessoa_Fisica ,
                CONVERT(VARCHAR, PF.Dta_Nascimento, 103) AS Dta_Nascimento ,
                CASE WHEN ( UF.Des_Funcao IS NULL )
                     THEN ( SELECT  Des_Funcao
                            FROM    plataforma.TAB_Funcao WITH ( NOLOCK )
                            WHERE   Idf_Funcao = UF.Idf_Funcao
                          )
                     ELSE UF.Des_Funcao
                END AS Des_Funcao ,
                PF.Nme_Pessoa ,
                PF.Num_CPF ,
                PF.Num_DDD_Celular ,
                PF.Num_Celular ,
                UFP.Idf_Perfil ,
                UFP.Flg_Inativo ,
                UFP.flg_usuario_responsavel,
                TotalCount = COUNT(*) OVER ( )
        FROM    BNE.TAB_Usuario_Filial_Perfil UFP WITH ( NOLOCK )
                LEFT JOIN BNE.BNE_Usuario_Filial UF WITH ( NOLOCK ) ON UF.Idf_Usuario_Filial_Perfil = UFP.Idf_Usuario_Filial_Perfil
                JOIN BNE.TAB_Pessoa_Fisica PF WITH ( NOLOCK ) ON UFP.Idf_Pessoa_Fisica = PF.Idf_Pessoa_Fisica
                JOIN BNE.TAB_Perfil P WITH ( NOLOCK ) ON UFP.Idf_Perfil = P.Idf_Perfil
        WHERE   UFP.Idf_Pessoa_Fisica <> 5894
                AND UFP.Idf_Filial = @Idf_Filial
                AND ( @Idf_Usuario_Filial_Perfil IS NULL
                      OR UFP.Idf_Usuario_Filial_Perfil = @Idf_Usuario_Filial_Perfil
                    )
                AND ( @Flg_Inativo IS NULL
                      OR UFP.Flg_Inativo = @Flg_Inativo
                    )
        ORDER BY PF.Dta_Cadastro DESC 
        OFFSET @PageSize * @PageNumber ROWS
        FETCH NEXT @PageSize ROWS ONLY;";
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
        SELECT  ROW_NUMBER() OVER (ORDER BY CONVERT(VARCHAR, PF.Nme_Pessoa) ASC) AS RowID,
                PF.Nme_Pessoa, PF.Dta_Nascimento, PF.Num_CPF, UFP.Flg_Inativo, P.Des_Perfil, UFP.Idf_Usuario_Filial_Perfil
        FROM    TAB_Usuario_Filial_Perfil UFP WITH(NOLOCK)
                INNER JOIN TAB_Pessoa_Fisica PF WITH(NOLOCK) ON UFP.Idf_Pessoa_Fisica = PF.Idf_Pessoa_Fisica
                INNER JOIN TAB_Perfil P WITH(NOLOCK) ON UFP.Idf_Perfil = P.Idf_Perfil
        WHERE   P.Idf_Perfil IN ('+ CONVERT(VARCHAR(200), @Idf_Perfil) + ')
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
        FROM    TAB_Usuario_Filial_Perfil UFP WITH(NOLOCK)
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

        #region SpSelectUsuariosFilial
        private const string SpSelectUsuariosFilial = @"
        SELECT  *
        FROM    TAB_Usuario_Filial_Perfil UFP WITH (NOLOCK)
        WHERE   UFP.Idf_Filial = @Idf_Filial 
        ";
        #endregion

        #region SPPRIMEIROUSUARIOMASTERFILIAL
        private const string SPPRIMEIROUSUARIOMASTERFILIAL = @"
        SELECT TOP 1
                Idf_Pessoa_Fisica
        FROM    TAB_Usuario_Filial_Perfil
        WHERE   Idf_Filial = @Idf_Filial
                AND Flg_Inativo = 0
                AND Idf_Perfil = @Idf_Perfil
        ORDER BY Dta_Cadastro DESC";
        #endregion

        #region Selectquantidadeusuarioresponsavel
        private const string Selectquantidadeusuarioresponsavel = @"
        SELECT COUNT(*) FROM TAB_Usuario_Filial_Perfil WHERE Idf_Filial = @Idf_Filial AND Flg_Usuario_Responsavel = 1
        ";
        #endregion

        #region Spselecttopnomeususarioporperfilativo
        private const string Spselecttopnomeususarioporperfilativo = @"
        SELECT  TOP (@top) PF.Nme_Pessoa
        FROM    TAB_Usuario_Filial_Perfil UFP WITH(NOLOCK)
                INNER JOIN TAB_Pessoa_Fisica PF WITH(NOLOCK) ON UFP.Idf_Pessoa_Fisica = PF.Idf_Pessoa_Fisica
        WHERE   Idf_Filial = @Idf_Filial
                AND UFP.Flg_Inativo = 0
                AND UFP.Idf_Perfil = @Idf_Perfil
        ORDER BY UFP.Dta_Cadastro ASC";
        #endregion

        #region Spselectemail
        private const string Spselectemail = @"
        SELECT  CASE WHEN UFP.Idf_Filial IS NULL THEN PF.Eml_Pessoa
                     WHEN UFP.Idf_Filial IS NOT NULL THEN UF.Eml_comercial
                     ELSE ''
                END AS Email
        FROM    BNE.TAB_Usuario_Filial_Perfil UFP
                LEFT JOIN bne.BNE_Usuario_Filial UF ON UF.Idf_Usuario_Filial_Perfil = UFP.Idf_Usuario_Filial_Perfil
                INNER JOIN BNE.TAB_Pessoa_Fisica PF ON PF.Idf_Pessoa_Fisica = UFP.Idf_Pessoa_Fisica
        WHERE   UFP.Idf_Usuario_Filial_Perfil = @Idf_Usuario_Filial_Perfil";
        #endregion

        #region Sppossuiperfilativo
        private const string Sppossuiperfilativo = @"SELECT COUNT(*) FROM TAB_Usuario_Filial_Perfil WITH(NOLOCK) WHERE Idf_Pessoa_Fisica = @Idf_Pessoa_Fisica AND Flg_Inativo = 0";
        #endregion

        #region Spselectprimeirousuarioperfilporemail
        private const string Spselectprimeirousuarioperfilporemail = @"
        SELECT  TOP 1 UFP.*
        FROM    BNE_Usuario_Filial UF WITH(NOLOCK)
                INNER JOIN TAB_Usuario_Filial_Perfil UFP WITH(NOLOCK) ON UF.Idf_Usuario_Filial_Perfil = UFP.Idf_Usuario_Filial_Perfil
                INNER JOIN TAB_Pessoa_Fisica PF WITH(NOLOCK) ON PF.Idf_Pessoa_Fisica = UFP.Idf_Pessoa_Fisica
        WHERE   Eml_Comercial LIKE @Eml_Comercial
                AND UFP.Flg_Inativo = 0
                AND PF.Flg_Inativo = 0
        ORDER BY UFP.Dta_Alteracao DESC
        ";
        #endregion

        #region spValidarPerfil
        private const string spValidarPerfil = @"SELECT  COUNT(1)
                                                            FROM    BNE.TAB_Usuario_Filial_Perfil with(Nolock) 
                                                            WHERE   Idf_Usuario_Filial_Perfil = @Idf_Usuario_Filial_Perfil
                                                                    AND Idf_Perfil = @Idf_Perfil";
        #endregion

        #region spRecuperarNomeUsuario
        private const string spRecuperarNomeUsuario = @"select top 1 pf.nme_pessoa from tab_usuario_filial_perfil ufp with(nolock) 
		 join tab_Pessoa_fisica pf with(nolock) on pf.idf_Pessoa_fisica = ufp.idf_pessoa_fisica
		 where ufp.Idf_Usuario_Filial_Perfil = @Idf_Usuario_Filial_Perfil 
";
        #endregion

        #region [spInativaResponsavel]
        private const string spInativaResponsavel = "UPDATE TAB_Usuario_Filial_Perfil SET flg_usuario_responsavel = 0, flg_inativo = 1 where idf_usuario_filial_perfil = @Idf_Inativar ";

        #region spPorCpfTipoPerfil
        const string spPorCpfTipoPerfil = @"
                SELECT ufp.* FROM BNE.TAB_Usuario_Filial_Perfil ufp
                JOIN BNE.TAB_Pessoa_Fisica pf ON ufp.Idf_Pessoa_Fisica = pf.Idf_Pessoa_Fisica
                WHERE ufp.Idf_Perfil = @Idf_Tipo_Perfil AND pf.Num_CPF = @Num_Cpf";
        #endregion spPorCpfTipoPerfil
        
        #endregion

        #region spAtivaNovoResponsavel]
        private const string spAtivaNovoResponsavel = "UPDATE TAB_Usuario_Filial_Perfil SET flg_usuario_responsavel = 1 where idf_usuario_filial_perfil = @Idf_Responsavel ";
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

        #region QuantidadeUsuarioEmpresa
        /// <summary>
        /// Retorna um int que informa em quantas empresas a pessoa fisica está ligada
        /// </summary>
        /// <param name="objPessoaFisica"></param>
        /// <returns></returns>
        public static int QuantidadeUsuarioEmpresa(PessoaFisica objPessoaFisica)
        {
            var parms = new List<SqlParameter>
			    {
			        new SqlParameter { ParameterName = "@Idf_Pessoa_Fisica", SqlDbType = SqlDbType.Int, Size = 4, Value = objPessoaFisica.IdPessoaFisica } 
			    };

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

        /// <summary>
        /// Método responsável por carregar uma instancia de Usuario Filial Perfil através do
        /// identificar de um usuario e uma filial
        /// </summary>
        /// <param name="idUsuario">Identificador de um Usuário</param>
        /// <param name="idFilial">Identificador de uma Filial</param>
        /// <returns>Boolean</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static bool CarregarUsuarioEmpresaPorPessoaFisica(int idPessoaFisica, decimal cnpj, out UsuarioFilialPerfil objUsuarioFilialPerfil)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Num_CNPJ", SqlDbType.Decimal, 14));
            parms[0].Value = idPessoaFisica;
            parms[1].Value = cnpj;

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SP_SELECT_USUARIO_EMPRESA_POR_PESSOA_FISICA_CNPJ, parms))
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

            var quantidade = (int)DataAccessLayer.ExecuteScalar(CommandType.Text, SPVALIDARTIPOPERFILUSUARIO, parms);
            return quantidade > 0;
        }
        #endregion


        #region [CarregarUsuariosMasterPorFilial]
        /// <summary>
        /// **** Não ira listar o responsável.
        /// </summary>
        /// <param name="idfilial"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        public static DataTable CarregarUsuariosMasterPorFilial(int idfilial, SqlTransaction trans = null)
        {
            var parm = new List<SqlParameter>
            {
                new SqlParameter { ParameterName = "@idf_filial", SqlDbType = SqlDbType.Int, Value = idfilial }
            };

            DataTable dt = null;
            try
            {
                using (IDataReader dr = DataAccessLayer.ExecuteReader(trans, CommandType.Text, SpSelectUsuarioMasterFilial, parm))
                {
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

        #region CarregarUsuariosCadastradosPorFilial
        /// <summary>
        /// Carrega os usuarios cadastrados pela filial informada
        /// </summary>
        /// <returns></returns>
        public static DataTable CarregarUsuariosCadastradosPorFilial(int paginaCorrente, int tamanhoPagina, int idFilial, int idPerfilUsuarioLogado, int idUsuarioFilialPerfilLogado, bool? flagAtivo, out int totalRegistros, SqlTransaction trans = null)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter("@Idf_Filial", SqlDbType.Int, 4),
                    new SqlParameter("@PageNumber", SqlDbType.Int, 4),
                    new SqlParameter("@PageSize", SqlDbType.Int, 4),
                    new SqlParameter("@Idf_Usuario_Filial_Perfil", SqlDbType.Int, 4),
                    new SqlParameter("@Flg_Inativo", SqlDbType.Bit, 1)
                };

            parms[0].Value = idFilial;
            parms[1].Value = paginaCorrente;
            parms[2].Value = tamanhoPagina;
            parms[3].Value = idPerfilUsuarioLogado != (int)Enumeradores.Perfil.AcessoEmpresaMaster ? (object)idUsuarioFilialPerfilLogado : DBNull.Value;

            if (flagAtivo.HasValue)
                parms[4].Value = (bool)flagAtivo ? 0 : 1;
            else
                parms[4].Value = DBNull.Value;

            totalRegistros = 0;
            DataTable dt = null;
            try
            {
                using (IDataReader dr = DataAccessLayer.ExecuteReader(trans, CommandType.Text, SPSELECTUSUARIOSCADASTRADOSPORFILIAL, parms))
                {
                    dt = new DataTable();
                    dt.Load(dr);

                    if (dt.Rows.Count > 0)
                    {
                        totalRegistros = Convert.ToInt32(dt.Rows[0]["TotalCount"]);
                    }
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


        #region ObterPorCpfPerfil
        /// <summary>
        /// Metodo responsável por carregar o usuario filial perfil por pessoa e tipo perfil candidato
        /// </summary>
        /// <param name="objPessoaFisica"></param>
        /// <param name="objUsuarioFilialPerfil"></param>
        /// <returns></returns>
        public static bool ObterPorCpfPerfil(decimal cpf, int idPerfil, out UsuarioFilialPerfil objUsuarioFilialPerfil)
        {
            return ObterPorCpfPerfil(cpf, idPerfil, null, out objUsuarioFilialPerfil);
        }
        public static bool ObterPorCpfPerfil(decimal cpf, int idPerfil, SqlTransaction trans, out UsuarioFilialPerfil objUsuarioFilialPerfil)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Num_Cpf", SqlDbType.Decimal, 11));
            parms.Add(new SqlParameter("@Idf_Tipo_Perfil", SqlDbType.Int, 4));

            parms[0].Value = cpf;
            parms[1].Value = idPerfil;

            IDataReader dr;

            if (trans != null)
                dr = DataAccessLayer.ExecuteReader(trans, CommandType.Text, spPorCpfTipoPerfil, parms);
            else
                dr = DataAccessLayer.ExecuteReader(CommandType.Text, spPorCpfTipoPerfil, parms);

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
        #endregion ObterPorCpfPerfil

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
        public static List<DestinatarioSMS> CarregarEnvioUsuariosAtivosPorFilialPerfil_SaldoZerou(int idFilial)
        {
            List<DestinatarioSMS> listaDestinatarios = new List<DestinatarioSMS>();

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

                    var objDestinatario = new DestinatarioSMS
                    {
                        IdDestinatario = Convert.ToInt32(dr["Idf_Usuario_Filial_Perfil"]),
                        DDDCelular = Convert.ToString(dr["Num_DDD_Celular"]),
                        NumeroCelular = Convert.ToString(dr["Num_Celular"]),
                        NomePessoa = Convert.ToString(dr["Nme_Pessoa"]),
                        Mensagem = string.Format(carta, primeiroNome)
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
        public static List<DestinatarioSMS> CarregarEnvioUsuariosAtivosPorFilialPerfil_SaldoDisponivel(int idFilial, int qtdSMS, int qtdSMSUtilizado)
        {
            List<DestinatarioSMS> listaDestinatarios = new List<DestinatarioSMS>();

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
                List<BLL.DTO.UsuarioFilialPerfiEnvioAvisoSaldoSMS> listaUsuariosCompleta = new List<DTO.UsuarioFilialPerfiEnvioAvisoSaldoSMS>();
                while (dr.Read())
                {
                    var objUsuarioEnvioSMS = new BLL.DTO.UsuarioFilialPerfiEnvioAvisoSaldoSMS
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

                    var objDestinatario = new DestinatarioSMS
                    {
                        IdDestinatario = Convert.ToInt32(usuario.Value.idUsuarioFilialPerfil),
                        DDDCelular = Convert.ToString(usuario.Value.dddCelular),
                        NumeroCelular = Convert.ToString(usuario.Value.numeroCelular),
                        NomePessoa = Convert.ToString(usuario.Value.nomePessoa),
                        Mensagem = string.Format(conteudoCarta, Convert.ToString(qtdSMSDisponivel))
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

            string urlDestinoAtualizarEmpresa = "~/" + Rota.RecuperarURLRota(RouteCollection.AtualizarDadosEmpresa); //~/CadastroEmpresaDados.aspx
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

        #region ListarUsuariosFilial

        /// <summary>
        /// Método responsável por retornar uma lista com todos os usuários de uma filial
        /// </summary>
        /// <param name="objFilial">Filial para recuperar os usuários</param>
        /// <param name="trans">SqlTransaction</param>
        /// <returns></returns>
        public static List<UsuarioFilialPerfil> ListarUsuariosFilial(Filial objFilial, SqlTransaction trans = null)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Filial", SqlDbType = SqlDbType.Int, Size = 4, Value = objFilial.IdFilial }
                };


            var lista = new List<UsuarioFilialPerfil>();

            IDataReader dr;
            if (trans != null)
                dr = DataAccessLayer.ExecuteReader(trans, CommandType.Text, SpSelectUsuariosFilial, parms);
            else
                dr = DataAccessLayer.ExecuteReader(CommandType.Text, SpSelectUsuariosFilial, parms);

            while (dr.Read())
            {
                var objUsuarioFilialPerfil = new UsuarioFilialPerfil();
                if (SetInstanceNotDipose(dr, objUsuarioFilialPerfil))
                    lista.Add(objUsuarioFilialPerfil);
            }

            if (!dr.IsClosed)
                dr.Close();

            dr.Dispose();

            return lista;
        }
        #endregion

        #region RecuperarIdentificadorPessoaFisicaPrimeiroMaster
        public static int? RecuperarIdentificadorPessoaFisicaPrimeiroMaster(int idFilial)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Filial", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Perfil", SqlDbType.Int, 4));

            parms[0].Value = idFilial;
            parms[1].Value = BNE.BLL.Enumeradores.Perfil.AcessoEmpresaMaster.GetHashCode();

            var res = DataAccessLayer.ExecuteScalar(CommandType.Text, SPPRIMEIROUSUARIOMASTERFILIAL, parms);

            if (Convert.IsDBNull(res) || res == null || res == DBNull.Value)
                return null;

            return Convert.ToInt32(res);
        }
        #endregion

        #region QuantidadeUsuarioResponsavel
        /// <summary>
        /// Retorna um int que informa em quantos usuários são responsáveis pela empresa
        /// </summary>
        /// <returns></returns>
        public static int QuantidadeUsuarioResponsavel(Filial objFilial, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Filial", SqlDbType.Int, 4));
            parms[0].Value = objFilial.IdFilial;

            return Convert.ToInt32(DataAccessLayer.ExecuteScalar(trans, CommandType.Text, Selectquantidadeusuarioresponsavel, parms));
        }
        #endregion

        #region ListarNomeUsuariosFilial
        /// <summary>
        /// Método responsável por retornar uma lista com todos os usuários de uma filial
        /// </summary>
        /// <param name="objFilial">Filial para recuperar os usuários</param>
        /// <returns></returns>
        public static List<string> ListarNomeUsuariosFilial(Filial objFilial, int quantidadeRetorno)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Filial", SqlDbType = SqlDbType.Int, Size = 4, Value = objFilial.IdFilial },
                    new SqlParameter { ParameterName = "@top", SqlDbType = SqlDbType.Int, Size = 4, Value = quantidadeRetorno },
                    new SqlParameter { ParameterName = "@Idf_Perfil", SqlDbType = SqlDbType.Int, Size = 4, Value = (int)Enumeradores.Perfil.AcessoEmpresaMaster }
                };


            var lista = new List<string>();

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spselecttopnomeususarioporperfilativo, parms))
            {
                while (dr.Read())
                {
                    lista.Add(dr["Nme_Pessoa"].ToString());
                }

                if (!dr.IsClosed)
                    dr.Close();
            }

            return lista;
        }
        #endregion

        #region SPInsertDadosOrigemAcesso
        /// <summary>
        /// Gravar dados de origem do acesso do usuario
        /// </summary>
        public static void GravarDadosOrigemAcesso(int idUsuarioFilialPerfil, string urlOrigem, string urlQueryString, string utmSource, string utmMedium, string utmCampaign, string utmTerm, string palavraChave)
        {
            var parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Usuario_Filial_Perfil", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Dta_Acesso", SqlDbType.DateTime, 8));
            parms.Add(new SqlParameter("@Des_UTM_Source", SqlDbType.VarChar, 200));
            parms.Add(new SqlParameter("@Des_UTM_Medium", SqlDbType.VarChar, 200));
            parms.Add(new SqlParameter("@Des_UTM_Campaign", SqlDbType.VarChar, 200));
            parms.Add(new SqlParameter("@Des_UTM_Term", SqlDbType.VarChar, 200));
            parms.Add(new SqlParameter("@Des_Palavra_Chave", SqlDbType.VarChar, 200));

            parms[0].Value = idUsuarioFilialPerfil;
            parms[1].Value = DateTime.Now;

            if (string.IsNullOrEmpty(utmSource) && string.IsNullOrEmpty(urlOrigem))
                parms[2].Value = DBNull.Value;
            else if (string.IsNullOrEmpty(utmSource) && urlOrigem != "")
                parms[2].Value = urlOrigem;
            else
                parms[2].Value = utmSource;

            if (string.IsNullOrEmpty(utmMedium))
                parms[3].Value = DBNull.Value;
            else
                parms[3].Value = utmMedium;

            if (string.IsNullOrEmpty(utmCampaign))
                parms[4].Value = DBNull.Value;
            else
                parms[4].Value = utmCampaign;

            if (string.IsNullOrEmpty(utmTerm))
                parms[5].Value = DBNull.Value;
            else
                parms[5].Value = utmTerm;

            if (string.IsNullOrEmpty(palavraChave))
                parms[6].Value = DBNull.Value;
            else
                parms[6].Value = palavraChave;

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPInsertDadosOrigemAcesso, parms);
        }
        #endregion

        #region Email
        /// <summary>
        /// Caso tenha filial retorno o email comercial do contrario o email da pessoa fisica
        /// </summary>
        /// <returns></returns>
        public string Email()
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Usuario_Filial_Perfil", SqlDbType = SqlDbType.Int, Size = 4, Value = this._idUsuarioFilialPerfil }
                };

            return Convert.ToString(DataAccessLayer.ExecuteScalar(CommandType.Text, Spselectemail, parms));
        }
        #endregion

        #region PossuiPerfilAtivo
        /// <summary>
        /// A pessoa física tem algum perfil ativo?
        /// identificar de um usuario e uma filial
        /// </summary>
        /// <param name="objPessoaFisica">Pessoa Física</param>
        /// <returns>Boolean</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static bool PossuiPerfilAtivo(PessoaFisica objPessoaFisica)
        {
            var parms = new List<SqlParameter>
			    {
			        new SqlParameter { ParameterName = "@Idf_Pessoa_Fisica", SqlDbType = SqlDbType.Int, Size = 4, Value = objPessoaFisica.IdPessoaFisica }
			    };

            return Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, Sppossuiperfilativo, parms)) > 0;
        }
        #endregion

        #region RecuperarPrimeiroPorEmail
        public static UsuarioFilialPerfil RecuperarPrimeiroPorEmail(string email)
        {
            var parms = new List<SqlParameter>
            {
                new SqlParameter {ParameterName = "@Eml_Comercial", SqlDbType = SqlDbType.VarChar, Size = 100, Value = email }
            };
            UsuarioFilialPerfil objUsuarioFilialPerfil = null;
            using (var dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spselectprimeirousuarioperfilporemail, parms))
            {
                if (dr.Read())
                {
                    objUsuarioFilialPerfil = new UsuarioFilialPerfil();
                    SetInstanceNotDipose(dr, objUsuarioFilialPerfil);
                }
            }
            return objUsuarioFilialPerfil;
        }
        #endregion


        #region ValidarPerfil
        /// <summary>
        /// 
        /// </summary>
        /// <param name="idUsuarioFilialPerfil"></param>
        /// <param name="idPerfil"></param>
        /// <returns></returns>
        public static bool ValidarPerfil(int idUsuarioFilialPerfil, int idPerfil)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Usuario_Filial_Perfil", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Perfil", SqlDbType.Int, 4));

            parms[0].Value = idUsuarioFilialPerfil;
            parms[1].Value = idPerfil;

            return (int)DataAccessLayer.ExecuteScalar(CommandType.Text, spValidarPerfil, parms) > 0;
        }
        #endregion

        #region RecuperarNomeUsuario
   /// <summary>
   /// 
   /// </summary>
   /// <param name="idUsuarioFilialPerfil"></param>
   /// <returns></returns>
        public static string RecuperarNomeUsuario(int idUsuarioFilialPerfil)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Usuario_Filial_Perfil", SqlDbType.Int, 4));

            parms[0].Value = idUsuarioFilialPerfil;

            using(IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, spRecuperarNomeUsuario,parms)){

                if (dr.Read())
                    return dr["nme_pessoa"].ToString();
                
            };
            return string.Empty;
        }

        #endregion

        #region [AtualizaNovoResponsavel]
        public static bool AtualizaNovoResponsavel(int IdNovoResponsavel, int IdInativar)
        {
            using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        List<SqlParameter> parm = new List<SqlParameter>
                        {
                            new SqlParameter { ParameterName = "@Idf_Responsavel", SqlDbType = SqlDbType.Int, Value = IdNovoResponsavel },
                            new SqlParameter { ParameterName = "@Idf_Inativar", SqlDbType = SqlDbType.Int, Value = IdInativar }
                        };
                        DataAccessLayer.ExecuteNonQuery(CommandType.Text, spInativaResponsavel, parm);
                        DataAccessLayer.ExecuteNonQuery(CommandType.Text, spAtivaNovoResponsavel, parm);
                        trans.Commit();
                        return true;
                    }
                    catch(Exception ex) {
                        trans.Rollback();
                        EL.GerenciadorException.GravarExcecao(ex, String.Format("Erro ao inativar usuario filial :  e colocar como responsavel o ufp: ", IdInativar, IdNovoResponsavel));
                        throw;
                    }
                }
            }
        }

        #endregion







        #region CarregarUsuarioFilialPerfilPorCPFeFilial
        /// <summary>
        /// Método responsável por carregar uma instancia de Usuario Filial Perfil através do
        /// número de CPF e código de filial.
        /// </summary>
        /// <param name="cpf">Cpf da pessoa física</param>
        /// <param name="idFilial">Identificador de uma Filial</param>
        /// <returns>Boolean</returns>
        /// <remarks>Francisco Ribas</remarks>
        public static bool CarregarUsuarioFilialPerfilPorCPFeFilial(decimal cpf, int idFilial, out UsuarioFilialPerfil objUsuarioFilialPerfil)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Num_CPF", SqlDbType.Decimal, 9));
            parms.Add(new SqlParameter("@Idf_Filial", SqlDbType.Int, 4));

            parms[0].Value = cpf;
            parms[1].Value = idFilial;

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SP_CARREGAR_USUARIO_FILIAL_PERFIL_POR_CPF_E_FILIAL, parms))
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





        #endregion

    }
}