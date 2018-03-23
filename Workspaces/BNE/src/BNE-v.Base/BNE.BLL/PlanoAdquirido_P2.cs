//-- Data: 08/04/2010 11:17
//-- Autor: Gieyson Stelmak

using BNE.EL;
using BNE.Services.Base.ProcessosAssincronos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net;

namespace BNE.BLL
{
    public partial class PlanoAdquirido // Tabela: BNE_Plano_Adquirido
    {

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
                C.Idf_Curriculo,
		        UFP.Idf_Perfil
        FROM    BNE_Curriculo C WITH(NOLOCK)
                JOIN TAB_Pessoa_Fisica PF WITH(NOLOCK) ON C.Idf_Pessoa_Fisica = PF.Idf_Pessoa_Fisica
                JOIN TAB_Usuario_Filial_Perfil UFP WITH(NOLOCK) ON PF.Idf_Pessoa_Fisica = UFP.Idf_Pessoa_Fisica
                JOIN BNE_Plano_Adquirido PA WITH(NOLOCK) ON UFP.Idf_Usuario_Filial_Perfil = PA.Idf_Usuario_Filial_Perfil
		        JOIN TAB_Perfil P WITH(NOLOCK) ON P.Idf_Perfil = UFP.Idf_Perfil
        WHERE   CONVERT(VARCHAR, PA.Dta_Fim_Plano, 112) < CONVERT(VARCHAR, GETDATE(), 112)
                AND PA.Idf_Plano_Situacao = 1 
                AND PA.Idf_Filial IS NULL AND
		        P.Idf_Tipo_Perfil = 1";
        #endregion

        #region Spretornaplanosvencidospj
        private const string Spretornaplanosvencidospj = @"
        SELECT  PA.*
        FROM    BNE_Plano_Adquirido PA WITH(NOLOCK)
        WHERE   CONVERT(VARCHAR, PA.Dta_Fim_Plano, 112) < CONVERT(VARCHAR, GETDATE(), 112)
                AND PA.Idf_Plano_Situacao = 1 /* Liberado */
                AND PA.Idf_Filial IS NOT NULL";
        #endregion

        #region Spcurriculovipcomplanoencerrado
        private const string Spcurriculovipcomplanoencerrado = @"
        SELECT  COUNT(*)
        FROM    BNE.BNE_Curriculo cv WITH ( NOLOCK )
        JOIN BNE.TAB_Pessoa_Fisica pf WITH ( NOLOCK ) ON pf.Idf_Pessoa_Fisica = cv.Idf_Pessoa_Fisica
        OUTER APPLY ( SELECT TOP 1
                                pa.*
                      FROM      BNE.BNE_Plano_Adquirido pa WITH ( NOLOCK )
                                JOIN BNE.TAB_Usuario_Filial_Perfil ufp WITH ( NOLOCK ) ON ufp.Idf_Usuario_Filial_Perfil = pa.Idf_Usuario_Filial_Perfil AND UFP.Idf_Filial IS NULL
                      WHERE     pf.Idf_Pessoa_Fisica = ufp.Idf_Pessoa_Fisica
                                AND PA.Idf_Plano_Situacao = 1
                      ORDER BY  pa.Dta_Inicio_Plano DESC
                    ) pl
        WHERE   pl.Idf_Plano_Adquirido IS NULL
                AND Flg_VIP = 1";
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
        FROM    BNE_Plano_Adquirido PA
		        LEFT JOIN BNE_Plano_Parcela PP ON PA.Idf_Plano_Adquirido = PP.Idf_Plano_Adquirido
		        CROSS APPLY ( SELECT TOP 1 * FROM BNE_Pagamento P WHERE pp.Idf_Plano_Parcela = P.Idf_Plano_Parcela ORDER BY P.Dta_Vencimento DESC ) AS P
        WHERE   PA.Idf_Filial IS NULL
                AND PA.Idf_Plano_Situacao = 0 
                AND DATEADD(DAY, @QuantidadeDias, P.Dta_Vencimento) < CONVERT(VARCHAR(10),GETDATE(),112)
        ";
        #endregion

