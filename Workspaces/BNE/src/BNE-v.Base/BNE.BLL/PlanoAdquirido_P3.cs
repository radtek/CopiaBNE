using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;
using System.Linq;
using BNE.BLL.Custom;
using BNE.BLL.Custom.Email;
using BNE.Services.Base.ProcessosAssincronos;
using BNE.BLL.AsyncServices;

namespace BNE.BLL
{
    public partial class PlanoAdquirido
    {

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

        #region Spselectexisteplanoadquirido
        private const string Spselectexisteplanoadquirido = @"
        SELECT  PA.Idf_Plano_Adquirido
        FROM    BNE_Plano_Adquirido PA WITH(NOLOCK)
                INNER JOIN TAB_Usuario_Filial_Perfil UFP WITH(NOLOCK) ON PA.Idf_Usuario_Filial_Perfil = UFP.Idf_Usuario_Filial_Perfil
                INNER JOIN BNE_Plano_Parcela PP WITH(NOLOCK) ON PP.Idf_Plano_Adquirido = PA.Idf_Plano_Adquirido
                LEFT JOIN BNE_Pagamento PG WITH(NOLOCK) ON PG.idf_Plano_Parcela = PP.Idf_Plano_Parcela AND PG.Idf_Pagamento_Situacao IN (2, 1)
        WHERE   UFP.Idf_Usuario_Filial_Perfil = @Idf_Usuario_Filial_Perfil
                AND PA.Idf_Plano_Situacao IN (0, 1)";
        #endregion

        #region Spselectretornaplanosfilialporsituacao
        private const string Spselectretornaplanosfilialporsituacao = @"
        SELECT  PA.*
        FROM    BNE_Plano_Adquirido PA WITH(NOLOCK)
        WHERE   PA.Idf_Plano_Situacao = @Idf_Plano_Situacao
                AND PA.Idf_Filial = @Idf_Filial
        ";
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
                        WHERE   1=1 AND PA.Idf_Plano_Situacao IN (0,1)'

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
                AND PA.Idf_Plano_Situacao = @Idf_Plano_Situacao ";
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
            WHERE   PA.Idf_Filial = ' + CONVERT(VARCHAR, @Idf_Filial)

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
            WHERE   PA.Idf_Filial IS NULL
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
                AND Idf_Filial = @Idf_Filial";
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

            var parms = new List<SqlParameter>
                {
                    new SqlParameter{ ParameterName = "@Idf_Usuario_Filial_Perfil", SqlDbType = SqlDbType.Int, Size = 4, Value = paramValueUsuarioFilialPerfil },
                    new SqlParameter{ ParameterName = "@Idf_Filial", SqlDbType = SqlDbType.Int, Size = 4, Value = paramValueFilial },
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

        #region ExistePlanoAdquirido
        public static bool ExistePlanoAdquirido(int idfUsuarioFilialPerfil)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter{ ParameterName = "@Idf_Usuario_Filial_Perfil", SqlDbType = SqlDbType.Int, Size = 4, Value = idfUsuarioFilialPerfil },
                };

            IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spselectexisteplanoadquirido, parms);

            if (dr.Read())
                return true;

