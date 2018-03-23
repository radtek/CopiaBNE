using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using BNE.BLL.Custom;
using BNE.BLL.Custom.Email;
using BNE.Services.Base.ProcessosAssincronos;
using BNE.BLL.Common;
using BNE.BLL.Custom.Solr.Buffer;
using BNE.BLL.DTO;
using System.Runtime.Caching;

namespace BNE.BLL
{
    public partial class PlanoAdquirido : ICloneable
    {

        #region Clone
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        #endregion

        #region Consultas

        #region Spselectplanovigentepessoafisicaporsituacao
        private const string Spselectplanovigentepessoafisicaporsituacao = @"  
        SELECT  *
        FROM    BNE_Plano_Adquirido WITH(NOLOCK)
        WHERE   Idf_Plano_Situacao = @Idf_Plano_Situacao
                AND Idf_Filial IS NULL
                AND Idf_Usuario_Filial_Perfil IN (
		            SELECT Idf_Usuario_Filial_Perfil FROM TAB_Usuario_Filial_Perfil UFP WITH(NOLOCK) WHERE UFP.Idf_Pessoa_Fisica = @Idf_Pessoa_Fisica)";
        #endregion

        #region SpselectexisteplanoadquiridoRenovacao
        private const string SpselectexisteplanoadquiridoRenovacao = @"
        SELECT  PA.Dta_Fim_Plano
        FROM    BNE_Plano_Adquirido PA WITH(NOLOCK)
                join bne.bne_plano pl with(nolock) on pa.Idf_Plano = pl.Idf_Plano
                INNER JOIN TAB_Usuario_Filial_Perfil UFP WITH(NOLOCK) ON PA.Idf_Usuario_Filial_Perfil = UFP.Idf_Usuario_Filial_Perfil
                INNER JOIN BNE_Plano_Parcela PP WITH(NOLOCK) ON PP.Idf_Plano_Adquirido = PA.Idf_Plano_Adquirido
                LEFT JOIN BNE_Pagamento PG WITH(NOLOCK) ON PG.idf_Plano_Parcela = PP.Idf_Plano_Parcela AND PG.Idf_Pagamento_Situacao IN (2, 1)
        WHERE   UFP.Idf_Usuario_Filial_Perfil = @Idf_Usuario_Filial_Perfil
                AND PA.Idf_Plano_Situacao IN (0, 1)
                AND PA.Idf_Plano_Adquirido <> @Idf_Plano_Adquirido";
        #endregion

        #region Spselectretornaplanosfilialporsituacao
        private const string Spselectretornaplanosfilialporsituacao = @"
        SELECT  PA.*
        FROM    BNE_Plano_Adquirido PA WITH(NOLOCK)
        WHERE   PA.Idf_Plano_Situacao = @Idf_Plano_Situacao
                AND PA.Idf_Filial = @Idf_Filial
        ";
        #endregion

        #region SpselectretornaplanosfilialporliberacaoFuturaOuAutomatica
        private const string SpselectretornaplanosfilialporliberacaoFuturaOuAutomatica = @"
        SELECT TOP 1 PA.*
        FROM    BNE_Plano_Adquirido PA WITH(NOLOCK)
        WHERE   PA.Idf_Plano_Situacao IN(5,6)
                AND PA.Idf_Filial = @Idf_Filial
		ORDER BY
			PA.Dta_Cadastro DESC
        ";
        #endregion

        #region SpRecuperaPlanoAdquiridoPelaTransacao
        private const string SpRecuperaPlanoAdquiridoPelaTransacao = @"
            SELECT PA.* 
            FROM BNE.BNE_Plano_Adquirido PA WITH(NOLOCK) 
                JOIN BNE.BNE_Transacao T WITH(NOLOCK) ON T.Idf_Plano_Adquirido = PA.Idf_Plano_Adquirido
            WHERE Idf_Transacao= @IdfTransacao";
        #endregion

        #region Spselectretornaplanoscandidatoporsituacao
        private const string Spselectretornaplanoscandidatoporsituacao = @"
        SELECT  PA.*
        FROM    BNE_Plano_Adquirido PA WITH(NOLOCK)
                INNER JOIN TAB_Usuario_Filial_Perfil UFP WITH(NOLOCK) ON PA.Idf_Usuario_Filial_Perfil = UFP.Idf_Usuario_Filial_Perfil
                INNER JOIN BNE_Curriculo C WITH(NOLOCK) ON UFP.Idf_Pessoa_Fisica = C.Idf_Pessoa_Fisica
        WHERE   PA.Idf_Plano_Situacao = @Idf_Plano_Situacao
                AND C.Idf_Curriculo = @Idf_Curriculo
                AND UFP.Idf_Filial IS NULL
        ";
        #endregion

        #region Spselectretornaplanoadquiridoliberadoemaberto
        private const string Spselectretornaplanoadquiridoliberadoemaberto = @"
        DECLARE @QUERY VARCHAR(500)
            
        SET @QUERY = '  SELECT  PA.*
                        FROM    BNE_Plano_Adquirido PA WITH(NOLOCK)
                        WHERE   1=1 AND PA.Idf_Plano_Situacao IN (0,1) AND idf_Plano !=  ' + Convert(Varchar,@Idf_Plano) +''

        IF(@Idf_Usuario_Filial_Perfil IS NOT NULL)
                SET  @QUERY = @QUERY + ' AND PA.Idf_Usuario_Filial_Perfil=' + CONVERT(VARCHAR,@Idf_Usuario_Filial_Perfil)
            
        IF(@Idf_Filial IS NOT NULL)
                SET  @QUERY = @QUERY + ' AND PA.Idf_Filial=' + CONVERT(VARCHAR,@Idf_Filial)
        
        EXEC(@QUERY) ";
        #endregion

        #region Sprecuperarplanoadquiridoaguardandoliberacaoporfilialplano
        private const string Sprecuperarplanoadquiridoaguardandoliberacaoporfilialplano = @"
        SELECT  TOP 1   PA.*
        FROM    BNE_Plano_Adquirido PA WITH(NOLOCK)
        WHERE   PA.Idf_Filial = @Idf_Filial
                AND PA.Idf_plano_situacao = 0
                AND PA.Idf_Plano = @Idf_Plano
        ORDER BY PA.Idf_Plano_Adquirido DESC";
        #endregion

        #region Sprecuperarplanoadquiridoaguardandoliberacaoporcandidatoplano
        private const string Sprecuperarplanoadquiridoaguardandoliberacaoporcandidatoplano = @"
        SELECT  TOP 1   PA.*
        FROM    BNE_Plano_Adquirido PA WITH(NOLOCK)
                INNER JOIN TAB_Usuario_Filial_Perfil UFP WITH (NOLOCK) ON PA.Idf_Usuario_Filial_Perfil = UFP.Idf_Usuario_Filial_Perfil
                INNER JOIN BNE_Curriculo C WITH(NOLOCK) ON UFP.Idf_Pessoa_Fisica = C.Idf_Pessoa_Fisica
        WHERE   C.Idf_Curriculo = @Idf_Curriculo
                AND UFP.Idf_Filial IS NULL
                AND PA.Idf_Plano = @Idf_Plano
                AND PA.Idf_Plano_situacao = 0
        ORDER BY PA.Idf_Plano_Adquirido DESC";
        #endregion

        #region Spselectplanovigentepessoajuridicaporsituacao
        private const string Spselectplanovigentepessoajuridicaporsituacao = @"
        SELECT  PA.*
        FROM    BNE_Plano_Adquirido PA WITH(NOLOCK)
        WHERE   PA.Idf_Filial = @Idf_Filial
                AND PA.Idf_Plano_Situacao = @Idf_Plano_Situacao
        ";
        #endregion

        #region Splistartodosplanospessoajuridica
        private const string Splistartodosplanospessoajuridica = @"
        DECLARE @FirstRec INT
        DECLARE @LastRec INT
                                                                    
        DECLARE @iSelect VARCHAR(8000)
        DECLARE @iSelectCount VARCHAR(8000)
        DECLARE @iSelectPag VARCHAR(8000)
        SET @FirstRec = ( @CurrentPage - 1 ) * @PageSize
        SET @LastRec = ( @CurrentPage * @PageSize + 1 )
 
        SET @iSelect = '
            SELECT  
                    ROW_NUMBER() OVER (ORDER BY PA.Dta_Cadastro DESC) AS RowID,
                    PA.Idf_Plano_Adquirido ,
                    P.Des_Plano ,
                    P.Idf_Plano ,
                    PA.Dta_Inicio_Plano ,
                    PA.Dta_Fim_Plano ,
                    PS.Des_Plano_Situacao ,
                    PS.Idf_Plano_Situacao ,
                    (   SELECT  SUM(Qtd_Visualizacao_Utilizado)
                      FROM    BNE_Plano_Quantidade PQ WITH(NOLOCK)
                      WHERE   PQ.Idf_Plano_Adquirido = PA.Idf_Plano_Adquirido ) AS Qtd_Visualizacao,
                    (   SELECT  SUM(Qtd_SMS_Utilizado)
                      FROM    BNE_Plano_Quantidade PQ WITH(NOLOCK)
                      WHERE   PQ.Idf_Plano_Adquirido = PA.Idf_Plano_Adquirido ) AS Qtd_SMS

                FROM    BNE_Plano_Adquirido PA WITH(NOLOCK)
                    INNER JOIN BNE_Plano_Situacao PS WITH(NOLOCK) ON PA.Idf_Plano_Situacao = PS.Idf_Plano_Situacao
                    INNER JOIN BNE_Plano P WITH(NOLOCK) ON PA.Idf_Plano = P.Idf_Plano
                    OUTER APPLY ( SELECT    IIF(
                                            COUNT(PP.Idf_Plano_Parcela) > 0,1,0 ) AS is_parcela_paga
                                  FROM      BNE.BNE_Plano_Parcela PP WITH ( NOLOCK )
                                            JOIN BNE.BNE_Pagamento PG ON PG.Idf_Plano_Parcela = PP.Idf_Plano_Parcela
                                  WHERE     PP.Idf_Plano_Adquirido = PA.Idf_Plano_Adquirido
                                            AND Idf_Pagamento_Situacao = 2
                                ) AS PP_PAGA
            WHERE   
                    ( 
                        ( 
                        is_parcela_paga = 1
                        AND PA.Idf_Plano_Situacao = 3
                      )
                      OR 
                        ( PA.Idf_Plano_Situacao <> 3 )
                    )
                    AND PA.Idf_Filial = ' + CONVERT(VARCHAR, @Idf_Filial)
                    

        SET @iSelectCount = 'Select Count(*) From ( ' + @iSelect + ' ) As TblTempCount'
        SET @iSelectPag = 'Select * From ( ' + @iSelect + ' ) As TblTempPag	Where RowID > ' + CONVERT(VARCHAR, @FirstRec) + ' And RowID < ' + CONVERT(VARCHAR, @LastRec)
        EXECUTE (@iSelectCount)
        EXECUTE (@iSelectPag) ";
        #endregion

        #region Splistartodosplanospessoafisica
        private const string Splistartodosplanospessoafisica = @"
        DECLARE @FirstRec INT
        DECLARE @LastRec INT
                                                                    
        DECLARE @iSelect VARCHAR(8000)
        DECLARE @iSelectCount VARCHAR(8000)
        DECLARE @iSelectPag VARCHAR(8000)
        SET @FirstRec = ( @CurrentPage - 1 ) * @PageSize
        SET @LastRec = ( @CurrentPage * @PageSize + 1 )
 
        SET @iSelect = '
            SELECT    
                    ROW_NUMBER() OVER (ORDER BY PA.Dta_Cadastro DESC) AS RowID,
                    PA.Idf_Plano_Adquirido , 
		            P.Des_Plano ,
                    P.Idf_Plano ,
		            PA.Dta_Inicio_Plano , 
		            PA.Dta_Fim_Plano ,
		            PS.Des_Plano_Situacao ,
                    PS.Idf_Plano_Situacao ,
                    '''' AS Qtd_Visualizacao,
					'''' AS Qtd_SMS
            FROM    BNE_Plano_Adquirido PA WITH(NOLOCK)
	            INNER JOIN BNE_Plano_Situacao PS WITH(NOLOCK) ON PA.Idf_Plano_Situacao = PS.Idf_Plano_Situacao
	            INNER JOIN BNE_Plano P WITH(NOLOCK) ON PA.Idf_Plano = P.Idf_Plano
				 OUTER APPLY ( SELECT    IIF(
                                COUNT(PP.Idf_Plano_Parcela) > 0,1,0 ) AS is_parcela_paga
                      FROM      BNE.BNE_Plano_Parcela PP WITH ( NOLOCK )
                      JOIN      BNE.BNE_Pagamento PG ON PG.Idf_Plano_Parcela = PP.Idf_Plano_Parcela
                                 WHERE     PP.Idf_Plano_Adquirido = PA.Idf_Plano_Adquirido
                                            AND Idf_Pagamento_Situacao = 2
                    ) AS PP_PAGA
            WHERE   PA.Idf_Filial IS NULL
					AND ( ( is_parcela_paga = 1
							AND PA.Idf_Plano_Situacao = 3
						  )
						  OR ( PA.Idf_Plano_Situacao <> 3 )
						)
                    AND Idf_Usuario_Filial_Perfil IN (
		                SELECT Idf_Usuario_Filial_Perfil FROM TAB_Usuario_Filial_Perfil UFP WITH(NOLOCK) WHERE UFP.Idf_Pessoa_Fisica = ' + CONVERT(VARCHAR, @Idf_Pessoa_Fisica) + ') '

        SET @iSelectCount = 'Select Count(*) From ( ' + @iSelect + ' ) As TblTempCount'
        SET @iSelectPag = 'Select * From ( ' + @iSelect + ' ) As TblTempPag	Where RowID > ' + CONVERT(VARCHAR, @FirstRec) + ' And RowID < ' + CONVERT(VARCHAR, @LastRec)
        EXECUTE (@iSelectCount)
        EXECUTE (@iSelectPag) ";
        #endregion

        #region Spcancelarplanoadquirido
        private const string Spcancelarplanoadquirido = @"
            UPDATE BNE_Plano_Adquirido
            SET Idf_Plano_Situacao = 3
            WHERE Idf_Plano_Adquirido= @Idf_Plano_Adquirido

			--UPDATE BNE_Plano_Parcela
            --SET Idf_Plano_Parcela_Situacao = 3
            --WHERE Idf_Plano_Adquirido= @Idf_Plano_Adquirido
			--AND Idf_Plano_Parcela_Situacao = 1

			UPDATE PP 
            SET PP.Idf_Plano_Parcela_Situacao = 3
            FROM BNE_Plano_Parcela PP
	        INNER JOIN BNE_Pagamento PG ON PG.Idf_Plano_Parcela = PP.Idf_Plano_Parcela
            WHERE PP.Idf_Plano_Adquirido= @Idf_Plano_Adquirido
			AND ((PG.Des_Identificador IS NULL AND PG.Idf_Pagamento_Situacao != 2)
				OR (PG.Des_Identificador IS NOT NULL AND PG.Idf_Pagamento_Situacao NOT IN(1,2)))

            UPDATE PG 
            SET PG.Flg_Inativo = 1, PG.Idf_Pagamento_Situacao = 3
            FROM BNE_Pagamento PG
	            INNER JOIN BNE_Plano_Parcela PP ON PG.Idf_Plano_Parcela = PP.Idf_Plano_Parcela
            WHERE PP.Idf_Plano_Adquirido= @Idf_Plano_Adquirido
			AND ((PG.Des_Identificador IS NULL AND PG.Idf_Pagamento_Situacao != 2)
				OR (PG.Des_Identificador IS NOT NULL AND PG.Idf_Pagamento_Situacao NOT IN(1,2))) 			

            /* Adicional */
             UPDATE BNE_Adicional_Plano
            SET Idf_Adicional_Plano_Situacao = 4
            WHERE Idf_Plano_Adquirido= @Idf_Plano_Adquirido

            UPDATE PG 
            SET PG.Flg_Inativo = 1,
            PG.Idf_Pagamento_Situacao = 3
            FROM BNE_Pagamento PG
	            INNER JOIN BNE_Adicional_Plano AP ON PG.Idf_Adicional_Plano = AP.Idf_Adicional_Plano
            WHERE AP.Idf_Plano_Adquirido= @Idf_Plano_Adquirido
            /* FIM: Adicional */

