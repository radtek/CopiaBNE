//-- Data: 02/03/2010 09:17
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using BNE.BLL.Common;
using BNE.BLL.Custom;
using BNE.BLL.Custom.Email;
using BNE.BLL.Enumeradores;
using BNE.EL;
using BNE.Services.Base.ProcessosAssincronos;
using BNE.BLL.AsyncServices.Enumeradores;
using BNE.BLL.Mensagem.DTO;
using System.Threading;
using System.Net;
using BNE.Domain.Events.CrossDomainEvents;
using BNE.Domain.Events.Handler;

namespace BNE.BLL
{
    public partial class VagaCandidato // Tabela: BNE_Vaga_Candidato
    {
        protected VagaCandidato() { }

        public VagaCandidato(Curriculo curriculo, Vaga vaga)
        {
            Curriculo = curriculo;
            Vaga = vaga;
            FlagInativo = false;
            StatusCandidatura = new BLL.StatusCandidatura((int)Enumeradores.StatusCandidatura.Candidatado);
            FlagAutoCandidatura = false;
        }

        #region Consultas

        #region [spVagasCandidatosInfoPorCurriculo]
        private const string spVagasCandidatosInfoPorCurriculo = @"
 DECLARE @FirstRec INT
        DECLARE @LastRec INT
        DECLARE @iSelect VARCHAR(8000)
        DECLARE @iSelectCount VARCHAR(8000)
        DECLARE @iSelectPag VARCHAR(8000)
        SET @FirstRec = ( @CurrentPage - 1 ) * @PageSize
        SET @LastRec = ( @CurrentPage * @PageSize + 1 )
        SET @iSelect = '
        SELECT	ROW_NUMBER() OVER (ORDER BY VC.Dta_Cadastro DESC) AS RowID,
		        VC.Dta_Cadastro ,
				v.Idf_vaga,
		        V.cod_vaga,
		        FUN.Des_Funcao ,
                V.Flg_Inativo,
                V.Flg_Vaga_Arquivada,
                cid.nme_cidade,
                iif(v.flg_inativo = 1, ''Inativa'', iif(v.flg_vaga_arquivada = 1, ''Oportunidade'',''Ativa'')) as statusvaga,
                cid.sig_estado
          FROM    BNE.BNE_Vaga_Candidato VC WITH(NOLOCK)
		        JOIN BNE.BNE_Curriculo C WITH(NOLOCK) ON VC.Idf_Curriculo = C.Idf_Curriculo
		        JOIN BNE.BNE_Vaga V WITH(NOLOCK) ON VC.Idf_Vaga = V.Idf_Vaga
		        JOIN plataforma.TAB_Funcao FUN WITH(NOLOCK) ON V.Idf_Funcao = FUN.Idf_Funcao
                JOIN plataforma.tab_Cidade cid with(nolock) on v.idf_cidade = cid.idf_cidade
		     WHERE  vc.flg_inativo = 0  and  VC.Idf_Curriculo = ' + CONVERT(VARCHAR, @Idf_Curriculo) 