            return false;
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
        public static PlanoAdquirido CriarPlanoAdquiridoPJ(UsuarioFilialPerfil objUsuarioFilialPerfil, Filial objFilial, UsuarioFilial objUsuarioFilial, Plano objPlano, int quantidadePrazoBoleto)
        {
            return CriarPlanoAdquiridoParcelaPagamento(objUsuarioFilialPerfil, objFilial, objUsuarioFilial, objPlano, quantidadePrazoBoleto);
        }
        /// <summary>
        /// Cria um novo planoAdquiro, PlanoParcela, Pagamento e PLanoQuantidade em caso de PJ.
        /// </summary>
        /// <param name="objUsuarioFilialPerfil"></param>
        /// <param name="objFilial"></param>
        /// <param name="objPlano"></param>
        /// <returns></returns>
        /// 
        private static PlanoAdquirido CriarPlanoAdquiridoParcelaPagamento(UsuarioFilialPerfil objUsuarioFilialPerfil, Filial objFilial, UsuarioFilial objUsuarioFilial, Plano objPlano, int? quantidadePrazoBoleto)
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
                            FlagBoletoRegistrado = objPlano.FlagBoletoRegistrado
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
                                    QuantidadeVisualizacaoUtilizado = 0
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
                                FilialGestora = new Filial(Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.FilialGestoraPadraoDoPlano)))
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
        public void CriarParcelas(TipoPagamento objTipoPagamento, CodigoDesconto objCodigoDesconto, int? prazoBoleto)
        {
            using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.CalcularParcelas(DateTime.Now.AddDays(prazoBoleto ?? 0), DateTime.Now, objTipoPagamento, objCodigoDesconto, trans);

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
        public void SalvarPlanoAdquiridoParcelaPagamento(UsuarioFilialPerfil objUsuarioFilialPerfil, Filial objFilial, Plano objPlano, PlanoAdquiridoDetalhes objPlanoAdquiridoDetalhes)
        {
            SalvarPlanoAdquiridoParcelaPagamento(objUsuarioFilialPerfil, objFilial, objPlano, objPlanoAdquiridoDetalhes, null, null);
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
        public void SalvarPlanoAdquiridoParcelaPagamento(UsuarioFilialPerfil objUsuarioFilialPerfil, Filial objFilial, Plano objPlano, PlanoAdquiridoDetalhes objPlanoAdquiridoDetalhes, DateTime? dataEnvioBoleto, DateTime? dataVencimentoBoleto)
        {
            using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();

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
                            if (!PlanoQuantidade.CarregarPorPlano(IdPlanoAdquirido, out objPlanoQuantidade, trans))
                            {
                                objPlanoQuantidade = new PlanoQuantidade
                                    {
                                        PlanoAdquirido = this,
                                        QuantidadeSMS = 0, //objPlano.QuantidadeSMS,
                                        QuantidadeVisualizacao = objPlano.QuantidadeVisualizacao
                                    };
                            }
                            objPlanoQuantidade.DataInicioQuantidade = DataInicioPlano;
                            objPlanoQuantidade.DataFimQuantidade = DataFimPlano;
                            objPlanoQuantidade.Save(trans);
                        }

                        //PlanoAdquiridoDetalhes
                        if (objPlanoAdquiridoDetalhes != null)
                        {
                            objPlanoAdquiridoDetalhes.PlanoAdquirido = this;
                            objPlanoAdquiridoDetalhes.Save(trans);
                        }

                        var pagamentos = new List<Pagamento>();
                        //Ajustando o boleto e parcelas iniciais
                        if (dataEnvioBoleto.HasValue && dataVencimentoBoleto.HasValue)
                        {
                            pagamentos = CalcularParcelas(dataVencimentoBoleto.Value, dataEnvioBoleto.Value, new TipoPagamento((int)Enumeradores.TipoPagamento.BoletoBancario), null, trans);
                        }
                        else //Ajustando parcelas a vencer
                        {
                            var todasParcelas = PlanoParcela.ListaParcelasPorPlanoAdquirido(this, trans);

                            //Recuperar todas as parcelas em aberto que o valor é diferente do valor base e atualizar os pagamento
                            var parcelasEmAberto = todasParcelas.Where(p => p.PlanoParcelaSituacao.IdPlanoParcelaSituacao == (int)Enumeradores.PlanoParcelaSituacao.EmAberto && p.ValorParcela != this.ValorBase);
                            foreach (var objPlanoParcela in parcelasEmAberto)
                            {
                                objPlanoParcela.ValorParcela = this.ValorBase;
                                objPlanoParcela.Save(trans);

                                var pagamentosParcela = Pagamento.CarregaPagamentosPorPlanoParcela(objPlanoParcela.IdPlanoParcela); //Recupera todos os pagamentos relacionados com a parcela
                                var objPagamentoAberto = pagamentosParcela.Select(p => p).FirstOrDefault(p => p.PagamentoSituacao.IdPagamentoSituacao == (int)Enumeradores.PagamentoSituacao.EmAberto && p.FlagInativo == false); //Recupera o primeeiro pagamento em aberto

                                if (objPagamentoAberto != null)
                                {
                                    //Cria um pagamento com o valor atual
                                    var objPagamento = objPlanoParcela.CriarPagamento(objPagamentoAberto.DataEmissao.Value, objPagamentoAberto.DataVencimento.Value, objPagamentoAberto.TipoPagamento, null, trans);

                                    objPlanoParcela.CancelarOutrosPagamentos(objPagamento, trans);
                                }
                            }
                        }

                        trans.Commit();

                        try
                        {
                            if (pagamentos.Count > 0)
                            {
                                //boletos
                                byte[] pdfArray = null;
                                string pdfURL;
                                var retorno = pagamentos.Count > 1 ? CobrancaBoleto.GerarBoleto(pagamentos, out pdfArray, out pdfURL) : CobrancaBoleto.GerarBoleto(pagamentos.First());

                                if (pagamentos.Count > 1 && pdfArray != null)
                                {
                                    if (objPlanoAdquiridoDetalhes != null && !string.IsNullOrWhiteSpace(objPlanoAdquiridoDetalhes.EmailEnvioBoleto))
                                    {
                                        var emailRemetente = Parametro.RecuperaValorParametro(Enumeradores.Parametro.ControleParcelasRemetente);
                                        var carta = CartaEmail.RecuperarCarta(BLL.Enumeradores.CartaEmail.ConteudoBoletoVencimento);

                                        EmailSenderFactory.Create(TipoEnviadorEmail.Fila).Enviar(carta.Assunto, carta.Conteudo, emailRemetente, objPlanoAdquiridoDetalhes.EmailEnvioBoleto, "boletos.pdf", pdfArray);
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


        #region AjustarParcela
        private void AjustarParcelas(PlanoAdquirido objPlanoAdquirido, DateTime dtVencimento, DateTime dtEnvioBoleto, TipoPagamento objTipoPagamento, CodigoDesconto objCodigoDesconto, SqlTransaction trans)
        {
            //Se já não foram criadas parcelas para este tipo de pagamento
            if (!PlanoParcela.ExisteTipoPagamentoParcela(objPlanoAdquirido.IdPlanoAdquirido, objTipoPagamento.IdTipoPagamento))
            {
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
        public List<Pagamento> CalcularParcelas(DateTime dtVencimento, DateTime dtEnvioBoleto, TipoPagamento objTipoPagamento, CodigoDesconto objCodigoDesconto, SqlTransaction trans)
        {
            var pagamentos = new List<Pagamento>();

            Plano objPlano = _plano;
            for (int i = 0; i < objPlano.QuantidadeParcela; i++)
                pagamentos.Add(PlanoParcela.CriarNovaParcela(this, 0, dtVencimento.AddMonths(i), dtEnvioBoleto.AddMonths(i), objTipoPagamento, objCodigoDesconto, trans));

            return pagamentos;
        }
        #endregion

        #region CancelarPlanoAdquirido
        public bool CancelarPlanoAdquirido(UsuarioFilialPerfil objUsuarioGerador, string nomeProcessoPaiParaSalvarNoCRM, SqlTransaction trans, bool derrubarVIP, int? idCurriculo = null)
        {
            object paramValueCurriculo = DBNull.Value;

            if (derrubarVIP) //A SP derruba o FLG_Vip se o Identificador do Currículo for passado.
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
                }

                //Atualiza o perfil do candidato
                UsuarioFilialPerfil.Perfil = new Perfil((int)Enumeradores.Perfil.AcessoVIP);
                UsuarioFilialPerfil.Save(trans);

                if (!String.IsNullOrEmpty(UsuarioFilialPerfil.PessoaFisica.EmailPessoa)) //Só envia mensagem caso o usuário possua e-mail
                {
                    #region CartilhaVIP
                    string assuntoCartilhaVIP;
                    string mensagemCartilhaVIP = CartaEmail.RetornarConteudoBNE(Enumeradores.CartaEmail.CartilhaVIP, out assuntoCartilhaVIP);

                    EmailSenderFactory
                        .Create(TipoEnviadorEmail.Fila)
                        .Enviar(null, null, UsuarioFilialPerfil, assuntoCartilhaVIP, mensagemCartilhaVIP, emailRemetente, UsuarioFilialPerfil.PessoaFisica.EmailPessoa, trans);
                    #endregion
                }

                if (!string.IsNullOrEmpty(UsuarioFilialPerfil.PessoaFisica.NumeroDDDCelular) && !string.IsNullOrEmpty(UsuarioFilialPerfil.PessoaFisica.NumeroCelular))
                    MensagemCS.SalvarSMS(null, null, UsuarioFilialPerfil, CartaSMS.RecuperaValorConteudo(Enumeradores.CartaSMS.BoasVindasVIP), UsuarioFilialPerfil.PessoaFisica.NumeroDDDCelular, UsuarioFilialPerfil.PessoaFisica.NumeroCelular, trans);
            }
            else if (Plano.PlanoTipo.IdPlanoTipo.Equals((int)Enumeradores.PlanoTipo.PessoaJuridica))
            {
                UsuarioFilial objUsuarioFilial;
                if (UsuarioFilial.CarregarUsuarioFilialPorUsuarioFilialPerfil(UsuarioFilialPerfil.IdUsuarioFilialPerfil, out objUsuarioFilial))
                {
                    if (!String.IsNullOrEmpty(objUsuarioFilial.EmailComercial)) //Só envia mensagem caso o usuário possua e-mail
                    {
                        //TODO Desnecessário apenas para recuperar a Razão Social
                        Filial.CompleteObject(trans);

                        string assunto;
                        string template = CartaEmail.RetornarConteudoBNE(Enumeradores.CartaEmail.ConfirmacaoPagamentoCIA, out assunto);
                        var parametros = new
                        {
                            Nome = Filial.RazaoSocial,
                            DescricaoPlano = Plano.DescricaoPlano
                        };
                        string mensagem = parametros.ToString(template);

                        EmailSenderFactory
                            .Create(TipoEnviadorEmail.Fila)
                            .Enviar(null, null, UsuarioFilialPerfil, assunto, mensagem, emailRemetente, objUsuarioFilial.EmailComercial, trans);
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
            PlanoSituacao = new PlanoSituacao((int)Enumeradores.PlanoSituacao.Encerrado);

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
                    }
                }
                else
                {
                    if (CarregarPlanoAdquiridoPorSituacao(this.Filial, (int)Enumeradores.PlanoSituacao.Liberado, out objPlanoAdquiridoLiberado)
                        && objPlanoAdquiridoLiberado.IdPlanoAdquirido != this.IdPlanoAdquirido)
                    {
                        this.PlanoSituacao = new PlanoSituacao((int)Enumeradores.PlanoSituacao.LiberacaoFutura);
                    }
                    else
                    {
                        this.PlanoSituacao = new PlanoSituacao((int)Enumeradores.PlanoSituacao.Liberado);
                    }
                }
            }

            if (this.PlanoSituacao.IdPlanoSituacao == (int)Enumeradores.PlanoSituacao.Liberado)
            {
                if (Plano.IdPlano == Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.VendaPlanoCIA_PlanoSmsEmailVagaIdentificador)))
                {
                    int idVaga;
                    string codVaga;
                    if (PlanoAdquiridoDetalhes.RecuperarIdVagaPorPlanoAdquirido(this.IdPlanoAdquirido, out idVaga, out codVaga) && idVaga > 0)
                    {
                        var parametros = new ParametroExecucaoCollection 
                                {
                                    {"idVaga","Vaga",idVaga.ToString(CultureInfo.InvariantCulture), codVaga ?? string.Empty}
                                };

                        ProcessoAssincrono.IniciarAtividade(
                            BNE.BLL.AsyncServices.Enumeradores.TipoAtividade.EnvioCandidatoVagaPerfil,
                            PluginsCompatibilidade.CarregarPorMetadata("EnvioCandidatoVagaPerfil", "PluginSaidaEmailSMSTanque"),
                            parametros,
                            null,
                            null,
                            null,
                            null,
                            DateTime.Now);
                    }
                }

                if (ajustarDatas)
                {
                    if (this.DataInicioPlano < DateTime.Now &&
                        this.PlanoSituacao.IdPlanoSituacao.Equals((int)Enumeradores.PlanoSituacao.AguardandoLiberacao))
                        this.DataInicioPlano = DateTime.Now;

                    // Atribui a data de vencimento a quantidade de dias referente a soma da data de inicio e a cadastrada no plano.
                    this.DataFimPlano = this.DataInicioPlano.AddDays(this.Plano.QuantidadeDiasValidade);
                }
            }

            if (null == trans)
                Save();
            else
                Save(trans);

            if (this.PlanoSituacao.IdPlanoSituacao == (int)Enumeradores.PlanoSituacao.Liberado)
                return true;

            return false;
        }
        #endregion

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

    }
}
