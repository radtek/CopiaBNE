//-- Data: 08/04/2010 11:17
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using BNE.BLL.Custom.Email;

namespace BNE.BLL
{
    public partial class Pagamento // Tabela: BNE_Pagamento
    {

        #region Propriedades

        #region DataVencimento
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public DateTime? DataVencimento
        {
            get
            {
                return this._dataVencimento;
            }
            set
            {
                if (value.HasValue) //Tratando vencimento no sábado ou domingo ou feriado
                    value = value.Value.AddDays(Feriado.RetornarDiaUtilVencimento(value.Value));

                this._dataVencimento = value;
                this._modified = true;
            }
        }
        #endregion

        #endregion

        #region Consultas

        #region SPCARREGARPAGAMENTOPRIMEIRAPARCELAAEMABERTOPORPLANOADQUIRIDO
        private const string SPCARREGARPAGAMENTOPRIMEIRAPARCELAAEMABERTOPORPLANOADQUIRIDO = @"
        SELECT TOP 1
            *
        FROM    BNE_Pagamento P WITH(NOLOCK)
            INNER JOIN BNE_Plano_Parcela PP WITH(NOLOCK) ON P.Idf_Plano_Parcela = PP.Idf_Plano_Parcela
        WHERE PP.Idf_Plano_Adquirido = @Idf_Plano_Adquirido
            AND PP.Idf_Plano_Parcela_Situacao = 1
        ORDER BY PP.Dta_Cadastro ASC, PP.Idf_Plano_Parcela ASC";
        #endregion

        #region Spcarregapagamentoporparcela
        private const string Spcarregapagamentoporparcela = @"
        select  * 
        from    BNE_Pagamento WITH(NOLOCK)
        where   Idf_Plano_Parcela = @Idf_Plano_Parcela";
        #endregion

        #region Sprecuperarvalor
        private const string Sprecuperarvalor = @"
        SELECT  Vlr_Pagamento
        FROM    BNE_Pagamento WITH(NOLOCK)
        WHERE   Idf_Pagamento = @Idf_Pagamento
        ";
        #endregion

        #region SprecuperaPagametosMesmaParcela
        private const string SprecuperaPagametosMesmaParcela = @"
        select pg.Idf_Pagamento
        from bne.bne_pagamento pg with(nolock)
        where pg.Idf_Plano_Parcela = @Idf_Plano_Parcela
            and pg.Idf_Pagamento <> @Idf_Pagamento
        ";
        #endregion

        #region SP_EXISTE_OUTRO_PAGAMENTO_PAGO

        private const string SP_EXISTE_OUTRO_PAGAMENTO_PAGO = @"
        SELECT TOP 1 * FROM BNE.BNE_Pagamento
        WHERE	Idf_Pagamento <> @Idf_Pagamento
		AND (
			(Idf_Adicional_Plano IS NOT NULL AND Idf_Adicional_Plano = @Idf_Adicional_Plano) 
			OR
			(Idf_Plano_Parcela IS NOT NULL AND Idf_Plano_Parcela = @Idf_Plano_Parcela)          
		)
		AND Idf_Pagamento_Situacao IN (1, 2) --PAGO ou ABERTO";

        #endregion

        #region SP_CARREGA_PAGAMENTO_DE_TRANSACAO
        private const string SP_CARREGA_PAGAMENTO_DE_TRANSACAO = @"
        SELECT TOP 1
            *
        FROM    BNE.BNE_Pagamento P WITH(NOLOCK)
		INNER JOIN BNE.BNE_TRANSACAO T WITH(NOLOCK) ON P.Idf_Pagamento = T.Idf_Pagamento
        WHERE Idf_Transacao = @Idf_Transacao";

        #endregion

        #region Sp_Pesquisar_Pagamentos_PF_Plano
        private const string Sp_Pesquisar_Pagamentos_PF_Plano = @"
        select fc.Des_Funcao_Categoria
	        , fn.Des_Funcao
	        , pf.Nme_Pessoa
	        , pf.Num_CPF
	        , ci.Nme_Cidade
	        , ci.Sig_Estado
	        , pg.Des_Identificador
			, pl.Vlr_Base
	        , pg.Vlr_Pagamento 
			, (pp.Idf_Plano_parcela - primeira_parcela.Idf_Plano_Parcela) as 'Parcela'
			, parcelas.qtd as 'Qtd_Parcela'
			, pgs.Des_Pagamento_Situacao as 'Situacao'
			, pg.Num_Nota_Fiscal as 'NF'
	        , convert(varchar,pp.Dta_Pagamento,103) as 'Data Pagamento'
            , convert(varchar,pg.Dta_Emissao, 103) as 'Data Emissao'
	        , convert(varchar,pg.Dta_Vencimento,103) as 'Data Vencimento'
	        , pp.Dta_Pagamento as 'Data Pagamento'
            , pg.Idf_Pagamento
            , pg.Flg_Baixado
			, pa.Idf_Plano
        from bne.BNE_Plano_Parcela pp with(nolock)
	        join bne.bne_plano_adquirido pa with(nolock) on pp.Idf_Plano_Adquirido = pa.Idf_Plano_Adquirido
	        join bne.bne_plano pl with(nolock) on pa.Idf_Plano = pl.Idf_Plano
	        join bne.BNE_Pagamento pg with(nolock) on pp.Idf_Plano_Parcela = pg.Idf_Plano_Parcela
	        left join bne.tab_usuario_filial_perfil ufp with (nolock) on pg.Idf_Usuario_Filial_Perfil = ufp.Idf_Usuario_Filial_Perfil
	        left join bne.tab_pessoa_fisica pf with (nolock) on ufp.Idf_Pessoa_Fisica = pf.Idf_Pessoa_Fisica
	        join bne.bne_pagamento_Situacao pgs with(nolock) on pg.Idf_Pagamento_Situacao = pgs.Idf_Pagamento_Situacao
	        cross apply(
			    	select top 1 pp2.Idf_Plano_Parcela
				    from bne.bne_plano_parcela pp2 with(nolock)
				    where pp2.Idf_Plano_Adquirido = pa.Idf_Plano_Adquirido
				    order by pp2.Idf_Plano_Parcela asc
			        ) as primeira_parcela
	        cross apply(
			    	select count(pp1.Idf_Plano_Parcela) as qtd
				    from bne.bne_plano_parcela pp1 with(nolock)
				    where pp1.Idf_Plano_Adquirido = pa.Idf_Plano_Adquirido
			    ) as parcelas			
	        LEFT join BNE.BNE_Curriculo cv WITH (NOLOCK) ON pf.Idf_Pessoa_Fisica = cv.Idf_Pessoa_Fisica
	        left join plataforma.TAB_Cidade ci WITH (NOLOCK) on ci.Idf_Cidade = pf.Idf_Cidade 
            outer apply ( 
					select top 1 IDF_FUNCAO 
					from BNE.BNE_Funcao_Pretendida fp with (nolock)
					where cv.Idf_Curriculo = fp.Idf_Curriculo 
					order by fp.Idf_Funcao_Pretendida 
				) AS FP
            left join plataforma.TAB_Funcao fn WITH (NOLOCK) on fn.Idf_Funcao = fp.Idf_Funcao
            left join plataforma.TAB_Funcao_Categoria fc WITH (NOLOCK) on fc.Idf_Funcao_Categoria = fn.Idf_Funcao_Categoria
            where convert(varchar,pp.Dta_Pagamento,103) = @Dta_Arquivo
				and (@Idf_Plano IS NULL OR pa.Idf_Plano = @Idf_Plano)
                --and pa.Idf_Plano = @Idf_Plano
	            and pg.Idf_Tipo_Pagamento = 2
	            and pl.Idf_Plano_Tipo = 1
	            and pg.Idf_Pagamento_Situacao = 2    
        ";
        #endregion

