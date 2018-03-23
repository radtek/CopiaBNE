
//-- Data: 08/04/2010 11:17
//-- Autor: Gieyson Stelmak

using BNE.EL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using BNE.BLL.Custom;
using BNE.BLL.Custom.Solr.Buffer;
using FormatObject = BNE.BLL.Common.FormatObject;

namespace BNE.BLL
{
    public partial class PlanoAdquirido // Tabela: BNE_Plano_Adquirido
    {

        #region Atributos
        internal PlanoQuantidade PlanoQuantidade { get; private set; }
        private bool _planoLiberadoNaoPago = false;
        private int _quantidadeParcelasPagas;

        #endregion

        #region Consultas

        #region SpVerificaPlanoRenovacao
        private const string SpVerificaPlanoRenovacao = @"
        select count(pa.Idf_Plano_Adquirido)
        from bne.BNE_Plano_Adquirido pa with(nolock) 
        where pa.Idf_Usuario_Filial_Perfil = @Idf_Usuario_Filial_Perfil
	        and pa.Idf_Plano_Situacao = 1
        ";
        #endregion

        #region Spretornaplanosvencidospf
        private const string Spretornaplanosvencidospf = @"
        SELECT  PA.Idf_Plano_Adquirido ,
                PA.Idf_Usuario_Filial_Perfil ,
                C.Idf_Curriculo ,
                UFP.Idf_Perfil
        FROM    BNE_Curriculo C WITH ( NOLOCK )
                JOIN TAB_Pessoa_Fisica PF WITH ( NOLOCK ) ON C.Idf_Pessoa_Fisica = PF.Idf_Pessoa_Fisica
                JOIN TAB_Usuario_Filial_Perfil UFP WITH ( NOLOCK ) ON PF.Idf_Pessoa_Fisica = UFP.Idf_Pessoa_Fisica
                JOIN BNE_Plano_Adquirido PA WITH ( NOLOCK ) ON UFP.Idf_Usuario_Filial_Perfil = PA.Idf_Usuario_Filial_Perfil
                JOIN TAB_Perfil P WITH ( NOLOCK ) ON P.Idf_Perfil = UFP.Idf_Perfil
        WHERE   PA.Idf_Filial IS NULL
                AND P.Idf_Tipo_Perfil = 1
                AND ( ( Flg_Recorrente = 0
                        AND CONVERT(VARCHAR, PA.Dta_Fim_Plano, 112) < CONVERT(VARCHAR, GETDATE(), 112)
                        AND PA.Idf_Plano_Situacao = 1 --Plano Liberado
                      )
                      OR ( Flg_Recorrente = 1 -- Recorrente
                           AND CONVERT(VARCHAR, PA.Dta_Fim_Plano, 112) < CONVERT(VARCHAR, DATEADD(DAY,
                                                                      -14, GETDATE()), 112) -- cancela após a 3 cobrança no 7 dia no caso
                           AND PA.Idf_Plano_Situacao IN ( 1, 4 ) -- Planos Liberados ou bloqueados
                         )
                    )";
        #endregion

        #region Spretornaplanosvencidospj
        private const string Spretornaplanosvencidospj = @"
        SELECT  PA.*
        FROM    BNE_Plano_Adquirido PA WITH ( NOLOCK )
        WHERE   PA.Idf_Plano_Situacao = 1 /* Liberado */
		        AND PA.Idf_Filial IS NOT NULL
                AND ( ( Flg_Recorrente = 0
                        AND CONVERT(VARCHAR, PA.Dta_Fim_Plano, 112) < CONVERT(VARCHAR, GETDATE(), 112)
                      )
                      OR ( Flg_Recorrente = 1
                           AND CONVERT(VARCHAR, PA.Dta_Fim_Plano, 112) < CONVERT(VARCHAR, DATEADD(DAY,-14,GETDATE()), 112)
                         )
                    )
                ";
        #endregion

        #region
        private const string SpRetonaPlanoVencidoComBoleto = @"
           /*Boleto*/
SELECT 
	pg.Idf_Tipo_Pagamento,
    transacoesCobradas.totalCobrancas,
    pg.Idf_Pagamento_Situacao,
    pa.Idf_Plano_Adquirido,
    pa.Dta_Fim_Plano,
    pg.Idf_Tipo_Pagamento,
    pa.Idf_Filial,
    pa.Qtd_Parcela AS 'QtdeParcPlanoAdq',
    p.Qtd_Parcela AS 'QtdeParcPlano', 
	p.Flg_RecorrenteAposTermPagamento,
	p.Flg_Recorrente	
FROM BNE.BNE_Plano_Adquirido pa
    INNER JOIN BNE.BNE_Plano_Parcela pp
        ON pp.Idf_Plano_Adquirido = pa.Idf_Plano_Adquirido
    INNER JOIN BNE.BNE_Pagamento pg
        ON pg.Idf_Plano_Parcela = pp.Idf_Plano_Parcela
    INNER JOIN BNE.BNE_Plano p
        ON p.Idf_Plano = pa.Idf_Plano
    OUTER APPLY
(
    SELECT COUNT(1) totalCobrancas
    FROM BNE.BNE_Plano_Parcela p2
        INNER JOIN BNE.BNE_Pagamento pg2
            ON pg2.Idf_Plano_Parcela = p2.Idf_Plano_Parcela
    WHERE pg2.Idf_Pagamento_Situacao = 2
          AND p2.Idf_Plano_Adquirido = pa.Idf_Plano_Adquirido
) AS transacoesCobradas
WHERE pa.Idf_Filial IS NOT NULL
      AND pg.Idf_Pagamento_Situacao = 1
      AND
      (
          (
              pa.Flg_Recorrente = 1
              AND (CONVERT(VARCHAR, pg.Dta_Vencimento + 14, 102) = CONVERT(VARCHAR, GETDATE(), 102) OR CONVERT(VARCHAR, PA.Dta_Fim_Plano + 14, 102) = CONVERT(VARCHAR, GETDATE(), 102))
          )
          OR
          (
              pa.Flg_Recorrente = 0
              AND (CONVERT(VARCHAR, pg.Dta_Vencimento, 102) = CONVERT(VARCHAR, GETDATE(), 102) OR CONVERT(VARCHAR, PA.Dta_Fim_Plano, 102) = CONVERT(VARCHAR, GETDATE(), 102))
          )
      )
      AND pa.Idf_Plano_Situacao = 1
      AND pp.Idf_Plano_Parcela_Situacao = 1
      AND pa.Idf_Filial IS NOT NULL;

            ";
        #endregion

        #region Spselectexisteplanoadquiridoplanofilial
        private const string Spselectexisteplanoadquiridoplanofilial = @"
        SELECT  COUNT(PA.Idf_Plano_Adquirido)
        FROM    BNE_Plano_Adquirido PA WITH(NOLOCK)
                INNER JOIN TAB_Usuario_Filial_Perfil UFP WITH(NOLOCK) ON PA.Idf_Usuario_Filial_Perfil = UFP.Idf_Usuario_Filial_Perfil
                INNER JOIN BNE_Plano_parcela PP WITH(NOLOCK) ON PP.Idf_Plano_Adquirido = PA.Idf_Plano_Adquirido
                INNER JOIN BNE_Pagamento PG WITH(NOLOCK) ON PG.Idf_Plano_Parcela = PP.Idf_Plano_Parcela AND ( PG.Idf_Pagamento_Situacao = 1 /* Em Aberto */ OR PG.Idf_Pagamento_Situacao = 2 /* Pago */ )
        WHERE   UFP.Idf_Filial = @Idf_Filial 
                AND PA.Idf_Plano = @Idf_Plano
                AND ( PA.Idf_Plano_Situacao = 0 OR PA.Idf_Plano_Situacao = 1 ) /* Aguardando Liberação ou Liberado */ ";
        #endregion

        #region Spselectexisteplanoadicional
        private const string Spselectexisteplanoadicional = @"
        SELECT  COUNT(AP.Idf_Adicional_Plano)
        FROM    BNE_Adicional_Plano AP WITH(NOLOCK)
        WHERE   AP.Idf_Plano_Adquirido = @Idf_Plano_Adquirido
                AND AP.Idf_Adicional_Plano_Situacao = @Idf_Adicional_Plano_Situacao";
        #endregion

        #region SpselectexisteplanoadquiridoPlaaguardandoliberacaofilialplano
        private const string Spexisteplanoadquiridoaguardandoliberacaoporfilialplano = @"
        SELECT  COUNT(PA.Idf_Plano_Adquirido)
        FROM    BNE_Plano_Adquirido PA WITH (NOLOCK)
            cross apply(
				select top 1 pg.Idf_Tipo_Pagamento
				from bne.bne_plano_parcela pp with(nolock)
					join bne.bne_pagamento pg with(nolock) on pp.Idf_Plano_Parcela = pg.Idf_Plano_Parcela
                where pp.Idf_Plano_Adquirido = pa.Idf_Plano_Adquirido
				order by pp.Idf_Plano_Parcela asc
			) as primeira_parcela 
        WHERE   PA.Idf_Filial = @Idf_Filial
                AND PA.Idf_Plano = @Idf_Plano
                AND PA.Idf_plano_situacao = 0";
        #endregion