            UPDATE BNE_Plano_Quantidade 
            SET Flg_Inativo = 1            
            WHERE Idf_Plano_Adquirido= @Idf_Plano_Adquirido 

            If(@Idf_Curriculo IS NOT NULL)
                Begin
                    UPDATE BNE.BNE_CURRICULO
                    SET Flg_Vip = 0
                    WHERE Idf_Curriculo =  @Idf_Curriculo
                End
            ";
        #endregion

        #region Spvarificaplanoadquiridofilialporsituacao
        private const string Spvarificaplanoadquiridofilialporsituacao = @"
        	 SELECT  COUNT(*)
        FROM    BNE.BNE_Plano_Adquirido WITH(NOLOCK)
        WHERE   Idf_Plano_Situacao = @Idf_Plano_Situacao
                AND Idf_Filial = @Idf_Filial
				";
        #endregion

        #region Spvarificaplanoadquiridocandidatoporsituacao
        private const string Spvarificaplanoadquiridocandidatoporsituacao = @"
        SELECT  COUNT(*)
        FROM    BNE.BNE_Plano_Adquirido PA WITH(NOLOCK)
                INNER JOIN TAB_Usuario_Filial_Perfil UFP WITH(NOLOCK) ON PA.Idf_Usuario_Filial_Perfil = UFP.Idf_Usuario_Filial_Perfil
                --INNER JOIN TAB_Pessoa_Fisica PF WITH(NOLOCK) ON UFP.Idf_Pessoa_Fisica = PF.Idf_Pessoa_Fisica
                INNER JOIN BNE_Curriculo C WITH(NOLOCK) ON UFP.Idf_Pessoa_Fisica = C.Idf_Pessoa_Fisica
        WHERE   Idf_Plano_Situacao = @Idf_Plano_Situacao
                AND PA.Idf_Filial IS NULL
                AND C.Idf_Curriculo = @Idf_Curriculo";
        #endregion

        #region Sprecuperardatafimultimoplanoadquiridoencerrado
        private const string Sprecuperardatafimultimoplanoadquiridoencerrado = @"
        SELECT  TOP 1 Dta_Fim_Plano
        FROM    BNE.BNE_Plano_Adquirido WITH(NOLOCK)
        WHERE   Idf_Plano_Situacao = 2 /* Encerrado */
                AND Idf_Filial = @Idf_Filial
        ORDER BY Dta_Fim_Plano DESC
        ";
        #endregion

        #region SpRecuperaDadosDasFiliaisParaEnvioAvisoSaldoSMS
        private const string SpRecuperaDadosDasFiliaisParaEnvioAvisoSaldoSMS = @"
        SELECT
            PA.Idf_Filial,
				PQ.Qtd_SMS,
				PQ.Qtd_SMS_Utilizado 
            FROM BNE_Plano_Adquirido PA WITH(NOLOCK)
            INNER JOIN BNE_Plano_Quantidade PQ WITH (NOLOCK) ON PA.Idf_Plano_Adquirido = PQ.Idf_Plano_Adquirido
            INNER JOIN BNE_Plano_Situacao PS WITH(NOLOCK) ON PA.Idf_Plano_Situacao = PS.Idf_Plano_Situacao
            OUTER APPLY(SELECT MAX(PAdic.Dta_Alteracao) as 'Dta_UltimaLiberacao_AdicionalPlano' 
			             FROM BNE_Adicional_Plano PAdic WITH(NOLOCK) 
                         WHERE PA.Idf_Plano_Adquirido = PAdic.Idf_Plano_Adquirido 
			             AND PAdic.Idf_Tipo_Adicional = 1  /*SMS Adicional*/
			             AND PAdic.Idf_Adicional_Plano_Situacao = 2  /*Liberado - Plano Adicional*/			 
			             ) PAdic
            LEFT JOIN TAB_Parametro_Filial Par WITH(NOLOCK) ON PA.Idf_Filial = Par.Idf_Filial AND Par.Idf_Parametro = 353 /*DataUltimoEnvioCartaAvisoSMSSaldoEmpresa*/
            WHERE
		            PA.Idf_Filial IS NOT NULL
		            AND CONVERT(DATE,PA.Dta_Inicio_Plano) <= CONVERT(DATE,@DataAnteriorIgualSeteDias) /*Verifica se Dta_Inicio_Plano é igual ou superior a 7 dias*/
					AND CONVERT(DATE,PA.Dta_Fim_Plano) >= CONVERT(DATE,PQ.Dta_Inicio_Quantidade) 
                    AND (PQ.Qtd_SMS - PQ.Qtd_SMS_Utilizado) > 0
		            AND PS.Idf_Plano_Situacao = 1  /*Liberado - Plano Adquirido*/
					AND PQ.Flg_Inativo = 0
		            AND (Par.Vlr_Parametro IS NULL 
			            OR DateDiff(Day,CONVERT(DATE,Par.Vlr_Parametro),GetDate()) > 30)
					AND (PAdic.Dta_UltimaLiberacao_AdicionalPlano IS NULL  
						OR CONVERT(DATE,PAdic.Dta_UltimaLiberacao_AdicionalPlano) <= CONVERT(DATE,@DataAnteriorIgualSeteDias)) /*Verifica se a data da última liberação de AdicionalSMS é igual ou superior a 7 dias*/	
            GROUP BY PA.Idf_filial, PQ.Qtd_SMS, PQ.Qtd_SMS_Utilizado, PAdic.Dta_UltimaLiberacao_AdicionalPlano
        ";
        #endregion

        #region Sprecuperarultimosplanoadquiridoencerrado
        private const string Sprecuperarultimosplanoadquiridoencerrado = @"
        SELECT  TOP (@top) PA.*
        FROM    BNE.BNE_Plano_Adquirido PA WITH(NOLOCK)
        WHERE   Idf_Plano_Situacao = 2 /* Encerrado */
                AND Idf_Filial = @Idf_Filial
        ORDER BY Dta_Fim_Plano DESC
        ";
        #endregion

        #region SpCancelarAssinaturaPlanoRecorrenteCia
        private const string SpCancelarAssinaturaPlanoRecorrenteCia = @"
        UPDATE BNE.BNE_Plano_Adquirido
        SET Flg_Recorrente = 0,
        Dta_Cancelamento = GETDATE()
        WHERE Idf_Plano_Adquirido = @Idf_Plano_Adquirido
        ";
        #endregion

        #region CriarEmailCancelamentoPlanoRecorrencia
        private const string CriarEmailCancelamentoPlanoRecorrencia = @"
        SELECT  DISTINCT
            IIF(F.Idf_Filial IS NOT NULL,CONCAT(Num_CNPJ, ' - ', Raz_Social),CONCAT(Num_CPF, ' - ', PF.Nme_Pessoa)) AS Nome ,
            CONCAT(C.Nme_Cidade,'/',C.Sig_Estado) AS Cidade ,
		    Nme_Vendedor AS Vendedor,
			Eml_Vendedor AS EmailVendedor,
		    p.Des_Plano AS DescricaoPlano,
			P.Idf_Plano AS CodigoPlano,
		    P.Vlr_Base AS ValorBase,
		    PG.Vlr_Pagamento AS ValorPago ,
		    CAST(FISTDTAPGTO.DataPrimeiroPGTO AS DATE)AS DataPrimeiroPGTO,
		    CAST(PA.Dta_Fim_Plano  AS DATE) AS DataFimPlano,
			PA.Dta_Cancelamento AS DataCancelamento,
		    Des_Tipo_Pagamaneto AS FormaPagamento,
			Idf_Curriculo AS IdCurriculo,
			F.Idf_Filial AS IdFilial
        FROM    
            BNE.BNE_Plano_Adquirido PA WITH ( NOLOCK )
            JOIN BNE.BNE_Plano P WITH ( NOLOCK ) ON P.Idf_Plano = PA.Idf_Plano
			JOIN BNE.TAB_Usuario_Filial_Perfil FP WITH(NOLOCK) ON FP.Idf_Usuario_Filial_Perfil = PA.Idf_Usuario_Filial_Perfil
			LEFT JOIN BNE.TAB_Pessoa_Fisica PF WITH(NOLOCK) ON PF.Idf_Pessoa_Fisica = FP.Idf_Pessoa_Fisica
			LEFT JOIN BNE.BNE_Curriculo CU WITH(NOLOCK) ON CU.Idf_Pessoa_Fisica = PF.Idf_Pessoa_Fisica
            LEFT JOIN BNE.TAB_Filial F WITH ( NOLOCK ) ON F.Idf_Filial = PA.Idf_Filial
            JOIN BNE.BNE_Plano_Parcela PP WITH ( NOLOCK ) ON PP.Idf_Plano_Adquirido = PA.Idf_Plano_Adquirido
            JOIN BNE.BNE_Pagamento PG WITH ( NOLOCK ) ON PG.Idf_Plano_Parcela = PP.Idf_Plano_Parcela
			JOIN BNE.BNE_Tipo_Pagamento TP WITH ( NOLOCK ) ON TP.Idf_Tipo_Pagamento = PG.Idf_Tipo_Pagamento
            LEFT JOIN BNE.TAB_Endereco E WITH ( NOLOCK ) ON E.Idf_Endereco = ISNULL(F.Idf_Endereco,PF.Idf_Endereco)
            LEFT JOIN plataforma.TAB_Cidade C WITH ( NOLOCK ) ON C.Idf_Cidade = E.Idf_Cidade
            OUTER APPLY ( SELECT TOP 1
                                    Nme_Pessoa
                          FROM      BNE.BNE_Codigo_Desconto CDU WITH ( NOLOCK )
                                    JOIN BNE.TAB_Usuario_Filial_Perfil FPU WITH ( NOLOCK ) ON FPU.Idf_Usuario_Filial_Perfil = CDU.Idf_Usuario_Filial_Perfil
								    JOIN BNE.TAB_Pessoa_Fisica PFU WITH(NOLOCK) ON PFU.Idf_Pessoa_Fisica = FPU.Idf_Pessoa_Fisica
                          WHERE     CDU.Idf_Codigo_Desconto = PG.Idf_Codigo_Desconto
                        ) AS VEN
		    OUTER APPLY ( SELECT TOP 1
                                    Dta_Pagamento AS DataPrimeiroPGTO
                          FROM      BNE.BNE_Plano_Parcela PPU WITH(NOLOCK) 
					      WHERE
								    PPU.Idf_Plano_Adquirido = PA.Idf_Plano_Adquirido
					      ORDER BY
								    Dta_Pagamento ASC
                        ) AS FISTDTAPGTO
			OUTER APPLY(SELECT * FROM BNE.SF_Retorna_Vendedor_Fez_Venda(PA.Idf_Plano_Adquirido)Vendedor) AS DADOS_VEND
        WHERE   P.Flg_Recorrente = 1
            AND PA.Idf_Plano_Adquirido = @idf_Plano_Adquirido";

        #endregion

        #region ExistePlanoAdquridoEnviadoOuNaoEnviadoDebitoHSBC

        private const string ExistePlanoAdquridoEnviadoOuNaoEnviadoJuridica = @"
        SELECT COUNT(1) AS qtd_trans
            FROM BNE.BNE_Transacao t 
			INNER JOIN BNE.BNE_Pagamento pag WITH(NOLOCK) ON t.Idf_Pagamento = pag.Idf_Pagamento
			INNER JOIN BNE.BNE_Plano_Parcela PP WITH(NOLOCK) ON PP.Idf_Plano_Parcela = pag.Idf_Plano_Parcela
            INNER JOIN BNE.BNE_Plano_Adquirido pa WITH(NOLOCK) ON pa.Idf_Plano_Adquirido = PP.Idf_Plano_Adquirido
            INNER JOIN BNE.BNE_Plano P ON P.Idf_Plano = pa.Idf_Plano
			WHERE t.Idf_Status_Transacao IN(0,5) --Não Enviada
		            AND t.Idf_Tipo_Pagamento = 8 -- Débito Recorrente
		            AND t.Idf_Banco = 399 --HSBC
					AND pag.Idf_Pagamento_Situacao = 1 --Em Aberto
		            AND pa.Idf_Plano_Situacao IN (0, 1) --Aguardando Liberação OU Liberado;
					AND pa.Idf_Filial = @Idf_Filial
                    AND Idf_Plano_Tipo = 2";


        private const string ExistePlanoAdquridoEnviadoOuNaoEnviadoPessoaFisica = @"
        SELECT COUNT(1) AS qtd_trans
            FROM BNE.BNE_Transacao t 
			INNER JOIN BNE.BNE_Pagamento pag WITH(NOLOCK) ON t.Idf_Pagamento = pag.Idf_Pagamento
			INNER JOIN BNE.BNE_Plano_Parcela PP WITH(NOLOCK) ON PP.Idf_Plano_Parcela = pag.Idf_Plano_Parcela
            INNER JOIN BNE.BNE_Plano_Adquirido pa WITH(NOLOCK) ON pa.Idf_Plano_Adquirido = PP.Idf_Plano_Adquirido
			INNER JOIN BNE.TAB_Usuario_Filial_Perfil FP WITH(NOLOCK) ON FP.Idf_Usuario_Filial_Perfil = pag.Idf_Usuario_Filial_Perfil
            WHERE t.Idf_Status_Transacao IN(0,5) --Não Enviada
		            AND t.Idf_Tipo_Pagamento = 8 -- Débito Recorrente
		            AND t.Idf_Banco = 399 --HSBC
					AND pag.Idf_Pagamento_Situacao = 1 --Em Aberto
		            AND pa.Idf_Plano_Situacao IN (0, 1) --Aguardando Liberação OU Liberado;
					AND pa.Idf_Filial IS NULL
					AND Idf_Pessoa_Fisica =	@Idf_Pessoa_Fisica";

        #endregion

        #region SpQuantidadeParcelasCobrancaCartao

        private const string SpQuantidadeParcelasCobrancaCartao = @"
        SELECT  COUNT(1) AS qtd
FROM    BNE.BNE_Plano_Adquirido PA WITH ( NOLOCK )
        JOIN BNE.BNE_Plano_Parcela pp WITH ( NOLOCK ) ON pp.Idf_Plano_Adquirido = PA.Idf_Plano_Adquirido
        JOIN BNE.BNE_Pagamento PG WITH ( NOLOCK ) ON PG.Idf_Plano_Parcela = pp.Idf_Plano_Parcela
        JOIN BNE.BNE_Plano P WITH ( NOLOCK ) ON P.Idf_Plano = PA.Idf_Plano
WHERE   p.Qtd_Parcela > 1
		AND PA.Idf_Plano_Adquirido = @Idf_Plano_Adquirido
		AND Idf_Tipo_Pagamento = 1
		AND Idf_Pagamento_Situacao IN (1,2)";

        #endregion

        #region SPExisteVipPlanoLiberadoVencimentoMenosDeXDias

        private const string SPExisteVipPlanoLiberadoVencimentoMenosDeXDias = @"
        SELECT  COUNT(1) AS qtd_trans
        FROM    BNE.BNE_Transacao t
                INNER JOIN BNE.BNE_Pagamento pag WITH ( NOLOCK ) ON t.Idf_Pagamento = pag.Idf_Pagamento
                INNER JOIN BNE.BNE_Plano_Parcela PP WITH ( NOLOCK ) ON PP.Idf_Plano_Parcela = pag.Idf_Plano_Parcela
                INNER JOIN BNE.BNE_Plano_Adquirido pa WITH ( NOLOCK ) ON pa.Idf_Plano_Adquirido = PP.Idf_Plano_Adquirido
                INNER JOIN BNE.TAB_Usuario_Filial_Perfil FP WITH ( NOLOCK ) ON FP.Idf_Usuario_Filial_Perfil = pag.Idf_Usuario_Filial_Perfil
				INNER JOIN BNE.BNE_Plano P WITH ( NOLOCK )  ON P.Idf_Plano = pa.Idf_Plano
                INNER JOIN BNE.TAB_Pessoa_Fisica PF ON PF.Idf_Pessoa_Fisica = FP.Idf_Pessoa_Fisica
        WHERE   PF.Idf_Pessoa_Fisica = @Idf_Pessoa_Fisica
				AND Idf_Plano_Tipo = 1 --Pessoa Física
                AND ( ( Idf_Plano_Situacao IN ( 5, 6 ) )
                      OR ( Idf_Plano_Situacao = 1
                           AND Dta_Fim_Plano > DATEADD(DAY, @Dias, GETDATE())
                         )
                    )";

