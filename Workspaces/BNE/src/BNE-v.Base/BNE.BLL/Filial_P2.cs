//-- Data: 30/11/2011 11:35
//-- Autor: Jhonatan Taborda

using BNE.BLL.AsyncServices;
using BNE.BLL.Custom;
using BNE.BLL.Custom.Email;
using BNE.BLL.Struct;
using BNE.EL;
using BNE.Services.Base.ProcessosAssincronos;
using Microsoft.SqlServer.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace BNE.BLL
{
    public partial class Filial : ICloneable // Tabela: TAB_Filial
    {

        #region Propriedades

        #region CNPJ
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public string CNPJ
        {
            get
            {
                if (_numeroCNPJ.HasValue)//##.###.###/####-##
                    return _numeroCNPJ.Value.ToString(CultureInfo.CurrentCulture).PadLeft(14, '0').Insert(2, ".").Insert(6, ".").Insert(10, "/").Insert(15, "-");
                return string.Empty;
            }
        }
        #endregion

        #region NomeFantasia
        /// <summary>
        /// Tamanho do campo: 100.
        /// Campo obrigatório.
        /// </summary>
        public string NomeFantasia
        {
            get
            {
                if (String.IsNullOrEmpty(this._nomeFantasia))
                {
                    this._nomeFantasia = this.RecuperarNomeFantasia();
                }
                return this._nomeFantasia;
            }
            set
            {
                this._nomeFantasia = value;
                this._modified = true;
            }
        }
        #endregion

        #endregion

        #region Consultas

        #region SpRecuperarInfoFilialParaCartaCvsPerfil
        private const string SpRecuperarInfoFilialParaCartaCvsPerfil = @"
        select ab.[Des_Area_BNE]
	        , pe.[Des_Porte_Empresa]
	        , ci.Nme_Cidade
	        , ci.Sig_Estado
        from bne.tab_filial fl with(nolock)
	        join bne.tab_endereco en with(nolock) on fl.Idf_Endereco = en.Idf_endereco
	        join plataforma.tab_cidade ci with(nolock) on en.Idf_Cidade = ci.Idf_Cidade
	        left join [plataforma].[TAB_CNAE_Sub_Classe] sc with(nolock) on fl.Idf_CNAE_Principal = sc.Idf_Cnae_Sub_Classe
	        left join [plataforma].[TAB_CNAE_Classe] cc with(nolock) on sc.Idf_Cnae_Classe = cc.Idf_Cnae_Classe
	        left join [plataforma].[TAB_CNAE_Grupo] cg with(nolock) on cc.Idf_Cnae_Grupo = cg.Idf_Cnae_Grupo
	        left join [plataforma].[TAB_CNAE_Divisao] cd with(nolock) on cg.Idf_Cnae_divisao = cd.Idf_Cnae_Divisao
	        left join plataforma.tab_area_bne ab with(nolock) on cd.Idf_Area_BNE = ab.Idf_Area_BNE
	        left join [plataforma].[TAB_Pessoa_Juridica] pj with(nolock) on fl.Num_CNPJ like pj.Num_CNPJ
	        left join [plataforma].[TAB_Porte_Empresa] pe with(nolock) on pj.Idf_Porte_Empresa = pe.Idf_Porte_Empresa
        where fl.Idf_Filial = @Idf_Filial
        ";
        #endregion

        #region SPSELECTCNPJ
        private const string SPSELECTCNPJ = @"  
        SELECT  *
        FROM    TAB_Filial F WITH(NOLOCK)
        WHERE   F.Num_CNPJ = @Num_CNPJ";

        private const string SPLISTTODASCNPJ = @"  
        SELECT  Idf_Filial, Nme_Fantasia
        FROM    TAB_Filial F WITH(NOLOCK)
        WHERE   F.Num_CNPJ LIKE '%'+CONVERT(VARCHAR(9),''+@Num_CNPJ+'')+'%'";
        #endregion

        #region SPSOMAQUANTIDADEVAGASDIVULGADAS
        private const string SPSOMAQUANTIDADEVAGASDIVULGADAS = @"
        SELECT  SUM(Qtd_Vaga) 
        FROM    BNE.BNE_Vaga WITH(NOLOCK)
        WHERE   Idf_Filial = @Idf_Filial 
                AND Flg_Auditada = 1
        ";
        #endregion

        #region SPQUANTIDADEVAGASANUNCIADAS
        private const string SPQUANTIDADEVAGASANUNCIADAS = @"
        SELECT  COUNT(Idf_Vaga)
        FROM    BNE.BNE_Vaga WITH(NOLOCK)
        WHERE   Idf_Filial = @Idf_Filial 
                AND Flg_Vaga_Arquivada = 0
                AND Flg_Inativo = 0
                AND Flg_Vaga_Rapida = 0
                AND (@Idf_Usuario_Filial_Perfil IS NULL OR Idf_Usuario_Filial_Perfil = @Idf_Usuario_Filial_Perfil)";
        #endregion

        #region SPQUANTIDADEVAGASARQUIVADAS
        private const string SPQUANTIDADEVAGASARQUIVADAS = @"
        SELECT  COUNT(Idf_Vaga)
        FROM    BNE.BNE_Vaga WITH(NOLOCK)
        WHERE   Idf_Filial = @Idf_Filial 
                AND Flg_Vaga_Arquivada = 1
                AND Flg_Inativo = 0
                AND Flg_Vaga_Rapida = 0
                AND (@Idf_Usuario_Filial_Perfil IS NULL OR Idf_Usuario_Filial_Perfil = @Idf_Usuario_Filial_Perfil)";
        #endregion

        #region SPCOUNTCVSVISUALIZADOS
        private const string SPCOUNTCVSVISUALIZADOS = @"
        SELECT  COUNT(Idf_Curriculo_Visualizacao) 
        FROM    BNE.BNE_Curriculo_Visualizacao WITH(NOLOCK)
        WHERE Idf_Filial = @Idf_Filial";
        #endregion

        #region SPCOUNTEMPRESASCADASTRADAS

        private const string SPCOUNTEMPRESASCADASTRADAS = @"
        SELECT  COUNT(F.Idf_Filial)
        FROM    TAB_Filial F WITH(NOLOCK)
        WHERE   CONVERT(VARCHAR, Dta_Cadastro, 103) = CONVERT(VARCHAR, GETDATE(), 103)
                AND Flg_Inativo=0 
                AND Idf_Situacao_Filial = 1";
        #endregion

        #region SPSELECTFILIAISRAZAORAMOCIDADE
        private const string SPSELECTFILIAISRAZAORAMOCIDADE = @"   
        DECLARE @FirstRec INT
        DECLARE @LastRec INT
        DECLARE @iSelect VARCHAR(8000)
        DECLARE @iSelectCount VARCHAR(8000)
        DECLARE @iSelectPag VARCHAR(8000)
        SET @FirstRec = ( @CurrentPage - 1 ) * @PageSize
        SET @LastRec = ( @CurrentPage * @PageSize + 1 )
        SET @iSelect = '
                    SELECT ROW_NUMBER() OVER (ORDER BY F.Dta_Cadastro DESC) AS RowID,
                    F.Idf_Filial ,
                    Raz_Social AS Nme_Empresa ,
                    Qtd_Funcionarios AS Num_Funcionario ,
                    DES_CNAE_SUB_Classe AS RamoDaAtividade ,
                    Nme_Cidade ,
                    AB.Des_Area_BNE,
                    Sig_Estado '
        
        IF (@Idf_Curriculo IS NOT NULL)
            BEGIN
                SET @iSelect = @iSelect + ' ,   (
					                                CASE (SELECT COUNT(1) FROM BNE.BNE_Intencao_Filial InF WITH(NOLOCK) WHERE Inf.Idf_Filial = F.Idf_Filial AND Inf.Idf_Curriculo = ' + Convert(VARCHAR, @Idf_Curriculo) +') 
						                                WHEN 0 THEN 0
						                                ELSE 1
					                                END 
				                                ) AS Flg_Candidatou '
            END
                    
                   
        SET @iSelect = @iSelect + 
           'FROM    TAB_Filial F WITH(NOLOCK)
                LEFT JOIN plataforma.TAB_CNAE_Sub_Classe CSC WITH(NOLOCK) ON CSC.Idf_CNAE_Sub_Classe = F.Idf_CNAE_Principal
                INNER JOIN TAB_Endereco E WITH(NOLOCK) ON F.Idf_Endereco = E.Idf_Endereco
                INNER JOIN plataforma.TAB_Cidade Cid WITH(NOLOCK) ON E.Idf_Cidade = Cid.Idf_Cidade
                LEFT JOIN plataforma.TAB_CNAE_Classe CC WITH(NOLOCK) ON CC.Idf_CNAE_Classe=CSC.Idf_CNAE_Classe
                LEFT JOIN plataforma.TAB_CNAE_Grupo CG WITH(NOLOCK) ON CG.Idf_CNAE_Grupo=CC.Idf_CNAE_Grupo
                LEFT JOIN plataforma.TAB_CNAE_Divisao CD WITH(NOLOCK) ON CD.IDF_CNAE_DIVISAO=CG.IDF_CNAE_DIVISAO
                LEFT JOIN plataforma.TAB_Area_BNE AB WITH(NOLOCK) ON CD.IDF_AREA_BNE=AB.Idf_Area_BNE
            WHERE   F.Flg_Inativo = 0
                    AND F.Idf_Situacao_Filial = 1'
                                                                        
            IF (@RazaoRamoCidade IS NOT NULL )
                SET @iSelect = @iSelect + ' AND ( F.Raz_Social LIKE ''%' + @RazaoRamoCidade + '%'' OR DES_CNAE_SUB_Classe LIKE ''%' + @RazaoRamoCidade + '%'' OR Cid.Nme_Cidade LIKE ''%' + @RazaoRamoCidade + '%'')'
                                                                  
        SET @iSelectCount = 'Select Count(*) From ( ' + @iSelect  + ' ) As TblTempCount'
        SET @iSelectPag = 'Select * From ( ' + @iSelect + ' ) As TblTempPag	Where RowID > ' + CONVERT(VARCHAR, @FirstRec)  + ' And RowID < ' + CONVERT(VARCHAR, @LastRec)
        EXECUTE (@iSelectCount)
        EXECUTE (@iSelectPag)";
        #endregion

        #region SPFILIAISDESATUALIZADAS
        private const string SPFILIAISDESATUALIZADAS = @"
        SELECT PF.Eml_Pessoa, UFP.Idf_Usuario_Filial_Perfil
        FROM BNE.TAB_Filial F WITH(NOLOCK)
            INNER JOIN BNE.TAB_Usuario_Filial_Perfil UFP WITH(NOLOCK) ON UFP.Idf_Filial = F.Idf_Filial
            INNER JOIN BNE.TAB_Pessoa_Fisica PF WITH(NOLOCK) ON UFP.Idf_Pessoa_Fisica = PF.Idf_Pessoa_Fisica 
        WHERE
            F.Flg_Inativo = 0
            AND UFP.Flg_Inativo = 0
            AND (DATEDIFF(DAY,F.Dta_Alteracao,GETDATE()) = (SELECT CONVERT(INT, Vlr_Parametro) FROM plataforma.TAB_Parametro WHERE Idf_Parametro = 13 ))
            AND PF.Eml_Pessoa IS NOT NULL
            AND UFP.Idf_Perfil = 4 /* Empresa Master */
            AND (	
		        F.Idf_Situacao_Filial <> 5 /* Bloqueado */
		        AND
		        F.Idf_Situacao_Filial <> 6 /* Cancelado */
		        )";
        #endregion

        #region SPSELECTFILIALEMPLOYERAUTOCOMPLETE

        private const string SPSELECTFILIALEMPLOYERAUTOCOMPLETE =
        @"
        SELECT  TOP(@Count) GF.Ape_Filial 
        FROM    BNE_Gerente_Filial GF WITH(NOLOCK)
        WHERE   GF.Ape_Filial LIKE @Ape_Filial + '%'
        ORDER BY GF.Ape_Filial
        ";

        #endregion

        #region SPSELECTEMAILDESTINATARIOFILIALEMPLOYER
        /// <summary>
        ///Criado por Charan 15/10
        ///Retorna DataTable com os dados de contato da Filial.
        /// </summary>
        private const string SPSELECTEMAILDESTINATARIOFILIALEMPLOYER =
        @"
        SELECT gc.Idf_Grupo_Cidade,ed.Des_Email,ed.Nme_Pessoa FROM BNE.BNE_Grupo_Cidade AS gc
        INNER JOIN BNE.BNE_Email_Destinatario_Cidade AS edc ON edc.Idf_Grupo_Cidade = gc.Idf_Grupo_Cidade
        INNER JOIN BNE.BNE_Email_Destinatario AS ed ON ed.Idf_Email_Destinatario = edc.Idf_Email_Destinatario
        WHERE gc.Flg_Inativo=0
        GROUP BY GC.Idf_Grupo_Cidade, ED.Des_Email,ED.Nme_Pessoa
        ";

        #endregion

        #region SPSELECTCIDADEGRUPOFILIALEMPLOYER
        /// <summary>
        ///Criado por Charan 15/10
        ///Retorna DataTable com as cidades de um Grupo_Cidade.
        /// </summary>
        private const string SPSELECTCIDADEGRUPOFILIALEMPLOYER =
        @"
        SELECT lc.Idf_Cidade FROM BNE.BNE_Lista_Cidade AS lc
        LEFT JOIN plataforma.TAB_Cidade AS c ON c.Idf_Cidade = lc.Idf_Cidade
        WHERE lc.Idf_Grupo_Cidade=@Idf_Grupo_Cidade
        ";

        #endregion

        #region SPSELECTRELATORIOMENSALFILIALEMPLOYER
        /// <summary>
        ///Criado por Charan 15/10
        ///Retorna DataTable com os dados de contato da Filial.
        /// </summary>
        private const string SPSELECTRELATORIOMENSALFILIALEMPLOYER =
        @"
        SELECT COUNT(*) AS TotalCurriculosCadastrados, 
(SELECT COUNT(*) AS TotalAtualizados FROM BNE.BNE_Curriculo AS c WITH (NOLOCK)
		WHERE c.Idf_Cidade_Endereco IN ({0}) AND c.Dta_Atualizacao BETWEEN @Data_Inicial AND @Data_Fim) AS TotalCurriculosAtualizados,
		(SELECT COUNT(*) AS VipNovos FROM BNE.BNE_Curriculo AS c WITH (NOLOCK)
		WHERE c.Idf_Cidade_Endereco IN ({0}) AND
		c.Flg_VIP =1 AND 
		c.Dta_Cadastro BETWEEN @Data_Inicial AND @Data_Fim) AS VIPNovosMes,
		(SELECT COUNT(*) AS VipAtivos FROM BNE.BNE_Curriculo AS c WITH (NOLOCK)
		WHERE c.Idf_Cidade_Endereco IN ({0}) AND
		c.Flg_VIP =1) AS VipAtivos,
		(SELECT COUNT(*) FROM BNE.BNE_Vaga AS v WITH(NOLOCK)
		WHERE v.Idf_Cidade IN ({0}) AND 
		Dta_Cadastro BETWEEN @Data_Inicial AND @Data_Fim) AS TotalNovasVagas,
		(SELECT COUNT(*) FROM BNE.TAB_Filial AS F WITH(NOLOCK)
			JOIN BNE.TAB_Endereco AS FEF ON FEF.Idf_Endereco = F.Idf_Endereco
			WHERE FeF.Idf_Cidade IN ({0}) AND
			f.Dta_Cadastro BETWEEN @Data_Inicial AND @Data_Fim) AS TotalEmpresasCadastradas,

			(SELECT COUNT(*) FROM BNE.TAB_Filial AS F WITH(NOLOCK)
	JOIN BNE.TAB_Endereco AS FEnd ON FEnd.Idf_Endereco = F.Idf_Endereco
	JOIN BNE.BNE_Plano_Adquirido AS PA ON pa.Idf_Filial = f.Idf_Filial
	WHERE  FEnd.Idf_Cidade IN({0}) AND
	f.Dta_Cadastro BETWEEN @Data_Inicial AND @Data_Fim AND
	pa.Idf_Filial IS NOT NULL AND
	pa.Dta_Fim_Plano > @Data_Fim AND 
	pa.Dta_Cadastro BETWEEN @Data_Inicial AND @Data_Fim) AS TotalEmpresasPlanoNovo,

	(SELECT COUNT(*) FROM BNE.TAB_Filial AS F WITH(NOLOCK)
	JOIN BNE.TAB_Endereco AS FEnd ON FEnd.Idf_Endereco = F.Idf_Endereco
	JOIN BNE.BNE_Plano_Adquirido AS PA ON pa.Idf_Filial = f.Idf_Filial
	WHERE  FEnd.Idf_Cidade IN({0}) AND
	pa.Dta_Fim_Plano > @Data_Fim) AS TotalEmpresasPlanosAtivos

	 FROM BNE.BNE_Curriculo AS c WITH (NOLOCK)
		WHERE  c.Idf_Cidade_Endereco IN ({0}) 
		AND c.Dta_Cadastro BETWEEN @Data_Inicial AND @Data_Fim

        ";

        #endregion

        #region SPSELECTFILIALPORNOMEFANTASIA

        private const string SPSELECTFILIALPORNOMEFANTASIA =
        @"
        SELECT TOP(@count) Nme_Fantasia, Idf_Filial FROM (
        SELECT  NME_FANTASIA 
        FROM    TAB_FILIAL WITH(NOLOCK)
        WHERE   NME_FANTASIA LIKE ''+@Nme_Fantasia+'%'
        GROUP BY NME_FANTASIA
        ) FIL
        CROSS APPLY(
	        SELECT TOP(1) TF2.IDF_FILIAL FROM TAB_FILIAL TF2 WITH(NOLOCK) WHERE TF2.NME_FANTASIA = FIL.NME_FANTASIA 
        ) TFC
		ORDER BY FIL.Nme_Fantasia
        ";

        #endregion

        #region SPSELECTFILIALEMPLOYERPORAPELIDO
        private const string SPSELECTFILIALEMPLOYERPORAPELIDO =
        @"
        SELECT  F.*
        FROM    BNE_Gerente_Filial GF WITH(NOLOCK)
                INNER JOIN TAB_Filial F WITH(NOLOCK) ON GF.Idf_Filial = F.Idf_Filial
        WHERE   GF.Ape_Filial LIKE @Ape_Filial + '%'
        ";
        #endregion

        #region TAB_SP_BuscaEmpresasData
        private const string TAB_SP_BuscaEmpresasData = @"TAB_SP_BuscaEmpresasData";
        #endregion

        #region SpQuantidadeUsuariosAtivos
        private const string SpQuantidadeUsuariosAtivos = @"
        SELECT  COUNT(UFP.Idf_Usuario_Filial_Perfil)
        FROM    TAB_Usuario_Filial_Perfil UFP WITH(NOLOCK)
        WHERE  
                UFP.Flg_Inativo = 0 AND
                UFP.Idf_Filial = @Idf_Filial
        ";
        #endregion

        #region SpQuantidadeAcessosAdquiridos
        private const string SpQuantidadeAcessosAdquiridos = @"
        SELECT  Qtd_Usuario_Adicional
        FROM    TAB_Filial F WITH(NOLOCK)
        WHERE   F.Idf_Filial = @Idf_Filial
        ";
        #endregion

        #region Spexistefilial
        private const string Spexistefilial = @"  
        SELECT  count(1)
        FROM    TAB_Filial F WITH(NOLOCK)
        WHERE   F.Num_CNPJ = @Num_CNPJ";
        #endregion

        #region SpRecuperarTipoParceiro
        private const string SpRecuperarTipoParceiro = @"  
        SELECT  F.Idf_Tipo_Parceiro
        FROM    TAB_Filial F WITH(NOLOCK)
        WHERE   F.Idf_Filial = @Idf_Filial";
        #endregion

        #region Sprecuperaridentificador
        private const string Sprecuperaridentificador = @"  
        SELECT  Idf_Filial
        FROM    TAB_Filial F WITH(NOLOCK)
        WHERE   F.Num_CNPJ = @Num_CNPJ";
        #endregion

        #region Sprecuperarsituacaofilial
        private const string Sprecuperarsituacaofilial = @"
        SELECT  SF.* 
        FROM    TAB_Filial F WITH(NOLOCK)
                INNER JOIN TAB_Situacao_Filial SF WITH(NOLOCK) ON SF.Idf_Situacao_Filial = F.Idf_Situacao_Filial   
        WHERE   F.Idf_Filial = @Idf_Filial
        ";
        #endregion

        #region Spselectsalaadministrador
        public const String Spselectsalaadministrador = @"
        DECLARE @FirstRec INT
        DECLARE @LastRec INT
        SET @FirstRec = @CurrentPage * @PageSize + 1
        SET @LastRec = @CurrentPage * @PageSize + @PageSize
 
        DECLARE @iSelect NVARCHAR(MAX)        
        DECLARE @iSelectCount NVARCHAR(MAX)          
        DECLARE @iSelectPag NVARCHAR(MAX)            
 
        DECLARE @ParmDefinition NVARCHAR(1000)
        SET @ParmDefinition = N'@Filtro VARCHAR(100), @Idf_Situacao INT, @Num_CNPJ DECIMAL, @Telefone VARCHAR(12), @Email VARCHAR(40)';
      
        SET @iSelect = '
        SELECT      ROW_NUMBER() OVER (ORDER BY Dta_Cadastro DESC) AS RowID,
               Idf_Filial, Raz_Social, Num_CNPJ, Dta_Ultima_pesquisa, Data_Validade_Plano, Idf_Situacao_Filial, Dta_Cadastro, CidadeUF
        FROM (
                SELECT F.Idf_Filial ,
                     F.Raz_Social ,
                     F.Num_CNPJ ,
                     [BNE].[Busca_Ultimo_Plano_Filial] ( f.idf_filial ) AS Data_Validade_Plano ,
                     F.Idf_Situacao_Filial ,
                     F.Dta_Cadastro,
                     ff.Nme_Cidade + ''/'' + ff.Sig_Estado AS CidadeUF,
                           [BNE].[Busca_Ultima_Pesquisa_Filial] ( f.idf_filial ) AS Dta_Ultima_Pesquisa
        FROM BNE.Tab_Filial F WITH(NOLOCK)
                           LEFT JOIN BNE.TAB_Usuario_Filial_Perfil ufp WITH(NOLOCK)  ON F.Idf_Filial = ufp.Idf_Filial                       
                     LEFT JOIN BNE.bne_usuario_filial uf WITH(NOLOCK) ON uf.idf_Usuario_Filial_Perfil = ufp.idf_Usuario_Filial_Perfil
                           JOIN BNE.TAB_Filial_Fulltext ff WITH ( NOLOCK ) ON F.Idf_Filial = ff.Idf_Filial                     
                     JOIN BNE.TAB_Endereco E WITH(NOLOCK) ON ( E.Idf_Endereco = F.Idf_Endereco )
                     JOIN plataforma.TAB_Cidade cid WITH(NOLOCK) ON ( cid.Idf_Cidade = E.Idf_Cidade )           
        WHERE F.Flg_Inativo = 0 '
 
        IF ( @Idf_Situacao IS NOT NULL )
            BEGIN
                SET @iSelect = @iSelect
                    + ' AND f.Idf_Situacao_Filial = @Idf_Situacao '
            END
 
        IF ( @Num_CNPJ IS NOT NULL )
            BEGIN
                SET @iSelect = @iSelect + ' AND F.Num_CNPJ = @Num_CNPJ '
            END
      
        IF ( @Telefone IS NOT NULL )
            BEGIN
                SET @iSelect = @iSelect
                    + ' AND F.Num_Comercial LIKE @Telefone'
            END
 
        IF ( @Email IS NOT NULL )
            BEGIN
                SET @iSelect = @iSelect + ' AND uf.Eml_comercial LIKE @Email'
            END    
      
        IF ( @Filtro IS NOT NULL )
            BEGIN
                SET @iSelect = @iSelect + ' AND CONTAINS(ff.Des_MetaBusca, '''
                    + [BNE].BNE_BuscaMontaFT(@Filtro) + ''')'
            END    
 
        SET @iSelect = @iSelect + ' group by F.Idf_Filial ,
                     F.Raz_Social ,
                           Num_CNPJ,
                     F.Idf_Situacao_Filial ,
                     F.Dta_Cadastro,
                     ff.Nme_Cidade,
                           ff.Sig_Estado ) AS temp'
 
 
        SET @iSelectCount = 'Select Count(*) From ( ' + @iSelect
            + ' ) As TblTempCount'
        SET @iSelectPag = 'Select * From ( ' + @iSelect
            + ' ) As TblTempPag   Where RowID >= ' + CONVERT(VARCHAR, @FirstRec)
            + ' And RowID <= ' + CONVERT(VARCHAR, @LastRec)
      
        EXEC sp_executesql @iSelectCount, @ParmDefinition, @Filtro = @Filtro,
            @Idf_Situacao = @Idf_Situacao, @Num_CNPJ = @Num_CNPJ,
            @Telefone = @Telefone, @Email = @Email
 
        EXEC sp_executesql @iSelectPag, @ParmDefinition, @Filtro = @Filtro,
            @Idf_Situacao = @Idf_Situacao, @Num_CNPJ = @Num_CNPJ,
            @Telefone = @Telefone, @Email = @Email";

        #endregion

        #region Spquantidadesemlocalizacao
        private const string Spquantidadesemlocalizacao = @"
        SELECT  COUNT(*) 
        FROM    TAB_Filial F WITH(NOLOCK) 
                INNER JOIN TAB_Endereco E WITH(NOLOCK) ON F.Idf_Endereco = E.Idf_Endereco
                INNER JOIN plataforma.TAB_Cidade Cid WITH(NOLOCK) ON Cid.Idf_Cidade = E.Idf_Cidade
        WHERE   F.Des_Localizacao IS NULL
                AND F.Flg_Inativo = 0
                AND (	
		            F.Idf_Situacao_Filial <> 5 /* Bloqueado */
		            AND
		            F.Idf_Situacao_Filial <> 6 /* Cancelado */
		            )";
        #endregion

        #region Sprecuperarsemlocalizacao
        private const string Sprecuperarsemlocalizacao = @"
        SELECT  TOP(@Limite) F.Idf_Filial, E.Num_CEP, E.Des_Logradouro, E.Num_Endereco, Cid.Nme_Cidade, Cid.Sig_Estado
        FROM    TAB_Filial F WITH(NOLOCK) 
                INNER JOIN TAB_Endereco E WITH(NOLOCK) ON F.Idf_Endereco = E.Idf_Endereco
                INNER JOIN plataforma.TAB_Cidade Cid WITH(NOLOCK) ON Cid.Idf_Cidade = E.Idf_Cidade
        WHERE   F.Des_Localizacao IS NULL
                AND F.Flg_Inativo = 0
                AND (	
		            F.Idf_Situacao_Filial <> 5 /* Bloqueado */
		            AND
		            F.Idf_Situacao_Filial <> 6 /* Cancelado */
		            )";
        #endregion

        #region Spatualizarlocalizacao
        private const string Spatualizarlocalizacao = "UPDATE TAB_Filial SET Des_Localizacao = @Des_Localizacao WHERE Idf_Filial = @Idf_Filial";
        #endregion

        #region Sprecuperarcnpj
        private const string Sprecuperarcnpj = "SELECT Num_CNPJ FROM TAB_Filial WHERE Idf_Filial = @Idf_Filial";
        #endregion

        #region SpquantidadeSMSenviadocandidatohoje
        private const string SpquantidadeSMSenviadocandidatohoje = @"
        SELECT	COUNT(*)
        FROM	BNE_Mensagem_CS M WITH(NOLOCK)
		        INNER JOIN TAB_Usuario_Filial_Perfil UFP WITH(NOLOCK) ON M.Idf_Usuario_Filial_Perfil = UFP.Idf_Usuario_Filial_Perfil AND UFP.Flg_Inativo = 0
        WHERE	Idf_Curriculo IS NOT NULL 
		        AND Idf_Tipo_Mensagem_CS = 1 /* SMS */
		        AND UFP.Idf_Filial = @Idf_Filial
		        AND M.Dta_Cadastro BETWEEN CAST(GETDATE() AS DATE) AND DATEADD(DAY, 1, CAST(GETDATE() AS DATE)) 
        ";
        #endregion

        #region Sprecuperardatacadastro
        private const string Sprecuperardatacadastro = "SELECT Dta_Cadastro FROM TAB_Filial WHERE Idf_Filial = @Idf_Filial";
        #endregion

        #region SP_SELECT_NOME_FANTASIA
        private const string SP_SELECT_NOME_FANTASIA = @"  
        SELECT  Nme_Fantasia
        FROM    TAB_Filial F WITH(NOLOCK)
        WHERE Idf_Filial = @Idf_Filial";

        #endregion

        #region SpConteudoFilialContratoDigitalPorUsuarioFilialPerfil
        private const string SpConteudoFilialContratoDigitalPorUsuarioFilialPerfil = @"
        select fl.Raz_Social
	        , fl.Num_CNPJ
	        , en.Des_Logradouro
	        , en.Num_Endereco
	        , ci.Nme_Cidade
	        , ci.Sig_Estado
	        , en.Num_CEP
        from bne.tab_filial fl with (nolock)
			join bne.tab_usuario_filial_perfil ufp with (nolock) on fl.Idf_Filial = ufp.Idf_Filial
	        join bne.tab_endereco en with (nolock) on fl.Idf_Endereco = en.Idf_Endereco
	        join plataforma.tab_cidade ci with (nolock) on en.Idf_Cidade = ci.Idf_Cidade
        where ufp.Idf_Usuario_Filial_Perfil = @Idf_Usuario_Filial_Perfil
        ";
        #endregion

        #region SpConteudoFilialContratoDigitalPorFilial
        private const string SpConteudoFilialContratoDigitalPorFilial = @"
        select fl.Raz_Social
	        , fl.Num_CNPJ
	        , en.Des_Logradouro
	        , en.Num_Endereco
	        , ci.Nme_Cidade
	        , ci.Sig_Estado
	        , en.Num_CEP
        from bne.tab_filial fl with (nolock)
	        join bne.tab_endereco en with (nolock) on fl.Idf_Endereco = en.Idf_Endereco
	        join plataforma.tab_cidade ci with (nolock) on en.Idf_Cidade = ci.Idf_Cidade
        where fl.Idf_Filial = @Idf_Filial
        ";
        #endregion

        #region SpRecuperarInformacoesIntegracaoFinanceiro
        private const string SpRecuperarInformacoesIntegracaoFinanceiro = @"
        select 
	        pp.Dta_Pagamento
	        , pg.Vlr_Pagamento
	        , pg.Des_identificador
	        , pa.Dta_Inicio_Plano
	        , pa.Dta_Fim_Plano
	        , fl_gestora.Nme_Fantasia as Filial_Gestora
	        , fl.Num_CNPJ
	        , fl.Raz_Social
	        , en.Num_CEP
	        , en.Des_Logradouro
	        , en.Num_Endereco
	        , en.Des_Complemento
	        , en.Des_Bairro
	        , ci.Nme_Cidade
	        , ci.Sig_Estado
	        , cnae.Cod_CNAE_Sub_Classe
	        , fl.Num_DDD_Comercial
	        , fl.Num_Comercial
            , fl.Nme_Fantasia
	        , pad.Nme_Res_Plano_Adquirido
	        , pad.Eml_Envio_Boleto
	        , trans_num_banco.Idf_Banco
        from bne.BNE_Pagamento pg with(nolock)
	        outer apply (
		        SELECT TOP 1 trans.Idf_Banco FROM BNE.BNE_Transacao trans with(nolock)
		        WHERE trans.Idf_Pagamento = pg.Idf_Pagamento
		        ORDER BY trans.Dta_Cadastro) trans_num_banco
	        join bne.BNE_Plano_Parcela pp with(nolock) on pg.Idf_Plano_Parcela = pp.Idf_Plano_Parcela
	        join bne.bne_plano_adquirido pa with(nolock) on pp.idf_Plano_Adquirido = pa.Idf_Plano_Adquirido
	        left join bne.bne_plano_adquirido_detalhes pad with(nolock) on pad.idf_Plano_Adquirido = pa.Idf_Plano_Adquirido
	        join bne.tab_filial fl with(nolock) on pa.Idf_Filial = fl.Idf_Filial
	        left join bne.tab_filial fl_gestora with(nolock) on pad.Idf_Filial_Gestora = fl_gestora.Idf_Filial
            left join plataforma.tab_cnae_sub_classe cnae with(nolock) on fl.Idf_Cnae_principal = cnae.Idf_Cnae_Sub_classe
	        join bne.TAB_Endereco en with(nolock) on fl.Idf_Endereco = en.Idf_Endereco
	        join plataforma.TAB_Cidade ci with(nolock) on en.Idf_Cidade = ci.Idf_Cidade
        where pg.Idf_Pagamento = @Idf_Pagamento;";
        #endregion

        #region SpRecuperarInfoCelularUsuariosFilial
        private const string SpRecuperarInfoCelularUsuariosFilial = @"
        select pf.Nme_Pessoa 
	        , pf.Num_DDD_Celular
	        , pf.Num_Celular
        from bne.tab_filial fl with(nolock)
	        join bne.tab_usuario_filial_perfil ufp with(nolock) on fl.Idf_Filial = ufp.Idf_Filial
	        join bne.tab_pessoa_fisica pf with(nolock) on ufp.Idf_Pessoa_Fisica = pf.Idf_Pessoa_Fisica
        where fl.Idf_Filial = @Idf_Filial
        ";
        #endregion

        #region SpVerificaAuditoria
        private const string SpVerificaAuditoria = @"
        select count(fo.Idf_Filial_Observacao)
        from bne.tab_filial_observacao fo with(nolock)
        where fo.Des_Observacao like '#Auditoria'
	        and fo.Idf_Filial = @Idf_Filial
            and fo.Dta_cadastro < getdate()
        ";
        #endregion

        #region SpRecuperarInfoISS
        private const string SpRecuperarInfoISS = @"
        select pfl1.Vlr_Parametro as 'flgISS'
	        , pfl2.Vlr_Parametro as 'textoNF'
            , en.Idf_Cidade
        from bne.tab_filial fl with(nolock)
            join bne.tab_endereco en with(nolock) on fl.idf_endereco = en.Idf_endereco
	        left join bne.tab_parametro_filial pfl1 with(nolock) on fl.Idf_filial = pfl1.Idf_Filial
		        and pfl1.Idf_Parametro = 347
	        left join bne.tab_parametro_filial pfl2 with(nolock) on fl.Idf_filial = pfl2.Idf_Filial
		        and pfl2.Idf_Parametro = 348
        where fl.Idf_Filial = @Idf_Filial
        ";
        #endregion

        #region Spselectfilialemployergerente
        private const string Spselectfilialemployergerente =
        @"
        SELECT  GF.*
        FROM    BNE_Gerente_Filial GF WITH(NOLOCK)
        WHERE   GF.Idf_Filial = @Idf_Filial
        ";
        #endregion

        #region Spfiliaisenviaremailfiliaisemployer
        private const string Spfiliaisenviaremailfiliaisemployer = @"
        SELECT  F.*
        FROM    BNE.TAB_Filial F WITH(NOLOCK)
        WHERE
            F.Flg_Inativo = 0
            AND DATEDIFF(DAY,F.Dta_Cadastro,GETDATE()) = @QuantidadeDias
            AND (	
		        F.Idf_Situacao_Filial <> 5 /* Bloqueado */
		        AND
		        F.Idf_Situacao_Filial <> 6 /* Cancelado */
		        )";
        #endregion

        #region SP_LISTAR_FILIAIS_EMPLOYER
        private const string SP_LISTAR_FILIAIS_EMPLOYER = @"
        SELECT	Idf_Filial AS Idf_Filial, 
		Nme_Fantasia + ' (' + CAST(Num_CNPJ as varchar) +')' AS Des_Filial
          FROM BNE.TAB_Filial
          WHERE (Nme_Fantasia LIKE '%Employer%' OR Num_CNPJ like '82344425%')
          AND Flg_Inativo = 0
          AND Idf_Situacao_Filial NOT IN (5,6)
		  ORDER BY 1";
        #endregion

        #endregion

        #region Metodos

        #region RecuperarInfoISS
        public static bool RecuperarInfoISSFlgIss(int idFilial, out bool flgIss, out string textoPersonalizadoNF, out int idCidade)
        {
            flgIss = false;
            textoPersonalizadoNF = string.Empty;
            bool retorno = false;
            idCidade = 0;

            var parms = new List<SqlParameter> 
                {
                    new SqlParameter{ ParameterName = "@Idf_Filial", SqlDbType = SqlDbType.Int, Value = idFilial}
                };

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SpRecuperarInfoISS, parms))
            {
                if (dr.Read())
                {
                    if (dr["flgISS"] != DBNull.Value)
                        flgIss = Convert.ToInt32(dr["flgISS"]) == 1 ? true : false;
                    if (dr["textoNF"] != DBNull.Value)
                        textoPersonalizadoNF = dr["textoNF"].ToString();
                    idCidade = Convert.ToInt32(dr["Idf_Cidade"]);

                    if (dr["flgISS"] != DBNull.Value)
                        retorno = true;
                }
            };

            return retorno;

        }

        public static bool RecuperarInfoISSTextoPersonalizado(int idFilial, out bool flgIss, out string textoPersonalizadoNF, out int idCidade)
        {
            flgIss = false;
            textoPersonalizadoNF = string.Empty;
            bool retorno = false;
            idCidade = 0;

            var parms = new List<SqlParameter> 
                {
                    new SqlParameter{ ParameterName = "@Idf_Filial", SqlDbType = SqlDbType.Int, Value = idFilial}
                };

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SpRecuperarInfoISS, parms))
            {
                if (dr.Read())
                {
                    if (dr["flgISS"] != DBNull.Value)
                        flgIss = Convert.ToInt32(dr["flgISS"]) == 1 ? true : false;
                    if (dr["textoNF"] != DBNull.Value)
                        textoPersonalizadoNF = dr["textoNF"].ToString();
                    idCidade = Convert.ToInt32(dr["Idf_Cidade"]);

                    if (!string.IsNullOrEmpty(textoPersonalizadoNF))
                        retorno = true;
                }
            };

            return retorno;
        }

        public static bool RecuperarInfoISS(int idFilial, out bool flgIss, out string textoPersonalizadoNF, out int idCidade, SqlTransaction trans = null)
        {
            flgIss = false;
            textoPersonalizadoNF = string.Empty;
            bool retorno = false;
            idCidade = 0;

            var parms = new List<SqlParameter> 
                {
                    new SqlParameter{ ParameterName = "@Idf_Filial", SqlDbType = SqlDbType.Int, Value = idFilial}
                };

            IDataReader dr = trans == null ? DataAccessLayer.ExecuteReader(CommandType.Text, SpRecuperarInfoISS, parms) : DataAccessLayer.ExecuteReader(trans, CommandType.Text, SpRecuperarInfoISS, parms);

            if (dr.Read())
            {
                if (dr["flgISS"] != DBNull.Value)
                    flgIss = Convert.ToInt32(dr["flgISS"]) == 1 ? true : false;
                if (dr["textoNF"] != DBNull.Value)
                    textoPersonalizadoNF = dr["textoNF"].ToString();
                idCidade = Convert.ToInt32(dr["Idf_Cidade"]);

                if (flgIss || !string.IsNullOrEmpty(textoPersonalizadoNF))
                    retorno = true;
            }

            if (!dr.IsClosed)
            {
                dr.Close();
            }
            dr.Dispose();

            return retorno;
        }
        #endregion

        #region Clone
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        #endregion

        #region SalvarDadosEmpresa
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objUsuarioFilialPerfil"></param>
        /// <param name="objUsuarioFilial"></param>
        /// <param name="objPessoaFisica"></param>
        /// <param name="objOrigem"></param>
        /// <param name="listaAlteracao">Lista de alterações para serem salvas em uma Filial Observação</param>
        /// <param name="idUsuarioFilialPerfilLogado">Id do Usuário Filial Perfil que realizou as alterações no cadastro </param>
        /// <param name="empresaAuditada">Booleano para representar se a empresa foi auditada</param>
        /// <param name="listaVagasParaPublicacao"></param>
        public void SalvarDadosEmpresa(UsuarioFilialPerfil objUsuarioFilialPerfil, UsuarioFilial objUsuarioFilial, PessoaFisica objPessoaFisica, Origem objOrigem, List<CompareObject.CompareResult> listaAlteracao, int? idUsuarioFilialPerfilLogado, bool empresaAuditada, bool autorizacaoPublicacaoVagas, bool contrataEstag, bool? usaWebEstagios, FilialLogo objFilialLogo, string cartaApresentacao, string cartaAgradecimentoCandidatura, out List<Vaga> listaVagasParaPublicacao)
        {
            listaVagasParaPublicacao = new List<Vaga>();

            using (SqlConnection conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();

                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        bool novo = !this._persisted;

                        this.Endereco.Save(trans);
                        this.Save(trans);

                        //Foto
                        objFilialLogo.Filial = this;
                        objFilialLogo.Save(trans);

                        ParametroFilial autorizoPublicarParamFilial;
                        if (!ParametroFilial.CarregarParametroPorFilial(Enumeradores.Parametro.AutorizoBNEPublicarVagas, this, out autorizoPublicarParamFilial, trans))
                            autorizoPublicarParamFilial = new ParametroFilial { IdFilial = this.IdFilial, IdParametro = (int)Enumeradores.Parametro.AutorizoBNEPublicarVagas };

                        autorizoPublicarParamFilial.ValorParametro = autorizacaoPublicacaoVagas.ToString();
                        autorizoPublicarParamFilial.Save(trans);

                        ParametroFilial contrataEstagParamFilial;
                        if (!ParametroFilial.CarregarParametroPorFilial(Enumeradores.Parametro.FilialContrataEstagiario, this, out contrataEstagParamFilial, trans))
                            contrataEstagParamFilial = new ParametroFilial { IdFilial = this.IdFilial, IdParametro = (int)Enumeradores.Parametro.FilialContrataEstagiario };

                        contrataEstagParamFilial.ValorParametro = contrataEstag.ToString();

                        contrataEstagParamFilial.Save(trans);

      


                        #region CartaApresentacao
                        ParametroFilial objParametroFilialApresentacao;
                        if (!ParametroFilial.CarregarParametroPorFilial(Enumeradores.Parametro.CartaApresentacao, this, out objParametroFilialApresentacao, trans))
                            objParametroFilialApresentacao = new ParametroFilial { IdFilial = this.IdFilial, IdParametro = (int)Enumeradores.Parametro.CartaApresentacao };

                        objParametroFilialApresentacao.ValorParametro = cartaApresentacao;
                        objParametroFilialApresentacao.Save(trans);
                        #endregion CartaApresentacao

                        #region CartaAgradecimento
                        ParametroFilial objParametroFilialAgradecimento;
                        if (!ParametroFilial.CarregarParametroPorFilial(Enumeradores.Parametro.CartaAgradecimentoCandidatura, this, out objParametroFilialAgradecimento, trans))
                            objParametroFilialAgradecimento = new ParametroFilial { IdFilial = this.IdFilial, IdParametro = (int)Enumeradores.Parametro.CartaAgradecimentoCandidatura };

                        objParametroFilialAgradecimento.ValorParametro = cartaAgradecimentoCandidatura;
                        objParametroFilialAgradecimento.Save(trans);
                        #endregion CartaAgradecimento

                        // parâmetro null pelo motivo de não ser necessário alterar se o usuário não for administrador           
                        if (usaWebEstagios != null)
                        {
                            ParametroFilial usaWebEstagiosParam;
                            if (!ParametroFilial.CarregarParametroPorFilial(Enumeradores.Parametro.FilialParceiraWebEstagios, this, out usaWebEstagiosParam, trans))
                                usaWebEstagiosParam = new ParametroFilial { IdFilial = this.IdFilial, IdParametro = (int)Enumeradores.Parametro.FilialParceiraWebEstagios };

                            usaWebEstagiosParam.ValorParametro = usaWebEstagios.Value.ToString();
                            usaWebEstagiosParam.Save(trans);
                        }

                        //Pessoa Física
                        objPessoaFisica.Save(trans);

                        //Usuario
                        objUsuarioFilialPerfil.PessoaFisica = objPessoaFisica;
                        objUsuarioFilialPerfil.Filial = this;
                        objUsuarioFilialPerfil.Save(trans);

                        objUsuarioFilial.UsuarioFilialPerfil = objUsuarioFilialPerfil;
                        objUsuarioFilial.Save(trans);

                        ParametroFilial objParamFilialCartaEmpresaLiberada;
                        ParametroFilial.CarregarParametroPorFilial(Enumeradores.Parametro.EnvioCartaEmailEmpresaLiberada, this, out objParamFilialCartaEmpresaLiberada);

                        if (objParamFilialCartaEmpresaLiberada == null && (empresaAuditada && !VerificaAuditoria(this.IdFilial)))
                        {
                            string assunto;
                            string template = CartaEmail.RetornarConteudoBNE(Enumeradores.CartaEmail.EmpresaLiberada, out assunto);

                            var parametros = new
                            {
                                Nome = objPessoaFisica.NomePessoa,
                                NomeEmpresa = RazaoSocial
                            };
                            string mensagem = parametros.ToString(template);

                            string emailRemetente = Parametro.RecuperaValorParametro(Enumeradores.Parametro.EmailMensagens);

                            EmailSenderFactory
                                .Create(TipoEnviadorEmail.Fila)
                                .Enviar(null, null, objUsuarioFilialPerfil, assunto, mensagem, emailRemetente, objUsuarioFilial.EmailComercial, trans);

                            //Grava Vlr_Parametro de carta enviada para EmpresaLiberada (idf_Carta_Email)
                            objParamFilialCartaEmpresaLiberada = new ParametroFilial
                            {
                                IdParametro = (int)Enumeradores.Parametro.EnvioCartaEmailEmpresaLiberada,
                                IdFilial = this.IdFilial,
                                ValorParametro = ((int)Enumeradores.CartaEmail.EmpresaLiberada).ToString(),
                                FlagInativo = false
                            };
                            objParamFilialCartaEmpresaLiberada.Save();

                            #region EnvioSMS

                            string idUFPRemetente = Parametro.RecuperaValorParametro(Enumeradores.Parametro.IdfUfpEnvioSMSTanqueCadastroEmpresa);
                            int idUFPDestino = objUsuarioFilialPerfil.IdUsuarioFilialPerfil;

                            string smsUm = ConteudoHTML.RecuperaValorConteudo(Enumeradores.ConteudoHTML.SMSBoasVindasUm);
                            smsUm = smsUm.Replace("{Nome_Usuario}", objPessoaFisica.PrimeiroNome);

                            string smsDois = ConteudoHTML.RecuperaValorConteudo(Enumeradores.ConteudoHTML.SMSBoasVindasDois);
                            smsDois = smsDois.Replace("{Funcao_Usuario}", objUsuarioFilial.DescricaoFuncao);

                            List<BNE.BLL.DTO.PessoaFisicaEnvioSMSTanque> listaUsuarios = new List<BNE.BLL.DTO.PessoaFisicaEnvioSMSTanque>();

                            var objUsuarioEnvioSMS = new BNE.BLL.DTO.PessoaFisicaEnvioSMSTanque
                            {
                                dddCelular = objPessoaFisica.NumeroDDDCelular,
                                numeroCelular = objPessoaFisica.NumeroCelular,
                                nomePessoa = objPessoaFisica.NomePessoa,
                                mensagem = smsUm,
                                idDestinatario = objUsuarioFilialPerfil.IdUsuarioFilialPerfil
                            };

                            listaUsuarios.Add(objUsuarioEnvioSMS);

                            var objUsuarioEnvioSMSDois = new BNE.BLL.DTO.PessoaFisicaEnvioSMSTanque
                            {
                                dddCelular = objPessoaFisica.NumeroDDDCelular,
                                numeroCelular = objPessoaFisica.NumeroCelular,
                                nomePessoa = objPessoaFisica.NomePessoa,
                                mensagem = smsDois,
                                idDestinatario = objUsuarioFilialPerfil.IdUsuarioFilialPerfil
                            };

                            listaUsuarios.Add(objUsuarioEnvioSMSDois);

                            Mensagem.EnvioSMSTanque(idUFPRemetente, listaUsuarios);

                            #endregion

                            //Se a empresa foi auditada agora ativa todas as vagas da empresa e dispara o rastreador
                            var listaVagas = Vaga.ListarVagasFilialEmAuditoria(this, trans);
                            foreach (var objVaga in listaVagas)
                            {
                                objVaga.FlagInativo = false;
                                objVaga.Save(trans);

                                listaVagasParaPublicacao.Add(objVaga);
                            }
                        }

                        if (objOrigem != null)
                            objOrigem.Save(trans);

                        if (listaAlteracao != null && listaAlteracao.Any())
                        {
                            string alteracoes = listaAlteracao.Aggregate("Os seguintes campos foram alterados:  <br/>", (current, objAlteracao) => current + (objAlteracao.ToString() + " <br/> "));

                            FilialObservacao.SalvarCRM(alteracoes, this, idUsuarioFilialPerfilLogado.HasValue ? new UsuarioFilialPerfil(idUsuarioFilialPerfilLogado.Value) : objUsuarioFilialPerfil, trans);
                        }

                        EnvioEmailWebEstagiosIntegracao(contrataEstag);

                        trans.Commit();

                        //Se a empresa for nova
                        if (novo)
                        {
                            try
                            {
                                var parametros = new ParametroExecucaoCollection
                                {
                                    {"idFilial", "Filial", this._idFilial.ToString(), this._idFilial.ToString()}
                                };

                                ProcessoAssincrono.IniciarAtividade(
                                BLL.AsyncServices.Enumeradores.TipoAtividade.InclusaoEmpresa,
                                PluginsCompatibilidade.CarregarPorMetadata("InclusaoEmpresa", "PluginSaidaEmailSMS"),
                                parametros,
                                null,
                                null,
                                null,
                                null,
                                DateTime.Now);
                            }
                            catch (Exception ex)
                            {
                                EL.GerenciadorException.GravarExcecao(ex);
                            }
                        }
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }

        #region [ Integração Web Estágios ]
        private void EnvioEmailWebEstagiosIntegracao(bool contrataEstag)
        {
            if (!contrataEstag)
                return;

            var newParam = Enumeradores.Parametro.EmailWebEstagiosIntegracaoCadastroEmpresa;
            var parms = new List<Enumeradores.Parametro>
             {
                 newParam
             };

            var valores = Parametro.ListarParametros(parms);
            var emailLst = valores[newParam];

            if (string.IsNullOrEmpty(emailLst))
                return;

            var newCarta = Enumeradores.CartaEmail.ConteudoWebEstagiosIntegracaoCadastroEmpresa;

            string assunto;
            string conteudo = CartaEmail.RetornarConteudoBNE(newCarta, out assunto);

            if (string.IsNullOrEmpty(conteudo))
                return;

            var parametros = new
            {
                NomeEmpresa = this.NomeFantasia,
                CNPJ = this.CNPJ
            };
            string mensagemEmail = parametros.ToString(conteudo);

            string emailRemetenteSistema = Parametro.RecuperaValorParametro(Enumeradores.Parametro.EmailMensagens);

            foreach (var item in emailLst.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
            {
                EmailSenderFactory.Create(TipoEnviadorEmail.Fila)
                          .Enviar(assunto, mensagemEmail, emailRemetenteSistema, item);
            }
        }
        #endregion
        #endregion

        #region CarregarPorCnpj
        public static bool CarregarPorCnpj(decimal cnpj, out Filial objFilial)
        {
            using (IDataReader dr = ListarPorCnpj(cnpj))
            {
                objFilial = new Filial();
                if (SetInstance(dr, objFilial))
                    return true;
                objFilial = null;
                return false;
            }
        }
        #endregion

        #region ListarPorCnpj
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados.
        /// </summary>
        /// <param name="sigla">Sigla do registro.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static IDataReader ListarPorCnpj(decimal cnpj)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Num_CNPJ", SqlDbType.Decimal, 9));

            parms[0].Value = cnpj;

            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTCNPJ, parms);
        }


        public static Dictionary<int, string> ListarTodasPorCnpj(decimal cnpj)
        {

            Dictionary<int, string> ListaFiliaisRetorno = new Dictionary<int, string>();

            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Num_CNPJ", SqlDbType.VarChar, 9));
            parms[0].Value = cnpj.ToString().Substring(0, 9);

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SPLISTTODASCNPJ, parms))
            {
                while (dr.Read())
                {
                    ListaFiliaisRetorno.Add(Convert.ToInt32(dr["Idf_Filial"]), dr["Nme_Fantasia"].ToString());
                }
            }
            return ListaFiliaisRetorno;
        }
        #endregion

        #region ListarFilialPorRazaoRamoCidade
        public static DataTable ListarFilialPorRazaoRamoCidade(string razaoRamoCidade, int? idCurriculo, int paginaCorrente, int tamanhoPagina, out int totalRegistros)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@CurrentPage", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@PageSize", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@RazaoRamoCidade", SqlDbType.VarChar, 100));
            parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4));

            parms[0].Value = paginaCorrente;
            parms[1].Value = tamanhoPagina;

            if (String.IsNullOrEmpty(razaoRamoCidade))
                parms[2].Value = DBNull.Value;
            else
                parms[2].Value = razaoRamoCidade;

            parms[3].Value = idCurriculo;

            totalRegistros = 0;
            DataTable dt = null;
            try
            {
                using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTFILIAISRAZAORAMOCIDADE, parms))
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

        #region ListarFiliaisEmployer
        /// <summary>
        /// Lista as filiais cadastradas que contenham Employer na razão social ou que tenham CNPJ de filial BNE
        /// </summary>
        /// <returns>Data reader com as filiais encontradas</returns>
        public static Dictionary<string, string> ListarFiliaisEmployer()
        {
            Dictionary<string, string> retorno = new Dictionary<string, string>();
            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SP_LISTAR_FILIAIS_EMPLOYER, null))
            {
                while (dr.Read())
                    retorno.Add(dr["Idf_Filial"].ToString(), dr["Des_Filial"].ToString());

                if (!dr.IsClosed)
                {
                    dr.Close();
                }
                dr.Dispose();
            }

            return retorno;
        }
        #endregion

        #region DescontarEnvioSMS
        public bool DescontarEnvioSMS(SqlTransaction trans)
        {
            //Carrega plano quantidade vigente.
            PlanoQuantidade objPlanoQuantidade;
            if (PlanoQuantidade.CarregarPlanoAtualVigente(trans, this, out objPlanoQuantidade))
            {
                var possueSaldo = objPlanoQuantidade.QuantidadeSMS - objPlanoQuantidade.QuantidadeSMSUtilizado > 0;
                if (possueSaldo)
                {
                    objPlanoQuantidade.QuantidadeSMSUtilizado++;

                    if (trans != null)
                        objPlanoQuantidade.Save(trans);
                    else
                        objPlanoQuantidade.Save();

                    //Se saldo de SMS acabou de zerar, enviar msg a todos os usuários ativos da empresa
                    if (objPlanoQuantidade.QuantidadeSMS - objPlanoQuantidade.QuantidadeSMSUtilizado == 0)
                        EnviarSMSAvisoSaldoZerou(this.IdFilial);
                }

                return possueSaldo;
            }

            return false;
        }
        #endregion

        public void EnviarSMSAvisoSaldoZerou(int idFilial)
        {
            var IdfUfpAvisoSMS = Parametro.RecuperaValorParametro(Enumeradores.Parametro.IdfUfpAvisoSMS);
            List<BNE.BLL.DTO.PessoaFisicaEnvioSMSTanque> listaEnvioUsuariosAtivosFilialPerfil = UsuarioFilialPerfil.CarregarEnvioUsuariosAtivosPorFilialPerfil_SaldoZerou(idFilial);
            Mensagem.EnvioSMSTanque(IdfUfpAvisoSMS, listaEnvioUsuariosAtivosFilialPerfil, true);
        }

        #region DescontarVisualizacaoCurriculo
        public bool DescontarVisualizacaoCurriculo(SqlTransaction trans)
        {
            //Carrega plano quantidade vigente.
            PlanoQuantidade objPlanoQuantidade;
            if (PlanoQuantidade.CarregarPlanoAtualVigente(trans, this, out objPlanoQuantidade))
            {
                objPlanoQuantidade.QuantidadeVisualizacaoUtilizado++;

                if (trans != null)
                    objPlanoQuantidade.Save(trans);
                else
                    objPlanoQuantidade.Save();

                return true;
            }

            return false;
        }
        #endregion

        #region RecuperarSaldoSMS
        public int RecuperarSaldoSMS(SqlTransaction trans = null)
        {
            //Carrega plano quantidade vigente.
            PlanoQuantidade objPlanoQuantidade;
            if (PlanoQuantidade.CarregarPlanoAtualVigente(trans, this, out objPlanoQuantidade))
                return objPlanoQuantidade.QuantidadeSMS - objPlanoQuantidade.QuantidadeSMSUtilizado;

            return 0;
        }
        #endregion

        #region RecuperarSaldoVisualizacao
        public int RecuperarSaldoVisualizacao()
        {
            //Carrega plano quantidade vigente.
            PlanoQuantidade objPlanoQuantidade;
            if (PlanoQuantidade.CarregarPlanoAtualVigente(this._idFilial, out objPlanoQuantidade))
                return objPlanoQuantidade.QuantidadeVisualizacao - objPlanoQuantidade.QuantidadeVisualizacaoUtilizado;

            return 0;
        }
        #endregion

        #region RecuperarQuantidadeVagasDivuldadas()
        public int RecuperarQuantidadeVagasDivuldadas()
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Filial", SqlDbType.Int, 4));

            parms[0].Value = this._idFilial;

            Object ret = DataAccessLayer.ExecuteScalar(CommandType.Text, SPSOMAQUANTIDADEVAGASDIVULGADAS, parms);
            if (ret != DBNull.Value)
                return Convert.ToInt32(ret);
            else
                return 0;
        }
        #endregion

        #region RecuperarQuantidadeVagasAnunciadas
        public static int RecuperarQuantidadeVagasAnunciadas(int idFilial, int? idUsuarioFilialPerfil)
        {
            var parms = new List<SqlParameter>
				{
					new SqlParameter { ParameterName = "@Idf_Filial", SqlDbType = SqlDbType.Int, Size = 4, Value = idFilial } 
				};

            var parametroUsuario = new SqlParameter { ParameterName = "@Idf_Usuario_Filial_Perfil", SqlDbType = SqlDbType.Int, Size = 4, Value = DBNull.Value };

            if (idUsuarioFilialPerfil.HasValue)
                parametroUsuario.Value = idUsuarioFilialPerfil.Value;

            parms.Add(parametroUsuario);

            return Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPQUANTIDADEVAGASANUNCIADAS, parms));
        }
        #endregion

        #region RecuperarQuantidadeVagasArquivadas
        public static int RecuperarQuantidadeVagasArquivadas(int idFilial, int? idUsuarioFilialPerfil)
        {
            var parms = new List<SqlParameter>
				{
					new SqlParameter { ParameterName = "@Idf_Filial", SqlDbType = SqlDbType.Int, Size = 4, Value = idFilial } 
				};

            var parametroUsuario = new SqlParameter { ParameterName = "@Idf_Usuario_Filial_Perfil", SqlDbType = SqlDbType.Int, Size = 4, Value = DBNull.Value };

            if (idUsuarioFilialPerfil.HasValue)
                parametroUsuario.Value = idUsuarioFilialPerfil.Value;

            parms.Add(parametroUsuario);

            return Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPQUANTIDADEVAGASARQUIVADAS, parms));
        }
        #endregion

        #region RecuperarQuantidadeCurriculosVisualizados()
        public int RecuperarQuantidadeCurriculosVisualizados()
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Filial", SqlDbType.Int, 4));

            parms[0].Value = this._idFilial;

            return (int)DataAccessLayer.ExecuteScalar(CommandType.Text, SPCOUNTCVSVISUALIZADOS, parms);
        }
        #endregion

        #region RecuperarNomeFantasia
        /// <summary>
        /// Método que retorna o NomeFantasia da Filial
        /// </summary>
        /// <returns>Nme_Pessoa</returns>
        private string RecuperarNomeFantasia()
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Filial", SqlDbType = SqlDbType.Int, Size = 4, Value = _idFilial }
                };

            return Convert.ToString(DataAccessLayer.ExecuteScalar(CommandType.Text, SP_SELECT_NOME_FANTASIA, parms));
        }
        #endregion

        #region QuantidadeEmpresasCadastradas
        /// <summary>
        /// Quantidade Empresas Cadastradas no na data atual do sistema
        /// </summary>
        /// <returns></returns>
        public static int QuantidadeEmpresasCadastradas()
        {
            return Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPCOUNTEMPRESASCADASTRADAS, null));
        }

        #endregion

        #region ListaFiliaisDesatualizadas
        /// <summary>
        /// Método utilizado por retornar um datatable com o Codigo Identificador de uma filial e o e-mail para envio de uma mensagem 
        /// para alertar o usuário da necessidade de atualizar o cadastro
        /// </summary>
        /// <returns>DataTable</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static DataTable ListaFiliaisDesatualizadas()
        {
            return DataAccessLayer.ExecuteReaderDs(CommandType.Text, SPFILIAISDESATUALIZADAS, null).Tables[0];
        }
        #endregion

        #region BloquearFilial
        /// <summary>
        /// Altera o status do currículo para bloqueado e salva o motivo na tabela de correção
        /// </summary>
        /// <param name="objFilial"></param>
        /// <param name="motivo"></param>
        /// <param name="objUsuarioFilialPerfil"></param>
        /// <returns></returns>
        public static bool BloquearFilial(Filial objFilial, UsuarioFilialPerfil objUsuarioFilialPerfil, string motivo)
        {
            using (SqlConnection conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();

                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        objFilial.CompleteObject(trans);

                        FilialObservacao.SalvarCRM(motivo, objFilial, objUsuarioFilialPerfil, trans);

                        objFilial.SituacaoFilial = new SituacaoFilial((int)Enumeradores.SituacaoFilial.Bloqueado);
                        objFilial.Save(trans);

                        trans.Commit();
                        return true;
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

        #region DesbloquearFilial
        /// <summary>
        /// Altera o status do currículo para AguardandoPublicacao e salva o motivo na tabela de correção
        /// </summary>
        /// <param name="objFilial"></param>
        /// <param name="motivo"></param>
        /// <param name="objUsuarioFilialPerfil"></param>
        /// <returns></returns>
        public static bool DesbloquearFilial(Filial objFilial, UsuarioFilialPerfil objUsuarioFilialPerfil, string motivo)
        {
            using (SqlConnection conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();

                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        objFilial.CompleteObject(trans);

                        FilialObservacao.SalvarCRM(motivo, objFilial, objUsuarioFilialPerfil, trans);

                        objFilial.SituacaoFilial = new SituacaoFilial((int)Enumeradores.SituacaoFilial.AguardandoPublicacao);
                        objFilial.Save(trans);

                        trans.Commit();
                        return true;
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

        #region ListarFiliaisDadosRepetidos
        public static DataTable ListarFiliaisDadosRepetidos(int idFilial, int paginaAtual, int tamanhoPagina, out int totalRegistros)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@CurrentPage", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@PageSize", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Filial", SqlDbType.Int, 4));

            parms[0].Value = paginaAtual;
            parms[1].Value = tamanhoPagina;
            parms[2].Value = idFilial;

            totalRegistros = 0;
            DataTable dt = null;
            try
            {
                using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.StoredProcedure, "FIL_SP_Rastrear_Duplicidade", parms))
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

        #region ListarFiliaisEmployerAutoComplete
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados.
        /// </summary>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static List<string> ListarFiliaisEmployerAutoComplete(string apelidoFilial, int totalRetornos)
        {
            List<string> lista = new List<string>();
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Ape_Filial", SqlDbType.VarChar, 80));
            parms.Add(new SqlParameter("@Count", SqlDbType.Int, 4));

            parms[0].Value = apelidoFilial;
            parms[1].Value = totalRetornos;

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTFILIALEMPLOYERAUTOCOMPLETE, parms))
            {
                while (dr.Read())
                {
                    lista.Add(dr["Ape_Filial"].ToString());
                }
            }

            return lista;
        }
        #endregion

        #region CarregarFilialEmployerPorApelido
        /// <summary>
        /// Método utilizado para retornar uma instância de Filial a partir do banco de dados.
        /// </summary>
        /// <param name="razaoSocial"></param>
        /// <returns>Instância de Filial.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static bool CarregarFilialEmployerPorApelido(string apeFilial, out Filial objFilial)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Ape_Filial", SqlDbType = SqlDbType.VarChar, Size = 100, Value = apeFilial }
                };

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTFILIALEMPLOYERPORAPELIDO, parms))
            {
                objFilial = new Filial();
                if (SetInstance(dr, objFilial))
                    return true;
                objFilial = null;
                return false;
            }
        }
        #endregion

        #region CarregarFilialEmployerPorFilial
        /// <summary>
        /// Método utilizado para retornar uma instância de Filial a partir do banco de dados.
        /// </summary>
        /// <param name="objFilial"></param>
        /// <returns>Instância de Filial.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static GerenteFilial CarregarFilialEmployerPorFilial(Filial objFilial)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Filial", SqlDbType = SqlDbType.Int, Size = 4, Value = objFilial.IdFilial }
                };

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spselectfilialemployergerente, parms))
            {
                if (dr.Read())
                {
                    var objGerenteFilial = new GerenteFilial
                    {
                        Nome = dr["Nme_Gerente_Filial"].ToString(),
                        Email = dr["Eml_Gerente"].ToString()
                    };

                    return objGerenteFilial;
                }
                if (!dr.IsClosed)
                    dr.Close();
            }
            throw (new RecordNotFoundException(typeof(GerenteFilial)));
        }
        #endregion

        #region QuantidadeUsuariosAtivos
        /// <summary>
        /// Quantidade de usuários ativos para a empresa atual
        /// </summary>
        /// <returns></returns>
        public int QuantidadeUsuariosAtivos()
        {
            var parms = new List<SqlParameter> { new SqlParameter("@Idf_Filial", SqlDbType.Int, 4) };
            parms[0].Value = this._idFilial;

            return Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SpQuantidadeUsuariosAtivos, parms));
        }
        #endregion

        #region RecuperarQuantidadeAcessosAdquiridos
        /// <summary>
        /// Quantidade de usuários que foram comprados pela empresa
        /// </summary>
        /// <returns></returns>
        public int RecuperarQuantidadeAcessosAdquiridos()
        {
            int quantidadeUsuariosPadrao = Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.QuantidadeLimiteUsuarios));

            var parms = new List<SqlParameter> { new SqlParameter("@Idf_Filial", SqlDbType.Int, 4) };
            parms[0].Value = this._idFilial;

            var retorno = DataAccessLayer.ExecuteScalar(CommandType.Text, SpQuantidadeAcessosAdquiridos, parms);
            if (retorno != DBNull.Value)
            {
                int quantidadeUsuarioAdquiridos = Convert.ToInt32(retorno);
                return quantidadeUsuariosPadrao + quantidadeUsuarioAdquiridos;
            }

            return quantidadeUsuariosPadrao;
        }
        #endregion

        #region ExisteFilial
        public static bool ExisteFilial(decimal numeroCNPJ)
        {
            var parms = new List<SqlParameter>
				{
					new SqlParameter { ParameterName = "@Num_CNPJ", SqlDbType = SqlDbType.Decimal, Size = 14, Value = numeroCNPJ } 
				};
            return Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, Spexistefilial, parms)) > 0;
        }
        public static bool ExisteFilial(string numeroCNPJ)
        {
            if (string.IsNullOrWhiteSpace(numeroCNPJ))
                return false;

            numeroCNPJ = new Regex("[.\\/-]").Replace(numeroCNPJ, "");
            return ExisteFilial(Convert.ToDecimal(numeroCNPJ));
        }
        #endregion

        #region RecuperarIdentificador
        public static int? RecuperarIdentificador(decimal numeroCNPJ)
        {
            var parms = new List<SqlParameter>
				{
					new SqlParameter { ParameterName = "@Num_CNPJ", SqlDbType = SqlDbType.Decimal, Size = 14, Value = numeroCNPJ } 
				};
            var retorno = DataAccessLayer.ExecuteScalar(CommandType.Text, Sprecuperaridentificador, parms);

            if (retorno != null)
                return Convert.ToInt32(retorno);

            return null;
        }
        public static int? RecuperarIdentificador(string numeroCNPJ)
        {
            numeroCNPJ = new Regex("[.\\/-]").Replace(numeroCNPJ, "");
            return RecuperarIdentificador(Convert.ToDecimal(numeroCNPJ));
        }
        #endregion

        #region EmpresaAssociacao
        public bool EmpresaAssociacao()
        {
            bool retorno = false;

            Enumeradores.TipoParceiro tipoParceiro = RecuperarTipoParceiro();

            if (tipoParceiro == Enumeradores.TipoParceiro.STCAssociacao)
                retorno = true;
            else
                retorno = false;

            return retorno;
        }
        #endregion

        #region RecuperarTipoParceiro
        public Enumeradores.TipoParceiro RecuperarTipoParceiro()
        {
            var parms = new List<SqlParameter>
				{
					new SqlParameter { ParameterName = "@Idf_Filial", SqlDbType = SqlDbType.Int, Size = 4, Value = _idFilial } 
				};

            object retornoBd =
                DataAccessLayer.ExecuteScalar(CommandType.Text, SpRecuperarTipoParceiro, parms);

            int idTipoParceiro;
            if (retornoBd != DBNull.Value)
                idTipoParceiro = Convert.ToInt32(retornoBd);
            else
                idTipoParceiro = 0;

            return (Enumeradores.TipoParceiro)idTipoParceiro;
        }
        #endregion

        #region PossuiSTCAssociacao
        public bool PossuiSTCAssociacao()
        {
            if (this._tipoParceiro != null)
                return this._tipoParceiro.IdTipoParceiro.Equals((int)Enumeradores.TipoParceiro.STCAssociacao);
            return RecuperarTipoParceiro() == Enumeradores.TipoParceiro.STCAssociacao;
        }
        #endregion

        #region PossuiSTCUniversitario
        public bool PossuiSTCUniversitario()
        {
            if (this._tipoParceiro != null)
                return this._tipoParceiro.IdTipoParceiro.Equals((int)Enumeradores.TipoParceiro.STCUniversitario);
            return RecuperarTipoParceiro() == Enumeradores.TipoParceiro.STCUniversitario;
        }
        #endregion

        #region PossuiSTCLanhouse
        public bool PossuiSTCLanhouse()
        {
            if (this._tipoParceiro != null)
                return this._tipoParceiro.IdTipoParceiro.Equals((int)Enumeradores.TipoParceiro.STCLanhouse);
            return RecuperarTipoParceiro() == Enumeradores.TipoParceiro.STCLanhouse;
        }
        #endregion

        #region DTO

        #region RecuperarDTO
        /// <summary>
        /// Método utilizado para retornar uma instância completa de Filial a partir do banco de dados.
        /// </summary>
        /// <param name="idFilial">Chave do registro.</param>
        /// <returns>Instância de Filial.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static DTO.Filial CarregarDTO(int idFilial)
        {
            using (IDataReader dr = RetornarDataReader(idFilial))
            {
                var objFilial = new DTO.Filial();
                if (SetInstanceDTO(dr, objFilial))
                    return objFilial;
            }
            throw (new RecordNotFoundException(typeof(DTO.Filial)));
        }
        /// <summary>
        /// Método utilizado para retornar uma instância de Filial a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idFilial">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Instância de Filial.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static DTO.Filial CarregarDTO(int idFilial, SqlTransaction trans)
        {
            using (IDataReader dr = RetornarDataReader(idFilial, trans))
            {
                var objFilial = new DTO.Filial();
                if (SetInstanceDTO(dr, objFilial))
                    return objFilial;
            }
            throw (new RecordNotFoundException(typeof(DTO.Filial)));
        }
        #endregion

        #region RetornarDataReader
        public static IDataReader RetornarDataReader(int idFilial)
        {
            return RetornarDataReader(idFilial, null);
        }
        public static IDataReader RetornarDataReader(int idFilial, SqlTransaction trans)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Filial", SqlDbType = SqlDbType.Int, Size = 4, Value = idFilial }
                };

            #region spselectfilial
            const string spselectfilial = @"
            SELECT  F.Idf_Filial ,
                    F.Num_CNPJ ,
                    F.Nme_Fantasia ,
                    F.Raz_Social ,
                    F.Num_DDD_Comercial ,
                    F.Num_Comercial
            FROM    TAB_Filial F WITH(NOLOCK)
            WHERE   F.Idf_Filial = @Idf_Filial
            ";
            #endregion

            if (trans != null)
                return DataAccessLayer.ExecuteReader(trans, CommandType.Text, spselectfilial, parms);

            return DataAccessLayer.ExecuteReader(CommandType.Text, spselectfilial, parms);
        }
        #endregion

        #region SetInstanceDTO
        /// <summary>
        /// Método auxiliar utilizado pelos métodos LoadObject e CompleteObject para percorrer um IDataReader e vincular as colunas com os atributos da classe.
        /// </summary>
        /// <param name="dr">Cursor de leitura do banco de dados.</param>
        /// <param name="objFilial">Instância a ser manipulada.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static bool SetInstanceDTO(IDataReader dr, DTO.Filial objFilial)
        {
            try
            {
                if (dr.Read())
                {

                    objFilial.IdFilial = Convert.ToInt32(dr["Idf_Filial"]);

                    if (dr["Num_CNPJ"] != DBNull.Value)
                        objFilial.NumeroCNPJ = Convert.ToDecimal(dr["Num_CNPJ"]);

                    if (dr["Nme_Fantasia"] != DBNull.Value)
                        objFilial.NomeFantasia = dr["Nme_Fantasia"].ToString();

                    if (dr["Raz_Social"] != DBNull.Value)
                        objFilial.RazaoSocial = dr["Raz_Social"].ToString();

                    if (dr["Num_DDD_Comercial"] != DBNull.Value)
                        objFilial.NumeroDDDComercial = dr["Num_DDD_Comercial"].ToString();

                    if (dr["Num_Comercial"] != DBNull.Value)
                        objFilial.NumeroComercial = dr["Num_Comercial"].ToString();

                    return true;
                }
                else
                {
                    return false;
                }
            }
            finally
            {
                dr.Dispose();
            }
        }
        #endregion

        #endregion

        #region ListarFiliaisDataCadastro
        public static DataTable ListarFiliaisDataCadastro(DateTime dataInicial, DateTime dataFinal)
        {
            List<SqlParameter> parms = new List<SqlParameter>();

            parms.Add(new SqlParameter("@dtInicial", SqlDbType.SmallDateTime, 15));
            parms.Add(new SqlParameter("@dtFinal", SqlDbType.SmallDateTime, 15));

            parms[0].Value = dataInicial;
            parms[1].Value = dataFinal;

            DataTable dt = null;
            try
            {
                using (DataSet ds = DataAccessLayer.ExecuteReaderDs(CommandType.StoredProcedure, TAB_SP_BuscaEmpresasData, parms))
                {
                    dt = ds.Tables[0];
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

        #region RecuperarSituacaoFilial
        private SituacaoFilial RecuperarSituacaoFilial()
        {
            var parms = new List<SqlParameter>
				{
					new SqlParameter { ParameterName = "@Idf_Filial", SqlDbType = SqlDbType.Int, Size = 4, Value = _idFilial } 
				};

            SituacaoFilial objSituacaoFilial;
            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Sprecuperarsituacaofilial, parms))
            {
                objSituacaoFilial = new SituacaoFilial();
                if (!SituacaoFilial.SetInstance(dr, objSituacaoFilial))
                    objSituacaoFilial = null;
            }

            return objSituacaoFilial;
        }
        #endregion

        #region EmpresaEmAuditoria
        /// <summary>
        /// Método responsável por retornar se a empresa está em processo de auditoria ou não
        /// </summary>
        /// <returns></returns>
        public bool EmpresaEmAuditoria()
        {
            return RecuperarSituacaoFilial().IdSituacaoFilial.Equals((int)Enumeradores.SituacaoFilial.AguardandoPublicacao);
        }
        #endregion

        #region EmpresaBloqueada
        /// <summary>
        /// Método responsável por retornar se a empresa está bloqueada, cancelada ou não
        /// </summary>
        /// <returns></returns>
        public bool EmpresaBloqueada()
        {
            return (RecuperarSituacaoFilial().IdSituacaoFilial.Equals((int)Enumeradores.SituacaoFilial.Bloqueado) || RecuperarSituacaoFilial().IdSituacaoFilial.Equals((int)Enumeradores.SituacaoFilial.Cancelado));
        }
        #endregion

        #region SalaAdministradoraListar
        /// <summary>
        /// Lista as empresas da sala da administradora
        /// </summary>
        /// <param name="situacao">Situação da empresa</param>
        /// <param name="filtro">Filtro dos Campos</param>
        /// <param name="paginaCorrente">Página Atual</param>
        /// <param name="tamanhoPagina">Tamanho da Página </param>
        /// <param name="totalRegistros">Quantidade Total de Registros</param>
        /// <returns>Uma datatable com os dados da empresa</returns>
        public static DataTable SalaAdministradoraListar(Enumeradores.SituacaoFilial? situacao, String filtro, int paginaCorrente, int tamanhoPagina, out int totalRegistros)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@CurrentPage", SqlDbType = SqlDbType.Int, Size = 4, Value = paginaCorrente },
                    new SqlParameter { ParameterName = "@PageSize", SqlDbType = SqlDbType.Int, Size = 4, Value = tamanhoPagina }
                };

            var sqlParamSituacao = new SqlParameter { ParameterName = "@Idf_Situacao", SqlDbType = SqlDbType.Int, Size = 4, Value = DBNull.Value };
            var sqlParamFiltro = new SqlParameter { ParameterName = "@Filtro", SqlDbType = SqlDbType.VarChar, Size = 100, Value = DBNull.Value };
            var sqlParamCNPJ = new SqlParameter { ParameterName = "@Num_CNPJ", SqlDbType = SqlDbType.Decimal, Size = 14, Value = DBNull.Value };
            var sqlParamTelefone = new SqlParameter { ParameterName = "@Telefone", SqlDbType = SqlDbType.VarChar, Size = 12, Value = DBNull.Value };
            var sqlParamEmail = new SqlParameter { ParameterName = "@Email", SqlDbType = SqlDbType.VarChar, Size = 40, Value = DBNull.Value };

            if (situacao.HasValue)
                sqlParamSituacao.Value = (int)situacao.Value;

            if (!string.IsNullOrEmpty(filtro))
            {
                if (Regex.IsMatch(filtro, @"^\d*$")) // valida se é numérico
                    if (Regex.IsMatch(filtro, @"^\d{13,14}$")) // valida se tem 14 digitos, CNPJ
                        sqlParamCNPJ.Value = Convert.ToDecimal(filtro);
                    else
                        sqlParamTelefone.Value = filtro;
                else if (Regex.IsMatch(filtro, @"(\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)")) //valida se for email
                    sqlParamEmail.Value = filtro;
                else if (Regex.IsMatch(filtro, @"\d{2,3}.\d{3}.\d{3}/\d{4}-\d{2}")) //valida se é um CNPJ com o formato xx.xxx.xxx/xxxx-xx
                {
                    string valor = filtro.Replace(".", String.Empty).Replace("/", String.Empty).Replace("-", String.Empty);
                    decimal decimalValue;
                    if (Decimal.TryParse(valor, out decimalValue))
                        sqlParamCNPJ.Value = decimalValue;
                }
                else if (Regex.IsMatch(filtro, @"\(?\d{2}?\)?[\s-]?\d{4}-?\d{4}$")) //valida se é um telefone com caracteres
                {
                    string valor = filtro.Replace("(", String.Empty).Replace(")", String.Empty).Replace("-", String.Empty).Trim();
                    decimal decimalValue;
                    if (Decimal.TryParse(valor, out decimalValue))
                        sqlParamTelefone.Value = decimalValue;
                }
                else
                    sqlParamFiltro.Value = filtro;
            }
            parms.Add(sqlParamFiltro);
            parms.Add(sqlParamSituacao);
            parms.Add(sqlParamCNPJ);
            parms.Add(sqlParamTelefone);
            parms.Add(sqlParamEmail);

            totalRegistros = 0;
            DataTable dt = null;
            try
            {
                using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spselectsalaadministrador, parms))
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

        #region ExisteSemLocalizacao
        public static bool ExisteSemLocalizacao()
        {
            return Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, Spquantidadesemlocalizacao, null)) > 0;
        }
        #endregion

        #region ListarSemLocalizacao
        /// <summary>
        /// Método utilizado por retornar um DataTable com todas as filiais sem localização
        /// </summary>
        /// <returns>DataTable</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static DataTable ListarSemLocalizacao(int limite)
        {
            var parms = new List<SqlParameter>
				{
					new SqlParameter { ParameterName = "@Limite", SqlDbType = SqlDbType.Int, Size = 4, Value = limite } 
				};

            return DataAccessLayer.ExecuteReaderDs(CommandType.Text, Sprecuperarsemlocalizacao, parms).Tables[0];
        }
        #endregion

        #region AlterarLocalizacao
        /// <summary>
        /// Altera a localizacao
        /// </summary>
        /// <returns></returns>
        public void AlterarLocalizacao(SqlGeography localizacao)
        {
            var parms = new List<SqlParameter>
				{
					new SqlParameter { ParameterName = "@Idf_Filial", SqlDbType = SqlDbType.Int, Size = 4, Value = IdFilial } ,
					new SqlParameter { ParameterName = "@Des_Localizacao", Size = 4, Value = localizacao, UdtTypeName = "Geography" }
				};

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, Spatualizarlocalizacao, parms);
        }
        #endregion

        #region RecuperarNumeroCNPJ
        public decimal RecuperarNumeroCNPJ()
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Filial", SqlDbType = SqlDbType.Int, Size = 4, Value = _idFilial }
                };

            Object ret = DataAccessLayer.ExecuteScalar(CommandType.Text, Sprecuperarcnpj, parms);
            if (ret != DBNull.Value)
                return Convert.ToDecimal(ret);

            return 0;
        }
        #endregion

        #region RecuperarDataCadastro
        public DateTime RecuperarDataCadastro()
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Filial", SqlDbType = SqlDbType.Int, Size = 4, Value = _idFilial }
                };

            Object ret = DataAccessLayer.ExecuteScalar(CommandType.Text, Sprecuperardatacadastro, parms);
            if (ret != DBNull.Value)
                return Convert.ToDateTime(ret);

            return DateTime.Today;
        }
        #endregion

        #region PossuiPlanoAtivo
        /// <summary>
        /// Retorna um booleano indicando se a empresa possui um plano adquirido liberado
        /// </summary>
        /// <returns></returns>
        public bool PossuiPlanoAtivo()
        {
            return PlanoAdquirido.ExistePlanoAdquiridoLiberadoPorFilial(this);
        }
        #endregion

        #region PossuiPlanoAtivo
        /// <summary>
        /// Retorna um booleano indicando se a empresa possui um plano adquirido liberado
        /// </summary>
        /// <returns></returns>
        public bool PossuiPlanoElegivel1Clique()
        {
            return PlanoAdquirido.ExistePlanoAdquiridoLiberadoPorFilialElegivel1Clique(this);
        }
        #endregion

        #region EmpresaSemPlanoPodeEnviarSMS
        /// <summary>
        /// Retorna um booleano indicando se a empresa sem plano adqurido liberado pode enviar SMS para candidatos VIP
        /// </summary>
        /// <returns></returns>
        public bool EmpresaSemPlanoPodeEnviarSMS(int quantidadeVIP)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Filial", SqlDbType = SqlDbType.Int, Size = 4, Value = _idFilial }
                };

            int quantidadeSMSEnviadoHoje = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SpquantidadeSMSenviadocandidatohoje, parms));

            int quantidadeLimiteSMS = Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.QuantidadeSMSDiarioEmpresaChupaVIP));

            if (quantidadeLimiteSMS >= (quantidadeSMSEnviadoHoje + quantidadeVIP))
                return true;

            return false;
        }
        #endregion

        #region EmpresaSemPlanoPodeVisualizarCurriculo
        /// <summary>
        /// Retorna um booleano indicando se a empresa sem plano adqurido liberado pode visualizar curriculo para candidatos VIP
        /// </summary>
        /// <returns></returns>
        public bool EmpresaSemPlanoPodeVisualizarCurriculo(int quantidadeVIP)
        {
            var dataEncerramentoUltimoPlanoAdquirido = PlanoAdquirido.RecuperarDataFimUltimoPlanoAdquiridoEncerrado(this);

            DateTime dataBase = dataEncerramentoUltimoPlanoAdquirido.HasValue ? dataEncerramentoUltimoPlanoAdquirido.Value : RecuperarDataCadastro();

            var parametros = new List<Enumeradores.Parametro>
                    {
                        Enumeradores.Parametro.ChupaVIPQuantidadePrimeiroDia,
                        Enumeradores.Parametro.ChupaVIPQuantidadeUltimoDia,
                        Enumeradores.Parametro.ChupaVIPQuantidadeVisualizacaoMenosPrimeiroDia,
                        Enumeradores.Parametro.ChupaVIPQuantidadeVisualizacaoEntrePrimeiroEUltimoDia,
                        Enumeradores.Parametro.ChupaVIPQuantidadeVisualizacaoMaisUltimoDia

                    };

            Dictionary<Enumeradores.Parametro, string> valoresParametros = Parametro.ListarParametros(parametros);

            int quantidadeVisualizacao = 0;

            var diasBase = (DateTime.Today - dataBase).Days;
            if (diasBase <= Convert.ToInt32(valoresParametros[Enumeradores.Parametro.ChupaVIPQuantidadeVisualizacaoMenosPrimeiroDia]))
                quantidadeVisualizacao = Convert.ToInt32(valoresParametros[Enumeradores.Parametro.ChupaVIPQuantidadeVisualizacaoMenosPrimeiroDia]);
            else if (diasBase > Convert.ToInt32(valoresParametros[Enumeradores.Parametro.ChupaVIPQuantidadePrimeiroDia]) && diasBase <= Convert.ToInt32(valoresParametros[Enumeradores.Parametro.ChupaVIPQuantidadeUltimoDia]))
                quantidadeVisualizacao = Convert.ToInt32(valoresParametros[Enumeradores.Parametro.ChupaVIPQuantidadeVisualizacaoEntrePrimeiroEUltimoDia]);
            else if (diasBase > Convert.ToInt32(valoresParametros[Enumeradores.Parametro.ChupaVIPQuantidadeUltimoDia]))
                quantidadeVisualizacao = Convert.ToInt32(valoresParametros[Enumeradores.Parametro.ChupaVIPQuantidadeVisualizacaoMaisUltimoDia]);

            int quantidadeVisualizacaoHoje = CurriculoVisualizacaoHistorico.RecuperarQuantidadeVisualizacaoDadosCompletosCurriculosVIP(this);

            if (quantidadeVisualizacao >= (quantidadeVisualizacaoHoje + quantidadeVIP))
                return true;

            return false;
        }
        #endregion

        #region ListarFiliaisPorNomeFantasia
        /// <summary>
        /// Retorna o Idf_Filial e o Nme_Fantasia dado um prefixo do nome fantasia
        /// </summary>
        /// <returns>Dicionário id-nome fantasia que coincidem com o prefixo dado</returns>
        /// <remarks>Uchimura</remarks>
        public static Dictionary<int, string> ListarFiliaisPorNomeFantasia(string nomeFantasia, int totalRetornos)
        {
            Dictionary<int, string> retorno = new Dictionary<int, string>();

            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Nme_Fantasia", SqlDbType.VarChar, 100));
            parms.Add(new SqlParameter("@Count", SqlDbType.Int, 4));

            parms[0].Value = nomeFantasia;
            parms[1].Value = totalRetornos;

            using (IDataReader dr =
                DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTFILIALPORNOMEFANTASIA, parms))
            {
                while (dr.Read())
                    retorno.Add(Convert.ToInt32(dr["Idf_Filial"]), dr["Nme_Fantasia"].ToString());
            }

            return retorno;
        }
        #endregion

        #region RecuperarConteudoFilialParaContratoPorFilial
        public static void RecuperarConteudoFilialParaContratoPorFilial(int idFilial, out string razaoSocial, out string numCNPJ, out string descRua, out string numeroRua, out string nomeCidade, out string estado, out string numCEP)
        {
            razaoSocial = string.Empty;
            numCNPJ = string.Empty;
            descRua = string.Empty;
            numeroRua = string.Empty;
            nomeCidade = string.Empty;
            estado = string.Empty;
            numCEP = string.Empty;

            var parms = new List<SqlParameter>
            {
                new SqlParameter{ ParameterName = "@Idf_Filial", SqlDbType = SqlDbType.Int, Size = 4, Value = idFilial  }        
            };

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SpConteudoFilialContratoDigitalPorFilial, parms))
            {
                if (dr.Read())
                {
                    razaoSocial = dr["Raz_Social"].ToString();
                    numCNPJ = dr["Num_CNPJ"].ToString();
                    descRua = dr["Des_Logradouro"].ToString();
                    numeroRua = dr["Num_Endereco"].ToStringNullSafe();
                    nomeCidade = dr["Nme_Cidade"].ToString();
                    estado = dr["Sig_Estado"].ToString();
                    numCEP = dr["Num_CEP"].ToStringNullSafe();
                }
            }
        }
        #endregion

        #region RecuperarConteudoFilialParaContratoPorUsuarioFilialPerfil
        public static void RecuperarConteudoFilialParaContratoPorUsuarioFilialPerfil(int idUsuarioFilialPerfil, out string razaoSocial, out string numCNPJ, out string descRua, out string numeroRua, out string nomeCidade, out string estado, out string numCEP)
        {

            razaoSocial = string.Empty;
            numCNPJ = string.Empty;
            descRua = string.Empty;
            numeroRua = string.Empty;
            nomeCidade = string.Empty;
            estado = string.Empty;
            numCEP = string.Empty;

            var parms = new List<SqlParameter>
            {
                new SqlParameter{ ParameterName = "@Idf_Usuario_Filial_Perfil", SqlDbType = SqlDbType.Int, Size = 4, Value = idUsuarioFilialPerfil  }        
            };

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SpConteudoFilialContratoDigitalPorUsuarioFilialPerfil, parms))
            {
                if (dr.Read())
                {
                    razaoSocial = dr["Raz_Social"].ToString();
                    numCNPJ = dr["Num_CNPJ"].ToString();
                    descRua = dr["Des_Logradouro"].ToString();
                    numeroRua = dr["Num_Endereco"].ToStringNullSafe();
                    nomeCidade = dr["Nme_Cidade"].ToString();
                    estado = dr["Sig_Estado"].ToString();
                    numCEP = dr["Num_CEP"].ToStringNullSafe();
                }
            }
        }
        #endregion

        #region RecuperarInformacoesIntegracaoFinanceiro
        public static bool RecuperarInformacoesIntegracaoFinanceiro(int idPagamento, out DateTime dataPagamento, out decimal valorPagamento, out string desIdentificador, out DateTime dataInicioPlano, out DateTime dataFimPlano, out string numCNPJ, out string razaoSocial, out string numCEP, out string rua, out int numEndereco, out string complemento, out string bairro, out string cidade, out string uf, out string nomeFantasia, out string idfCnaePrincipal, out string emailContato, out string ddd, out string telefone, out string nomeContato, out int numeroBanco, out string filialGestora, SqlTransaction trans = null)
        {

            var retorno = false;

            dataPagamento = new DateTime();
            valorPagamento = 0;
            desIdentificador = string.Empty;
            dataInicioPlano = new DateTime();
            dataFimPlano = new DateTime();
            numCNPJ = string.Empty;
            razaoSocial = string.Empty;
            numCEP = string.Empty;
            rua = string.Empty;
            numEndereco = 0;
            complemento = string.Empty;
            bairro = string.Empty;
            cidade = string.Empty;
            uf = string.Empty;
            nomeFantasia = string.Empty;
            idfCnaePrincipal = string.Empty;
            emailContato = string.Empty;
            ddd = string.Empty;
            telefone = string.Empty;
            nomeContato = string.Empty;
            numeroBanco = (int)Enumeradores.Banco.HSBC; //Default HSBC
            filialGestora = "BNE"; //Default BNE

            var parms = new List<SqlParameter> 
                {
                    new SqlParameter{ ParameterName = "@Idf_Pagamento", SqlDbType = SqlDbType.Int, Size = 6, Value = idPagamento}
                };


            IDataReader dr = trans == null ? DataAccessLayer.ExecuteReader(CommandType.Text, SpRecuperarInformacoesIntegracaoFinanceiro, parms) : DataAccessLayer.ExecuteReader(trans, CommandType.Text, SpRecuperarInformacoesIntegracaoFinanceiro, parms);

            if (dr.Read())
            {
                if (dr["Dta_Pagamento"] != DBNull.Value)
                    dataPagamento = Convert.ToDateTime(dr["Dta_Pagamento"]);
                valorPagamento = Convert.ToDecimal(dr["Vlr_Pagamento"]);
                desIdentificador = dr["Des_Identificador"].ToString();
                dataInicioPlano = Convert.ToDateTime(dr["Dta_Inicio_Plano"]);
                dataFimPlano = Convert.ToDateTime(dr["Dta_Fim_Plano"]);
                numCNPJ = dr["Num_CNPJ"].ToString();
                razaoSocial = dr["Raz_Social"].ToString();
                numCEP = dr["Num_CEP"].ToString();
                rua = dr["Des_Logradouro"].ToString();
                if (!Int32.TryParse(dr["Num_Endereco"].ToString(), out numEndereco))
                    numEndereco = 0;
                if (dr["Des_Complemento"] != DBNull.Value)
                    complemento = dr["Des_Complemento"].ToString();
                bairro = dr["Des_Bairro"].ToString();
                cidade = dr["Nme_Cidade"].ToString();
                uf = dr["Sig_Estado"].ToString();
                nomeFantasia = dr["Nme_Fantasia"].ToString();
                idfCnaePrincipal = Convert.ToString(dr["Cod_CNAE_Sub_Classe"]);
                ddd = dr["Num_DDD_Comercial"].ToString();
                telefone = dr["Num_Comercial"].ToString();
                emailContato = dr["Eml_Envio_Boleto"].ToString();
                nomeContato = dr["Nme_Res_Plano_Adquirido"].ToString();
                if (dr["Filial_Gestora"] != DBNull.Value)
                    filialGestora = dr["Filial_Gestora"].ToString();
                if (dr["Idf_Banco"] != DBNull.Value)
                    numeroBanco = Convert.ToInt32(dr["Idf_Banco"].ToString());

                retorno = true;
            }

            if (!dr.IsClosed)
            {
                dr.Close();
            }

            dr.Dispose();

            return retorno;
        }
        #endregion

        #region VerificaAuditoria
        public static bool VerificaAuditoria(int idFilial)
        {
            var parms = new List<SqlParameter> 
                {
                    new SqlParameter{ ParameterName = "@Idf_Filial", SqlDbType = SqlDbType.Int, Size = 4, Value = idFilial}
                };

            return Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SpVerificaAuditoria, parms)) > 0;
        }

        #endregion

        #region RecuperarInformacoesFilialParaCartaCvPerfil
        public static bool RecuperarInformacoesFilialParaCartaCvPerfil(int idFilial, out string setorEmpresa, out string porteEmpresa, out string cidade, out string uf)
        {
            bool retorno = false;

            setorEmpresa = string.Empty;
            porteEmpresa = string.Empty;
            cidade = string.Empty;
            uf = string.Empty;

            var parms = new List<SqlParameter> 
                {
                    new SqlParameter{ ParameterName = "@Idf_Filial", SqlDbType = SqlDbType.Int, Value = idFilial}  
                };

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SpRecuperarInfoFilialParaCartaCvsPerfil, parms))
            {
                if (dr.Read())
                {
                    if (dr["Des_Area_BNE"] != DBNull.Value)
                        setorEmpresa = dr["Des_Area_BNE"].ToString();
                    if (dr["Des_Porte_Empresa"] != DBNull.Value)
                        porteEmpresa = dr["Des_Porte_Empresa"].ToString();
                    if (dr["Nme_Cidade"] != DBNull.Value)
                        cidade = dr["Nme_Cidade"].ToString();
                    if (dr["Sig_Estado"] != DBNull.Value)
                        uf = dr["Sig_Estado"].ToString();

                    retorno = true;
                }
            };

            return retorno;

        }
        #endregion

        #region EnviarEmailFiliais
        public static void EnviarEmailFiliais()
        {
            string assunto;
            string template = CartaEmail.RetornarConteudoBNE(Enumeradores.CartaEmail.EmailParaFiliais, out assunto);
            var emailRemetente = Parametro.RecuperaValorParametro(Enumeradores.Parametro.EmailMensagens);
            var lista = ListaFiliaisEnviarEmailParaFiliais();
            foreach (var objFilial in lista)
            {
                UsuarioFilialPerfil objUsuarioFilialPerfil;
                if (UsuarioFilialPerfil.CarregarPorPerfilFilial((int)BLL.Enumeradores.Perfil.AcessoEmpresaMaster, objFilial.IdFilial, out objUsuarioFilialPerfil))
                {
                    UsuarioFilial objUsuarioFilial;
                    if (UsuarioFilial.CarregarUsuarioFilialPorUsuarioFilialPerfil(objUsuarioFilialPerfil.IdUsuarioFilialPerfil, out objUsuarioFilial))
                    {
                        EnviarMailingParaFiliais(objFilial, objUsuarioFilialPerfil, objUsuarioFilial, emailRemetente, assunto, template);
                    }
                }
            }

        }
        #endregion EnviarEmailFiliais

        #region ListaFiliaisEnviarEmailParaFiliais
        /// <summary>
        /// Método utilizado por retornar uma lista com todas as filiais que devem ser enviado um email para as filiais da employer
        /// </summary>
        /// <returns>List</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static List<Filial> ListaFiliaisEnviarEmailParaFiliais()
        {
            var quantidadeDias = Parametro.RecuperaValorParametro(Enumeradores.Parametro.EmailFiliaisQuantidadeDiasEnviarPartirDataCadastro);

            var parms = new List<SqlParameter> 
                {
                    new SqlParameter{ ParameterName = "@QuantidadeDias", SqlDbType = SqlDbType.Int, Size = 4, Value = quantidadeDias}
                };

            var lista = new List<Filial>();
            using (var dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spfiliaisenviaremailfiliaisemployer, parms))
            {
                while (dr.Read())
                {
                    var objFilial = new Filial();
                    if (SetInstance_NotDispose(dr, objFilial))
                        lista.Add(objFilial);
                }
            }

            return lista;
        }
        #endregion

        #region EnviarMailing
        /// <summary>
        /// Enviar e-mail de cadastro da empresa para todos os e-mails do grupo pela cidade
        /// </summary>
        /// <param name="objFilial"></param>
        /// <param name="objUsuarioFilial"></param>
        /// <param name="emailRemetenteSistema"></param>
        private static void EnviarMailingParaFiliais(Filial objFilial, UsuarioFilialPerfil objUsuarioFilialPerfil, UsuarioFilial objUsuarioFilial, string emailRemetenteSistema, string assunto, string template)
        {
            //recupera os candidatos a receber o e-mail referente a este cadastro
            if (objFilial.Endereco != null)
            {
                objFilial.Endereco.CompleteObject();

                IEnumerable<GrupoCidade> listGrupoCidade = GrupoCidade.ListarGruposPorCidade(objFilial.Endereco.Cidade);

                foreach (GrupoCidade objGrupoCidade in listGrupoCidade)
                {
                    string mensagem = FormatarMensagemCadastroNovaEmpresaParaFilial(objFilial, objUsuarioFilialPerfil, objUsuarioFilial, objGrupoCidade, template);
                    List<EmailDestinatarioCidade> listEmails = EmailDestinatarioCidade.ListarPorGrupoCidade(objGrupoCidade, null);

                    foreach (EmailDestinatarioCidade objEmailDestinatarioCidade in listEmails)
                    {
                        objEmailDestinatarioCidade.EmailDestinatario.CompleteObject();

                        EmailSenderFactory.Create(TipoEnviadorEmail.Fila).Enviar(assunto, mensagem, emailRemetenteSistema, objEmailDestinatarioCidade.EmailDestinatario.DescricaoEmail);
                    }
                }
            }
        }
        #endregion

        #region FormatarMensagemCadastroNovaEmpresaParaFilial
        private static string FormatarMensagemCadastroNovaEmpresaParaFilial(Filial objFilial, UsuarioFilialPerfil objUsuarioFilialPerfil, UsuarioFilial objUsuarioFilial, GrupoCidade objGrupoCidade, string templateMensagem)
        {
            objFilial.Endereco.Cidade.CompleteObject();
            objFilial.Endereco.Cidade.Estado.CompleteObject();

            string descricaoAtividade;
            string nomeFilial = descricaoAtividade = string.Empty;

            if (objGrupoCidade.Filial != null)
            {
                objGrupoCidade.Filial.CompleteObject();
                nomeFilial = objGrupoCidade.Filial.RazaoSocial;
            }

            string nomeCliente = objFilial.RazaoSocial;
            string emailCliente = objUsuarioFilial.EmailComercial;
            string cnpjCliente = objFilial.NumeroCNPJ.ToString();
            string nomeCidade = objFilial.Endereco.Cidade.NomeCidade;

            //Estado
            string nomeEstado = objFilial.Endereco.Cidade.Estado.NomeEstado;

            string endereco = String.Format("{0}, {1} {4} {2} - {3}", objFilial.Endereco.DescricaoLogradouro, objFilial.Endereco.NumeroEndereco, objFilial.Endereco.DescricaoComplemento,
                objFilial.Endereco.DescricaoBairro, String.IsNullOrEmpty(objFilial.Endereco.DescricaoComplemento) ? string.Empty : "-");

            //Números de contato
            string numeroTelefone = String.Format("({0}) {1}", objFilial.NumeroDDDComercial, objFilial.NumeroComercial);
            //numeroFax = String.Format("({0}) {1}", objFilial.NumeroDDDFax, objFilial.NumeroFax);

            if (objFilial.CNAEPrincipal != null)
            {
                objFilial.CNAEPrincipal.CompleteObject();
                descricaoAtividade = objFilial.CNAEPrincipal.DescricaoCNAESubClasse;
            }

            //Responsável
            string nomeResponsavel = objUsuarioFilialPerfil.PessoaFisica.NomeCompleto;

            //Função
            string descricaoFuncao;
            if (objUsuarioFilial.Funcao != null)
            {
                objUsuarioFilial.Funcao.CompleteObject();
                descricaoFuncao = objUsuarioFilial.Funcao.DescricaoFuncao;
            }
            else
                descricaoFuncao = objUsuarioFilial.DescricaoFuncao;

            string numeroFuncinarios = objFilial.QuantidadeFuncionarios.ToString();
            //periodoMaisContrata = objFilial.MesInicioSazonalidade + "-" + objFilialBNE.MesFimSazonalidade;

            var parametros = new
            {
                NomeFilial = nomeFilial,
                NomeCliente = nomeCliente,
                Email = emailCliente,
                Cnpj = cnpjCliente,
                Cidade = nomeCidade,
                Estado = nomeEstado,
                Endereco = endereco,
                Telefone = numeroTelefone,
                //Fax = numeroFax,
                Atividade = descricaoAtividade,
                Responsavel = nomeResponsavel,
                Funcao = descricaoFuncao,
                NumeroFuncionarios = numeroFuncinarios,
                //PeriodoMaisContrata = periodoMaisContrata
            };

            return parametros.ToString(templateMensagem);
        }
        #endregion

        #region SetInstance_NotDispose
        /// <summary>
        /// Método auxiliar utilizado pelos métodos LoadObject e CompleteObject para percorrer um IDataReader e vincular as colunas com os atributos da classe.
        /// </summary>
        /// <param name="dr">Cursor de leitura do banco de dados.</param>
        /// <param name="objFilial">Instância a ser manipulada.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static bool SetInstance_NotDispose(IDataReader dr, Filial objFilial)
        {
            objFilial._idFilial = Convert.ToInt32(dr["Idf_Filial"]);
            objFilial._flagMatriz = Convert.ToBoolean(dr["Flg_Matriz"]);
            if (dr["Num_CNPJ"] != DBNull.Value)
                objFilial._numeroCNPJ = Convert.ToDecimal(dr["Num_CNPJ"]);
            objFilial._razaoSocial = Convert.ToString(dr["Raz_Social"]);
            objFilial._nomeFantasia = Convert.ToString(dr["Nme_Fantasia"]);
            if (dr["Idf_CNAE_Principal"] != DBNull.Value)
                objFilial._cNAEPrincipal = new CNAESubClasse(Convert.ToInt32(dr["Idf_CNAE_Principal"]));
            if (dr["Idf_Natureza_Juridica"] != DBNull.Value)
                objFilial._naturezaJuridica = new NaturezaJuridica(Convert.ToInt32(dr["Idf_Natureza_Juridica"]));
            objFilial._endereco = new Endereco(Convert.ToInt32(dr["Idf_Endereco"]));
            if (dr["End_Site"] != DBNull.Value)
                objFilial._enderecoSite = Convert.ToString(dr["End_Site"]);
            objFilial._numeroDDDComercial = Convert.ToString(dr["Num_DDD_Comercial"]);
            objFilial._numeroComercial = Convert.ToString(dr["Num_Comercial"]);
            objFilial._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
            objFilial._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
            objFilial._dataAlteracao = Convert.ToDateTime(dr["Dta_Alteracao"]);
            objFilial._quantidadeFuncionarios = Convert.ToInt32(dr["Qtd_Funcionarios"]);
            objFilial._descricaoIP = Convert.ToString(dr["Des_IP"]);
            objFilial._flagOfereceCursos = Convert.ToBoolean(dr["Flg_Oferece_Cursos"]);
            objFilial._situacaoFilial = new SituacaoFilial(Convert.ToInt32(dr["Idf_Situacao_Filial"]));
            if (dr["Des_Pagina_Facebook"] != DBNull.Value)
                objFilial._descricaoPaginaFacebook = Convert.ToString(dr["Des_Pagina_Facebook"]);
            if (dr["Qtd_Usuario_Adicional"] != DBNull.Value)
                objFilial._quantidadeUsuarioAdicional = Convert.ToInt32(dr["Qtd_Usuario_Adicional"]);
            if (dr["Des_Localizacao"] != DBNull.Value)
                objFilial._descricaoLocalizacao = (SqlGeography)dr["Des_Localizacao"];
            if (dr["Idf_Tipo_Parceiro"] != DBNull.Value)
                objFilial._tipoParceiro = new TipoParceiro(Convert.ToInt32(dr["Idf_Tipo_Parceiro"]));

            objFilial._persisted = true;
            objFilial._modified = false;

            return true;
        }
        #endregion

        /// <summary>
        /// Determina se a Flag Enumeradores.Parametro.FilialParceiraWebEstagios está marcada
        /// esta flag é utilizada para clientes do web estágios, ela pode ser marcada no cadastro da empresa quando se tem acesso de admin.
        /// </summary>
        /// <returns></returns>
        public bool AvalWebEstagios()
        {
            if (this.IdFilial <= 0)
                return false;

            ParametroFilial clienteWebEstagio;
            if (ParametroFilial.CarregarParametroPorFilial(Enumeradores.Parametro.FilialParceiraWebEstagios, this,
                                                           out clienteWebEstagio))
            {
                bool valor;
                if (bool.TryParse(clienteWebEstagio.ValorParametro, out valor))
                {
                    return valor;
                }
            }
            return false;
        }

        public static IEnumerable<int> BuscarFiliaisIdModificacaoExportacao(DateTime? fromDate, int idfFilialMin, int take, int skip, SqlTransaction trans = null)
        {
            bool toOpen = trans == null || trans.Connection == null || trans.Connection.State == ConnectionState.Closed || trans.Connection.State == ConnectionState.Broken;
            SqlConnection conn = null;
            try
            {
                if (toOpen)
                {
                    conn = new SqlConnection(DataAccessLayer.CONN_STRING);
                    conn.Open();
                    trans = conn.BeginTransaction(IsolationLevel.ReadUncommitted);
                }

                List<SqlParameter> list;
                string sqlToFormat;
                if (fromDate.HasValue)
                {
                    list = new List<SqlParameter>
                {
                      new SqlParameter { ParameterName = "@Idf_Filial", SqlDbType = SqlDbType.Int, Size = 4, Value = idfFilialMin },
                      new SqlParameter { ParameterName = "@Update_Date", SqlDbType = SqlDbType.DateTime,  Value = fromDate }
                };
                    sqlToFormat = @"
               SELECT TOP {0} *
                    FROM (
                        SELECT one.Idf_Filial, ROW_NUMBER() OVER (ORDER BY one.Idf_Filial ASC) as row_num 
                        FROM (
		                        SELECT DISTINCT fil.Idf_Filial
		                        FROM TAB_Pessoa_Fisica AS pf
		                        INNER JOIN TAB_Usuario_Filial_Perfil usufp ON usufp.Idf_Pessoa_Fisica = pf.Idf_Pessoa_Fisica
		                        INNER JOIN TAB_Filial fil ON usufp.Idf_Filial = fil.Idf_Filial
		                        INNER JOIN BNE_Usuario usu ON usu.Idf_Pessoa_Fisica = pf.Idf_Pessoa_Fisica
                                INNER JOIN BNE_Usuario_Filial usuFi ON usuFi.Idf_Usuario_Filial_Perfil = usufp.Idf_Usuario_Filial_Perfil
		                        WHERE (usu.Dta_Ultima_Atividade > @Update_Date OR pf.Dta_Alteracao > @Update_Date 
		                            OR fil.Dta_Alteracao > @Update_Date) AND fil.Idf_Filial > @Idf_Filial
	                        ) AS one
                        )  AS two
                    WHERE two.row_num > {1}
                    ORDER BY two.Idf_Filial ASC
                ";
                }
                else
                {
                    list = new List<SqlParameter>
                    {
                          new SqlParameter { ParameterName = "@Idf_Filial", SqlDbType = SqlDbType.Int, Size = 4, Value = idfFilialMin }
                    };

                    sqlToFormat = @"
                   SELECT TOP {0} *
                        FROM (
                            SELECT one.Idf_Filial, ROW_NUMBER() OVER (ORDER BY one.Idf_Filial ASC) as row_num 
                            FROM (
		                            SELECT DISTINCT fil.Idf_Filial
		                            FROM TAB_Pessoa_Fisica AS pf
		                            INNER JOIN TAB_Usuario_Filial_Perfil usufp ON usufp.Idf_Pessoa_Fisica = pf.Idf_Pessoa_Fisica
		                            INNER JOIN TAB_Filial fil ON usufp.Idf_Filial = fil.Idf_Filial
		                            INNER JOIN BNE_Usuario usu ON usu.Idf_Pessoa_Fisica = pf.Idf_Pessoa_Fisica
                                    INNER JOIN BNE_Usuario_Filial usuFi ON usuFi.Idf_Usuario_Filial_Perfil = usufp.Idf_Usuario_Filial_Perfil
		                            WHERE fil.Idf_Filial > @Idf_Filial AND fil.Flg_Inativo = 0
	                            ) AS one
                            )  AS two
                        WHERE two.row_num > {1}
                        ORDER BY two.Idf_Filial ASC
                    ";
                }

                string sql = string.Format(sqlToFormat, take, skip);

                using (var reader = DataAccessLayer.ExecuteReader(trans, CommandType.Text, sql, list))
                {
                    while (reader.Read())
                    {
                        yield return Convert.ToInt32(reader["Idf_Filial"]);
                    }
                }
            }
            finally
            {
                if (toOpen)
                {
                    if (trans != null)
                        trans.Dispose();
                    if (conn != null)
                        conn.Dispose();
                }
            }
        }

        public static DTO.AllInFilial CarregaFilialExportacaoAllIn(int idFilial, SqlTransaction trans)
        {
            var objFilial = new DTO.AllInFilial();

            bool toOpen = trans == null || trans.Connection == null || trans.Connection.State == ConnectionState.Closed || trans.Connection.State == ConnectionState.Broken;

            SqlConnection conn = null;
            try
            {
                if (toOpen)
                {
                    conn = new SqlConnection(DataAccessLayer.CONN_STRING);
                    conn.Open();
                    trans = conn.BeginTransaction(IsolationLevel.ReadUncommitted);
                }

                var filial = new BLL.Filial(idFilial);
                if (!filial.CompleteObject(trans))
                    return null;

                if (filial.Endereco != null && filial.Endereco.IdEndereco > 0)
                {
                    if (filial.Endereco.CompleteObject(trans))
                        if (filial.Endereco.Cidade != null && filial.Endereco.Cidade.IdCidade > 0)
                        {
                            if (filial.Endereco.Cidade.CompleteObject(trans))
                                if (filial.Endereco.Cidade.Estado != null && filial.Endereco.Cidade.Estado.IdEstado > 0)
                                {
                                    filial.Endereco.Cidade.Estado.CompleteObject();
                                }
                        }
                }

                if (filial.CNAEPrincipal != null && filial.CNAEPrincipal.IdCNAESubClasse > 0)
                    filial.CNAEPrincipal.CompleteObject(trans);

                if (filial.SituacaoFilial != null && filial.SituacaoFilial.IdSituacaoFilial > 0)
                    filial.SituacaoFilial.CompleteObject(trans);

                if (filial.TipoParceiro != null && filial.TipoParceiro.IdTipoParceiro > 0)
                    filial.TipoParceiro.CompleteObject(trans);

                ParametroFilial contrataEstag;
                bool usaWebEstagios;
                if (ParametroFilial.CarregarParametroPorFilial(Enumeradores.Parametro.FilialContrataEstagiario, filial,
                        out contrataEstag, trans) && bool.TryParse(contrataEstag.ValorParametro, out usaWebEstagios))
                {
                    objFilial.AceitaEstag = usaWebEstagios;
                }

                ParametroFilial autorizoPublicarParamFilial;
                bool autorizacaoPublicacaoVagas;
                if (ParametroFilial.CarregarParametroPorFilial(Enumeradores.Parametro.AutorizoBNEPublicarVagas, filial,
                    out autorizoPublicarParamFilial, trans) &&
                    bool.TryParse(autorizoPublicarParamFilial.ValorParametro, out autorizacaoPublicacaoVagas))
                {
                    objFilial.PublicaVaga = autorizacaoPublicacaoVagas;
                }

                objFilial.Filial = filial;
                objFilial.Plano = PlanoAdquirido.ExistePlanoAdquiridoLiberadoPorFilial(filial, trans);

                var plano = PlanoAdquirido.CarregarUltimoPlanoPessoaJuridica(filial.IdFilial, trans);
                if (plano != null)
                {
                    objFilial.PlanoInicio = plano.DataInicioPlano;
                    objFilial.PlanoFim = plano.DataFimPlano;
                }

                var vaga = Vaga.UltimaVagaFilialPerfil(filial.IdFilial, trans);
                if (vaga != null)
                {
                    if (vaga.Cidade != null && vaga.Cidade.IdCidade > 0)
                    {
                        if (vaga.Cidade.CompleteObject(trans))
                            if (vaga.Cidade.Estado != null && vaga.Cidade.Estado.IdEstado > 0)
                            {
                                vaga.Cidade.Estado.CompleteObject();
                            }
                    }
                    objFilial.UltimaVagaDados = vaga;
                    var tipoVinculos = VagaTipoVinculo.ListarTipoVinculoPorVaga(vaga.IdVaga);
                    if (tipoVinculos != null && tipoVinculos.Count > 0)
                        objFilial.UltimaVagaVinculos =
                            tipoVinculos
                                .Select(a => a.TipoVinculo)
                                .Where(a => a.CompleteObject(trans))
                                .ToArray();
                }
                int totalRegistros;
                var result = UsuarioFilialPerfil.CarregarUsuariosCadastradosPorFilial(1, 99, filial.IdFilial, (int)Enumeradores.Perfil.AcessoEmpresaMaster,
                    filial.IdFilial, true, out totalRegistros, trans);

                var listPerf = new List<UsuarioFilialPerfil>();
                foreach (DataRow item in result.Rows)
                {
                    var idUsuFilialPerf = Convert.ToInt32(item["Idf_Usuario_Filial_Perfil"].ToString());

                    var usuFilialPerf = new UsuarioFilialPerfil(idUsuFilialPerf);
                    usuFilialPerf.CompleteObject(trans);

                    listPerf.Add(usuFilialPerf);

                    if (usuFilialPerf.PessoaFisica != null && usuFilialPerf.PessoaFisica.IdPessoaFisica > 0)
                    {
                        usuFilialPerf.PessoaFisica.CompleteObject(trans);
                    }

                    if (usuFilialPerf.Perfil != null && usuFilialPerf.Perfil.IdPerfil > 0)
                    {
                        usuFilialPerf.Perfil.CompleteObject(trans);
                    }

                    UsuarioFilial usuFilial;
                    if (UsuarioFilial.CarregarUsuarioFilialPorUsuarioFilialPerfil(usuFilialPerf.IdUsuarioFilialPerfil,
                        out usuFilial, trans))
                    {
                        objFilial.UsuariosFilial[usuFilialPerf] = usuFilial;
                    }

                    var idPf = usuFilialPerf.PessoaFisica != null ? usuFilialPerf.PessoaFisica.IdPessoaFisica : -1;
                    Usuario usu;
                    if (Usuario.CarregarPorPessoaFisica(idPf, out usu, trans))
                    {
                        objFilial.Interacoes[usuFilialPerf] = usu;
                    }

                    PesquisaCurriculo pesq;
                    if (PesquisaCurriculo.RecuperarUltimaPesquisa(idPf, out pesq, trans))
                    {
                        objFilial.Pesquisas[usuFilialPerf] = pesq;
                    }
                }

                objFilial.Perfis = listPerf.ToArray();
            }
            finally
            {
                if (toOpen)
                {
                    if (trans != null)
                        trans.Dispose();

                    if (conn != null)
                        conn.Dispose();
                }
            }

            return objFilial;
        }

        #region CarregarGrupoDestinatarioFiliaisEmployer
        /// <summary>
        /// Método utilizado para retornar os grupos e destinatarios das filiais Employer (BNE).
        /// </summary>
        /// <returns>Relatório com dados das Filiais</returns>
        /// <remarks>Fabiano Charan</remarks>
        public static DataTable CarregarGrupoDestinatarioFiliaisEmployer()
        {
            return DataAccessLayer.ExecuteReaderDs(CommandType.Text, SPSELECTEMAILDESTINATARIOFILIALEMPLOYER, null).Tables[0];
        }
        #endregion

        #region CarregarCidadesGrupoFiliaisEmployer
        /// <summary>
        /// Método utilizado para retornar as cidades de um Grupo Cidade.
        /// </summary>
        /// <returns>DataTable com ids das cidades</returns>
        /// <remarks>Fabiano Charan</remarks>
        public static DataTable CarregarCidadesGrupoFiliaisEmployer(int idGrupoCidade)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Grupo_Cidade", SqlDbType.Int, 4));

            parms[0].Value = idGrupoCidade;

            return DataAccessLayer.ExecuteReaderDs(CommandType.Text, SPSELECTCIDADEGRUPOFILIALEMPLOYER, parms).Tables[0];
        }
        #endregion

        #region CarregarCidadesGrupoFiliaisEmployer
        /// <summary>
        /// Método utilizado para retornar as dados do relatório mensal para a filial.
        /// </summary>
        /// <returns>DataTable com os dados da região</returns>
        /// <remarks>Fabiano Charan</remarks>
        public static DataTable CarregarDadosRelatorioMensalFilialEmployer(DateTime DataInicio, DateTime DataFim, string idsCidades)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Data_Inicial", SqlDbType.DateTime, 8));
            parms.Add(new SqlParameter("@Data_Fim", SqlDbType.DateTime, 8));

            parms[0].Value = DataInicio;
            parms[1].Value = DataFim;

            return DataAccessLayer.ExecuteReaderDs(CommandType.Text, string.Format(SPSELECTRELATORIOMENSALFILIALEMPLOYER, idsCidades), parms).Tables[0];
        }
        #endregion

        #endregion
    }
}