        #region Spselectexisteplanoadquiridoaguardandoliberacaocandidatoplano
        private const string Spexisteplanoadquiridoaguardandoliberacaoporcandidatoplano = @"
        SELECT  COUNT(PA.Idf_Plano_Adquirido)
        FROM    BNE_Plano_Adquirido PA WITH (NOLOCK)
                INNER JOIN TAB_Usuario_Filial_Perfil UFP WITH (NOLOCK) ON PA.Idf_Usuario_Filial_Perfil = UFP.Idf_Usuario_Filial_Perfil
                INNER JOIN BNE_Curriculo C WITH(NOLOCK) ON UFP.Idf_Pessoa_Fisica = C.Idf_Pessoa_Fisica
        WHERE   C.Idf_Curriculo = @Idf_Curriculo
                AND UFP.Idf_Filial IS NULL
                AND PA.Idf_Plano = @Idf_Plano
                AND PA.Idf_plano_situacao = 0";
        #endregion

        #region Spretornaplanosvencidosaguardandoliberacao
        private const string Spretornaplanosvencidosaguardandoliberacao = @"
        SELECT	PA.*
        FROM    BNE_Plano_Adquirido PA WITH(NOLOCK)
		        LEFT JOIN BNE_Plano_Parcela PP WITH(NOLOCK) ON PA.Idf_Plano_Adquirido = PP.Idf_Plano_Adquirido
		        CROSS APPLY ( SELECT TOP 1 * FROM BNE_Pagamento P WITH(NOLOCK) WHERE pp.Idf_Plano_Parcela = P.Idf_Plano_Parcela ORDER BY P.Dta_Vencimento DESC ) AS P
        WHERE   PA.Idf_Plano_Situacao = 0 
                AND DATEADD(DAY, @QuantidadeDias, P.Dta_Vencimento) < CONVERT(VARCHAR(10),GETDATE(),112)
        UNION 

        SELECT	PA.*
        FROM    BNE_Plano_Adquirido PA WITH(NOLOCK)
		        LEFT JOIN BNE_Plano_Parcela PP WITH(NOLOCK) ON PA.Idf_Plano_Adquirido = PP.Idf_Plano_Adquirido
        WHERE   
                PA.Idf_Plano_Situacao = 0 
				AND PP.Idf_Plano_Adquirido IS NULL
		        AND DATEADD(DAY, 30, Dta_Inicio_Plano) < CONVERT(VARCHAR(10),GETDATE(),112) 
        ";
        #endregion

        #region SP_RETORNA_PLANOS_LIBERACAO_FUTURA_PARA_ATUALIZACAO
        private const string SP_RETORNA_PLANOS_LIBERACAO_FUTURA_PARA_ATUALIZACAO = @"
        SELECT parcelas_pagas.count AS num_parcelas_pagas, pa.* FROM BNE.BNE_Plano_Adquirido pa WITH(NOLOCK)
        OUTER APPLY (
				        SELECT COUNT(*) AS count
				        FROM BNE.BNE_Plano_Parcela pp WITH(NOLOCK)
				        WHERE pp.Idf_Plano_Adquirido = pa.Idf_Plano_Adquirido
					        AND pp.Idf_Plano_Parcela_Situacao = 2 --Pago
			        ) AS parcelas_pagas
        OUTER APPLY (
	        SELECT COUNT(*) AS count
	        FROM BNE.BNE_Plano_Adquirido pa2 WITH(NOLOCK)
	        WHERE pa2.Idf_Plano_Adquirido <> pa.Idf_Plano_Adquirido
		        AND (pa2.Idf_Filial = pa.Idf_Filial OR pa2.Idf_Usuario_Filial_Perfil = pa.Idf_Usuario_Filial_Perfil)
		        AND pa2.Idf_Plano_Situacao = 1 --Liberado
        ) AS planos_adquiridos_liberados
        WHERE	pa.Idf_Plano_Situacao = 5 --Liberação Futura
		        AND planos_adquiridos_liberados.count <= 0
		        AND pa.Dta_Inicio_Plano <= GETDATE()";
        #endregion

        #region SP_RETORNA_PLANOS_LIBERACAO_AUTOMATICA_PARA_ATUALIZACAO
        private const string SP_RETORNA_PLANOS_LIBERACAO_AUTOMATICA_PARA_ATUALIZACAO = @"
        SELECT pa.* FROM BNE.BNE_Plano_Adquirido pa WITH(NOLOCK)
        OUTER APPLY (
	        SELECT COUNT(*) AS count
	        FROM BNE.BNE_Plano_Adquirido pa2 WITH(NOLOCK)
	        WHERE pa2.Idf_Plano_Adquirido <> pa.Idf_Plano_Adquirido
		        AND (pa2.Idf_Filial = pa.Idf_Filial OR pa2.Idf_Usuario_Filial_Perfil = pa.Idf_Usuario_Filial_Perfil)
		        AND pa2.Idf_Plano_Situacao = 1 --Liberado
        ) AS planos_adquiridos_liberados
        WHERE	pa.Idf_Plano_Situacao = 6 --Liberação Automatica
		        AND planos_adquiridos_liberados.count <= 0
		        AND pa.Dta_Inicio_Plano <= GETDATE()";
        #endregion

        #region Spselectexisteplanoadquiridoliberadofilial
        private const string Spselectexisteplanoadquiridoliberadofilial = @"
        SELECT  COUNT(PA.Idf_Plano_Adquirido)
        FROM    BNE_Plano_Adquirido PA WITH(NOLOCK)
        WHERE   PA.Idf_Filial = @Idf_Filial
                AND PA.Idf_Plano_Situacao = 1";
        #endregion

        #region SpselectexisteplanoadquiridoIlimitadoliberadofilial
        private const string SpselectexisteplanoadquiridoIlimitadoliberadofilial = @"
        SELECT  COUNT(PA.Idf_Plano_Adquirido)
        FROM    BNE_Plano_Adquirido PA WITH(NOLOCK)
                join bne.bne_plano p with(nolock) on pa.idf_plano = p.idf_plano
        WHERE   PA.Idf_Filial = @Idf_Filial
                And p.flg_Ilimitado = 1
                AND PA.Idf_Plano_Situacao = 1";
        #endregion

        #region SELECT_PLANO_ADQUIRIDO_DE_PAGAMENTO
        private const string SELECT_PLANO_ADQUIRIDO_DE_PAGAMENTO = @"
        --Plano Adquirido Normal
        SELECT pa.* 
        FROM    bne.BNE_Plano_Adquirido pa with(nolock) 
                INNER JOIN BNE.BNE_Plano_Parcela pp with(nolock) ON pa.Idf_Plano_Adquirido = pp.Idf_Plano_Adquirido
                INNER JOIN BNE.BNE_Pagamento pag with(nolock) ON pp.Idf_Plano_Parcela = pag.Idf_Plano_Parcela
        WHERE pag.Idf_Pagamento = @Idf_Pagamento
        UNION
        --Plano Adquirido Adicional
        SELECT pa.* FROM BNE.BNE_Plano_Adquirido pa
	        INNER JOIN BNE.BNE_Adicional_Plano ap with(nolock) ON ap.Idf_Plano_Adquirido = pa.Idf_Plano_Adquirido
	        INNER JOIN BNE.BNE_Pagamento pag with(nolock) ON ap.Idf_Adicional_Plano = pag.Idf_Adicional_Plano
        WHERE pag.Idf_Pagamento = @Idf_Pagamento;";
        #endregion

        #region Spselectexisteplanoadquiridoliberadofilialelegivel1clique
        private const string Spselectexisteplanoadquiridoliberadofilialelegivel1clique = @"
        SELECT  COUNT(PA.Idf_Plano_Adquirido)
        FROM    BNE_Plano_Adquirido PA WITH(NOLOCK)
                INNER JOIN bne.BNE_Plano P WITH(NOLOCK) ON P.Idf_Plano = PA.Idf_Plano AND P.Idf_Plano_Tipo = 2 AND P.Qtd_Parcela > 1 AND P.Flg_Inativo = 0 AND P.Qtd_Visualizacao > 0
        WHERE   PA.Idf_Filial = @Idf_Filial
                AND PA.Idf_Plano_Situacao = 1
                AND P.Flg_Ilimitado = 1";
        #endregion