        #endregion

        private const string SPExistePlanoVipLiberado = @"
SELECT  COUNT(*) qtd_trans
FROM    BNE.TAB_Pessoa_Fisica PF
        JOIN BNE.TAB_Usuario_Filial_Perfil FP ON FP.Idf_Pessoa_Fisica = PF.Idf_Pessoa_Fisica
        JOIN BNE.BNE_Plano_Adquirido PA ON PA.Idf_Usuario_Filial_Perfil = FP.Idf_Usuario_Filial_Perfil
WHERE
		PF.Idf_Pessoa_Fisica = @Idf_Pessoa_Fisica
		AND Idf_Plano_Situacao = 1";

        #region SP_LISTA_TODOS_PLANOS_COM_RECORRENCIA_VENCIDA
        private const string SP_LISTA_TODOS_PLANOS_WEBFORPAG = @"
        SELECT  
                PA.Idf_Plano_Adquirido 
        FROM    BNE.BNE_Plano_Adquirido PA WITH ( NOLOCK )
                JOIN BNE.BNE_Plano_Parcela PP WITH ( NOLOCK ) ON PP.Idf_Plano_Adquirido = PA.Idf_Plano_Adquirido
                JOIN BNE.BNE_Pagamento PG WITH ( NOLOCK ) ON PG.Idf_Plano_Parcela = PP.Idf_Plano_Parcela
        WHERE   Idf_Plano IN ( 609 )
                AND Idf_Plano_Situacao = 1
                AND Idf_Pagamento_Situacao = 1
                AND Dta_Vencimento <= GETDATE()";

        #endregion

        #region [spSelectVagasRecebidasPeloJornal]
        private const string spSelectVagasRecebidasPeloJornal = @" SELECT obs_mensagem FROM alerta.Log_Envio_Mensagem with(nolock)
                                                            WHERE Idf_Curriculo = @Idf_Curriculo and dta_cadastro >= @Dta_Cadastro ";
        #endregion

        #endregion

        #region Métodos

        #region ConcederPlanoPessoaJuridica
        /// <summary>
        /// Método responsável por conceder um plano para empresa. 
        /// </summary>
        /// <returns>Status do procedimento de aquisição de plano</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static bool ConcederPlanoPessoaJuridica(UsuarioFilialPerfil objUsuarioFilialPerfil, Plano objPlano, SqlTransaction trans)
        {
            objPlano.CompleteObject(trans);

            #region BNE_Plano_Adquirido

            var objPlanoAdquirido = new PlanoAdquirido
            {
                UsuarioFilialPerfil = objUsuarioFilialPerfil,
                Plano = objPlano,
                DataInicioPlano = DateTime.Today,
                DataFimPlano = DateTime.Today.AddDays(objPlano.QuantidadeDiasValidade),
                PlanoSituacao = new PlanoSituacao((int)Enumeradores.PlanoSituacao.Liberado),
                Filial = objUsuarioFilialPerfil.Filial,
                ValorBase = objPlano.ValorBase,
                QuantidadeSMS = objPlano.QuantidadeSMS,
                FlagBoletoRegistrado = objPlano.FlagBoletoRegistrado
            };

            objPlanoAdquirido.Save(trans);

            #endregion

            #region PlanoParcela

            var objPlanoParcela = new PlanoParcela
            {
                PlanoAdquirido = objPlanoAdquirido,
                DataPagamento = null,
                ValorParcela = objPlano.ValorBase,
                FlagInativo = false,
                QuantidadeSMSTotal = objPlanoAdquirido.QuantidadeSMS / (objPlano.QuantidadeParcela == 0 ? 1 : objPlano.QuantidadeParcela),
                PlanoParcelaSituacao = new PlanoParcelaSituacao((int)Enumeradores.PlanoParcelaSituacao.Pago)
            };
            objPlanoParcela.Save(trans);

            #endregion

            #region PlanoQuantidade

            var objPlanoQuantidade = new PlanoQuantidade
            {
                DataInicioQuantidade = DateTime.Now,
                DataFimQuantidade = DateTime.Now.AddDays(objPlano.QuantidadeDiasValidade + 5),
                QuantidadeSMS = objPlanoAdquirido.QuantidadeSMS,
                QuantidadeVisualizacao = objPlano.QuantidadeVisualizacao,
                QuantidadeSMSUtilizado = 0,
                QuantidadeVisualizacaoUtilizado = 0,
                QuantidadeCampanha = objPlano.QuantidadeCampanha,
                QuantidadeCampanhaUtilizado = 0,
                PlanoAdquirido = objPlanoAdquirido
            };

            objPlanoQuantidade.Save(trans);

            #endregion

            return true;
        }
        #endregion

        #region CarregarPlanoVigentePessoaJuridica
        /// <summary>
        /// Método responsável por carregar uma instancia de Plano de Empresa
        /// </summary>
        /// <returns>Boolean</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static bool CarregarPlanoVigentePessoaJuridicaPorSituacao(Filial objFilial, PlanoSituacao objPlanoSituacao, out PlanoAdquirido objPlanoAdquirido)
        {
            return CarregarPlanoVigentePessoaJuridicaPorSituacao(objFilial, objPlanoSituacao, out objPlanoAdquirido, null);
        }
        public static bool CarregarPlanoVigentePessoaJuridicaPorSituacao(Filial objFilial, PlanoSituacao objPlanoSituacao, out PlanoAdquirido objPlanoAdquirido, SqlTransaction trans)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter{ ParameterName = "@Idf_Filial", SqlDbType = SqlDbType.Int, Size = 4, Value = objFilial.IdFilial },
                    new SqlParameter{ ParameterName = "@Idf_Plano_Situacao", SqlDbType = SqlDbType.Int, Size = 4, Value = objPlanoSituacao.IdPlanoSituacao }
                };

            IDataReader dr = trans != null ? DataAccessLayer.ExecuteReader(trans, CommandType.Text, Spselectplanovigentepessoajuridicaporsituacao, parms) : DataAccessLayer.ExecuteReader(CommandType.Text, Spselectplanovigentepessoajuridicaporsituacao, parms);

            objPlanoAdquirido = new PlanoAdquirido();

            if (SetInstance(dr, objPlanoAdquirido))
                return true;

            if (!dr.IsClosed)
                dr.Close();

            dr.Dispose();

            objPlanoAdquirido = null;