        #region Sp_Pesquisar_Pagamentos_PJ_Plano
        private const string Sp_Pesquisar_Pagamentos_PJ_Plano = @"
        select pl.Des_Plano
	        , fl.Raz_Social
	        , fl.Num_CNPJ
	        , ci.Nme_Cidade
	        , ci.Sig_Estado
	        , pg.Des_Identificador
			, pl.Vlr_Base
	        , pg.Vlr_Pagamento 
			, (pp.Idf_Plano_parcela - primeira_parcela.Idf_Plano_Parcela) as 'Parcela'
			, parcelas.qtd as 'Qtd_Parcela'
			, pgs.Des_Pagamento_Situacao as 'Situacao'
			, pg.Num_Nota_Fiscal as 'NF'
	        , convert(varchar,pp.Dta_Pagamento,103) as 'Data Pagamento'
            , convert(varchar,pg.Dta_Emissao, 103) as 'Data Emissao'
	        , convert(varchar,pg.Dta_Vencimento,103) as 'Data Vencimento'
	        , pp.Dta_Pagamento as 'Data Pagamento'
            , pg.Idf_Pagamento
            , pg.Flg_Baixado
        from bne.BNE_Plano_Parcela pp with(nolock)
	        join bne.BNE_Pagamento pg with(nolock) on pp.Idf_Plano_Parcela = pg.Idf_Plano_Parcela
	        join bne.bne_pagamento_Situacao pgs with(nolock) on pg.Idf_Pagamento_Situacao = pgs.Idf_Pagamento_Situacao
	        join bne.tab_filial fl with (nolock) on pg.Idf_Filial = fl.Idf_Filial
	        join bne.tab_endereco en with (nolock) on fl.Idf_Endereco = en.Idf_Endereco	
	        join plataforma.TAB_Cidade ci WITH (NOLOCK) ON en.Idf_Cidade = ci.Idf_Cidade
            left join bne.BNE_Adicional_Plano ap WITH (NOLOCK) ON pg.Idf_Adicional_Plano = ap.Idf_Adicional_Plano
	        left join bne.BNE_Plano_Adquirido pa WITH (NOLOCK) ON pp.Idf_Plano_Adquirido = pa.Idf_Plano_Adquirido OR ap.Idf_Plano_Adquirido = pa.Idf_Plano_Adquirido
		    cross apply(
				select top 1 pp2.Idf_Plano_Parcela
				from bne.bne_plano_parcela pp2 with(nolock)
				where pp2.Idf_Plano_Adquirido = pa.Idf_Plano_Adquirido
				order by pp2.Idf_Plano_Parcela asc
			) as primeira_parcela
			cross apply(
				select count(pp1.Idf_Plano_Parcela) as qtd
				from bne.bne_plano_parcela pp1 with(nolock)
				where pp1.Idf_Plano_Adquirido = pa.Idf_Plano_Adquirido
			) as parcelas
	            join bne.BNE_Plano pl WITH (NOLOCK) ON pa.Idf_Plano = pl.Idf_Plano
        where convert(varchar,pp.Dta_Pagamento,103) = @Dta_Arquivo
		    and (@Idf_Plano IS NULL OR pa.Idf_Plano = @Idf_Plano)
            --and pa.Idf_Plano = @Idf_Plano
	        and pg.Idf_Tipo_Pagamento = 2
	        and pl.Idf_Plano_Tipo = 2
	        and pg.Idf_Pagamento_Situacao = 2
        ";
        #endregion

        #region Sp_Recuperar_Pagamento_Boleto
        private const string Sp_Recuperar_Pagamento_Boleto = @"
        SELECT    *
        FROM      bne.bne_pagamento pg WITH ( NOLOCK )
                JOIN BNE.BNE_Plano_Parcela pp WITH ( NOLOCK ) ON pg.Idf_Plano_Parcela = pp.Idf_Plano_Parcela
                JOIN BNE.BNE_Plano_Adquirido pa WITH ( NOLOCK ) ON pp.Idf_Plano_Adquirido = pa.Idf_Plano_Adquirido
                JOIN BNE.BNE_Plano pl WITH ( NOLOCK ) ON pa.Idf_Plano = pl.Idf_Plano
                JOIN BNE.TAB_Usuario_Filial_Perfil ufp WITH ( NOLOCK ) ON pa.Idf_Usuario_Filial_Perfil = ufp.Idf_Usuario_Filial_Perfil
                JOIN BNE.TAB_Pessoa_Fisica pf WITH ( NOLOCK ) ON ufp.Idf_Pessoa_Fisica = pf.Idf_Pessoa_Fisica
        WHERE     pg.Des_Identificador LIKE @Num_Boleto
                OR CASE WHEN ISNUMERIC(@Num_Boleto) = 1 THEN CAST(@Num_Boleto AS DECIMAL)
                    END = pg.Idf_Pagamento";
        #endregion

        #region SprecuperarinfoPagamentoPF
        private const string SprecuperarinfoPagamentoPF = @"
        select pl.Des_Plano
	        , pf.Nme_Pessoa
	        , pf.Num_CPF
        	, pg.Des_Identificador
	        , pg.Num_Nota_Fiscal
	        , pgs.Des_Pagamento_Situacao
        from bne.bne_pagamento pg with(nolock)
	        join bne.BNE_Pagamento_Situacao pgs with(nolock) on pg.Idf_Pagamento_Situacao = pgs.Idf_Pagamento_Situacao
	        join bne.bne_plano_parcela pp with(nolock) on pg.Idf_Plano_Parcela = pp.Idf_Plano_Parcela
	        join bne.BNE_Plano_Adquirido pa with(nolock) on pp.Idf_Plano_Adquirido = pa.Idf_Plano_Adquirido
	        join bne.BNE_Plano pl with(nolock) on pa.Idf_Plano = pl.Idf_Plano
	        join bne.TAB_Usuario_Filial_Perfil ufp with(nolock) on pa.Idf_Usuario_Filial_Perfil = ufp.Idf_Usuario_Filial_Perfil
	        join bne.tab_pessoa_fisica pf with(nolock) on ufp.Idf_Pessoa_Fisica = pf.Idf_Pessoa_Fisica
        where pg.Idf_Pagamento = @Idf_Pagamento
        ";
        #endregion