        #region Sppossuecontratosemaceite
        private const string Sppossuecontratosemaceite = @"
        SELECT  COUNT(*)
        FROM    bne.BNE_Plano_Adquirido PA WITH(NOLOCK)
		        INNER join BNE.BNE_Plano P WITH(NOLOCK) ON P.Idf_Plano = PA.Idf_Plano
                INNER JOIN BNE.TAB_Usuario_Filial_Perfil UFP WITH(NOLOCK) ON UFP.Idf_Filial = PA.Idf_Filial
		        LEFT JOIN BNE.BNE_Plano_Adquirido_Contrato PAC WITH(NOLOCK) ON PAC.Idf_Plano_Adquirido = PA.Idf_Plano_Adquirido
                OUTER APPLY (SELECT Idf_Usuario_Filial_Perfil FROM BNE.BNE_Plano_Adquirido_Contrato_Usuario WHERE Idf_Plano_Adquirido_Contrato = PAC.Idf_Plano_Adquirido_Contrato ) AS Aceite /* Verificando se tem pelo menos um aceite */
        WHERE   Dta_Inicio_Plano > @DataImplantacaoAceiteContrato
                AND pa.Idf_Plano_Situacao = 1
		        AND P.Flg_Enviar_Contrato = 1
		        AND PA.Idf_Filial = @Idf_Filial
				AND (UFP.Idf_Usuario_Filial_Perfil IS NOT NULL AND Aceite.Idf_Usuario_Filial_Perfil IS NULL)";
        #endregion

        #region Sppossuecontratosemaceiteusuario
        private const string Sppossuecontratosemaceiteusuario = @"
        SELECT  COUNT(*)
        FROM    bne.BNE_Plano_Adquirido PA WITH(NOLOCK)
		        INNER join BNE.BNE_Plano P ON P.Idf_Plano = PA.Idf_Plano
                INNER JOIN BNE.TAB_Usuario_Filial_Perfil UFP WITH(NOLOCK) ON UFP.Idf_Filial = PA.Idf_Filial
		        LEFT JOIN BNE.BNE_Plano_Adquirido_Contrato PAC WITH(NOLOCK) ON PAC.Idf_Plano_Adquirido = PA.Idf_Plano_Adquirido
                LEFT JOIN BNE.BNE_Plano_Adquirido_Contrato_Usuario PACU WITH(NOLOCK) ON PACU.Idf_Plano_Adquirido_Contrato = PAC.Idf_Plano_Adquirido_Contrato AND PACU.Idf_Usuario_Filial_Perfil = UFP.Idf_Usuario_Filial_Perfil
        WHERE   Dta_Inicio_Plano > @DataImplantacaoAceiteContrato
                AND pa.Idf_Plano_Situacao = 1
		        AND P.Flg_Enviar_Contrato = 1
                AND UFP.Idf_Usuario_Filial_Perfil = @Idf_Usuario_Filial_Perfil
				AND PACU.Idf_Usuario_Filial_Perfil IS NULL
				AND UFP.Idf_Perfil = 4";
        #endregion

        #region SP_LISTA_TODOS_PLANOS_COM_RECORRENCIA_VENCIDA
        private const string SP_LISTA_TODOS_PLANOS_COM_RECORRENCIA_VENCIDA = @"
          
SELECT  pa.Idf_Plano_Adquirido ,
                    Idf_Tipo_Pagamento ,
                    Dta_Fim_Plano ,
                    pa.Qtd_Parcela ,
                    transacoesCobradas.totalCobrancas,
					pa.Idf_Plano_Situacao
            FROM    BNE.BNE_Plano_Adquirido pa WITH ( NOLOCK )
					INNER JOIN BNE.BNE_Plano p WITH (NOLOCK) ON p.Idf_Plano = pa.Idf_Plano
                    JOIN BNE.BNE_Transacao T WITH ( NOLOCK ) ON T.Idf_Plano_Adquirido = pa.Idf_Plano_Adquirido
                    OUTER APPLY ( SELECT    COUNT(1) totalCobrancas
                                  FROM      BNE.BNE_Transacao tra  WITH(NOLOCK)
                                  WHERE     tra.Idf_Plano_Adquirido = pa.Idf_Plano_Adquirido
                                            AND tra.Idf_Status_Transacao = 3
                                ) AS transacoesCobradas
            WHERE   1=1 AND (pa.Flg_Recorrente = 1 OR p.Flg_RecorrenteAposTermPagamento = 1) --1 => Com Recorrência
                    AND ( ( ( CAST(Dta_Fim_Plano AS DATE) = CAST(GETDATE() AS DATE) -- Data fim igual
                              AND Idf_Tipo_Pagamento = 8 -- Debito HSBC
                            ) -- Pegar somente o dia para evitar envio de duplicidade
                            AND pa.Idf_Plano_Situacao = 1 -- 1 => Liberado
                          )
                          OR ( DATEDIFF(DAY, Dta_Fim_Plano, GETDATE()) IN ( 0,7,14 ) -- Que esteja com 3
                               AND ( ( Idf_Plano_Situacao = 1 )-- PJ e PF
                                     OR ( Idf_Plano_Situacao = 4
                                          AND Idf_Filial IS NULL
                                        ) -- Pessoa Física
                                   )
                               AND Idf_Tipo_Pagamento = 1
							   AND pa.Idf_Plano_Situacao = 1 
							   AND pa.Dta_Cancelamento IS null
                             )
                        )
            GROUP BY pa.Idf_Plano_Adquirido ,
                    Idf_Tipo_Pagamento ,
                    Dta_Fim_Plano ,
                    pa.Qtd_Parcela ,
                    transacoesCobradas.totalCobrancas,
					pa.Idf_Plano_Situacao  

				
        ";

        #endregion

        #region SP_EXISTE_PARCELA_ABERTA_NO_PLANO_ADQUIRIDO
        private const string SP_EXISTE_PARCELA_ABERTA_NO_PLANO_ADQUIRIDO = @"
        SELECT  MIN(PP.Idf_Plano_Parcela) as Parcela
        FROM    BNE.BNE_Plano_Adquirido PA WITH ( NOLOCK )
                JOIN BNE.BNE_Plano_Parcela PP WITH ( NOLOCK ) ON PP.Idf_Plano_Adquirido = PA.Idf_Plano_Adquirido
		        JOIN BNE.BNE_Pagamento PG WITH(NOLOCK) ON PG.Idf_Plano_Parcela = PP.Idf_Plano_Parcela
		        JOIN BNE.BNE_Plano P WITH(NOLOCK) ON P.Idf_Plano = PA.Idf_Plano
        WHERE 
				1=1
		        AND PA.Idf_Plano_Adquirido = @Idf_Plano_Adquirido
		        AND P.Idf_Plano_Forma_Pagamento = 4 -- 4 => Mensalidade
                AND PG.Idf_Pagamento_Situacao = 1 -- 1 => Em Aberto
                AND PG.Idf_Tipo_Pagamento = 8 -- 8 => Débito Recorrente
                AND PG.Dta_Vencimento > GETDATE()
                AND PA.Idf_Plano_Situacao = 1 -- 1 => Liberado
                AND PP.Idf_Plano_Parcela_Situacao = 1 -- 1 => aberto
                AND P.Idf_Plano_Tipo = 1  -- 1 => Pessoa Fisica - Definido inicialmente";
        #endregion

        #region Spexisteplanoadiquiridoparavaga
        private const string Spexisteplanoadiquiridoparavaga = @"
        SELECT  PA.*
        FROM    BNE_Plano_Adquirido PA WITH(NOLOCK)
                INNER JOIN BNE_Plano_Adquirido_Detalhes PAD WITH(NOLOCK) ON PA.Idf_Plano_Adquirido = PAD.Idf_Plano_Adquirido
        WHERE   PA.Idf_Plano_Situacao = 2 /* Quando o fluxo está correto o plano está ligado a uma vaga pelo plano detalhes */
                AND PAD.Idf_Vaga = @Idf_Vaga
                AND DATEADD(d, DATEDIFF(d, 0, GETDATE()), 0) BETWEEN  DATEADD(d, DATEDIFF(d, 0, PA.Dta_Inicio_Plano), 0)  AND  DATEADD(d, DATEDIFF(d, 0, PA.Dta_Fim_Plano), 0)";
        #endregion

        #region SP_QTD_PARCELAS_PAGAS_PLANO_ADQUIRIDO
        private const string SP_QTD_PARCELAS_PAGAS_PLANO_ADQUIRIDO = @"
        SELECT  COUNT(DISTINCT Idf_Plano_Parcela) QTD_PAGA
        FROM    BNE.BNE_Plano_Parcela PP  WITH(NOLOCK)
        WHERE   Idf_Plano_Adquirido = @Idf_Plano_Adquirido
		        AND Idf_Plano_Parcela_Situacao = 2 -- Parcela Paga";
        #endregion

        #region spEncerrarCandidaturaPremium
        private const string spEncerrarCandidaturaPremium = @"update bne_plano_adquirido
                    set idf_plano_situacao = @idf_Plano_Situacao where idf_plano_adquirido = @idf_Plano_Adquirido";
        #endregion

        #endregion

        #region Métodos

        #region PlanoLiberadoNaoPago
        public bool PlanoLiberadoNaoPago
        {
            get { return _planoLiberadoNaoPago; }
            set { _planoLiberadoNaoPago = value; }
        }
        #endregion

        public int QuantidadeParcelasPagas
        {
            get { return _quantidadeParcelasPagas; }
            set { _quantidadeParcelasPagas = value; }

        }