        #region SP_RETORNA_PLANOS_LIBERACAO_FUTURA_PARA_ATUALIZACAO
        private const string SP_RETORNA_PLANOS_LIBERACAO_FUTURA_PARA_ATUALIZACAO = @"
        SELECT parcelas_pagas.count AS num_parcelas_pagas, pa.* FROM BNE.BNE_Plano_Adquirido pa
        OUTER APPLY (
				        SELECT COUNT(*) AS count
				        FROM BNE.BNE_Plano_Parcela pp
				        WHERE pp.Idf_Plano_Adquirido = pa.Idf_Plano_Adquirido
					        AND pp.Idf_Plano_Parcela_Situacao = 2 --Pago
			        ) AS parcelas_pagas
        OUTER APPLY (
	        SELECT COUNT(*) AS count
	        FROM BNE.BNE_Plano_Adquirido pa2
	        WHERE pa2.Idf_Plano_Adquirido <> pa.Idf_Plano_Adquirido
		        AND (pa2.Idf_Filial = pa.Idf_Filial OR pa2.Idf_Usuario_Filial_Perfil = pa.Idf_Usuario_Filial_Perfil)
		        AND pa2.Idf_Plano_Situacao = 1 --Liberado
        ) AS planos_adquiridos_liberados
        WHERE	pa.Idf_Plano_Situacao = 5 --Liberação Futura
		        AND planos_adquiridos_liberados.count <= 0
		        AND pa.Dta_Inicio_Plano <= GETDATE()";
        #endregion

        #region Spselectexisteplanoadquiridoliberadofilial
        private const string Spselectexisteplanoadquiridoliberadofilial = @"
            SELECT  COUNT(PA.Idf_Plano_Adquirido)
            FROM    BNE_Plano_Adquirido PA WITH(NOLOCK)
                    INNER JOIN TAB_Usuario_Filial_Perfil UFP WITH(NOLOCK) ON PA.Idf_Usuario_Filial_Perfil = UFP.Idf_Usuario_Filial_Perfil
                    INNER JOIN bne_Plano_parcela PP WITH(NOLOCK) on PP.idf_plano_adquirido = PA.idf_Plano_Adquirido
            WHERE   PA.Idf_Filial = @Idf_Filial
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

