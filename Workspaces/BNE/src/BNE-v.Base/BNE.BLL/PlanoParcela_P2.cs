﻿//-- Data: 08/04/2010 11:17
//-- Autor: Gieyson Stelmak

using BNE.Services.Base.ProcessosAssincronos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;

namespace BNE.BLL
{
    public partial class PlanoParcela // Tabela: BNE_Plano_Parcela
    {
        #region Consultas

        #region Spparcelasporplanoadquirido
        private const string Spparcelasporplanoadquirido =
        @"DECLARE @FirstRec INT
        DECLARE @LastRec INT
        DECLARE @iSelect VARCHAR(8000)
        DECLARE @iSelectCount VARCHAR(8000)
        DECLARE @iSelectPag VARCHAR(8000)
        
        SET @FirstRec = ( @CurrentPage * @PageSize + 1 )
        SET @LastRec = ( @CurrentPage * @PageSize ) + @PageSize

        SET @iSelect = '
            SELECT	
			        ROW_NUMBER() OVER (ORDER BY PG.Dta_Emissao, PP.Idf_Plano_Adquirido) AS RowID , 
                     CONVERT(VARCHAR, ROW_NUMBER() OVER (ORDER BY PP.Idf_Plano_Adquirido)) + ''/'' + 
                            CONVERT(VARCHAR, (SELECT COUNT(PP2.Idf_Plano_Adquirido) 
                            FROM BNE_Plano_Parcela PP2 WITH(NOLOCK)
                            WHERE PP2.Idf_Plano_Adquirido = PP.Idf_Plano_Adquirido )) AS Parcela,                    
                    PG.Idf_Pagamento ,
                    PG.Des_Identificador ,
                    PG.Dta_Emissao ,
                    PG.Dta_Vencimento ,
                    PG.Vlr_Pagamento ,
                    PG.Idf_Tipo_Pagamento ,
                    TP.Des_Tipo_Pagamaneto ,
                    PS.Des_Pagamento_Situacao  ,
                    PS.Idf_Pagamento_Situacao ,
                    PD.Eml_Envio_Boleto ,
                    ( CASE WHEN PL.Vlr_Base = 0 THEN 1 ELSE 0 END ) AS Cortesia
                    ,PG.Num_Nota_Fiscal
                    ,PG.Url_Nota_Fiscal
                    ,PP.Qtd_SMS_Total
                    ,PP.Qtd_SMS_Liberada
                    ,PA.Flg_Nota_Antecipada  
                    ,PP.Dta_Pagamento                  
            FROM    BNE_Plano PL WITH(NOLOCK)
                    INNER JOIN BNE_Plano_Adquirido PA WITH(NOLOCK) ON PL.Idf_Plano = PA.Idf_Plano
                    INNER JOIN BNE_Plano_Parcela PP WITH(NOLOCK) ON PA.Idf_Plano_Adquirido = PP.Idf_Plano_Adquirido
                    INNER JOIN BNE_Pagamento PG WITH(NOLOCK) ON PG.Idf_Plano_Parcela = PP.Idf_Plano_Parcela
                    LEFT JOIN BNE_Plano_Adquirido_Detalhes PD WITH(NOLOCK) ON PP.Idf_Plano_Adquirido = PD.Idf_Plano_Adquirido
                    LEFT JOIN BNE_Pagamento_Situacao PS WITH(NOLOCK) ON PG.Idf_Pagamento_Situacao = PS.Idf_Pagamento_Situacao
                    LEFT JOIN BNE_Tipo_Pagamento TP WITH(NOLOCK) ON TP.Idf_Tipo_Pagamento = PG.Idf_Tipo_Pagamento
            WHERE pg.Idf_Pagamento_Situacao <> 3	
                and PP.Idf_Plano_Adquirido = ' + CONVERT(VARCHAR, @Idf_Plano_Adquirido) 

        SET @iSelectCount = 'Select Count(*) From ( ' + @iSelect + ' ) As TblTempCount'
        SET @iSelectPag = 'Select * From ( ' + @iSelect + ' ) As TblTempPag	Where RowID BETWEEN ' + CONVERT(VARCHAR, @FirstRec) + ' AND ' + CONVERT(VARCHAR, @LastRec)
        EXECUTE (@iSelectCount)
        EXECUTE (@iSelectPag)";
        #endregion

        #region SpJaExisteTipoPagamentoParcela
        private const string SpJaExisteTipoPagamentoParcela = @"
        select count(pp.Idf_Plano_Parcela)
        from bne.BNE_Plano_Parcela pp with(nolock)
            join bne.bne_pagamento pg with(nolock) on pp.Idf_Plano_Parcela = pg.Idf_Plano_Parcela
        where pp.Idf_Plano_Adquirido = @Idf_Plano_Adquirido
            and pg.Idf_Tipo_Pagamento = @Idf_Tipo_Pagamento
        ";
        #endregion

        #region SpJaExisteParcelaCriada
        private const string SpJaExisteParcelaCriada = @"
        select count(pp.Idf_Plano_Parcela)
        from bne.BNE_Plano_Parcela pp with(nolock)
            join bne.bne_pagamento pg with(nolock) on pp.Idf_Plano_Parcela = pg.Idf_Plano_Parcela
        where pp.Idf_Plano_Adquirido = @Idf_Plano_Adquirido
        ";
        #endregion

        #region SpRecuperarParcelasPlanoAdquirido
        private const string SpRecuperarParcelasPlanoAdquirido = @"
        SELECT  pp.*
        FROM    bne.BNE_Plano_Parcela pp with(nolock)
        WHERE   pp.Idf_Plano_Adquirido = @Idf_Plano_Adquirido
        ";
        #endregion

        #region Spparcelasporplanoadquiridolist
        private const string Spparcelasporplanoadquiridolist = @"
        SELECT  * 
        FROM    BNE_Plano_Parcela PP WITH(NOLOCK) 
        WHERE   Idf_Plano_Adquirido = @Idf_Plano_Adquirido";
        #endregion

        #region Spprimeiraparcelaporusuariofilialperfil
        private const string Spprimeiraparcelaporusuariofilialperfil = @"
        SELECT  TOP 1 *
        FROM    BNE_Plano_Parcela WITH(NOLOCK)
        WHERE   Idf_Plano_Adquirido = @Idf_Plano_Adquirido
        ORDER BY Idf_Plano_Parcela ASC";
        #endregion

        #region Spultimaparcelaporusuariofilialperfil
        private const string Spultimaparcelaporplanoadquirido = @"
        SELECT  TOP 1 *
        FROM    BNE_Plano_Parcela WITH(NOLOCK)
        WHERE   Idf_Plano_Adquirido = @Idf_Plano_Adquirido
        ORDER BY Idf_Plano_Parcela DESC";
        #endregion

        #region Spparcelaatualemabertoporplanoadquirido
        private const string Spparcelaatualemabertoporplanoadquirido = @"
        SELECT  TOP 1 *
        FROM    BNE_Plano_Parcela P WITH(NOLOCK)
        WHERE   P.Idf_Plano_Adquirido = @Idf_Plano_Adquirido
                AND Idf_Plano_Parcela_Situacao = 1
                AND Flg_Inativo = 0
        ORDER BY P.Idf_Plano_Parcela ASC";
        #endregion

