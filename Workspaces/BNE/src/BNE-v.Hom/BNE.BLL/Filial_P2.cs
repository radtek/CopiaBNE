//-- Data: 30/11/2011 11:35
//-- Autor: Jhonatan Taborda

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
using System.Threading.Tasks;
using BNE.BLL.DTO;
using System.ComponentModel.DataAnnotations;
using FormatObject = BNE.BLL.Common.FormatObject;
using BNE.BLL.Mensagem.DTO;
using System.Threading;

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

        #region RazaoSocial
        /// <summary>
        /// Tamanho do campo: 100.
        /// Campo obrigatório.
        /// </summary>
        public string RazaoSocial
        {
            get
            {
                if (String.IsNullOrEmpty(this._razaoSocial))
                {
                    this._razaoSocial = this.RecuperarRazaoSocial();
                }
                return this._razaoSocial;
            }
            set
            {
                this._razaoSocial = value;
                this._modified = true;
            }
        }
        #endregion

        #region SituacaoFilial
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public SituacaoFilial SituacaoFilial
        {
            get
            {
                if (this._situacaoFilial == null)
                    this._situacaoFilial = RecuperarSituacaoFilial();
                return this._situacaoFilial;
            }
            set
            {
                this._situacaoFilial = value;
                this._modified = true;
            }
        }
        #endregion

        #region DataCadastro
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public DateTime DataCadastro
        {
            get
            {
                return this._dataCadastro;
            }
            set
            {
                this._dataCadastro = value;
                this._modified = true;
            }
        }
        #endregion

        #region DataAlteracao
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        [Display(Name = "IgnoreData")]
        public DateTime DataAlteracao
        {
            get
            {
                return this._dataAlteracao;
            }
            set
            {
                this._dataAlteracao = value;
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
                        Sig_Estado,
			            F.Dta_Cadastro '
        
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
		            LEFT JOIN bne.bne_curriculo_nao_visivel_filial cvf WITH(NOLOCK) on cvf.idf_filial = f.idf_filial and cvf.idf_curriculo = '+ Convert(varchar,@Idf_Curriculo) +'
                WHERE   F.Flg_Inativo = 0
                        AND F.Idf_Situacao_Filial = 1
                        AND F.Idf_Filial <> 182089 --Idf da vaga rapida igual em hom e prd
			            and cvf.idf_filial is null '

                                                                       
                IF (@Razao IS NOT NULL )
                    SET @iSelect = @iSelect + ' AND F.Raz_Social LIKE ''%' + @Razao + '%'''

                IF (@Ramo IS NOT NULL )
                    SET @iSelect = @iSelect + ' AND DES_CNAE_SUB_Classe LIKE ''%' + @Ramo + '%'''

                IF (@Cidade IS NOT NULL )
                    SET @iSelect = @iSelect + ' AND Cid.Nme_Cidade LIKE ''%' + @Cidade + '%'''

                IF (@Estado IS NOT NULL )
                    SET @iSelect = @iSelect + ' AND Sig_Estado LIKE ''' + @Estado + ''''

            SET @iSelectCount = 'Select Count(*) From ( ' + @iSelect  + ' ) As TblTempCount'

            SET @iSelectPag = 'Select * From ( ' + @iSelect + ' ) As TblTempPag	Where RowID > ' + CONVERT(VARCHAR, @FirstRec)  + ' And RowID < ' + CONVERT(VARCHAR, @LastRec)
            SET @iSelectPag = @iSelectPag + ' ORDER BY Dta_Cadastro DESC' 
            EXECUTE (@iSelectCount)
            EXECUTE (@iSelectPag)
        ";
        #endregion

        #region SPFILIAISDESATUALIZADAS
        private const string SPFILIAISDESATUALIZADAS = @"
        SELECT PF.Eml_Pessoa, UFP.Idf_Usuario_Filial_Perfil, F.Idf_Filial, F.Num_CNPJ, PF.Nme_Pessoa
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
        SET @ParmDefinition = N'@Filtro VARCHAR(100), @Num_CNPJ DECIMAL, @Telefone VARCHAR(12), @Email VARCHAR(40)';
      
        SET @iSelect = '
        SELECT ROW_NUMBER() OVER (ORDER BY idf_situacao_filial desc, Dta_Cadastro DESC) AS RowID,
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
                FROM Tab_Filial F WITH(NOLOCK)
                LEFT JOIN TAB_Usuario_Filial_Perfil ufp WITH(NOLOCK)  ON F.Idf_Filial = ufp.Idf_Filial
                LEFT JOIN bne_usuario_filial uf WITH(NOLOCK) ON uf.idf_Usuario_Filial_Perfil = ufp.idf_Usuario_Filial_Perfil
                LEFT JOIN TAB_Endereco E WITH(NOLOCK) ON ( E.Idf_Endereco = F.Idf_Endereco )
                LEFT JOIN plataforma.TAB_Cidade cid WITH(NOLOCK) ON ( cid.Idf_Cidade = E.Idf_Cidade )
                JOIN TAB_Filial_Fulltext ff WITH ( NOLOCK ) ON F.Idf_Filial = ff.Idf_Filial
        WHERE F.Flg_Inativo = 0 '
 
        IF ( @AguardandoPublicacao IS NOT NULL
             AND @AguardandoPublicacao = 1
           ) 
            BEGIN
                SET @iSelect = @iSelect + ' AND ( f.Idf_Situacao_Filial = 3 OR f.Idf_Situacao_Filial = 7 OR F.idf_situacao_filial = 9 ) '
            END
 
        IF ( @Num_CNPJ IS NOT NULL ) 
            BEGIN
                SET @iSelect = @iSelect + ' AND F.Num_CNPJ = @Num_CNPJ '
            END
      
        IF ( @Telefone IS NOT NULL ) 
            BEGIN
                SET @iSelect = @iSelect + ' AND F.Num_Comercial LIKE @Telefone'
            END
 
        IF ( @Email IS NOT NULL ) 
            BEGIN
                SET @iSelect = @iSelect + ' AND uf.Eml_comercial LIKE @Email'
            END    
      
        IF ( @Filtro IS NOT NULL ) 
            BEGIN
                SET @iSelect = @iSelect + ' AND CONTAINS(ff.Des_MetaBusca, ''' + [BNE].BNE_BuscaMontaFT(@Filtro) + ''')'
            END    
 
        SET @iSelect = @iSelect + ' GROUP BY F.Idf_Filial, F.Raz_Social, Num_CNPJ, F.Idf_Situacao_Filial, F.Dta_Cadastro, FF.Nme_Cidade, FF.Sig_Estado) AS temp '
 
        SET @iSelectCount = 'Select Count(*) From ( ' + @iSelect + ' ) As TblTempCount'
        SET @iSelectPag = 'Select * From ( ' + @iSelect + ' ) As TblTempPag   Where RowID >= ' + CONVERT(VARCHAR, @FirstRec) + ' And RowID <= ' + CONVERT(VARCHAR, @LastRec)
      
        EXEC sp_executesql @iSelectCount, @ParmDefinition, @Filtro = @Filtro, @Num_CNPJ = @Num_CNPJ, @Telefone = @Telefone, @Email = @Email
        EXEC sp_executesql @iSelectPag, @ParmDefinition, @Filtro = @Filtro, @Num_CNPJ = @Num_CNPJ, @Telefone = @Telefone, @Email = @Email";
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

        #region SP_SELECT_RAZAO_SOCIAL
        private const string SP_SELECT_RAZAO_SOCIAL = @"  
        SELECT  Raz_Social
        FROM    TAB_Filial F WITH(NOLOCK)
        WHERE   Idf_Filial = @Idf_Filial";
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

        #region SpCarregarUsuarioResponsavel
        private const string SpCarregarUsuarioResponsavel = @"
        select pf.Nme_Pessoa
	        , pf.Num_CPF
            , uf.Eml_Comercial
        from bne.tab_usuario_filial_perfil ufp with (nolock)
	        join bne.tab_pessoa_fisica pf with (nolock) on ufp.Idf_Pessoa_Fisica = pf.Idf_Pessoa_Fisica
            join bne.bne_usuario_filial uf with (nolock) on ufp.Idf_Usuario_Filial_Perfil = uf.Idf_Usuario_Filial_Perfil
        where ufp.Idf_Filial = @Idf_Filial AND UFP.Flg_Usuario_Responsavel = 1
        ";
        #endregion

        #region SpCarregarUsuarioMaster
        private const string SpCarregarUsuarioMaster = @"
            select TOP 1
	            pf.Nme_Pessoa
                , uf.Eml_Comercial
	            , en.Des_Logradouro
	            , en.Num_Endereco
	            , ci.Nme_Cidade
	            , ci.Sig_Estado
            from bne.tab_usuario_filial_perfil ufp with (nolock)
	            join BNE.tab_pessoa_fisica pf with (nolock) on ufp.Idf_Pessoa_Fisica = pf.Idf_Pessoa_Fisica
                join BNE.bne_usuario_filial uf with (nolock) on ufp.Idf_Usuario_Filial_Perfil = uf.Idf_Usuario_Filial_Perfil
	            JOIN BNE.TAB_Filial fil WITH(NOLOCK) ON ufp.Idf_Filial = fil.Idf_Filial
	            join BNE.tab_endereco en with (nolock) on fil.Idf_Endereco = en.Idf_Endereco
	            join plataforma.tab_cidade ci with (nolock) on en.Idf_Cidade = ci.Idf_Cidade
            where 1 = 1
                AND ufp.Idf_Filial = @Idf_Filial
	            AND ufp.Flg_Inativo = 0
	            AND pf.Flg_Inativo = 0
                AND Idf_Perfil = 4
            ORDER BY PF.Dta_Cadastro desc
        ";
        #endregion


        #region SpRecuperarInformacoesIntegracaoFinanceiro
        private const string SpRecuperarInformacoesIntegracaoFinanceiro = @"
         SELECT pp.Dta_Pagamento ,
        pg.Vlr_Pagamento ,
        pg.Des_Identificador ,
        pa.Dta_Inicio_Plano ,
        pa.Dta_Fim_Plano ,
        fl_gestora.Nme_Fantasia AS Filial_Gestora ,
        fl.Num_CNPJ ,
        fl.Raz_Social ,
        en.Num_CEP ,
        en.Des_Logradouro ,
        en.Num_Endereco ,
        en.Des_Complemento ,
        en.Des_Bairro ,
        ci.Nme_Cidade ,
        ci.Sig_Estado ,
        cnae.Cod_CNAE_Sub_Classe ,
        fl.Num_DDD_Comercial ,
        fl.Num_Comercial ,
        fl.Nme_Fantasia ,
        pad.Nme_Res_Plano_Adquirido ,
        (CASE WHEN pp.Eml_Envio_Boleto IS NULL
                  OR pp.Eml_Envio_Boleto = '' THEN pad.Eml_Envio_Boleto
             ELSE pp.Eml_Envio_Boleto
        END) AS Eml_Envio_Boleto ,
        trans_num_banco.Idf_Banco
 FROM   BNE.BNE_Pagamento pg WITH ( NOLOCK )
        OUTER APPLY ( SELECT TOP 1
                                trans.Idf_Banco
                      FROM      BNE.BNE_Transacao trans WITH ( NOLOCK )
                      WHERE     trans.Idf_Pagamento = pg.Idf_Pagamento
                      ORDER BY  trans.Dta_Cadastro
                    ) trans_num_banco
        JOIN BNE.BNE_Plano_Parcela pp WITH ( NOLOCK ) ON pg.Idf_Plano_Parcela = pp.Idf_Plano_Parcela
        JOIN BNE.BNE_Plano_Adquirido pa WITH ( NOLOCK ) ON pp.Idf_Plano_Adquirido = pa.Idf_Plano_Adquirido
        LEFT JOIN BNE.BNE_Plano_Adquirido_Detalhes pad WITH ( NOLOCK ) ON pad.Idf_Plano_Adquirido = pa.Idf_Plano_Adquirido
        JOIN BNE.TAB_Filial fl WITH ( NOLOCK ) ON pa.Idf_Filial = fl.Idf_Filial
        LEFT JOIN BNE.TAB_Filial fl_gestora WITH ( NOLOCK ) ON pad.Idf_Filial_Gestora = fl_gestora.Idf_Filial
        LEFT JOIN plataforma.TAB_CNAE_Sub_Classe cnae WITH ( NOLOCK ) ON fl.Idf_CNAE_Principal = cnae.Idf_CNAE_Sub_Classe
        JOIN BNE.TAB_Endereco en WITH ( NOLOCK ) ON fl.Idf_Endereco = en.Idf_Endereco
        JOIN plataforma.TAB_Cidade ci WITH ( NOLOCK ) ON en.Idf_Cidade = ci.Idf_Cidade
 WHERE  pg.Idf_Pagamento = @Idf_Pagamento
 UNION
 SELECT ap.Dta_Alteracao AS Dta_Pagamento ,
        pg.Vlr_Pagamento ,
        pg.Des_Identificador ,
        pa.Dta_Inicio_Plano ,
        pa.Dta_Fim_Plano ,
        fl_gestora.Nme_Fantasia AS Filial_Gestora ,
        fl.Num_CNPJ ,
        fl.Raz_Social ,
        en.Num_CEP ,
        en.Des_Logradouro ,
        en.Num_Endereco ,
        en.Des_Complemento ,
        en.Des_Bairro ,
        ci.Nme_Cidade ,
        ci.Sig_Estado ,
        cnae.Cod_CNAE_Sub_Classe ,
        fl.Num_DDD_Comercial ,
        fl.Num_Comercial ,
        fl.Nme_Fantasia ,
        pad.Nme_Res_Plano_Adquirido ,
        (CASE WHEN PP.Eml_Envio_Boleto IS NULL
                  OR PP.Eml_Envio_Boleto = '' THEN pad.Eml_Envio_Boleto
             ELSE PP.Eml_Envio_Boleto
        END) AS Eml_Envio_Boleto ,
        trans_num_banco.Idf_Banco
 FROM   BNE.BNE_Pagamento pg WITH ( NOLOCK )
        OUTER APPLY ( SELECT TOP 1
                                trans.Idf_Banco
                      FROM      BNE.BNE_Transacao trans WITH ( NOLOCK )
                      WHERE     trans.Idf_Pagamento = pg.Idf_Pagamento
                      ORDER BY  trans.Dta_Cadastro
                    ) trans_num_banco
        JOIN BNE.BNE_Adicional_Plano ap WITH ( NOLOCK ) ON ap.Idf_Adicional_Plano = pg.Idf_Adicional_Plano
        JOIN BNE.BNE_Plano_Adquirido pa WITH ( NOLOCK ) ON pa.Idf_Plano_Adquirido = ap.Idf_Plano_Adquirido
        LEFT JOIN BNE.BNE_Plano_Adquirido_Detalhes pad WITH ( NOLOCK ) ON pad.Idf_Plano_Adquirido = pa.Idf_Plano_Adquirido
        JOIN BNE.TAB_Filial fl WITH ( NOLOCK ) ON pa.Idf_Filial = fl.Idf_Filial
        LEFT JOIN BNE.BNE_Plano_Parcela PP ON PP.Idf_Plano_Adquirido = pa.Idf_Plano_Adquirido
        LEFT JOIN BNE.TAB_Filial fl_gestora WITH ( NOLOCK ) ON pad.Idf_Filial_Gestora = fl_gestora.Idf_Filial
        LEFT JOIN plataforma.TAB_CNAE_Sub_Classe cnae WITH ( NOLOCK ) ON fl.Idf_CNAE_Principal = cnae.Idf_CNAE_Sub_Classe
        JOIN BNE.TAB_Endereco en WITH ( NOLOCK ) ON fl.Idf_Endereco = en.Idf_Endereco
        JOIN plataforma.TAB_Cidade ci WITH ( NOLOCK ) ON en.Idf_Cidade = ci.Idf_Cidade
 WHERE  pg.Idf_Pagamento = @Idf_Pagamento;";
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
        SELECT  Idf_Filial AS Idf_Filial ,
                Nme_Fantasia + ' (' + CAST(Num_CNPJ AS VARCHAR) + ')' AS Des_Filial
        FROM    BNE.TAB_Filial
        WHERE   ( Nme_Fantasia LIKE '%Employer%'
                  OR Num_CNPJ LIKE '82344425%'
		          OR Num_CNPJ = 06138623000101
                )
                AND Flg_Inativo = 0
                AND Idf_Situacao_Filial NOT IN ( 5, 6 )
        ORDER BY 1";
        #endregion

        #region AreaBne
        private const string spAreaBne = @"SELECT Des_Area_BNE
                 FROM  BNE.TAB_Filial f
                JOIN plataforma.TAB_CNAE_Sub_Classe c WITH (NOLOCK) ON f.Idf_CNAE_Principal = c.Idf_CNAE_Sub_Classe
                JOIN plataforma.TAB_CNAE_Classe cl WITH (NOLOCK) ON c.Idf_CNAE_Classe = cl.Idf_CNAE_Classe
                JOIN plataforma.TAB_CNAE_Grupo gp WITH (NOLOCK) ON cl.Idf_CNAE_Grupo = gp.Idf_CNAE_Grupo
                JOIN plataforma.TAB_CNAE_Divisao d WITH (NOLOCK) ON gp.Idf_CNAE_Divisao = d.Idf_CNAE_Divisao
                JOIN plataforma.TAB_Area_BNE area WITH (NOLOCK) ON d.Idf_Area_BNE = area.Idf_Area_BNE
                WHERE Idf_Filial = @idf_Filial";
        #endregion

        #region [ SELECTFILIALCARRINHOABANDONADO ]
        private const string SELECTFILIALCARRINHOABANDONADO = @"SELECT lm.*, plano.* ,
    Nme_Vendedor,
    Eml_Vendedor    
FROM    BNE.TAB_Filial f WITH ( NOLOCK )
    JOIN DW_CRM2012.dbo.CRM_Vendedor_Empresa ve WITH ( NOLOCK ) ON ve.Num_CNPJ = f.Num_CNPJ
                                AND GETDATE() BETWEEN ve.Dta_Inicio AND ve.Dta_Fim
    JOIN DW_CRM2012.dbo.CRM_Vendedor v WITH ( NOLOCK ) ON ve.Num_CPF = v.Num_CPF
    CROSS APPLY ( SELECT TOP 1
                Idf_Plano_Adquirido ,
                Eml_Comercial ,
                pa.Idf_Usuario_Filial_Perfil ,
                pa.Dta_Cadastro AS data ,
                pf.Nme_Pessoa
            FROM      BNE.BNE_Plano_Adquirido pa WITH ( NOLOCK )
                JOIN BNE.TAB_Usuario_Filial_Perfil ufp WITH ( NOLOCK ) ON pa.Idf_Usuario_Filial_Perfil = ufp.Idf_Usuario_Filial_Perfil
                JOIN BNE.BNE_Usuario_Filial usu WITH ( NOLOCK ) ON ufp.Idf_Usuario_Filial_Perfil = usu.Idf_Usuario_Filial_Perfil
                JOIN BNE.TAB_Pessoa_Fisica AS pf ON pf.Idf_Pessoa_Fisica = ufp.Idf_Pessoa_Fisica
            WHERE     f.Idf_Filial = pa.Idf_Filial
                AND Idf_Plano_Situacao = 0
                AND CONVERT(DATE, pa.Dta_Cadastro) <= CONVERT(DATE, DATEADD(DAY,
                                -3, GETDATE()))
            ORDER BY  pa.Dta_Cadastro DESC
          ) plano
    LEFT JOIN BNE.BNE_Plano_Adquirido pAtivo WITH ( NOLOCK ) ON f.Idf_Filial = pAtivo.Idf_Filial
                                AND pAtivo.Idf_Plano_Situacao = 1
    LEFT JOIN DW_CRM2012.dbo.CRM_Fato fa WITH ( NOLOCK ) ON f.Num_CNPJ = fa.Num_CNPJ
                                AND Idf_Atendimento IS NOT NULL
                                AND Convert(Date,fa.Dta_Cadastro) >= Convert(Date,plano.data)
	LEFT JOIN BNE_Log_Envio_Mensagem lm with(nolock) on lm.eml_destinatario = plano.Eml_Comercial
								AND lm.idf_carta_email = 98 --CIACarrinhoAbandonado
WHERE  fa.Idf_Fato IS NULL
    AND pAtivo.Idf_Plano_Adquirido IS NULL
	AND f.Idf_Situacao_Filial IN ( 1, 2, 3, 4, 7 )
	AND lm.idf_log_Envio_mensagem is null ";
        #endregion

        #region [spSelectFiliaisSTC]
        private const string spSelectFiliaisSTC = @"select o.des_url, orf.idf_filial, f.num_cnpj,
		pf.num_cpf, pf.dta_nascimento, o.idf_origem --, usuEmp.dta_nascimento
		, uf.eml_comercial
                    from bne.tab_origem o with(nolock)
                    join bne.TAB_Origem_Filial orf with(nolock) on orf.idf_origem = o.idf_origem
                    join bne.tab_filial f with(nolock) on f.idf_filial = orf.idf_filial
					cross apply (
						select top 1 ufp.idf_pessoa_fisica, ufp.idf_usuario_filial_perfil from bne.tab_usuario_filial_perfil ufp with(nolock) 
						 where ufp.idf_perfil = 4 --master
						   and  ufp.idf_filial = f.Idf_filial) as usuEmp
				    join bne.tab_pessoa_fisica pf with(nolock) on pf.idf_pessoa_fisica = usuEmp.idf_pessoa_fisica
					JOIN bne.bne_usuario_filial uf WITH ( NOLOCK ) ON uf.idf_usuario_filial_perfil = usuEmp.idf_usuario_filial_perfil
					where o.idf_origem not in(1,2,3) --bne - sine - bne novo
                    and f.idf_situacao_filial not in(5,6,9)
					and o.des_url is not null ";
        #endregion

        #region [spCvsCadastrosSTC]
        private const string spCvsCadastrosSTC = @"select distinct top 5 cv.idf_curriculo, pf.nme_pessoa, pf.dta_nascimento, cid.Nme_Cidade, cid.Sig_Estado,
ende.Des_Bairro, esc.Des_BNE
                from bne.bne_curriculo_origem co with(nolock)
                join bne.tab_origem o with(nolock) on o.idf_origem = co.idf_origem
                join bne.TAB_Origem_Filial orf with(nolock) on orf.idf_origem = o.idf_origem
				join bne.bne_curriculo cv with(nolock) on cv.idf_curriculo = co.Idf_Curriculo
				join bne.tab_pessoa_Fisica pf with(nolock) on pf.idf_pessoa_fisica = cv.idf_pessoa_Fisica
				join plataforma.tab_cidade  cid with(nolock) on cid.idf_cidade = pf.Idf_Cidade
				left join bne.TAB_Endereco ende with(nolock) on pf.idf_endereco = ende.Idf_Endereco
				left join plataforma.tab_escolaridade esc with(nolock) on esc.Idf_Escolaridade = pf.Idf_Escolaridade
  where orf.idf_filial = @Idf_filial and co.dta_cadastro > (getdate() - 30)";
        #endregion

        #region [spColocarEmailVagasBronquinha]
        private const string spColocarEmailVagasBronquinha = @"select distinct eml_vaga from bne.BNE_Vaga 
                                                                with(nolock) where idf_filial = @Idf_Filial
                                                                and Eml_Vaga is not null";
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

        #region SalvarInformacoesEmpresa
        public void SalvarInformacoesEmpresa(int idUsuarioFilialPerfilLogado, List<CompareObject.CompareResult> listaAlteracao)
        {
            if (listaAlteracao != null && listaAlteracao.Any())
            {
                using (SqlConnection conn = new SqlConnection(DataAccessLayer.CONN_STRING))
                {
                    conn.Open();

                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            string alteracoes = listaAlteracao.Aggregate("Os seguintes campos foram alterados:  <br/>", (current, objAlteracao) => current + (objAlteracao.ToString() + " <br/> "));
                            FilialObservacao.SalvarCRM(alteracoes, this, UsuarioFilialPerfil.LoadObject(idUsuarioFilialPerfilLogado), trans);
                            this.Save(trans);

                            if (Convert.ToBoolean(Parametro.RecuperaValorParametro(Enumeradores.Parametro.MapearParaONovo)))
                            {
                                try
                                {
                                    new Custom.Mapper.PessoaJuridica().Salvar(this, UsuarioFilialPerfil.ListarUsuariosFilial(this, trans), trans);
                                }
                                catch (Exception ex)
                                {
                                    EL.GerenciadorException.GravarExcecao(ex, "MAPEAMENTO");
                                }
                            }

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

                        #region [Remover/Retornar vagas de acordo com a situação filial]
                        BLL.Filial objFilial = BLL.Filial.LoadObject(this.IdFilial);
                        if (objFilial.EmpresaBloqueada() && !EmpresaBloqueada())
                            SituacaoVagasFilial(this.IdFilial, false);
                        else if (!objFilial.EmpresaBloqueada() && EmpresaBloqueada())
                            SituacaoVagasFilial(this.IdFilial, true);
                        #endregion

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

                        if (novo)
                        {
                            var objParametroSaldoCriacaoCampanha = new ParametroFilial
                            {
                                IdParametro = (int)Enumeradores.Parametro.CampanhaRecrutamentoQuantidadeSaldoEnvioCampanha,
                                IdFilial = this.IdFilial,
                                ValorParametro = Parametro.RecuperaValorParametro(Enumeradores.Parametro.CampanhaRecrutamentoQuantidadeSaldoEnvioCampanha),
                                FlagInativo = false
                            };
                            objParametroSaldoCriacaoCampanha.Save(trans);
                        }

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
                        ParametroFilial.CarregarParametroPorFilial(Enumeradores.Parametro.EnvioCartaEmailEmpresaLiberada, this, out objParamFilialCartaEmpresaLiberada, trans);

                        if (objParamFilialCartaEmpresaLiberada == null && (empresaAuditada && !VerificaAuditoria(this.IdFilial)))
                        {
                            string assunto;
                            string template = CartaEmail.RetornarConteudoBNE(Enumeradores.CartaEmail.EmpresaLiberada, out assunto);

                            string link = String.Format("http://{0}/logar/{1}", Helper.RecuperarURLAmbiente(), LoginAutomatico.GerarHashAcessoLogin(objPessoaFisica.CPF, objPessoaFisica.DataNascimento, "/" + Rota.RecuperarURLRota(Enumeradores.RouteCollection.SalaSelecionador)));
                            var parametros = new
                            {
                                Nome = objPessoaFisica.NomePessoa,
                                NomeEmpresa = RazaoSocial,
                                Link = link

                            };
                            string mensagem = FormatObject.ToString(parametros, template);

                            string emailRemetente = Parametro.RecuperaValorParametro(Enumeradores.Parametro.EmailMensagens);

                            EmailSenderFactory
                                .Create(TipoEnviadorEmail.Fila)
                                .Enviar(null, null, objUsuarioFilialPerfil, assunto, mensagem, Enumeradores.CartaEmail.EmpresaLiberada, emailRemetente, objUsuarioFilial.EmailComercial, trans);

                            //Grava Vlr_Parametro de carta enviada para EmpresaLiberada (idf_Carta_Email)
                            objParamFilialCartaEmpresaLiberada = new ParametroFilial
                            {
                                IdParametro = (int)Enumeradores.Parametro.EnvioCartaEmailEmpresaLiberada,
                                IdFilial = this.IdFilial,
                                ValorParametro = ((int)Enumeradores.CartaEmail.EmpresaLiberada).ToString(),
                                FlagInativo = false
                            };
                            objParamFilialCartaEmpresaLiberada.Save(trans);

                            #region EnvioSMS

                            string idUFPRemetente = Parametro.RecuperaValorParametro(Enumeradores.Parametro.IdfUfpEnvioSMSTanqueCadastroEmpresa);
                            //int idUFPDestino = objUsuarioFilialPerfil.IdUsuarioFilialPerfil;

                            string smsUm = ConteudoHTML.RecuperaValorConteudo(Enumeradores.ConteudoHTML.SMSBoasVindasUm);
                            smsUm = smsUm.Replace("{Nome_Usuario}", objPessoaFisica.PrimeiroNome);

                            List<DestinatarioSMS> listaUsuarios = new List<DestinatarioSMS>();

                            var objUsuarioEnvioSMS = new DestinatarioSMS
                            {
                                DDDCelular = objPessoaFisica.NumeroDDDCelular,
                                NumeroCelular = objPessoaFisica.NumeroCelular,
                                NomePessoa = objPessoaFisica.NomePessoa,
                                Mensagem = smsUm,
                                IdDestinatario = objUsuarioFilialPerfil.IdUsuarioFilialPerfil
                            };

                            listaUsuarios.Add(objUsuarioEnvioSMS);

                            MensagemCS.EnvioSMSTanque(idUFPRemetente, listaUsuarios);

                            #endregion

                            //Se a empresa foi auditada agora ativa todas as vagas da empresa e dispara o rastreador
                            var listaVagas = Vaga.ListarVagasFilialEmAuditoria(this, trans);
                            foreach (var objVaga in listaVagas)
                            {
                                objVaga.FlagInativo = false;
                                objVaga.Save(trans, idUsuarioFilialPerfilLogado, Enumeradores.VagaLog.EmpresaAuditadaAtivaTodasVagasDaEmpresaDisparaRastreador);
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

                        if (Convert.ToBoolean(Parametro.RecuperaValorParametro(Enumeradores.Parametro.MapearParaONovo)))
                        {
                            try
                            {
                                new Custom.Mapper.PessoaJuridica().Salvar(this, UsuarioFilialPerfil.ListarUsuariosFilial(this, trans), trans);
                            }
                            catch (Exception ex)
                            {
                                EL.GerenciadorException.GravarExcecao(ex, "MAPEAMENTO");
                            }
                        }

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
            string mensagemEmail = FormatObject.ToString(parametros, conteudo);

            string emailRemetenteSistema = Parametro.RecuperaValorParametro(Enumeradores.Parametro.EmailMensagens);

            foreach (var item in emailLst.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
            {
                EmailSenderFactory.Create(TipoEnviadorEmail.Fila)
                          .Enviar(assunto, mensagemEmail, Enumeradores.CartaEmail.ConteudoWebEstagiosIntegracaoCadastroEmpresa, emailRemetenteSistema, item);
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
        public static DataTable ListarFilialPorRazaoRamoCidade(string cidade, string ramoAtividade, string nomeEmpresa, int? idCurriculo, int paginaCorrente, int tamanhoPagina, out int totalRegistros)
        {
            var filtroCidade = string.Empty;
            var filtroEstado = string.Empty;

            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@CurrentPage", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@PageSize", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Razao", SqlDbType.VarChar, 100));
            parms.Add(new SqlParameter("@Ramo", SqlDbType.VarChar, 100));
            parms.Add(new SqlParameter("@Cidade", SqlDbType.VarChar, 100));
            parms.Add(new SqlParameter("@Estado", SqlDbType.VarChar, 2));

            parms[0].Value = paginaCorrente;
            parms[1].Value = tamanhoPagina;
            parms[2].Value = idCurriculo;

            if (string.IsNullOrEmpty(nomeEmpresa))
                parms[3].Value = DBNull.Value;
            else
                parms[3].Value = nomeEmpresa;

            if (string.IsNullOrEmpty(ramoAtividade))
                parms[4].Value = DBNull.Value;
            else
                parms[4].Value = ramoAtividade;

            var filtrosCidade = TratarCampoCidade(cidade);
            filtroCidade = filtrosCidade.Item1;
            filtroEstado = filtrosCidade.Item2;

            if (string.IsNullOrEmpty(filtroCidade))
                parms[5].Value = DBNull.Value;
            else
                parms[5].Value = filtroCidade;

            if (string.IsNullOrEmpty(filtroEstado))
                parms[6].Value = DBNull.Value;
            else
                parms[6].Value = filtroEstado;

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

        #region TratarCampoCidade
        private static Tuple<string, string> TratarCampoCidade(string cidade)
        {
            try
            {
                if (string.IsNullOrEmpty(cidade))
                    return new Tuple<string, string>("", "");


                if (cidade.Contains("/"))
                {
                    var dadosCidade = cidade.Split('/');
                    return new Tuple<string, string>(dadosCidade[0], dadosCidade[1]);
                }
                else
                {
                    return new Tuple<string, string>(cidade, "");
                }

            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex, "Falha no tratamento do filtro cidade. Lista de empresas");
                return new Tuple<string, string>("", "");
            }
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

        public void EnviarSMSAvisoSaldoZerou(int idFilial)
        {
            var IdfUfpAvisoSMS = Parametro.RecuperaValorParametro(Enumeradores.Parametro.IdfUfpAvisoSMS);
            List<DestinatarioSMS> listaEnvioUsuariosAtivosFilialPerfil = UsuarioFilialPerfil.CarregarEnvioUsuariosAtivosPorFilialPerfil_SaldoZerou(idFilial);
            MensagemCS.EnvioSMSTanque(IdfUfpAvisoSMS, listaEnvioUsuariosAtivosFilialPerfil, true);

        }

        #region RecuperarCotaSMSTanque
        /// <summary>
        /// Recupera a cota de sms do tanque
        /// </summary>
        /// <returns></returns>
        public int RecuperarCotaSMSTanque()
        {
            int quantidadeUsuarios = this.RecuperarQuantidadeAcessosAdquiridos();

            return quantidadeUsuarios * CelularSelecionador.RecuperarCotaPadrao();
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

        #region RecuperarRazaoSocial
        /// <summary>
        /// Método que retorna a Razão Social da Filial
        /// </summary>
        /// <returns>Nme_Pessoa</returns>
        private string RecuperarRazaoSocial()
        {
            var parms = new List<SqlParameter>
            {
                new SqlParameter { ParameterName = "@Idf_Filial", SqlDbType = SqlDbType.Int, Size = 4, Value = _idFilial }
            };

            return Convert.ToString(DataAccessLayer.ExecuteScalar(CommandType.Text, SP_SELECT_RAZAO_SOCIAL, parms));
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

                        CelularSelecionador.DesabilitarUsuarios(objFilial);

                        objFilial.SituacaoFilial = new SituacaoFilial((int)Enumeradores.SituacaoFilial.Bloqueado);
                        objFilial.Save(trans);
                        trans.Commit();
                        //inativar vagas
                        SituacaoVagasFilial(objFilial.IdFilial, true);
                        //remover vagas do sine e não deixar importar mais vagas com o e-mai dele.
                        Dictionary<int, string> dic = new Dictionary<int, string>();
                        dic.Add(objFilial.IdFilial, motivo);
                          new Thread(new ParameterizedThreadStart(ColocarEmailVagasBronquinha)).Start(dic);
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
                        //Criar fila no assincrono
                        SituacaoVagasFilial(objFilial.IdFilial, false);
                        Dictionary<int, string> dic = new Dictionary<int, string>();
                        dic.Add(objFilial.IdFilial, motivo);
                        new Thread(new ParameterizedThreadStart(RemoverEmailVagasBronquinha)).Start(dic);

                        Task.Factory.StartNew(() => CelularSelecionador.HabilitarDesabilitarUsuarios(objUsuarioFilialPerfil.Filial));
                    
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
        public SituacaoFilial RecuperarSituacaoFilial(SqlTransaction tras = null)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Filial", SqlDbType = SqlDbType.Int, Size = 4, Value = _idFilial }
                };

            SituacaoFilial objSituacaoFilial;
          
                using (IDataReader dr = DataAccessLayer.ExecuteReader(tras, CommandType.Text, Sprecuperarsituacaofilial, parms))
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
            return SituacaoFilial.IdSituacaoFilial.Equals((int)Enumeradores.SituacaoFilial.AguardandoPublicacao);
        }
        #endregion

        #region EmpresaSemDadosReceita
        /// <summary>
        /// Método responsável por retornar se a empresa está sem dados da receita ou não
        /// </summary>
        /// <returns></returns>
        public bool EmpresaSemDadosReceita()
        {
            return SituacaoFilial.IdSituacaoFilial.Equals((int)Enumeradores.SituacaoFilial.FaltaDadosReceita);
        }
        #endregion

        #region EmpresaBloqueada
        /// <summary>
        /// Método responsável por retornar se a empresa está bloqueada, cancelada ou não
        /// </summary>
        /// <returns></returns>
        public bool EmpresaBloqueada()
        {
            PlanoAdquirido objPlanoAdquirido = null;
            PlanoAdquirido.CarregarPlanoAdquiridoPorSituacao(this, (int)Enumeradores.PlanoSituacao.Bloqueado, out objPlanoAdquirido);
          
            return SituacaoFilial.IdSituacaoFilial.Equals((int)Enumeradores.SituacaoFilial.Bloqueado) || SituacaoFilial.IdSituacaoFilial.Equals((int)Enumeradores.SituacaoFilial.Cancelado) || (objPlanoAdquirido != null && objPlanoAdquirido.PlanoAdquiridoBloqueado());
        }
        #endregion

        #region SalaAdministradoraListar
        /// <summary>
        /// Lista as empresas da sala da administradora
        /// </summary>
        /// <param name="empresasAguardandoPublicacao">Busca empresas aguardando publicação?</param>
        /// <param name="filtro">Filtro dos Campos</param>
        /// <param name="paginaCorrente">Página Atual</param>
        /// <param name="tamanhoPagina">Tamanho da Página </param>
        /// <param name="totalRegistros">Quantidade Total de Registros</param>
        /// <returns>Uma datatable com os dados da empresa</returns>
        public static DataTable SalaAdministradoraListar(bool empresasAguardandoPublicacao, String filtro, int paginaCorrente, int tamanhoPagina, out int totalRegistros)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@CurrentPage", SqlDbType = SqlDbType.Int, Size = 4, Value = paginaCorrente },
                    new SqlParameter { ParameterName = "@PageSize", SqlDbType = SqlDbType.Int, Size = 4, Value = tamanhoPagina }
                };

            var sqlParamSituacao = new SqlParameter { ParameterName = "@AguardandoPublicacao", SqlDbType = SqlDbType.Bit, Value = empresasAguardandoPublicacao };
            var sqlParamFiltro = new SqlParameter { ParameterName = "@Filtro", SqlDbType = SqlDbType.VarChar, Size = 100, Value = DBNull.Value };
            var sqlParamCNPJ = new SqlParameter { ParameterName = "@Num_CNPJ", SqlDbType = SqlDbType.Decimal, Size = 14, Value = DBNull.Value };
            var sqlParamTelefone = new SqlParameter { ParameterName = "@Telefone", SqlDbType = SqlDbType.VarChar, Size = 12, Value = DBNull.Value };
            var sqlParamEmail = new SqlParameter { ParameterName = "@Email", SqlDbType = SqlDbType.VarChar, Size = 40, Value = DBNull.Value };

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
        public DateTime RecuperarDataCadastro(SqlTransaction trans)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Filial", SqlDbType = SqlDbType.Int, Size = 4, Value = _idFilial }
                };

            Object ret = DataAccessLayer.ExecuteScalar(trans, CommandType.Text, Sprecuperardatacadastro, parms);
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
        public bool PossuiPlanoAtivo(SqlTransaction trans = null)
        {
            return PlanoAdquirido.ExistePlanoAdquiridoLiberadoPorFilial(this, trans);
        }
        #endregion

        #region PossuiPlanoElegivel1Clique
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
        public bool EmpresaSemPlanoPodeVisualizarCurriculo(int quantidadeVIP, SqlTransaction trans = null)
        {
            var parametros = Parametro.ListarParametros(
                new List<Enumeradores.Parametro>
                    {
                        Enumeradores.Parametro.ChupaVIPQuantidadePrimeiroDia,
                        Enumeradores.Parametro.ChupaVIPQuantidadeUltimoDia,
                        Enumeradores.Parametro.ChupaVIPQuantidadeVisualizacaoMenosPrimeiroDia,
                        Enumeradores.Parametro.ChupaVIPQuantidadeVisualizacaoEntrePrimeiroEUltimoDia,
                        Enumeradores.Parametro.ChupaVIPQuantidadeVisualizacaoMaisUltimoDia,
                        Enumeradores.Parametro.ChupaVIPQuantidadeVisualizacaoEmpresaPlanoEncerrado
                    });

            DateTime? dataEncerramentoUltimoPlanoAdquirido;
            if (trans != null)
                dataEncerramentoUltimoPlanoAdquirido = PlanoAdquirido.RecuperarDataFimUltimoPlanoAdquiridoEncerrado(this, trans);
            else
                dataEncerramentoUltimoPlanoAdquirido = PlanoAdquirido.RecuperarDataFimUltimoPlanoAdquiridoEncerrado(this);

            int quantidadeVisualizacao = 0;

            if (dataEncerramentoUltimoPlanoAdquirido.HasValue)
                quantidadeVisualizacao = Convert.ToInt32(parametros[Enumeradores.Parametro.ChupaVIPQuantidadeVisualizacaoEmpresaPlanoEncerrado]);
            else
            {
                DateTime dataBase;
                if (trans != null)
                    dataBase = RecuperarDataCadastro(trans).Date;
                else
                    dataBase = RecuperarDataCadastro().Date;

                var diasBase = (DateTime.Today - dataBase).Days + 1;
                if (diasBase <= Convert.ToInt32(parametros[Enumeradores.Parametro.ChupaVIPQuantidadePrimeiroDia]))
                    quantidadeVisualizacao = Convert.ToInt32(parametros[Enumeradores.Parametro.ChupaVIPQuantidadeVisualizacaoMenosPrimeiroDia]);
                else if (diasBase > Convert.ToInt32(parametros[Enumeradores.Parametro.ChupaVIPQuantidadePrimeiroDia]) && diasBase <= Convert.ToInt32(parametros[Enumeradores.Parametro.ChupaVIPQuantidadeUltimoDia]))
                    quantidadeVisualizacao = Convert.ToInt32(parametros[Enumeradores.Parametro.ChupaVIPQuantidadeVisualizacaoEntrePrimeiroEUltimoDia]);
                else if (diasBase > Convert.ToInt32(parametros[Enumeradores.Parametro.ChupaVIPQuantidadeUltimoDia]))
                    quantidadeVisualizacao = Convert.ToInt32(parametros[Enumeradores.Parametro.ChupaVIPQuantidadeVisualizacaoMaisUltimoDia]);
            }

            int quantidadeVisualizacaoHoje;
            if (trans != null)
                quantidadeVisualizacaoHoje = CurriculoVisualizacaoHistorico.RecuperarQuantidadeVisualizacaoDadosCompletosCurriculosVIP(this, trans);
            else
                quantidadeVisualizacaoHoje = CurriculoVisualizacaoHistorico.RecuperarQuantidadeVisualizacaoDadosCompletosCurriculosVIP(this);


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
        public void RecuperarConteudoFilialParaContratoPorFilial(out string razaoSocial, out decimal numCNPJ, out string descRua, out string numeroRua, out string nomeCidade, out string estado, out string numCEP)
        {
            razaoSocial = string.Empty;
            numCNPJ = 0;
            descRua = string.Empty;
            numeroRua = string.Empty;
            nomeCidade = string.Empty;
            estado = string.Empty;
            numCEP = string.Empty;

            var parms = new List<SqlParameter>
            {
                new SqlParameter{ ParameterName = "@Idf_Filial", SqlDbType = SqlDbType.Int, Size = 4, Value = this.IdFilial  }
            };

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SpConteudoFilialContratoDigitalPorFilial, parms))
            {
                if (dr.Read())
                {
                    razaoSocial = dr["Raz_Social"].ToString();
                    numCNPJ = Convert.ToDecimal(dr["Num_CNPJ"]);
                    descRua = dr["Des_Logradouro"].ToString();
                    numeroRua = dr["Num_Endereco"].ToStringNullSafe();
                    nomeCidade = dr["Nme_Cidade"].ToString();
                    estado = dr["Sig_Estado"].ToString();
                    numCEP = dr["Num_CEP"].ToStringNullSafe();
                }
            }
        }
        #endregion

        #region CarregarDadosUsuarioResponsavel
        public void CarregarDadosUsuarioResponsavel(out string nomePessoa, out decimal numCPF)
        {
            nomePessoa = string.Empty;
            numCPF = 0;

            var parms = new List<SqlParameter>
            {
                new SqlParameter { ParameterName = "@Idf_Filial", SqlDbType = SqlDbType.Int, Size = 4, Value = this._idFilial}
            };

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SpCarregarUsuarioResponsavel, parms))
            {
                if (dr.Read())
                {
                    nomePessoa = dr["Nme_Pessoa"].ToString();
                    numCPF = Convert.ToDecimal(dr["Num_CPF"].ToString());
                }
            }
        }
        #endregion

        #region CarregarDadosUsuarioMaster
        public void CarregarDadosUsuarioMaster(out string nomePessoa, out string emailComercial, out string enderecoFilial)
        {
            nomePessoa = string.Empty;
            emailComercial = string.Empty;
            enderecoFilial = string.Empty;

            var parms = new List<SqlParameter>
            {
                new SqlParameter { ParameterName = "@Idf_Filial", SqlDbType = SqlDbType.Int, Size = 4, Value = this._idFilial}
            };

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SpCarregarUsuarioMaster, parms))
            {
                if (dr.Read())
                {
                    nomePessoa = dr["Nme_Pessoa"].ToString();
                    emailComercial = dr["Eml_Comercial"].ToString();
                    enderecoFilial = string.Concat(dr["Des_Logradouro"].ToString(), ", ", dr["Num_Endereco"].ToString(), " - ", dr["Nme_Cidade"].ToString(), "/", dr["Sig_Estado"].ToString());
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
                if (dr["Dta_Pagamento"] != DBNull.Value && !string.IsNullOrEmpty(dr["Dta_Pagamento"].ToString()))
                    dataPagamento = Convert.ToDateTime(dr["Dta_Pagamento"]);
                if (dr["Vlr_Pagamento"] != DBNull.Value)
                    valorPagamento = Convert.ToDecimal(dr["Vlr_Pagamento"]);
                if (dr["Des_Identificador"] != DBNull.Value)
                    desIdentificador = dr["Des_Identificador"].ToString();
                if (dr["Dta_Inicio_Plano"] != DBNull.Value)
                    dataInicioPlano = Convert.ToDateTime(dr["Dta_Inicio_Plano"]);
                if (dr["Dta_Fim_Plano"] != DBNull.Value)
                    dataFimPlano = Convert.ToDateTime(dr["Dta_Fim_Plano"]);
                if (dr["Num_CNPJ"] != DBNull.Value)
                    numCNPJ = dr["Num_CNPJ"].ToString();
                if (dr["Raz_Social"] != DBNull.Value)
                    razaoSocial = dr["Raz_Social"].ToString();
                if (dr["Num_CEP"] != DBNull.Value)
                    numCEP = dr["Num_CEP"].ToString();
                if (dr["Des_Logradouro"] != DBNull.Value)
                    rua = dr["Des_Logradouro"].ToString();
                if (!Int32.TryParse(dr["Num_Endereco"].ToString(), out numEndereco))
                    numEndereco = 0;
                if (dr["Des_Complemento"] != DBNull.Value)
                    complemento = dr["Des_Complemento"].ToString();
                if (dr["Des_Bairro"] != DBNull.Value)
                    bairro = dr["Des_Bairro"].ToString();
                if (dr["Nme_Cidade"] != DBNull.Value)
                    cidade = dr["Nme_Cidade"].ToString();
                if (dr["Sig_Estado"] != DBNull.Value)
                    uf = dr["Sig_Estado"].ToString();
                if (dr["Nme_Fantasia"] != DBNull.Value)
                    nomeFantasia = dr["Nme_Fantasia"].ToString();
                if (dr["Cod_CNAE_Sub_Classe"] != DBNull.Value)
                    idfCnaePrincipal = Convert.ToString(dr["Cod_CNAE_Sub_Classe"]);
                if (dr["Num_DDD_Comercial"] != DBNull.Value)
                    ddd = dr["Num_DDD_Comercial"].ToString();
                if (dr["Num_Comercial"] != DBNull.Value)
                    telefone = dr["Num_Comercial"].ToString();
                if (dr["Eml_Envio_Boleto"] != DBNull.Value)
                    emailContato = dr["Eml_Envio_Boleto"].ToString();
                if (dr["Nme_Res_Plano_Adquirido"] != DBNull.Value)
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

                        EmailSenderFactory.Create(TipoEnviadorEmail.Fila).Enviar(assunto, mensagem, null, emailRemetenteSistema, objEmailDestinatarioCidade.EmailDestinatario.DescricaoEmail);
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

            return FormatObject.ToString(parametros, templateMensagem);
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

        #region Vendedor
        public Vendedor Vendedor()
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter{ ParameterName = "@num_Cnpj", SqlDbType = SqlDbType.Decimal, Value = this._numeroCNPJ}
                };

            using (var dr = DataAccessLayer.ExecuteReader(CommandType.StoredProcedure, "SP_Retorna_Vendedor_Responsavel", parms))
            {
                if (dr.Read())
                {
                    return new Vendedor
                    {
                        NomeVendedor = dr["Nme_Vendedor"] != DBNull.Value ? dr["Nme_Vendedor"].ToString() : string.Empty,
                        EmailVendedor = dr["Eml_Vendedor"] != DBNull.Value ? dr["Eml_Vendedor"].ToString() : string.Empty,
                        NumeroDDD = dr["Num_DDD_Comercial"] != DBNull.Value ? dr["Num_DDD_Comercial"].ToString() : string.Empty,
                        NumeroTelefone = dr["Num_Comercial"] != DBNull.Value ? dr["Num_Comercial"].ToString() : string.Empty
                    };
                }
            }
            return null;
        }

        public static Vendedor Vendedor(decimal num_cnpj)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter{ ParameterName = "@num_Cnpj", SqlDbType = SqlDbType.Decimal, Value = num_cnpj}
                };

            using (var dr = DataAccessLayer.ExecuteReader(CommandType.StoredProcedure, "SP_Retorna_Vendedor_Responsavel", parms))
            {
                if (dr.Read())
                {
                    return new Vendedor
                    {
                        NomeVendedor = dr["Nme_Vendedor"] != DBNull.Value ? dr["Nme_Vendedor"].ToString() : string.Empty,
                        EmailVendedor = dr["Eml_Vendedor"] != DBNull.Value ? dr["Eml_Vendedor"].ToString() : string.Empty,
                        NumeroDDD = dr["Num_DDD_Comercial"] != DBNull.Value ? dr["Num_DDD_Comercial"].ToString() : string.Empty,
                        NumeroTelefone = dr["Num_Comercial"] != DBNull.Value ? dr["Num_Comercial"].ToString() : string.Empty
                    };
                }
            }
            return null;
        }
        #endregion

        #region NotificiarTentantivaVisualizacao
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objPessoaFisica">Pessoa Fisica logada.</param>
        /// <param name="linkCurriculo">Link para visualizar o currículo.</param>
        public void NotificiarTentantivaVisualizacao(PessoaFisica objPessoaFisica, Curriculo objCurriculo)
        {

            Task.Factory.StartNew(() =>
            {
                try
                {
                    Vendedor objVendedor = this.Vendedor();
                    if (Vendedor() != null)
                    {
                        var ultimosPlanos = PlanoAdquirido.CarregarUltimosPlanosEncerrados(this, 3);
                        var descricaoUltimosPlanos = "Nenhum plano foi adquirido";
                        if (ultimosPlanos.Count > 0)
                            descricaoUltimosPlanos = ultimosPlanos.Aggregate(string.Empty, (current, objPlanoAdquirido) => current + string.Format("{0} - {1} até {2}<br>", objPlanoAdquirido.Plano.DescricaoPlano, objPlanoAdquirido.DataInicioPlano.ToShortDateString(), objPlanoAdquirido.DataFimPlano.ToShortDateString()));

                        var parametros = new
                        {
                            NomeVendedor = objVendedor.NomeVendedor,
                            RazaoSocial = this.ToString(),
                            Usuario = objPessoaFisica.NomeCompleto,
                            LinkCurriculo = objCurriculo.URL(),
                            UtmCampaign = objCurriculo.IdCurriculo,
                            Hora = DateTime.Now,
                            UltimosPlanos = descricaoUltimosPlanos,
                            NomeCurriculo = new PessoaFisica(PessoaFisica.RecuperarIdPorCurriculo(objCurriculo)).NomeCompleto
                        };

                        string assunto;
                        string template = CartaEmail.RetornarConteudoBNE(Enumeradores.CartaEmail.EmpresaSemPlanoTentandoVisualizarCurriculo, out assunto);
                        string emailRemetente = Parametro.RecuperaValorParametro(Enumeradores.Parametro.EmailMensagens);

                        string mensagem = FormatObject.ToString(parametros, template);

                        if (!string.IsNullOrWhiteSpace(objVendedor.EmailVendedor))
                            EmailSenderFactory.Create(TipoEnviadorEmail.Fila).Enviar(assunto, mensagem, Enumeradores.CartaEmail.EmpresaSemPlanoTentandoVisualizarCurriculo, emailRemetente, objVendedor.EmailVendedor);
                    }
                }
                catch (Exception ex)
                {
                    EL.GerenciadorException.GravarExcecao(ex, "Falha ao enviar notificação para o vendedor responsável pela empresa.");
                }
            });
        }
        #endregion

        #region ToString
        public override string ToString()
        {
            return string.Format("{0} ({1})", this.RazaoSocial, this.CNPJ);
        }
        #endregion

        #region SaldoSMS
        /// <summary>
        /// Recupera o saldo de sms da filial atual
        /// </summary>
        /// <param name="trans"></param>
        /// <returns></returns>
        public int SaldoSMS(SqlTransaction trans = null)
        {
            PlanoAdquirido objPlanoAdquirido;
            if (PlanoAdquirido.CarregarPlanoAdquiridoPorSituacao(this, (int)Enumeradores.PlanoSituacao.Liberado, out objPlanoAdquirido, trans))
            {
                return objPlanoAdquirido.SaldoSMS(trans);
            }
            return 0;
        }
        #endregion

        #region SaldoVisualizacao
        /// <summary>
        /// Recupera o saldo de visualizacao da filial atual
        /// </summary>
        /// <param name="trans"></param>
        /// <returns></returns>
        public int SaldoVisualizacao(SqlTransaction trans = null)
        {
            PlanoAdquirido objPlanoAdquirido;
            if (PlanoAdquirido.CarregarPlanoAdquiridoPorSituacao(this, (int)Enumeradores.PlanoSituacao.Liberado, out objPlanoAdquirido, trans))
            {
                return objPlanoAdquirido.SaldoVisualizacao(trans);
            }
            return 0;
        }
        #endregion

        #region SaldoCampanha
        /// <summary>
        /// Recupera o saldo de campanha da filial atual
        /// </summary>
        /// <param name="trans"></param>
        /// <returns></returns>
        public int SaldoCampanha(SqlTransaction trans = null)
        {
            int saldo = 0;
            PlanoAdquirido objPlanoAdquirido;
            if (PlanoAdquirido.CarregarPlanoAdquiridoPorSituacao(this, (int)Enumeradores.PlanoSituacao.Liberado, out objPlanoAdquirido, trans))
            {
                saldo = objPlanoAdquirido.SaldoCampanha(trans);
            }

            ParametroFilial objParametroFilial;
            if (ParametroFilial.CarregarParametroPorFilial(Enumeradores.Parametro.CampanhaRecrutamentoQuantidadeSaldoEnvioCampanha, this, out objParametroFilial, trans))
            {
                Int16 valorParametroFilial;
                if (Int16.TryParse(objParametroFilial.ValorParametro, out valorParametroFilial))
                {
                    saldo += valorParametroFilial;
                }
            }
            return saldo;
        }
        #endregion

        #region DescontarSMS
        /// <summary>
        /// Recupera o saldo de sms da filial atual
        /// </summary>
        /// <param name="trans"></param>
        /// <returns></returns>
        public void DescontarSMS(SqlTransaction trans = null)
        {
            PlanoAdquirido objPlanoAdquirido;
            if (PlanoAdquirido.CarregarPlanoAdquiridoPorSituacao(this, (int)Enumeradores.PlanoSituacao.Liberado, out objPlanoAdquirido, trans))
            {
                objPlanoAdquirido.DescontarSMS(trans);
            }
        }
        #endregion

        #region DescontarVisualizacao
        /// <summary>
        /// Recupera o saldo de visualizacao da filial atual
        /// </summary>
        /// <param name="trans"></param>
        /// <returns></returns>
        public void DescontarVisualizacao(SqlTransaction trans = null)
        {
            PlanoAdquirido objPlanoAdquirido;
            if (PlanoAdquirido.CarregarPlanoAdquiridoPorSituacao(this, (int)Enumeradores.PlanoSituacao.Liberado, out objPlanoAdquirido, trans))
            {
                objPlanoAdquirido.DescontarVisualizacao(trans);
            }
        }
        #endregion

        #region DescontarCampanha
        /// <summary>
        /// Recupera o saldo de campanha da filial atual
        /// </summary>
        /// <param name="trans"></param>
        /// <returns></returns>
        public bool DescontarCampanha(SqlTransaction trans = null)
        {
            if (SaldoCampanha(trans) > 0)
            {
                PlanoAdquirido objPlanoAdquirido;
                if (PlanoAdquirido.CarregarPlanoAdquiridoPorSituacao(this, (int)Enumeradores.PlanoSituacao.Liberado, out objPlanoAdquirido, trans))
                {
                    if (objPlanoAdquirido.SaldoCampanha() > 0)
                    {
                        objPlanoAdquirido.DescontarCampanha(trans);
                        return true;
                    }
                }

                ParametroFilial objParametroFilial;
                if (ParametroFilial.CarregarParametroPorFilial(Enumeradores.Parametro.CampanhaRecrutamentoQuantidadeSaldoEnvioCampanha, this, out objParametroFilial, trans))
                {
                    Int16 valorParametroFilial;
                    if (Int16.TryParse(objParametroFilial.ValorParametro, out valorParametroFilial))
                    {
                        if (valorParametroFilial > 0)
                        {
                            var novoSaldo = valorParametroFilial - 1;
                            objParametroFilial.ValorParametro = novoSaldo.ToString();

                            objParametroFilial.Save(trans);
                            return true;
                        }
                    }
                }
            }

            return false;
        }
        #endregion

        #region VagaRapida
        /// <summary>
        /// Verifica se a filial é do fluxo de vaga rápida
        /// </summary>
        /// <returns></returns>
        public bool VagaRapida()
        {
            return _idFilial == Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.VagaRapida_Filial));
        }
        #endregion

        #region SituacaoVagasFilial
        /// <summary>
        /// Status TRUE vai remover as vagas da pesquisa salvando na bne_parametro_vaga,
        /// FALSE vai apagar o parametro_vaga, voltando as vagas para a pesquisa
        /// </summary>
        /// <param name="idFilial"></param>
        /// <param name="status"></param>
        public static void SituacaoVagasFilial(int idFilial, Boolean status)
        {
            try
            {
                //Criar fila no assincrono
                var parametros = new ParametroExecucaoCollection
                            {
                                {"Idf_Filial", "Idf_Filial", idFilial.ToString(), idFilial.ToString()},
                                {"Status", "Status", status.ToString(), status.ToString()}

                            };
                ProcessoAssincrono.IniciarAtividade(AsyncServices.Enumeradores.TipoAtividade.RemoverVagaEmpresaBloqueada, parametros);
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex, "Criar Fila no assincrono no metodo SituacaoVagaFilial");
            }

        }
        #endregion
        #endregion

        #region Mapeamento Novo -> Velho
        public void SalvarMigracao(SqlTransaction trans)
        {
            if (!this._persisted)
            {
                //Task 41692 - Horario para cadastrar empresa/ se o dia é útil.
                if (DateTime.Now.Hour < Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.HorarioCadastorEmpresaInicio))
                    || DateTime.Now.Hour > Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.HorarioCadastorEmpresaFim))
                    || Feriado.RetornarDiaUtilVencimento(DateTime.Now) > 0)
                {
                    this._situacaoFilial.IdSituacaoFilial = (int)Enumeradores.SituacaoFilial.ForaDoHorarioComercial;
                }


                this.InsertMigracao(trans);
            }
            else
            {//Não alterar situação da empresa que ja tem cadastro, evita de desbloquear quem ta bloqueado.
                this.SituacaoFilial = RecuperarSituacaoFilial(trans);
                this.UpdateMigracao(trans);
            }
        }
        #region InsertMigracao
        /// <summary>
        /// Método utilizado para inserir uma instância de Filial no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        private void InsertMigracao(SqlTransaction trans)
        {
            List<SqlParameter> parms = GetParameters();
            SetParametersMigracao(parms);
            SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
            this._idFilial = Convert.ToInt32(cmd.Parameters["@Idf_Filial"].Value);
            cmd.Parameters.Clear();
            this._persisted = true;
            this._modified = false;
        }
        #endregion

        #region Update
        /// <summary>
        /// Método utilizado para atualizar uma instância de Filial no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        private void UpdateMigracao(SqlTransaction trans)
        {
            if (this._modified)
            {
                List<SqlParameter> parms = GetParameters();
                SetParametersMigracao(parms);
                DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPUPDATE, parms);
                this._modified = false;
            }
        }
        #endregion

        #region SetParametersMigracao
        /// <summary>
        /// Método auxiliar que recebe e preenche a lista de parâmetros passada de acordo com os valores da instância.
        /// </summary>
        /// <param name="parms">Lista de parâmetros SQL.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        private void SetParametersMigracao(List<SqlParameter> parms)
        {
            parms[0].Value = this._idFilial;
            parms[1].Value = this._flagMatriz;

            if (this._numeroCNPJ.HasValue)
                parms[2].Value = this._numeroCNPJ;
            else
                parms[2].Value = DBNull.Value;

            parms[3].Value = this._razaoSocial;
            parms[4].Value = this._nomeFantasia;

            if (this._cNAEPrincipal != null)
                parms[5].Value = this._cNAEPrincipal.IdCNAESubClasse;
            else
                parms[5].Value = DBNull.Value;


            if (this._naturezaJuridica != null)
                parms[6].Value = this._naturezaJuridica.IdNaturezaJuridica;
            else
                parms[6].Value = DBNull.Value;

            if (this._endereco != null)
                parms[7].Value = this._endereco.IdEndereco;
            else
                parms[7].Value = DBNull.Value;

            if (!String.IsNullOrEmpty(this._enderecoSite))
                parms[8].Value = this._enderecoSite;
            else
                parms[8].Value = DBNull.Value;

            parms[9].Value = this._numeroDDDComercial;
            parms[10].Value = this._numeroComercial;
            parms[11].Value = this._flagInativo;

            if (this._quantidadeUsuarioAdicional.HasValue)
                parms[14].Value = this._quantidadeUsuarioAdicional;
            else
                parms[14].Value = DBNull.Value;

            parms[15].Value = this._quantidadeFuncionarios;
            parms[16].Value = this._descricaoIP;
            parms[17].Value = this._flagOfereceCursos;

            parms[18].Value = this._situacaoFilial.IdSituacaoFilial;

            if (!String.IsNullOrEmpty(this._descricaoPaginaFacebook))
                parms[19].Value = this._descricaoPaginaFacebook;
            else
                parms[19].Value = DBNull.Value;

            if (this._quantidadeUsuarioAdicional != null)
                parms[20].Value = this._quantidadeUsuarioAdicional;
            else
                parms[20].Value = DBNull.Value;

            if (this._descricaoLocalizacao != null)
                parms[21].Value = this._descricaoLocalizacao;
            else
                parms[21].Value = SqlGeography.Null;

            if (this._tipoParceiro != null)
                parms[22].Value = this._tipoParceiro.IdTipoParceiro;
            else
                parms[22].Value = DBNull.Value;

            if (!this._persisted)
            {
                parms[0].Direction = ParameterDirection.Output;
            }
            else
            {
                parms[0].Direction = ParameterDirection.Input;
            }
            parms[12].Value = this._dataCadastro;
            parms[13].Value = this._dataAlteracao;
        }
        #endregion

        #region ListarFiliais
        public static IEnumerable<Filial> ListarFiliais()
        {
            const string sp = @"
            SELECT  --TOP 10 
                    *
            FROM    TAB_Filial F
                    LEFT JOIN BNE2_PRD.pessoajuridica.PessoaJuridica PJ ON F.Num_CNPJ = PJ.CNPJ
            WHERE   PJ.CNPJ IS NULL
            ORDER BY F.Dta_Cadastro DESC";

            using (var dr = DataAccessLayer.ExecuteReader(CommandType.Text, sp, null))
            {
                while (dr.Read())
                {
                    var objFilial = new Filial();
                    if (SetInstance_NotDispose(dr, objFilial))
                        yield return objFilial;
                }
            }
        }
        #endregion

        #endregion

        #region AreaBne
        /// <summary>
        /// Retorna a des_area_bne da empresa
        /// </summary>
        /// <param name="idFilial"></param>
        /// <returns></returns>
        public static string AreaBne(int idFilial)
        {
            var parms = new List<SqlParameter>{
                new SqlParameter{ParameterName = "@idf_Filial", SqlDbType = SqlDbType.Int, Value = idFilial}
            };

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, spAreaBne, parms))
            {
                if (dr.Read())
                    return dr["Des_Area_BNE"].ToString();

                return "";
            }
        }
        #endregion


        /// <summary>
        /// Listar as filiais que não finalizaram a compra de um plano.
        /// </summary>
        /// <returns></returns>
        public static DataTable FilialCarrinhaAbandonado()
        {
            return DataAccessLayer.ExecuteReaderDs(CommandType.Text, SELECTFILIALCARRINHOABANDONADO, null).Tables[0];
        }

        #region [CartaInscritos]
        /// <summary>
        // • carta periódica (30 dias) mostrando a qtde de novos inscritos no STC.
        /// </summary>
        public static void CartaInscritos()
        {
            int count = 0;
            int countFilialComMenosInscritos = 0;
            int QtdMinimaInscritos = Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.QtdMinimaInscritosSTC));
            //Selecionar as empresas com stc
            DataTable dt = null;
            Filial objFilial = new Filial();
            string emailRemetente = string.Empty;
            try
            {
                using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, spSelectFiliaisSTC, null))
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
            var templateCarta = CartaEmail.LoadObject((int)Enumeradores.CartaEmail.InscritosSTC);
            var cartaCandidatos = CartaEmail.RecuperarConteudo(Enumeradores.CartaEmail.InscritosSTC_Candidatos);
            //Pegar os candidatos da empresa que se cadastraram nos ultimos 30 dias

            foreach (DataRow filial in dt.Rows)
            {
                //Montar carta.
                try
                {
                    var candidatos = string.Empty;
                    string listCandidatos = string.Empty;
                    string urlPadrao = string.Concat("http://", Helper.RecuperarURLAmbiente(), "/logar/");
                    var parm = new List<SqlParameter>
                    {
                        new SqlParameter { ParameterName ="@Idf_Filial", SqlDbType = SqlDbType.Int, Size =4,Value = (int)filial["idf_Filial"] }// Convert.ToInt32(filial["idf_filial"]) }
                    };
          
                    using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, spCvsCadastrosSTC, parm))
                    {
                        while (dr.Read())
                        {//preencher os inscritos
                            candidatos = cartaCandidatos;
                            candidatos = candidatos.Replace("{Nome_Completo}", dr["nme_pessoa"].ToString());
                            candidatos = candidatos.Replace("{Idade}", Helper.CalcularIdade(Convert.ToDateTime(dr["dta_nascimento"])).ToString());
                            candidatos = candidatos.Replace("{Escolaridade}", dr["Des_BNE"].ToString());
                            candidatos = candidatos.Replace("{Bairro}", !string.IsNullOrEmpty(dr["des_bairro"].ToString()) ? $" - {dr["des_bairro"].ToString()}" : string.Empty);
                            candidatos = candidatos.Replace("{Cidade_Estado}", Helper.FormatarCidade(dr["nme_Cidade"].ToString(), dr["sig_estado"].ToString()));
                            var stringFuncoes  = FuncaoPretendida.CarregarNomeDeFuncoesPretendidasPorCurriculo(new Curriculo(Convert.ToInt32(dr["idf_curriculo"]))).Split(';').Where(x => !string.IsNullOrEmpty(x)).ToArray();

                            var funcoes = string.Join(", ", stringFuncoes);
                            candidatos = candidatos.Replace("{Funcoes_Pretendidas}", funcoes);
                            listCandidatos += candidatos;
                        }
                            
                    }
                    if (!string.IsNullOrEmpty(listCandidatos))//Envia Carta para stc que tiveram mais de X inscritos.
                    {
                        objFilial.NumeroCNPJ = Convert.ToDecimal(filial["num_cnpj"]);

                        emailRemetente = objFilial.Vendedor().EmailVendedor;
                        string linkLista = string.Empty;
                        PesquisaCurriculo objPesq = new PesquisaCurriculo
                        {
                            Origem = new Origem((int)filial["Idf_origem"])
                        };
                        objPesq.Salvar(new List<PesquisaCurriculoFuncao>());
                        linkLista = string.Format("{0}?url=lista-de-curriculos/{1}", filial["des_url"].ToString(), objPesq.IdPesquisaCurriculo);
                        linkLista = string.Concat(urlPadrao, LoginAutomatico.GerarHashAcessoLogin((decimal)filial["num_cpf"], (DateTime)filial["dta_nascimento"], linkLista));
                        var mensagem = templateCarta.ValorCartaEmail.Replace("{Quantidade_Curriculos}", listCandidatos).Replace("{link_Conferir_Lista}", linkLista);

                        if (Validacao.ValidarEmail(filial["eml_comercial"].ToString()))
                        {
                            //Enviar E-mail para o candidato
                            EmailSenderFactory.Create(TipoEnviadorEmail.Fila)
                                .Enviar(templateCarta.DescricaoAssunto, mensagem, BLL.Enumeradores.CartaEmail.InscritosSTC, emailRemetente,
                                    filial["eml_comercial"].ToString());
                            count++;
                        }
                    }
                    else
                        countFilialComMenosInscritos++;
                }
                catch (Exception ex)
                {
                    EL.GerenciadorException.GravarExcecao(ex, String.Format("Enviar carta de inscritos stc para a filial {0}", (int)filial["idf_Filial"]));
                }

            }
            EmailSenderFactory.Create(TipoEnviadorEmail.Fila)
                 .Enviar("Email Inscritos STC", $"Email enviados: {count} <br>Empresas com menos de 10 inscritos no seu stc: {countFilialComMenosInscritos}", null, "polaco@bne.com.br",
                  "polaco@bne.com.br");

        }
        #endregion

        #region [ColocarEmailVagasBronquinha]
        public static void ColocarEmailVagasBronquinha(object IdfFilial)
        {
            try
            {
                var dic = (Dictionary<int, string>)IdfFilial;
                var emailVaga = EmailBroquinha(dic.Keys.First());
                using (var ws = new wsSine.AppClient())
                {
                    var resultado = ws.Bronquinha(emailVaga.ToArray(), dic.Values.First());
                }
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex, "Erro ao requisitar o Bronquinha do WS SINE");
            }

        }
        #endregion

        #region [RemoverEmailVagasBronquinha]
        public static void RemoverEmailVagasBronquinha(object IdfFilial)
        {
            try
            {
                var dic = (Dictionary<int, string>)IdfFilial;
                var emailVaga = EmailBroquinha(dic.Keys.First());
                using (var ws = new wsSine.AppClient())
                {
                    var resultado = ws.RemoverBronquinha(emailVaga.ToArray(), dic.Values.First());
                }
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex, "Erro ao requisitar o removerBronquinha do WS SINE");
            }
            
        }

        #region EmailBroquinha
        private static List<string> EmailBroquinha(int idFilial)
        {
            List<SqlParameter> parametros = new List<SqlParameter>
            {
                new SqlParameter {ParameterName="@Idf_Filial", SqlDbType = SqlDbType.Int, Value = idFilial  }
            };
        List<string> emailVaga = new List<string>();

            using (var dr = DataAccessLayer.ExecuteReader(CommandType.Text, spColocarEmailVagasBronquinha, parametros))
            {
                while (dr.Read())
                    emailVaga.Add(dr["Eml_Vaga"].ToString().Trim());
            }

            return emailVaga;
        }
        #endregion
     
        #endregion

        #region [ Migration Mapping ]
        /// <summary>
        /// Médodos e atributos auxiliares à migração de dados para o novo
        /// domínio.
        /// </summary>
        public int MigrationId
        {
            set
            {
                this._idFilial = value;
            }
            get { return this._idFilial; }
        }
        #endregion

        #region PossuiPlanoAtivoIlimitado
        /// <summary>
        /// Retorna um booleano indicando se a empresa possui um plano adquirido liberado e Ilimitado
        /// </summary>
        /// <returns></returns>
        public bool PossuiPlanoAtivoIlimitado()
        {
            return PlanoAdquirido.ExistePlanoAdquiridoIlimitadoLiberadoPorFilial(this);
        }
        #endregion
    }
}