			 if(@Dta_Inicio is not null and @Dta_Fim is not null)
			 begin
			 SET @iSelect = @iSelect + ' AND  vc.dta_cadastro BETWEEN convert(date, ''' + @Dta_Inicio + ''')
                                   AND     '''+ @Dta_Fim +''''
			 end
          
        SET @iSelectCount = ' Select Count(*) From ( ' + @iSelect + ' ) As TblTempCount'
        SET @iSelectPag = ' Select * From ( ' + @iSelect	+ ' ) As TblTempPag	Where RowID > ' + CONVERT(VARCHAR, @FirstRec) + ' And RowID < ' + CONVERT(VARCHAR, @LastRec)


        EXECUTE (@iSelectCount)
        EXECUTE (@iSelectPag)
";
        #endregion

        private const string Spselectporvagacurriculo = @"
        SELECT * FROM BNE_Vaga_Candidato WITH(NOLOCK) WHERE Idf_Vaga = @Idf_Vaga AND Idf_Curriculo = @Idf_Curriculo";

        private const string Spselectvagacandidatoporfilial = @"
        SELECT  COUNT(C.Idf_Vaga_Candidato)
        FROM    BNE.BNE_Vaga V WITH ( NOLOCK )
                INNER JOIN BNE.BNE_Vaga_Candidato C WITH ( NOLOCK ) ON C.Idf_Vaga = V.Idf_Vaga
                LEFT JOIN BNE.BNE_Curriculo_Visualizacao cv WITH ( NOLOCK ) ON cv.Idf_Curriculo = C.Idf_Curriculo AND cv.Idf_Filial = @Idf_Filial
        WHERE   V.Idf_Filial = @Idf_Filial
                AND V.Flg_Vaga_Arquivada = 0
                AND V.Flg_Inativo = 0
                AND V.Flg_Vaga_Rapida = 0
                AND cv.Idf_Curriculo_Visualizacao IS NULL";

        private const string Spvagacandidatoquantidade = @"
        SELECT  COUNT(VC.Idf_Curriculo)
        FROM    BNE_Vaga_Candidato VC WITH(NOLOCK)
        WHERE   VC.Idf_Curriculo = @Idf_Curriculo 
                AND VC.Flg_Inativo = 0";

        private const string Spvagacandidatoporcurriculo = @"
        DECLARE @FirstRec INT
        DECLARE @LastRec INT
        DECLARE @iSelect VARCHAR(8000)
        DECLARE @iSelectCount VARCHAR(8000)
        DECLARE @iSelectPag VARCHAR(8000)
        SET @FirstRec = ( @CurrentPage - 1 ) * @PageSize
        SET @LastRec = ( @CurrentPage * @PageSize + 1 )
        SET @iSelect = '
        SELECT	ROW_NUMBER() OVER (ORDER BY VC.Dta_Cadastro DESC) AS RowID,
		        VC.Dta_Cadastro ,
		        F.Idf_Filial,
		        V.Idf_Vaga,
                FUN.Idf_Funcao,
		        FUN.Des_Funcao ,
                Des_Area_BNE,
                V.Flg_Deficiencia,
                V.Idf_Deficiencia,
		              CASE WHEN Vlr_Salario_De > 0 AND Vlr_Salario_Para > 0
						THEN ''R$ '' + REPLACE(Vlr_Salario_De, ''.'', '','') + '' a '' + ''R$ '' + REPLACE(Vlr_Salario_Para, ''.'', '','')
					WHEN  Vlr_Salario_De > 0
						THEN ''R$ '' + Replace(vlr_salario_De, ''.'','','')
					when Vlr_Salario_Para >0
						THEN ''R$ '' + Replace(vlr_salario_Para, ''.'','','')
			        ELSE ''A combinar''
		        END AS Vlr_Salario ,                                                                                   
		        ( CASE 
                   WHEN ( V.Flg_Confidencial = 1 )
				        THEN ''Empresa Confidencial''
		           WHEN ( O.Idf_Tipo_Origem = 3 )
                        THEN V.Nme_Empresa
                   ELSE F.Raz_Social
		        END ) AS Raz_Social ,
		        ( CASE WHEN ( V.Flg_Confidencial = 1 ) THEN 1 
			          ELSE 0 -- Visible = true
		        END ) AS Img_Empresa_Confidencial_Visible ,
                C.Flg_VIP,
                V.Flg_Inativo,
                V.Flg_Vaga_Arquivada,
                V.Dta_Prazo,
                cid.nme_cidade,
                cid.sig_estado,
                VC.Flg_Auto_Candidatura
          FROM    BNE.BNE_Vaga_Candidato VC WITH(NOLOCK)
		        JOIN BNE.BNE_Curriculo C WITH(NOLOCK) ON VC.Idf_Curriculo = C.Idf_Curriculo
		        JOIN BNE.BNE_Vaga V WITH(NOLOCK) ON VC.Idf_Vaga = V.Idf_Vaga
                JOIN BNE.TAB_Origem O WITH(NOLOCK) ON O.Idf_Origem = V.Idf_Origem
		        JOIN plataforma.TAB_Funcao FUN WITH(NOLOCK) ON V.Idf_Funcao = FUN.Idf_Funcao
                JOIN plataforma.tab_Cidade cid with(nolock) on v.idf_cidade = cid.idf_cidade
		        JOIN BNE.TAB_Filial F WITH(NOLOCK) ON V.Idf_Filial = F.Idf_Filial
				JOIN plataforma.TAB_CNAE_Sub_Classe csc WITH (NOLOCK) ON f.Idf_CNAE_Principal = csc.Idf_CNAE_Sub_Classe
                JOIN plataforma.TAB_CNAE_Classe cl WITH (NOLOCK) ON csc.Idf_CNAE_Classe = cl.Idf_CNAE_Classe
                JOIN plataforma.TAB_CNAE_Grupo gp WITH (NOLOCK) ON cl.Idf_CNAE_Grupo = gp.Idf_CNAE_Grupo
                JOIN plataforma.TAB_CNAE_Divisao d WITH (NOLOCK) ON gp.Idf_CNAE_Divisao = d.Idf_CNAE_Divisao
                JOIN plataforma.TAB_Area_BNE area WITH (NOLOCK) ON d.Idf_Area_BNE = area.Idf_Area_BNE
           LEFT JOIN BNE.BNE_Vaga_Candidato VC2 ON VC2.Idf_Curriculo = C.Idf_Curriculo AND C.Flg_VIP = 0
				 AND VC2.Flg_Auto_Candidatura = 1 AND VC2.Idf_Vaga = VC.Idf_Vaga
        WHERE    VC.Idf_Curriculo = ' + CONVERT(VARCHAR, @Idf_Curriculo) + '
          AND VC2.Idf_Vaga IS NULL  and v.Flg_Vaga_Arquivada =  ' + CONVERT(VARCHAR, @Status)
        SET @iSelectCount = 'Select Count(*) From ( ' + @iSelect + ' ) As TblTempCount'
        SET @iSelectPag = 'Select * From ( ' + @iSelect	+ ' ) As TblTempPag	Where RowID > ' + CONVERT(VARCHAR, @FirstRec) + ' And RowID < ' + CONVERT(VARCHAR, @LastRec)

        EXECUTE (@iSelectCount)
        EXECUTE (@iSelectPag)";

        private const string Spselectcurriculojacandidatou = @"
        SELECT count(*) FROM BNE_Vaga_Candidato WITH(NOLOCK) WHERE Idf_Vaga = @Idf_Vaga AND Idf_Curriculo = @Idf_Curriculo";

        private const string Spcurriculocandidatadoautomaticamente = @"
        SELECT count(*) FROM BNE_Vaga_Candidato WITH(NOLOCK) WHERE Idf_Vaga = @Idf_Vaga AND Idf_Curriculo = @Idf_Curriculo AND Flg_Auto_Candidatura = 1";


        private const string spSelectVagasCandidatadas = @"
            DECLARE @FirstRec INT
            DECLARE @LastRec INT
            DECLARE	@iSelect VARCHAR(8000)
            DECLARE	@iSelectCount VARCHAR(8000)
            DECLARE	@iSelectPag VARCHAR(8000)

            SET @FirstRec = ( @CurrentPage - 1 ) * @PageSize
            SET @LastRec = ( @CurrentPage ) * @PageSize 

            SET @iSelect = '
            SELECT
		            ROW_NUMBER() OVER (ORDER BY v.Dta_Abertura DESC) AS RowID,
		            v.* 
            FROM BNE.BNE_Vaga v WITH(NOLOCK)
            WHERE v.Idf_Vaga IN (SELECT vc.Idf_Vaga FROM BNE.BNE_Vaga_Candidato vc '
			
			IF(@tipoVinculo IS NOT NULL)
			BEGIN
				SET @iSelect = @iSelect + ' JOIN BNE.BNE_Vaga_Tipo_Vinculo vtv ON vc.Idf_Vaga = vtv.Idf_Vaga '
			END  

			SET @iSelect = @iSelect + ' WHERE v.Flg_Vaga_Arquivada = 0 AND
					            vc.Idf_Curriculo = ' + CONVERT(VARCHAR, @Idf_Curriculo)

			IF(@tipoVinculo IS NOT NULL)
			BEGIN
				SET @iSelect = @iSelect + ' AND Idf_Tipo_Vinculo = ' + CONVERT(VARCHAR, @tipoVinculo)
			END

			SET @iSelect = @iSelect + ')'

            SET @iSelectCount = 'Select Count(1) From ( ' + @iSelect
	            + ' ) As TblTempCount'
            SET @iSelectPag = 'Select * From ( ' + @iSelect
	            + ' ) As TblTempPag  Where RowID > ' + CONVERT(VARCHAR, @FirstRec)
	            + ' And RowID <= ' + CONVERT(VARCHAR, @LastRec)
	            + ' ORDER BY RowID ASC'
    
            --select @iSelectPag
            --select @iSelectCount
            EXECUTE (@iSelectCount)
            EXECUTE (@iSelectPag)";

        #region spCandidaturaOportunidade
        private const string spCandidaturaOportunidade = @"select top 20 pf.idf_pessoa_fisica, pf.nme_pessoa, pf.eml_pessoa, pf.dta_nascimento,
					cu.idf_curriculo,
					f.des_funcao,
					c.nme_cidade, c.Sig_Estado
			from bne_vaga_candidato vc with(nolock)
				join bne_curriculo cu with(nolock) on cu.idf_curriculo = vc.idf_curriculo
				join tab_pessoa_fisica pf with(nolock) on pf.idf_pessoa_fisica = cu.idf_pessoa_fisica
				cross apply(
				select top 1 idf_curriculo, idf_funcao, idf_cidade
					 from bne_funcao_pretendida with(nolock)
					 where idf_curriculo = cu.idf_curriculo
		             order by idf_funcao_pretendida asc
					 ) as fp 
				join plataforma.TAB_Funcao f with(nolock) on f.idf_funcao = fp.idf_funcao
				left join plataforma.TAB_Cidade c with(nolock) on c.idf_cidade = pf.idf_cidade
				join bne_vaga v with(nolock) on vc.idf_vaga = v.idf_vaga
            where vc.idf_vaga= @idf_vaga
                and vc.flg_inativo = 0 
				and vc.dta_cadastro > @dta_prazo
				and vc.dta_cadastro > (getdate() -15)
				and fp.idf_funcao = v.idf_funcao
				and fp.idf_cidade = v.idf_cidade
                and cu.idf_situacao_curriculo not in(6,7,12)
            order by vc.dta_cadastro desc

            ";
        #endregion

        #region Spselectvagasporfilial
        private const string Spselectvagasporfilial = @"
        DECLARE @FirstRec INT
        DECLARE @LastRec INT
        DECLARE @iSelect VARCHAR(8000)
        DECLARE @iSelectCount VARCHAR(8000)
        DECLARE @iSelectPag VARCHAR(8000)

        SET @FirstRec = @CurrentPage * @PageSize + 1
        SET @LastRec = @CurrentPage * @PageSize + @PageSize

        SET @iSelect = '
            SELECT 
			        ROW_NUMBER() OVER (ORDER BY Flg_Vaga_Arquivada, V.Dta_Cadastro DESC) AS RowID ,
                    V.Idf_Vaga ,
                    ( BNE.BNE_FUNCAO_VAGA_ORIGEM('+ ISNULL(Convert(VARCHAR,@Idf_Origem), '''''') + ',V.Idf_Vaga) ) AS Des_Funcao, 
                    V.Cod_Vaga ,
                    CONVERT(VARCHAR, CONVERT(DATETIME, V.Dta_Prazo, 103), 103) AS Dta_Prazo ,
                    (   
				        SELECT  COUNT (VCSub.Idf_Vaga_Candidato) 
                        FROM    bne.BNE_Vaga_Candidato VCSub WITH(NOLOCK)
                        WHERE   VCSub.Idf_Vaga = V.Idf_Vaga
                                AND VCSub.Flg_Inativo = 0) as Num_Cvs_Recebidos ,
                    V.Dta_Cadastro ,
                    C.Nme_Cidade ,
                    C.Sig_Estado ,
                    Flg_Vaga_Arquivada ,
                    ( 
				        SELECT  COUNT (VCSub.Idf_Vaga_Candidato) 
                        FROM    bne.BNE_Vaga_Candidato VCSub WITH(NOLOCK)
                        WHERE   VCSub.Idf_Vaga = V.Idf_Vaga 
						        AND VCSub.Idf_Curriculo = ' + CONVERT(VARCHAR, @Idf_Curriculo) + '
                                AND VCSub.Flg_Inativo = 0
                    ) Ja_Candidatou
            FROM    bne.BNE_Vaga V WITH(NOLOCK)
                    LEFT JOIN plataforma.TAB_Funcao F WITH(NOLOCK) ON F.Idf_Funcao = V.Idf_Funcao
                    INNER JOIN plataforma.TAB_Cidade C WITH(NOLOCK) ON C.Idf_Cidade = V.Idf_Cidade
            WHERE 
                    V.Idf_Filial = ' + CONVERT(VARCHAR, @Idf_Filial) + '
                    AND V.Flg_Inativo = 0
                    AND V.Flg_Vaga_Rapida = 0
                    AND Flg_Vaga_Arquivada = 0 '

        IF (@ids IS NOT NULL)
            SET @iSelect = @iSelect + ' AND V.Idf_Funcao IN ( '+ @ids +' ) '

        SET @iSelectCount = 'Select Count(1) From ( ' + @iSelect + ' ) As TblTempCount'
        SET @iSelectPag = 'Select * From ( ' + @iSelect  + ' ) As TblTempPag	Where RowID >= ' + CONVERT(VARCHAR, @FirstRec) + ' And RowID <= ' + CONVERT(VARCHAR, @LastRec)

        EXECUTE (@iSelectCount)
        EXECUTE (@iSelectPag)";
        #endregion

        #region Spquantidadecandidaturatotalporvaga
        private const string Spquantidadecandidaturatotalporvaga = @"
        SELECT  COUNT(VC.Idf_Curriculo)
        FROM    BNE_Vaga_Candidato VC WITH(NOLOCK)
        WHERE   VC.Idf_Vaga = @Idf_Vaga";
        #endregion

        #region Sptodoscurriculoscandidatados
        private const string Sptodoscurriculoscandidatados = @"
        SELECT  VC.Idf_Curriculo
        FROM    BNE_Vaga_Candidato VC WITH(NOLOCK)
        WHERE   VC.Idf_Vaga = @Idf_Vaga";
        #endregion

        #region SPCandidaturasDoDia
        private const string SPCandidaturasDoDia = @"
        SET NOCOUNT ON
        DECLARE @date DATETIME
        SET @date = CONVERT(VARCHAR(21), GETDATE(), 127)

        SELECT  cv.Idf_Curriculo ,
                Nme_Pessoa ,
                Eml_Pessoa ,
                v.Idf_Vaga ,
                f.Des_Funcao ,
                Nme_Cidade ,
                c.Sig_Estado,
		        v.Des_Atribuicoes,
		        --CONVERT(DECIMAL(10,2), v.Vlr_Salario_De) AS Vlr_Salario_De,
		        --CONVERT(DECIMAL(10,2),v.Vlr_Salario_Para) AS Vlr_Salario_Para,
                v.Vlr_Salario_De,
                v.Vlr_Salario_Para,
                cv.Idf_Pessoa_Fisica
        FROM    BNE.BNE_Curriculo cv WITH ( NOLOCK )
                JOIN BNE.TAB_Pessoa_Fisica pf WITH ( NOLOCK ) ON pf.Idf_Pessoa_Fisica = cv.Idf_Pessoa_Fisica
                JOIN BNE.BNE_Vaga_Candidato vc WITH ( NOLOCK ) ON vc.Idf_Curriculo = cv.Idf_Curriculo
                JOIN BNE.BNE_Vaga v WITH ( NOLOCK ) ON v.Idf_Vaga = vc.Idf_Vaga
                JOIN plataforma.TAB_Cidade c WITH ( NOLOCK ) ON c.Idf_Cidade = v.Idf_Cidade
                JOIN plataforma.TAB_Funcao f WITH ( NOLOCK ) ON f.Idf_Funcao = v.Idf_Funcao
        WHERE   vc.Dta_Cadastro BETWEEN CONVERT(VARCHAR(21), DATEADD(DAY,-1, @date),127)
                                AND @date
                                and vc.flg_inativo = 0 
                                and cv.idf_situacao_curriculo not in(6,7,12) ";//Bloqueado - Cancelado - Exclusão Lógica
        #endregion

        #region Spalterarstatus
        private const string Spalterarstatus = @"
        UPDATE BNE_Vaga_Candidato SET Idf_Status_Curriculo_Vaga = @Idf_Status_Curriculo_Vaga WHERE Idf_Vaga_Candidato = @Idf_Vaga_Candidato
        ";
        #endregion

        #region Spcandidaturasparanotificar
        private const string Spcandidaturasparanotificar = @"
 SELECT  V.Idf_Vaga ,
        F.Des_Funcao ,
        Cid.Nme_Cidade ,
        Cid.Sig_Estado ,
        Nme_Pessoa ,
        Dta_Nascimento ,
        VC.Idf_Curriculo ,
        Idf_Vaga_Candidato
FROM    BNE.BNE_Vaga V WITH ( NOLOCK )
        JOIN BNE.TAB_Filial fil ON V.Idf_Filial = fil.Idf_Filial
        JOIN BNE.BNE_Vaga_Candidato VC WITH ( NOLOCK ) ON VC.Idf_Vaga = V.Idf_Vaga
        JOIN BNE.BNE_Curriculo C WITH ( NOLOCK ) ON C.Idf_Curriculo = VC.Idf_Curriculo
		JOIN plataforma.TAB_Cidade Cid WITH ( NOLOCK ) ON c.Idf_Cidade_pretendida = Cid.Idf_Cidade
		cross apply (select top 1 idf_funcao from bne.bne_funcao_pretendida with(nolock)
						where idf_curriculo = c.Idf_Curriculo order by idf_funcao_pretendida asc) as funcao
        JOIN BNE.TAB_Pessoa_Fisica PF WITH ( NOLOCK ) ON PF.Idf_Pessoa_Fisica = C.Idf_Pessoa_Fisica
		JOIN plataforma.TAB_Funcao F WITH ( NOLOCK ) ON funcao.Idf_Funcao = F.Idf_Funcao
        LEFT JOIN BNE.BNE_Vaga_Integracao vi WITH(NOLOCK) ON vi.Idf_Vaga = V.Idf_Vaga	
WHERE   VC.Idf_Status_Curriculo_Vaga = 2
        AND VC.Flg_Inativo = 0
        AND C.Flg_Inativo = 0
        AND V.Flg_Inativo = 0
        AND fil.Idf_Situacao_Filial IN ( 1, 2, 3, 4, 7 )
        AND V.flg_vaga_arquivada = 0
        AND vi.Idf_Vaga_Integracao IS null
        AND V.Flg_Receber_Todos_CV = 1 
ORDER BY V.Idf_Vaga DESC
        ";
        #endregion Spcandidaturasparanotificar

        #region [spRecuperarFuncoesDosCandidatosVaga]
        private const string spRecuperarFuncoesDosCandidatosVaga = @"select f.idf_funcao, f.des_funcao 
                        from bne.bne_vaga_candidato vc with(nolock)
                        join bne.BNE_Vaga v with(nolock) on v.idf_vaga = vc.idf_vaga
                        join bne.bne_funcao_pretendida fp with(nolock) on fp.Idf_Curriculo = vc.idf_curriculo
                        join plataforma.tab_funcao f with(nolock) on f.idf_funcao = fp.idf_funcao
                        join plataforma.tab_funcao fvaga with(nolock) on fvaga.idf_funcao = v.Idf_Funcao
                        join bne.BNE_Curriculo cv with(nolock) on cv.Idf_Curriculo = vc.Idf_Curriculo
                        where vc.idf_vaga = @Idf_Vaga  
                        and vc.flg_inativo = 0
                        and cv.Flg_Inativo = 0
                        group by f.idf_funcao, f.des_funcao, f.idf_funcao_agrupadora,fvaga.idf_funcao_agrupadora
                        order by iif(f.idf_funcao_agrupadora = fvaga.idf_funcao_agrupadora, 1,0) desc";
        #endregion

        #region [spRecuperarCidadeDosCandidatosVaga]
        private const string spRecuperarCidadeDosCandidatosVaga = @"
                                select pf.Idf_Cidade, c.nme_cidade + '/'+c.Sig_estado as Cidade from bne.bne_vaga_candidato vc with(nolock)
                                join bne.bne_curriculo cv with(nolock) on vc.idf_curriculo = cv.idf_curriculo
                                join bne.tab_pessoa_fisica pf with(nolock) on pf.idf_pessoa_fisica = cv.idf_pessoa_fisica
                                join plataforma.tab_cidade c with(nolock) on c.idf_cidade = pf.idf_cidade
                                where  vc.idf_vaga = @Idf_Vaga  
                                and cv.flg_inativo = 0
                                group by pf.idf_cidade, c.nme_cidade, c.sig_estado
                                order by c.nme_cidade asc";
        #endregion

        #region [spCandidatosVisualizadosVaga]
        private const string spCandidatosVisualizadosVaga = @"select vc.Idf_Curriculo, vc.Dta_Visualizacao, pf.Nme_Pessoa from bne.bne_vaga_candidato vc with(nolock)
                    join bne.tab_usuario_filial_perfil ufp with(nolock) on ufp.Idf_Usuario_Filial_Perfil = vc.Idf_Usuario_Filial_Perfil_Visualizacao
                    join bne.TAB_Pessoa_Fisica pf with(nolock) on pf.Idf_Pessoa_Fisica = ufp.Idf_Pessoa_Fisica
                    join bne.BNE_Curriculo cv with(nolock) on cv.Idf_Curriculo = vc.Idf_Curriculo
                    where vc.Flg_Inativo = 0 and vc.idf_vaga = @Idf_Vaga
                    and vc.Dta_Visualizacao is not null
                    and cv.Flg_Inativo = 0
                    order by vc.Idf_Curriculo asc";
        #endregion

        #region [spAtualizarVisualiazacao]
        private const string spAtualizarVisualiazacao = @" update bne.bne_vaga_candidato set dta_visualizacao = getdate(), Idf_Usuario_Filial_Perfil_Visualizacao = @Idf_Usuario_filial_Perfil 
                                                            where Idf_vaga = @Idf_Vaga and Idf_Curriculo = @Idf_Curriculo ";
        #endregion

        #region [spRecuperarEscolaridadeDosCandidatosVaga]
        private const string spRecuperarEscolaridadeDosCandidatosVaga = @"";
        #endregion

        #region [spEmailEmpresaVagaPoucosCVsVaga]
        private const string spEmailEmpresaVagaPoucosCVsVaga = @"  
          select  Candidaturas.Total, v.eml_vaga, v.idf_vaga, v.dta_cadastro,v.idf_cidade,
	   v.idf_funcao,Vlr_salario_de, v.idf_Escolaridade, v.num_idade_minima, v.num_idade_maxima,
	   v.idf_sexo,v.idf_deficiencia,v.vlr_salario_para, f.idf_Filial, f.num_cnpj,
	   pf.num_cpf, pf.dta_nascimento, ufp.idf_usuario_Filial_perfil,
	    f.num_cnpj, f.raz_social, cid.nme_cidade, cid.sig_estado , func.des_funcao from bne.bne_vaga v with(nolock)
                                  outer apply(select count(idf_vaga) Total from bne.bne_vaga_candidato vc with(nolock)
                                  where vc.idf_Vaga = v.idf_vaga) Candidaturas
								  join bne.tab_filial f with(nolock) on f.idf_filial = v.idf_Filial
								  join bne.tab_usuario_filial_perfil ufp with(nolock) on ufp.idf_usuario_filial_perfil = v.idf_usuario_filial_perfil
								  join bne.tab_pessoa_fisica pf with(nolock) on pf.idf_pessoa_Fisica = ufp.idf_pessoa_Fisica
								  join plataforma.tab_cidade cid with(nolock) on cid.idf_cidade = v.idf_cidade
								  join plataforma.tab_funcao func with(nolock) on func.idf_funcao = v.idf_Funcao
                                  where v.flg_inativo = 0
		                                and v.idf_origem = 1 -- BNE
                                        and v.flg_vaga_arquivada = 0
                                        and Candidaturas.Total <= 10
                                        and v.Flg_Auditada = 1
										and v.Eml_Vaga is not null
										and ufp.flg_inativo = 0
										and f.idf_Filial <> 182089 -- empresa da vaga rapida
                                        and Convert(date,v.Dta_Cadastro) = Convert(date, @Dta_Cadastro)
                                  order by f.num_cnpj asc ";

        #endregion

        #endregion

        #region Métodos
        public static DataTable CandidaturasDoDia()
        {
            return DataAccessLayer.ExecuteReaderDs(CommandType.Text, SPCandidaturasDoDia, null).Tables[0];
        }

        #region CandidatosOportunidade
        /// <summary>
        ///     Retorna as  candidatuas dos ultimos 15 dias para as vagas de oportunidade(Arquivadas)
        /// </summary>
        /// <returns></returns>
        public static DataTable CandidatosOportunidade(int idfVaga, DateTime dtaPrazo)
        {
            var parms = new List<SqlParameter>
            {
                new SqlParameter("@idf_Vaga", SqlDbType.Int, 4),
                new SqlParameter("@dta_Prazo", SqlDbType.DateTime)
            };
            parms[0].Value = idfVaga;
            parms[1].Value = dtaPrazo;
            return DataAccessLayer.ExecuteReaderDs(CommandType.Text, spCandidaturaOportunidade, parms).Tables[0];
        }
        #endregion

        #region CarregarPorVagaCurriculo
        /// <summary>
        ///     Método responsável por carregar uma instancia de VagaCandidato através do
        ///     identificar de uma vaga e um curriculo
        /// </summary>
        /// <param name="idVaga">Identificador da Vaga</param>
        /// <param name="idCurriculo">Identificador de Curriculo</param>
        /// <param name="objVagaCandidato">Parametro out VagaCandidato </param>
        /// <returns>Boolean</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static bool CarregarPorVagaCurriculo(int idVaga, int idCurriculo, out VagaCandidato objVagaCandidato, SqlTransaction trans = null)
        {
            var parms = new List<SqlParameter>
            {
                new SqlParameter("@Idf_Vaga", SqlDbType.Int, 4), /* 0 */
                new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4) /* 1 */
            };
            parms[0].Value = idVaga;
            parms[1].Value = idCurriculo;

            using (IDataReader dr = DataAccessLayer.ExecuteReader(trans, CommandType.Text, Spselectporvagacurriculo, parms))
            {
                objVagaCandidato = new VagaCandidato();
                if (SetInstance(dr, objVagaCandidato))
                    return true;

                if (!dr.IsClosed)
                    dr.Close();
            }
            objVagaCandidato = null;
            return false;
        }
        #endregion

        #region Salvar
        /// <summary>
        /// </summary>
        /// <param name="objVaga"></param>
        /// <param name="objCurriculo"></param>
        /// <param name="objOrigem">
        ///     Como as vagas na pesquisa dentro do STC podem vir do BNE, a origem é passada como parametro,
        ///     para relacionar o CV a origem do STC
        /// </param>
        /// <param name="descricaoIP"></param>
        /// <param name="objIntencaoFilial"></param>
        /// <param name="listVagaPergunta"></param>
        /// <param name="origemCandidatura">Origem da candidatura (site, wsmobile, tanque) </param>
        public void Salvar(Vaga objVaga,
            Curriculo objCurriculo,
            Origem objOrigem,
            string descricaoIP,
            IntencaoFilial objIntencaoFilial,
            List<VagaCandidatoPergunta> listVagaPergunta,
            bool siteSTC,
            bool siteSTCCandidaturaLivre,
            bool avulsaMobile,
            Enumeradores.OrigemCandidatura origemCandidatura,
            out int? quantidadeCandidaturasGratis)
        {
            quantidadeCandidaturasGratis = null;
            using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();

                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        var podeCandidatar = false;
                        ParametroCurriculo objParametroCurriculo = null;

                        if ((siteSTC && siteSTCCandidaturaLivre) || avulsaMobile || ValidaCandidatura(objCurriculo, objVaga, trans))
                            podeCandidatar = true;
                        else
                        {
                            if (siteSTCCandidaturaLivre)
                            {
                                if (objOrigem != null && objVaga.Origem.IdOrigem.Equals(objOrigem.IdOrigem))
                                    podeCandidatar = true;
                                else
                                    quantidadeCandidaturasGratis = 0;
                            }
                            else
                            {
                                //Se tem o parametro, verifica se possui saldo
                                if (ParametroCurriculo.CarregarParametroPorCurriculo(Enumeradores.Parametro.QuantidadeCandidaturaDegustacao, objCurriculo, out objParametroCurriculo, trans))
                                {
                                    var quantidadeCandidaturas = Convert.ToInt32(objParametroCurriculo.ValorParametro);
                                    if (quantidadeCandidaturas > 0)
                                    {
                                        quantidadeCandidaturasGratis = quantidadeCandidaturas;
                                        var candidaturasRestantes = quantidadeCandidaturas - 1;
                                        podeCandidatar = true;
                                        objParametroCurriculo.ValorParametro = candidaturasRestantes.ToString(); //Desconta a candidatura atual
                                    }
                                    else
                                    {
                                        objParametroCurriculo = null;
                                        quantidadeCandidaturasGratis = 0;
                                    }
                                }
                                else
                                {
                                    quantidadeCandidaturasGratis = 0;
                                }
                            }
                        }

                        //Salvando o saldo para o currículo atual
                        if (objParametroCurriculo != null)
                            objParametroCurriculo.Save(trans);

                        if (podeCandidatar) //Se pode candidatar (é vip ou tem saldo)
                        {
                            if (objOrigem != null) //Utilizado quando a Origem é Minha Empresa Oferece Cursos, onde vagas do BNE aparecem na pesquisa de vaga
                            {
                                if (!CurriculoOrigem.ExisteCurriculoNaOrigem(objCurriculo, objOrigem, trans))
                                {
                                    var objCurriculoOrigem = new CurriculoOrigem
                                    {
                                        Curriculo = objCurriculo,
                                        Origem = objOrigem,
                                        DescricaoIP = descricaoIP
                                    };
                                    objCurriculoOrigem.Save(trans);

                                    PlanoAdquirido.ConcederPlanoPF(objCurriculo, new Plano(Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.PlanoVIPUniversitarioGratis))), trans);
                                }
                            }
                            else
                            {
                                if (!CurriculoOrigem.ExisteCurriculoNaOrigem(objCurriculo, objVaga.Origem, trans))
                                {
                                    var objCurriculoOrigem = new CurriculoOrigem
                                    {
                                        Curriculo = objCurriculo,
                                        Origem = objVaga.Origem,
                                        DescricaoIP = descricaoIP
                                    };
                                    objCurriculoOrigem.Save(trans);
                                }
                            }

                            objIntencaoFilial.Save(trans);

                            OrigemCandidatura = new OrigemCandidatura((int)origemCandidatura);
                            StatusCurriculoVaga = new StatusCurriculoVaga((int)Enumeradores.StatusCurriculoVaga.SemEnvio);
                            //Se a vaga exigir envio de todos os curriculos, um service irá enviar o email nestes casos
                            if (Convert.ToBoolean(objVaga.FlagReceberTodosCV) && objVaga.FlagReceberTodosCV.HasValue)
                            {
                                //Pode ser que uma vaga esteja ainda indexada no GOOGLE, não deve enviar e-mail para a empresa
                                if (objVaga.FlagVagaArquivada || objVaga.FlagInativo)
                                    StatusCurriculoVaga = new StatusCurriculoVaga((int)Enumeradores.StatusCurriculoVaga.SemEnvio);
                                else
                                    StatusCurriculoVaga = new StatusCurriculoVaga((int)Enumeradores.StatusCurriculoVaga.AguardoEnvio);
                            }

                            if (objVaga.CampanhaRecrutamentoNotificarCandidatura(objCurriculo, trans))
                            {
                                try
                                {
                                    HistoricoCandidatura.Logar(this.Curriculo, this.Vaga, string.Format("Campanha Recrutamento - SMS para retornar ligação - Entrou no fluxo."), trans);
                                    StatusCurriculoVaga = new StatusCurriculoVaga((int)Enumeradores.StatusCurriculoVaga.SemEnvio);

                                    UsuarioFilial objUsuarioFilial;
                                    if (UsuarioFilial.CarregarUsuarioFilialPorUsuarioFilialPerfil(objVaga.UsuarioFilialPerfil.IdUsuarioFilialPerfil, out objUsuarioFilial))
                                    {
                                        HistoricoCandidatura.Logar(this.Curriculo, this.Vaga, string.Format("Campanha Recrutamento - SMS para retornar ligação - Encontrou usuário filial."), trans);

                                        var parametros = new
                                        {
                                            NomeCandidato = objCurriculo.PessoaFisica.PrimeiroNome,
                                            Telefone = BNE.BLL.Custom.Helper.FormatarTelefone(objUsuarioFilial.NumeroDDDComercial, objUsuarioFilial.NumeroComercial),
                                            Funcao = objVaga.DescricaoFuncao
                                        };

                                        var sms = parametros.ToString(CartaSMS.RecuperaValorConteudo(Enumeradores.CartaSMS.CampanhaVagaRetornoParaLigacao));

                                        var idUsuario = Parametro.RecuperaValorParametro(Enumeradores.Parametro.IdfUsuarioEnvioCampanha);
                                        objCurriculo.PessoaFisica.CompleteObject();

                                        var objSMSTanque = new List<DestinatarioSMS>
                                        {
                                            new DestinatarioSMS
                                            {
                                                Mensagem = sms,
                                                DDDCelular = objCurriculo.PessoaFisica.NumeroDDDCelular,
                                                NumeroCelular = objCurriculo.PessoaFisica.NumeroCelular,
                                                NomePessoa = objCurriculo.PessoaFisica.PrimeiroNome
                                            }
                                        };

                                        HistoricoCandidatura.Logar(this.Curriculo, this.Vaga, string.Format("Campanha Recrutamento - SMS para retornar ligação - Montou a mensagem para enviar para o tanque"), trans);

                                        MensagemCS.EnvioSMSTanque(idUsuario, objSMSTanque, true);

                                        HistoricoCandidatura.Logar(this.Curriculo, this.Vaga, string.Format("Campanha Recrutamento - SMS para retornar ligação - Enviou a mensagem para o tanque"), trans);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    GerenciadorException.GravarExcecao(ex, "Falha ao enviar mensagem de retorno para o candidato");
                                }
                            }

                            Save(trans);

                            if (listVagaPergunta.Count > 0)
                            {
                                foreach (var objVagaCandidatoPergunta in listVagaPergunta)
                                    objVagaCandidatoPergunta.Save(trans);
                            }

                            #region FlagReceberCadaCV - Enviar Email
                            //Se a vaga estiver ativa
                            objVaga.Filial.CompleteObject();
                            if (!objVaga.FlagVagaArquivada && !objVaga.FlagInativo && !objVaga.Filial.EmpresaBloqueada())
                            {
                                //Se a vaga exigir envio de cada curriculo por email                        
                                if (objVaga.FlagReceberCadaCV.HasValue && objVaga.FlagReceberCadaCV.Value)
                                {
                                    string curriculoCandidato;
                                    if (objVaga.Origem.IdOrigem.Equals((int)Enumeradores.Origem.BNE))
                                    {
                                        objVaga.UsuarioFilialPerfil.CompleteObject();
                                        objVaga.UsuarioFilialPerfil.PessoaFisica.CompleteObject();
                                        curriculoCandidato = objCurriculo.RecuperarHTMLCurriculo(objIntencaoFilial.Filial.IdFilial, null, objVaga.IdVaga, false, true, false, true, FormatoVisualizacaoImpressao.Empresa, objVaga.Filial.PossuiPlanoAtivoIlimitado(), objVaga.UsuarioFilialPerfil.PessoaFisica.CPF, objVaga.UsuarioFilialPerfil.PessoaFisica.DataNascimento);
                                    }
                                    else
                                        curriculoCandidato = objCurriculo.RecuperarHTMLCurriculo(objIntencaoFilial.Filial.IdFilial, null, objVaga.IdVaga, true, true, false, false, FormatoVisualizacaoImpressao.Empresa, objVaga.Filial.PossuiPlanoAtivoIlimitado(), null, null);

                                    Notificar(curriculoCandidato, trans);
                                }
                            }
                            #endregion

                            #region Integração de Candidatura
                            //Verificando se a vaga tem integração
                            //Se tiver, efetua a requisição na url parametrizada
                            try
                            {
                                var parametrosAtividade = new ParametroExecucaoCollection
                                    {
                                        {"IdVaga","IdVaga", objVaga.IdVaga.ToString(), objVaga.IdVaga.ToString()},
                                        {"IdCurriculo","IdCurriculo", objCurriculo.IdCurriculo.ToString(), objCurriculo.IdCurriculo.ToString() }
                                    };
                                ProcessoAssincrono.IniciarAtividade(TipoAtividade.IntegrarCandidaturaSine, parametrosAtividade);
                            }
                            catch (Exception ex)
                            {
                                string vagas = listVagaPergunta.Aggregate("", (current, vaga) => current + vaga.IdVagaCandidatoPergunta.ToString() + "/");

                                quantidadeCandidaturasGratis = 0;
                                EL.GerenciadorException.GravarExcecao(ex, "Erro para integrar candidatura - Vaga:" + objVaga.IdVaga.ToString() + "/ Curriculo" + ":" +
                                    objCurriculo + "/ Origem:" + objOrigem
                                    + "/ descricaoIP:" + descricaoIP + "/ objIntencaoFilial:" + objIntencaoFilial +
                                    "/ listVagaPergunta:" + vagas + "/ siteSTC:" + siteSTC
                                    + "siteSTCCandidaturaLivre:" + siteSTCCandidaturaLivre + "/ avulsaMobile:" +
                                    avulsaMobile + "/ origemCandidatura:" + origemCandidatura
                                    + " / quantidadeCandidaturasGratis:" + quantidadeCandidaturasGratis);
                            }

                            #endregion


                        }
                        trans.Commit();

                        DomainEventsHandler.Handle(new OnNovaCandidatura(this.Curriculo.IdCurriculo, this.Vaga.IdVaga));
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

        #region ValidaCandidatura
        /// <summary>
        ///     Verificacão de candidatura
        /// </summary>
        /// <returns>Boolean</returns>
        private static bool ValidaCandidatura(Curriculo objCurriculo, Vaga objVaga, SqlTransaction trans)
        {
            if (objCurriculo.FlagVIP) //Se for VIP
                return true;

            if (objVaga.FlagBNERecomenda) //Se for VIP
                return true;

            if (objVaga.Filial.PossuiPlanoAtivo()) //Se empresa possui plano ativo
                return true;

            if (objVaga.PossuiPlanoPublicacaoImediata() != null) //Se empresa comprou plano de publicacao imediata 
                return true;

            if (PlanoAdquiridoDetalhesCurriculo.RecebeuCampanhaVagaPerfil(objCurriculo, objVaga, trans)) //Se o currículo recebeu campanha de recrutamento
                return true;

            if (objVaga.FlagInativo) //Se a vaga está inativa (regra que subiu no natal)
                return true;

            if (objVaga.FlagVagaArquivada) //Se a vaga está arquivada (regra que subiu no natal)
                return true;

            //RETIRADA VAGA PREMIUM TASK: 41857
            // if (ParametroVaga.Premium((int)Enumeradores.Parametro.VagaPremium, objVaga.IdVaga)) //candidatura comprada
            //  return true;

            return false;
        }
        #endregion

        #region CandidatoVagaPorFilial
        /// <summary>
        ///     Carrega a quantidade de curriculos candidatados para as vagas de uma determinada empresa.
        /// </summary>
        /// <param name="idFilial"></param>
        /// <returns></returns>
        public static int CandidatoVagaPorFilial(int idFilial)
        {
            var parms = new List<SqlParameter>
            {
                new SqlParameter("@Idf_Filial", SqlDbType.Int, 4) /* 0 */
            };
            parms[0].Value = idFilial;

            return Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, Spselectvagacandidatoporfilial, parms));
        }
        #endregion

        #region QuantidadeVagaCandidato
        public static int QuantidadeVagaCandidato(int idCurriculo)
        {
            var parms = new List<SqlParameter>
            {
                new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4)
            };
            parms[0].Value = idCurriculo;

            return Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, Spvagacandidatoquantidade, parms));
        }
        #endregion

        #region ListarVagaCandidatadaPorCurriculo
        public static List<Vaga> ListarVagaCandidatadaPorCurriculo(int idCurriculo,
            int? idTipoVinculo,
            int paginaCorrente,
            int tamanhoPagina,
            out int totalRegistros)
        {
            var parms = new List<SqlParameter>
            {
                new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4),
                new SqlParameter("@tipoVinculo", SqlDbType.Int, 4),
                new SqlParameter("@CurrentPage", SqlDbType.Int, 4),
                new SqlParameter("@PageSize", SqlDbType.Int, 4),
            };

            parms[0].Value = idCurriculo;
            if (idTipoVinculo.HasValue && idTipoVinculo.Value > 0)
                parms[1].Value = idTipoVinculo.Value;
            else
                parms[1].Value = DBNull.Value;
            parms[2].Value = paginaCorrente;
            parms[3].Value = tamanhoPagina;

            totalRegistros = 0;
            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, spSelectVagasCandidatadas, parms))
            {
                if (dr.Read())
                    totalRegistros = Convert.ToInt32(dr[0]);

                dr.NextResult();
                List<Vaga> retorno = new List<Vaga>();

                while (dr.Read())
                {
                    var objVaga = new Vaga();
                    Vaga.SetInstanceNonDispose(dr, objVaga);
                    retorno.Add(objVaga);
                }

                return retorno;
            }
        }
        #endregion

        #region CarregarVagaCandidatadaPorCurriculo
        public static DataTable CarregarVagaCandidatadaPorCurriculo(int idCurriculo, bool status, int paginaCorrente, int tamanhoPagina, out int totalRegistros)
        {
            var parms = new List<SqlParameter>
            {
                new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4),
                new SqlParameter("@CurrentPage", SqlDbType.Int, 4),
                new SqlParameter("@PageSize", SqlDbType.Int, 4),
                new SqlParameter("@Status", SqlDbType.Bit)
            };

            parms[0].Value = idCurriculo;
            parms[1].Value = paginaCorrente;
            parms[2].Value = tamanhoPagina;
            parms[3].Value = status;

            totalRegistros = 0;
            DataTable dt = null;
            try
            {
                using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spvagacandidatoporcurriculo, parms))
                {
                    if (dr.Read())
                        totalRegistros = Convert.ToInt32(dr[0]);

                    dr.NextResult();
                    dt = new DataTable();


                    dt.Load(dr);
                    dt.Columns.Add("MediaSalarial", typeof(string));
                    foreach (DataRow row in dt.Rows)
                    {
                        row["MediaSalarial"] = Vaga.MediaSalarialComRegra((int)row["idf_Funcao"]);
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

        #region Notificar
        /// <summary>
        /// Metodo responsável por notificar a candidatura de um currículo para uma vaga
        /// </summary>
        /// <param name="curriculoCandidato"> </param>
        /// <param name="trans">Transaction</param>
        public void Notificar(string curriculoCandidato, SqlTransaction trans)
        {
            #region MensagemCS
            string templateAssunto;
            string template = CartaEmail.RetornarConteudoBNE(Enumeradores.CartaEmail.ApresentacaoTopoEmailcomCVAnexoparaEmpresa, out templateAssunto);

            //TODO: Precisa tudo isso?
            Vaga.Filial.CompleteObject(trans);
            Vaga.Cidade.CompleteObject(trans);
            Vaga.Funcao.CompleteObject(trans);

            var linkCurriculo = string.IsNullOrEmpty(curriculoCandidato) ? Curriculo.RetornaLinkVisualizacaoCurriculo(new List<int> { this.Curriculo.IdCurriculo }) : curriculoCandidato;
            var parametros = new
            {
                Vaga.Filial.RazaoSocial,
                Vaga.Cidade,
                NomeFuncao = Vaga.Funcao.DescricaoFuncao,
                LinkCurriculo = linkCurriculo,
                utmcampaign = String.Format("cv_{0}", this.Curriculo.IdCurriculo)

            };
            var assunto = parametros.ToString(templateAssunto);
            var mensagem = parametros.ToString(template);
            var emailRemetente = Parametro.RecuperaValorParametro(Enumeradores.Parametro.EmailMensagens, trans);

            try
            {
                EmailSenderFactory.Create(TipoEnviadorEmail.Fila).Enviar(assunto, mensagem, Enumeradores.CartaEmail.ApresentacaoTopoEmailcomCVAnexoparaEmpresa, emailRemetente, Vaga.EmailVaga);
            }
            catch (Exception ex)
            {
                GerenciadorException.GravarExcecao(ex, "Falha ao enviar mensagem");
            }
            #endregion

            //So altera o status se a vaga não estiver com o flag receber todos os cvs
            if (!Vaga.FlagReceberTodosCV.HasValue || !Vaga.FlagReceberTodosCV.Value)
            {
                AlterarStatus(Enumeradores.StatusCurriculoVaga.Enviado, trans);
            }
        }

        /// <summary>
        /// Metodo responsável por enviar os links dos curriculos por email e atualizar a statusCurriculoVaga para Curriculo enviado
        /// </summary>
        /// <param name="objVaga">Vaga</param>
        /// <param name="candidaturas">Candidaturas</param>
        public static void Notificar(Vaga objVaga, List<DTO.Candidatura> candidaturas)
        {
            using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();

                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        var retornoCarta = CartaEmail.RecuperarCarta(Enumeradores.CartaEmail.CandidaturaEnvioTodosCurriculos);
                        var linkCandidato = CartaEmail.RecuperarConteudo(Enumeradores.CartaEmail.CandidaturaEnvioTodosCurriculosLinkCurriculo);
                        var template = retornoCarta.Conteudo;
                        var templateAssunto = retornoCarta.Assunto;

                        objVaga.CompleteObject(trans);
                        objVaga.Filial.CompleteObject(trans);
                        objVaga.Cidade.CompleteObject(trans);
                        objVaga.Funcao.CompleteObject(trans);
                        objVaga.UsuarioFilialPerfil.CompleteObject(trans);
                        objVaga.UsuarioFilialPerfil.PessoaFisica.CompleteObject(trans);
                        var sb = new StringBuilder();

                        foreach (var curriculo in candidaturas)
                        {
                            string urlVisualizacao;
                            if (objVaga.FlagVagaRapida)
                            {
                                urlVisualizacao = $"http://{Helper.RecuperarURLAmbiente()}/campanha/vagarapida?e={Helper.ToBase64(objVaga.EmailVaga)}";
                            }
                            else
                            {
                                urlVisualizacao = string.Format("{0}?idVaga={1}&", SitemapHelper.MontarCaminhoVisualizacaoCurriculo(curriculo.NomeFuncaoPretendida, curriculo.NomeCidade, curriculo.SiglaEstado, curriculo.IdCurriculo).NormalizarURL(), objVaga.IdVaga);
                            }
                            //var url = String.Format("http://{0}/logar/{1}", Parametro.RecuperaValorParametro(Enumeradores.Parametro.URLAmbiente),
                            //    LoginAutomatico.GerarHashAcessoLoginAutomatico(objVaga.UsuarioFilialPerfil.PessoaFisica.CPF,
                            //    objVaga.UsuarioFilialPerfil.PessoaFisica.DataNascimento, urlVisualizacao));

                            //Funcao estagio

                            var parametroCandidato = new
                            {
                                Candidato = curriculo.PrimeiroNome,
                                Funcao = curriculo.NomeFuncaoPretendida,
                                Idade = curriculo.Idade,
                                Cidade = BNE.BLL.Custom.Helper.FormatarCidade(curriculo.NomeCidade, curriculo.SiglaEstado),
                                VerCurriculo = string.Format("http://{0}{1}", Parametro.RecuperaValorParametro(Enumeradores.Parametro.URLAmbiente), urlVisualizacao),
                                utmcampaign = String.Format("cv_{0}", curriculo.IdCurriculo)
                            };
                            sb.Append(parametroCandidato.ToString(linkCandidato));
                        }

                        //Funcao estagio
                        var funcao = VagaCurso.ListaCursoParaCartaEmail(objVaga.Funcao.DescricaoFuncao, objVaga.IdVaga, trans);

                        var parametros = new
                        {
                            Empresa = objVaga.Filial.NomeFantasia.Equals("SINE") ? string.Empty : objVaga.Filial.RazaoSocial,
                            Cidade = objVaga.Cidade,
                            Funcao = funcao,
                            Plural = candidaturas.Count > 1 ? "s" : "",
                            PluralEnviar = candidaturas.Count > 1 ? "aram" : "ou",
                            Candidatos = sb.ToString()
                        };
                        var assunto = parametros.ToString(templateAssunto);
                        var mensagem = parametros.ToString(template);
                        var emailRemetente = Parametro.RecuperaValorParametro(Enumeradores.Parametro.EmailMensagens, trans);

                        try
                        {
                            EmailSenderFactory.Create(TipoEnviadorEmail.Fila).Enviar(assunto, mensagem, Enumeradores.CartaEmail.CandidaturaEnvioTodosCurriculos, emailRemetente, objVaga.EmailVaga);
                        }
                        catch (Exception ex)
                        {
                            GerenciadorException.GravarExcecao(ex, "Falha ao enviar mensagem");
                        }

                        candidaturas.ForEach(x => new VagaCandidato(x.IdVagaCandidato).AlterarStatus(Enumeradores.StatusCurriculoVaga.Enviado, trans));

                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        GerenciadorException.GravarExcecao(ex);
                        trans.Rollback();
                    }
                }
            }
        }
        #endregion

        #region AlterarStatus
        /// <summary>
        /// Altera o status do registro atual
        /// </summary>
        public void AlterarStatus(Enumeradores.StatusCurriculoVaga statusCurriculoVaga, SqlTransaction trans)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Vaga_Candidato", SqlDbType = SqlDbType.Int, Size = 4, Value = _idVagaCandidato },
                    new SqlParameter { ParameterName = "@Idf_Status_Curriculo_Vaga", SqlDbType = SqlDbType.Int, Size = 4, Value = (int)statusCurriculoVaga },
                };

            DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, Spalterarstatus, parms);
        }
        #endregion

        #region CurriculoJaCandidatouVaga
        /// <summary>
        ///     Método responsável por verificar se um curriculo já candidatou a uma vaga
        /// </summary>
        /// <param name="objVaga">Identificador da Vaga</param>
        /// <param name="objCurriculo">Identificador de Curriculo</param>
        /// <returns>Boolean</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static bool CurriculoJaCandidatouVaga(Curriculo objCurriculo, Vaga objVaga, SqlTransaction trans = null)
        {
            var parms = new List<SqlParameter>
            {
                new SqlParameter("@Idf_Vaga", SqlDbType.Int, 4), /* 0 */
                new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4) /* 1 */
            };
            parms[0].Value = objVaga.IdVaga;
            parms[1].Value = objCurriculo.IdCurriculo;

            if (trans != null)
                return Convert.ToInt32(DataAccessLayer.ExecuteScalar(trans, CommandType.Text, Spselectcurriculojacandidatou, parms)) > 0;

            return Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, Spselectcurriculojacandidatou, parms)) > 0;
        }
        #endregion

        #region CurriculoFoiCandidatoAutomaticamente
        /// <summary>
        ///     Método responsável por verificar se um curriculo já candidatou a uma vaga
        /// </summary>
        /// <param name="objVaga">Identificador da Vaga</param>
        /// <param name="objCurriculo">Identificador de Curriculo</param>
        /// <returns>Boolean</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static bool CurriculoFoiCandidatadoAutomaticamente(Curriculo objCurriculo, Vaga objVaga, SqlTransaction trans = null)
        {
            var parms = new List<SqlParameter>
            {
                new SqlParameter("@Idf_Vaga", SqlDbType.Int, 4), /* 0 */
                new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4) /* 1 */
            };
            parms[0].Value = objVaga.IdVaga;
            parms[1].Value = objCurriculo.IdCurriculo;

            if (trans != null)
                return Convert.ToInt32(DataAccessLayer.ExecuteScalar(trans, CommandType.Text, Spcurriculocandidatadoautomaticamente, parms)) > 0;

            return Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, Spcurriculocandidatadoautomaticamente, parms)) > 0;
        }
        #endregion

        #region AssociarCurriculoVaga
        /// <summary>
        ///     Metodo utilizado para associar um currículo a uma vaga.
        /// </summary>
        public static bool AssociarCurriculoVaga(Curriculo objCurriculo, Vaga objVaga)
        {
            using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();

                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        VagaCandidato objVagaCandidato;
                        if (CarregarPorVagaCurriculo(objVaga.IdVaga, objCurriculo.IdCurriculo, out objVagaCandidato))
                        {
                            objVagaCandidato.FlagInativo = false;
                        }
                        else
                        {
                            objVagaCandidato = new VagaCandidato
                            {
                                Curriculo = objCurriculo,
                                Vaga = objVaga,
                                StatusCurriculoVaga = new StatusCurriculoVaga((int)Enumeradores.StatusCurriculoVaga.SemEnvio)
                            };
                        }

                        objVagaCandidato.Save(trans);
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        return false;
                    }
                }
            }

            DomainEventsHandler.Handle(new OnNovaCandidatura(objCurriculo.IdCurriculo, objVaga.IdVaga));

            return true;
        }
        #endregion

        #region ListarVagasFilialDT
        /// <summary>
        ///     Método utilizado por retornar as colunas de um registro no banco de dados.
        /// </summary>
        /// <returns>DataTable</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static DataTable ListarVagasFilial(int idCurriculo, int idFilial, int? idOrigem, int paginaCorrente, int tamanhoPagina, List<int> listIdFuncoes, out int totalRegistros)
        {
            var parms = new List<SqlParameter>
            {
                new SqlParameter("@Idf_Filial", SqlDbType.Int, 4),
                new SqlParameter("@CurrentPage", SqlDbType.Int, 4),
                new SqlParameter("@PageSize", SqlDbType.Int, 4),
                new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4)
            };

            parms[0].Value = idFilial;
            parms[1].Value = paginaCorrente;
            parms[2].Value = tamanhoPagina;
            parms[3].Value = idCurriculo;

            var param = new SqlParameter("@ids", SqlDbType.VarChar, 1600);
            if (listIdFuncoes != null && listIdFuncoes.Count > 0)
                param.Value = string.Join(",", listIdFuncoes.Select(item => item.ToString(CultureInfo.CurrentCulture)).ToArray());
            else
                param.Value = DBNull.Value;

            parms.Add(param);

            var parm = new SqlParameter("@Idf_Origem", SqlDbType.Int, 4);

            if (idOrigem.HasValue)
                parm.Value = idOrigem.Value;
            else
                parm.Value = DBNull.Value;

            parms.Add(parm);

            totalRegistros = 0;
            DataTable dt = null;
            try
            {
                using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spselectvagasporfilial, parms))
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

        #region Inativar
        public void Inativar()
        {
            using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();

                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        FlagInativo = true;
                        this.StatusCandidatura = new BLL.StatusCandidatura((int)Enumeradores.StatusCandidatura.ExcluidoProcessoSelecao);
                        Save(trans);

                        var objPessoaFisica = new PessoaFisica(PessoaFisica.RecuperarIdPorCurriculo(Curriculo));
                        var emailDestinatario = objPessoaFisica.EmailPessoa;

                        if (!string.IsNullOrEmpty(emailDestinatario)) //Se o candidato tem email 
                        {
                            //TODO: Analisar performance: Método estatico apenas para verificar se a vaga é confidencial ou não
                            var dtoVaga = DTO.Vaga.CarregarVaga(Vaga.IdVaga, trans); //Recupera um objeto com todos os dados da vaga

                            if (!dtoVaga.FlagConfidencial) //Se a vaga não é confidencial
                            {
                                if (Convert.ToBoolean(Parametro.RecuperaValorParametro(Enumeradores.Parametro.EnviaCartaAgradecimentoCandidatura, trans))) //Se é para enviar a carta de agradecimento para todos os clientes
                                {
                                    string cartaFilial;
                                    ParametroFilial objParametroFilial;
                                    if (ParametroFilial.CarregarParametroPorFilial(Enumeradores.Parametro.EnviaCartaAgradecimentoCandidatura, new Filial(dtoVaga.IdEmpresa), out objParametroFilial, trans))
                                        cartaFilial = objParametroFilial.ValorParametro; //Recuperando o valor setado no banco
                                    else //Se não possuir parametro específico, recupera a carta padrão
                                        cartaFilial = CartaEmail.RecuperarConteudo(Enumeradores.CartaEmail.AgradecimentoCandidatura);

                                    if (!string.IsNullOrEmpty(cartaFilial)) //Se tiver valor manda a carta
                                    {
                                        string templateAssunto;
                                        var templateMensagem = CartaEmail.RetornarConteudoEmpresa(Enumeradores.CartaEmail.AgradecimentoCandidatura, cartaFilial, dtoVaga.CNPJEmpresa, dtoVaga.NomeEmpresa, out templateAssunto);

                                        //Criando os parametros da mensagem e montando a partir do template
                                        var parametros = new
                                        {
                                            NomeCandidato = objPessoaFisica.NomeCompleto,
                                            NomeFuncao = dtoVaga.DescricaoFuncao,
                                            dtoVaga.NomeSelecionadora,
                                            NomeEmpresaSelecionadora = dtoVaga.NomeEmpresa,
                                            DadosVaga = CartaEmail.MontarExtratoVaga(dtoVaga)
                                        };
                                        var descricaoMensagem = parametros.ToString(templateMensagem);
                                        var descricaoAssunto = parametros.ToString(templateAssunto);

                                        var emailRemetente = dtoVaga.EmailVaga;

                                        EmailSenderFactory
                                            .Create(TipoEnviadorEmail.Fila)
                                            .Enviar(descricaoAssunto, descricaoMensagem, Enumeradores.CartaEmail.AgradecimentoCandidatura, emailRemetente, emailDestinatario, trans);
                                    }
                                }
                            }
                        }

                        trans.Commit();
                        //Exluir a candidatura do solr
                        RemoverCandidaturaSolr(this.IdVagaCandidato);
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }

        public static void Inativar(Vaga objVaga, List<int> list)
        {
            foreach (var idCurriculo in list)
            {
                try
                {
                    VagaCandidato objVagaCandidato;
                    if (CarregarPorVagaCurriculo(objVaga.IdVaga, idCurriculo, out objVagaCandidato))
                        objVagaCandidato.Inativar();
                }
                catch (Exception ex)
                {
                    GerenciadorException.GravarExcecao(ex);
                }
            }
        }
        #endregion

        #region Candidatar
        public static bool Candidatar(Curriculo objCurriculo,
            Vaga objVaga,
            Origem objOrigem,
            List<VagaCandidatoPergunta> listPerguntas,
            string descricaoIP,
            bool siteSTC,
            bool siteSTCCandidaturaLivre,
            bool avulsaMobile,
            Enumeradores.OrigemCandidatura origemCandidatura,
            out int? quantidadeCandidaturasGratis)
        {
            quantidadeCandidaturasGratis = null;

            IntencaoFilial objIntencaoFilial;
            objVaga.CompleteObject();
            objCurriculo.CompleteObject();
            if (!IntencaoFilial.CarregarPorFilialCurriculo(objCurriculo.IdCurriculo, objVaga.Filial.IdFilial, out objIntencaoFilial))
            {
                objIntencaoFilial = new IntencaoFilial { Curriculo = objCurriculo, Filial = new Filial(objVaga.Filial.IdFilial) };
            }
            objIntencaoFilial.FlagInativo = false;

            VagaCandidato objVagaCandidato;
            if (!CarregarPorVagaCurriculo(objVaga.IdVaga, objCurriculo.IdCurriculo, out objVagaCandidato))
            {
                objVagaCandidato = new VagaCandidato(objCurriculo, objVaga);

                foreach (var objVagaCandidatoPergunta in listPerguntas)
                {
                    objVagaCandidatoPergunta.VagaCandidato = objVagaCandidato;
                }

                objVagaCandidato.Salvar(objVaga, objCurriculo, objOrigem, descricaoIP, objIntencaoFilial, listPerguntas, siteSTC, siteSTCCandidaturaLivre, avulsaMobile, origemCandidatura, out quantidadeCandidaturasGratis);
            }

            return true;
        }
        #endregion

        public static void CandidatarAutomaticamente(Curriculo objCurriculo, Vaga objVaga, SqlTransaction trans)
        {
            var objVagaCandidato = new VagaCandidato(objCurriculo, objVaga)
            {
                OrigemCandidatura = new OrigemCandidatura((int)Enumeradores.OrigemCandidatura.Site),
                StatusCurriculoVaga = new StatusCurriculoVaga((int)Enumeradores.StatusCurriculoVaga.SemEnvio),
                FlagAutoCandidatura = true
            };

            if (!CurriculoOrigem.ExisteCurriculoNaOrigem(objCurriculo, objVaga.Origem, trans))
            {
                var objCurriculoOrigem = new CurriculoOrigem
                {
                    Curriculo = objCurriculo,
                    Origem = objVaga.Origem
                };
                objCurriculoOrigem.Save(trans);
            }

            objVagaCandidato.Save(trans);
        }

        #region QuantidadeVagaCandidato
        public static int QuantidadeCandidaturas(Vaga objVaga)
        {
            var parms = new List<SqlParameter>
            {
                new SqlParameter {ParameterName = "@Idf_Vaga", SqlDbType = SqlDbType.Int, Size = 4, Value = objVaga.IdVaga}
            };

            return Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, Spquantidadecandidaturatotalporvaga, parms));
        }
        #endregion

        #region ListaIdsCurriculosCandidatados
        /// <summary>
        ///     Retorna uma lista com os ids dos currículos já candidatados
        /// </summary>
        /// <param name="objVaga"></param>
        /// <returns></returns>
        public static List<int> ListaIdsCurriculosCandidatados(Vaga objVaga)
        {
            var lista = new List<int>();

            var parms = new List<SqlParameter>
            {
                new SqlParameter {ParameterName = "@Idf_Vaga", SqlDbType = SqlDbType.Int, Size = 4, Value = objVaga.IdVaga}
            };

            using (var dr = DataAccessLayer.ExecuteReader(CommandType.Text, Sptodoscurriculoscandidatados, parms))
            {
                while (dr.Read())
                    lista.Add(Convert.ToInt32(dr["Idf_Curriculo"]));
            }

            return lista;
        }
        #endregion

        #region RecuperarVagasComCandidaturasAguardandoEnvio
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static IDictionary<int, List<DTO.Candidatura>> RecuperarVagasComCandidaturasAguardandoEnvio()
        {
            var parms = new List<SqlParameter>
                        {
                            new SqlParameter("@Idf_Status_Curriculo_Vaga", SqlDbType.Int, 4)
                        };
            parms[0].Value = (int)Enumeradores.StatusCurriculoVaga.AguardoEnvio;

            var dicionario = new Dictionary<int, List<DTO.Candidatura>>();

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spcandidaturasparanotificar, parms))
            {
                while (dr.Read())
                {
                    var vaga = Convert.ToInt32(dr["Idf_Vaga"]);
                    var funcao = dr["Des_Funcao"].ToString();
                    var cidade = dr["Nme_Cidade"].ToString();
                    var siglaEstado = dr["Sig_Estado"].ToString();
                    var nome = dr["Nme_Pessoa"].ToString();
                    var idade = Helper.CalcularIdade(Convert.ToDateTime(dr["Dta_Nascimento"]));
                    var idCurriculo = Convert.ToInt32(dr["Idf_Curriculo"]);
                    var idVagaCandidato = Convert.ToInt32(dr["Idf_Vaga_Candidato"]);

                    if (!dicionario.ContainsKey(vaga))
                    {
                        dicionario.Add(vaga, new List<DTO.Candidatura>());
                    }

                    dicionario[vaga].Add(new DTO.Candidatura(funcao, cidade, siglaEstado, nome, idade, idCurriculo, idVagaCandidato));
                }
            }

            return dicionario;
        }
        #endregion

        #region SalvarVisualizacaoCandidato
        public static void SalvarVisualizacaoCandidato(int idCurriculo, int idVaga, int idFilial, int idUsuarioFilialPefil)
        {
            var objVagaCandidato = new VagaCandidato();

            if (VagaCandidato.CarregarPorVagaCurriculo(idVaga, idCurriculo, out objVagaCandidato) && !objVagaCandidato.DataVisualizacao.HasValue)
            {
                VagaCandidato.AcaoAbrirCV objAcaoCV = new VagaCandidato.AcaoAbrirCV();
                objAcaoCV.IdCurriculo = idCurriculo;
                objAcaoCV.IdFilial = idFilial;
                objAcaoCV.IdUsuarioFilialLogado = idUsuarioFilialPefil;
                objAcaoCV.IdVaga = idVaga;
                objAcaoCV.Executar();
            }

        }
        #endregion

        #region [RecuperarFuncoesDosCandidatosVaga]
        public static IDataReader RecuperarFuncoesDosCandidatosVaga(int idVaga)
        {
            List<SqlParameter> parametros = new List<SqlParameter>()
            {
                new SqlParameter {ParameterName = "@Idf_Vaga", SqlDbType = SqlDbType.Int, Value = idVaga }
            };
            return DataAccessLayer.ExecuteReader(CommandType.Text, spRecuperarFuncoesDosCandidatosVaga, parametros);
        }
        #endregion

        #region [RecuperarCidadeDosCandidatosVaga]
        public static IDataReader RecuperarCidadeDosCandidatosVaga(int idVaga)
        {
            List<SqlParameter> parametros = new List<SqlParameter>()
            {
                new SqlParameter {ParameterName ="@Idf_Vaga", SqlDbType = SqlDbType.Int, Value = idVaga }
            };
            return DataAccessLayer.ExecuteReader(CommandType.Text, spRecuperarCidadeDosCandidatosVaga, parametros);
        }
        #endregion

        #region [RecuperarEscolaridadeDosCandidatosVaga]
        public static IDataReader RecuperarEscolaridadeDosCandidatosVaga(int idVaga)
        {
            List<SqlParameter> parametros = new List<SqlParameter>()
            {
                new SqlParameter {ParameterName ="@Idf_Vaga", SqlDbType = SqlDbType.Int, Value = idVaga }
            };
            return DataAccessLayer.ExecuteReader(CommandType.Text, spRecuperarEscolaridadeDosCandidatosVaga, parametros);
        }
        #endregion

        #endregion

        #region [Candidatos Visualizados da vaga]
        public static List<Tuple<int, string, DateTime>> CandidatosVisualizadosVaga(int Idf_vaga)
        {
            var listCurriculos = new List<Tuple<int, string, DateTime>>();
            List<SqlParameter> Parametros = new List<SqlParameter>(){
                new SqlParameter {ParameterName = "@idf_Vaga", SqlDbType = SqlDbType.Int, Value = Idf_vaga }
            };

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, spCandidatosVisualizadosVaga, Parametros))
            {
                while (dr.Read())
                {
                    listCurriculos.Add(new Tuple<int, string, DateTime>(Convert.ToInt32(dr["Idf_Curriculo"]),
                        dr["nme_pessoa"] != DBNull.Value ? dr["nme_pessoa"].ToString() : string.Empty,
                        Convert.ToDateTime(dr["Dta_Visualizacao"])));
                }
            }

            return listCurriculos;
        }
        #endregion

        #region [AtualizarVisualização]
        public static void AtualizarVisualiazacao(int idCurriculo, int idVaga, int IdfUsuarioFilialPerfil)
        {
            var listCurriculos = new List<Tuple<int, string, DateTime>>();
            List<SqlParameter> Parametros = new List<SqlParameter>(){
                new SqlParameter {ParameterName = "@Idf_Curriculo", SqlDbType = SqlDbType.Int, Value = idCurriculo },
                new SqlParameter {ParameterName = "@Idf_Vaga", SqlDbType = SqlDbType.Int, Value = idVaga },
                new SqlParameter {ParameterName = "@Idf_Usuario_filial_Perfil", SqlDbType = SqlDbType.Int, Value = IdfUsuarioFilialPerfil  }
            };

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, spAtualizarVisualiazacao, Parametros);


        }
        #endregion

        #region AcaoAbrirCurriculo
        private static void AcaoAbrirCurriculo(object obj)
        {
            try
            {
                var acao = (AcaoAbrirCV)obj;
                var objFilial = new Filial(acao.IdFilial);
                var objVaga = acao.IdVaga.HasValue ? new Vaga(acao.IdVaga.Value) : null;

                if (!objFilial.EmpresaBloqueada())
                    CurriculoQuemMeViu.SalvarQuemMeViuSite(objFilial, new Curriculo(acao.IdCurriculo), acao.IdUsuarioFilialLogado);

                CurriculoVisualizacaoHistorico.SalvarHistoricoVisualizacao(objFilial, new UsuarioFilialPerfil(acao.IdUsuarioFilialLogado),
                    new Curriculo(acao.IdCurriculo), false, acao.IP, acao.PesquisaCv, objVaga, acao.Rastreador);

                //Update no vaga candidato ultima visualização
                if (acao.IdVaga.HasValue)
                {
                    AtualizarVisualiazacao(acao.IdCurriculo, acao.IdVaga.Value, acao.IdUsuarioFilialLogado);
                    DomainEventsHandler.Handle(new OnNovaCandidatura(acao.IdCurriculo, acao.IdVaga.Value));
                }
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex, "AcaoAbrirCurriculo - quando abre a sanfona do grid de curriculos");
            }

        }

        public class AcaoAbrirCV
        {
            public int IdFilial { get; set; }
            public int IdCurriculo { get; set; }
            public int IdUsuarioFilialLogado { get; set; }
            public int? IdVaga { get; set; }
            public string IP { get; set; }
            public RastreadorCurriculo Rastreador { get; set; }
            public PesquisaCurriculo PesquisaCv { get; set; }

            public void Executar()
            {
                ThreadPool.QueueUserWorkItem(AcaoAbrirCurriculo, this);
            }
        }
        #endregion

        #region [IndexarVagaCandidato]
        /// <summary>
        /// Indexar a vaga Candidato 
        /// </summary>
        /// <param name="IdVaga"></param>
        /// <param name="IdCurriculo"></param>
        public static void IndexarVagaCandidato(int IdVaga, int IdCurriculo)
        {
            var urlSLOR = Parametro.RecuperaValorParametro(Enumeradores.Parametro.UrlIndexarCandidaturaUnitaria);

            if (!string.IsNullOrEmpty(urlSLOR))
            {
                urlSLOR = urlSLOR.Replace("{Idf_Vaga}", IdVaga.ToString()).Replace("{Idf_Curriculo}", IdCurriculo.ToString());
                var request = WebRequest.Create(urlSLOR);
                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    response.Close();
                }
            }
        }
        #endregion

        #region [RemoverCandidaturaSolr]
        private static void RemoverCandidaturaSolr(int IdfVagaCandidatura)
        {
            try
            {
                var urlSLOR = Parametro.RecuperaValorParametro(Enumeradores.Parametro.UrlSolrDeleteCandidatura);

                if (!string.IsNullOrEmpty(urlSLOR))
                {
                    urlSLOR = urlSLOR.Replace("{Idf_Vaga_Candidato}", IdfVagaCandidatura.ToString());
                    var request = WebRequest.Create(urlSLOR);
                    using (var response = (HttpWebResponse)request.GetResponse())
                    {
                        response.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex, "Erro ao excluir a candidatura do solr.");
            }

        }
        #endregion

        #region [EmailEmpresaVagaPoucosCVs]
        /// <summary>
        /// Recuperar as vagas que obtiveram poucos candidatos, selecionar os top 5 no perfil que não se candidataram as vagas para enviar.
        /// </summary>
        public static void EmailEmpresaVagaPoucosCVs()
        {
            var cartaEmail = CartaEmail.LoadObject((int)Enumeradores.CartaEmail.PoucosCvsParaEmpresa);

            string candidatoLayout = CartaEmail.RecuperarConteudo(Enumeradores.CartaEmail.InscritosSTC_Candidatos);
            string urlPadrao = string.Concat("http://", Helper.RecuperarURLAmbiente(), "/logar/");
            StringBuilder listVagaMenos10Candidatos = new StringBuilder();

            DataTable dt = null;


            List<SqlParameter> parametros = new List<SqlParameter>()
            {
                new SqlParameter{ParameterName = "@Dta_Cadastro", SqlDbType = SqlDbType.DateTime, Value = DateTime.Now.AddDays(-7)}
            };

            #region [Buscar vagas com poucos Candidatos]
            try
            {
                using (var dr = DataAccessLayer.ExecuteReader(CommandType.Text, spEmailEmpresaVagaPoucosCVsVaga, parametros))
                {

                    dt = new DataTable();
                    dt.Load(dr);
                }
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex, "Service - erro buscar vagas com poucos candidatos");
            }

            #endregion

            #region [Buscar Curriculos no Perfil]

            foreach (DataRow linha in dt.Rows)
            {
                DataTable dtCand;
                var layout = cartaEmail.ValorCartaEmail;
                List<SqlParameter> parametrosCandi = new List<SqlParameter>()
                {
                    new SqlParameter { ParameterName = "@Idf_Vaga", SqlDbType = SqlDbType.Int, Value = (int)linha["Idf_Vaga"] }
                };

                using (var dr = DataAccessLayer.ExecuteReader(CommandType.StoredProcedure, "[BNE].[BNE_Candidato_Perfil_Vaga]", parametrosCandi))
                {
                    dtCand = new DataTable();
                    dtCand.Load(dr);
                }
                if (dtCand.Rows.Count > 10)
                {
                    var candidatos = string.Empty;

                    for (int i = 0; i < (dtCand.Rows.Count > 5 ? 5 : dtCand.Rows.Count); i++)
                    {
                        var cand = candidatoLayout;
                        candidatos += cand.Replace("{Nome_Completo}", dtCand.Rows[i]["Nme_pessoa"].ToString()).Replace("{Idade}", dtCand.Rows[i]["Idade"].ToString())
                                    .Replace("{Bairro}", !string.IsNullOrEmpty(dtCand.Rows[i]["Des_Bairro"].ToString()) ? $" - {dtCand.Rows[i]["Des_Bairro"].ToString()}" : string.Empty)
                                    .Replace("{Cidade_Estado}", Helper.FormatarCidade(dtCand.Rows[i]["Nme_Cidade"].ToString(), dtCand.Rows[i]["Sig_Estado"].ToString()))
                                    .Replace("{Escolaridade}", dtCand.Rows[i]["Des_BNE"].ToString())

                                    .Replace("{Funcoes_Pretendidas}", dtCand.Rows[i]["Funcoes"].ToString());


                    }
                    layout = layout.Replace("{Candidatos}", candidatos);

                    //gerar pesquisa avançada para cair logado
                    #region [Montar pesquisa avançada]
                    PesquisaCurriculo objPesq = new PesquisaCurriculo();
                    objPesq.UsuarioFilialPerfil = new UsuarioFilialPerfil(Convert.ToInt32(linha["idf_usuario_Filial_perfil"]));
                    objPesq.FlagPesquisaAvancada = true;
                    if (linha["Idf_Cidade"] != DBNull.Value)
                        objPesq.Cidade = new Cidade(Convert.ToInt32(linha["Idf_Cidade"]));
                    if (linha["Idf_Sexo"] != DBNull.Value)
                        objPesq.Sexo = new Sexo(Convert.ToInt32(linha["Idf_Sexo"]));
                    if (linha["Vlr_salario_de"] != DBNull.Value && Convert.ToDecimal(linha["Vlr_salario_de"]) > 1)
                        objPesq.NumeroSalarioMin = Convert.ToDecimal(linha["Vlr_salario_de"]);
                    if (linha["Vlr_salario_para"] != DBNull.Value && Convert.ToDecimal(linha["Vlr_salario_de"]) > 2)
                        objPesq.NumeroSalarioMax = Convert.ToDecimal(linha["Vlr_salario_para"]);
                    if (linha["num_idade_minima"] != DBNull.Value)
                        objPesq.NumeroIdadeMin = Convert.ToInt16(linha["num_idade_minima"]);
                    if (linha["num_idade_maxima"] != DBNull.Value)
                        objPesq.NumeroIdadeMax = Convert.ToInt16(linha["num_idade_maxima"]);
                    if (linha["idf_Escolaridade"] != DBNull.Value)
                        objPesq.Escolaridade = new Escolaridade(Convert.ToInt32(linha["idf_Escolaridade"]));
                    if (linha["idf_Deficiencia"] != DBNull.Value)
                        objPesq.Deficiencia = new Deficiencia(Convert.ToInt32(linha["idf_Deficiencia"]));

                    var listaFuncaoPesq = new List<PesquisaCurriculoFuncao>();
                    var FuncPesq = new PesquisaCurriculoFuncao();
                    FuncPesq.Funcao = new Funcao(Convert.ToInt32(linha["Idf_Funcao"]));
                    listaFuncaoPesq.Add(FuncPesq);

                    objPesq.Salvar(listaFuncaoPesq);
                    #endregion


                    //link de logar automatico 
                    var linkLista = string.Format("/lista-de-curriculos/{0}", objPesq.IdPesquisaCurriculo);
                    var linkListaLogin = string.Concat(urlPadrao, BLL.Custom.LoginAutomatico.GerarHashAcessoLogin((decimal)linha["num_cpf"], (DateTime)linha["dta_nascimento"], linkLista));

                    layout = layout.Replace("{Link}", linkListaLogin)
                         .Replace("{Funcao}", linha["des_Funcao"].ToString())
                                    .Replace("{Cidade_Vaga}", Helper.FormatarCidade(linha["nme_cidade"].ToString(), linha["sig_Estado"].ToString()))
                                    .Replace("{Qtd_Candidatos}", linha["Total"].ToString());

                    var objFilial = new Filial(Convert.ToInt32(linha["Idf_Filial"]));
                    objFilial.NumeroCNPJ = (decimal)linha["Num_cnpj"];
                    string emlvendedor = objFilial.Vendedor().EmailVendedor;

                    if (Validacao.ValidarEmail(linha["eml_vaga"].ToString()) && Validacao.ValidarEmail(emlvendedor))
                    {
                        // Enviar E-mail para a empresa
                        EmailSenderFactory.Create(TipoEnviadorEmail.Fila)
                            .Enviar(cartaEmail.DescricaoAssunto, layout, BLL.Enumeradores.CartaEmail.PoucosCvsParaEmpresa, emlvendedor,
                                linha["eml_comercial"].ToString());
                    }

                }
                else
                {/// menos que 10 curriculos informar a Yanka
                    listVagaMenos10Candidatos.Append($" Vaga: {linha["Idf_Vaga"].ToString()} com apenas {dtCand.Rows.Count.ToString()} candidatos no perfil que ainda não se candidataram a vaga- Empresa: {linha["num_cnpj"].ToString()} - {linha["raz_social"].ToString()}<br>");
                }



            }

            if (dt != null)
                dt.Dispose();
            #endregion


            // Enviar E-mail informativo pra  para a empresa
            EmailSenderFactory.Create(TipoEnviadorEmail.Fila)
                .Enviar("Vagas de empresa com poucos candidatos", listVagaMenos10Candidatos.ToString(), BLL.Enumeradores.CartaEmail.PoucosCvsParaEmpresa, "yankalemos@bne-empregos.com.br",
                    "yankalemos@bne-empregos.com.br");
        }
        #endregion

        #region [VagasCandidatosInfoPorCurriculo]
        /// <summary>
        /// Retorna as vagas candidatadas pelo curriculo mais o link da vaga
        /// </summary>
        /// <param name="idCurriculo"></param>
        /// <param name="status"></param>
        /// <param name="paginaCorrente"></param>
        /// <param name="tamanhoPagina"></param>
        /// <param name="totalRegistros"></param>
        /// <returns></returns>
        public static DataTable VagasCandidatosInfoPorCurriculo(int idCurriculo, bool status, int paginaCorrente, int tamanhoPagina, out int totalRegistros, DateTime? dta_inicio = null, DateTime? dta_fim = null)
        {
            var parms = new List<SqlParameter>
            {
                new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4),
                new SqlParameter("@CurrentPage", SqlDbType.Int, 4),
                new SqlParameter("@PageSize", SqlDbType.Int, 4),
                new SqlParameter("@Dta_Inicio", SqlDbType.VarChar),
                new SqlParameter("@Dta_Fim", SqlDbType.VarChar)
            };

            parms[0].Value = idCurriculo;
            parms[1].Value = paginaCorrente;
            parms[2].Value = tamanhoPagina;
            if (dta_inicio.HasValue)
                parms[3].Value = dta_inicio.Value.ToString("MM/dd/yyyy");
            else
                parms[3].Value = DBNull.Value;
            if (dta_fim.HasValue)
                parms[4].Value = Convert.ToDateTime(dta_fim.Value).AddHours(23).AddMinutes(59).ToString("MM/dd/yyyy HH:mm");
            else
                parms[4].Value = DBNull.Value;


            totalRegistros = 0;
            DataTable dt = null;
            try
            {
                using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, spVagasCandidatosInfoPorCurriculo, parms))
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