        #region SP_PAGOS_SEM_NOTA
        private const string SP_PAGOS_SEM_NOTA = @"
            SELECT * 
            FROM BNE.BNE_Pagamento pag WITH(NOLOCK)
			JOIN BNE.BNE_Tipo_Pagamento tpag WITH(NOLOCK) ON pag.Idf_Tipo_Pagamento = tpag.Idf_Tipo_Pagamento
            JOIN BNE.BNE_Plano_Parcela pp WITH(NOLOCK) ON pag.Idf_Plano_Parcela = pp.Idf_Plano_Parcela
            JOIN BNE.BNE_Plano_Adquirido pa WITH(NOLOCK) ON pp.Idf_Plano_Adquirido = pa.Idf_Plano_Adquirido
			JOIN BNE.BNE_Plano pl WITH(NOLOCK) ON pa.Idf_Plano = pl.Idf_Plano
			JOIN plataforma.BNE_Tipo_Contrato tc WITH(NOLOCK) ON pl.Idf_Tipo_Contrato = tc.Idf_Tipo_Contrato
                            WHERE 1 = 1
							AND pp.Dta_Pagamento BETWEEN DATEADD(d, -90, GETDATE()) AND GETDATE()
				            AND pa.Idf_Plano_Situacao <> 3 --Cancelado
                            AND Idf_Pagamento_Situacao = 2 --pago
				            AND Num_Nota_Fiscal IS NULL
                            AND Flg_baixado = 0
                            AND Vlr_Pagamento > 0";

        #endregion

        #endregion

        #region Metodos