        #region CancelarPlanoVip
        public bool CancelarPlanoVip()
        {
            using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        PlanoSituacao = new PlanoSituacao((int)Enumeradores.PlanoSituacao.Cancelado);
                        Save(trans);

                        UsuarioFilialPerfil.CompleteObject(trans);

                        Curriculo objCurriculo;
                        Curriculo.CarregarPorPessoaFisica(trans, UsuarioFilialPerfil.PessoaFisica.IdPessoaFisica, out objCurriculo);
                        objCurriculo.FlagVIP = false;
                        objCurriculo.SituacaoCurriculo = new SituacaoCurriculo((int)Enumeradores.SituacaoCurriculo.Publicado);
                        objCurriculo.Save(trans);

                        //AlertaCurriculos.OnAlterarCurriculo(objCurriculo);

                        //Retorna o usuário para o perfil Não VIP
                        UsuarioFilialPerfil.Perfil = new Perfil((int)Enumeradores.Perfil.AcessoNaoVIP);
                        UsuarioFilialPerfil.Save(trans);

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

        #region ConcederPlanoPF
        /// <summary>
        /// Método responsável por conceder um plano vip a um Candidato 
        /// </summary>
        /// <param name="objCurriculo">Curriculo que receberá o plano</param>
        /// <param name="objPlano">Plano a ser concedido</param>
        /// <returns>Status do processo de concessão de plano</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static bool ConcederPlanoPF(Curriculo objCurriculo, Plano objPlano)
        {
            using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        ConcederPlanoPF(objCurriculo, objPlano, trans);
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
        public static bool ConcederPlanoPF(Curriculo objCurriculo, Plano objPlano, SqlTransaction trans)
        {
            objPlano.CompleteObject(trans);
            objCurriculo.PessoaFisica.CompleteObject(trans);

            #region TAB_Usuario_Filial_Perfil
            UsuarioFilialPerfil objUsuarioFilialPerfil;

            if (!UsuarioFilialPerfil.CarregarUsuarioFilialPerfilCandidatoAtivoEInativo(objCurriculo.PessoaFisica, trans, out objUsuarioFilialPerfil))
            {
                objUsuarioFilialPerfil = new UsuarioFilialPerfil
                {
                    PessoaFisica = objCurriculo.PessoaFisica,
                    SenhaUsuarioFilialPerfil = objCurriculo.PessoaFisica.DataNascimento.ToString("ddMMyyyy"),
                    Perfil = new Perfil((int)Enumeradores.Perfil.AcessoVIP)
                };
                objUsuarioFilialPerfil.Save(trans);
            }
            else
            {
                //Corrige o perfil do Candidato
                objUsuarioFilialPerfil.Perfil = new Perfil((int)Enumeradores.Perfil.AcessoVIP);
                objUsuarioFilialPerfil.Save(trans);
            }
            #endregion

            #region BNE_Plano_Adquirido

            var objPlanoAdquirido = new PlanoAdquirido
            {
                UsuarioFilialPerfil = objUsuarioFilialPerfil,
                Plano = objPlano,
                DataInicioPlano = DateTime.Now,
                DataFimPlano = DateTime.Now.AddDays(objPlano.QuantidadeDiasValidade),
                PlanoSituacao = new PlanoSituacao((int)Enumeradores.PlanoSituacao.Liberado),
                ValorBase = objPlano.ValorBase,
                QuantidadeSMS = objPlano.QuantidadeSMS,
                FlagBoletoRegistrado = objPlano.FlagBoletoRegistrado
            };
            objPlanoAdquirido.Save(trans);

            #endregion

            #region BNE_Plano_Parcela

            var objPlanoParcela = new PlanoParcela
            {
                DataPagamento = DateTime.Now,
                FlagInativo = false,
                PlanoAdquirido = objPlanoAdquirido,
                PlanoParcelaSituacao = new PlanoParcelaSituacao((int)Enumeradores.PlanoParcelaSituacao.Pago),
                ValorParcela = objPlano.ValorBase
            };
            objPlanoParcela.Save(trans);

            #endregion

            #region Curriculo
            //Atualizar a situação do Currículo do Candidato
            objCurriculo.SituacaoCurriculo = new SituacaoCurriculo((int)Enumeradores.SituacaoCurriculo.AguardandoRevisaoVIP);
            objCurriculo.FlagVIP = true;
            objCurriculo.Save(trans);
            #endregion

            BufferAtualizacaoCurriculo.Update(objCurriculo);

            return true;
        }
        #endregion

        #region AtualizarPlanoPF
        /// <summary>
        /// Metodo utilizado pelo robô(service) atualizar plano PF
        /// </summary>
        public static void AtualizarPlanoPF()
        {

            using (var dt = ListaPlanosParaEncerramento())
            {
                foreach (DataRow row in dt.Rows)
                {
                    try
                    {
                        EncerrarPlanoAdquiridoPessoaFisica(new PlanoAdquirido(Convert.ToInt32(row["Idf_Plano_Adquirido"])), new Curriculo(Convert.ToInt32(row["Idf_Curriculo"])), new UsuarioFilialPerfil(Convert.ToInt32(row["Idf_Usuario_Filial_Perfil"])));
                    }
                    catch (Exception ex)
                    {
                        EL.GerenciadorException.GravarExcecao(ex);
                    }
                }
            }
        }
        #endregion

        #region ListaPlanosParaEncerramento
        private static DataTable ListaPlanosParaEncerramento()
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(Spretornaplanosvencidospf, DataAccessLayer.CONN_STRING);
            da.Fill(dt);
            return dt;
        }
        #endregion

        #region AtualizarPlanoPJ
        ///// <summary>
        ///// Metodo utilizado pelo robô(service) atualizar plano PJ
        ///// </summary>
        //public static void AtualizarPlanoPJ()
        //{
        //    try
        //    {
        //        var lista = new List<PlanoAdquirido>();

        //        using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spretornaplanosvencidospj, null))
        //        {
        //            while (dr.Read())
        //            {
        //                var objPlanoAdquirido = new PlanoAdquirido();
        //                if (SetInstanceNotDipose(dr, objPlanoAdquirido))
        //                    lista.Add(objPlanoAdquirido);
        //            }

        //            if (!dr.IsClosed)
        //                dr.Close();
        //        }

        //        foreach (PlanoAdquirido objPlanoAdquirido in lista)
        //        {
        //            try
        //            {
        //                objPlanoAdquirido.EncerrarPlanoAdquirido();
        //            }
        //            catch (Exception ex)
        //            {
        //                EL.GerenciadorException.GravarExcecao(ex);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        EL.GerenciadorException.GravarExcecao(ex);
        //        throw;
        //    }
        //}

        public static void AtualizarPlanoPJ()
        {
            try
            {
                List<PlanoAdquirido> listaPlanosAdquirido = new List<PlanoAdquirido>();
                var planoAdquirido = new PlanoAdquirido();
                using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SpRetonaPlanoVencidoComBoleto, null))
                {
                    List<PlanoAdquirido> planosAdquirido = new List<PlanoAdquirido>();
                    while (dr.Read())
                    {
                        var planoAdquiridoCancelameto = new PlanoAdiquiridoVencidos();
                        planoAdquiridoCancelameto.TipoPagamento = dr["Idf_Tipo_Pagamento"] != DBNull.Value ? Convert.ToInt32(dr["Idf_Tipo_Pagamento"]) : 0;
                        planoAdquiridoCancelameto.TotalCobranca = dr["totalCobrancas"] != DBNull.Value ? Convert.ToInt32(dr["totalCobrancas"]) : 0;
                        planoAdquiridoCancelameto.PagamentoSituacao = dr["Idf_Pagamento_Situacao"] != DBNull.Value ? Convert.ToInt32(dr["Idf_Pagamento_Situacao"]) : 0;
                        planoAdquiridoCancelameto.IdPlanoAdquirido = dr["Idf_Plano_Adquirido"] != DBNull.Value ? Convert.ToInt32(dr["Idf_Plano_Adquirido"]) : 0;
                        planoAdquiridoCancelameto.DataFimPlano = Convert.ToDateTime(dr["Dta_Fim_Plano"]);
                        planoAdquiridoCancelameto.IdFilial = dr["Idf_Filial"] != DBNull.Value ? Convert.ToInt32(dr["Idf_Filial"]) : 0;
                        planoAdquiridoCancelameto.QuantidadeParcelasPlanoAdquirido = dr["QtdeParcPlanoAdq"] != DBNull.Value ? Convert.ToInt32(dr["QtdeParcPlanoAdq"]) : 0;
                        planoAdquiridoCancelameto.QuantidadeParcelasPlano = dr["QtdeParcPlano"] != DBNull.Value ? Convert.ToInt32(dr["QtdeParcPlano"]) : 0;

                       
                        planoAdquirido = PlanoAdquirido.LoadObject(planoAdquiridoCancelameto.IdPlanoAdquirido);
                        int totalParcela = planoAdquiridoCancelameto.QuantidadeParcelasPlanoAdquirido > 0 ? planoAdquiridoCancelameto.QuantidadeParcelasPlanoAdquirido : planoAdquiridoCancelameto.QuantidadeParcelasPlano;
                        
                        if (planoAdquiridoCancelameto.TotalCobranca > 0)
                        {
                            planoAdquirido.PlanoSituacao = planoAdquiridoCancelameto.TotalCobranca >= totalParcela ?  new PlanoSituacao((int)Enumeradores.PlanoSituacao.Encerrado) : new PlanoSituacao((int)Enumeradores.PlanoSituacao.Bloqueado);
                        }
                        else
                        {
                            planoAdquirido.PlanoSituacao = new PlanoSituacao((int)Enumeradores.PlanoSituacao.Encerrado);
                        }
                        planosAdquirido.Add(planoAdquirido);
                    }

                    foreach (var plano in planosAdquirido)
                    {
                        plano.EncerrarPlanoAdquirido();
                    }


                }
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
                throw;
            }
        }

