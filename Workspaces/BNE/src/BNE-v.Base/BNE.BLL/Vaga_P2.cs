//-- Data: 02/03/2010 09:17
//-- Autor: Gieyson Stelmak

using BNE.BLL.AsyncServices;
using BNE.BLL.Common.Sitemap;
using BNE.BLL.Custom;
using BNE.BLL.Custom.Email;
using BNE.BLL.DTO;
using BNE.Services.Base.ProcessosAssincronos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace BNE.BLL
{
    public partial class Vaga // Tabela: BNE_Vaga
    {

        #region Consultas

        #region Spselectvagas
        private const string Spselectvagas = @"
        SELECT  TOP 4 Idf_Vaga ,
                F.Des_Funcao ,
                Vlr_Salario_De ,
                Vlr_Salario_Para ,
                Nme_Cidade ,
                Sig_Estado ,
                CASE WHEN CONVERT(VARCHAR, GETDATE(), 103) = CONVERT(VARCHAR, V.Dta_Abertura, 103) THEN CONVERT(VARCHAR(5), V.Dta_Abertura, 108)
                        ELSE CASE WHEN DATEDIFF(dd, V.Dta_Abertura, GETDATE()) = 1 THEN CONVERT(VARCHAR, DATEDIFF(d, V.Dta_Abertura, GETDATE())) + ' Dia'
                                ELSE CONVERT(VARCHAR, DATEDIFF(d, V.Dta_Abertura, GETDATE())) + ' Dias'
                            END
                END AS Descricao_Data ,
                V.Dta_Cadastro ,
                AB.Des_Area_BNE
        FROM    BNE.BNE_Vaga V WITH(NOLOCK)
                INNER JOIN plataforma.TAB_Funcao F WITH(NOLOCK) ON F.Idf_Funcao = V.Idf_Funcao
                INNER JOIN plataforma.TAB_Cidade C WITH(NOLOCK) ON C.Idf_Cidade = V.Idf_Cidade
                INNER JOIN plataforma.TAB_Area_BNE AB WITH(NOLOCK) ON F.Idf_Area_BNE = AB.Idf_Area_BNE
        WHERE   V.Flg_Vaga_Rapida = 0
                AND V.Flg_Inativo = 0
                AND V.Flg_Vaga_Arquivada = 0
                AND V.Flg_Auditada = 1
                AND V.Idf_Origem = 1
        ORDER BY  V.Dta_Abertura DESC
        ";
        #endregion

        #region SPINSERT
        private const string Spinsert = "INSERT INTO BNE_Vaga (Idf_Funcao, Idf_Cidade, Cod_Vaga, Vlr_Salario_De, Dta_Abertura, Dta_Prazo, Eml_Vaga, Des_Requisito, Qtd_Vaga, Nme_Empresa, Flg_Vaga_Rapida, Dta_Cadastro, Flg_Inativo, Idf_Filial, Flg_Confidencial, Idf_Usuario_Filial_Perfil, Idf_Escolaridade, Num_Idade_Minima, Num_Idade_Maxima, Idf_Sexo, Des_Beneficio, Des_Atribuicoes, Num_DDD, Num_Telefone, Flg_Receber_Cada_CV, Flg_Receber_Todos_CV, Des_Funcao, Flg_Auditada, Flg_BNE_Recomenda, Flg_Vaga_Arquivada, Flg_Vaga_Massa, Idf_Origem, Flg_Liberada, Idf_Deficiencia, Dta_Auditoria, Vlr_Salario_Para, Flg_Deficiencia, Flg_Empresa_Em_Auditoria) VALUES (@Idf_Funcao, @Idf_Cidade, @Cod_Vaga, @Vlr_Salario_De, @Dta_Abertura, @Dta_Prazo, @Eml_Vaga, @Des_Requisito, @Qtd_Vaga, @Nme_Empresa, @Flg_Vaga_Rapida, @Dta_Cadastro, @Flg_Inativo, @Idf_Filial, @Flg_Confidencial, @Idf_Usuario_Filial_Perfil, @Idf_Escolaridade, @Num_Idade_Minima, @Num_Idade_Maxima, @Idf_Sexo, @Des_Beneficio, @Des_Atribuicoes, @Num_DDD, @Num_Telefone, @Flg_Receber_Cada_CV, @Flg_Receber_Todos_CV, @Des_Funcao, @Flg_Auditada, @Flg_BNE_Recomenda, @Flg_Vaga_Arquivada, @Flg_Vaga_Massa, @Idf_Origem, @Flg_Liberada, @Idf_Deficiencia, @Dta_Auditoria, @Vlr_Salario_Para, @Flg_Deficiencia, @Flg_Empresa_Em_Auditoria);SET @Idf_Vaga = SCOPE_IDENTITY();SET @Cod_Vaga = ( SELECT 'V' + REPLICATE('0', 8 - LEN(ISNULL(CONVERT(VARCHAR, @Idf_Vaga), 0))) + CONVERT(VARCHAR, @Idf_Vaga));UPDATE BNE_Vaga SET Cod_Vaga = @Cod_Vaga WHERE Idf_Vaga = @Idf_Vaga;";
        #endregion

        #region Spselectfuncoesvagasporfilial
        private const string Spselectfuncoesvagasporfilial = @"
        DECLARE @iSelect VARCHAR(8000)
        SET @iSelect = ' 
            SELECT  DISTINCT F.Idf_Funcao ,
                    F.Des_Funcao
            FROM    BNE.BNE_Vaga V WITH(NOLOCK)
                    INNER JOIN plataforma.TAB_Funcao F WITH(NOLOCK) ON V.Idf_Funcao = F.Idf_Funcao
            WHERE   V.Idf_Filial = ' + CONVERT(VARCHAR, @Idf_Filial) + '
                    AND V.Flg_Inativo = 0
                    AND V.Flg_Vaga_Rapida = 0'

            IF (@BitVagasAnunciadas = 1)
                SET @iSelect = @iSelect + ' AND Flg_Vaga_Arquivada = 0 '
            ELSE IF (@BitVagasAnunciadas = 0)
                SET @iSelect = @iSelect + ' AND Flg_Vaga_Arquivada = 1 '
		
            SET @iSelect = @iSelect + ' ORDER BY F.Des_Funcao'

        EXECUTE (@iSelect)";
        #endregion

        #region Spselectcountperguntas
        private const string Spselectcountperguntas = "SELECT COUNT(Idf_Vaga_Pergunta) FROM BNE_Vaga_Pergunta WITH(NOLOCK) WHERE Idf_Vaga = @Idf_Vaga AND Flg_Inativo = 0";
        #endregion

        #region Spvagasnaoarquivadas
        private const string Spvagasnaoarquivadas = @"
        DECLARE @data DATE 
        SET @data = DATEADD(dd, -1, GETDATE())		

        SELECT  V.*
        FROM    bne.BNE_Vaga V WITH ( NOLOCK )
        WHERE   Dta_Prazo < @data
                AND V.Flg_Inativo = 0
                AND V.Flg_Vaga_Arquivada = 0
                AND V.Flg_Auditada = 1";
        #endregion

        #region Spvagasemmassatwitter
        private const string Spvagasemmassatwitter = @"
        select
	        v.idf_vaga,
	        v.idf_funcao, 
	        v.idf_cidade,
	        v.vlr_salario_de,
            v.vlr_salario_para,
	        v.idf_sexo,
	        func.Des_Funcao,
	        ci.Nme_Cidade,
	        ci.Sig_Estado,
	        tw.Des_Consumer_Key, 
	        tw.Des_Consumer_Secret,
	        tw.Des_Access_Token, 
	        tw.Des_Access_Token_Secret  
	    from 
	        BNE_vaga v WITH ( NOLOCK )
	        join Plataforma.Tab_Funcao func WITH ( NOLOCK ) on (func.Idf_Funcao = v.Idf_funcao)
	        join plataforma.TAB_Cidade ci WITH ( NOLOCK ) on (ci.Idf_Cidade = v.Idf_cidade)
	        left join BNE_Rede_Social_Conta rs WITH ( NOLOCK ) on (rs.Idf_Area_Bne = func.Idf_Area_Bne)
	        left join BNE_Conta_Twitter tw WITH ( NOLOCK ) on (tw.Idf_Conta_Twitter = rs.Idf_Conta_Twitter)
	    where 
	        v.Idf_Vaga= @Idf_Vaga";
        #endregion

        #region Spvagasemmassa
        private const string Spvagasemmassa = @"
        SELECT TOP 50
                v.idf_vaga ,
                v.idf_funcao ,
                v.idf_cidade ,
                v.vlr_salario_de ,
                v.vlr_salario_para ,
                v.idf_sexo ,
                func.Des_Funcao ,
                ci.Nme_Cidade ,
                ci.Sig_Estado
        FROM    BNE_Vaga v WITH ( NOLOCK )
                JOIN Plataforma.Tab_Funcao func WITH ( NOLOCK ) ON ( func.Idf_Funcao = v.Idf_funcao )
                JOIN plataforma.TAB_Cidade ci WITH ( NOLOCK ) ON ( ci.Idf_Cidade = v.Idf_cidade )
        WHERE   v.Flg_Vaga_Rapida = 0
                AND v.Flg_Inativo = 0
                AND v.Flg_Vaga_Massa = 1
                AND v.Flg_Auditada = 1
                AND v.Flg_vaga_Arquivada = 0";
        #endregion

        #region SpSelectFilialUltimaVaga
        private const string SpSelectFilialUltimaVaga = @"
        select top 1
            v.Idf_Vaga
        from 
            BNE_vaga v WITH(NOLOCK)
            join BNE.Tab_Filial emp WITH(NOLOCK) on (emp.Idf_Filial = v.Idf_Filial)
        where 
            v.Flg_Inativo = 0 AND
            v.Idf_Filial = @Idf_Filial
        order by v.Idf_Vaga DESC";
        #endregion

        #region Spselectsalaadministradorvagas
        private const string Spselectsalaadministradorvagas = @"
        select top 1000
            v.Idf_Vaga,
            v.Cod_Vaga,
            v.Flg_Auditada,
            (case 
            when v.Idf_funcao is null then v.Des_Funcao
            else f.Des_Funcao
            end ) as Des_Funcao,
            emp.Raz_Social,
            CONVERT(VARCHAR, v.Dta_Abertura, 103) as Dta_Abertura,
            CONVERT(VARCHAR, v.Dta_Cadastro, 103) as Dta_Cadastro
        from 
            BNE_vaga v WITH(NOLOCK)
            join BNE.Tab_Filial emp WITH(NOLOCK) on (emp.Idf_Filial = v.Idf_Filial)
            left join plataforma.Tab_Funcao f WITH(NOLOCK) on (f.Idf_Funcao = v.Idf_Funcao)
        where 
            v.Flg_Inativo = 0   
            and (
                f.Des_Funcao like '%'+@filtro+'%'
            or emp.Raz_Social like '%'+@filtro+'%'
            or emp.Num_CNPJ like '%'+@filtro+'%'    
            or V.Cod_Vaga like '%'+@filtro+'%'  
            )
            ORDER BY V.Dta_Cadastro ASC";
        #endregion

        #region Spselectsalaadministradorvagasnaoauditadas
        private const string Spselectsalaadministradorvagasnaoauditadas = @"
        select 
            top 1000 v.Idf_Vaga,
            v.Flg_Auditada,
            v.Cod_Vaga,
            (
                case 
                    when v.Idf_funcao is null then v.Des_Funcao
                    else f.Des_Funcao
                end 
            ) AS Des_Funcao,
            emp.Raz_Social,
            CONVERT(VARCHAR, v.Dta_Abertura, 103) as Dta_Abertura,
            CONVERT(VARCHAR, v.Dta_Cadastro, 103) as Dta_Cadastro
        from 
            BNE_vaga v WITH(NOLOCK)
            join BNE.Tab_Filial emp WITH(NOLOCK) on (emp.Idf_Filial = v.Idf_Filial)
            left join plataforma.Tab_Funcao f WITH(NOLOCK) on (f.Idf_Funcao = v.Idf_Funcao)
        where 
            v.Flg_Inativo = 0  
            and v.Flg_Auditada = 0
            AND V.Flg_Empresa_Em_Auditoria = 0 --Não mostrar vagas onde a empresa está em auditoria               
            and v.Flg_Vaga_Arquivada = 0
            and (
                f.Des_Funcao like '%'+@filtro+'%'
                or emp.Raz_Social like '%'+@filtro+'%'
                or emp.Num_CNPJ like '%'+@filtro+'%'    
                or V.Cod_Vaga like '%'+@filtro+'%'  
            )
            ORDER BY V.Dta_Cadastro ASC ";
        #endregion

        #region SpRecuperarVagasSiteMap
        private const string SpRecuperarVagasSiteMap = @"
        /* TODAS AS VAGAS */
        SELECT  V.Idf_Vaga ,
                F.Des_Funcao ,
                V.Qtd_Vaga ,
                V.Vlr_Salario_De ,
                V.Vlr_Salario_Para ,
                C.Nme_Cidade ,
                C.Sig_Estado,
                AB.Des_Area_BNE
        FROM    BNE.BNE_Vaga V WITH (NOLOCK)
                INNER JOIN plataforma.TAB_Funcao F WITH (NOLOCK) ON V.Idf_Funcao = F.Idf_Funcao
                INNER JOIN plataforma.TAB_Cidade C WITH (NOLOCK) ON V.Idf_Cidade = C.Idf_Cidade
                INNER JOIN plataforma.TAB_Area_BNE AB WITH(NOLOCK) ON AB.Idf_Area_BNE = F.Idf_Area_BNE
        /* VAGAS POR FUNCAO */
        UNION
        SELECT  NULL AS Idf_Vaga ,
                F.Des_Funcao ,
                NULL AS Qtd_Vaga ,
                NULL AS Vlr_Salario_De ,
                NULL AS Vlr_Salario_Para ,
                NULL AS Nme_Cidade ,
                NULL AS Sig_Estado,
                AB.Des_Area_BNE
        FROM    BNE.BNE_Vaga V WITH (NOLOCK)
                INNER JOIN plataforma.TAB_Funcao F WITH (NOLOCK) ON V.Idf_Funcao = F.Idf_Funcao
                INNER JOIN plataforma.TAB_Cidade C WITH (NOLOCK) ON V.Idf_Cidade = C.Idf_Cidade
                INNER JOIN plataforma.TAB_Area_BNE AB WITH(NOLOCK) ON AB.Idf_Area_BNE = F.Idf_Area_BNE
        GROUP BY F.Des_Funcao , AB.Des_Area_BNE
        /* VAGAS POR CIDADE */
        UNION
        SELECT  NULL AS Idf_Vaga ,
                NULL AS Des_Funcao ,
                NULL AS Qtd_Vaga ,
                NULL AS Vlr_Salario_De ,
                NULL AS Vlr_Salario_Para ,
                C.Nme_Cidade ,
                C.Sig_Estado,
                NULL AS Des_Area_BNE
        FROM    BNE.BNE_Vaga V WITH (NOLOCK)
                INNER JOIN plataforma.TAB_Funcao F WITH (NOLOCK) ON V.Idf_Funcao = F.Idf_Funcao
                INNER JOIN plataforma.TAB_Cidade C WITH (NOLOCK) ON V.Idf_Cidade = C.Idf_Cidade
                INNER JOIN plataforma.TAB_Area_BNE AB WITH(NOLOCK) ON AB.Idf_Area_BNE = F.Idf_Area_BNE
        GROUP BY C.Nme_Cidade , C.Sig_Estado
        /* VAGAS POR FUNCAO E CIDADE */
        UNION
        SELECT  NULL AS Idf_Vaga ,
                F.Des_Funcao ,
                NULL AS Qtd_Vaga ,
                NULL AS Vlr_Salario_De ,
                NULL AS Vlr_Salario_Para ,
                C.Nme_Cidade ,
                C.Sig_Estado,
                AB.Des_Area_BNE
        FROM    BNE.BNE_Vaga V WITH (NOLOCK)
                INNER JOIN plataforma.TAB_Funcao F WITH (NOLOCK) ON V.Idf_Funcao = F.Idf_Funcao
                INNER JOIN plataforma.TAB_Cidade C WITH (NOLOCK) ON V.Idf_Cidade = C.Idf_Cidade
                INNER JOIN plataforma.TAB_Area_BNE AB WITH(NOLOCK) ON AB.Idf_Area_BNE = F.Idf_Area_BNE
        GROUP BY F.Des_Funcao , AB.Des_Area_BNE, C.Nme_Cidade , C.Sig_Estado
        ORDER BY V.Idf_Vaga	ASC, F.Des_Funcao , C.Nme_Cidade , C.Sig_Estado
        ";
        #endregion

        #region SpUpdateAnunciarEmMassa
        private const string SpUpdateAnunciarEmMassa = @"
        UPDATE BNE_Vaga SET Flg_Vaga_Massa = @Flg_Vaga_Massa, Flg_BNE_Recomenda = @Flg_BNE_Recomenda, Flg_Liberada = @Flg_Liberada WHERE Idf_Vaga = @Idf_Vaga ";
        #endregion

        #region Splistarvagasnaopublicadasfilialemauditadoria
        private const string Splistarvagasnaopublicadasfilialemauditadoria = @"
        SELECT  V.*
        FROM    BNE_Vaga V WITH(NOLOCK)
        WHERE   V.Flg_Empresa_Em_Auditoria = 1
                AND V.Flg_Inativo = 0
                AND V.Flg_Vaga_Arquivada = 0
                AND V.Flg_Auditada = 0
                AND V.Idf_Filial = @Idf_Filial";
        #endregion

        #region Spinativar
        private const string Spinativar = "UPDATE BNE_Vaga SET Flg_Inativo = 1 WHERE Idf_Vaga = @Idf_Vaga";
        #endregion

        #region Spmontarurlvaga

        private const string Spmontarurlvaga = @"
        SELECT [BNE].[MontarUrlVaga] (
            @Nome_Funcao,
            @Nome_Area_BNE,
            @Nome_Cidade,
            @Sigla_Estado,
            @Idf_Vaga)";

        private const string SpmontarurlvagaPorIdfVaga = @"BNE.SP_MONTAR_URL_VAGA_POR_IDF_VAGA";

        #endregion

        #region Spselectcodigovaga
        private const string Spselectcodigovaga = "SELECT Cod_Vaga FROM BNE.BNE_Vaga WITH(NOLOCK) WHERE Idf_Vaga = @Idf_Vaga";
        #endregion

        #region SpQtdVagasPorFilial
        private const string SpQtdVagasPorFilial = @"
        select count(vg.Idf_Vaga)
        from bne.TAB_Filial fl with(nolock)
	        join bne.bne_vaga vg with(nolock) on fl.Idf_Filial = vg.Idf_Filial
        where fl.Idf_filial = @Idf_Filial
        ";
        #endregion

        #region SpVagasPoucosCandidatos
        private const string SpVagasPoucosCandidatos = @"
        select vg.Idf_Vaga
	        , vg.Eml_Vaga
            , vg.Idf_Usuario_Filial_Perfil
        from bne.bne_vaga vg with(nolock)
	        join bne.BNE_Vaga_Candidato vc with(nolock) on vg.Idf_Vaga = vc.Idf_Vaga
        where vg.Dta_Cadastro between @Dta_Inicio and @Dta_Fim
        group by vg.Idf_Vaga
	        , vg.Eml_Vaga
        having(count(vc.Idf_Curriculo) <= 5)        
        ";
        #endregion

        #region SpListaUsuariosCadastraramVagas
        /// <summary>
        /// Seleciona os usuários que cadastraram vagas a 7 dias atras, passando intervalo de datas por parametro
        /// </summary>
        private const string SpListaUsuariosCadastraramVagas = @"
        SELECT
	        Vaga.Idf_Usuario_Filial_Perfil,	
            PF.Idf_Pessoa_Fisica,		
	        PF.Nme_Pessoa,
            PF.Num_DDD_Celular,
            PF.Num_Celular,
	        UF.Eml_Comercial,					
            BNE.SF_Retorna_Vendedor_Responsavel(F.Num_CNPJ) AS EmailVendedorResponsavel
        FROM BNE.BNE_Vaga Vaga WITH(NOLOCK)
	        INNER JOIN BNE.TAB_Usuario_Filial_Perfil UFP WITH(NOLOCK) ON Vaga.Idf_Usuario_Filial_Perfil = UFP.Idf_Usuario_Filial_Perfil	 
			INNER JOIN BNE.TAB_Pessoa_Fisica PF WITH(NOLOCK) ON UFP.Idf_Pessoa_Fisica = PF.Idf_Pessoa_Fisica  	
			INNER JOIN BNE.BNE_Usuario_Filial UF WITH(NOLOCK) ON UF.Idf_Usuario_Filial_Perfil = UFP.Idf_Usuario_Filial_Perfil
			INNER JOIN BNE.TAB_Filial F WITH(NOLOCK) ON UFP.Idf_Filial = F.Idf_Filial	
        WHERE CONVERT(DATE, Vaga.Dta_Cadastro) BETWEEN CONVERT(DATE, @DataInicial) AND CONVERT(DATE, @DataFinal)
	        AND Vaga.Flg_Inativo = 0
	        AND Vaga.Flg_Auditada = 1
	        AND Vaga.Flg_Vaga_Arquivada = 0
            AND Vaga.Flg_Empresa_Em_Auditoria = 0     
			AND Vaga.Idf_Origem <> 2 --Vagas Importadas do Sine	       
            AND UFP.Flg_Inativo = 0
			AND ((PF.Num_DDD_Celular IS NOT NULL AND PF.Num_Celular IS NOT NULL)
		        OR UF.Eml_Comercial IS NOT NULL)
            AND F.Idf_Situacao_Filial IN (1, 2) -- 1 Publicado Empresa / 2 Publicado Agência
        GROUP BY Vaga.Idf_Usuario_Filial_Perfil, 
			PF.Idf_Pessoa_Fisica,
	        PF.Nme_Pessoa,
            PF.Num_DDD_Celular,
            PF.Num_Celular,
	        UF.Eml_Comercial,
			F.Nme_Fantasia,
			BNE.SF_Retorna_Vendedor_Responsavel(F.Num_CNPJ)";
        #endregion

        #region SpListaUltimasVagasCadastradasPorUsuario
        /// <summary>
        /// Seleciona os dados dos usuários que cadastraram vagas 7 dias atras
        /// </summary>
        private const string SpListaUltimasVagasCadastradasPorUsuario = @"
        SELECT TOP 5            
            Vaga.Idf_Vaga,
			Vaga.Cod_Vaga,
			Vaga.Des_Funcao,
	        Vaga.Idf_Usuario_Filial_Perfil,
			COUNT(VC.Idf_Vaga_Candidato) as [qtdeCvsNaoVistos]
        FROM BNE.BNE_Vaga Vaga WITH(NOLOCK)
	        INNER JOIN BNE.TAB_Usuario_Filial_Perfil UFP WITH(NOLOCK) ON Vaga.Idf_Usuario_Filial_Perfil = UFP.Idf_Usuario_Filial_Perfil
			INNER JOIN BNE.BNE_Vaga_Candidato VC WITH(NOLOCK) ON Vaga.Idf_Vaga = VC.Idf_Vaga
			LEFT JOIN BNE.BNE_Curriculo_Quem_Me_Viu QMV WITH(NOLOCK) ON VC.Idf_curriculo = QMV.Idf_curriculo AND QMV.Idf_Filial = Vaga.Idf_Filial			
        WHERE Vaga.Flg_Empresa_Em_Auditoria = 0
	        AND Vaga.Flg_Inativo = 0
	        AND Vaga.Flg_Auditada = 1
	        AND Vaga.Flg_Vaga_Arquivada = 0
            AND CONVERT(DATE, Vaga.Dta_Cadastro) BETWEEN CONVERT(DATE, @DataInicial) AND CONVERT(DATE, @DataFinal)
	        AND UFP.Flg_Inativo = 0
			AND VC.Flg_Inativo = 0
			AND QMV.Idf_Curriculo_Quem_Me_Viu IS NULL
			AND Vaga.Idf_Origem <> 2 --Vagas Importadas do Sine	
			AND Vaga.Idf_Usuario_Filial_Perfil = @Idf_Usuario_Filial_Perfil			
		GROUP BY Vaga.Idf_Vaga,
			Vaga.Cod_Vaga,
			Vaga.Des_Funcao,
	        Vaga.Idf_Usuario_Filial_Perfil
		ORDER BY Vaga.Idf_Vaga DESC";
        #endregion

        #endregion Consultas

        #region Métodos

        #region Insert
        /// <summary>
        /// Método utilizado para inserir uma instância de Vaga no banco de dados.
        /// </summary>
        /// <remarks>Gieyson Stelmak</remarks>
        private void Insert()
        {
            List<SqlParameter> parms = GetParameters();
            SetParameters(parms);

            using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, Spinsert, parms);
                        _idVaga = Convert.ToInt32(cmd.Parameters["@Idf_Vaga"].Value);
                        _codigoVaga = cmd.Parameters["@Cod_Vaga"].Value.ToString();
                        cmd.Parameters.Clear();
                        _persisted = true;
                        _modified = false;
                        trans.Commit();

                        this.AtualizaVagaDW();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }
        /// <summary>
        /// Método utilizado para inserir uma instância de Vaga no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        private void Insert(SqlTransaction trans)
        {
            List<SqlParameter> parms = GetParameters();
            SetParameters(parms);
            SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, Spinsert, parms);
            _idVaga = Convert.ToInt32(cmd.Parameters["@Idf_Vaga"].Value);
            _codigoVaga = cmd.Parameters["@Cod_Vaga"].Value.ToString();
            cmd.Parameters.Clear();
            _persisted = true;
            _modified = false;

            this.AtualizaVagaDW();
        }
        #endregion

        #region Update
        /// <summary>
        /// Método utilizado para atualizar uma instância de Vaga no banco de dados.
        /// </summary>
        /// <remarks>Gieyson Stelmak</remarks>
        private void Update()
        {
            if (this._modified)
            {
                List<SqlParameter> parms = GetParameters();
                SetParameters(parms);
                DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPUPDATE, parms);
                this._modified = false;

                this.AtualizaVagaDW();
            }
        }
        /// <summary>
        /// Método utilizado para atualizar uma instância de Vaga no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        private void Update(SqlTransaction trans)
        {
            if (this._modified)
            {
                List<SqlParameter> parms = GetParameters();
                SetParameters(parms);
                DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPUPDATE, parms);
                this._modified = false;

                this.AtualizaVagaDW();
            }
        }
        #endregion

        #region SetParameters
        /// <summary>
        /// Método auxiliar que recebe e preenche a lista de parâmetros passada de acordo com os valores da instância.
        /// </summary>
        /// <param name="parms">Lista de parâmetros SQL.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        private void SetParameters(List<SqlParameter> parms)
        {
            parms[0].Value = _idVaga;
            if (_funcao != null)
                parms[1].Value = _funcao.IdFuncao;
            else
                parms[1].Value = DBNull.Value;

            if (_cidade != null)
                parms[2].Value = _cidade.IdCidade;
            else
                parms[2].Value = DBNull.Value;

            if (!String.IsNullOrEmpty(_codigoVaga))
                parms[3].Value = _codigoVaga;
            else
                parms[3].Value = DBNull.Value;

            if (_valorSalarioDe.HasValue)
                parms[4].Value = _valorSalarioDe;
            else
                parms[4].Value = DBNull.Value;

            if (_dataAbertura.HasValue)
                parms[5].Value = _dataAbertura;
            else
                parms[5].Value = DBNull.Value;

            if (_dataPrazo.HasValue)
                parms[6].Value = _dataPrazo;
            else
                parms[6].Value = DBNull.Value;

            if (!String.IsNullOrEmpty(_emailVaga))
                parms[7].Value = _emailVaga;
            else
                parms[7].Value = DBNull.Value;

            if (!String.IsNullOrEmpty(_descricaoRequisito))
                parms[8].Value = _descricaoRequisito;
            else
                parms[8].Value = DBNull.Value;

            if (_quantidadeVaga.HasValue)
                parms[9].Value = _quantidadeVaga;
            else
                parms[9].Value = DBNull.Value;

            if (!String.IsNullOrEmpty(_nomeEmpresa))
                parms[10].Value = _nomeEmpresa;
            else
                parms[10].Value = DBNull.Value;

            parms[11].Value = _flagVagaRapida;
            parms[13].Value = _flagInativo;

            if (_filial != null)
                parms[14].Value = _filial.IdFilial;
            else
                parms[14].Value = DBNull.Value;

            parms[15].Value = _flagConfidencial;
            parms[16].Value = _usuarioFilialPerfil.IdUsuarioFilialPerfil;

            if (_escolaridade != null)
                parms[17].Value = _escolaridade.IdEscolaridade;
            else
                parms[17].Value = DBNull.Value;

            if (_numeroIdadeMinima.HasValue)
                parms[18].Value = _numeroIdadeMinima;
            else
                parms[18].Value = DBNull.Value;

            if (_numeroIdadeMaxima.HasValue)
                parms[19].Value = _numeroIdadeMaxima;
            else
                parms[19].Value = DBNull.Value;

            if (_sexo != null)
                parms[20].Value = _sexo.IdSexo;
            else
                parms[20].Value = DBNull.Value;

            if (!String.IsNullOrEmpty(_descricaoBeneficio))
                parms[21].Value = _descricaoBeneficio;
            else
                parms[21].Value = DBNull.Value;

            if (!String.IsNullOrEmpty(_descricaoAtribuicoes))
                parms[22].Value = _descricaoAtribuicoes;
            else
                parms[22].Value = DBNull.Value;

            if (!String.IsNullOrEmpty(_numeroDDD))
                parms[23].Value = _numeroDDD;
            else
                parms[23].Value = DBNull.Value;

            if (!String.IsNullOrEmpty(_numeroTelefone))
                parms[24].Value = _numeroTelefone;
            else
                parms[24].Value = DBNull.Value;

            if (_flagReceberCadaCV.HasValue)
                parms[25].Value = _flagReceberCadaCV;
            else
                parms[25].Value = DBNull.Value;

            if (_flagReceberTodosCV.HasValue)
                parms[26].Value = _flagReceberTodosCV;
            else
                parms[26].Value = DBNull.Value;

            if (!String.IsNullOrEmpty(_descricaoFuncao))
                parms[27].Value = _descricaoFuncao;
            else
                parms[27].Value = DBNull.Value;

            if (_flagAuditada.HasValue)
                parms[28].Value = _flagAuditada;
            else
                parms[28].Value = DBNull.Value;

            parms[29].Value = _flagBNERecomenda;
            parms[30].Value = _flagVagaArquivada;
            parms[31].Value = _flagVagaMassa;

            if (_origem != null)
                parms[32].Value = _origem.IdOrigem;
            else
                parms[32].Value = DBNull.Value;

            if (_flagLiberada.HasValue)
                parms[33].Value = _flagLiberada;
            else
                parms[33].Value = DBNull.Value;

            if (_deficiencia != null)
                parms[34].Value = _deficiencia.IdDeficiencia;
            else
                parms[34].Value = DBNull.Value;

            if (_dataAuditoria.HasValue)
                parms[35].Value = _dataAuditoria;
            else
                parms[35].Value = DBNull.Value;

            if (_valorSalarioPara.HasValue)
                parms[36].Value = _valorSalarioPara;
            else
                parms[36].Value = DBNull.Value;

            if (_flagDeficiencia.HasValue)
                parms[37].Value = this._flagDeficiencia;
            else
                parms[37].Value = DBNull.Value;

            parms[38].Value = _flagEmpresaEmAuditoria;

            if (!_persisted)
            {
                parms[0].Direction = ParameterDirection.Output;
                _dataCadastro = DateTime.Now;

                /* Sempre colocar Output no código*/
                parms[3].Direction = ParameterDirection.Output;
            }
            else
            {
                parms[0].Direction = ParameterDirection.Input;
                parms[3].Direction = ParameterDirection.Input;
            }
            parms[12].Value = _dataCadastro;
        }
        #endregion

        #region SalvarVaga
        /// <summary>
        /// 
        /// </summary>
        /// <param name="listVagaDisponibilidade"></param>
        /// <param name="listVagaTipoVinculo"></param>
        /// <param name="listVagaPergunta"></param>
        /// <param name="listPalavrasChave"></param>
        /// <param name="auditada">Define se uma vaga foi auditada agora ou não, para realizar o envio desta vaga para os currículos no perfil</param>
        public void SalvarVaga(List<VagaDisponibilidade> listVagaDisponibilidade, List<VagaTipoVinculo> listVagaTipoVinculo, List<VagaPergunta> listVagaPergunta, List<string> listPalavrasChave, bool auditada)
        {
            using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();

                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        //Verificando se é um novo cadastro
                        bool novoCadastro = !_persisted;

                        Save(trans);

                        var listVagaPalavraChave = new List<string>();
                        foreach (var palavraChave in listPalavrasChave)
                        {
                            var palavra = palavraChave.Trim();
                            VagaPalavraChave objVagaPalavraChave;
                            if (!VagaPalavraChave.CarregarPorPalavraChave(palavra, this, out objVagaPalavraChave, trans))
                            {
                                PalavraChave objPalavraChave;
                                if (!PalavraChave.CarregarPorDescricao(palavra, out objPalavraChave, trans))
                                {
                                    objPalavraChave = new PalavraChave
                                    {
                                        DescricaoPalavraChave = palavra,
                                        FlagInativo = false
                                    };
                                    objPalavraChave.Save(trans);
                                }
                                objVagaPalavraChave = new VagaPalavraChave
                                {
                                    PalavraChave = objPalavraChave,
                                    Vaga = this,
                                    FlagInativo = false
                                };
                                objVagaPalavraChave.Save(trans);
                            }
                            listVagaPalavraChave.Add(objVagaPalavraChave.IdVagaPalavraChave.ToString(CultureInfo.CurrentCulture));
                        }

                        VagaPalavraChave.AtualizaPalavrasChaveDaVaga(this, listVagaPalavraChave, trans);

                        VagaDisponibilidade.DeletePorVaga(_idVaga, trans);
                        foreach (VagaDisponibilidade objVagaDisponibilidade in listVagaDisponibilidade)
                        {
                            objVagaDisponibilidade.Save(trans);
                        }

                        VagaTipoVinculo.DeletePorVaga(_idVaga, trans);
                        foreach (VagaTipoVinculo objVagaTipoVinculo in listVagaTipoVinculo)
                        {
                            objVagaTipoVinculo.Save(trans);
                        }

                        foreach (VagaPergunta objVagaPergunta in listVagaPergunta)
                        {
                            objVagaPergunta.Save(trans);
                        }

                        DataTable dtPerguntasSalvas = VagaPergunta.RecuperarPerguntas(_idVaga, trans);

                        if (dtPerguntasSalvas != null)
                        {
                            foreach (DataRow dr in dtPerguntasSalvas.Rows)
                            {
                                bool excluir = true;

                                foreach (VagaPergunta objVagaPergunta in listVagaPergunta)
                                {
                                    if (dr["Idf_Vaga_Pergunta"].ToString().Equals(objVagaPergunta.IdVagaPergunta.ToString(CultureInfo.CurrentCulture)))
                                        excluir = false;
                                }

                                if (excluir)
                                    VagaPergunta.Inativar(Convert.ToInt32(dr["Idf_Vaga_Pergunta"]), trans);
                            }
                        }

                        EnvioEmailWebEstagiosIntegracao(listVagaTipoVinculo.ToArray());

                        if (novoCadastro)
                        {
                            //TODO: Performance: Manter os parametros em cache
                            int dias = Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.QuantidadeDiasPrazoVaga, trans));

                            //TODO: Performance
                            Filial.CompleteObject(trans);

                            //Salvando a mensagem de acordo com as regras de envio.
                            string assunto;
                            string template = CartaEmail.RetornarConteudoBNE(Enumeradores.CartaEmail.CadastroVaga, out assunto);
                            var parametros = new
                            {
                                Dias = dias > 1 ? dias + " dias" : dias + " dia",
                                Nome = Filial.RazaoSocial
                            };
                            string mensagem = parametros.ToString(template);

                            EmailSenderFactory
                                .Create(TipoEnviadorEmail.Fila)
                                .Enviar(assunto, mensagem, Parametro.RecuperaValorParametro(Enumeradores.Parametro.EmailMensagens), EmailVaga, trans);
                        }

                        if (auditada)
                            DispararPluginEnvioCandidatoVagaPerfil();

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

        #region DispararPluginEnvioCandidatoVagaPerfil

        public void DispararPluginEnvioCandidatoVagaPerfil()
        {
            if (Filial.PossuiPlanoAtivo())
            {
                var parametrosAtividade = new ParametroExecucaoCollection 
                                    {
                                        {"idVaga","Vaga", _idVaga.ToString(CultureInfo.InvariantCulture), _codigoVaga}
                                    };

                ProcessoAssincrono.IniciarAtividade(
                    BNE.BLL.AsyncServices.Enumeradores.TipoAtividade.EnvioCandidatoVagaPerfil,
                    PluginsCompatibilidade.CarregarPorMetadata("EnvioCandidatoVagaPerfil", "PluginSaidaEmailSMSTanque"),
                    parametrosAtividade,
                    null,
                    null,
                    null,
                    null,
                    DateTime.Now);
            }
        }

        #endregion

        public Vaga CloneManualSuperficial()
        {
            // necessário testar
            var v1 = new Vaga();

            var prop = v1.GetType().GetProperties();

            foreach (var item in prop)
            {
                if (item.CanRead && item.CanWrite)
                {
                    item.SetValue(v1, item.GetValue(v1, null), null);
                }
            }

            return v1;
        }

        #region [ Integração Web Estágios ]
        private void EnvioEmailWebEstagiosIntegracao(VagaTipoVinculo[] listVagTipoVinculo)
        {
            if (listVagTipoVinculo == null || listVagTipoVinculo.Length == 0)
                return;

            var safeTipoVinculo = listVagTipoVinculo.Where(obj => obj.TipoVinculo != null);
            if (safeTipoVinculo.All(obj => obj.TipoVinculo.IdTipoVinculo != (int)Enumeradores.TipoVinculo.Estágio
                     && obj.TipoVinculo.IdTipoVinculo != (int)Enumeradores.TipoVinculo.Aprendiz))
                return;

            Enumeradores.Parametro newParam = Enumeradores.Parametro.EmailWebEstagiosIntegracaoAnuncioVaga;

            var parms = new List<Enumeradores.Parametro>
             {
                 newParam
             };

            var valores = Parametro.ListarParametros(parms);
            var emailLst = valores[newParam];

            if (string.IsNullOrEmpty(emailLst))
                return;

            Enumeradores.CartaEmail newCarta = Enumeradores.CartaEmail.ConteudoWebEstagiosIntegracaoAnuncioVaga;

            string assunto;
            string conteudo = CartaEmail.RetornarConteudoBNE(newCarta, out assunto);

            if (string.IsNullOrEmpty(conteudo))
                return;

            var cnpj = !string.IsNullOrEmpty(this.Filial.CNPJ)
                            ? this.Filial.CNPJ :
                            (this.Filial.CompleteObject() ? this.Filial.CNPJ : "(CNPJ não disponível)");

            var parametros = new
            {
                NomeEmpresa = this.NomeEmpresa,
                NomeFuncao = this.Funcao.DescricaoFuncao,
                CodigoVaga = this.CodigoVaga,
                CNPJ = cnpj,
                TipoVinculo = safeTipoVinculo.Select(obj => PegarDescricao(obj.TipoVinculo)).Aggregate((a, b) => a + ", " + b)
            };
            string mensagemEmail = parametros.ToString(conteudo);

            string emailRemetenteSistema = Parametro.RecuperaValorParametro(Enumeradores.Parametro.EmailMensagens);

            foreach (var item in emailLst.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
            {
                EmailSenderFactory.Create(TipoEnviadorEmail.Fila)
                          .Enviar(assunto, mensagemEmail, emailRemetenteSistema, item);
            }
        }

        private string PegarDescricao(TipoVinculo arg)
        {
            if (!string.IsNullOrEmpty(arg.DescricaoTipoVinculo))
                return arg.DescricaoTipoVinculo;

            if (arg.CompleteObject())
                return arg.DescricaoTipoVinculo;

            return "Vínculo Indisponível";
        }
        #endregion

        #region ListarVagasPaginaInicial
        /// <summary>
        /// Método responsável por retornar uma lista com as Rede social conta divulgadoras de vaga
        /// </summary>
        /// <returns></returns>
        public static DataTable ListarVagasPaginaInicial()
        {
            return DataAccessLayer.ExecuteReaderDs(CommandType.Text, Spselectvagas, null).Tables[0];
        }
        #endregion

        #region ListarVagasFilial
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados.
        /// </summary>
        /// <returns>DataTable</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static DataTable ListarVagasFilial(int idFilial, int? idOrigem, int paginaCorrente, int tamanhoPagina, bool? anunciadas, int? idUsuarioFilialPerfil, List<int> listIdFuncoes, out int totalRegistros)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter("@Idf_Filial", SqlDbType.Int, 4), 
                    new SqlParameter("@CurrentPage", SqlDbType.Int, 4), 
                    new SqlParameter("@PageSize", SqlDbType.Int, 4),
                    new SqlParameter("@Flg_Vagas_Inativas", SqlDbType.Bit)
                };

            parms[0].Value = idFilial;
            parms[1].Value = paginaCorrente;
            parms[2].Value = tamanhoPagina;

            if (anunciadas.HasValue)
                parms[3].Value = (bool)anunciadas ? 1 : 0;
            else
                parms[3].Value = DBNull.Value;

            var param = new SqlParameter { ParameterName = "@ids", SqlDbType = SqlDbType.VarChar, Size = 1600, Value = DBNull.Value };

            if (listIdFuncoes != null && listIdFuncoes.Count > 0)
                param.Value = String.Join(",", listIdFuncoes.Select(item => item.ToString(CultureInfo.CurrentCulture)).ToArray());

            parms.Add(param);

            var parametroOrigem = new SqlParameter { ParameterName = "@Idf_Origem", SqlDbType = SqlDbType.Int, Size = 4, Value = DBNull.Value };

            if (idOrigem.HasValue)
                parametroOrigem.Value = idOrigem.Value;

            parms.Add(parametroOrigem);

            var parametroUsuario = new SqlParameter { ParameterName = "@Idf_Usuario_Filial_Perfil", SqlDbType = SqlDbType.Int, Size = 4, Value = DBNull.Value };

            if (idUsuarioFilialPerfil.HasValue)
                parametroUsuario.Value = idUsuarioFilialPerfil.Value;

            parms.Add(parametroUsuario);

            totalRegistros = 0;
            DataTable dt = null;
            try
            {
                using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.StoredProcedure, "[BNE].[VAGAS_SALASELECIONADOR]", parms))
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

        #region ListarFuncoesVagasFilial
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados.
        /// </summary>
        /// <returns>DataTable</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static IDataReader ListarFuncoesVagasFilial(int idFilial, bool? anunciadas)
        {
            Object paramAnunciadas = DBNull.Value;

            if (anunciadas.HasValue)
                paramAnunciadas = (bool)anunciadas ? 1 : 0;

            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Filial", SqlDbType = SqlDbType.Int, Size = 4, Value = idFilial } ,
                    new SqlParameter { ParameterName = "@BitVagasAnunciadas", SqlDbType = SqlDbType.Bit, Value = paramAnunciadas }
                };

            return DataAccessLayer.ExecuteReader(CommandType.Text, Spselectfuncoesvagasporfilial, parms);
        }
        #endregion

        #region ExistePerguntas
        public bool ExistePerguntas()
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Vaga", SqlDbType = SqlDbType.Int, Size = 4, Value = _idVaga }
                };

            return (int)DataAccessLayer.ExecuteScalar(CommandType.Text, Spselectcountperguntas, parms) > 0;
        }
        #endregion

        #region ArquivarVagas
        /// <summary>
        /// Metodo utilizado pelo serviço ArquivarVaga
        /// </summary>
        public static void ArquivarVagas()
        {
            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spvagasnaoarquivadas, null))
            {
                while (dr.Read())
                {
                    var objVaga = new Vaga();
                    if (SetInstanceNonDispose(dr, objVaga))
                    {
                        objVaga.FlagVagaArquivada = true;
                        objVaga.Save();
                    }
                }
            }
        }
        #endregion

        #region ListarVagasEmMassa
        /// <summary>
        /// Lista todas as vagas marcadas para envio em massa
        /// </summary>
        /// <returns>Uma DataTable com as vagas selecionadas na rotina</returns>
        public static DataTable ListarVagasEmMassa()
        {
            return DataAccessLayer.ExecuteReaderDs(CommandType.Text, Spvagasemmassa, null).Tables[0];
        }
        #endregion

        #region ListarTwittersPorVaga
        /// <summary>
        /// Lista todas as vagas marcadas para envio em massa
        /// </summary>
        /// <returns>Uma DataTable com as vagas selecionadas na rotina</returns>
        public static DataTable ListarTwitterPorVaga(int idVaga)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Vaga", SqlDbType = SqlDbType.Int, Size = 4, Value = idVaga }
                };

            return DataAccessLayer.ExecuteReaderDs(CommandType.Text, Spvagasemmassatwitter, parms).Tables[0];
        }
        #endregion

        #region ListarVagasSalaAdministrador
        /// <summary>
        /// Lista as vagas da sala do administrador
        /// </summary>
        /// <param name="filtro">O filtro a ser aplicado nas colunas</param>
        /// <param name="naoAuditadas">Se trás ou não somente as vagas não auditadas</param>
        /// <returns>A datatable contendo os resultados</returns>
        public static DataTable ListarVagasSalaAdministrador(String filtro, bool naoAuditadas = false)
        {
            var param = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@filtro", SqlDbType = SqlDbType.VarChar, Value = filtro }
                };

            if (naoAuditadas)
                return DataAccessLayer.ExecuteReaderDs(CommandType.Text, Spselectsalaadministradorvagasnaoauditadas, param).Tables[0];

            return DataAccessLayer.ExecuteReaderDs(CommandType.Text, Spselectsalaadministradorvagas, param).Tables[0];
        }
        #endregion

        #region UltimaVagaFilialPerfil
        public static Vaga UltimaVagaFilialPerfil(int idFilial, SqlTransaction trans = null)
        {
            var param = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Filial", SqlDbType = SqlDbType.Int, Value = idFilial }
                };

            using (var dr = trans == null
                    ? DataAccessLayer.ExecuteReader(CommandType.Text, SpSelectFilialUltimaVaga, param)
                    : DataAccessLayer.ExecuteReader(trans, CommandType.Text, SpSelectFilialUltimaVaga, param))
            {
                if (dr.Read())
                {
                    var vaga = new Vaga(Convert.ToInt32(dr["Idf_Vaga"].ToString()));
                    if (trans == null)
                        vaga.CompleteObject();
                    else
                        vaga.CompleteObject(trans);

                    return vaga;
                }
                return null;
            }
        }
        #endregion

        #region InativarVaga
        /// <summary>
        /// Marca uma vaga como inativa
        /// </summary>
        public static void InativarVaga(int idVaga)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Vaga", SqlDbType = SqlDbType.Int, Size = 4, Value = idVaga }
                };

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, Spinativar, parms);
        }
        #endregion

        #region RetornarTituloPagina
        public string RetornarTituloPagina()
        {
            _cidade.CompleteObject();
            _funcao.CompleteObject();
            return string.Format("Vaga de {0} - {1}/{2} - {3}", _funcao.DescricaoFuncao, _cidade.NomeCidade, _cidade.Estado.SiglaEstado, FormataValorSalarioVaga(_valorSalarioDe, _valorSalarioPara));
        }
        #endregion

        #region FormataValorSalarioVaga
        private static string FormataValorSalarioVaga(decimal? valorSalarioDe, decimal? valorSalarioAte)
        {
            if (valorSalarioDe.HasValue && valorSalarioAte.HasValue)
                return string.Format("{0} a {1}", valorSalarioDe.Value.ToString("N2"), valorSalarioAte.Value.ToString("N2"));

            return "A Combinar";
        }
        #endregion

        #region RecuperarVagasSiteMap
        public static IEnumerable<VagaSitemap> RecuperarVagasSiteMap()
        {
            var listSiteMap = new List<VagaSitemap>();
            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SpRecuperarVagasSiteMap, null))
            {
                while (dr.Read())
                {
                    listSiteMap.Add(new VagaSitemap
                    {
                        IdfVaga = dr["Idf_Vaga"] != DBNull.Value ? Convert.ToInt32(dr["Idf_Vaga"]) : (int?)null,
                        DescricaoFuncao = dr["Des_Funcao"].ToString(),
                        QuantidadeVaga = dr["Qtd_Vaga"] != DBNull.Value ? Convert.ToInt32(dr["Qtd_Vaga"]) : 0,
                        NomeCidade = dr["Nme_Cidade"].ToString(),
                        SiglaEstado = dr["Sig_Estado"].ToString(),
                        DescricaoAreaBNE = dr["Des_Area_BNE"].ToString()
                    });
                }
            }

            return listSiteMap;
        }
        #endregion

        #region AnunciarEmMassa
        public void AnunciarEmMassa()
        {
            var parms = new List<SqlParameter>
                            {
                                new SqlParameter("@Idf_Vaga", SqlDbType.Int, 4),
                                new SqlParameter("@Flg_Vaga_Massa", SqlDbType.Bit, 1),
                                new SqlParameter("@Flg_BNE_Recomenda", SqlDbType.Bit, 1),
                                new SqlParameter("@Flg_Liberada", SqlDbType.Bit, 1)
                            };

            parms[0].Value = _idVaga;
            parms[1].Value = true;
            parms[2].Value = true;
            parms[3].Value = true;

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, SpUpdateAnunciarEmMassa, parms);
        }
        #endregion

        #region RetornarProtocolo
        public static string RetornarProtocolo(int idCurriculo, string codigoVaga)
        {
            return String.Format("{0}{1}{2}", idCurriculo, codigoVaga, DateTime.Today.ToString("yyyyMMdd"));
        }
        #endregion

        #region SetInstanceNonDispose
        /// <summary>
        /// Método auxiliar utilizado pelos métodos LoadObject e CompleteObject para percorrer um IDataReader e vincular as colunas com os atributos da classe.
        /// </summary>
        /// <param name="dr">Cursor de leitura do banco de dados.</param>
        /// <param name="objVaga">InstÃ¢ncia a ser manipulada.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static bool SetInstanceNonDispose(IDataReader dr, Vaga objVaga)
        {
            objVaga._idVaga = Convert.ToInt32(dr["Idf_Vaga"]);
            if (dr["Idf_Funcao"] != DBNull.Value)
                objVaga._funcao = new Funcao(Convert.ToInt32(dr["Idf_Funcao"]));
            objVaga._cidade = new Cidade(Convert.ToInt32(dr["Idf_Cidade"]));
            if (dr["Cod_Vaga"] != DBNull.Value)
                objVaga._codigoVaga = Convert.ToString(dr["Cod_Vaga"]);
            if (dr["Vlr_Salario_De"] != DBNull.Value)
                objVaga._valorSalarioDe = Convert.ToDecimal(dr["Vlr_Salario_De"]);
            if (dr["Vlr_Salario_Para"] != DBNull.Value)
                objVaga._valorSalarioPara = Convert.ToDecimal(dr["Vlr_Salario_Para"]);
            if (dr["Dta_Abertura"] != DBNull.Value)
                objVaga._dataAbertura = Convert.ToDateTime(dr["Dta_Abertura"]);
            if (dr["Dta_Prazo"] != DBNull.Value)
                objVaga._dataPrazo = Convert.ToDateTime(dr["Dta_Prazo"]);
            if (dr["Eml_Vaga"] != DBNull.Value)
                objVaga._emailVaga = Convert.ToString(dr["Eml_Vaga"]);
            if (dr["Des_Requisito"] != DBNull.Value)
                objVaga._descricaoRequisito = Convert.ToString(dr["Des_Requisito"]);
            if (dr["Qtd_Vaga"] != DBNull.Value)
                objVaga._quantidadeVaga = Convert.ToInt16(dr["Qtd_Vaga"]);
            if (dr["Nme_Empresa"] != DBNull.Value)
                objVaga._nomeEmpresa = Convert.ToString(dr["Nme_Empresa"]);
            objVaga._flagVagaRapida = Convert.ToBoolean(dr["Flg_Vaga_Rapida"]);
            objVaga._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
            objVaga._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
            if (dr["Idf_Filial"] != DBNull.Value)
                objVaga._filial = new Filial(Convert.ToInt32(dr["Idf_Filial"]));
            objVaga._flagConfidencial = Convert.ToBoolean(dr["Flg_Confidencial"]);
            objVaga._usuarioFilialPerfil = new UsuarioFilialPerfil(Convert.ToInt32(dr["Idf_Usuario_Filial_Perfil"]));
            if (dr["Idf_Escolaridade"] != DBNull.Value)
                objVaga._escolaridade = new Escolaridade(Convert.ToInt32(dr["Idf_Escolaridade"]));
            if (dr["Num_Idade_Minima"] != DBNull.Value)
                objVaga._numeroIdadeMinima = Convert.ToInt16(dr["Num_Idade_Minima"]);
            if (dr["Num_Idade_Maxima"] != DBNull.Value)
                objVaga._numeroIdadeMaxima = Convert.ToInt16(dr["Num_Idade_Maxima"]);
            if (dr["Idf_Sexo"] != DBNull.Value)
                objVaga._sexo = new Sexo(Convert.ToInt32(dr["Idf_Sexo"]));
            if (dr["Des_Beneficio"] != DBNull.Value)
                objVaga._descricaoBeneficio = Convert.ToString(dr["Des_Beneficio"]);
            if (dr["Des_Atribuicoes"] != DBNull.Value)
                objVaga._descricaoAtribuicoes = Convert.ToString(dr["Des_Atribuicoes"]);
            if (dr["Num_DDD"] != DBNull.Value)
                objVaga._numeroDDD = Convert.ToString(dr["Num_DDD"]);
            if (dr["Num_Telefone"] != DBNull.Value)
                objVaga._numeroTelefone = Convert.ToString(dr["Num_Telefone"]);
            if (dr["Flg_Receber_Cada_CV"] != DBNull.Value)
                objVaga._flagReceberCadaCV = Convert.ToBoolean(dr["Flg_Receber_Cada_CV"]);
            if (dr["Flg_Receber_Todos_CV"] != DBNull.Value)
                objVaga._flagReceberTodosCV = Convert.ToBoolean(dr["Flg_Receber_Todos_CV"]);
            if (dr["Des_Funcao"] != DBNull.Value)
                objVaga._descricaoFuncao = Convert.ToString(dr["Des_Funcao"]);
            if (dr["Flg_Auditada"] != DBNull.Value)
                objVaga._flagAuditada = Convert.ToBoolean(dr["Flg_Auditada"]);
            objVaga._flagBNERecomenda = Convert.ToBoolean(dr["Flg_BNE_Recomenda"]);
            objVaga._flagVagaArquivada = Convert.ToBoolean(dr["Flg_Vaga_Arquivada"]);
            objVaga._flagVagaMassa = Convert.ToBoolean(dr["Flg_Vaga_Massa"]);
            if (dr["Idf_Origem"] != DBNull.Value)
                objVaga._origem = new Origem(Convert.ToInt32(dr["Idf_Origem"]));
            if (dr["Flg_Liberada"] != DBNull.Value)
                objVaga._flagLiberada = Convert.ToBoolean(dr["Flg_Liberada"]);
            if (dr["Idf_Deficiencia"] != DBNull.Value)
                objVaga._deficiencia = new Deficiencia(Convert.ToInt32(dr["Idf_Deficiencia"]));
            if (dr["Dta_Auditoria"] != DBNull.Value)
                objVaga._dataAuditoria = Convert.ToDateTime(dr["Dta_Auditoria"]);

            objVaga._persisted = true;
            objVaga._modified = false;

            return true;
        }
        #endregion

        #region ArquivarVaga
        public void ArquivarVaga()
        {
            FlagVagaArquivada = true;
            Save();
        }
        #endregion

        #region AtivarVaga
        public void AtivarVaga()
        {
            FlagVagaArquivada = false;
            DataAbertura = DateTime.Now;
            DataPrazo = DataAbertura.Value.AddDays(Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.QuantidadeDiasPrazoVaga)));
            FlagAuditada = false;

            Save();
        }
        #endregion

        #region Publicacar
        public void Publicacar()
        {
            /*
            var parametros = new ParametroExecucaoCollection
                    {
                        {"idVaga", "Vaga", objVaga.IdVaga.ToString(CultureInfo.InvariantCulture), objVaga.CodigoVaga},
                        {"EnfileraRastreador", "Deve enfileirar rastreador", "true", "Verdadeiro"}
                    };

            ProcessoAssincrono.IniciarAtividade(
                Enumeradores.TipoAtividade.PublicacaoVaga,
                PluginsCompatibilidade.CarregarPorMetadata("PublicacaoVaga", "PublicacaoVagaRastreador"),
                parametros,
                null,
                null,
                null,
                null,
                DateTime.Now,
                null);
             * */
        }
        #endregion

        #region ListarVagasFilialEmAuditoria
        /// <summary>
        /// Retorna todas as vagas cadastradas pela empresa em processo de auditoria
        /// </summary>
        /// <param name="objFilial"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        public static List<Vaga> ListarVagasFilialEmAuditoria(Filial objFilial, SqlTransaction trans)
        {
            var param = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Filial", SqlDbType = SqlDbType.Int, Size = 4, Value = objFilial.IdFilial }
                };

            var listaVaga = new List<Vaga>();
            using (IDataReader dr = DataAccessLayer.ExecuteReader(trans, CommandType.Text, Splistarvagasnaopublicadasfilialemauditadoria, param))
            {
                while (dr.Read())
                {
                    var objVaga = new Vaga();
                    if (SetInstanceNonDispose(dr, objVaga))
                        listaVaga.Add(objVaga);
                }
            }

            return listaVaga;
        }
        #endregion

        #region Montar URL amigável (SEO)
        public static string MontarUrlVaga(int idVaga, string nomeFuncao, string nomeAreaBNE, string nomeCidade, string siglaEstado)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter() { ParameterName = "@Idf_Vaga", SqlDbType = SqlDbType.Int, Size = 4, Value = idVaga });
            parms.Add(new SqlParameter() { ParameterName = "@Nome_Funcao", SqlDbType = SqlDbType.NVarChar, Size = 4000, Value = nomeFuncao });
            parms.Add(new SqlParameter() { ParameterName = "@Nome_Area_BNE", SqlDbType = SqlDbType.NVarChar, Size = 4000, Value = nomeAreaBNE });
            parms.Add(new SqlParameter() { ParameterName = "@Nome_Cidade", SqlDbType = SqlDbType.NVarChar, Size = 4000, Value = nomeCidade });
            parms.Add(new SqlParameter() { ParameterName = "@Sigla_Estado", SqlDbType = SqlDbType.NVarChar, Size = 4000, Value = siglaEstado });

            return DataAccessLayer.ExecuteScalar(CommandType.Text, Spmontarurlvaga, parms) as string;
        }
        /// <summary>
        /// Monta a Url da Vaga de acordo com o Idf informado
        /// </summary>
        /// <param name="idVaga">Identificador da Vaga</param>
        /// <returns>A Url da Vaga</returns>
        /// <remarks>Luan Fernandes</remarks>
        public static string MontarUrlVaga(int idVaga)
        {
            string urlVaga = null;
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter() { ParameterName = "@Idf_Vaga", SqlDbType = SqlDbType.Int, Size = 4, Value = idVaga });

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.StoredProcedure, SpmontarurlvagaPorIdfVaga, parms))
            {
                if (dr.Read())
                {
                    urlVaga = Convert.ToString(dr["Url_Vaga"]);
                }
            }
            return urlVaga;
        }
        #endregion

        public bool Equals(Vaga vaga)
        {
            if (this.Cidade != null && vaga.Cidade != null && this.Cidade.IdCidade != vaga.Cidade.IdCidade)
                return false;
            if (this.Funcao != null && vaga.Funcao != null && this.Funcao.IdFuncao != vaga.Funcao.IdFuncao)
                return false;
            if (this.NomeEmpresa != vaga.NomeEmpresa)
                return false;
            Regex r = new Regex("[\n\t]");
            if (r.Replace(this.DescricaoAtribuicoes.ToLower(), "") != r.Replace(vaga.DescricaoAtribuicoes.ToLower(), ""))
                return false;

            return true;
        }

        #region ListarVagasSemelhantes
        /// <summary>
        /// Lista vagas semelhantes para a vaga atual
        /// </summary>
        /// <returns>Uma DataReader com as vagas</returns>
        public static List<DTO.VagaSemelhante> ListarVagasSemelhantes(Vaga objVaga)
        {
            var lista = new List<DTO.VagaSemelhante>();

            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Vaga", SqlDbType = SqlDbType.Int, Size = 4, Value = objVaga.IdVaga }
                };

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.StoredProcedure, "BNE.BNE_Buscar_Vagas_Semelhantes", parms))
            {
                while (dr.Read())
                    lista.Add(new VagaSemelhante { Id = Convert.ToInt32(dr["Idf_Vaga"]), Funcao = dr["Des_Funcao"].ToString(), NomeCidade = dr["Nme_Cidade"].ToString(), SiglaEstado = dr["Sig_Estado"].ToString(), AreaBNE = dr["Des_Area_BNE"].ToString() });
            }

            return lista;
        }
        #endregion

        #region RecuperarCodigo
        /// <summary>
        /// Método que retorna o código da vaga
        /// </summary>
        /// <returns>Cod_Vaga</returns>
        public string RecuperarCodigo()
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Vaga", SqlDbType = SqlDbType.Int, Size = 4, Value = _idVaga }
                };

            return Convert.ToString(DataAccessLayer.ExecuteScalar(CommandType.Text, Spselectcodigovaga, parms));
        }
        #endregion

        #region RetornarKeywords
        /// <summary>
        /// Retonar as keywords SEO da vaga
        /// </summary>
        /// <returns></returns>
        public string RetornarKeywords()
        {
            var parametros = new
            {
                Funcao = this.Funcao.DescricaoFuncao,
                Cidade = Helper.FormatarCidade(this.Cidade.NomeCidade, this.Cidade.Estado.SiglaEstado),
                NomeCidade = this.Cidade.NomeCidade,
                SiglaEstado = this.Cidade.Estado.SiglaEstado
            };

            return parametros.ToString("{Funcao}, {NomeCidade}, vaga de {Funcao}, vaga de {Funcao} em {Cidade}");
        }
        #endregion

        #region RetornarDescription
        /// <summary>
        /// Retonar a description SEO da vaga
        /// </summary>
        /// <returns></returns>
        public string RetornarDescription()
        {
            var parametros = new
            {
                Funcao = this.Funcao.DescricaoFuncao,
                Cidade = Helper.FormatarCidade(this.Cidade.NomeCidade, this.Cidade.Estado.SiglaEstado),
                Salario = Helper.RetornarDesricaoSalario(this.ValorSalarioDe, this.ValorSalarioPara),
                CodigoVaga = this.CodigoVaga
            };

            return parametros.ToString("Procurando emprego? Candidate-se para a vaga de {Funcao} em {Cidade}. Salário {Salario}. {CodigoVaga}. Banco Nacional de Empregos | BNE.");
        }
        #endregion

        #region RecuperarQtdVagasFilial
        public static int RecuperarQtdVagasFilial(int idFilial)
        {
            var parms = new List<SqlParameter> 
                {
                    new SqlParameter{ ParameterName = "@Idf_Filial", SqlDbType = SqlDbType.Int, Value = idFilial }
                };

            return Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SpQtdVagasPorFilial, parms));
        }
        #endregion

        #region RecuperarVagasPoucosCandidatos
        public static DataTable RecuperarVagasPoucosCandidatos()
        {
            DateTime dtInicio, dtFim;
            dtInicio = DateTime.Now.AddDays(-1);
            string dtUm = dtInicio.ToShortDateString() + " 00:00";
            string dtDois = dtInicio.ToShortDateString() + " 23:59";

            dtInicio = Convert.ToDateTime(dtUm);
            dtFim = Convert.ToDateTime(dtDois);

            var parms = new List<SqlParameter> 
                {
                    new SqlParameter{ ParameterName = "@Dta_Inicio", SqlDbType = SqlDbType.DateTime, Value = dtInicio},
                    new SqlParameter{ ParameterName = "@Dta_Fim", SqlDbType = SqlDbType.DateTime, Value = dtDois}
                };

            DataTable dtVagas;
            dtVagas = DataAccessLayer.ExecuteReaderDs(CommandType.Text, SpVagasPoucosCandidatos, parms).Tables[0];

            return dtVagas;
        }
        #endregion

        #region AtualizaVagaDW
        /// <summary>
        /// Altera a data de atualizacao no DW
        /// </summary>
        /// <returns></returns>
        public void AtualizaVagaDW()
        {
            try
            {
                var parms = new List<SqlParameter>
				{
					new SqlParameter { ParameterName = "@Idf_Vaga", SqlDbType = SqlDbType.Int, Size = 4, Value = IdVaga }
				};

                DataAccessLayer.ExecuteNonQuery(CommandType.StoredProcedure, "[BNE].[BUSCA_CURRICULO_VAGA_PERFIL_PROXIMO]", parms);
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
            }
        }
        #endregion

        #region SalarioEstaAcimaMedia
        public bool SalarioEstaAcimaMedia()
        {
            this.Cidade.CompleteObject();
            this.Cidade.Estado.CompleteObject();
            var parms = new List<SqlParameter> 
                {
                    new SqlParameter{ ParameterName = "@Idf_Funcao", SqlDbType = SqlDbType.Int, Value = this.Funcao.IdFuncao },
                    new SqlParameter{ ParameterName = "@Idf_Estado", SqlDbType = SqlDbType.Int, Value = this.Cidade.Estado.IdEstado },
                    new SqlParameter{ ParameterName = "@Vlr_Salario_De", SqlDbType = SqlDbType.Decimal, Value = this.ValorSalarioDe },
                    new SqlParameter{ ParameterName = "@Vlr_Salario_Para", SqlDbType = SqlDbType.Decimal, Value = this.ValorSalarioPara }
                };

            return Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.StoredProcedure, "VAGA_VERIFICA_SALARIO_ACIMA_MEDIA", parms)) > 0;
        }
        #endregion

        #region ListaUsuariosCadastraramVagas
        /// <summary>
        /// Metodo utilizado por retornar as colunas de um registro no banco de dados.
        /// </summary>
        /// <returns>DataTable</returns>
        /// <remarks>Valéria Neves</remarks>
        public static DataTable ListaUsuariosCadastraramVagas(DateTime dataInicial, DateTime dataFinal)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter {ParameterName = "@DataInicial", SqlDbType = SqlDbType.DateTime, Size = 8, Value = dataInicial},
                    new SqlParameter {ParameterName = "@DataFinal", SqlDbType = SqlDbType.DateTime, Size = 8, Value = dataFinal}
                };

            return DataAccessLayer.ExecuteReaderDs(CommandType.Text, SpListaUsuariosCadastraramVagas, parms).Tables[0];
        }
        #endregion

        #region ListaUltimasVagasCadastradasPorUsuario
        /// <summary>
        /// Retorna as 5 últimas vagas que possuem Cvs Não vistos, considerando o intervalo de datas passados por parâmetro
        /// </summary>
        /// <returns>DataTable</returns>
        /// <remarks>Valéria Neves</remarks>
        public static DataTable ListaUltimasVagasCadastradasPorUsuario(int idf_Usuario_Filial_Perfil, DateTime dataInicial, DateTime dataFinal)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter {ParameterName = "@DataInicial", SqlDbType = SqlDbType.DateTime, Size = 8, Value = dataInicial},
                    new SqlParameter {ParameterName = "@DataFinal", SqlDbType = SqlDbType.DateTime, Size = 8, Value = dataFinal},
                    new SqlParameter {ParameterName = "@Idf_Usuario_Filial_Perfil", SqlDbType = SqlDbType.Int, Size = 4, Value = idf_Usuario_Filial_Perfil}
                };

            return DataAccessLayer.ExecuteReaderDs(CommandType.Text, SpListaUltimasVagasCadastradasPorUsuario, parms).Tables[0];
        }
        #endregion

        #endregion

    }
}