        #region SpQuantidadeNotaFiscalAntecipadaNaoEnviada
        private const string SpQuantidadeNotaFiscalAntecipadaNaoEnviada = @"
        SELECT COUNT(*)
         FROM   [BNE].BNE_Pagamento pg WITH ( NOLOCK )
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
                        AND pa.Idf_Plano_Situacao = 0
                      ) -- Primeira parcela do plano: não existe parcela paga anterior e plano aguardando liberação
                      OR ( ultima_parcela_paga.Idf_Tipo_Pagamento = 2
                           AND pa.Idf_Plano_Situacao = 1
                         ) --Ultima parcela paga por boleto e plano já liberado
                    )
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

        #region Spselectexisteplanoadquiridoliberadofilialelegivel1clique
        private const string Spselectexisteplanoadquiridoliberadofilialelegivel1clique = @"
        SELECT  COUNT(PA.Idf_Plano_Adquirido)
        FROM    BNE_Plano_Adquirido PA WITH(NOLOCK)
                INNER JOIN TAB_Usuario_Filial_Perfil UFP WITH(NOLOCK) ON PA.Idf_Usuario_Filial_Perfil = UFP.Idf_Usuario_Filial_Perfil
                INNER JOIN bne_Plano_parcela PP WITH(NOLOCK) on PP.idf_plano_adquirido = PA.idf_Plano_Adquirido
                INNER JOIN bne.BNE_Plano P WITH(NOLOCK) ON P.Idf_Plano = PA.Idf_Plano AND P.Idf_Plano_Tipo = 2 AND P.Qtd_Parcela > 1 AND P.Flg_Inativo = 0 AND P.Qtd_Visualizacao > 0
        WHERE   PA.Idf_Filial = @Idf_Filial
                AND PA.Idf_Plano_Situacao = 1";
        #endregion

        #endregion

        #region Métodos

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
        /// <summary>
        /// Metodo utilizado pelo robô(service) atualizar plano PJ
        /// </summary>
        public static void AtualizarPlanoPJ()
        {
            try
            {
                var lista = new List<PlanoAdquirido>();

                using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spretornaplanosvencidospj, null))
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
                        objPlanoAdquirido.EncerrarPlanoAdquirido();
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

        #endregion

        #region EncerrarPlanoAdquiridoPessoaFisica
        /// <summary>
        /// Metodo resposável por encerrar o plano adquirido de pessoa fisica
        /// </summary>
        public static void EncerrarPlanoAdquiridoPessoaFisica(PlanoAdquirido objPlanoAdquirido, Curriculo objCurriculo, UsuarioFilialPerfil objUsuarioFilialPerfil)
        {
            bool success = false;
            using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();

                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        EncerrarPlanoAdquiridoPessoaFisica(objPlanoAdquirido, objCurriculo, objUsuarioFilialPerfil, trans);

                        trans.Commit();
                        success = true;
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            if (success)
            {
                string urlSLOR = Parametro.RecuperaValorParametro(Enumeradores.Parametro.URLSolrAtualizaCV);                
                if (!String.IsNullOrEmpty(urlSLOR))
                {
                    urlSLOR = urlSLOR + objCurriculo.IdCurriculo.ToString();
                    WebRequest request = WebRequest.Create(urlSLOR);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                }
                DispararGatilhoFimPlano(objPlanoAdquirido, objCurriculo, objUsuarioFilialPerfil);                
            }
        }

        private static void DispararGatilhoFimPlano(PlanoAdquirido objPlanoAdquirido, Curriculo objCurriculo, UsuarioFilialPerfil objUsuarioFilialPerfil)
        {
            try
            {
                var parametros = new ParametroExecucaoCollection()
                        {
                            new ParametroExecucao
                            {
                                Parametro = "IdTipoGatilho",
                                DesParametro = "IdTipoGatilho",
                                Valor = ((int)Enumeradores.TipoGatilho.AcabouVip).ToString()
                            },
                            new ParametroExecucao
                            {
                                Parametro = "IdCurriculo",
                                DesParametro = "IdCurriculo",
                                Valor = objCurriculo.IdCurriculo.ToString()    
                            },
                            new ParametroExecucao
                            {
                                Parametro = "IdPlanoAdquirido",
                                DesParametro = "IdPlanoAdquirido",
                                Valor = objPlanoAdquirido.IdPlanoAdquirido.ToString()
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
        internal static bool SetInstanceNotDipose(IDataReader dr, PlanoAdquirido objPlanoAdquirido)
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
        #endregion

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

        #region Dashboard

        #region QuantidadePlanosPessoaFisicaEmAberto
        /// <summary>
        /// Metodo utilizado pelo robô(service) atualizar plano PF
        /// </summary>
        public static int QuantidadePlanosPessoaFisicaEmAberto()
        {
            try
            {
                using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spretornaplanosvencidospf, null))
                {
                    int count = 0;
                    while (dr.Read())
                        count++;

                    if (!dr.IsClosed)
                        dr.Close();

                    return count;
                }
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
                throw;
            }
        }
        #endregion

        #region QuantidadePlanosPessoaJuridicaEmAberto
        /// <summary>
        /// Metodo utilizado pelo robô(service) atualizar plano PF
        /// </summary>
        public static int QuantidadePlanosPessoaJuridicaEmAberto()
        {
            try
            {
                using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spretornaplanosvencidospj, null))
                {
                    int count = 0;
                    while (dr.Read())
                        count++;

                    if (!dr.IsClosed)
                        dr.Close();

                    return count;
                }
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
                throw;
            }
        }
        #endregion

        #region QuantidadeCurriculoVIPComPlanoEncerrado
        /// <summary>
        /// Metodo utilizado pelo dashboard
        /// </summary>
        public static int QuantidadeCurriculoVIPComPlanoEncerrado()
        {
            try
            {
                return Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, Spcurriculovipcomplanoencerrado, null));
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
                throw;
            }
        }
        #endregion

        #region QuantidadeNotaFiscalAntecipadaNaoEnviada
        /// <summary>
        /// Metodo utilizado pelo dashboard
        /// </summary>
        public static int QuantidadeNotaFiscalAntecipadaNaoEnviada()
        {
            try
            {
                return Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SpQuantidadeNotaFiscalAntecipadaNaoEnviada, null));
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
                throw;
            }
        }
        #endregion

        #endregion

        #endregion

    }

}