        #region CarregarPagamentoPrimeiraParcelaEmAbertoPorPlanoAdquirido
        /// <summary>
        /// Carrega a primeira parcela de qualquer plano adquirido atraves do ID do plano Adquirido
        /// </summary>
        /// <returns></returns>
        public static bool CarregarPagamentoPrimeiraParcelaEmAbertoPorPlanoAdquirido(int idPlanoAdquirido, out Pagamento objPagamento)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Plano_Adquirido", SqlDbType.Int, 4));
            parms[0].Value = idPlanoAdquirido;

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SPCARREGARPAGAMENTOPRIMEIRAPARCELAAEMABERTOPORPLANOADQUIRIDO, parms))
            {
                objPagamento = new Pagamento();

                if (SetInstance(dr, objPagamento))
                    return true;

                if (!dr.IsClosed)
                    dr.Close();

                dr.Dispose();
            }

            objPagamento = null;
            return false;
        }
        #endregion

        #region CarregarPagamentoDeTransacao
        /// <summary>
        /// Carrega a primeira parcela de qualquer plano adquirido atraves do ID do plano Adquirido
        /// </summary>
        /// <returns></returns>
        public static bool CarregarPagamentoDeTransacao(int idTransacao, out Pagamento objPagamento)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Transacao", SqlDbType.Int, 4));
            parms[0].Value = idTransacao;

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SP_CARREGA_PAGAMENTO_DE_TRANSACAO, parms))
            {
                objPagamento = new Pagamento();

                if (SetInstance(dr, objPagamento))
                    return true;

                if (!dr.IsClosed)
                    dr.Close();

                dr.Dispose();
            }

            objPagamento = null;
            return false;
        }
        #endregion

        #region CarregaPagamentosPorPlanoParcela
        /// <summary>
        /// Carrega todos os pagamentos em aberto referente "objPlanoParcela" passados como parâmetros
        /// </summary>
        /// <param name="objPlanoParcela"></param>
        /// <returns></returns>
        public static List<Pagamento> CarregaPagamentosPorPlanoParcela(int idPlanoParcela, SqlTransaction trans = null)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter{ ParameterName = "Idf_Plano_Parcela", SqlDbType = SqlDbType.Int, Size = 4, Value = idPlanoParcela}
                };

            var listRetornoPagamento = new List<Pagamento>();

            IDataReader dr = trans != null ? DataAccessLayer.ExecuteReader(trans, CommandType.Text, Spcarregapagamentoporparcela, parms) : DataAccessLayer.ExecuteReader(CommandType.Text, Spcarregapagamentoporparcela, parms);

            Pagamento objPagamento;
            while (dr.Read())
            {
                objPagamento = new Pagamento();
                if (SetInstance_NonDispose(dr, objPagamento))
                    listRetornoPagamento.Add(objPagamento);
            }


            if (!dr.IsClosed)
                dr.Close();

            dr.Dispose();

            return listRetornoPagamento;
        }
        #endregion

        #region CarregarPagosSemNota
        /// <summary>
        /// Retorna as pagamentos com situação paga, mas sem nota fical
        /// </summary>
        /// <returns>Lista com pagamentos sem nota.</returns>
        /// <remarks>Francisco Ribas</remarks>
        public static List<Pagamento> CarregarPagosSemNota()
        {
            List<Pagamento> lstRetorno = new List<Pagamento>();

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SP_PAGOS_SEM_NOTA, null))
            {
                Pagamento objPagamento;
                while (dr.Read())
                {
                    objPagamento = new Pagamento();

                    SetInstance_NonDispose(dr, objPagamento);
                    PlanoParcela.SetInstance_NonDispose(dr, objPagamento.PlanoParcela);
                    PlanoAdquirido.SetInstanceNotDipose(dr, objPagamento.PlanoParcela.PlanoAdquirido);
                    Plano.SetInstanceNotDipose(dr, objPagamento.PlanoParcela.PlanoAdquirido.Plano);
                    TipoContrato.SetInstance(dr, objPagamento.PlanoParcela.PlanoAdquirido.Plano.TipoContrato, false, false);
                    TipoPagamento.SetInstance(dr, objPagamento.TipoPagamento, false, false);

                    lstRetorno.Add(objPagamento);
                }
                if (!dr.IsClosed)
                    dr.Close();
            }

            return lstRetorno;
        }
        #endregion

        #region CarregarComNotaSolicitada
        /// <summary>
        /// Retorna as pagamentos com situação paga, mas sem nota fical
        /// </summary>
        /// <returns>Lista com pagamentos sem nota.</returns>
        /// <remarks>Francisco Ribas</remarks>
        public static List<Pagamento> CarregarComNotaSolicitada()
        {
            List<Pagamento> lstRetorno = new List<Pagamento>();

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SP_PAGOS_SEM_NOTA, null))
            {
                Pagamento objPagamento;
                while (dr.Read())
                {
                    objPagamento = new Pagamento();
                    if (SetInstance_NonDispose(dr, objPagamento))
                        lstRetorno.Add(objPagamento);
                }
                if (!dr.IsClosed)
                    dr.Close();
            }

            return lstRetorno;
        }
        #endregion CarregarComNotaSolicitada

        #region Salvar
        /// <summary>
        /// Salva a parcela, atualizando as quantidades de SMS
        /// </summary>
        /// <param name="qtdeSMSTotal"></param>
        /// <param name="qtdeSMSLiberada"></param>
        public void Salvar(int? qtdeSMSTotal, int? qtdeSMSLiberada)
        {
            using (SqlConnection conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();

                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        Save(trans);

                        //Se existir um plano adicional
                        if (AdicionalPlano != null)
                        {
                            AdicionalPlano.CompleteObject(trans);

                            if (PagamentoSituacao.IdPagamentoSituacao.Equals((int)Enumeradores.PagamentoSituacao.Cancelado))
                                AdicionalPlano.CancelarPlanoAdicional(trans);

                            AdicionalPlano.Save(trans);
                        }
                        else
                        {
                            PlanoParcela.CompleteObject(trans);

                            // Cancela os outros boletos da mesma parcela
                            var listaPagamentosMesmaParcela = RecuperarPagamentosMesmaParcela(this.PlanoParcela.IdPlanoParcela, this.IdPagamento, trans);

                            foreach (int pagamento in listaPagamentosMesmaParcela)
                            {
                                var objPagamentoAux = LoadObject(pagamento, trans);

                                objPagamentoAux.PagamentoSituacao = new PagamentoSituacao(Convert.ToInt32(Enumeradores.PagamentoSituacao.Cancelado));

                                objPagamentoAux.Save(trans);
                            }

                            if ((int)Enumeradores.PagamentoSituacao.EmAberto == PagamentoSituacao.IdPagamentoSituacao)
                            {
                                PlanoParcela.QuantidadeSMSTotal = qtdeSMSTotal.HasValue ? qtdeSMSTotal.Value : PlanoParcela.QuantidadeSMSTotal;
                                PlanoParcela.RecarregarSMS(qtdeSMSLiberada.HasValue ? qtdeSMSLiberada.Value : 0, trans);
                                PlanoParcela.Save(trans);
                            }
                            else if ((int)Enumeradores.PagamentoSituacao.Pago == PagamentoSituacao.IdPagamentoSituacao)
                            {
                                PlanoParcela.PlanoParcelaSituacao = new PlanoParcelaSituacao((int)Enumeradores.PlanoParcelaSituacao.Pago);
                                PlanoParcela.DataPagamento = DateTime.Now;
                                PlanoParcela.RecarregarSMS(null, trans);
                                PlanoParcela.Save(trans);
                            }
                            else if ((int)Enumeradores.PagamentoSituacao.Cancelado == PagamentoSituacao.IdPagamentoSituacao)
                            {
                                int idPlanoAdquirido = PlanoParcela.PlanoAdquirido.IdPlanoAdquirido;

                                PlanoParcela.PlanoParcelaSituacao = new PlanoParcelaSituacao((int)Enumeradores.PlanoParcelaSituacao.Cancelado);
                                PlanoParcela.Save(trans);

                                PlanoParcela.RedistribuirQtdeSMSTotalNasParcelasEmAberto(idPlanoAdquirido, trans);
                            }
                        }

                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        throw ex;
                    }
                }
            }
        }
        #endregion

        #region Liberar
        /// <summary>
        /// Libera o pagamento
        /// </summary>
        /// <returns></returns>
        public bool Liberar(DateTime dataPagamento)
        {
            bool retorno = false;

            using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();
                using (SqlTransaction transLiberacao = conn.BeginTransaction())
                {
                    try
                    {
                        retorno = Liberar(transLiberacao, dataPagamento);
                        transLiberacao.Commit();
                    }
                    catch (Exception ex)
                    {
                        transLiberacao.Rollback();
                        EL.GerenciadorException.GravarExcecao(ex);
                        throw;
                    }
                }
            }

            return retorno;
        }

        /// <summary>
        /// Libera o pagamento
        /// </summary>
        /// <param name="trans"></param>
        /// <returns></returns>
        public bool Liberar(SqlTransaction trans, DateTime dataPagamento)
        {
            try
            {
                //Carregando objeto somente caso ele não tenha sido carregado
                if (this.PlanoParcela == null)
                    CompleteObject(trans);

                //Caso seja um pagamento de adicional de SMS, soma a quantidade de SMS ao plano quantidade vigente.
                if (AdicionalPlano != null)
                {
                    AdicionalPlano.LiberarPlanoAdicional(trans); //Alterando a situação do plano adiconal para PAGO
                }
                else
                {
                    this.PlanoParcela.Liberar(this, dataPagamento, trans);
                    this.PlanoParcela.PlanoAdquirido.Liberar(true, trans);
                }

                // usa codigo de credito, caso haja
                if (CodigoDesconto != null)
                    CodigoDesconto.Utilizar(trans);

                PagamentoSituacao = new PagamentoSituacao((int)Enumeradores.PagamentoSituacao.Pago);
                Save(trans);

                return true;
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
                throw;
            }
        }
        #endregion

        #region RecuperarValor
        /// <summary>
        /// Recupera o valor do pagamento atual
        /// </summary>
        /// <returns></returns>
        public decimal RecuperarValor()
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Pagamento", SqlDbType = SqlDbType.Int, Size = 4, Value = _idPagamento }
                };

            return Convert.ToDecimal(DataAccessLayer.ExecuteScalar(CommandType.Text, Sprecuperarvalor, parms));
        }
        #endregion

        #region ConcederDesconto
        /// <summary>
        /// Concede o desconto ao objeto Pagamento,
        /// chamar logo antes do procedimento que autoriza o pagamento do objeto Pagamento
        /// </summary>
        /// <param name="trans">transacao do SQL Server</param>
        /// <param name="objCodigoDesconto">objeto CodigoDesconto referente ao cupom de desconto</param>
        /// <returns>true se a concessão ocorreu corretamente, false se o objeto cupom de desconto ja tiver sido usado ou se o objeto Pagamento ja tiver um codigo de desconto</returns>
        public bool ConcederDesconto(SqlTransaction trans, CodigoDesconto objCodigoDesconto)
        {
            decimal valor = ValorPagamento;

            if (!objCodigoDesconto.JaUtilizado())
            {
                objCodigoDesconto.CalcularDesconto(ref valor);

                ValorPagamento = valor;
                CodigoDesconto = objCodigoDesconto;

                if (trans != null)
                    Save(trans);
                else
                    Save();

                return true;
            }

            return false;
        }

        /// <summary>
        /// Concede o desconto ao objeto Pagamento,
        /// chamar logo antes do procedimento que autoriza o pagamento do objeto Pagamento
        /// </summary>
        /// <param name="objCodigoDesconto">objeto CodigoDesconto referente ao cupom de desconto</param>
        /// <returns>true se a concessão ocorreu corretamente, false se o objeto cupom de desconto ja tiver sido usado ou se o objeto Pagamento ja tiver um codigo de desconto</returns>
        public bool ConcederDesconto(CodigoDesconto objCodigoDesconto)
        {
            return ConcederDesconto(null, objCodigoDesconto);
        }
        #endregion

        #region ConcederDescontoIntegral
        /// <summary>
        /// Concede e libera desconto integral ao usuario
        /// </summary>
        /// <param name="idUsuarioFilialPerfil">id do objeto UsuarioFilialPerfil</param>
        /// <param name="idPlano">id do objeto Plano</param>
        /// <param name="idCodigoDesconto">id do objeto CodigoDesconto</param>
        /// <param name="erro">mensagem de erro</param>
        /// <returns>true se a liberação ocorreu corretamente, false se houve erro, sendo que o parâmetro erro contém a mensagem relacionada</returns>
        public static bool ConcederDescontoIntegral(Curriculo objCurriculo, UsuarioFilialPerfil objUsuarioFilialPerfil, Plano objPlano, CodigoDesconto objCodigoDesconto, out string erro)
        {
            erro = null;

            BLL.Pagamento objPagamento = null;

            objPlano.CompleteObject();

            if (objPlano.PlanoTipo.IdPlanoTipo.Equals((int)Enumeradores.PlanoTipo.PessoaFisica))
            {
                objUsuarioFilialPerfil.CompleteObject();

                var objPlanoAdquirido = PlanoAdquirido.CriarPlanoAdquiridoPF(objUsuarioFilialPerfil, objPlano);

                objPlanoAdquirido.CriarParcelas(new TipoPagamento((int)Enumeradores.TipoPagamento.Parceiro), objCodigoDesconto, null);

                PlanoParcela objPlanoParcela;
                //Se o valor base do plano for zerado, parcelas já estão pagas
                if (objPlano.ValorBase > 0)
                    objPlanoParcela = PlanoParcela.CarregaParcelaAtualEmAbertoPorPlanoAdquirido(objPlanoAdquirido);
                else
                    objPlanoParcela = PlanoParcela.CarregarPrimeiraParcelaPorPlanoAdquirido(objPlanoAdquirido.IdPlanoAdquirido, null);

                // cria ou recupera o pagamento para o plano adquirido
                List<BLL.Pagamento> objListPagamentosPorParcela = BLL.Pagamento.CarregaPagamentosPorPlanoParcela(objPlanoParcela.IdPlanoParcela);

                if (objListPagamentosPorParcela != null && objListPagamentosPorParcela.Count > 0)
                {
                    objPagamento = objListPagamentosPorParcela
                        .FirstOrDefault(p =>
                            (p.TipoPagamento == null || p.TipoPagamento.IdTipoPagamento == (int)Enumeradores.TipoPagamento.Parceiro)
                            && p.FlagInativo == false);

                    // Atualiza pagamentos selecionados com o tipo == null, caso não exista entao cria um novo.
                    if (objPagamento == null)
                        objPagamento =
                            PlanoAdquirido.CriarPagamento(objPlanoParcela, objPlano, null, objUsuarioFilialPerfil,
                            new TipoPagamento((int)Enumeradores.TipoPagamento.Parceiro));

                    PlanoAdquirido.AtualizarPagamento(objPagamento,
                        new TipoPagamento((int)Enumeradores.TipoPagamento.Parceiro),
                        objPlanoAdquirido,
                        objPlano);

                    using (SqlConnection conn = new SqlConnection(DataAccessLayer.CONN_STRING))
                    {
                        conn.Open();

                        using (SqlTransaction transLiberacao = conn.BeginTransaction())
                        {
                            try
                            {
                                // liberando por cupom de desconto
                                objPagamento.DescricaoIdentificador = "Liberado por cupom de desconto";
                                objPagamento.ConcederDesconto(transLiberacao, objCodigoDesconto);
                                objPagamento.Liberar(transLiberacao, DateTime.Now);

                                transLiberacao.Commit();
                            }
                            catch (Exception ex)
                            {
                                if (!(ex is ThreadAbortException))
                                    EL.GerenciadorException.GravarExcecao(ex);

                                erro = ex.Message;
                                transLiberacao.Rollback();

                                return false;
                            }
                        }
                    }

                    return true;
                }
                else
                {
                    erro = "Não há registro de pagamento associado ao perfil do usuário";
                }
            }
            else
            {
                erro = "O plano selecionado não é para pessoa física";
            }

            return false;
        }

        #endregion

        #region JaPago
        public bool JaPago(SqlTransaction trans = null)
        {
            if (null == this.PagamentoSituacao)
            {
                if (null == trans)
                    CompleteObject();
                else
                    CompleteObject(trans);
            }

            return (int)Enumeradores.PagamentoSituacao.Pago == this.PagamentoSituacao.IdPagamentoSituacao;
        }
        #endregion

        #region ExisteOutroPagamentoPago
        /// <summary>
        /// Recupera outro pagamento pago ou aberto para a mesma parcela ou plano adicional, se houver. 
        /// Utilizado pelo fluxo dos Intermediadores de pagamento, que podem retornar status dias depois da transação aberta.
        /// </summary>
        /// <returns>Instância do pagamento ou null, se não existir pagamento pago ou aberto para a mesma parcela ou plano adicional</returns>
        public Pagamento RecuperaOutroPagamentoPagoOuAberto()
        {
            var parms = new List<SqlParameter>
            {
                new SqlParameter { ParameterName = "@Idf_Pagamento", SqlDbType = SqlDbType.Int, Size = 4, Value = _idPagamento }
            };

            if (_planoParcela == null)
            {
                new SqlParameter { ParameterName = "@Idf_Plano_Parcela", SqlDbType = SqlDbType.Int, Size = 4, Value = DBNull.Value };
            }
            else
            {
                new SqlParameter { ParameterName = "@Idf_Plano_Parcela", SqlDbType = SqlDbType.Int, Size = 4, Value = _planoParcela.IdPlanoParcela };
            }

            if (_adicionalPlano == null)
            {
                new SqlParameter { ParameterName = "@Idf_Adicional_Plano", SqlDbType = SqlDbType.Int, Size = 4, Value = DBNull.Value };
            }
            else
            {
                new SqlParameter { ParameterName = "@Idf_Adicional_Plano", SqlDbType = SqlDbType.Int, Size = 4, Value = _adicionalPlano.IdAdicionalPlano };
            }

            Pagamento objOutroPagamento = null;

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SP_CARREGA_PAGAMENTO_DE_TRANSACAO, parms))
            {
                objOutroPagamento = new Pagamento();

                if (SetInstance(dr, objOutroPagamento))
                    return objOutroPagamento;

                if (!dr.IsClosed)
                    dr.Close();

                dr.Dispose();
            }

            return objOutroPagamento;
        }
        #endregion

        #region RecuperarPagamentosMesmaParcela
        public static List<int> RecuperarPagamentosMesmaParcela(int idfPlanoParcela, int idfPagamento, SqlTransaction trans = null)
        {
            var listaPagamentos = new List<int>();

            var parms = new List<SqlParameter> 
            {
                new SqlParameter { ParameterName = "@Idf_Plano_Parcela", SqlDbType = SqlDbType.Int, Size = 4, Value = idfPlanoParcela },
                new SqlParameter { ParameterName = "@Idf_Pagamento", SqlDbType = SqlDbType.Int, Size = 4, Value = idfPagamento }
            };


            IDataReader dr = trans == null ? DataAccessLayer.ExecuteReader(CommandType.Text, SprecuperaPagametosMesmaParcela, parms) : DataAccessLayer.ExecuteReader(trans, CommandType.Text, SprecuperaPagametosMesmaParcela, parms);

            while (dr.Read())
            {
                var idPagamento = Convert.ToInt32(dr["Idf_Pagamento"].ToString());

                listaPagamentos.Add(idPagamento);
            }

            if (!dr.IsClosed)
            {
                dr.Close();
            }
            dr.Dispose();

            return listaPagamentos;
        }
        #endregion

        #region PesquisarPagamentos
        // Pesquisar pagamentos de acordo com tipo do plano

        public static InformacaoPagamento PesquisarPagamentos(DateTime dataArquivo, Plano objPlano = null)
        {
            var informacaoPagamento = new InformacaoPagamento();

            PagamentosPF(informacaoPagamento.PF, dataArquivo.ToShortDateString(), objPlano);

            PagamentosPJ(informacaoPagamento.PJ, dataArquivo.ToShortDateString(), objPlano);

            return informacaoPagamento;
        }

        public static InformacaoPagamento PesquisarPagamentos(DateTime dataArquivo, Enumeradores.PlanoTipo planoTipo, Plano objPlano = null)
        {
            var informacaoPagamento = new InformacaoPagamento();

            if (planoTipo == Enumeradores.PlanoTipo.PessoaFisica)
                PagamentosPF(informacaoPagamento.PF, dataArquivo.ToShortDateString(), objPlano);
            else
                PagamentosPJ(informacaoPagamento.PJ, dataArquivo.ToShortDateString(), objPlano);

            return informacaoPagamento;
        }
        #endregion

        #region PagamentosPF
        public static void PagamentosPF(List<InformacaoPagamento.InformacaoPagamentoBoleto> listaPagamentoBoleto, string dataArquivo, Plano objPlano = null)
        {
            object valuePlano = DBNull.Value;

            if (objPlano != null)
                valuePlano = objPlano.IdPlano;

            var parms = new List<SqlParameter> 
                {
                    new SqlParameter{ ParameterName = "@Dta_Arquivo", SqlDbType = SqlDbType.VarChar, Value = dataArquivo},
                    new SqlParameter{ ParameterName = "@Idf_Plano", SqlDbType = SqlDbType.Int, Value = valuePlano }
                };

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Sp_Pesquisar_Pagamentos_PF_Plano, parms))
            {
                while (dr.Read())
                {
                    var parcela = Convert.ToInt32(dr["Parcela"]);
                    var quantidadeParcela = Convert.ToInt32(dr["Qtd_Parcela"]);

                    var informacaoBoleto = new InformacaoPagamento.InformacaoPagamentoBoleto
                {
                    IdPagamento = Convert.ToInt32(dr["Idf_Pagamento"]),
                    Nome = dr["Nme_Pessoa"].ToString(),
                    NotaFiscal = dr["NF"].ToString(),
                    NumeroBoleto = dr["Des_Identificador"].ToString(),
                    NumeroDocumento = Convert.ToDecimal(dr["Num_CPF"]),
                    Parcela = parcela == 0 ? "1 de " + quantidadeParcela : parcela + " de " + quantidadeParcela,
                    Plano = dr["Des_Funcao_Categoria"].ToString(),
                    ValorPlano = Convert.ToDecimal(dr["Vlr_Pagamento"])
                };
                    listaPagamentoBoleto.Add(informacaoBoleto);
                }
            };
        }
        #endregion

        #region PagamentosPJ
        public static void PagamentosPJ(List<InformacaoPagamento.InformacaoPagamentoBoleto> listaPagamentoBoleto, string dataArquivo, Plano objPlano = null)
        {
            object valuePlano = DBNull.Value;

            if (objPlano != null)
                valuePlano = objPlano.IdPlano;

            var parms = new List<SqlParameter> 
                {
                    new SqlParameter{ ParameterName = "@Dta_Arquivo", SqlDbType = SqlDbType.VarChar, Value = dataArquivo},
                    new SqlParameter{ ParameterName = "@Idf_Plano", SqlDbType = SqlDbType.Int, Value = valuePlano }
                };

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Sp_Pesquisar_Pagamentos_PJ_Plano, parms))
            {
                while (dr.Read())
                {
                    var parcela = Convert.ToInt32(dr["Parcela"]);
                    var quantidadeParcela = Convert.ToInt32(dr["Qtd_Parcela"]);

                    var informacaoBoleto = new InformacaoPagamento.InformacaoPagamentoBoleto
                {
                    IdPagamento = Convert.ToInt32(dr["Idf_Pagamento"]),
                    Nome = dr["Raz_Social"].ToString(),
                    NotaFiscal = dr["NF"].ToString(),
                    NumeroBoleto = dr["Des_Identificador"].ToString(),
                    NumeroDocumento = Convert.ToDecimal(dr["Num_CNPJ"]),
                    Parcela = parcela == 0 ? "1 de " + quantidadeParcela : parcela + " de " + quantidadeParcela,
                    Plano = dr["Des_Plano"].ToString(),
                    ValorPlano = Convert.ToDecimal(dr["Vlr_Pagamento"])
                };
                    listaPagamentoBoleto.Add(informacaoBoleto);
                }
            };
        }

        #endregion

        #region CarregarPagamentoPorNossoNumeroBoleto
        public static bool CarregarPagamentoPorNossoNumeroBoleto(string desIdentificador, out Pagamento objPagamento)
        {
            objPagamento = new Pagamento();
            var parms = new List<SqlParameter> 
                {
                    new SqlParameter{ ParameterName = "@Num_Boleto", SqlDbType = SqlDbType.VarChar, Value = desIdentificador}
                };

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Sp_Recuperar_Pagamento_Boleto, parms))
            {
                try
                {
                    if (dr.Read())
                    {
                        SetInstance_NonDispose(dr, objPagamento);
                        try
                        {
                            PlanoParcela.SetInstance_NonDispose(dr, objPagamento.PlanoParcela);
                        }
                        catch { }
                        try
                        {
                            PlanoAdquirido.SetInstanceNotDipose(dr, objPagamento.PlanoParcela.PlanoAdquirido);
                        }
                        catch { }
                        try
                        {
                            Plano.SetInstanceNotDipose(dr, objPagamento.PlanoParcela.PlanoAdquirido.Plano);
                        }
                        catch { }
                        try
                        {
                            UsuarioFilialPerfil.SetInstanceNotDipose(dr, objPagamento.PlanoParcela.PlanoAdquirido.UsuarioFilialPerfil);
                        }
                        catch { }
                        try
                        {
                            PessoaFisica.SetInstanceNotDipose(dr, objPagamento.PlanoParcela.PlanoAdquirido.UsuarioFilialPerfil.PessoaFisica);
                        }
                        catch { }
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    dr.Dispose();
                }
            }

            return true;
        }
        #endregion

        #region RecuperarInfoPagamentoPF
        public static bool RecuperarInfoPagamentoPF(int idPagamento, out string desPlano, out string nomePessoa, out string numCpf, out string desIdentificador, out string numNotaFiscal, out string situacaoPg)
        {
            desPlano = string.Empty;
            nomePessoa = string.Empty;
            numCpf = string.Empty;
            desIdentificador = string.Empty;
            numNotaFiscal = string.Empty;
            situacaoPg = string.Empty;

            bool retorno = false;

            var parms = new List<SqlParameter> 
                {
                    new SqlParameter{ ParameterName = "@Idf_Pagamento", SqlDbType = SqlDbType.Int, Value = idPagamento}
                };

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SprecuperarinfoPagamentoPF, parms))
            {
                if (dr.Read())
                {
                    desPlano = dr["Des_Plano"].ToString();
                    nomePessoa = dr["Nme_Pessoa"].ToString();
                    numCpf = dr["Num_CPF"].ToString();
                    desIdentificador = dr["Des_Identificador"].ToString();
                    numNotaFiscal = dr["Num_Nota_Fiscal"].ToString();
                    situacaoPg = dr["Des_Pagamento_Situacao"].ToString();

                    retorno = true;
                }
            }

            return retorno;
        }
        #endregion

        #region CancelarPagamento
        public static void CancelarPagamento(Pagamento objPagamento, int idUsuarioFilialPerfil)
        {
            string template = ConteudoHTML.RecuperaValorConteudo(BLL.Enumeradores.ConteudoHTML.ConteudoPadraoEmailNovo);
            string urlSite = string.Concat("http://", Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.URLAmbiente));
            string urlImagens = Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.URLImagens);

            string corpo = ConteudoHTML.RecuperaValorConteudo(Enumeradores.ConteudoHTML.MensagemCancelamentoNF);

            string mensagem = string.Format(template, urlImagens, urlSite, corpo);

            using (var objWsIntegracao = new BLL.wsIntegracaoWFat.IntegracaoFaturamentoFinanceiro())
            {
                objPagamento.TipoPagamento.CompleteObject();

                var objUsuarioLogado = UsuarioFilialPerfil.LoadObject(idUsuarioFilialPerfil);
                objUsuarioLogado.PessoaFisica.CompleteObject();

                if (objPagamento.AdicionalPlano != null)
                {
                    objPagamento.AdicionalPlano.CompleteObject();
                    objPagamento.AdicionalPlano.PlanoAdquirido.CompleteObject();
                }
                else
                {
                    objPagamento.PlanoParcela.CompleteObject();
                    objPagamento.PlanoParcela.PlanoAdquirido.CompleteObject();
                }

                if ((objPagamento.AdicionalPlano != null && objPagamento.AdicionalPlano.PlanoAdquirido.ParaPessoaFisica()) ||
                    (objPagamento.PlanoParcela != null && objPagamento.PlanoParcela.PlanoAdquirido.ParaPessoaFisica()))
                {
                    String numCPF;
                    if (objPagamento.AdicionalPlano != null)
                    {
                        objPagamento.AdicionalPlano.PlanoAdquirido.UsuarioFilialPerfil.CompleteObject();
                        objPagamento.AdicionalPlano.PlanoAdquirido.UsuarioFilialPerfil.PessoaFisica.CompleteObject();

                        numCPF = objPagamento.AdicionalPlano.PlanoAdquirido.UsuarioFilialPerfil.PessoaFisica.CPF.ToString().Replace(".", "").Replace("-", "").Replace("/", "");
                    }
                    else
                    {
                        objPagamento.PlanoParcela.PlanoAdquirido.UsuarioFilialPerfil.CompleteObject();
                        objPagamento.PlanoParcela.PlanoAdquirido.UsuarioFilialPerfil.PessoaFisica.CompleteObject();

                        numCPF = objPagamento.PlanoParcela.PlanoAdquirido.UsuarioFilialPerfil.PessoaFisica.CPF.ToString().Replace(".", "").Replace("-", "").Replace("/", "");
                    }

                    string descricaoIdentificador = string.IsNullOrWhiteSpace(objPagamento.DescricaoIdentificador) ? objPagamento.IdPagamento.ToString() : objPagamento.DescricaoIdentificador;

                    if (objPagamento.TipoPagamento.IdTipoPagamento == (int)Enumeradores.TipoPagamento.BoletoBancario)
                        objWsIntegracao.CancelarNotaFiscalIntegracao(BLL.wsIntegracaoWFat.TipoTransacao.Boleto, descricaoIdentificador, BLL.wsIntegracaoWFat.TipoDocumento.CPF, Convert.ToString(numCPF), Convert.ToDecimal(objUsuarioLogado.PessoaFisica.CPF), objUsuarioLogado.PessoaFisica.NomePessoa, mensagem);
                    else
                        objWsIntegracao.CancelarNotaFiscalIntegracao(BLL.wsIntegracaoWFat.TipoTransacao.CartaoCredito, descricaoIdentificador, BLL.wsIntegracaoWFat.TipoDocumento.CPF, Convert.ToString(numCPF), Convert.ToDecimal(objUsuarioLogado.PessoaFisica.CPF), objUsuarioLogado.PessoaFisica.NomePessoa, mensagem);

                }
                else
                {
                    String numeroCNPJ;
                    if (objPagamento.AdicionalPlano != null)
                    {
                        objPagamento.AdicionalPlano.PlanoAdquirido.Filial.CompleteObject();

                        numeroCNPJ = objPagamento.AdicionalPlano.PlanoAdquirido.Filial.CNPJ.Replace(".", "").Replace("-", "").Replace("/", "");
                    }
                    else
                    {
                        objPagamento.PlanoParcela.CompleteObject();
                        objPagamento.PlanoParcela.PlanoAdquirido.CompleteObject();
                        objPagamento.PlanoParcela.PlanoAdquirido.Filial.CompleteObject();

                        numeroCNPJ = objPagamento.PlanoParcela.PlanoAdquirido.Filial.CNPJ.Replace(".", "").Replace("-", "").Replace("/", "");
                    }

                    string descricaoIdentificador = string.IsNullOrWhiteSpace(objPagamento.DescricaoIdentificador) ? objPagamento.IdPagamento.ToString() : objPagamento.DescricaoIdentificador;

                    if (objPagamento.TipoPagamento.IdTipoPagamento == (int)Enumeradores.TipoPagamento.BoletoBancario)
                        objWsIntegracao.CancelarNotaFiscalIntegracao(BLL.wsIntegracaoWFat.TipoTransacao.Boleto, descricaoIdentificador, BLL.wsIntegracaoWFat.TipoDocumento.CNPJ, Convert.ToString(numeroCNPJ), Convert.ToDecimal(objUsuarioLogado.PessoaFisica.CPF), objUsuarioLogado.PessoaFisica.NomePessoa, mensagem);
                    else
                        objWsIntegracao.CancelarNotaFiscalIntegracao(BLL.wsIntegracaoWFat.TipoTransacao.CartaoCredito, descricaoIdentificador, BLL.wsIntegracaoWFat.TipoDocumento.CNPJ, Convert.ToString(numeroCNPJ), Convert.ToDecimal(objUsuarioLogado.PessoaFisica.CPF), objUsuarioLogado.PessoaFisica.NomePessoa, mensagem);

                }
            }
        }
        #endregion

        #region SetInstance_NonDispose
        /// <summary>
        /// Método auxiliar utilizado pelos métodos LoadObject e CompleteObject para percorrer um IDataReader e vincular as colunas com os atributos da classe.
        /// </summary>
        /// <param name="dr">Cursor de leitura do banco de dados.</param>
        /// <param name="objPagamento">Instância a ser manipulada.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static bool SetInstance_NonDispose(IDataReader dr, Pagamento objPagamento)
        {
            objPagamento._idPagamento = Convert.ToInt32(dr["Idf_Pagamento"]);

            if (dr["Idf_Filial"] != DBNull.Value)
                objPagamento._filial = new Filial(Convert.ToInt32(dr["Idf_Filial"]));
            if (dr["Idf_Tipo_Pagamento"] != DBNull.Value)
                objPagamento._tipoPagamento = new TipoPagamento(Convert.ToInt32(dr["Idf_Tipo_Pagamento"]));
            objPagamento._usuarioFilialPerfil = new UsuarioFilialPerfil(Convert.ToInt32(dr["Idf_Usuario_Filial_Perfil"]));
            if (dr["Dta_Emissao"] != DBNull.Value)
                objPagamento._dataEmissao = Convert.ToDateTime(dr["Dta_Emissao"]);
            if (dr["Dta_Vencimento"] != DBNull.Value)
                objPagamento._dataVencimento = Convert.ToDateTime(dr["Dta_Vencimento"]);
            if (dr["Des_Identificador"] != DBNull.Value)
                objPagamento._descricaoIdentificador = Convert.ToString(dr["Des_Identificador"]);
            if (dr["Des_Descricao"] != DBNull.Value)
                objPagamento._descricaoDescricao = Convert.ToString(dr["Des_Descricao"]);
            objPagamento._valorPagamento = Convert.ToDecimal(dr["Vlr_Pagamento"]);
            objPagamento._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
            objPagamento._flagAvulso = Convert.ToBoolean(dr["Flg_Avulso"]);
            objPagamento._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
            objPagamento._pagamentoSituacao = new PagamentoSituacao(Convert.ToInt32(dr["Idf_Pagamento_Situacao"]));
            if (dr["Idf_Operadora"] != DBNull.Value)
                objPagamento._operadora = new Operadora(Convert.ToInt32(dr["Idf_Operadora"]));
            if (dr["Idf_Plano_Parcela"] != DBNull.Value)
                objPagamento._planoParcela = new PlanoParcela(Convert.ToInt32(dr["Idf_Plano_Parcela"]));
            if (dr["IDF_Usuario_Gerador"] != DBNull.Value)
                objPagamento._usuarioGerador = new UsuarioFilialPerfil(Convert.ToInt32(dr["IDF_Usuario_Gerador"]));
            if (dr["Cod_Guid"] != DBNull.Value)
                objPagamento._codigoGuid = Convert.ToString(dr["Cod_Guid"]);
            if (dr["Idf_Adicional_Plano"] != DBNull.Value)
                objPagamento._adicionalPlano = new AdicionalPlano(Convert.ToInt32(dr["Idf_Adicional_Plano"]));
            if (dr["Num_Nota_Fiscal"] != DBNull.Value)
                objPagamento._numeroNotaFiscal = Convert.ToString(dr["Num_Nota_Fiscal"]);
            if (dr["Idf_Codigo_Desconto"] != DBNull.Value)
                objPagamento._codigoDesconto = new CodigoDesconto(Convert.ToInt32(dr["Idf_Codigo_Desconto"]));
            objPagamento._flagbaixado = Convert.ToBoolean(dr["Flg_baixado"]);
            if (dr["Url_Nota_Fiscal"] != DBNull.Value)
                objPagamento._UrlNotaFiscal = Convert.ToString(dr["Url_Nota_Fiscal"]);

            objPagamento._persisted = true;
            objPagamento._modified = false;

            return true;
        }
        #endregion

        #endregion

        [Serializable]
        public class InformacaoPagamento
        {

            public InformacaoPagamento()
            {
                PJ = new List<InformacaoPagamentoBoleto>();
                PF = new List<InformacaoPagamentoBoleto>();
            }

            public List<InformacaoPagamentoBoleto> PJ { get; set; }

            public List<InformacaoPagamentoBoleto> PF { get; set; }

            [Serializable]
            public struct InformacaoPagamentoBoleto
            {
                public int IdPagamento { get; set; }
                public string Plano { get; set; }
                public string Nome { get; set; }
                public decimal NumeroDocumento { get; set; }
                public string NumeroBoleto { get; set; }
                public decimal ValorPlano { get; set; }
                public string Parcela { get; set; }
                public string NotaFiscal { get; set; }
                public string TipoPagamento { get; set; }
            }

            public List<InformacaoPagamentoBoleto> ToList()
            {
                return this.PF.Concat(this.PJ).ToList();
            }

        }

        #region SalvarNotaFiscal
        public void SalvarNotaFiscal(string numeroNf, string linkNf)
        {
            this.NumeroNotaFiscal = numeroNf;
            this.UrlNotaFiscal = linkNf;

            #region Propriedades Integracao
            string desIdentificador, numCPF, numCNPJ, razaoSocial, cep, rua, complemento, bairro, cidade, uf, nomeFantasia, idfCnaePrincipal, emailContato, ddd, telefone, nomeContato, filialGestora;
            DateTime dataPagamento, dataInicioPlano, dataFimPLano;
            decimal valorPagamento;
            int numEndereco, numBanco;
            #endregion

            if (this.PlanoParcela.PlanoAdquirido.ParaPessoaJuridica())
            {
                if (Filial.RecuperarInformacoesIntegracaoFinanceiro(this.IdPagamento, out dataPagamento, out valorPagamento, out desIdentificador, out dataInicioPlano, out dataFimPLano, out numCNPJ, out razaoSocial, out cep, out rua, out numEndereco, out complemento, out bairro, out cidade, out uf, out nomeFantasia, out idfCnaePrincipal, out emailContato, out ddd, out telefone, out nomeContato, out numBanco, out filialGestora))
                {
                    string assunto;
                    string mensagem = CartaEmail.RetornarConteudoBNE(Enumeradores.CartaEmail.MensagemCiaNF, out assunto);

                    mensagem = mensagem.Replace("{link_nfe}", linkNf);

                    var emailRemetente = Parametro.RecuperaValorParametro(Enumeradores.Parametro.EmailRemetenteEnvioNF);

                    EmailSenderFactory.Create(TipoEnviadorEmail.Fila).Enviar(assunto, mensagem, emailRemetente, emailContato);
                }
            }
            else
            {
                if (PessoaFisica.RecuperarInformacoesIntegracaoFinanceiro(this.IdPagamento, out dataPagamento, out valorPagamento, out desIdentificador, out dataInicioPlano, out dataFimPLano, out numCPF, out nomeContato, out cep, out rua, out numEndereco, out complemento, out bairro, out cidade, out uf, out emailContato, out ddd, out telefone, out numBanco, out filialGestora))
                {
                    string assunto;
                    string mensagem = CartaEmail.RetornarConteudoBNE(Enumeradores.CartaEmail.MensagemVipNF, out assunto);

                    mensagem = mensagem.Replace("{link_nfe}", linkNf);

                    var emailRemetente = Parametro.RecuperaValorParametro(Enumeradores.Parametro.EmailRemetenteEnvioNF);

                    EmailSenderFactory.Create(TipoEnviadorEmail.Fila).Enviar(assunto, mensagem, emailRemetente, emailContato);
                }
            }

            this.Save();
        }
        #endregion

    }


}