        #endregion

        #region EncerrarPlanoAdquiridoPessoaFisica
        /// <summary>
        /// Metodo resposável por encerrar o plano adquirido de pessoa fisica
        /// </summary>
        public static void EncerrarPlanoAdquiridoPessoaFisica(PlanoAdquirido objPlanoAdquirido, Curriculo objCurriculo, UsuarioFilialPerfil objUsuarioFilialPerfil)
        {
            using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();

                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        EncerrarPlanoAdquiridoPessoaFisica(objPlanoAdquirido, objCurriculo, objUsuarioFilialPerfil, trans);
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }

                    BufferAtualizacaoCurriculo.Update(objCurriculo);
                }
            }
        }


        private static void EncerrarPlanoAdquiridoPessoaFisica(PlanoAdquirido objPlanoAdquirido, Curriculo objCurriculo, UsuarioFilialPerfil objUsuarioFilialPerfil, SqlTransaction trans)
        {
            objPlanoAdquirido.CompleteObject(trans);
            objCurriculo.CompleteObject(trans);
            objUsuarioFilialPerfil.CompleteObject(trans);

            objPlanoAdquirido.PlanoSituacao = new PlanoSituacao((int)Enumeradores.PlanoSituacao.Encerrado);

            PlanoParcela.EncerrarPlanosParcelaPorPlanoAdquirido(objPlanoAdquirido, trans);

            objPlanoAdquirido.Save(trans);

            // Validar se existe algum plano liberado  ** renovacao
            if (ExistePlanoAdquiridoRenovado(objPlanoAdquirido._usuarioFilialPerfil.IdUsuarioFilialPerfil) == 0)
            {
                objCurriculo.FlagVIP = false;

                objCurriculo.SituacaoCurriculo = new SituacaoCurriculo((int)Enumeradores.SituacaoCurriculo.Publicado);
                objCurriculo.Save(trans);

                objUsuarioFilialPerfil.Perfil = new Perfil((int)Enumeradores.Perfil.AcessoNaoVIP);
                objUsuarioFilialPerfil.Save(trans);
            }

            //AlertaCurriculos.OnAlterarCurriculo(objCurriculo);
        }
        #endregion

        #region SetInstanceNotDipose
        /// <summary>
        /// Método auxiliar utilizado para percorrer um IDataReader e vincular as colunas com os atributos da classe sem fechar o datareader
        /// </summary>
        /// <param name="dr">Cursor de leitura do banco de dados.</param>
        /// <param name="objPlanoAdquirido">Instância a ser manipulada.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson</remarks>
        public static bool SetInstanceNotDipose(IDataReader dr, PlanoAdquirido objPlanoAdquirido)
        {
            objPlanoAdquirido._idPlanoAdquirido = Convert.ToInt32(dr["Idf_Plano_Adquirido"]);
            objPlanoAdquirido._plano = new Plano(Convert.ToInt32(dr["Idf_Plano"]));
            objPlanoAdquirido._usuarioFilialPerfil = new UsuarioFilialPerfil(Convert.ToInt32(dr["Idf_Usuario_Filial_Perfil"]));
            objPlanoAdquirido._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
            objPlanoAdquirido._dataInicioPlano = Convert.ToDateTime(dr["Dta_Inicio_Plano"]);
            objPlanoAdquirido._dataFimPlano = Convert.ToDateTime(dr["Dta_Fim_Plano"]);
            objPlanoAdquirido._planoSituacao = new PlanoSituacao(Convert.ToInt32(dr["Idf_Plano_Situacao"]));
            if (dr["Idf_Filial"] != DBNull.Value)
                objPlanoAdquirido._filial = new Filial(Convert.ToInt32(dr["Idf_Filial"]));
            objPlanoAdquirido._quantidadeSMS = Convert.ToInt32(dr["Qtd_SMS"]);
            objPlanoAdquirido._valorBase = Convert.ToDecimal(dr["Vlr_Base"]);
            if (dr["Qtd_Prazo_Boleto"] != DBNull.Value)
                objPlanoAdquirido._quantidadePrazoBoleto = Convert.ToInt32(dr["Qtd_Prazo_Boleto"]);
            objPlanoAdquirido._flagBoletoRegistrado = Convert.ToBoolean(dr["Flg_Boleto_Registrado"]);
            if (dr["Flg_Nota_Antecipada"] != DBNull.Value)
                objPlanoAdquirido._flagNotaAntecipada = Convert.ToBoolean(dr["Flg_Nota_Antecipada"]);
            objPlanoAdquirido._flagRecorrente = Convert.ToBoolean(dr["Flg_Recorrente"]);
            if (dr["Dta_Cancelamento"] != DBNull.Value)
                objPlanoAdquirido._dataCancelamento = Convert.ToDateTime(dr["Dta_Cancelamento"]);
            objPlanoAdquirido._persisted = true;
            objPlanoAdquirido._modified = false;

            return true;
        }
        #endregion

        #region ExistePlanoAdquiridoAguardandoLiberacao
        /// <summary>
        /// Verifica se existe um plano adquirido aguardando liberação para a <paramref name="objFilial"/> informada e para o <paramref name="objPlano"/> informado
        /// </summary>
        /// <param name="objFilial"></param>
        /// <param name="objPlano"></param>
        /// <returns></returns>
        public static bool ExistePlanoAdquiridoAguardandoLiberacao(Filial objFilial, Plano objPlano)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Filial", SqlDbType = SqlDbType.Int, Size = 4, Value = objFilial.IdFilial } ,
                    new SqlParameter { ParameterName = "@Idf_Plano", SqlDbType = SqlDbType.Int, Size = 4, Value = objPlano.IdPlano }
                };

            return Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, Spexisteplanoadquiridoaguardandoliberacaoporfilialplano, parms)) > 0;
        }
        /// <summary>
        /// Verifica se existe um plano adquirido aguardando liberação para a <paramref name="objCurriculo"/> informada e para o <paramref name="objPlano"/> informado
        /// </summary>
        /// <param name="objCurriculo"></param>
        /// <param name="objPlano"></param>
        /// <returns></returns>
        public static bool ExistePlanoAdquiridoAguardandoLiberacao(Curriculo objCurriculo, Plano objPlano)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Curriculo", SqlDbType = SqlDbType.Int, Size = 4, Value = objCurriculo.IdCurriculo } ,
                    new SqlParameter { ParameterName = "@Idf_Plano", SqlDbType = SqlDbType.Int, Size = 4, Value = objPlano.IdPlano }
                };

            return Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, Spexisteplanoadquiridoaguardandoliberacaoporcandidatoplano, parms)) > 0;
        }
        #endregion

        #region ExistePlanoAdquiridoRenovado
        public static int ExistePlanoAdquiridoRenovado(int idUsuarioFilialPerfil)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter{ ParameterName = "@Idf_Usuario_Filial_Perfil", SqlDbType = SqlDbType.Int, Size = 4, Value = idUsuarioFilialPerfil}
                };

            return Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SpVerificaPlanoRenovacao, parms));
        }
        #endregion

        #region ExistePlanoAdquiridoLiberadoPorFilial
        public static bool ExistePlanoAdquiridoLiberadoPorFilial(Filial objFilial, SqlTransaction trans = null)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Filial", SqlDbType = SqlDbType.Int, Size = 4, Value = objFilial.IdFilial }
                };

            if (trans == null)
                return Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, Spselectexisteplanoadquiridoliberadofilial, parms)) > 0;
            return Convert.ToInt32(DataAccessLayer.ExecuteScalar(trans, CommandType.Text, Spselectexisteplanoadquiridoliberadofilial, parms)) > 0;
        }
        #endregion

        #region ExistePlanoAdicionalAguardandoPagamento
        /// <summary>
        /// Verifica se existe algum plano adicional ligado ao plano adquirido atual que esteja aguardando pagamento
        /// </summary>
        /// <returns></returns>
        public bool ExistePlanoAdicionalAguardandoLiberacao()
        {
            return ExistePlanoAdicionalPorSituacao(Enumeradores.AdicionalPlanoSituacao.AguardandoLiberacao);
        }
        #endregion

        #region ExistePlanoAdicionalPorSituacao
        /// <summary>
        /// Verifica se existe algum plano adicional dado a sua situação para o Plano Adquirido Atual
        /// </summary>
        /// <param name="adicionalPlanoSituacao">Situação deseja para o plano adicional</param>
        /// <returns></returns>
        public bool ExistePlanoAdicionalPorSituacao(Enumeradores.AdicionalPlanoSituacao adicionalPlanoSituacao)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Plano_Adquirido", SqlDbType = SqlDbType.Int, Size = 4, Value = _idPlanoAdquirido },
                    new SqlParameter { ParameterName = "@Idf_Adicional_Plano_Situacao", SqlDbType = SqlDbType.Int, Size = 4, Value = (int)adicionalPlanoSituacao }
                };

            return Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, Spselectexisteplanoadicional, parms)) > 0;
        }
        #endregion

        #region DerrubarPlanosAguardandoLiberacao
        /// <summary>
        /// Método responsável por derrubar todos os planos que estão com o status aguardando publicação
        /// </summary>
        public static void DerrubarPlanosAguardandoLiberacao()
        {
            try
            {
                var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@QuantidadeDias", SqlDbType = SqlDbType.Int, Size = 4, Value = Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.QuantidadeDiasVencimentoDerrubarPlanoAguardandoLiberacao)) }
                };

                var lista = new List<PlanoAdquirido>();
                using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spretornaplanosvencidosaguardandoliberacao, parms))
                {
                    while (dr.Read())
                    {
                        var objPlanoAdquirido = new PlanoAdquirido();
                        if (SetInstanceNotDipose(dr, objPlanoAdquirido))
                            lista.Add(objPlanoAdquirido);
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }

                foreach (PlanoAdquirido objPlanoAdquirido in lista)
                {
                    try
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
                        objPlanoAdquirido.CancelarPlanoAdquirido(null, "PlanoAdquirido > DerrubarPlanosAguardandoLiberacao", false, idCurriculo);
                    }
                    catch (Exception ex)
                    {
                        EL.GerenciadorException.GravarExcecao(ex);
                    }
                }
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
                throw;
            }
        }
        #endregion Liberado

        #region Liberado
        private bool Liberado(SqlTransaction trans = null)
        {
            if (null == this.PlanoSituacao)
            {
                if (null == trans)
                    CompleteObject();
                else
                    CompleteObject(trans);
            }

            return (this.PlanoSituacao.IdPlanoSituacao == (int)Enumeradores.PlanoSituacao.Liberado);
        }
        #endregion Liberado

        #region AtualizaPlanosDeLiberacaoFutura
        /// <summary>
        /// Método responsável por alterar o status dos planos de liberação futura para "Aguardando Liberação" (se nenhuma parcela foi paga) ou "Liberado" (se existem parcelas pagas)
        /// </summary>
        public static void AtualizaPlanosDeLiberacaoFutura()
        {
            try
            {
                using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SP_RETORNA_PLANOS_LIBERACAO_FUTURA_PARA_ATUALIZACAO, null))
                {
                    while (dr.Read())
                    {
                        int numParcelasPagas = Convert.ToInt32(dr["num_parcelas_pagas"]);
                        var objPlanoAdquirido = new PlanoAdquirido();
                        if (SetInstanceNotDipose(dr, objPlanoAdquirido))
                        {
                            if (numParcelasPagas > 0)
                            {
                                objPlanoAdquirido.Liberar(true);
                            }
                            else
                            {
                                objPlanoAdquirido.PlanoSituacao = new PlanoSituacao((int)Enumeradores.PlanoSituacao.AguardandoLiberacao);
                            }
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
                throw;
            }
        }
        #endregion

        #region AtualizaPlanosDeLiberacaoAutomatica
        /// <summary>
        /// Método responsável por alterar o status dos planos de "Liberação Automática" para "Liberado"
        /// </summary>
        public static void AtualizaPlanosDeLiberacaoAutomatica()
        {
            try
            {
                using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SP_RETORNA_PLANOS_LIBERACAO_AUTOMATICA_PARA_ATUALIZACAO, null))
                {
                    while (dr.Read())
                    {
                        var objPlanoAdquirido = new PlanoAdquirido();
                        if (SetInstanceNotDipose(dr, objPlanoAdquirido))
                        {
                            objPlanoAdquirido.Liberar(true);
                        }
                    }

                    if (!dr.IsClosed)
                        dr.Close();
                }
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
                throw;
            }
        }
        #endregion

        #region RecuperarValor
        public decimal RecuperarValor()
        {
            CompleteObject();

            return this.ValorBase;
        }
        #endregion

        #region CarregarPlanoAdquiridopDePagamento
        /// <summary>
        /// Carrega o plano adquirido do pagamento informado.
        /// </summary>
        /// <param name="idPagamento">Id do registro de pagamento</param>
        /// <returns>Instância de Plano Adquirido do pagamento informado</returns>
        /// <exception cref="RecordNotFoundException">RecordNotFoundException se registro de plano adquirido não encontrado</exception>
        public static PlanoAdquirido CarregarPlanoAdquiridopDePagamento(int idPagamento)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter{ ParameterName = "@Idf_Pagamento", SqlDbType = SqlDbType.Int, Size = 4
                        , Value = idPagamento}
                };

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SELECT_PLANO_ADQUIRIDO_DE_PAGAMENTO, parms))
            {
                PlanoAdquirido objPlanoAdquirido = new PlanoAdquirido();
                if (SetInstance(dr, objPlanoAdquirido))
                    return objPlanoAdquirido;
            }
            throw (new RecordNotFoundException(typeof(Plano)));
        }
        #endregion

        #region ExistePlanoAdquiridoLiberadoPorFilialElegivel1Clique
        public static bool ExistePlanoAdquiridoLiberadoPorFilialElegivel1Clique(Filial objFilial, SqlTransaction trans = null)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Filial", SqlDbType = SqlDbType.Int, Size = 4, Value = objFilial.IdFilial }
                };

            if (trans == null)
                return Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, Spselectexisteplanoadquiridoliberadofilialelegivel1clique, parms)) > 0;
            return Convert.ToInt32(DataAccessLayer.ExecuteScalar(trans, CommandType.Text, Spselectexisteplanoadquiridoliberadofilialelegivel1clique, parms)) > 0;
        }
        #endregion

        #region ExistePlanoAdquiridoPrecisandoAceiteContrato
        public static bool ExistePlanoAdquiridoPrecisandoAceiteContrato(Filial objFilial)
        {
            var parametro = Convert.ToDateTime(Parametro.RecuperaValorParametro(Enumeradores.Parametro.DataImplantacaoAceiteContrato));
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Filial", SqlDbType = SqlDbType.Int, Size = 4, Value = objFilial.IdFilial },
                    new SqlParameter { ParameterName = "@DataImplantacaoAceiteContrato", SqlDbType = SqlDbType.DateTime, Size = 4, Value = parametro }
                };

            return Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, Sppossuecontratosemaceite, parms)) > 0;
        }
        #endregion

        #region ExistePlanoAdquiridoPrecisandoAceiteContrato
        public static bool ExistePlanoAdquiridoPrecisandoAceiteContrato(UsuarioFilialPerfil objUsuarioFilialPerfil)
        {
            var parametro = Convert.ToDateTime(Parametro.RecuperaValorParametro(Enumeradores.Parametro.DataImplantacaoAceiteContrato));
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Usuario_Filial_Perfil", SqlDbType = SqlDbType.Int, Size = 4, Value = objUsuarioFilialPerfil.IdUsuarioFilialPerfil },
                    new SqlParameter { ParameterName = "@DataImplantacaoAceiteContrato", SqlDbType = SqlDbType.DateTime, Size = 4, Value = parametro }
                };

            return Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, Sppossuecontratosemaceiteusuario, parms)) > 0;
        }
        #endregion

        #region Contrato
        public string Contrato()
        {
            var aceites = PlanoAdquiridoContratoUsuario.RecuperarAceites(this);
            string usuariosAceite = aceites.Any() ? "Contrato Aceito Por: <br />" : string.Empty;

            foreach (var objPlanoAdquiridoContrato in aceites)
            {
                objPlanoAdquiridoContrato.UsuarioFilialPerfil.CompleteObject();
                objPlanoAdquiridoContrato.UsuarioFilialPerfil.PessoaFisica.CompleteObject();
                usuariosAceite += String.Format("<b>Usuário: {0}</b><br><b>CPF: {1}</b><br><b>Data/Hora: {2}</b><br>", objPlanoAdquiridoContrato.UsuarioFilialPerfil.PessoaFisica.NomeCompleto, objPlanoAdquiridoContrato.UsuarioFilialPerfil.PessoaFisica.NumeroCPF, objPlanoAdquiridoContrato.DataCadastro);
            }

            string nomePessoaResponsavel, razaoSocial, lougradouro, numeroRua, cidade, estado, cep;
            decimal numeroCPFResponsavel, numeroCNPJ;
            int numeroParcelas = 0;

            var tempoPlano = Convert.ToInt32(Math.Ceiling(((double)this.DataFimPlano.Subtract(this.DataInicioPlano).Days) / (365.25 / 12)));
            tempoPlano = tempoPlano > 12 ? 12 : tempoPlano;

            this.Filial.CarregarDadosUsuarioResponsavel(out nomePessoaResponsavel, out numeroCPFResponsavel);
            this.Filial.RecuperarConteudoFilialParaContratoPorFilial(out razaoSocial, out numeroCNPJ, out lougradouro, out numeroRua, out cidade, out estado, out cep);

            int quantidadeSMS = 0, quantidadSMSTanque = 0;
            if (this.Plano.CompleteObject())
            {
                numeroParcelas = this.QtdParcela.HasValue ? this.QtdParcela.Value : this.Plano.QuantidadeParcela;
                if (this.Plano.FlagLiberaUsuariosTanque)
                    quantidadSMSTanque = this.Filial.RecuperarCotaSMSTanque();
                else
                    quantidadeSMS = this.QuantidadeSMS / numeroParcelas;
            }

            string descricaoQuantidadaSMS = string.Empty;
            if (quantidadeSMS > 0)
                descricaoQuantidadaSMS += quantidadeSMS + " sms por mês;";

            if (quantidadSMSTanque > 0)
                descricaoQuantidadaSMS += quantidadSMSTanque + " sms por usuário/dia;";

            var quantidadeUsuario = this.Plano.QuantidadeParcela == 3 ? 3 : 1;
            var parametros = new
            {
                NumeroCNPJ = numeroCNPJ,
                RazaoSocial = razaoSocial,
                Logradouro = lougradouro,
                NumeroRua = numeroRua,
                Cidade = Helper.FormatarCidade(cidade, estado),
                NumeroCEP = cep,
                NomeResponsavelEmpresa = nomePessoaResponsavel,
                CPFResponsavelEmpresa = Helper.FormatarCPF(numeroCPFResponsavel),
                ValorPlano = numeroParcelas * this.ValorBase,
                NumeroParcelas = this.QtdParcela.HasValue ? this.QtdParcela : this.Plano.QuantidadeParcela,
                ValorParcelas = this.ValorBase,
                QuantidadeUsuarios = quantidadeUsuario,
                QuantidadeSMS = descricaoQuantidadaSMS,
                ValorTempoPlano = tempoPlano,
                UsuariosAceite = usuariosAceite,
                NomePlano = this.Plano.DescricaoPlano,
                ParcelasMultaRecisao = this.Plano.QuantidadeParcela == 3 ? 1 : (this.Plano.QuantidadeParcela == 6 ? 2 : 3),
                TextQuantidadeVisualizacoesTipo3 = "Visualização ilimitada de currículos por mês",
                QuantidadeSMSTipo3 = "Envio de 100 sms por usuário/dia"
            };

            var template = PlanoAdquiridoContrato.RecuperarContrato(this);
            return FormatObject.ToString(parametros, template);
        }
        #endregion

        #region ContratoPlanoRecorrenteCia
        public string ContratoPlanoRecorrenteCia(PlanoAdquirido objPlanoAdquirido)
        {
            var aceites = PlanoAdquiridoContratoUsuario.RecuperarAceites(this);
            string usuariosAceite = aceites.Any() ? "Contrato Aceito Por: <br />" : string.Empty;

            foreach (var objPlanoAdquiridoContrato in aceites)
            {
                objPlanoAdquiridoContrato.UsuarioFilialPerfil.CompleteObject();
                objPlanoAdquiridoContrato.UsuarioFilialPerfil.PessoaFisica.CompleteObject();
                usuariosAceite += String.Format("<b>Usuário: {0}</b><br><b>CPF: {1}</b><br><b>Data/Hora: {2}</b><br>", objPlanoAdquiridoContrato.UsuarioFilialPerfil.PessoaFisica.NomeCompleto, objPlanoAdquiridoContrato.UsuarioFilialPerfil.PessoaFisica.NumeroCPF, objPlanoAdquiridoContrato.DataCadastro);
            }

            int quantidadeParcelas = objPlanoAdquirido.Plano.QuantidadeParcela;
            string nrParcelas = quantidadeParcelas.ToString() + (quantidadeParcelas > 1 ? " meses" : " mês");
            string nomePlano = objPlanoAdquirido.Plano.DescricaoPlano;

            string multaRecisao = quantidadeParcelas == 1 ? quantidadeParcelas.ToString() + " valor mensal estipulado" : (quantidadeParcelas == 6 ? " 2 valores mensais estipulados" : " 3 valores mensais estipulados");
            string nomePessoaResponsavel, razaoSocial, lougradouro, numeroRua, cidade, estado, cep;
            decimal numeroCPFResponsavel, numeroCNPJ;

            this.Filial.CarregarDadosUsuarioResponsavel(out nomePessoaResponsavel, out numeroCPFResponsavel);
            this.Filial.RecuperarConteudoFilialParaContratoPorFilial(out razaoSocial, out numeroCNPJ, out lougradouro, out numeroRua, out cidade, out estado, out cep);

            string descricaoQuantidadeSMS = objPlanoAdquirido.Plano.QuantidadeSMS.ToString() + (!objPlanoAdquirido.Plano.FlagBoletoRecorrente ? " sms por mês" : " envios de SMS e e-mails");
            string quantidadeVisualizacoes = objPlanoAdquirido.Plano.QuantidadeVisualizacao.ToString();

            string descricaoQuantidadeVisualizacoes = "Visualização de " + quantidadeVisualizacoes + (!objPlanoAdquirido.Plano.FlagBoletoRecorrente ? " currículos de forma limitada e anúncio de vagas ilimitado" : " currículos por mês");

            var parametros = new
            {
                NumeroCNPJ = numeroCNPJ,
                RazaoSocial = razaoSocial,
                Logradouro = lougradouro,
                NumeroRua = numeroRua,
                Cidade = Helper.FormatarCidade(cidade, estado),
                NumeroCEP = cep,
                NomeResponsavelEmpresa = nomePessoaResponsavel,
                CPFResponsavelEmpresa = Helper.FormatarCPF(numeroCPFResponsavel),
                ValorPlano = this.ValorBase.ToString(),
                QuantidadeUsuarios = this.Filial.RecuperarQuantidadeAcessosAdquiridos(),
                TextQuantidadeVisualizacoes = descricaoQuantidadeVisualizacoes,
                QuantidadeSMS = descricaoQuantidadeSMS,
                UsuariosAceite = usuariosAceite,
                NumeroDeParcelas = nrParcelas,
                MultaRecisao = multaRecisao,
                NomePlano = nomePlano
            };

            var template = PlanoAdquiridoContrato.RecuperarContrato(this);
            return FormatObject.ToString(parametros, template);
        }
        #endregion

        #region CarregaListaPlanoAdquiridosComRecorrencia
        public static List<PlanoAdquirido> CarregaListaPlanoAdquiridosComRecorrencia()
        {
            List<PlanoAdquirido> listPlanoAdquirido = new List<PlanoAdquirido>();
            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SP_LISTA_TODOS_PLANOS_COM_RECORRENCIA_VENCIDA, null))
            {
                while (dr.Read())
                {
                    var planoAdiquirido = PlanoAdquirido.LoadObject(Convert.ToInt32(dr["Idf_Plano_Adquirido"]));
                    if (dr["Idf_Plano_Adquirido"] != DBNull.Value && Convert.ToInt32(dr["totalCobrancas"]) > 0)
                    {
                        planoAdiquirido.QuantidadeParcelasPagas = Convert.ToInt32(dr["totalCobrancas"]);
                    }
                    listPlanoAdquirido.Add(planoAdiquirido);
                }

            }
            return listPlanoAdquirido;
        }
        #endregion

        #region ExisteParcelaEmAbertoRecorrenciaDebito
        public bool ExisteParcelaEmAbertoRecorrenciaDebito(SqlTransaction trans)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter{ ParameterName = "@Idf_Plano_Adquirido", SqlDbType = SqlDbType.Int, Size = 4, Value = this.IdPlanoAdquirido}
                };

            using (IDataReader dr = DataAccessLayer.ExecuteReader(trans, CommandType.Text, SP_EXISTE_PARCELA_ABERTA_NO_PLANO_ADQUIRIDO, parms))
            {
                while (dr.Read())
                    if (string.IsNullOrEmpty(dr["Parcela"].ToString()) || Convert.ToInt32(dr["Parcela"]) == 0)
                        return false;
            }
            return true;
        }
        #endregion

        #region ExistePlanoAdquiridoPublicacaoImediata
        /// <summary>
        /// Verifica se existe algum plano adquirido encerrado para a vaga
        /// </summary>
        /// <param name="objVaga"></param>
        /// <returns></returns>
        internal static PlanoAdquirido ExistePlanoAdquiridoPublicacaoImediata(Vaga objVaga)
        {
            var objPlanoAdquirido = new PlanoAdquirido();

            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Vaga", SqlDbType = SqlDbType.Int, Size = 4, Value = objVaga.IdVaga }
                };

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spexisteplanoadiquiridoparavaga, parms))
            {
                if (!SetInstance(dr, objPlanoAdquirido))
                {
                    objPlanoAdquirido = null;
                }
                else
                {
                    objPlanoAdquirido.PlanoQuantidade = new PlanoQuantidade(objPlanoAdquirido);
                }
            }

            return objPlanoAdquirido;
        }
        #endregion

        #region ExisteParcelaEmAbertoRecorrenciaDebito
        public int QuantidadeDeParcelasPagaPlanoAdquirido(SqlTransaction trans)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter{ ParameterName = "@Idf_Plano_Adquirido", SqlDbType = SqlDbType.Int, Size = 4, Value = this.IdPlanoAdquirido}
                };

            using (IDataReader dr = DataAccessLayer.ExecuteReader(trans, CommandType.Text, SP_QTD_PARCELAS_PAGAS_PLANO_ADQUIRIDO, parms))
            {
                while (dr.Read())
                    return Convert.ToInt32(dr["QTD_PAGA"]);
            }
            return 0;
        }
        #endregion

        #region Saldo

        #region SaldoSMS
        /// <summary>
        /// Recupera o saldo vigente de sms para o plano adquirido 
        /// </summary>
        /// <param name="trans"></param>
        /// <returns></returns>
        //TODO Ver a possibilidade de mudar para possui saldo
        public int SaldoSMS(SqlTransaction trans = null)
        {
            if (PlanoQuantidade != null)
            {
                return PlanoQuantidade.SaldoSMS(trans);
            }
            return 0;
        }
        #endregion

        #region SaldoVisualizacao
        /// <summary>
        /// Recupera o saldo vigente de visualizacao para o plano adquirido 
        /// </summary>
        /// <param name="trans"></param>
        /// <returns></returns>
        //TODO Ver a possibilidade de mudar para possui saldo
        public int SaldoVisualizacao(SqlTransaction trans = null)
        {
            if (PlanoQuantidade != null)
            {
                return PlanoQuantidade.SaldoVisualizacao(trans);
            }
            return 0;
        }
        #endregion

        #region SaldoCampanha
        /// <summary>
        /// Recupera o saldo vigente de campanha para o plano adquirido 
        /// </summary>
        /// <param name="trans"></param>
        /// <returns></returns>
        public int SaldoCampanha(SqlTransaction trans = null)
        {
            if (PlanoQuantidade != null && PlanoQuantidade.IdPlanoQuantidade > 0)
            {
                return PlanoQuantidade.SaldoCampanha(trans);
            }
            return 0;
        }
        #endregion

        #endregion

        #region Desconto de Saldo

        #region DescontarVisualizacao
        public bool DescontarVisualizacao(SqlTransaction trans)
        {
            if (PlanoQuantidade != null)
            {
                try
                {
                    PlanoQuantidade.DescontarVisualizacao(trans);
                    return true;
                }
                catch (Exception ex)
                {
                    EL.GerenciadorException.GravarExcecao(ex);
                }
            }
            return false;
        }
        #endregion

        #region DescontarSMS
        public bool DescontarSMS(SqlTransaction trans)
        {
            if (PlanoQuantidade != null)
            {
                try
                {
                    PlanoQuantidade.DescontarSMS(trans);
                    return true;
                }
                catch (Exception ex)
                {
                    EL.GerenciadorException.GravarExcecao(ex);
                }
            }
            return false;
        }
        #endregion

        #region DescontarCampanha
        public bool DescontarCampanha(SqlTransaction trans)
        {
            if (PlanoQuantidade != null)
            {
                try
                {
                    PlanoQuantidade.DescontarCampanha(trans);
                    return true;
                }
                catch (Exception ex)
                {
                    EL.GerenciadorException.GravarExcecao(ex);
                }
            }
            return false;
        }
        #endregion

        #endregion

        #region Incremento de Saldo

        #region IncrementarSaldoVisualizacao
        public bool IncrementarSaldoVisualizacao(SqlTransaction trans)
        {
            if (PlanoQuantidade != null)
            {
                try
                {
                    PlanoQuantidade.IncrementarVisualizacao(trans);
                    return true;
                }
                catch (Exception ex)
                {
                    EL.GerenciadorException.GravarExcecao(ex);
                }
            }
            return false;
        }
        #endregion

        #endregion

        #region  ConcederPlanoPFViaMobile
        public static bool ConcederPlanoPFViaMobile(Curriculo objCurriculo, BLL.Plano objPlano, string purchaseToken, string orderId)
        {
            using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        objPlano.CompleteObject(trans);
                        objCurriculo.PessoaFisica.CompleteObject(trans);


                        #region TAB_Usuario_Filial_Perfil
                        UsuarioFilialPerfil objUsuarioFilialPerfil;

                        if (!UsuarioFilialPerfil.CarregarUsuarioFilialPerfilCandidatoAtivoEInativo(objCurriculo.PessoaFisica, trans, out objUsuarioFilialPerfil))
                        {
                            objUsuarioFilialPerfil = new UsuarioFilialPerfil
                            {
                                PessoaFisica = objCurriculo.PessoaFisica,
                                SenhaUsuarioFilialPerfil = objCurriculo.PessoaFisica.DataNascimento.ToString("ddMMyyyy"),
                                Perfil = new Perfil((int)Enumeradores.Perfil.AcessoVIP)
                            };
                            objUsuarioFilialPerfil.Save(trans);
                        }
                        else
                        {
                            //Corrige o perfil do Candidato
                            objUsuarioFilialPerfil.Perfil = new Perfil((int)Enumeradores.Perfil.AcessoVIP);
                            objUsuarioFilialPerfil.Save(trans);
                        }
                        #endregion

                        #region BNE_Plano_Adquirido

                        var objPlanoAdquirido = new PlanoAdquirido
                        {
                            UsuarioFilialPerfil = objUsuarioFilialPerfil,
                            Plano = objPlano,
                            DataInicioPlano = DateTime.Now,
                            DataFimPlano = DateTime.Now.AddDays(objPlano.QuantidadeDiasValidade),
                            PlanoSituacao = new PlanoSituacao((int)Enumeradores.PlanoSituacao.Liberado),
                            ValorBase = objPlano.ValorBase,
                            QuantidadeSMS = objPlano.QuantidadeSMS,
                            FlagBoletoRegistrado = objPlano.FlagBoletoRegistrado
                        };
                        objPlanoAdquirido.Save(trans);

                        #endregion

                        #region BNE_Plano_Parcela

                        var objPlanoParcela = new PlanoParcela
                        {
                            DataPagamento = DateTime.Now,
                            FlagInativo = false,
                            PlanoAdquirido = objPlanoAdquirido,
                            PlanoParcelaSituacao = new PlanoParcelaSituacao((int)Enumeradores.PlanoParcelaSituacao.Pago),
                            ValorParcela = objPlano.ValorBase
                        };
                        objPlanoParcela.Save(trans);

                        #endregion

                        #region BNE_Pagamento
                        var objPagamento = new Pagamento
                        {
                            DataEmissao = DateTime.Now,
                            DataVencimento = DateTime.Now,
                            DescricaoDescricao = purchaseToken,
                            DescricaoIdentificador = orderId,
                            FlagBaixado = true,
                            FlagInativo = false,
                            PagamentoSituacao = new PagamentoSituacao((int)Enumeradores.PagamentoSituacao.Pago),
                            PlanoParcela = objPlanoParcela,
                            TipoPagamento = new TipoPagamento((int)Enumeradores.TipoPagamento.Mobile),
                            UsuarioFilialPerfil = objUsuarioFilialPerfil,
                            ValorPagamento = 0
                        };
                        objPagamento.Save(trans);
                        #endregion

                        #region Curriculo
                        //Atualizar a situação do Currículo do Candidato
                        objCurriculo.SituacaoCurriculo = new SituacaoCurriculo((int)Enumeradores.SituacaoCurriculo.AguardandoRevisaoVIP);
                        objCurriculo.FlagVIP = true;
                        objCurriculo.Save(trans);
                        #endregion

                        BufferAtualizacaoCurriculo.Update(objCurriculo);

                        trans.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        EL.GerenciadorException.GravarExcecao(ex, "erro em vc");
                        throw;
                    }
                }
            }
        }

        #endregion

        #region ExistePlanoAdquiridoIlimitadoLiberadoPorFilial
        public static bool ExistePlanoAdquiridoIlimitadoLiberadoPorFilial(Filial objFilial, SqlTransaction trans = null)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Filial", SqlDbType = SqlDbType.Int, Size = 4, Value = objFilial.IdFilial }
                };

            if (trans == null)
                return Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SpselectexisteplanoadquiridoIlimitadoliberadofilial, parms)) > 0;
            return Convert.ToInt32(DataAccessLayer.ExecuteScalar(trans, CommandType.Text, SpselectexisteplanoadquiridoIlimitadoliberadofilial, parms)) > 0;
        }
        #endregion

        #endregion

    }

}

public class PlanoAdiquiridoVencidos {
    public int IdPlanoAdquirido { get; set; }
    public int TipoPagamento { get; set; }
    public int? TotalCobranca {get;set;}
    public int PagamentoSituacao { get; set; }
    public DateTime DataFimPlano { get; set; }
    public int IdFilial { get; set; }
    public int QuantidadeParcelasPlanoAdquirido { get; set; }
    public int QuantidadeParcelasPlano { get; set; }
}