            return false;
        }
        #endregion

        #region CarregarPlanoVigentePessoaFisicaPorSituacao
        /// <summary>
        /// Método responsável por carregar uma instancia de Plano VIP através do
        /// identificar de um curriculo
        /// </summary>
        /// <param name="idPessoaFisica">Identificador do Usuario </param>
        /// <param name="objPlanoSituacao"> </param>
        /// <param name="objPlanoAdquirido"> </param>
        /// <returns>Boolean</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static bool CarregarPlanoVigentePessoaFisicaPorSituacao(int idPessoaFisica, PlanoSituacao objPlanoSituacao, out PlanoAdquirido objPlanoAdquirido)
        {
            return CarregarPlanoVigentePessoaFisicaPorSituacao(null, idPessoaFisica, objPlanoSituacao, out objPlanoAdquirido);
        }
        /// <summary>
        /// Método responsável por carregar uma instancia de Plano VIP através do
        /// identificar de um curriculo
        /// </summary>
        /// <param name="trans"> </param>
        /// <param name="idPessoaFisica">Identificador do Usuario </param>
        /// <param name="objPlanoSituacao"> </param>
        /// <param name="objPlanoAdquirido"> </param>
        /// <returns>Boolean</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static bool CarregarPlanoVigentePessoaFisicaPorSituacao(SqlTransaction trans, int idPessoaFisica, PlanoSituacao objPlanoSituacao, out PlanoAdquirido objPlanoAdquirido)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter{ ParameterName = "@Idf_Pessoa_Fisica", SqlDbType = SqlDbType.Int, Size = 4, Value = idPessoaFisica },
                    new SqlParameter{ ParameterName = "@Idf_Plano_Situacao", SqlDbType = SqlDbType.Int, Size = 4, Value = objPlanoSituacao.IdPlanoSituacao }
                };

            IDataReader dr = trans != null ? DataAccessLayer.ExecuteReader(trans, CommandType.Text, Spselectplanovigentepessoafisicaporsituacao, parms) : DataAccessLayer.ExecuteReader(CommandType.Text, Spselectplanovigentepessoafisicaporsituacao, parms);

            objPlanoAdquirido = new PlanoAdquirido();

            if (SetInstance(dr, objPlanoAdquirido))
                return true;

            if (!dr.IsClosed)
                dr.Close();

            dr.Dispose();

            objPlanoAdquirido = null;
            return false;
        }
        #endregion

        #region CarregarPlanoAdquiridoPorSituacao
        /// <summary>
        /// Método responsável por carregar uma instancia de plano candidato
        /// </summary>
        /// <returns>Boolean</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static bool CarregarPlanoAdquiridoPorSituacao(Curriculo objCurriculo, int idfPlanoSituacao, out PlanoAdquirido objPlanoAdquirido)
        {
            return CarregarPlanoAdquiridoPorSituacao(objCurriculo, idfPlanoSituacao, out objPlanoAdquirido, null);
        }
        public static bool CarregarPlanoAdquiridoPorSituacao(Curriculo objCurriculo, int idfPlanoSituacao, out PlanoAdquirido objPlanoAdquirido, SqlTransaction trans)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter{ ParameterName = "@Idf_Curriculo", SqlDbType = SqlDbType.Int, Size = 4, Value = objCurriculo.IdCurriculo },
                    new SqlParameter{ ParameterName = "@Idf_Plano_Situacao", SqlDbType = SqlDbType.Int, Size = 4, Value = idfPlanoSituacao }
                };

            bool retorno;

            IDataReader dr = trans != null ? DataAccessLayer.ExecuteReader(trans, CommandType.Text, Spselectretornaplanoscandidatoporsituacao, parms) : DataAccessLayer.ExecuteReader(CommandType.Text, Spselectretornaplanoscandidatoporsituacao, parms);

            objPlanoAdquirido = new PlanoAdquirido();

            if (SetInstance(dr, objPlanoAdquirido))
                retorno = true;
            else
            {
                objPlanoAdquirido = null;
                retorno = false;
            }

            if (!dr.IsClosed)
                dr.Close();

            dr.Dispose();

            return retorno;
        }
        public static bool CarregarPlanoAdquiridoPorSituacao(Filial objFilial, int idfPlanoSituacao, out PlanoAdquirido objPlanoAdquirido)
        {
            return CarregarPlanoAdquiridoPorSituacao(objFilial, idfPlanoSituacao, out objPlanoAdquirido, null);
        }
        public static bool CarregarPlanoAdquiridoPorSituacao(Filial objFilial, int idfPlanoSituacao, out PlanoAdquirido objPlanoAdquirido, SqlTransaction trans)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter{ ParameterName = "@Idf_Filial", SqlDbType = SqlDbType.Int, Size = 4, Value = objFilial.IdFilial },
                    new SqlParameter{ ParameterName = "@Idf_Plano_Situacao", SqlDbType = SqlDbType.Int, Size = 4, Value = idfPlanoSituacao }
                };

            bool retorno;

            IDataReader dr = trans != null ? DataAccessLayer.ExecuteReader(trans, CommandType.Text, Spselectretornaplanosfilialporsituacao, parms) : DataAccessLayer.ExecuteReader(CommandType.Text, Spselectretornaplanosfilialporsituacao, parms);

            objPlanoAdquirido = new PlanoAdquirido();

            if (SetInstance(dr, objPlanoAdquirido))
            {
                retorno = true;
                objPlanoAdquirido.PlanoQuantidade = new PlanoQuantidade(objPlanoAdquirido, trans);
            }
            else
            {
                objPlanoAdquirido = null;
                retorno = false;
            }

            if (!dr.IsClosed)
                dr.Close();

            dr.Dispose();

            return retorno;
        }
        #endregion

        #region CarregarPlanoAdquiridoPorSituacao
        /// <summary>
        /// Método responsável por carregar uma instancia de plano candidato
        /// </summary>
        /// <returns>Boolean</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static List<PlanoAdquirido> CarregarPlanosAdquiridoPorSituacao(Curriculo objCurriculo, int idfPlanoSituacao)
        {
            return CarregarPlanosAdquiridoPorSituacao(objCurriculo, idfPlanoSituacao, null);
        }
        public static List<PlanoAdquirido> CarregarPlanosAdquiridoPorSituacao(Curriculo objCurriculo, int idfPlanoSituacao, SqlTransaction trans)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter{ ParameterName = "@Idf_Curriculo", SqlDbType = SqlDbType.Int, Size = 4, Value = objCurriculo.IdCurriculo },
                    new SqlParameter{ ParameterName = "@Idf_Plano_Situacao", SqlDbType = SqlDbType.Int, Size = 4, Value = idfPlanoSituacao }
                };

            var lista = new List<PlanoAdquirido>();

            IDataReader dr = trans != null ? DataAccessLayer.ExecuteReader(trans, CommandType.Text, Spselectretornaplanoscandidatoporsituacao, parms) : DataAccessLayer.ExecuteReader(CommandType.Text, Spselectretornaplanoscandidatoporsituacao, parms);

            while (dr.Read())
            {
                var objPlanoAdquirido = new PlanoAdquirido();
                if (SetInstanceNotDipose(dr, objPlanoAdquirido))
                    lista.Add(objPlanoAdquirido);
            }

            if (!dr.IsClosed)
                dr.Close();

            dr.Dispose();

            return lista;
        }
        public static List<PlanoAdquirido> CarregarPlanosAdquiridoPorSituacao(Filial objFilial, int idfPlanoSituacao)
        {
            return CarregarPlanosAdquiridoPorSituacao(objFilial, idfPlanoSituacao, null);
        }
        public static List<PlanoAdquirido> CarregarPlanosAdquiridoPorSituacao(Filial objFilial, int idfPlanoSituacao, SqlTransaction trans)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter{ ParameterName = "@Idf_Filial", SqlDbType = SqlDbType.Int, Size = 4, Value = objFilial.IdFilial },
                    new SqlParameter{ ParameterName = "@Idf_Plano_Situacao", SqlDbType = SqlDbType.Int, Size = 4, Value = idfPlanoSituacao }
                };

            var lista = new List<PlanoAdquirido>();

            IDataReader dr = trans != null ? DataAccessLayer.ExecuteReader(trans, CommandType.Text, Spselectretornaplanosfilialporsituacao, parms) : DataAccessLayer.ExecuteReader(CommandType.Text, Spselectretornaplanosfilialporsituacao, parms);

            while (dr.Read())
            {
                var objPlanoAdquirido = new PlanoAdquirido();
                if (SetInstanceNotDipose(dr, objPlanoAdquirido))
                    lista.Add(objPlanoAdquirido);
            }

            if (!dr.IsClosed)
                dr.Close();

            dr.Dispose();

            return lista;
        }
        #endregion

        #region CarregarPlanoAdquiridoAguardandoLiberacao
        /// <summary>
        /// Método responsável por carregar uma instancia de Plano de Empresa
        /// </summary>
        /// <returns>Boolean</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static bool CarregarPlanoAdquiridoAguardandoLiberacao(Curriculo objCurriculo, Plano objPlano, out PlanoAdquirido objPlanoAdquirido)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter{ ParameterName = "@Idf_Curriculo", SqlDbType = SqlDbType.Int, Size = 4, Value = objCurriculo.IdCurriculo },
                    new SqlParameter{ ParameterName = "@Idf_Plano", SqlDbType = SqlDbType.Int, Size = 4, Value = objPlano.IdPlano }
                };

            bool retorno;

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Sprecuperarplanoadquiridoaguardandoliberacaoporcandidatoplano, parms))
            {
                objPlanoAdquirido = new PlanoAdquirido();

                if (SetInstance(dr, objPlanoAdquirido))
                    retorno = true;
                else
                {
                    objPlanoAdquirido = null;
                    retorno = false;
                }

                if (!dr.IsClosed)
                    dr.Close();

                dr.Dispose();
            }

            return retorno;
        }
        public static bool CarregarPlanoAdquiridoAguardandoLiberacao(Filial objFilial, Plano objPlano, out PlanoAdquirido objPlanoAdquirido)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter{ ParameterName = "@Idf_Filial", SqlDbType = SqlDbType.Int, Size = 4, Value = objFilial.IdFilial },
                    new SqlParameter{ ParameterName = "@Idf_Plano", SqlDbType = SqlDbType.Int, Size = 4, Value = objPlano.IdPlano }
                };

            bool retorno;

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Sprecuperarplanoadquiridoaguardandoliberacaoporfilialplano, parms))
            {
                objPlanoAdquirido = new PlanoAdquirido();

                if (SetInstance(dr, objPlanoAdquirido))
                    retorno = true;
                else
                {
                    objPlanoAdquirido = null;
                    retorno = false;
                }

                if (!dr.IsClosed)
                    dr.Close();

                dr.Dispose();
            }

            return retorno;
        }
        #endregion

        #region CarregaListaPlanoAdquiridoLiberadoOuEmAberto
        public static List<PlanoAdquirido> CarregaListaPlanoAdquiridoLiberadoOuEmAberto(int? idfUsuarioFilialPerfil, int? idFilial = null)
        {
            object paramValueUsuarioFilialPerfil = DBNull.Value;
            object paramValueFilial = DBNull.Value;

            if (idfUsuarioFilialPerfil.HasValue)
                paramValueUsuarioFilialPerfil = idfUsuarioFilialPerfil;

            if (idFilial.HasValue)
                paramValueFilial = idFilial;

            var idfPlanoPremium = Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.PlanoCandidaturaPremium));
            var parms = new List<SqlParameter>
                {
                    new SqlParameter{ ParameterName = "@Idf_Usuario_Filial_Perfil", SqlDbType = SqlDbType.Int, Size = 4, Value = paramValueUsuarioFilialPerfil },
                    new SqlParameter{ ParameterName = "@Idf_Filial", SqlDbType = SqlDbType.Int, Size = 4, Value = paramValueFilial },
                    new SqlParameter{ ParameterName = "@Idf_Plano", SqlDbType = SqlDbType.Int, Size =4,Value = idfPlanoPremium}
                };

            var listPlanoAdquirido = new List<PlanoAdquirido>();
            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spselectretornaplanoadquiridoliberadoemaberto, parms))
            {
                while (dr.Read())
                {
                    var objPlanoAdquirido = new PlanoAdquirido();
                    if (SetInstanceNotDipose(dr, objPlanoAdquirido))
                        listPlanoAdquirido.Add(objPlanoAdquirido);
                }
            }

            return listPlanoAdquirido;
        }
        #endregion

        #region ExistePlanoAdquiridoRenovacao
        public static bool ExistePlanoAdquiridoRenovacao(int idUsuarioFilialPerfil, int idPlanoAdquirido, out DateTime dataFimPlanoAtual)
        {
            dataFimPlanoAtual = DateTime.Now;

            var parms = new List<SqlParameter>
                {
                    new SqlParameter{ ParameterName = "@Idf_Usuario_Filial_Perfil", SqlDbType = SqlDbType.Int, Size = 4, Value = idUsuarioFilialPerfil},
                    new SqlParameter{ ParameterName = "@Idf_Plano_Adquirido", SqlDbType = SqlDbType.Int, Size = 4, Value = idPlanoAdquirido}
                };
            IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SpselectexisteplanoadquiridoRenovacao, parms);

            if (dr.Read())
            {
                dataFimPlanoAtual = Convert.ToDateTime(dr["Dta_Fim_Plano"]);

                return true;
            }

            return false;
        }
        #endregion

        #region ExistePlanoAdquiridoFilial

        public static bool ExistePlanoAdquiridoFilial(int idFilial, Enumeradores.PlanoSituacao planoSituacao)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter{ ParameterName = "@Idf_Filial", SqlDbType = SqlDbType.Int, Size = 4, Value = idFilial },
                    new SqlParameter{ ParameterName = "@Idf_Plano_Situacao", SqlDbType = SqlDbType.Int, Size = 4, Value = (int)planoSituacao }
                };

            return Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, Spvarificaplanoadquiridofilialporsituacao, parms)) > 0;
        }
        #endregion

        #region ExistePlanoAdquiridoCandidato
        public static bool ExistePlanoAdquiridoCandidato(Curriculo objCurriculo, Enumeradores.PlanoSituacao planoSituacao, SqlTransaction trans)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter{ ParameterName = "@Idf_Curriculo", SqlDbType = SqlDbType.Int, Size = 4, Value = objCurriculo.IdCurriculo },
                    new SqlParameter{ ParameterName = "@Idf_Plano_Situacao", SqlDbType = SqlDbType.Int, Size = 4, Value = (int)planoSituacao }
                };

            return Convert.ToInt32(DataAccessLayer.ExecuteScalar(trans, CommandType.Text, Spvarificaplanoadquiridocandidatoporsituacao, parms)) > 0;
        }
        #endregion

        #region AjustarParcelas
        public bool AjustarParcelas(TipoPagamento objTipoPagamento, CodigoDesconto objCodigoDesconto, int? prazoBoleto)
        {
            using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        /*if (!prazoBoleto.HasValue)
                        {
                            if (this.ParaPessoaJuridica(trans))
                        {

                        }
                            if (this.ParaPessoaFisica(trans))
                    {

                }
                        }*/

                        this.AjustarParcelas(this, DateTime.Now.AddDays(prazoBoleto ?? 0), DateTime.Now, objTipoPagamento, objCodigoDesconto, trans);

                        trans.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        EL.GerenciadorException.GravarExcecao(ex);
                        throw;
                    }
                }
            }
        }
        #endregion

        #region CriarPlanoAdquiridoParcelaPagamento
        /// <summary>
        /// Cria um novo planoAdquiro, PlanoParcela, Pagamento e PLanoQuantidade em caso de PJ.
        /// </summary>
        /// <param name="objUsuarioFilialPerfil"></param>
        /// <param name="objPlano"></param>
        /// <returns></returns>
        public static PlanoAdquirido CriarPlanoAdquiridoPF(UsuarioFilialPerfil objUsuarioFilialPerfil, Plano objPlano, int? quantidadePrazoBoleto = null)
        {
            return CriarPlanoAdquiridoParcelaPagamento(objUsuarioFilialPerfil, null, null, objPlano, quantidadePrazoBoleto);
        }
        /// <summary>
        /// Cria um novo planoAdquiro, PlanoParcela, Pagamento e PLanoQuantidade em caso de PJ.
        /// </summary>
        /// <param name="objUsuarioFilialPerfil"></param>
        /// <param name="objUsuarioFilial">Usuário filial que será utilizado para recuperar informações de telefone e e-mail</param>
        /// <param name="objPlano"></param>
        /// <param name="dtInicio">Parametro usado para atricuir a data Inicial dos novos objetos.</param>
        /// <param name="objFilial">Filial para qual será aberto o plano</param>
        /// <returns></returns>
        public static PlanoAdquirido CriarPlanoAdquiridoPJ(UsuarioFilialPerfil objUsuarioFilialPerfil, Filial objFilial, UsuarioFilial objUsuarioFilial, Plano objPlano, int quantidadePrazoBoleto, Vaga objVaga = null)
        {
            return CriarPlanoAdquiridoParcelaPagamento(objUsuarioFilialPerfil, objFilial, objUsuarioFilial, objPlano, quantidadePrazoBoleto, objVaga);
        }

        /// <summary>
        /// Cria um novo planoAdquiro, PlanoParcela, Pagamento e PLanoQuantidade em caso de PJ.
        /// </summary>
        /// <param name="objUsuarioFilialPerfil"></param>
        /// <param name="objFilial"></param>
        /// <param name="objPlano"></param>
        /// <param name="objVaga">Vaga - Quando é um plano para impulsionar vaga</param>
        /// <returns></returns>
        private static PlanoAdquirido CriarPlanoAdquiridoParcelaPagamento(UsuarioFilialPerfil objUsuarioFilialPerfil, Filial objFilial, UsuarioFilial objUsuarioFilial, Plano objPlano, int? quantidadePrazoBoleto, Vaga objVaga = null)
        {
            using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        var dataInicioPlano = DateTime.Now;

                        var objPlanoAdquirido = new PlanoAdquirido
                        {
                            Filial = objFilial,
                            Plano = objPlano,
                            UsuarioFilialPerfil = objUsuarioFilialPerfil,
                            PlanoSituacao = new PlanoSituacao((int)Enumeradores.PlanoSituacao.AguardandoLiberacao),
                            DataInicioPlano = dataInicioPlano,
                            DataFimPlano = dataInicioPlano.AddDays(objPlano.QuantidadeDiasValidade),
                            QuantidadeSMS = objPlano.QuantidadeSMS,
                            ValorBase = objPlano.ValorBase,
                            QuantidadePrazoBoleto = quantidadePrazoBoleto.HasValue ? quantidadePrazoBoleto : 0,
                            FlagBoletoRegistrado = objPlano.FlagBoletoRegistrado,
                            FlagRecorrente = objPlano.FlagRecorrente,
                            QtdParcela = objPlano.QuantidadeParcela
                        };

                        objPlanoAdquirido.Save(trans);

                        //Cria o Plano Quantidade se for um plano Empresa
                        if (objPlanoAdquirido.ParaPessoaJuridica(trans))
                        {
                            var objPlanoQuantidade = new PlanoQuantidade
                            {
                                DataInicioQuantidade = objPlanoAdquirido.DataInicioPlano,
                                DataFimQuantidade = objPlanoAdquirido.DataInicioPlano.AddDays(objPlano.QuantidadeDiasValidade),
                                FlagInativo = false,
                                PlanoAdquirido = objPlanoAdquirido,
                                QuantidadeSMS = 0,
                                QuantidadeSMSUtilizado = 0,
                                QuantidadeVisualizacao = objPlano.QuantidadeVisualizacao,
                                QuantidadeVisualizacaoUtilizado = 0,
                                QuantidadeCampanha = objPlano.QuantidadeCampanha,
                                QuantidadeCampanhaUtilizado = 0
                            };
                            objPlanoQuantidade.Save(trans);

                            var objPlanoAdquiridoDetalhes = new PlanoAdquiridoDetalhes
                            {
                                EmailEnvioBoleto = objUsuarioFilial.EmailComercial,
                                FlagNotaFiscal = true,
                                NomeResPlanoAdquirido = objUsuarioFilialPerfil.PessoaFisica.NomeCompleto,
                                NumeroResDDDTelefone = objUsuarioFilial.NumeroDDDComercial,
                                NumeroResTelefone = objUsuarioFilial.NumeroComercial,
                                Funcao = objUsuarioFilial.Funcao,
                                PlanoAdquirido = objPlanoAdquirido,
                                FilialGestora = new Filial(Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.FilialGestoraPadraoDoPlano))),
                                Vaga = objVaga
                            };
                            objPlanoAdquiridoDetalhes.Save(trans);
                        }

                        trans.Commit();
                        return objPlanoAdquirido;
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        EL.GerenciadorException.GravarExcecao(ex);
                        throw;
                    }
                }
            }

        }
        #endregion

        #region CriarParcelas
        /// <summary>
        /// Cria as parcelas para o plano adquirido
        /// </summary>
        /// <param name="objTipoPagamento"></param>
        /// <param name="prazoBoleto"></param>
        /// <returns></returns>
        /// 
        public void CriarParcelas(TipoPagamento objTipoPagamento, CodigoDesconto objCodigoDesconto, int? prazoBoleto, int? quantidadeParcelas)
        {
            using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.CalcularParcelas(DateTime.Now.AddDays(prazoBoleto ?? 0), DateTime.Now, null, objTipoPagamento, objCodigoDesconto, trans, quantidadeParcelas);

                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        EL.GerenciadorException.GravarExcecao(ex);
                        throw;
                    }
                }
            }

        }
        #endregion

        #region CriarPagamento
        public static Pagamento CriarPagamento(PlanoParcela objPlanoParcela, Plano objPlano, Filial objFilial, UsuarioFilialPerfil objUsuarioFilialPerfil, TipoPagamento objTipoPagamento, decimal? valorPagamento = null, int? idCodigoDesconto = null)
        {
            objPlanoParcela.CompleteObject();
            objPlanoParcela.PlanoAdquirido.CompleteObject();

            var objPagamentoNovo = new Pagamento
            {
                TipoPagamento = objTipoPagamento,
                PlanoParcela = objPlanoParcela,
                DataEmissao = DateTime.Now,
                DataVencimento = DateTime.Today,
                UsuarioFilialPerfil = objUsuarioFilialPerfil,
                ValorPagamento = valorPagamento.HasValue ? valorPagamento.Value : objPlanoParcela.PlanoAdquirido.ValorBase,
                PagamentoSituacao = new PagamentoSituacao((int)Enumeradores.PagamentoSituacao.EmAberto),
                Filial = objFilial
            };

            if (idCodigoDesconto.HasValue)
            {
                objPagamentoNovo.CodigoDesconto = new CodigoDesconto(idCodigoDesconto.Value);
            }

            //Caso o tipo do pagamento seja débito, define o número de dias do vencimento baseado no parâmetro.
            //Necessário pois alguns bancos pedem um limite mínimo de dias para o envio da requisição de débito
            Int32 diasMinimoParaVencimento = Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.PagamentoDebitoDiasMinimosVencimentoParaEnvio));
            if (objTipoPagamento.IdTipoPagamento == (int)Enumeradores.TipoPagamento.DebitoRecorrente)
            {
                objPagamentoNovo.DataVencimento = DateTime.Now.AddDays(diasMinimoParaVencimento);
            }

            objPagamentoNovo.Save();

            return objPagamentoNovo;
        }
        #endregion

        #region AtualizarPagamento
        /// <summary>
        /// Metodo responsávelpor atualizar o pagamento
        /// </summary>
        /// <param name="objPagamento">Object</param>
        /// <param name="objTipoPagamento">Object</param>
        /// <param name="objPlanoAdquirido">Plano adquirido que está sendo manipulado pelo processo de pagamento </param>
        /// <param name="objPlano">O plano escolhido pelo usuário para efetuar a compra </param>
        /// <returns>Object</returns>
        public static Pagamento AtualizarPagamento(Pagamento objPagamento, TipoPagamento objTipoPagamento, PlanoAdquirido objPlanoAdquirido, Plano objPlano)
        {
            using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();

                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        objPagamento.TipoPagamento = objTipoPagamento;
                        objPagamento.ValorPagamento = objPlanoAdquirido.ValorBase;
                        objPagamento.DataEmissao = DateTime.Now;
                        objPagamento.DataVencimento = DateTime.Today;
                        objPagamento.PagamentoSituacao = new PagamentoSituacao((int)Enumeradores.PagamentoSituacao.EmAberto);
                        objPagamento.Save(trans);

                        trans.Commit();

                        return objPagamento;
                    }
                    catch (Exception ex)
                    {
                        EL.GerenciadorException.GravarExcecao(ex);
                        throw;
                    }
                }
            }
        }
        #endregion

        #region CarregarTodosPlanosPessoaFisica
        public static DataTable CarregarTodosPlanosPessoaFisica(int idPessoaFisica, int paginaCorrente, int tamanhoPagina, out int totalRegistros)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter {ParameterName = "@CurrentPage", SqlDbType = SqlDbType.Int, Size = 4, Value = paginaCorrente},
                    new SqlParameter {ParameterName = "@PageSize", SqlDbType = SqlDbType.Int, Size = 4, Value = tamanhoPagina},
                    new SqlParameter {ParameterName = "@Idf_Pessoa_fisica", SqlDbType = SqlDbType.Int, Size = 4, Value = idPessoaFisica}
                };

            totalRegistros = 0;
            DataTable dt = null;
            try
            {
                using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Splistartodosplanospessoafisica, parms))
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

        #region CarregarTodosPlanosPessoaJuridica

        public static PlanoAdquirido CarregarUltimoPlanoPessoaJuridica(int idFilial, SqlTransaction trans = null)
        {
            int count;
            var res = CarregarTodosPlanosPessoaJuridica(idFilial, 1, 1, out count);
            if (count <= 0)
                return null;

            var selection = res.Rows[0];

            var pa = new PlanoAdquirido(Convert.ToInt32(selection["Idf_Plano_Adquirido"]));

            if (trans != null)
                pa.CompleteObject(trans);
            else
                pa.CompleteObject();

            pa.Plano = new Plano(Convert.ToInt32(selection["Idf_Plano"]));

            if (trans != null)
                pa.Plano.CompleteObject(trans);
            else
                pa.Plano.CompleteObject();

            pa.PlanoSituacao = new PlanoSituacao(Convert.ToInt32(selection["Idf_Plano_Situacao"]));

            if (trans != null)
                pa.PlanoSituacao.CompleteObject(trans);
            else
                pa.PlanoSituacao.CompleteObject();

            return pa;
        }

        public static DataTable CarregarTodosPlanosPessoaJuridica(int idFilial, int paginaCorrente, int tamanhoPagina, out int totalRegistros)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter {ParameterName = "@CurrentPage", SqlDbType = SqlDbType.Int, Size = 4, Value = paginaCorrente},
                    new SqlParameter {ParameterName = "@PageSize", SqlDbType = SqlDbType.Int, Size = 4, Value = tamanhoPagina},
                    new SqlParameter {ParameterName = "@Idf_Filial", SqlDbType = SqlDbType.Int, Size = 4, Value = idFilial}
                };

            totalRegistros = 0;
            DataTable dt = null;
            try
            {
                using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Splistartodosplanospessoajuridica, parms))
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objUsuarioFilialPerfil"></param>
        /// <param name="objFilial"></param>
        /// <param name="objPlano"></param>
        /// <param name="objPlanoAdquiridoDetalhes"></param>
        /// <param name="objUsuarioGerador">Utilizado para salvar histórico de liberação de um plano novo</param>
        public void SalvarPlanoAdquiridoParcelaPagamento(UsuarioFilialPerfil objUsuarioFilialPerfil, Filial objFilial, Plano objPlano, PlanoAdquiridoDetalhes objPlanoAdquiridoDetalhes, Boolean gerarParcela)
        {
            SalvarPlanoAdquiridoParcelaPagamento(objUsuarioFilialPerfil, objFilial, objPlano, objPlanoAdquiridoDetalhes, null, null, null, gerarParcela);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objUsuarioFilialPerfil"></param>
        /// <param name="objFilial"></param>
        /// <param name="objPlano"></param>
        /// <param name="objPlanoAdquiridoDetalhes"></param>
        /// <param name="dataEnvioBoleto"></param>
        /// <param name="dataVencimentoBoleto"></param>
        public void SalvarPlanoAdquiridoParcelaPagamento(UsuarioFilialPerfil objUsuarioFilialPerfil, Filial objFilial, Plano objPlano, PlanoAdquiridoDetalhes objPlanoAdquiridoDetalhes, DateTime? dataEnvioBoleto, DateTime? dataVencimentoBoleto, DateTime? dataEmissaoNFAntecipada, Boolean gerarParcela)
        {
            using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();

                var listaPagamentos = new List<Pagamento>();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        if (objPlano.PlanoTipo.IdPlanoTipo.Equals((int)Enumeradores.PlanoTipo.PessoaJuridica))
                        {

                            //Verifica se o plano é plano para liberação futura. 
                            //Utilizado para planos de renovação
                            //Se existe um plano liberado, e a data de início do plano é maior que a data e o plano adquirido não for para plano adicional
                            PlanoAdquirido objPlanoAdquiridoLiberado;
                            if (this.PlanoSituacao.IdPlanoSituacao != (int)Enumeradores.PlanoSituacao.Bloqueado
                                && this.PlanoSituacao.IdPlanoSituacao != (int)Enumeradores.PlanoSituacao.Cancelado
                                && this.PlanoSituacao.IdPlanoSituacao != (int)Enumeradores.PlanoSituacao.Encerrado
                                && this.PlanoSituacao.IdPlanoSituacao != (int)Enumeradores.PlanoSituacao.LiberacaAutomatica
                                && CarregarPlanoAdquiridoPorSituacao(objFilial, (int)Enumeradores.PlanoSituacao.Liberado, out objPlanoAdquiridoLiberado)
                                && objPlanoAdquiridoLiberado.IdPlanoAdquirido != this.IdPlanoAdquirido
                                && this.DataInicioPlano > objPlanoAdquiridoLiberado.DataFimPlano
                                && this.Plano != null)
                            {
                                this.PlanoSituacao = new PlanoSituacao((int)Enumeradores.PlanoSituacao.LiberacaoFutura);
                            }

                        }

                        if (objPlano.PlanoTipo.IdPlanoTipo.Equals((int)Enumeradores.PlanoTipo.PessoaFisica))
                        {
                            Curriculo objCurriculo;
                            Curriculo.CarregarPorPessoaFisica(trans, objUsuarioFilialPerfil.PessoaFisica.IdPessoaFisica, out objCurriculo);

                            //Verifica se o plano é plano para liberação futura. 
                            //Utilizado para planos de renovação
                            //Se existe um plano liberado, e a data de início do plano é maior que a data e o plano adquirido não for para plano adicional
                            PlanoAdquirido objPlanoAdquiridoLiberado;
                            if (this.PlanoSituacao.IdPlanoSituacao != (int)Enumeradores.PlanoSituacao.Bloqueado
                                && this.PlanoSituacao.IdPlanoSituacao != (int)Enumeradores.PlanoSituacao.Cancelado
                                && this.PlanoSituacao.IdPlanoSituacao != (int)Enumeradores.PlanoSituacao.Encerrado
                                && CarregarPlanoAdquiridoPorSituacao(objCurriculo, (int)Enumeradores.PlanoSituacao.Liberado, out objPlanoAdquiridoLiberado)
                                && objPlanoAdquiridoLiberado.IdPlanoAdquirido != this.IdPlanoAdquirido
                                && this.DataInicioPlano > objPlanoAdquiridoLiberado.DataFimPlano
                                && this.Plano != null)
                            {
                                this.PlanoSituacao = new PlanoSituacao((int)Enumeradores.PlanoSituacao.LiberacaoFutura);
                            }
                        }

                        //PlanoAdquirido
                        Plano = objPlano;
                        UsuarioFilialPerfil = objUsuarioFilialPerfil;
                        Filial = objFilial;
                        Save(trans);

                        //Cria o Plano Quantidade se for um plano Empresa
                        if (objPlano.PlanoTipo.IdPlanoTipo.Equals((int)Enumeradores.PlanoTipo.PessoaJuridica))
                        {
                            //Plano Quantidade 
                            PlanoQuantidade objPlanoQuantidade;
                            if (!PlanoQuantidade.CarregarPorPlanoAdquirido(this, out objPlanoQuantidade, trans))
                            {
                                objPlanoQuantidade = new PlanoQuantidade
                                {
                                    PlanoAdquirido = this,
                                    QuantidadeSMS = 0, //objPlano.QuantidadeSMS,
                                    QuantidadeVisualizacao = objPlano.QuantidadeVisualizacao,
                                    QuantidadeCampanha = objPlano.QuantidadeCampanha
                                };
                            }
                            objPlanoQuantidade.DataInicioQuantidade = DataInicioPlano;
                            objPlanoQuantidade.DataFimQuantidade = DataFimPlano;
                            objPlanoQuantidade.Save(trans);
                        }

                        //PlanoAdquiridoDetalhes
                        int qtdPlanoAdquiridoParcela = 0;
                        if (objPlanoAdquiridoDetalhes != null)
                        {
                            objPlanoAdquiridoDetalhes.PlanoAdquirido = this;
                            objPlanoAdquiridoDetalhes.Save(trans);
                            if (objPlanoAdquiridoDetalhes.PlanoAdquirido != null && objPlanoAdquiridoDetalhes.PlanoAdquirido.QtdParcela.HasValue)
                                qtdPlanoAdquiridoParcela = objPlanoAdquiridoDetalhes.PlanoAdquirido.QtdParcela.Value;
                        }

                        var pagamentos = new List<Pagamento>();

                        //Ajustando o boleto e parcelas iniciais
                        if (dataEnvioBoleto.HasValue && dataVencimentoBoleto.HasValue && gerarParcela)                        
                            pagamentos = CalcularParcelas(dataVencimentoBoleto.Value, dataEnvioBoleto.Value, dataEmissaoNFAntecipada, new TipoPagamento((int)Enumeradores.TipoPagamento.BoletoBancario), null, trans, qtdPlanoAdquiridoParcela);
                        else //Ajustando parcelas a vencer
                        {
                            var todasParcelas = PlanoParcela.ListaParcelasPorPlanoAdquirido(this, trans);

                            var parcelasEmAberto = new List<PlanoParcela>();
                            foreach (PlanoParcela parcelas in todasParcelas)
                            {
                                if (parcelas.PlanoParcelaSituacao.IdPlanoParcelaSituacao == (int)Enumeradores.PlanoParcelaSituacao.EmAberto)
                                {
                                    if (parcelas.ValorParcela != this.ValorBase)
                                        parcelasEmAberto.Add(parcelas);
                                    else if (parcelas.DataEmissaoNotaAntecipada.HasValue != dataEmissaoNFAntecipada.HasValue)
                                        parcelasEmAberto.Add(parcelas);
                                    else if ((parcelas.DataEmissaoNotaAntecipada.HasValue && dataEmissaoNFAntecipada.HasValue) && (parcelas.DataEmissaoNotaAntecipada.Value != dataEmissaoNFAntecipada.Value))
                                        parcelasEmAberto.Add(parcelas);
                                }
                            }

                            //Recuperar todas as parcelas em aberto que o valor é diferente do valor base e atualizar os pagamento
                            foreach (var objPlanoParcela in parcelasEmAberto)
                            {
                                objPlanoParcela.ValorParcela = this.ValorBase;

                                var pagamentosParcela = Pagamento.CarregaPagamentosPorPlanoParcela(objPlanoParcela.IdPlanoParcela); //Recupera todos os pagamentos relacionados com a parcela
                                var objPagamentoAberto = pagamentosParcela.Select(p => p).FirstOrDefault(p => p.PagamentoSituacao.IdPagamentoSituacao == (int)Enumeradores.PagamentoSituacao.EmAberto && p.FlagInativo == false); //Recupera o primeeiro pagamento em aberto

                                if (objPagamentoAberto != null)
                                {
                                    //Cria um pagamento com o valor atual
                                    var objPagamento = objPlanoParcela.CriarPagamento(objPagamentoAberto.DataEmissao.Value, objPagamentoAberto.DataVencimento.Value, objPagamentoAberto.TipoPagamento, null, trans);


                                    objPagamento.DescricaoIdentificador = objPagamentoAberto.DescricaoIdentificador;
                                    objPagamento.NumeroNotaFiscal = objPagamentoAberto.NumeroNotaFiscal;
                                    objPagamento.UrlNotaFiscal = objPagamentoAberto.UrlNotaFiscal;
                                    objPagamento.CodigoDesconto = objPagamentoAberto.CodigoDesconto;
                                    objPagamento.DesOrdemDeCompra = objPagamentoAberto.DesOrdemDeCompra;

                                    objPagamento.Save(trans);

                                    listaPagamentos.Add(objPagamento);

                                    objPlanoParcela.CancelarOutrosPagamentos(objPagamento, trans);

                                    if (dataEmissaoNFAntecipada.HasValue)
                                    {
                                        objPlanoParcela.DataEmissaoNotaAntecipada = RetornaDataAntecipada(objPagamento, dataEmissaoNFAntecipada);
                                    }

                                }

                                objPlanoParcela.Save(trans);
                            }


                        }

                        trans.Commit();

                        if (listaPagamentos.Count > 0)
                        {
                            BoletoBancario.CriarBoletos(listaPagamentos);
                        }

                        try
                        {
                            if (pagamentos.Count > 0)
                            {
                                //boletos
                                if (dataEnvioBoleto.HasValue)
                                {
                                    if (objPlanoAdquiridoDetalhes != null && !string.IsNullOrWhiteSpace(objPlanoAdquiridoDetalhes.EmailEnvioBoleto))
                                    {
                                        var emailRemetente = string.Empty;
                                        DTO.CartaEmail carta = null;
                                        if (objPlano.IdPlano.Equals(Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.IdfPlanoSinePJ)))
                                            || objPlano.IdPlano.Equals(Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.IdfPlanoSinePF))))
                                        {
                                            carta = CartaEmail.RecuperarCarta(BLL.Enumeradores.CartaEmail.ConteudoBoletoVencimentoSINE);
                                            emailRemetente = Parametro.RecuperaValorParametro(Enumeradores.Parametro.EmailRemetenteSinePlano);
                                        }
                                        else
                                        {
                                            carta = CartaEmail.RecuperarCarta(BLL.Enumeradores.CartaEmail.ConteudoBoletoVencimento);
                                            emailRemetente = Parametro.RecuperaValorParametro(Enumeradores.Parametro.ControleParcelasRemetente);
                                        }

                                            byte[] anexos = BoletoBancario.CriarAnexoParaEnviarPorEmail(BoletoBancario.GerarLayoutBoletoHTML(BoletoBancario.CriarBoletos(pagamentos)), Enumeradores.FormatoBoleto.PDF);
                                        MensagemCS.EnvioDeEmailComValidacao(TipoEnviadorEmail.Fila, carta.Assunto, carta.Conteudo, BLL.Enumeradores.CartaEmail.ConteudoBoletoVencimento, emailRemetente, objPlanoAdquiridoDetalhes.EmailEnvioBoleto, $"Boleto_{objPlano.DescricaoPlano}.pdf", anexos);
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            EL.GerenciadorException.GravarExcecao(ex, "Falha ao gerar e enviar boletos.");
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
        #endregion

        public DateTime RetornaDataAntecipada(Pagamento pagamento, DateTime? dataNotaAntecipada)
        {
            bool controle = true;
            TimeSpan ts;

            if (dataNotaAntecipada.HasValue)
            {
                while (controle)
                {
                    ts = pagamento.DataVencimento.Value.Date - dataNotaAntecipada.Value.Date;
                    if (ts.Days > 30)
                        dataNotaAntecipada = dataNotaAntecipada.Value.Date.AddMonths(1);
                    else
                        controle = false;
                }
            }
            return dataNotaAntecipada.Value.Date;

        }

        #region CarregaUltimoPlanoFilialPorSituacao
        public static bool CarregaUltimoPlanoFilialPorLiberacaoFuturaOuAutomatica(BLL.Filial objFilial, out PlanoAdquirido objPlanoAdquiridoFuturo, SqlTransaction trans = null)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter{ ParameterName = "@Idf_Filial", SqlDbType = SqlDbType.Int, Size = 4, Value = objFilial.IdFilial }
                };

            bool retorno;

            IDataReader dr = trans != null ? DataAccessLayer.ExecuteReader(trans, CommandType.Text, SpselectretornaplanosfilialporliberacaoFuturaOuAutomatica, parms) : DataAccessLayer.ExecuteReader(CommandType.Text, SpselectretornaplanosfilialporliberacaoFuturaOuAutomatica, parms);

            objPlanoAdquiridoFuturo = new PlanoAdquirido();

            if (SetInstance(dr, objPlanoAdquiridoFuturo))
                retorno = true;
            else
            {
                objPlanoAdquiridoFuturo = null;
                retorno = false;
            }

            if (!dr.IsClosed)
                dr.Close();

            dr.Dispose();

            return retorno;
        }
        #endregion

        #region AjustarParcela
        private void AjustarParcelas(PlanoAdquirido objPlanoAdquirido, DateTime dtVencimento, DateTime dtEnvioBoleto, TipoPagamento objTipoPagamento, CodigoDesconto objCodigoDesconto, SqlTransaction trans)
        {
            //Se já não foram criadas parcelas para este tipo de pagamento
            if (!PlanoParcela.ExisteTipoPagamentoParcela(objPlanoAdquirido.IdPlanoAdquirido, objTipoPagamento.IdTipoPagamento))
            {
                PlanoParcela.CancelarParcelasEPagamentosEmAberto(objPlanoAdquirido);

                List<PlanoParcela> listaParcelas = PlanoParcela.RecuperarParcelasPlanoAdquirido(objPlanoAdquirido);

                var i = 0;

                foreach (var objPlanoParcela in listaParcelas)
                {
                    objPlanoParcela.CriarPagamento(dtEnvioBoleto.AddMonths(i), dtVencimento.AddMonths(i), objTipoPagamento, objCodigoDesconto, trans);

                    i++;

                }
            }

        }
        #endregion

        #region CalcularParcelas
        public List<Pagamento> CalcularParcelas(DateTime dtVencimento, DateTime dtEnvioBoleto, DateTime? dtaEmissaoNotaAntecipada, TipoPagamento objTipoPagamento, CodigoDesconto objCodigoDesconto, SqlTransaction trans, int? quantidadeParcelas)
        {
            var pagamentos = new List<Pagamento>();
            Plano objPlano = _plano;

            int parcelas = RetornaQuantidadeParcelas(objPlano.QuantidadeParcela, quantidadeParcelas);

            if (objTipoPagamento.IdTipoPagamento == (int)Enumeradores.TipoPagamento.CartaoCredito && parcelas > 1 )
                parcelas = 1;

            for (int i = 0; i < parcelas; i++)
            {
                if (this.Plano.IdPlano.ToString() == Parametro.RecuperaValorParametro(Enumeradores.Parametro.WebfopagControle50))//PlanoWebForPag 50 gratuito
                    pagamentos.Add(PlanoParcela.CriarNovaParcela(this, 0, dtVencimento.AddMonths(i), dtEnvioBoleto.AddMonths(i), null, objTipoPagamento, objCodigoDesconto, trans, true));
                else if (dtaEmissaoNotaAntecipada.HasValue)
                    pagamentos.Add(PlanoParcela.CriarNovaParcela(this, 0, dtVencimento.AddMonths(i), dtEnvioBoleto.AddMonths(i), dtaEmissaoNotaAntecipada.Value.AddMonths(i), objTipoPagamento, objCodigoDesconto, trans));
                else
                    pagamentos.Add(PlanoParcela.CriarNovaParcela(this, 0, dtVencimento.AddMonths(i), dtEnvioBoleto.AddMonths(i), null, objTipoPagamento, objCodigoDesconto, trans));
            }

            return pagamentos;
        }
        #endregion

        public int RetornaQuantidadeParcelas(int planoQuantidadeParcela, int? planoAdquiridoQuantidadeParcela)
        {
            return planoAdquiridoQuantidadeParcela != null && planoAdquiridoQuantidadeParcela > 0
                ? (int)planoAdquiridoQuantidadeParcela
                : planoQuantidadeParcela;
        }

        #region CancelarPlanoAdquirido
        public bool CancelarPlanoAdquirido(UsuarioFilialPerfil objUsuarioGerador, string nomeProcessoPaiParaSalvarNoCRM, SqlTransaction trans, bool derrubarVIP, int? idCurriculo = null)
        {
            object paramValueCurriculo = DBNull.Value;

            if (derrubarVIP && this.Liberado(trans)) //A SP derruba o FLG_Vip se o Identificador do Currículo for passado.
            {
                if (idCurriculo.HasValue)
                    paramValueCurriculo = idCurriculo;
            }

            var parms = new List<SqlParameter>
                            {
                                new SqlParameter { ParameterName = "@Idf_Plano_Adquirido", SqlDbType = SqlDbType.Int, Size = 4, Value = _idPlanoAdquirido },
                                new SqlParameter { ParameterName = "@Idf_Curriculo", SqlDbType = SqlDbType.Int, Size = 4, Value = paramValueCurriculo }
                            };

            DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, Spcancelarplanoadquirido, parms);

            var descricaoCRM = string.Concat("Plano adquirido ", _idPlanoAdquirido, " cancelado!");
            if (idCurriculo.HasValue)
            {
                if (objUsuarioGerador != null)
                    CurriculoObservacao.SalvarCRM(descricaoCRM, new Curriculo(idCurriculo.Value), objUsuarioGerador, trans);
                else
                    CurriculoObservacao.SalvarCRM(descricaoCRM, new Curriculo(idCurriculo.Value), nomeProcessoPaiParaSalvarNoCRM, trans);
            }
            else
            {
                if (objUsuarioGerador != null)
                    FilialObservacao.SalvarCRM(descricaoCRM, Filial, objUsuarioGerador, trans);
                else
                    FilialObservacao.SalvarCRM(descricaoCRM, Filial, nomeProcessoPaiParaSalvarNoCRM, trans);
            }

            if (Filial != null)
                Task.Factory.StartNew(() => CelularSelecionador.HabilitarDesabilitarUsuarios(Filial));

            return true;
        }
        public bool CancelarPlanoAdquirido(UsuarioFilialPerfil objUsuarioGerador, string nomeProcessoPaiParaSalvarNoCRM, bool derrubaVIP, int? idCurriculo = null)
        {
            using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        CancelarPlanoAdquirido(objUsuarioGerador, nomeProcessoPaiParaSalvarNoCRM, trans, derrubaVIP, idCurriculo);

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

        #region LiberarPlanoAdquirido

        /// <param name="objUsuarioGerador">Utilizado para salvar histórico de liberação de um plano novo</param>
        public bool LiberarPlanoAdquirido(UsuarioFilialPerfil objUsuarioGerador)
        {
            using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();

                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        LiberarPlanoAdquirido(objUsuarioGerador, trans);
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

        /// <param name="objUsuarioGerador">Utilizado para salvar histórico de liberação de um plano novo</param>
        public bool LiberarPlanoAdquirido(UsuarioFilialPerfil objUsuarioGerador, SqlTransaction trans)
        {
            bool planoEstavaLiberado = Liberado(trans);
            if (!Liberar(true, trans))
            {
                return true;
            }

            string emailRemetente = Parametro.LoadObject((int)Enumeradores.Parametro.EmailMensagens, trans).ValorParametro;

            if (Plano.PlanoTipo.IdPlanoTipo.Equals((int)Enumeradores.PlanoTipo.PessoaFisica))
            {
                UsuarioFilialPerfil.CompleteObject(trans);
                UsuarioFilialPerfil.PessoaFisica.CompleteObject(trans);

                //Passa o candidato para VIP
                Curriculo objCurriculo;
                if (Curriculo.CarregarPorPessoaFisica(trans, UsuarioFilialPerfil.PessoaFisica.IdPessoaFisica, out objCurriculo))
                {
                    objCurriculo.FlagVIP = true;
                    //Muda a situação do Currículo
                    objCurriculo.SituacaoCurriculo = new SituacaoCurriculo((int)Enumeradores.SituacaoCurriculo.AguardandoRevisaoVIP);
                    objCurriculo.Save(trans);

                    BufferAtualizacaoCurriculo.Update(objCurriculo);
                }

                //Atualiza o perfil do candidato
                UsuarioFilialPerfil.Perfil = new Perfil((int)Enumeradores.Perfil.AcessoVIP);
                UsuarioFilialPerfil.Save(trans);

                var primeiroNome = Helper.RetornarPrimeiroNome(UsuarioFilialPerfil.PessoaFisica.NomeCompleto);

                if (!String.IsNullOrEmpty(UsuarioFilialPerfil.PessoaFisica.EmailPessoa) && !planoEstavaLiberado) //Só envia mensagem caso o usuário possua e-mail e plano não estava liberado
                {
                    var template = CartaEmail.LoadObject((int)Enumeradores.CartaEmail.ConfirmacaoPagamentoVIP);

                    string assunto = template.DescricaoAssunto.Replace("{Primeiro_Nome}", primeiroNome);
                    string mensagem = template.ValorCartaEmail.Replace("{Nome_Completo}", UsuarioFilialPerfil.PessoaFisica.NomeCompleto);

                    EmailSenderFactory
                        .Create(TipoEnviadorEmail.Fila)
                        .Enviar(null, null, UsuarioFilialPerfil, assunto, mensagem, Enumeradores.CartaEmail.ConfirmacaoPagamentoVIP, emailRemetente, UsuarioFilialPerfil.PessoaFisica.EmailPessoa, trans);

                }

                if (!string.IsNullOrEmpty(UsuarioFilialPerfil.PessoaFisica.NumeroDDDCelular) && !string.IsNullOrEmpty(UsuarioFilialPerfil.PessoaFisica.NumeroCelular) && !planoEstavaLiberado)
                {
                    string sms = CartaSMS.RecuperaValorConteudo(Enumeradores.CartaSMS.BoasVindasVIP).Replace("{Primeiro_Nome}", primeiroNome);

                    if (UsuarioFilialPerfil.PessoaFisica.Sexo.IdSexo == (int)Enumeradores.Sexo.Feminino)
                        sms = sms.Replace("Bem-vindo", "Bem-vinda");

                    MensagemCS.SalvarSMS(null, null, UsuarioFilialPerfil, sms, UsuarioFilialPerfil.PessoaFisica.NumeroDDDCelular, UsuarioFilialPerfil.PessoaFisica.NumeroCelular, trans);
                }
            }
            else if (Plano.PlanoTipo.IdPlanoTipo.Equals((int)Enumeradores.PlanoTipo.PessoaJuridica))
            {
                //Não enviar confirmação para lanhouse
                if (!Filial.PossuiSTCLanhouse())
                {
                    UsuarioFilial objUsuarioFilial;
                    if (UsuarioFilial.CarregarUsuarioFilialPorUsuarioFilialPerfil(UsuarioFilialPerfil.IdUsuarioFilialPerfil, out objUsuarioFilial))
                    {
                        if (!String.IsNullOrEmpty(objUsuarioFilial.EmailComercial)) //Só envia mensagem caso o usuário possua e-mail
                        {
                            //TODO Desnecessário apenas para recuperar a Razão Social
                            Filial.CompleteObject(trans);

                            var objVendedor = Filial.Vendedor();

                            string assunto;
                            string template = CartaEmail.RetornarConteudoBNE(Enumeradores.CartaEmail.ConfirmacaoPagamentoCIA, out assunto);
                            var parametros = new
                            {
                                Nome = Filial.RazaoSocial,
                                DescricaoPlano = Plano.DescricaoPlano,
                                Vendedor = objVendedor != null ? objVendedor.ToMailSignature() : string.Empty
                            };
                            string mensagem = parametros.ToString(template);

                            EmailSenderFactory
                                .Create(TipoEnviadorEmail.Fila)
                                .Enviar(null, null, UsuarioFilialPerfil, assunto, mensagem, Enumeradores.CartaEmail.ConfirmacaoPagamentoCIA, emailRemetente, objUsuarioFilial.EmailComercial, trans);
                        }
                    }
                }
            }

            if (objUsuarioGerador != null)
            {
                var descricaoParametros = new
                {
                    NomePlano = Plano.DescricaoPlano
                };
                const string templateDescricao = "Plano {NomePlano} liberado.";

                if (Plano.PlanoTipo.IdPlanoTipo.Equals((int)Enumeradores.PlanoTipo.PessoaJuridica))
                    FilialObservacao.SalvarCRM(descricaoParametros.ToString(templateDescricao), Filial, objUsuarioGerador, trans);
                if (Plano.PlanoTipo.IdPlanoTipo.Equals((int)Enumeradores.PlanoTipo.PessoaFisica))
                    CurriculoObservacao.SalvarCRM(descricaoParametros.ToString(templateDescricao), new Curriculo(Curriculo.RecuperarIdPorPessoaFisica(UsuarioFilialPerfil.PessoaFisica)), objUsuarioGerador, trans);
            }

            Save(trans);
            return true;
        }
        #endregion

        #region EncerrarPlanoAdquirido
        /// <summary>
        /// Metodo resposável por encerrar o plano adquirido e inativar as parcelas e o plano quantidade desse plano
        /// </summary>
        public void EncerrarPlanoAdquirido()
        {
            using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();

                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        EncerrarPlanoAdquirido(trans);
                        trans.Commit();
                        //encerra planos suas vaga tem que deixar de ser livre.
                        BufferAtualizacaoVagaFilial.UpdateVagaFilial(this.Filial.IdFilial);
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }

        public void EncerrarPlanoAdquirido(SqlTransaction trans)
        {
            //this.Plano.CompleteObject(trans);
            //this.Plano.TipoContrato.CompleteObject(trans);
            //PlanoSituacao = this.Plano.TipoContrato.IdTipoContrato == 0 ? new PlanoSituacao((int)Enumeradores.PlanoSituacao.Bloqueado) :  new PlanoSituacao((int)Enumeradores.PlanoSituacao.Encerrado);

            PlanoQuantidade.EncerrarPlanosQuantidadePorPlanoAdquirido(this, trans);
            PlanoParcela.EncerrarPlanosParcelaPorPlanoAdquirido(this, trans);
            
            Save(trans);
        }
        #endregion

        #region RecuperarDataFimUltimoPlanoAdquiridoEncerrado
        public static DateTime? RecuperarDataFimUltimoPlanoAdquiridoEncerrado(Filial objFilial)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter{ ParameterName = "@Idf_Filial", SqlDbType = SqlDbType.Int, Size = 4, Value = objFilial.IdFilial }
                };

            Object retorno = DataAccessLayer.ExecuteScalar(CommandType.Text, Sprecuperardatafimultimoplanoadquiridoencerrado, parms);

            if (retorno != null)
                return Convert.ToDateTime(retorno);

            return null;
        }
        public static DateTime? RecuperarDataFimUltimoPlanoAdquiridoEncerrado(Filial objFilial, SqlTransaction trans)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter{ ParameterName = "@Idf_Filial", SqlDbType = SqlDbType.Int, Size = 4, Value = objFilial.IdFilial }
                };

            Object retorno = DataAccessLayer.ExecuteScalar(trans, CommandType.Text, Sprecuperardatafimultimoplanoadquiridoencerrado, parms);

            if (retorno != null)
                return Convert.ToDateTime(retorno);

            return null;
        }
        #endregion

        #region ParaPessoaFisica
        /// <summary>
        /// O plano adquirido é para pessoa fisica?
        /// </summary>
        /// <param name="trans">transação SQL Server</param>
        /// <returns>true se foi plano para pessoa fisica, false se nao</returns>
        public bool ParaPessoaFisica(SqlTransaction trans = null)
        {
            if (null == trans)
            {
                if (this.Plano == null)
                {
                    CompleteObject();
                }
                if (this.Plano.PlanoTipo == null || this.Plano.PlanoTipo.IdPlanoTipo <= 0)
                {
                    Plano.CompleteObject();
                }
            }
            else
            {
                if (this.Plano == null)
                {
                    CompleteObject(trans);
                }
                if (this.Plano.PlanoTipo == null || this.Plano.PlanoTipo.IdPlanoTipo <= 0)
                {
                    Plano.CompleteObject(trans);
                }
            }

            return this.Plano.PlanoTipo.IdPlanoTipo.Equals((int)Enumeradores.PlanoTipo.PessoaFisica);
        }
        #endregion

        #region ParaPessoaJuridica
        /// <summary>
        /// O plano adquirido é para pessoa juridica?
        /// </summary>
        /// <param name="trans">transação SQL Server</param>
        /// <returns>true se foi plano para pessoa juridica, false se nao</returns>
        public bool ParaPessoaJuridica(SqlTransaction trans = null)
        {
            if (null == trans)
            {
                //Verificando se o plano adquirido já foi carregado
                if (this.Plano == null)
                {
                    CompleteObject();
                }
                //Verificando se o plano já foi carregado
                if (this.Plano.PlanoTipo == null || this.Plano.PlanoTipo.IdPlanoTipo <= 0)
                {
                    Plano.CompleteObject();
                }
            }
            else
            {
                //Verificando se o plano adquirido já foi carregado
                if (this.Plano == null)
                {
                    CompleteObject(trans);
                }
                //Verificando se o plano já foi carregado
                if (this.Plano.PlanoTipo == null || this.Plano.PlanoTipo.IdPlanoTipo <= 0)
                {
                    Plano.CompleteObject(trans);
                }
            }

            return this.Plano.PlanoTipo.IdPlanoTipo.Equals((int)Enumeradores.PlanoTipo.PessoaJuridica);
        }

        #endregion

        #region Liberar
        /// <summary>
        /// Efeuta a liberação do plano adquirido
        /// </summary>
        /// <param name="ajustarDatas">Ajusta a data do início do plano para que o VIP/CIA não perca dias de utilização</param>
        /// <param name="trans">SQL Transaction</param>
        /// <returns>Se o plano foi liberado, retorna true. Caso o VIP/CIA já tenha um plano liberado, marca o plano para liberação futura, retornando false.</returns>
        public bool Liberar(bool ajustarDatas, SqlTransaction trans = null)
        {

            if (null == trans)
            {
                CompleteObject();
                Plano.CompleteObject();
            }
            else
            {
                CompleteObject(trans);
                Plano.CompleteObject(trans);
            }
            //Pega PlanoSituacao para poder ou não ajustar data de ínicio
            PlanoSituacao objPlanoSituacaoCorrente = this.PlanoSituacao;

            //Verifica se o plano é plano para liberação futura. 
            //Utilizado para planos de renovação
            //Se existe um plano liberado, e a data de início do plano é maior que a data e o plano adquirido não for para plano adicional
            if (this.Plano != null)
            {
                PlanoAdquirido objPlanoAdquiridoLiberado;
                if (this.ParaPessoaFisica(trans))
                {
                    if (this.UsuarioFilialPerfil == null)
                        if (trans != null)
                            this.CompleteObject(trans);
                        else
                            this.CompleteObject();

                    if (this.UsuarioFilialPerfil.PessoaFisica == null)
                        if (trans != null)
                            this.UsuarioFilialPerfil.CompleteObject(trans);
                        else
                            this.UsuarioFilialPerfil.CompleteObject();

                    Curriculo objCurriculo;
                    Curriculo.CarregarPorPessoaFisica(this.UsuarioFilialPerfil.PessoaFisica.IdPessoaFisica, out objCurriculo);

                    if (CarregarPlanoAdquiridoPorSituacao(objCurriculo, (int)Enumeradores.PlanoSituacao.Liberado, out objPlanoAdquiridoLiberado)
                        && objPlanoAdquiridoLiberado.IdPlanoAdquirido != this.IdPlanoAdquirido)
                    {
                        this.PlanoSituacao = new PlanoSituacao((int)Enumeradores.PlanoSituacao.LiberacaoFutura);
                    }
                    else
                    {
                        this.PlanoSituacao = new PlanoSituacao((int)Enumeradores.PlanoSituacao.Liberado);


                        objCurriculo.FlagVIP = true;
                        objCurriculo.SituacaoCurriculo = new SituacaoCurriculo((int)Enumeradores.SituacaoCurriculo.AguardandoRevisaoVIP);
                        objCurriculo.Save(trans);

                        //Atualiza o perfil do candidato
                        this.UsuarioFilialPerfil.Perfil = new Perfil((int)Enumeradores.Perfil.AcessoVIP);
                        this.UsuarioFilialPerfil.Save(trans);
                        CancelarOutrosPlanosAdquiridos(objCurriculo, trans);

                    }
                }
                else
                {
                    if (CarregarPlanoAdquiridoPorSituacao(this.Filial, (int)Enumeradores.PlanoSituacao.Liberado, out objPlanoAdquiridoLiberado)
                        && objPlanoAdquiridoLiberado.IdPlanoAdquirido != this.IdPlanoAdquirido)
                    {
                        this.PlanoSituacao = new PlanoSituacao((int)Enumeradores.PlanoSituacao.LiberacaoFutura);
                    }
                    else if (this.PlanoSituacao.IdPlanoSituacao == (int)Enumeradores.PlanoSituacao.Bloqueado)
                    {
                        this.DesbloquearPlano(null, "Regularização após Pagamento de parcela do PlanoAdquirido: " + this.IdPlanoAdquirido);

                        List<PlanoQuantidade> listPlanoQuantidade = PlanoQuantidade.ListaPlanosQuantidadePorPlanoAdquirido(this, trans);

                        if (listPlanoQuantidade.Any())
                        {
                            listPlanoQuantidade.Last().FlagInativo = false;
                            listPlanoQuantidade.Last().Save(trans);
                        }

                        List<PlanoParcela> listPlanoParcela = PlanoParcela.ListaParcelasPorPlanoAdquirido(this, trans);

                        foreach (PlanoParcela objPlanoParcela in listPlanoParcela)
                        {
                            objPlanoParcela.FlagInativo = false;
                            objPlanoParcela.Save(trans);
                        }

                    }
                    else
                    {
                        this.PlanoSituacao = new PlanoSituacao((int)Enumeradores.PlanoSituacao.Liberado);
                        //Atualizar suas vagas no solr para ficarem livres.
                        BufferAtualizacaoVagaFilial.UpdateVagaFilial(this.Filial.IdFilial);
                    }
                }
            }

            if (this.PlanoSituacao.IdPlanoSituacao == (int)Enumeradores.PlanoSituacao.Liberado)
            {
                //Quando for venda de plano de sms ou publicação imediata, disparar campanha
                if (Plano.EhPlanoParaImpulsionarVaga())
                {
                    int idVaga;
                    string codVaga;
                    if (PlanoAdquiridoDetalhes.RecuperarIdVagaPorPlanoAdquirido(this.IdPlanoAdquirido, out idVaga, out codVaga) && idVaga > 0)
                    {
                        //Ajusta data da vaga
                        if (Plano.IdPlano == Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.VendaPlanoCIA_PublicacaoImediataVaga)))
                        {
                            FilialObservacao.SalvarCRM("Usuário comprou aviso imediato para a vaga " + codVaga, this.Filial, this.UsuarioFilialPerfil);
                            new Vaga(idVaga).AtualizarDataAbertura(DateTime.Now);
                        }

                        var parametros = new ParametroExecucaoCollection
                                {
                                    {"idVaga","Vaga",idVaga.ToString(CultureInfo.InvariantCulture), codVaga ?? string.Empty}
                                };

                        ProcessoAssincrono.IniciarAtividade(AsyncServices.Enumeradores.TipoAtividade.EnvioCandidatoVagaPerfil, parametros);
                    }
                }

                if (objPlanoSituacaoCorrente.IdPlanoSituacao != (int)Enumeradores.PlanoSituacao.Liberado
                        && objPlanoSituacaoCorrente.IdPlanoSituacao == (int)Enumeradores.PlanoSituacao.AguardandoLiberacao)
                    this.DataInicioPlano = DateTime.Now.Date;

                if (ajustarDatas)
                {
                    if (this.DataInicioPlano < DateTime.Now &&
                        this.PlanoSituacao.IdPlanoSituacao.Equals((int)Enumeradores.PlanoSituacao.AguardandoLiberacao))
                        this.DataInicioPlano = DateTime.Now;

                    // Atribui a data de vencimento a quantidade de dias referente a soma da data de inicio e a cadastrada no plano.
                    if (!Plano.FlagRecorrente)
                        this.DataFimPlano = this.DataInicioPlano.AddDays(this.Plano.QuantidadeDiasValidade);

                    if (this.ParaPessoaJuridica() && !Plano.FlagRecorrente)
                    {
                        PlanoQuantidade objPlanoQuantidade = null;
                        PlanoQuantidade.CarregarPorPlanoAdquirido(this, out objPlanoQuantidade, trans);

                        objPlanoQuantidade.DataInicioQuantidade = this.DataInicioPlano;
                        objPlanoQuantidade.DataFimQuantidade = this.DataFimPlano;

                        objPlanoQuantidade.Save(trans);
                    }



                }
            }

            if (null == trans)
                Save();
            else
                Save(trans);

            if (this.ParaPessoaJuridica())
                CelularSelecionador.HabilitarDesabilitarUsuarios(this.Filial);

            if (this.PlanoSituacao.IdPlanoSituacao == (int)Enumeradores.PlanoSituacao.Liberado)
                return true;

            return false;
        }
        #endregion

        #region RecuperaPlanoAdquiridoPelaTransacao
        public static PlanoAdquirido RecuperaPlanoAdquiridoPelaTransacao(Transacao objTransacao)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@IdfTransacao", SqlDbType = SqlDbType.Int, Value = objTransacao.IdTransacao }
                };
            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SpRecuperaPlanoAdquiridoPelaTransacao, parms))
            {
                while (dr.Read())
                {
                    var objPlanoAdquirido = new PlanoAdquirido();
                    if (SetInstanceNotDipose(dr, objPlanoAdquirido))
                        return objPlanoAdquirido;
                }
            }
            return null;
        }
        #endregion

        #region DerrubarPlanoLiberadoPagamentoEmAberto
        public static bool DerrubarPlanoLiberadoPagamentoEmAberto(Transacao objTransacao, SqlTransaction trans)
        {
            try
            {
                var objPlanoAdquirido = RecuperaPlanoAdquiridoPelaTransacao(objTransacao);
                if (objPlanoAdquirido == null)
                    return false;
                else
                {
                    int? idCurriculo = null;

                    if (objPlanoAdquirido.Plano != null)
                    {
                        objPlanoAdquirido.Plano.CompleteObject();
                        if (objPlanoAdquirido.Plano.PlanoTipo.IdPlanoTipo.Equals((int)Enumeradores.PlanoTipo.PessoaFisica))
                        {
                            objPlanoAdquirido.UsuarioFilialPerfil.CompleteObject();

                            int idPessoaFisica = objPlanoAdquirido.UsuarioFilialPerfil.PessoaFisica.IdPessoaFisica;
                            Curriculo objCurriculo;
                            if (Curriculo.CarregarPorPessoaFisica(idPessoaFisica, out objCurriculo))
                            {
                                idCurriculo = objCurriculo.IdCurriculo;
                            }
                        }
                    }
                    return objPlanoAdquirido.CancelarPlanoAdquirido(null, "PlanoAdquirido > DerrubarPlanoLiberadoPagamentoEmAberto", idCurriculo != null && objPlanoAdquirido.Liberado(trans) ? true : false, idCurriculo);
                }
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
            }
            return false;
        }
        #endregion

        #region RecuperarDadosDasFiliaisParaEnvioAvisoSaldoSMS
        /// <summary>
        /// Método que Lista os ids das filiais que: Compraram plano e adicionais e estão Liberados ha mais de 6 dias, Possuem saldo de SMS maior que 0 e
        /// Não receberam Aviso de Saldo de SMS nos últimos 30 dias.
        /// </summary>    
        /// <returns>Retorna um DataTable contendo 3 colunas, com Idf_Filial, Qtd_SMS, e Qtd_SMS_Utilizado</returns>
        public static DataTable RecuperarDadosDasFiliaisParaEnvioAvisoSaldoSMS(SqlTransaction trans)
        {
            DateTime dataAnteriorIgualSeteDias = Convert.ToDateTime(DateTime.Now.ToShortDateString() + " 00:00:00");
            dataAnteriorIgualSeteDias = dataAnteriorIgualSeteDias.AddDays(-7); //volta 7 dias para pegar planos/adicionais que foram cadastrados a pelo menos mais de 7 dias

            var parms = new List<SqlParameter>
                {
                    new SqlParameter {ParameterName = "@DataAnteriorIgualSeteDias", SqlDbType = SqlDbType.DateTime, Size = 8, Value = dataAnteriorIgualSeteDias}
                };

            return DataAccessLayer.ExecuteReaderDs(trans, CommandType.Text, SpRecuperaDadosDasFiliaisParaEnvioAvisoSaldoSMS, parms).Tables[0];
        }
        #endregion

        #region CarregarUltimosPlanosEncerrados
        public static List<PlanoAdquirido> CarregarUltimosPlanosEncerrados(Filial objFilial, int quantidade)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter{ ParameterName = "@Idf_Filial", SqlDbType = SqlDbType.Int, Size = 4, Value = objFilial.IdFilial },
                    new SqlParameter{ ParameterName = "@Top", SqlDbType = SqlDbType.Int, Size = 4, Value = quantidade }
                };

            var lista = new List<PlanoAdquirido>();

            using (var dr = DataAccessLayer.ExecuteReader(CommandType.Text, Sprecuperarultimosplanoadquiridoencerrado, parms))
            {
                while (dr.Read())
                {
                    var objPlanoAdquirido = new PlanoAdquirido();
                    if (SetInstanceNotDipose(dr, objPlanoAdquirido))
                        lista.Add(objPlanoAdquirido);
                }

                if (!dr.IsClosed)
                    dr.Close();

                dr.Dispose();
            }
            return lista;
        }
        #endregion

        #region CancelarAssinaturaPlanoRecorrente
        public static bool CancelarAssinaturaPlanoRecorrente(int idPlanoAdquirido, int IdUsuarioFilialPerfilLogado, List<PlanoMotivoCancelamento> listMotivoCancelamento = null)
        {
            using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        DesabilitaRecorrenciaDePlanoAdquirido(idPlanoAdquirido, trans);
                        Filial objFilial = null;
                        Curriculo objCurriculo = null;
                        if (listMotivoCancelamento != null)
                        {
                            foreach (var motivo in listMotivoCancelamento)
                            {
                                motivo.Save(trans);
                            }
                        }
                        if (EnvioDeEmailInformativoDeCancelamentoDeRecorrencia(idPlanoAdquirido, IdUsuarioFilialPerfilLogado, out objFilial, out objCurriculo, trans))
                        {
                            if (objCurriculo == null)
                                FilialObservacao.SalvarCRM("Plano Recorrente " + idPlanoAdquirido + "Cancelado, via click no site", objFilial, new UsuarioFilialPerfil(IdUsuarioFilialPerfilLogado), trans);
                            else
                                CurriculoObservacao.SalvarCRM("Plano Recorrente " + idPlanoAdquirido + "Cancelado, via click no site", objCurriculo, new UsuarioFilialPerfil(IdUsuarioFilialPerfilLogado), trans);
                            trans.Commit();
                            return true;
                        }
                        else
                        {
                            trans.Rollback();
                            return false;
                        }


                    }
                    catch (SqlException ex)
                    {
                        EL.GerenciadorException.GravarExcecao(ex, "Erro ao cancelar Recorrencia");  
                        trans.Rollback();
                        return false;
                    }
                }

            }


        }

        private static void DesabilitaRecorrenciaDePlanoAdquirido(int idPlanoAdquirido, SqlTransaction trans)
        {
            var parms = new List<SqlParameter>
            {
                new SqlParameter { ParameterName = "@Idf_Plano_Adquirido", SqlDbType = SqlDbType.Int, Size = 4, Value = idPlanoAdquirido }
            };

            DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SpCancelarAssinaturaPlanoRecorrenteCia, parms);
        }
        #endregion

        #region SalvarNovaDataFimPlano
        public static void SalvarNovaDataFimPlano(PlanoAdquirido planoAdquirido, SqlTransaction trans = null)
        {
            PlanoAdquirido objPlanoAdquirido = PlanoAdquirido.LoadObject(planoAdquirido.IdPlanoAdquirido, trans);

            objPlanoAdquirido.QuantidadeParcelasPagas = planoAdquirido.QuantidadeParcelasPagas;
            objPlanoAdquirido.DataFimPlano = RegrasDataFImPlano(objPlanoAdquirido);

            if (trans != null)
                objPlanoAdquirido.Save(trans);
            else
                objPlanoAdquirido.Save();

            if (objPlanoAdquirido.ParaPessoaJuridica())
                PlanoQuantidade.ReiniciarContagemSaldoRecorrencia(objPlanoAdquirido, objPlanoAdquirido.DataFimPlano, trans);

        }
        #endregion

        public static DateTime RegrasDataFImPlano(PlanoAdquirido planoAdquirido)
        {
            var novaData = planoAdquirido.DataFimPlano < DateTime.Today ? planoAdquirido.DataFimPlano.AddMonths(1) : planoAdquirido.DataFimPlano = DateTime.Today.AddMonths(1);
            return novaData;
        }

        #region AjustarDatasDePagamentos
        public void AjustarDatasDePagamentos(DateTime dataVencimentoBoleto, DateTime dataEnvioBoleto)
        {
            using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();

                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        var count = 0;
                        foreach (var objPlanoParcela in PlanoParcela.ListaParcelasPorPlanoAdquirido(this, trans).Where(p => p.PlanoParcelaSituacao.IdPlanoParcelaSituacao == (int)Enumeradores.PlanoParcelaSituacao.EmAberto).OrderBy(o => o.IdPlanoParcela))
                        {

                            var pagamentosParcela = Pagamento.CarregaPagamentosPorPlanoParcela(objPlanoParcela.IdPlanoParcela); //Recupera todos os pagamentos relacionados com a parcela
                            var objPagamentoAberto = pagamentosParcela.Select(p => p).FirstOrDefault(p => p.PagamentoSituacao.IdPagamentoSituacao == (int)Enumeradores.PagamentoSituacao.EmAberto && p.FlagInativo == false); //Recupera o primeeiro pagamento em aberto

                            if (objPagamentoAberto != null)
                            {
                                objPagamentoAberto.DataVencimento = dataVencimentoBoleto.AddMonths(count);
                                objPagamentoAberto.DataEmissao = dataEnvioBoleto.AddMonths(count);
                                objPagamentoAberto.Save(trans);
                                objPlanoParcela.CancelarOutrosPagamentos(objPagamentoAberto, trans);
                                count++;
                            }
                        }
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                    finally
                    {
                        trans.Commit();
                    }
                }
            }
        }
        #endregion

        #region BloquearPlano

        public bool BloquearPlano(UsuarioFilialPerfil objUsuarioFilialPerfil, string motivo)
        {
            using (SqlConnection conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();

                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        if (this.ParaPessoaFisica(trans))
                        {
                            this.UsuarioFilialPerfil.CompleteObject(trans);
                            this.UsuarioFilialPerfil.PessoaFisica.CompleteObject(trans);

                            Curriculo objCurriculo = null;
                            if (Curriculo.CarregarPorPessoaFisica(this.UsuarioFilialPerfil.PessoaFisica.IdPessoaFisica, out objCurriculo))
                            {
                                if (objUsuarioFilialPerfil != null)
                                    CurriculoObservacao.SalvarCRM(motivo, objCurriculo, objUsuarioFilialPerfil, trans);
                                else
                                    CurriculoObservacao.SalvarCRM(motivo, objCurriculo, "Controle Parcela => Cobrança Mensalidade", trans);
                            }
                        }
                        else
                        {
                            this.Filial.CompleteObject(trans);
                            this.UsuarioFilialPerfil.CompleteObject(trans);

                            if (objUsuarioFilialPerfil != null)
                                FilialObservacao.SalvarCRM(motivo, this.Filial, objUsuarioFilialPerfil, trans);
                            else
                                FilialObservacao.SalvarCRM(motivo, this.Filial, "Controle Parcela => Cobrança Mensalidade", trans);
                        }



                        this.PlanoSituacao = new PlanoSituacao((int)Enumeradores.PlanoSituacao.Bloqueado);
                        this.Save(trans);
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

        #region DesbloquearPlano

        public bool DesbloquearPlano(UsuarioFilialPerfil objUsuarioFilialPerfil, string motivo)
        {
            using (SqlConnection conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();

                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        if (this.ParaPessoaFisica(trans))
                        {
                            this.UsuarioFilialPerfil.CompleteObject(trans);
                            this.UsuarioFilialPerfil.PessoaFisica.CompleteObject(trans);

                            Curriculo objCurriculo = null;
                            if (Curriculo.CarregarPorPessoaFisica(this.UsuarioFilialPerfil.PessoaFisica.IdPessoaFisica, out objCurriculo))
                            {
                                if (objUsuarioFilialPerfil != null)
                                    CurriculoObservacao.SalvarCRM(motivo, objCurriculo, objUsuarioFilialPerfil, trans);
                                else
                                    CurriculoObservacao.SalvarCRM(motivo, objCurriculo, "Controle Parcela => Cobrança Mensalidade", trans);
                            }
                        }
                        else
                        {
                            this.Filial.CompleteObject(trans);
                            this.UsuarioFilialPerfil.CompleteObject(trans);

                            if (objUsuarioFilialPerfil != null)
                                FilialObservacao.SalvarCRM(motivo, this.Filial, objUsuarioFilialPerfil, trans);
                            else
                                FilialObservacao.SalvarCRM(motivo, this.Filial, "Controle Parcela => Cobrança Mensalidade", trans);
                        }

                        this.PlanoSituacao = new PlanoSituacao((int)Enumeradores.PlanoSituacao.Liberado);
                        //Liberar Parcelas

                      
                        this.Save(trans);
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

        #region DesbloquearPlano

        public bool PlanoAdquiridoBloqueado()
        {
            return this.PlanoSituacao.IdPlanoSituacao.Equals((int)Enumeradores.PlanoSituacao.Bloqueado);
        }
        #endregion

        #region CancelarOutrosPlanosAdquiridos
        /// <summary>
        /// Cancela planos adquiridos diferentes do informado
        /// </summary>
        /// <param name="objCurriculo"></param>
        /// <param name="trans"></param>
        public void CancelarOutrosPlanosAdquiridos(Curriculo objCurriculo, SqlTransaction trans = null)
        {
            var planosAguardandoLiberacao =
                PlanoAdquirido.CarregarPlanosAdquiridoPorSituacao(objCurriculo, (int)Enumeradores.PlanoSituacao.AguardandoLiberacao, trans)
                    .Where(p => p.IdPlanoAdquirido != this.IdPlanoAdquirido);

            foreach (var objPlanoAdquirido in planosAguardandoLiberacao)
            {
                objPlanoAdquirido.CancelarPlanoAdquirido(null, "PlanoParcela > CancelarOutrosPlanosAdquiridos", trans, false, objCurriculo.IdCurriculo);
            }
            BufferAtualizacaoCurriculo.Update(objCurriculo);
        }
        #endregion

        #region EnvioDeEmailInformativoDeCancelamentoDeRecorrência

        public static bool EnvioDeEmailInformativoDeCancelamentoDeRecorrencia(int PlanoAdquiridoId, int idUsuarioFilialPerfilLogado, out Filial objFilial, out Curriculo objCurriculo, SqlTransaction trans)
        {
            objFilial = null;
            objCurriculo = null;

            var parms = new List<SqlParameter>
            {
                new SqlParameter { ParameterName = "@idf_Plano_Adquirido", SqlDbType = SqlDbType.Int, Size = 4, Value = PlanoAdquiridoId }
            };

            using (IDataReader dr = DataAccessLayer.ExecuteReader(trans, CommandType.Text, CriarEmailCancelamentoPlanoRecorrencia, parms))
            {

                while (dr.Read())
                {
                    try
                    {
                        if (dr["IdFilial"] == DBNull.Value && dr["IdCurriculo"] == DBNull.Value) return false;
                        //ENVIO DE EMAIL
                        string assunto, carta, valoresDestinatarios;
                        var emailRemetente = Parametro.RecuperaValorParametro(Enumeradores.Parametro.EmailMensagens);

                        if (dr["IdCurriculo"] != DBNull.Value)
                            valoresDestinatarios = "financeiro@bne.com.br;rodrigobandini@bne.com.br";
                        else
                            valoresDestinatarios = "financeiro@bne.com.br;adrianogoncalves@bne.com.br;" + dr["EmailVendedor"].ToString();

                        if (!string.IsNullOrWhiteSpace(valoresDestinatarios))
                        {

                            if (dr["IdFilial"] != DBNull.Value)
                            {
                                carta = CartaEmail.RetornarConteudoBNE(Enumeradores.CartaEmail.CancelamentoRecorrenciaCIA, out assunto);
                                objFilial = Filial.LoadObject(Convert.ToInt32(dr["IdFilial"]));
                            }
                            else
                            {
                                carta = CartaEmail.RetornarConteudoBNE(Enumeradores.CartaEmail.CancelamentoRecorrenciaVIP, out assunto);
                                objCurriculo = Curriculo.LoadObject(Convert.ToInt32(dr["IdCurriculo"]));
                            }


                            var parametros = new
                            {
                                NomeEmpresa = dr["Nome"].ToString(),
                                Cidade = dr["Cidade"].ToString(),
                                Vendedor = string.IsNullOrEmpty(dr["Vendedor"].ToString()) ? "Venda feita pelo SITE" : dr["Vendedor"].ToString(),
                                DescricaoPlano = dr["DescricaoPlano"].ToString(),
                                CodigoPlano = dr["CodigoPlano"].ToString(),
                                ValorBase = dr["ValorBase"].ToString(),
                                ValorPago = dr["ValorPago"].ToString(),
                                DataPrimeiroPGTO = dr["DataPrimeiroPGTO"].ToString(),
                                DataFimPlano = dr["DataFimPlano"].ToString(),
                                DataCancelamento = dr["DataCancelamento"].ToString(),
                                FormaPagamento = dr["FormaPagamento"].ToString(),
                                CarteiraCliente = objFilial != null ? objFilial.Vendedor().NomeVendedor : "VIP"
                            };

                            if (!string.IsNullOrWhiteSpace(valoresDestinatarios))
                            {
                                //Quando o vendedor vende abaixo do mínimo para o plano escolhido (Vlr_Plano_Base), é enviado um e-mail para informar o financeiro
                                assunto = string.Concat("Aviso de Cancelamento de Plano Recorrente");
                                //Ajustando conteúdo
                                carta = parametros.ToString(carta);

                                MensagemCS.EnvioDeEmailComValidacao(TipoEnviadorEmail.Fila, assunto, carta, objCurriculo != null ? Enumeradores.CartaEmail.CancelamentoRecorrenciaVIP : Enumeradores.CartaEmail.CancelamentoRecorrenciaCIA, emailRemetente, valoresDestinatarios);
                            }
                            return true;
                        }
                    }
                    catch (Exception ex)
                    {
                        objFilial = null;
                        objCurriculo = null;
                        EL.GerenciadorException.GravarExcecao(ex);
                        return false;
                    }

                }
            }
            return false;
        }
        #endregion

        #region ExistePlanoAdquridoEnviadoOuNaoEnviadoDebitoHSBC
        public static bool ExistePlanoAdquridoEnviadoOuNaoEnviadoDebitoHSBC(Filial objFilial)
        {
            var parms = new List<SqlParameter>(){
                new SqlParameter { ParameterName = "@Idf_Filial", SqlDbType = SqlDbType.Int, Size = 4, Value = objFilial.IdFilial }
            };

            using (var dr = DataAccessLayer.ExecuteReader(CommandType.Text, ExistePlanoAdquridoEnviadoOuNaoEnviadoJuridica, parms))
            {
                while (dr.Read())
                {
                    if (Convert.ToInt32(dr["qtd_trans"]) > 0)
                        return true;
                    else
                        return false;
                }
            }
            return false;
        }

        public static bool ExistePlanoAdquridoEnviadoOuNaoEnviadoDebitoHSBC(PessoaFisica objPessoaFisica)
        {
            var parms = new List<SqlParameter>(){
                new SqlParameter { ParameterName = "@Idf_Pessoa_Fisica", SqlDbType = SqlDbType.Int, Size = 4, Value = objPessoaFisica.IdPessoaFisica }
            };

            using (var dr = DataAccessLayer.ExecuteReader(CommandType.Text, ExistePlanoAdquridoEnviadoOuNaoEnviadoPessoaFisica, parms))
            {
                while (dr.Read())
                {
                    if (Convert.ToInt32(dr["qtd_trans"]) > 0)
                        return true;
                    else
                        return false;
                }
            }
            return false;
        }
        #endregion

        #region ExisteVipPlanoLiberadoVencimentoEmXDias
        public static bool ExisteVipPlanoLiberadoVencimentoEmXDias(PessoaFisica objPessoaFisica, int dias)
        {
            var parms = new List<SqlParameter>(){
                new SqlParameter { ParameterName = "@Idf_Pessoa_Fisica", SqlDbType = SqlDbType.Int, Size = 4, Value = objPessoaFisica.IdPessoaFisica },
                new SqlParameter { ParameterName = "@Dias", SqlDbType = SqlDbType.Int, Size = 4, Value = dias }
            };

            using (var dr = DataAccessLayer.ExecuteReader(CommandType.Text, SPExisteVipPlanoLiberadoVencimentoMenosDeXDias, parms))
            {
                while (dr.Read())
                {
                    if (Convert.ToInt32(dr["qtd_trans"]) > 0)
                        return true;
                    else
                        return false;
                }
            }
            return false;
        }
        #endregion

        #region ExisteVipPlanoLiberado
        public static bool ExisteVipPlanoLiberado(int IdPessoaFisica)
        {
            var parms = new List<SqlParameter>(){
                new SqlParameter { ParameterName = "@Idf_Pessoa_Fisica", SqlDbType = SqlDbType.Int, Size = 4, Value = IdPessoaFisica }
            };

            using (var dr = DataAccessLayer.ExecuteReader(CommandType.Text, SPExistePlanoVipLiberado, parms))
            {
                while (dr.Read())
                {
                    if (Convert.ToInt32(dr["qtd_trans"]) > 0)
                        return true;
                    else
                        return false;
                }
            }
            return false;
        }
        #endregion

        #region CarregaListaPlanoAdquiridosWebForPag50
        public static List<PlanoAdquirido> CarregaListaPlanoAdquiridosWebForPag50(SqlTransaction trans)
        {
            List<PlanoAdquirido> listPlanoAdquirido = new List<PlanoAdquirido>();
            using (var dr = DataAccessLayer.ExecuteReader(trans, CommandType.Text, SP_LISTA_TODOS_PLANOS_WEBFORPAG, null))
            {
                while (dr.Read())
                    listPlanoAdquirido.Add(PlanoAdquirido.LoadObject(Convert.ToInt32(dr["Idf_Plano_Adquirido"])));
            }
            return listPlanoAdquirido;
        }
        #endregion

        #region PlanoEmpresaCobrancaCartao
        public static bool PlanoEmpresaCobrancaCartao(int idPlanoAdquirido)
        {
            var parms = new List<SqlParameter>(){
                new SqlParameter { ParameterName = "@Idf_Plano_Adquirido", SqlDbType = SqlDbType.Int, Size = 4, Value = idPlanoAdquirido}
            };

            using (var dr = DataAccessLayer.ExecuteReader(CommandType.Text, SpQuantidadeParcelasCobrancaCartao, parms))
            {
                while (dr.Read())
                {
                    if (Convert.ToInt32(dr["qtd"]) > 0)
                        return true;
                    else
                        return false;
                }
            }
            return false;
        }
        #endregion

        public static void SalvarDataFimPlanoBoletoRecorrente(int idPlanoAdquirido, DateTime data, SqlTransaction trans = null)
        {
            PlanoAdquirido objPlanoAdquirido = PlanoAdquirido.LoadObject(idPlanoAdquirido, trans);

            objPlanoAdquirido.DataFimPlano = data;

            if (trans != null)
                objPlanoAdquirido.Save(trans);
            else
                objPlanoAdquirido.Save();

            if (objPlanoAdquirido.ParaPessoaJuridica())
                PlanoQuantidade.ReiniciarContagemSaldoRecorrencia(objPlanoAdquirido, objPlanoAdquirido.DataFimPlano, trans);

        }

        #endregion

        #region [ExtratoVIP]
        /// <summary>
        /// Retorna valores dos ultimos 30 dias e deste quando se tornou vip (data inicio do plano).
        /// </summary>
        /// <param name="idf_Curriculo"></param>
        /// <param name="Dta_Cadastro_VIP"></param>
        /// <returns></returns>
        public static ExtratoVIP ExtratovipDias(int idf_Curriculo, int dias)
        {
            String cacheKey = $"ExtratoVip:{idf_Curriculo}-{dias}";

            if (MemoryCache.Default[cacheKey] != null)
                return (ExtratoVIP)MemoryCache.Default.Get(cacheKey);

            ExtratoVIP objExtrato = new ExtratoVIP();
            var parms = new List<SqlParameter>()
            {
                new SqlParameter {ParameterName = "@Idf_Curriculo", SqlDbType = SqlDbType.Int, Value = idf_Curriculo  },
                new SqlParameter { ParameterName = "@Qtd_Dias", SqlDbType = SqlDbType.Int, Value = dias }
            };
            using (var dr = DataAccessLayer.ExecuteReader(CommandType.StoredProcedure, "BNE.SP_Metricas_CV_Plano ", parms))
            {
                if (dr.Read())
                {
                    objExtrato.VagasNoPerfil = Convert.ToInt32(dr["QtdVagasPerfil"]);
                    if (objExtrato.VagasNoPerfil <= 0)
                        objExtrato.VagasNoPerfil = Convert.ToInt32(dr["QtdVagasNaCidadeERegiao"]);
                    objExtrato.VagasPeloJornal = Convert.ToInt32(dr["QtdVagasRecebidasJornal"]);
                    objExtrato.EmpresaBuscouSeuPerfil = Convert.ToInt32(dr["QtdEmpresasPesquisaramnoPerfil"]);
                    objExtrato.VisualizacoesCurriculo = Convert.ToInt32(dr["QtdQuemMeViu"]);
                    objExtrato.ApareceuNasBuscas = Convert.ToInt32(dr["QtdVezesApareciNabusca"]);
                    objExtrato.VagasCandidatadas = Convert.ToInt32(dr["QtdCandidaturas"]);
                    objExtrato.VagasVisualizadas = Convert.ToInt32(dr["QtdVagasVisualizadas"]);
                    objExtrato.VagasNaoVisualizadas = Convert.ToInt32(dr["QtdVagasNaoVisualizada"]);
                    objExtrato.BuscaSeuPerfil = Convert.ToInt32(dr["QtdBuscaPerfil"]);
                }
            }

            CacheItemPolicy policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = DateTime.Now.AddMinutes(Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.MinutosLimpezaCache)));
            MemoryCache.Default.Add(cacheKey, objExtrato, policy);

            return objExtrato;
        }
        #endregion



    }
}