        #region Spnotasantecipadas
        private const string Spnotasantecipadas = @"
        SELECT  pg.Idf_Pagamento ,
                ISNULL(pad.Eml_Envio_Boleto, usuario_master.Eml_Comercial) AS 'Eml_Envio_Boleto'
        FROM    [BNE].BNE_Pagamento pg WITH ( NOLOCK )
                INNER JOIN [BNE].BNE_Plano_Parcela pp WITH ( NOLOCK ) ON pg.Idf_Plano_Parcela = pp.Idf_Plano_Parcela
                JOIN [BNE].BNE_Plano_Adquirido pa WITH ( NOLOCK ) ON pp.Idf_Plano_Adquirido = pa.Idf_Plano_Adquirido
                LEFT JOIN bne.BNE_Plano_Adquirido_Detalhes pad WITH ( NOLOCK ) ON pa.Idf_Plano_Adquirido = pad.Idf_Plano_Adquirido
                LEFT JOIN [BNE].TAB_Usuario_Filial_Perfil ufp WITH ( NOLOCK ) ON pa.Idf_Usuario_Filial_Perfil = ufp.Idf_Usuario_Filial_Perfil
                LEFT JOIN [BNE].BNE_Usuario_Filial uf WITH ( NOLOCK ) ON ufp.Idf_Usuario_Filial_Perfil = uf.Idf_Usuario_Filial_Perfil
                JOIN [BNE].BNE_Plano pl WITH ( NOLOCK ) ON pl.idf_plano = pa.idf_plano
                JOIN [BNE].TAB_Filial fil WITH ( NOLOCK ) ON fil.Idf_Filial = pg.Idf_Filial
                JOIN [BNE].TAB_Pessoa_Fisica pf WITH ( NOLOCK ) ON pf.Idf_Pessoa_Fisica = ufp.Idf_Pessoa_Fisica
                OUTER APPLY ( SELECT TOP ( 1 )
                                        pag_paga.Dta_Vencimento ,
                                        pag_paga.Idf_Tipo_Pagamento
                              FROM      BNE.BNE_Plano_Parcela pp_paga
                                        INNER JOIN BNE.BNE_Pagamento pag_paga ON pp_paga.Idf_Plano_Parcela = pag_paga.Idf_Plano_Parcela
                              WHERE     pp_paga.Idf_Plano_Adquirido = pp.Idf_Plano_Adquirido
                                        AND pag_paga.Idf_Pagamento_Situacao = 2 --Situação pago
                                        AND pag_paga.Dta_Vencimento < pg.Dta_Vencimento
                              ORDER BY  Dta_Pagamento
                            ) ultima_parcela_paga
                OUTER APPLY ( SELECT TOP ( 1 )
                                        pp_primeira.Idf_Plano_Parcela
                              FROM      BNE.BNE_Plano_Parcela pp_primeira
                                        JOIN BNE.BNE_Pagamento pg_primeira ON pp_primeira.Idf_Plano_Parcela = pg_primeira.Idf_Plano_Parcela
                              WHERE     pp_primeira.Idf_Plano_Adquirido = pa.Idf_Plano_Adquirido
                                        AND pg_primeira.Idf_Pagamento_Situacao IN ( 1, 2 )
                              ORDER BY  Dta_Vencimento
                            ) primeira_parcela
                OUTER APPLY ( SELECT    pf.Nme_Pessoa ,
                                        UF.Eml_Comercial
                              FROM      BNE.TAB_Usuario_Filial_Perfil UFP WITH ( NOLOCK )
                                        JOIN BNE.BNE_Usuario_Filial UF WITH ( NOLOCK ) ON UFP.Idf_Usuario_Filial_Perfil = UF.Idf_Usuario_Filial_Perfil
                                        LEFT JOIN BNE.TAB_Pessoa_Fisica PF WITH ( NOLOCK ) ON UFP.Idf_Pessoa_Fisica = PF.Idf_Pessoa_Fisica
                              WHERE     UFP.Idf_Filial = fil.Idf_Filial
                                        AND UFP.Idf_Perfil = 4 --Acesso Empresa Master
                                        AND UFP.Flg_Inativo = 0
                                        AND pad.Eml_Envio_Boleto IS NULL
                            ) usuario_master
         WHERE  1 = 1
                AND ( ( primeira_parcela.Idf_Plano_Parcela = pp.Idf_Plano_Parcela
                        AND pa.Idf_Plano_Situacao = 0) OR pa.Idf_Plano_Situacao = 1) -- Primeira parcela do plano: não existe parcela paga anterior e plano aguardando liberação ou liberado      
                AND pl.idf_plano_tipo = 2 --Plano tipo Pessoa Jurídica
                AND CONVERT(DATE, pg.Dta_Vencimento) < CONVERT(DATE, DATEADD(DAY, 20, GETDATE())) -- Parcelas que vençam daqui a 10 dias
                AND pp.Idf_Plano_Parcela_Situacao = 1 -- Parcela em Aberto
                AND pl.Qtd_Parcela > 1 -- Planos que tenham mais de uma parcela
                AND pl.Des_Plano NOT LIKE '%R1%' --Plano que não seja do R1
                AND pl.Des_Plano NOT LIKE '%Salario BR%' --Plano que não seja do Salário BR
                AND pg.Idf_Pagamento_Situacao = 1 -- Pagamento em aberto
                AND Pa.Flg_Nota_Antecipada = 1
                AND pg.Num_Nota_Fiscal IS NULL";
        #endregion

        #region Spcreditopagamentos
        private const string Spcreditopagamentos = @"
SELECT p.Idf_Plano, pa.Idf_Plano_Adquirido, pg.Idf_Pagamento
FROM [BNE].BNE_Pagamento pg WITH (NOLOCK)
INNER JOIN [BNE].BNE_Plano_Parcela pp WITH (NOLOCK) ON pg.Idf_Plano_Parcela = pp.Idf_Plano_Parcela
JOIN [BNE].BNE_Plano_Adquirido pa WITH (NOLOCK) ON pp.Idf_Plano_Adquirido = pa.Idf_Plano_Adquirido
JOIN [BNE].BNE_Plano p WITH (NOLOCK) ON pa.Idf_Plano = p.Idf_Plano
WHERE pg.Flg_Inativo = 0
AND p.Idf_Plano_Forma_Pagamento = 4 -- 4 => Mensalidade
AND pg.Idf_Pagamento_Situacao = 1 -- 1 => Em Aberto
AND pg.Idf_Tipo_Pagamento = 1 -- 1 => Cartao de Credito
AND pg.Dta_Vencimento <= CONVERT(DATE, GETDATE())";
        #endregion

        #region SP_NUMERO_PARCELA

        private const string SP_NUMERO_PARCELA = @"SELECT Num_Parcela
                                                                    FROM (SELECT	PP.Idf_Plano_Parcela,
		                                                                    CONVERT(VARCHAR, ROW_NUMBER() OVER (ORDER BY PP.Idf_Plano_Adquirido)) AS Num_Parcela
                                                                    FROM    BNE.BNE_Plano_Adquirido PA WITH(NOLOCK)
                                                                                        INNER JOIN BNE.BNE_Plano_Parcela PP WITH(NOLOCK) ON PA.Idf_Plano_Adquirido = PP.Idf_Plano_Adquirido
                                                                    WHERE PP.Idf_Plano_Parcela_Situacao <> 3 AND PA.Idf_Plano_Adquirido = (SELECT Idf_Plano_Adquirido 
                                                                                                                                           FROM BNE.BNE_Plano_Parcela 
                                                                                                                                           WHERE Idf_Plano_Parcela = @Idf_Plano_Parcela)) AS numeracao_parcelas
                                                                    WHERE Idf_Plano_Parcela = @Idf_Plano_Parcela;";

        #endregion

        #endregion

        #region Metodos

        #region ExisteTipoPagamentoParcela
        public static bool ExisteTipoPagamentoParcela(int idPlanoAdquirido, int idTipoPagamento)
        {
            var parms = new List<SqlParameter> 
                {
                    new SqlParameter{ ParameterName = "@Idf_Plano_Adquirido", SqlDbType = SqlDbType.Int, Value = idPlanoAdquirido},
                    new SqlParameter{ ParameterName = "@Idf_Tipo_Pagamento", SqlDbType = SqlDbType.Int, Value = idTipoPagamento}
                };

            return Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SpJaExisteTipoPagamentoParcela, parms)) > 0;
        }
        #endregion

        #region ExisteParcelaCriada
        /// <summary>
        /// Verifica se já foi criada parcela para o plano atual
        /// </summary>
        /// <param name="objPlanoAdquirido"></param>
        /// <returns></returns>
        public static bool ExisteParcelaCriada(PlanoAdquirido objPlanoAdquirido)
        {
            var parms = new List<SqlParameter> 
                {
                    new SqlParameter { ParameterName = "@Idf_Plano_Adquirido", SqlDbType = SqlDbType.Int, Value = objPlanoAdquirido.IdPlanoAdquirido }
                };

            return Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SpJaExisteParcelaCriada, parms)) > 0;
        }
        #endregion

        #region RecuperarParcelasPlanoAdquiridoEmAberto
        public static List<PlanoParcela> RecuperarParcelasPlanoAdquirido(PlanoAdquirido objPlanoAdquirido)
        {
            var listaParcelas = new List<PlanoParcela>();

            var parms = new List<SqlParameter> 
                {
                    new SqlParameter{ ParameterName = "@Idf_Plano_Adquirido", SqlDbType = SqlDbType.Int, Value = objPlanoAdquirido.IdPlanoAdquirido}
                };

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SpRecuperarParcelasPlanoAdquirido, parms))
            {
                var objPlanoParcela = new PlanoParcela();
                while (dr.Read())
                {
                    SetInstance_NonDispose(dr, objPlanoParcela);
                    listaParcelas.Add(objPlanoParcela);
                    objPlanoParcela = new PlanoParcela();
                }
            }

            return listaParcelas;
        }
        #endregion

        #region ListaParcelasPorPlanoAdquirido
        public static DataTable ListaParcelasPorPlanoAdquirido(int idPlanoAdquirido, int paginaAtual, int tamanhoPagina, out int totalRegistros)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@CurrentPage", SqlDbType = SqlDbType.Int, Size = 4, Value = paginaAtual },
                    new SqlParameter { ParameterName = "@PageSize", SqlDbType = SqlDbType.Int, Size = 4, Value = tamanhoPagina },
                    new SqlParameter { ParameterName = "@Idf_Plano_Adquirido", SqlDbType = SqlDbType.Int, Size = 4, Value = idPlanoAdquirido }
                };

            DataTable dt = null;
            totalRegistros = 0;
            try
            {
                using (SqlDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spparcelasporplanoadquirido, parms))
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

        public static List<PlanoParcela> ListaParcelasPorPlanoAdquirido(PlanoAdquirido objPlanoAdquirido, SqlTransaction trans)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Plano_Adquirido", SqlDbType = SqlDbType.Int, Size = 4, Value = objPlanoAdquirido.IdPlanoAdquirido }
                };

            var lstPlanoParcela = new List<PlanoParcela>();
            var objPlanoParcela = new PlanoParcela();

            SqlDataReader dr;

            if (trans != null)
                dr = DataAccessLayer.ExecuteReader(trans, CommandType.Text, Spparcelasporplanoadquiridolist, parms);
            else
                dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spparcelasporplanoadquiridolist, parms);

            while (dr.Read())
            {
                SetInstance_NonDispose(dr, objPlanoParcela);
                lstPlanoParcela.Add(objPlanoParcela);
                objPlanoParcela = new PlanoParcela();
            }

            if (!dr.IsClosed)
                dr.Close();

            dr.Dispose();


            return lstPlanoParcela;
        }

        #endregion

        #region CarregarPrimeiraParcelaPorPlanoAdquirido
        /// <summary>
        /// Carrega a primeira parcela de qualquer plano adquirido atraves do ID do plano Adquirido
        /// </summary>
        public static PlanoParcela CarregarPrimeiraParcelaPorPlanoAdquirido(int idPlanoAdquirido, SqlTransaction trans)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Plano_Adquirido", SqlDbType = SqlDbType.Int, Size = 4, Value = idPlanoAdquirido }
                };

            PlanoParcela objPlanoParcela;
            using (IDataReader dr = DataAccessLayer.ExecuteReader(trans, CommandType.Text, Spprimeiraparcelaporusuariofilialperfil, parms))
            {
                objPlanoParcela = new PlanoParcela();
                if (!SetInstance(dr, objPlanoParcela))
                    objPlanoParcela = null;
            }
            return objPlanoParcela;
        }
        #endregion

        #region CarregarUltimaParcelaPorPlanoAdquirido
        /// <summary>
        /// Carrega a ultima parcela de qualquer plano adquirido atraves do ID do plano Adquirido
        /// </summary>
        public static PlanoParcela CarregarUltimaParcelaPorPlanoAdquirido(int idPlanoAdquirido, SqlTransaction trans)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Plano_Adquirido", SqlDbType = SqlDbType.Int, Size = 4, Value = idPlanoAdquirido }
                };

            PlanoParcela objPlanoParcela;
            using (IDataReader dr = DataAccessLayer.ExecuteReader(trans, CommandType.Text, Spultimaparcelaporplanoadquirido, parms))
            {
                objPlanoParcela = new PlanoParcela();
                if (!SetInstance(dr, objPlanoParcela))
                    objPlanoParcela = null;
            }
            return objPlanoParcela;
        }
        #endregion

        #region CarregaParcelaAtualEmAbertoPorPlanoAdquirido
        public static PlanoParcela CarregaParcelaAtualEmAbertoPorPlanoAdquirido(PlanoAdquirido objPlanoAdquirido)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Plano_Adquirido", SqlDbType = SqlDbType.Int, Size = 4, Value = objPlanoAdquirido.IdPlanoAdquirido }
                };

            PlanoParcela objPlanoParcela;
            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spparcelaatualemabertoporplanoadquirido, parms))
            {
                objPlanoParcela = new PlanoParcela();
                if (!SetInstance(dr, objPlanoParcela))
                    objPlanoParcela = null;
            }
            return objPlanoParcela;
        }
        #endregion

        #region EncerrarPlanosParcelaPorPlanoAdquirido
        /// <summary>
        /// Metodo resposável por encerrar todos os planos parcela de um plano adquirido
        /// </summary>
        public static void EncerrarPlanosParcelaPorPlanoAdquirido(PlanoAdquirido objPlanoAdquirido)
        {
            using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();

                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        EncerrarPlanosParcelaPorPlanoAdquirido(objPlanoAdquirido, trans);

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

        public static void EncerrarPlanosParcelaPorPlanoAdquirido(PlanoAdquirido objPlanoAdquirido, SqlTransaction trans)
        {
            List<PlanoParcela> listPlanoParcela = ListaParcelasPorPlanoAdquirido(objPlanoAdquirido, trans);

            foreach (PlanoParcela objPlanoParcela in listPlanoParcela)
            {
                objPlanoParcela.FlagInativo = true;
                objPlanoParcela.Save(trans);
            }
        }
        #endregion

        #region RecarregarSMS
        /// <summary>
        /// Recarrega SMS 
        /// </summary>
        /// <param name="novaQtdeLiberada">quantidade de recarga antecipada (antes do pagamento da parcela)</param>
        /// <param name="trans">transação SQL Server</param>
        public void RecarregarSMS(int? novaQtdeLiberada, SqlTransaction trans = null)
        {
            int recarga;
            int qtdeJaLiberada = QuantidadeSMSLiberada.HasValue ? QuantidadeSMSLiberada.Value : 0;

            if (novaQtdeLiberada.HasValue)
            {
                recarga = novaQtdeLiberada.Value - qtdeJaLiberada;
                QuantidadeSMSLiberada = novaQtdeLiberada;

                if (null == trans)
                    Save();
                else
                    Save(trans);
            }
            else
                recarga = QuantidadeSMSTotal - qtdeJaLiberada;

            PlanoQuantidade.RecarregarSMS(PlanoAdquirido, recarga, trans);
        }
        #endregion

        #region LiberarOuBloquearSaldoTotalSMSDoPlano
        /// <summary>
        /// Libera ou bloqueia os SMSs do plano
        /// </summary>
        /// <param name="idPlanoAdquirido">objeto IdPlanoAdquirido</param>
        /// <param name="liberar">true para liberar, false para bloquear</param>
        /// <param name="trans">transação SQL Server</param>
        public static void LiberarOuBloquearSaldoTotalSMSDoPlano(int idPlanoAdquirido, bool liberar, SqlTransaction trans = null)
        {
            PlanoAdquirido objPlanoAdquirido =
                PlanoAdquirido.LoadObject(idPlanoAdquirido);

            PlanoQuantidade objPlanoQuantidade;
            if (!PlanoQuantidade.CarregarPorPlano(idPlanoAdquirido, out objPlanoQuantidade, trans))
                throw new InvalidOperationException("PlanoQuantidade para o PlanoAdquirido " + idPlanoAdquirido + " n�o foi encontrado");

            var todasParcelasEmAberto =
                PlanoParcela.ListaParcelasPorPlanoAdquirido(objPlanoAdquirido, trans)
                    .Where(parcela =>
                        parcela.PlanoParcelaSituacao.IdPlanoParcelaSituacao == (int)Enumeradores.PlanoParcelaSituacao.EmAberto);

            foreach (PlanoParcela parcela in todasParcelasEmAberto)
            {
                int valorAnterior = parcela.QuantidadeSMSLiberada.HasValue ? parcela.QuantidadeSMSLiberada.Value : 0;

                // novo valor a ser liberado
                parcela.QuantidadeSMSLiberada =
                    liberar ? parcela.QuantidadeSMSTotal : 0;

                if (null == trans)
                    parcela.Save();
                else
                    parcela.Save(trans);

                if (liberar)
                    objPlanoQuantidade.QuantidadeSMS += (parcela.QuantidadeSMSTotal - valorAnterior);
                else
                    objPlanoQuantidade.QuantidadeSMS -= valorAnterior;
            }

            if (null == trans)
                objPlanoQuantidade.Save();
            else
                objPlanoQuantidade.Save(trans);
        }

        #endregion

        #region RedistribuirQtdeSMSTotalNasParcelasEmAberto
        /// <summary>
        /// Pega o total de SMSs do plano, subtrai a qtde de SMSs ja pagas e divide o resultado ao longo das parcelas em aberto
        /// </summary>
        /// <param name="idPlanoAdquirido">objeto PlanoAdquirido</param>
        /// <param name="trans">transação SQL Server</param>
        public static void RedistribuirQtdeSMSTotalNasParcelasEmAberto(int idPlanoAdquirido, SqlTransaction trans = null)
        {
            PlanoAdquirido objPlanoAdquirido =
                PlanoAdquirido.LoadObject(idPlanoAdquirido);

            var todasParcelas =
                PlanoParcela.ListaParcelasPorPlanoAdquirido(objPlanoAdquirido, trans);

            var parcelasEmAberto =
                todasParcelas
                    .Where(parcela =>
                        (int)Enumeradores.PlanoParcelaSituacao.EmAberto == parcela.PlanoParcelaSituacao.IdPlanoParcelaSituacao);

            var parcelasPagas =
                todasParcelas
                    .Where(parcela =>
                        (int)Enumeradores.PlanoParcelaSituacao.Pago == parcela.PlanoParcelaSituacao.IdPlanoParcelaSituacao);

            int numeroParcelas = parcelasEmAberto.Count();

            if (numeroParcelas > 0)
            {
                objPlanoAdquirido.Plano.CompleteObject();

                int qtdeSMSDoPlano = objPlanoAdquirido.QuantidadeSMS;
                int qtdeSMSJaPaga = parcelasPagas.Sum(parcela =>
                    parcela.QuantidadeSMSTotal - (parcela.QuantidadeSMSLiberada.HasValue ? parcela.QuantidadeSMSLiberada.Value : 0));

                // cálculo da quantidade de SMS total por parcela
                int novaQtdeSMSPorParcela = (qtdeSMSDoPlano - qtdeSMSJaPaga) / numeroParcelas;

                foreach (PlanoParcela parcela in parcelasEmAberto)
                {
                    parcela.CompleteObject();
                    parcela.QuantidadeSMSTotal = novaQtdeSMSPorParcela;
                    if (null == trans)
                        parcela.Save();
                    else
                        parcela.Save(trans);
                }
            }
        }
        #endregion

        #region EstaLiberadoSMS

        public static bool EstaLiberadoSMS(int idPlanoAdquirido, SqlTransaction trans = null)
        {
            PlanoAdquirido objPlanoAdquirido =
                PlanoAdquirido.LoadObject(idPlanoAdquirido);

            PlanoQuantidade objPlanoQuantidade;
            PlanoQuantidade.CarregarPorPlano(idPlanoAdquirido, out objPlanoQuantidade, trans);

            var parcelasEmAberto =
                PlanoParcela.ListaParcelasPorPlanoAdquirido(objPlanoAdquirido, trans)
                    .Where(parcela =>
                        parcela.PlanoParcelaSituacao.IdPlanoParcelaSituacao == (int)Enumeradores.PlanoParcelaSituacao.EmAberto);

            var primeiraParcela =
                parcelasEmAberto.FirstOrDefault();

            if (null != primeiraParcela && primeiraParcela.QuantidadeSMSLiberada.HasValue)
            {
                int qtdeSMSLiberada = primeiraParcela.QuantidadeSMSLiberada.Value;

                return parcelasEmAberto
                    .All(parcela =>
                        parcela.QuantidadeSMSLiberada.HasValue &&
                        parcela.QuantidadeSMSLiberada.Value != 0 &&
                        parcela.QuantidadeSMSLiberada.Value == qtdeSMSLiberada) &&
                    parcelasEmAberto.Sum(parcela =>
                        parcela.QuantidadeSMSLiberada) <= objPlanoQuantidade.QuantidadeSMS;
            }

            return false;
        }

        #endregion

        #region CancelarOutrosPagamentos
        /// <summary>
        /// Cancela pagamentos diferentes do pagamento informado
        /// </summary>
        /// <param name="objPagamento">Pagamento informado</param>
        /// <param name="trans">transação SQL Server</param>
        public void CancelarOutrosPagamentos(Pagamento objPagamento, SqlTransaction trans = null)
        {
            //Cancela os outros pagamento relacionados a parcela atual que eventualmente estejam abertos
            var objListPagamentosEmAberto = Pagamento.CarregaPagamentosPorPlanoParcela(this.IdPlanoParcela, trans);
            var pagamentos = objListPagamentosEmAberto
                .Where(p => p.FlagInativo == false
                    && p.PagamentoSituacao.IdPagamentoSituacao == (int)Enumeradores.PagamentoSituacao.EmAberto
                    && p.IdPagamento != objPagamento.IdPagamento);

            foreach (var pagamentoEmAberto in pagamentos)
            {
                pagamentoEmAberto.PagamentoSituacao = new PagamentoSituacao((int)Enumeradores.PagamentoSituacao.Cancelado);
                pagamentoEmAberto.FlagInativo = true;

                if (null == trans)
                    pagamentoEmAberto.Save();
                else
                    pagamentoEmAberto.Save(trans);
            }
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
                    .Where(p => p.IdPlanoAdquirido != this.PlanoAdquirido.IdPlanoAdquirido);
            
            EnviarAtualizacaoCVSolr(objCurriculo.IdCurriculo);                           
            foreach (var objPlanoAdquirido in planosAguardandoLiberacao)
            {
                objPlanoAdquirido.CancelarPlanoAdquirido(null, "PlanoParcela > CancelarOutrosPlanosAdquiridos", trans, false, objCurriculo.IdCurriculo);
            }
        }

        /// <summary>
        /// Cancela planos adquiridos diferentes do informado
        /// </summary>
        /// <param name="objFilial"></param>
        /// <param name="trans"></param>
        public void CancelarOutrosPlanosAdquiridos(Filial objFilial, SqlTransaction trans = null)
        {
            var planosAguardandoLiberacao =
                PlanoAdquirido.CarregarPlanosAdquiridoPorSituacao(objFilial, (int)Enumeradores.PlanoSituacao.AguardandoLiberacao, trans)
                    .Where(p => p.IdPlanoAdquirido != this.PlanoAdquirido.IdPlanoAdquirido);

            foreach (var objPlanoAdquirido in planosAguardandoLiberacao)
            {
                objPlanoAdquirido.CancelarPlanoAdquirido(null, "PlanoParcela > CancelarOutrosPlanosAdquiridos", false);
            }
        }
        #endregion

        #region Liberar
        /// <summary>
        /// Liberar parcela
        /// </summary>
        /// <param name="objPagamento">objeto Pagamento referente ao pagamento da parcela</param>
        /// <param name="dtaPagamento">data do pagamento</param>
        /// <param name="trans">transação SQL Server</param>
        /// <returns>true se tudo correu bem, false se não for plano para pessoa fisica nem para pessoa juridica</returns>
        public bool Liberar(Pagamento objPagamento, DateTime dtaPagamento, SqlTransaction trans = null)
        {
            //Carregando objeto somente caso ele não tenha sido carregado
            if (this.PlanoAdquirido == null)
            {
            CompleteObject();
            }

            if (PlanoAdquirido.ParaPessoaFisica(trans))
            {
                Curriculo objCurriculo;
                Curriculo.LiberarVIP(PlanoAdquirido, out objCurriculo, trans);
                CancelarOutrosPagamentos(objPagamento, trans);
                CancelarOutrosPlanosAdquiridos(objCurriculo, trans);
            }
            else if (PlanoAdquirido.ParaPessoaJuridica(trans))
            {
                //Carregando objeto somente caso ele não tenha sido carregado
                if (String.IsNullOrEmpty(PlanoAdquirido.Plano.DescricaoPlano))
                {
                PlanoAdquirido.Plano.CompleteObject();
                }

                UsuarioFilial objUsuarioFilial;

                UsuarioFilial.LiberarCIA(PlanoAdquirido, this, out objUsuarioFilial, trans);
                CancelarOutrosPlanosAdquiridos(PlanoAdquirido.Filial, trans);
                RecarregarSMS(null, trans);    // libera SMSs da parcela
            }
            else
            {
                return false;
            }

            DataPagamento = dtaPagamento;
            PlanoParcelaSituacao = new PlanoParcelaSituacao((int)Enumeradores.PlanoParcelaSituacao.Pago);

            Save(trans);

            #region Integracao WFat
            // Notas serão geradas por robo
            ////Só integra com o financeiro se o valor não for zerado
            //if (objPagamento.ValorPagamento > 0)
            //{
            //    // Emissao de NF, Criacao contrato Wfat/ Contas à receber/ Baixa CR / Baixa CR / Conciliação caso boleto
            //    objPagamento.TipoPagamento.CompleteObject(trans);
            //    objPagamento.UsuarioFilialPerfil.CompleteObject(trans);
            //    EmitirNF(objPagamento, objPagamento.UsuarioFilialPerfil.IdUsuarioFilialPerfil, trans);
            //}

            #endregion

            return true;
        }
        #endregion

        #region CriarNovaParcela
        public static Pagamento CriarNovaParcela(PlanoAdquirido objPlanoAdquirido, int? numeroDesconto, DateTime dtaVencimento, DateTime dtaEnvioBoleto, TipoPagamento objTipoPagamento, CodigoDesconto objCodigoDesconto, SqlTransaction trans)
        {
            Plano objPlano = objPlanoAdquirido.Plano;

            var objPlanoParcela = new PlanoParcela
            {
                PlanoAdquirido = objPlanoAdquirido,
                ValorParcela = objPlanoAdquirido.ValorBase,
                //Se for cortesia gerar parcela com a situação pago
                PlanoParcelaSituacao = objPlanoAdquirido.ValorBase > 0 ? new PlanoParcelaSituacao((int)Enumeradores.PlanoParcelaSituacao.EmAberto) : new PlanoParcelaSituacao((int)Enumeradores.PlanoParcelaSituacao.Pago),
                NumeroDesconto = numeroDesconto,
                FlagInativo = false,
                QuantidadeSMSTotal = objPlanoAdquirido.QuantidadeSMS / (objPlano.QuantidadeParcela == 0 ? 1 : objPlano.QuantidadeParcela)
            };

            objPlanoParcela.Save(trans);

            return objPlanoParcela.CriarPagamento(dtaEnvioBoleto, dtaVencimento, objTipoPagamento, objCodigoDesconto, trans);
        }
        #endregion

        public Pagamento CriarPagamento(DateTime dataEmissaoBoleto, DateTime dataVencimentoBoleto, TipoPagamento objTipoPagamento, CodigoDesconto objCodigoDesconto, SqlTransaction trans)
        {
            this.PlanoAdquirido.CompleteObject(trans);

            var objPagamento = new Pagamento
            {
                DataEmissao = dataEmissaoBoleto,
                DataVencimento = dataVencimentoBoleto,
                PlanoParcela = this,
                TipoPagamento = objTipoPagamento,
                //Se for cortesia gerar o pagamento com a situação pago
                PagamentoSituacao = this.PlanoAdquirido.ValorBase > 0 ? new PagamentoSituacao((int)Enumeradores.PagamentoSituacao.EmAberto) : new PagamentoSituacao((int)Enumeradores.PagamentoSituacao.Pago),
                UsuarioFilialPerfil = this.PlanoAdquirido.UsuarioFilialPerfil,
                Filial = this.PlanoAdquirido.Filial,
                FlagAvulso = false,
                FlagInativo = false,
                CodigoDesconto = objCodigoDesconto
            };

            if (this.PlanoAdquirido.ParaPessoaJuridica())
            {
                //this.PlanoAdquirido.Filial.CompleteObject();
                bool flgISS;
                string textoNF;
                int idCidade;
                decimal valorLiquido;

                Filial.RecuperarInfoISS(this.PlanoAdquirido.Filial.IdFilial, out flgISS, out textoNF, out idCidade);

                if (flgISS)
                    CalcularISS(idCidade, this.PlanoAdquirido.ValorBase, out valorLiquido);
                else
                    if (this.NumeroDesconto.HasValue)
                        valorLiquido = this.ValorParcela - (this.ValorParcela * this.NumeroDesconto.Value / 100);
                    else
                        valorLiquido = this.ValorParcela;

                objPagamento.ValorPagamento = valorLiquido;
            }
            else
            {
                if (this.NumeroDesconto.HasValue)
                    objPagamento.ValorPagamento = this.ValorParcela - (this.ValorParcela * this.NumeroDesconto.Value / 100);
                else
                    objPagamento.ValorPagamento = this.ValorParcela;
            }

            objPagamento.Save(trans);

            return objPagamento;
        }

        #region #EnviarAtualizacaoCVSolr
        public static void EnviarAtualizacaoCVSolr(int idCurriculo)        {

            string urlSLOR = Parametro.RecuperaValorParametro(Enumeradores.Parametro.URLSolrAtualizaCV);
            if (!String.IsNullOrEmpty(urlSLOR))
            {
                urlSLOR = urlSLOR + idCurriculo;
                WebRequest request = WebRequest.Create(urlSLOR);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            }

        }
        #endregion

        #region ListarParcelasEmissaoNotaFiscalAntecipada
        public static DataTable ListarParcelasEmissaoNotaFiscalAntecipada(DateTime? considerarData)
        {
            if (!considerarData.HasValue)
            {
                considerarData = DateTime.Now;
            }

            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(Spnotasantecipadas.Replace("{data}", considerarData.Value.ToString("yyyy-MM-dd")), DataAccessLayer.CONN_STRING);
            da.Fill(dt);
            return dt;
        }
        #endregion

        #region ListarParcelasVencimentoCredito
        public static List<Tuple<int, int>> ListarParcelasVencimentoCredito()
        {
            var valores = new List<Tuple<int, int>>();
            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spcreditopagamentos, new List<SqlParameter>(0)))
            {
                while (dr.Read())
                {
                    int IdPlanoAdquirido;
                    int IdPagamento;

                    if (!int.TryParse(dr["Idf_Plano_Adquirido"].ToString(), out IdPlanoAdquirido))
                    {
                        EL.GerenciadorException.GravarExcecao(new Exception("VencimentoParcelasCredito: Plano Adquirido inválido"));
                        continue;
                    }
                    if (!int.TryParse(dr["Idf_Pagamento"].ToString(), out IdPagamento))
                    {
                        EL.GerenciadorException.GravarExcecao(new Exception("VencimentoParcelasCredito: Pagamento inválido"));
                        continue;
                    }
                    valores.Add(Tuple.Create<int, int>(IdPlanoAdquirido, IdPagamento));
                }
            }
            return valores;
        }
        #endregion

        #region PrimeiraParcela
        /// <summary>
        /// Verifica qual o número da parcela do Plano Adquirido.
        /// </summary>
        /// <returns>Número da parcela. Se não encontrado, retorna um número negativo.</returns>
        public Int32 NumeroParcela()
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Plano_Parcela", SqlDbType = SqlDbType.Int, Size = 4, Value = this.IdPlanoParcela }
                };

            object o = DataAccessLayer.ExecuteScalar(CommandType.Text, SP_NUMERO_PARCELA, parms);

            return (o == null || DBNull.Value == o) ? -1 : Convert.ToInt16(o);
        }

        #endregion

        #region EmitirNF
        public static bool EmitirNF(Pagamento objPagamento, int idUsuarioLogado, SqlTransaction trans)
        {
            var objUsuarioLogado = UsuarioFilialPerfil.LoadObject(idUsuarioLogado, trans);
            objUsuarioLogado.PessoaFisica.CompleteObject(trans);

            return EmitirNF(objPagamento, objUsuarioLogado, trans);
        }

        public static bool EmitirNF(Pagamento objPagamento, UsuarioFilialPerfil objUsuarioLogado, SqlTransaction trans)
        {
            #region Propriedades Integracao
            string desIdentificador, numCPF, numCNPJ, razaoSocial, cep, rua, complemento, bairro, cidade, uf, nomeFantasia, idfCnaePrincipal, emailContato, ddd, telefone, nomeContato, filialGestora;
            DateTime dataPagamento, dataInicioPlano, dataFimPLano;
            decimal valorPagamento;
            int numEndereco, numBanco;
            var objWsIntegracao = new wsIntegracaoWFat.IntegracaoFaturamentoFinanceiro();
            #endregion

            bool retorno = false;
            bool baixaNota = false;

            #region Verificando o carregamento dos objetos necessários à execução
            if (objPagamento.PlanoParcela.PlanoAdquirido == null)
                objPagamento.PlanoParcela.CompleteObject(trans);
            if (objPagamento.PlanoParcela.PlanoAdquirido.Plano == null)
                objPagamento.PlanoParcela.PlanoAdquirido.CompleteObject(trans);
            if (objPagamento.PlanoParcela.PlanoAdquirido.Plano.TipoContrato == null)
                objPagamento.PlanoParcela.PlanoAdquirido.Plano.CompleteObject();
            #endregion Verificando o carregamento dos objetos necessários à execução

            var emailRemetente = Parametro.RecuperaValorParametro(Enumeradores.Parametro.EmailRemetenteEnvioNF, trans);
            var nomeRemetente = Parametro.RecuperaValorParametro(Enumeradores.Parametro.NomeRemetenteEnvioNF, trans);

            if (objPagamento.PlanoParcela.PlanoAdquirido.ParaPessoaJuridica(trans))
            {
                if (Filial.RecuperarInformacoesIntegracaoFinanceiro(objPagamento.IdPagamento, out dataPagamento, out valorPagamento, out desIdentificador, out dataInicioPlano, out dataFimPLano, out numCNPJ, out razaoSocial, out cep, out rua, out numEndereco, out complemento, out bairro, out cidade, out uf, out nomeFantasia, out idfCnaePrincipal, out emailContato, out ddd, out telefone, out nomeContato, out numBanco, out filialGestora, trans))
                {
                    string assunto;
                    string mensagem = CartaEmail.RetornarConteudoBNE(Enumeradores.CartaEmail.MensagemCiaNF, out assunto);

                    var tipoTransacaoWfat = new wsIntegracaoWFat.TipoTransacao();

                    // Adaptando Id Tipo Pagamento de acordo com WFat
                    switch (objPagamento.TipoPagamento.IdTipoPagamento)
                    {
                        case (int)Enumeradores.TipoPagamento.BoletoBancario:
                            tipoTransacaoWfat = wsIntegracaoWFat.TipoTransacao.Boleto;
                            break;
                        case (int)Enumeradores.TipoPagamento.CartaoCredito:
                            tipoTransacaoWfat = wsIntegracaoWFat.TipoTransacao.CartaoCredito;
                            break;
                        case (int)Enumeradores.TipoPagamento.DebitoOnline:
                            tipoTransacaoWfat = wsIntegracaoWFat.TipoTransacao.CartaoDebito;
                            break;
                        case (int)Enumeradores.TipoPagamento.DepositoIdentificado:
                            tipoTransacaoWfat = wsIntegracaoWFat.TipoTransacao.Deposito;
                            break;
                        case (int)Enumeradores.TipoPagamento.Parceiro:
                            tipoTransacaoWfat = wsIntegracaoWFat.TipoTransacao.Parceiro;
                            break;
                        case (int)Enumeradores.TipoPagamento.PagSeguro:
                            tipoTransacaoWfat = wsIntegracaoWFat.TipoTransacao.PagSeguro;
                            break;
                        case (int)Enumeradores.TipoPagamento.PayPal:
                            tipoTransacaoWfat = wsIntegracaoWFat.TipoTransacao.PayPal;
                            break;
                        case (int)Enumeradores.TipoPagamento.DebitoRecorrente:
                            tipoTransacaoWfat = wsIntegracaoWFat.TipoTransacao.DebitoRecorrente;
                            break;
                    }

                    mensagem = mensagem.Replace("{nome do plano}", objPagamento.PlanoParcela.PlanoAdquirido.Plano.DescricaoPlano);

                    #region InfoISS
                    bool flgISS;
                    int idCidade;
                    string textoPersonalizado;
                    objPagamento.PlanoParcela.PlanoAdquirido.Filial.CompleteObject(trans);
                    Filial.RecuperarInfoISS(objPagamento.PlanoParcela.PlanoAdquirido.Filial.IdFilial, out flgISS, out textoPersonalizado, out idCidade, trans);
                    #endregion

                    ContaBancaria objContaBancaria = ContaBancaria.RecuperarContaBancariaDeTipoPagamento((Enumeradores.TipoPagamento)objPagamento.TipoPagamento.IdTipoPagamento, (Enumeradores.Banco)numBanco);

                    var descricaoIdentificador = string.IsNullOrWhiteSpace(objPagamento.DescricaoIdentificador) ? objPagamento.IdPagamento.ToString() : objPagamento.DescricaoIdentificador;

                    try
                    {
                        baixaNota = objWsIntegracao.IntegrarFaturamentoFinanceiro(wsIntegracaoWFat.SistemaOrigem.BNE, dataPagamento == default(DateTime) ? new DateTime?() : new DateTime?(dataPagamento), valorPagamento, tipoTransacaoWfat, descricaoIdentificador, (wsIntegracaoWFat.TipoContrato)objPagamento.PlanoParcela.PlanoAdquirido.Plano.TipoContrato.IdfTipoContrato, dataInicioPlano, dataFimPLano, wsIntegracaoWFat.TipoDocumento.CNPJ, numCNPJ, razaoSocial, cep, rua, numEndereco, complemento, bairro, cidade, uf, nomeFantasia, idfCnaePrincipal, emailContato, ddd, telefone, nomeContato, objUsuarioLogado.PessoaFisica.CPF, objUsuarioLogado.PessoaFisica.NomePessoa, emailRemetente, nomeRemetente, assunto, mensagem, Convert.ToDateTime(objPagamento.DataVencimento), textoPersonalizado, objPagamento.PlanoParcela.ValorParcela, "", filialGestora, objContaBancaria.DescricaoAgencia, objContaBancaria.DescricaoConta, objContaBancaria.Banco.IdBanco.ToString());
                    }
                    finally
                    {
                        objWsIntegracao.Dispose();
                    }

                    if (baixaNota && dataPagamento != new DateTime())
                    {
                        objPagamento.FlagBaixado = true;
                        objPagamento.Save(trans);
                    }

                    retorno = baixaNota;

                }
            }
            else
            {
                if (PessoaFisica.RecuperarInformacoesIntegracaoFinanceiro(objPagamento.IdPagamento, out dataPagamento, out valorPagamento, out desIdentificador, out dataInicioPlano, out dataFimPLano, out numCPF, out nomeContato, out cep, out rua, out numEndereco, out complemento, out bairro, out cidade, out uf, out emailContato, out ddd, out telefone, out numBanco, out filialGestora, trans))
                {
                    string assunto;
                    string mensagem = CartaEmail.RetornarConteudoBNE(Enumeradores.CartaEmail.MensagemVipNF, out assunto);

                    var tipoTransacaoWfat = new wsIntegracaoWFat.TipoTransacao();

                    // Adaptando Id Tipo Pagamento de acordo com WFat
                    switch (objPagamento.TipoPagamento.IdTipoPagamento)
                    {
                        case (int)Enumeradores.TipoPagamento.BoletoBancario:
                            tipoTransacaoWfat = wsIntegracaoWFat.TipoTransacao.Boleto;
                            break;
                        case (int)Enumeradores.TipoPagamento.CartaoCredito:
                            tipoTransacaoWfat = wsIntegracaoWFat.TipoTransacao.CartaoCredito;
                            break;
                        case (int)Enumeradores.TipoPagamento.DebitoOnline:
                            tipoTransacaoWfat = wsIntegracaoWFat.TipoTransacao.CartaoDebito;
                            break;
                        case (int)Enumeradores.TipoPagamento.DepositoIdentificado:
                            tipoTransacaoWfat = wsIntegracaoWFat.TipoTransacao.Deposito;
                            break;
                        case (int)Enumeradores.TipoPagamento.Parceiro:
                            tipoTransacaoWfat = wsIntegracaoWFat.TipoTransacao.Parceiro;
                            break;
                        case (int)Enumeradores.TipoPagamento.PagSeguro:
                            tipoTransacaoWfat = wsIntegracaoWFat.TipoTransacao.PagSeguro;
                            break;
                        case (int)Enumeradores.TipoPagamento.PayPal:
                            tipoTransacaoWfat = wsIntegracaoWFat.TipoTransacao.PayPal;
                            break;
                        case (int)Enumeradores.TipoPagamento.DebitoRecorrente:
                            tipoTransacaoWfat = wsIntegracaoWFat.TipoTransacao.DebitoRecorrente;
                            break;
                    }

                    var idTipoContrato = objPagamento.PlanoParcela.PlanoAdquirido.Plano.TipoContrato.IdfTipoContrato;

                    if (string.IsNullOrEmpty(emailContato))
                        emailContato = Parametro.RecuperaValorParametro(Enumeradores.Parametro.EmailContatoNotaFiscal, trans);

                    ContaBancaria objContaBancaria = ContaBancaria.RecuperarContaBancariaDeTipoPagamento((Enumeradores.TipoPagamento)objPagamento.TipoPagamento.IdTipoPagamento, (Enumeradores.Banco)numBanco);

                    var descricaoIdentificador = string.IsNullOrWhiteSpace(objPagamento.DescricaoIdentificador) ? objPagamento.IdPagamento.ToString() : objPagamento.DescricaoIdentificador;

                    try
                    {
                        baixaNota = objWsIntegracao.IntegrarFaturamentoFinanceiro(wsIntegracaoWFat.SistemaOrigem.BNE, dataPagamento == default(DateTime) ? new DateTime?() : new DateTime?(dataPagamento), valorPagamento, tipoTransacaoWfat, descricaoIdentificador, (wsIntegracaoWFat.TipoContrato)idTipoContrato.GetHashCode(), dataInicioPlano, dataFimPLano, wsIntegracaoWFat.TipoDocumento.CPF, numCPF, nomeContato, cep, rua, numEndereco, complemento, bairro, cidade, uf, "", "", emailContato, ddd, telefone, nomeContato, objUsuarioLogado.PessoaFisica.CPF, objUsuarioLogado.PessoaFisica.NomePessoa, emailRemetente, nomeRemetente, assunto, mensagem, Convert.ToDateTime(objPagamento.DataVencimento), "", objPagamento.PlanoParcela.ValorParcela, "", filialGestora, objContaBancaria.DescricaoAgencia, objContaBancaria.DescricaoConta, objContaBancaria.Banco.IdBanco.ToString());
                    }
                    catch (Exception ex)
                    {
                        EL.GerenciadorException.GravarExcecao(ex);
                    }
                    finally
                    {
                        objWsIntegracao.Dispose();
                    }

                    if (baixaNota && dataPagamento != new DateTime())
                    {
                        objPagamento.FlagBaixado = true;
                        objPagamento.Save(trans);
                    }

                    retorno = baixaNota;

                    if (retorno)
                    {
                        EnviarAtualizacaoCVSolr(Curriculo.RecuperarIdPorPessoaFisica(objUsuarioLogado.PessoaFisica, trans));   
                        DispararGatilhoVipAdquirido(objUsuarioLogado, objPagamento, objUsuarioLogado, dataInicioPlano, dataFimPLano, trans);
                    }
                }
            }

            return retorno;
        }

        private static void DispararGatilhoVipAdquirido(UsuarioFilialPerfil objUsuarioFilialPerfil, Pagamento objPagamento, UsuarioFilialPerfil objUsuarioLogado, DateTime inicio, DateTime fim, SqlTransaction trans = null)
        {
            try
            {
                var curriculoId = Curriculo.RecuperarIdPorPessoaFisica(objUsuarioLogado.PessoaFisica, trans);

                var parametros = new ParametroExecucaoCollection
                            {
                                new ParametroExecucao
                                {
                                    Parametro = "IdTipoGatilho",
                                    DesParametro = "IdTipoGatilho",
                                    Valor = ((int)Enumeradores.TipoGatilho.CompraAprovadaVip).ToString()
                                },
                                new ParametroExecucao
                                {
                                    Parametro = "IdCurriculo",
                                    DesParametro = "IdCurriculo",
                                    Valor = curriculoId.ToString()    
                                },
                                new ParametroExecucao
                                {
                                    Parametro = "IdPlanoAdquirido",
                                    DesParametro = "IdPlanoAdquirido",
                                    Valor = objPagamento.PlanoParcela.PlanoAdquirido.IdPlanoAdquirido.ToString()
                                },
                                new ParametroExecucao
                                {
                                    Parametro = "IdPlano",
                                    DesParametro = "IdPlano",
                                    Valor = objPagamento.PlanoParcela.PlanoAdquirido.Plano.IdPlano.ToString()

                                },
                                new ParametroExecucao
                                {
                                    Parametro = "IdPagamento",
                                    DesParametro = "IdPagamento",
                                    Valor = objPagamento.IdPagamento.ToString()
                                },
                                new ParametroExecucao
                                {
                                    Parametro = "DataInicioPlano",
                                    DesParametro = "DataInicioPlano",
                                    Valor = inicio.ToString("dd/MM/yyyy HH:mm")
                                },
                                new ParametroExecucao
                                {
                                    Parametro = "DataFimPlano",
                                    DesParametro = "DataFimPlano",
                                    Valor =  fim.ToString("dd/MM/yyyy HH:mm")
                                },
                                new ParametroExecucao
                                {
                                    Parametro = "IdUsuarioFilialPerfil",
                                    DesParametro = "IdUsuarioFilialPerfil",
                                    Valor = objUsuarioFilialPerfil.IdUsuarioFilialPerfil.ToString()
                                }
                            };

                ProcessoAssincrono.IniciarAtividade(
                    BNE.BLL.AsyncServices.Enumeradores.TipoAtividade.GatilhosBNE,
                    BNE.BLL.AsyncServices.PluginsCompatibilidade.CarregarPorMetadata("GatilhosEmail", "PluginSaidaGatilhos"),
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

        #endregion

        #region RecuperarInformacoesNF
        public static void RecuperarInformacoesNF(int idPagamento, out string infoUm, out string infoDois, out string infoTres, out string infoQuatro, out string infoCinco, out string infoSeis, out string infoSete, out string infoOito)
        {
            infoUm = string.Empty;
            infoDois = string.Empty;
            infoTres = string.Empty;
            infoQuatro = string.Empty;
            infoCinco = string.Empty;
            infoSeis = string.Empty;
            infoSete = string.Empty;
            infoOito = string.Empty;

            using (wsIntegracaoWFat.IntegracaoFaturamentoFinanceiro objWsIntegracao = new wsIntegracaoWFat.IntegracaoFaturamentoFinanceiro())
            {
                var objPagamento = BLL.Pagamento.LoadObject(idPagamento);

                objPagamento.PlanoParcela.CompleteObject();
                objPagamento.PlanoParcela.PlanoAdquirido.CompleteObject();

                if (objPagamento.PlanoParcela.PlanoAdquirido.ParaPessoaFisica())
                {
                    objPagamento.PlanoParcela.PlanoAdquirido.UsuarioFilialPerfil.CompleteObject();
                    objPagamento.PlanoParcela.PlanoAdquirido.UsuarioFilialPerfil.PessoaFisica.CompleteObject();

                    var numeroCPF = objPagamento.PlanoParcela.PlanoAdquirido.UsuarioFilialPerfil.PessoaFisica.NumeroCPF.Replace(".", "").Replace("-", "").Replace("/", "");

                    DataTable retorno = objWsIntegracao.ConsultarIntegracaoHistorico(wsIntegracaoWFat.TipoTransacao.Boleto, objPagamento.DescricaoIdentificador, wsIntegracaoWFat.TipoDocumento.CPF, numeroCPF);

                    if (retorno.Rows.Count > 0)
                    {

                        infoUm = retorno.Rows[0][0].ToString();
                        infoDois = retorno.Rows[0][1].ToString();
                        infoTres = retorno.Rows[0][2].ToString();
                        infoQuatro = retorno.Rows[0][3].ToString();
                        infoCinco = retorno.Rows[0][4].ToString();
                        infoSeis = retorno.Rows[0][5].ToString();
                        infoSete = retorno.Rows[0][6].ToString();
                        infoOito = retorno.Rows[0][7].ToString();
                    }
                }
                else
                {
                    objPagamento.PlanoParcela.PlanoAdquirido.Filial.CompleteObject();

                    var numeroCNPJ = objPagamento.PlanoParcela.PlanoAdquirido.Filial.CNPJ.Replace(".", "").Replace("-", "").Replace("/", "");

                    DataTable retorno = objWsIntegracao.ConsultarIntegracaoHistorico(wsIntegracaoWFat.TipoTransacao.Boleto, objPagamento.DescricaoIdentificador, wsIntegracaoWFat.TipoDocumento.CNPJ, numeroCNPJ);

                    if (retorno.Rows.Count > 0)
                    {

                        infoUm = retorno.Rows[0][0].ToString();
                        infoDois = retorno.Rows[0][1].ToString();
                        infoTres = retorno.Rows[0][2].ToString();
                        infoQuatro = retorno.Rows[0][3].ToString();
                        infoCinco = retorno.Rows[0][4].ToString();
                        infoSeis = retorno.Rows[0][5].ToString();
                    }
                }
            };
        }
        #endregion

        #region CalcularISS
        public static void CalcularISS(int idCidade, decimal valorBruto, out decimal valorLiquido)
        {
            valorLiquido = 0;

            decimal taxaISS;
            decimal desconto;

            Cidade.RecuperarTaxaISS(idCidade, out taxaISS);

            desconto = (valorBruto * taxaISS) / 100;

            valorLiquido = valorBruto - desconto;

        }
        #endregion

        #region SetInstance_NonDispose
        public static bool SetInstance_NonDispose(IDataReader dr, PlanoParcela objPlanoParcela)
        {
            objPlanoParcela._idPlanoParcela = Convert.ToInt32(dr["Idf_Plano_Parcela"]);
            objPlanoParcela._planoAdquirido = new PlanoAdquirido(Convert.ToInt32(dr["Idf_Plano_Adquirido"]));
            objPlanoParcela._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
            if (dr["Dta_Pagamento"] != DBNull.Value)
                objPlanoParcela._dataPagamento = Convert.ToDateTime(dr["Dta_Pagamento"]);
            objPlanoParcela._valorParcela = Convert.ToDecimal(dr["Vlr_Parcela"]);
            objPlanoParcela._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
            objPlanoParcela._planoParcelaSituacao = new PlanoParcelaSituacao(Convert.ToInt32(dr["Idf_Plano_Parcela_Situacao"]));
            if (dr["Num_Desconto"] != DBNull.Value)
                objPlanoParcela._numeroDesconto = Convert.ToInt32(dr["Num_Desconto"]);
            objPlanoParcela._quantidadeSMSTotal = Convert.ToInt32(dr["Qtd_SMS_Total"]);
            if (dr["Qtd_SMS_Liberada"] != DBNull.Value)
                objPlanoParcela._quantidadeSMSLiberada = Convert.ToInt32(dr["Qtd_SMS_Liberada"]);

            objPlanoParcela._persisted = true;
            objPlanoParcela._modified = false;

            return true;
        }

        #endregion

        #endregion

    }
}