using BNE.EL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;


namespace BNE.BLL.Custom.RetornoBoleto
{
    public enum TipoArquivo
    {
        RetornoCNR,
        RetornoCOB
    }

    public class Detalhe
    {

        #region NumeroDocumento
        public string NumeroDocumento { get; set; }
        #endregion

        #region DataPagamento
        public string DataPagamento { get; set; }
        #endregion

        #region ValorPago
        public string ValorPago { get; set; }
        #endregion

    }

    public class Arquivo
    {
        #region InterpretarArquivo

        //private MemoryStream arquivo { get; set; }
        //private readonly char identificaCabecalho = '0';
        //private readonly char identificaDetalhe = '1';
        //public List<Detalhe> listaDetalhe = new List<Detalhe>();
        //public TipoArquivo tipoArquivo = TipoArquivo.RetornoCNR;
        //public String codigoOcorrencia = String.Empty;

        //public Arquivo(byte[] arquivo)
        //{
        //    this.arquivo = new MemoryStream(arquivo);
        //}

        //public CobrancaBoletoArquivoRetorno InterpretarArquivo(SqlTransaction trans)
        //{
        //    CobrancaBoletoArquivoRetorno objCobrancaBoletoArquivoRetorno = new CobrancaBoletoArquivoRetorno();
        //    StringBuilder sbArquivo = new StringBuilder();

        //    objCobrancaBoletoArquivoRetorno.DataRetorno = DateTime.Now;
        //    objCobrancaBoletoArquivoRetorno.Save(trans);

        //    string linhaAtual;
        //    Detalhe objDetalhe = new Detalhe();

        //    using (StreamReader sr = new StreamReader(arquivo))
        //    {
        //        while ((linhaAtual = sr.ReadLine()) != null)
        //        {
        //            sbArquivo.AppendLine(linhaAtual);

        //            if (linhaAtual[0] == identificaCabecalho)
        //            {
        //                String servico = linhaAtual.Substring(11, 15).Trim();
        //                switch (servico)
        //                {
        //                    case "COBRANCA CNR":
        //                        tipoArquivo = TipoArquivo.RetornoCNR;
        //                        break;
        //                    case "COBRANCA":
        //                        tipoArquivo = TipoArquivo.RetornoCOB;
        //                        break;
        //                    default:
        //                        break;
        //                }
        //                continue;
        //            }
        //            if (linhaAtual[0] == identificaDetalhe)
        //            {
        //                String nossoNumero = string.Empty;
        //                String codigoRetorno = string.Empty;
        //                Boolean pago = false;

        //                Detalhe det = new Detalhe { NumeroDocumento = (linhaAtual.Substring(37, 13)), DataPagamento = (linhaAtual.Substring(110, 6)) };
        //                switch (tipoArquivo)
        //                {
        //                    case TipoArquivo.RetornoCNR:
        //                        nossoNumero = linhaAtual.Substring(37, 13);
        //                        codigoRetorno = "CNR" + linhaAtual.Substring(108, 2).Trim();
        //                        if (codigoRetorno == "CNR06")
        //                        {
        //                            pago = true;
        //                        }
        //                        break;
        //                    case TipoArquivo.RetornoCOB:
        //                        nossoNumero = linhaAtual.Substring(37, 13);
        //                        codigoRetorno = "COB" + linhaAtual.Substring(108, 2).Trim();
        //                        if (Regex.IsMatch(codigoRetorno, "COB(0(6|7)|1(5|6)|3(1|2|3|6|8|9))"))
        //                        {
        //                            pago = true;
        //                        }
        //                        break;
        //                    default:
        //                        break;
        //                }

        //                CobrancaBoletoListaRetorno objCobrancaBoletoListaRetorno = new CobrancaBoletoListaRetorno();
        //                CobrancaBoleto objCobrancaBoleto;
        //                if (!CobrancaBoleto.CarregarPeloNossoNumero(nossoNumero, out objCobrancaBoleto))
        //                {
        //                    continue;
        //                }

        //                objCobrancaBoletoListaRetorno.CobrancaBoleto = objCobrancaBoleto;
        //                objCobrancaBoletoListaRetorno.IdCobrancaBoletoArquivoRetorno = objCobrancaBoletoArquivoRetorno.IdCobrancaBoletoArquivoRetorno;
        //                objCobrancaBoletoListaRetorno.Save(trans);

        //                CobrancaBoletoLOG objCobrancaBoletoLOG = new CobrancaBoletoLOG();
        //                objCobrancaBoletoLOG.CobrancaBoleto = objCobrancaBoleto;
        //                objCobrancaBoletoLOG.DataTransacao = DateTime.Now;
        //                objCobrancaBoletoLOG.DescricaoTransacao = "Arquivo de Retorno";

        //                MensagemRetornoBoleto objMensagemRetornoBoleto;
        //                if (!MensagemRetornoBoleto.RecuperarPorCodigo(codigoRetorno, out objMensagemRetornoBoleto))
        //                {
        //                    throw new RecordNotFoundException(typeof(MensagemRetornoBoleto));
        //                }
        //                objCobrancaBoletoLOG.MensagemRetornoBoleto = objMensagemRetornoBoleto;

        //                objCobrancaBoletoLOG.Save(trans);

        //                //Definindo Transação Global como Paga
        //                if (pago)
        //                {
        //                    objCobrancaBoleto.CobrancaBoletoTransacao.CompleteObject();
        //                    objCobrancaBoleto.CobrancaBoletoTransacao.CobrancaBoletoStatusTransacao = new CobrancaBoletoStatusTransacao((int)BLL.Enumeradores.CobrancaBoletoStatusTransacao.Pago);
        //                    objCobrancaBoleto.CobrancaBoletoTransacao.Save(trans);
        //                    this.listaDetalhe.Add(det);
        //                }

        //                continue;
        //            }
        //        }

        //    }


        //    objCobrancaBoletoArquivoRetorno.ArquivoRetorno = sbArquivo.ToString();
        //    return objCobrancaBoletoArquivoRetorno;
        //}


        #endregion
    }

    public class BNE
    {
        #region Consultas

        #region Spselectpornumeroboleto

        private const string Spselectpornumeroboleto = @"
        select pg.Idf_Pagamento
        from [bne].[bne_pagamento] pg with (nolock)
            left join [bne].[bne_plano_parcela] pp with (nolock) on pg.Idf_Plano_Parcela = pp.Idf_Plano_Parcela
        where pg.Des_Identificador = @Numero_Boleto";

        #endregion

        #region Spselectquempagoupf

        private const string Spselectquempagoupf = @"
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
        from bne.bne_pagamento pg with (nolock)
			join bne.bne_pagamento_Situacao pgs with(nolock) on pg.Idf_Pagamento_Situacao = pgs.Idf_Pagamento_Situacao
	        left join bne.tab_usuario_filial_perfil ufp with (nolock) on pg.Idf_Usuario_Filial_Perfil = ufp.Idf_Usuario_Filial_Perfil
	        left join bne.tab_pessoa_fisica pf with (nolock) on ufp.Idf_Pessoa_Fisica = pf.Idf_Pessoa_Fisica
	        left join bne.bne_plano_parcela pp with (nolock) on pg.Idf_Plano_Parcela = pp.Idf_Plano_Parcela
	        left join bne.bne_plano_adquirido pa with (nolock) on pp.Idf_Plano_Adquirido = pa.Idf_Plano_Adquirido
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
	        left join bne.bne_plano pl with (nolock) on pa.Idf_Plano = pl.Idf_Plano
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
        where pg.Des_Identificador = @Numero_Boleto
            and pl.Idf_Plano_Tipo = 1
        order by 2 desc";


        #endregion

        #region SPselectquempagoupj

        private const string Spselectquempagoupj = @"
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
	            , convert(varchar,pg.Dta_Emissao, 103) as 'Data Emissao'
	            , convert(varchar,pg.Dta_Vencimento,103) as 'Data Vencimento'
	            , convert(varchar,pp.Dta_Pagamento,103) as 'Data Pagamento'
                , pp.Dta_Pagamento
                , pg.Idf_Pagamento
                , pg.Flg_Baixado
             from bne.bne_pagamento pg with (nolock)
				join bne.bne_pagamento_Situacao pgs with(nolock) on pg.Idf_Pagamento_Situacao = pgs.Idf_Pagamento_Situacao
	            join bne.tab_filial fl with (nolock) on pg.Idf_Filial = fl.Idf_Filial
	            join bne.tab_endereco en with (nolock) on fl.Idf_Endereco = en.Idf_Endereco	
	            join plataforma.TAB_Cidade ci WITH (NOLOCK) ON en.Idf_Cidade = ci.Idf_Cidade
	            left join bne.BNE_Plano_Parcela pp WITH (NOLOCK) ON pg.Idf_Plano_Parcela = pp.Idf_Plano_Parcela
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
            where pg.Des_Identificador = @Numero_Boleto 
                and pl.Idf_Plano_Tipo = 2	
            order by 2 desc";
        #endregion

        #endregion

        #region Métodos

        #region LiberarPagamentos

        #region ConverterData

        public static DateTime ConverterData(string dataPagamento)
        {
            string recebe_data = string.Empty;

            Regex formato = new Regex(@"^(\d{2})(\d{2})(\d{2})$");

            if (formato.IsMatch(dataPagamento))
                recebe_data = formato.Replace(dataPagamento, "$1/$2/$3");

            DateTime dataDetalhe = Convert.ToDateTime(recebe_data);

            return dataDetalhe;
        }

        #endregion

        //public static Pagamento.InformacaoPagamento LiberarPagamentos(byte[] arquivo, int idUsuarioLogado)
        //{
        //    var informacaoPagamento = new Pagamento.InformacaoPagamento();

        //    using (SqlConnection conn = new SqlConnection(DataAccessLayer.CONN_STRING))
        //    {
        //        conn.Open();

        //        using (SqlTransaction trans = conn.BeginTransaction())
        //        {
        //            try
        //            {
        //                var arquivoBoleto = new Arquivo(arquivo);
        //                arquivoBoleto.InterpretarArquivo(trans);

        //                var listaPagamento = new List<Pagamento>();
        //                if (arquivoBoleto.listaDetalhe.Count > 0)
        //                {
        //                    foreach (var detalhe in arquivoBoleto.listaDetalhe)
        //                    {
        //                        var timer = Stopwatch.StartNew();
        //                        var idPagamento = RecuperarIdPagamento(detalhe);

        //                        if (idPagamento != 0)
        //                        {
        //                            #region Liberação Pagamentos BNE

        //                            var objPagamento = Pagamento.LoadObject(idPagamento, trans);

        //                            if (objPagamento.AdicionalPlano != null)
        //                            {
        //                                objPagamento.AdicionalPlano.CompleteObject();
        //                                objPagamento.AdicionalPlano.PlanoAdquirido.CompleteObject();
        //                            }
        //                            else
        //                            {

        //                                objPagamento.PlanoParcela.CompleteObject();
        //                                objPagamento.PlanoParcela.PlanoAdquirido.CompleteObject();

        //                                #region Cancela demais boletos

        //                                var listaPgMesmaParcela = Pagamento.RecuperarPagamentosMesmaParcela(objPagamento.PlanoParcela.IdPlanoParcela, idPagamento);

        //                                foreach (int pagamento in listaPgMesmaParcela)
        //                                {
        //                                    var objPagamentoAux = BLL.Pagamento.LoadObject(pagamento);

        //                                    objPagamentoAux.PagamentoSituacao = new PagamentoSituacao(Convert.ToInt32(Enumeradores.PagamentoSituacao.Cancelado));

        //                                    objPagamentoAux.Save(trans);
        //                                }

        //                                #endregion
        //                            }

        //                            //se o objeto Pagamento ja estiver marcado como pago, não reefetua o pagamento
        //                            if (!objPagamento.JaPago(trans))
        //                            {
        //                                var dataDetalhe = ConverterData(detalhe.DataPagamento);

        //                                objPagamento.TipoPagamento = new TipoPagamento((int)Enumeradores.TipoPagamento.BoletoBancario);
        //                                objPagamento.Liberar(trans, dataDetalhe);
        //                            }
        //                            else
        //                            {
        //                                if (DateTime.Now.DayOfWeek == DayOfWeek.Saturday)
        //                                    objPagamento.PlanoParcela.DataPagamento = DateTime.Now.AddDays(-3);
        //                                else
        //                                    objPagamento.PlanoParcela.DataPagamento = DateTime.Now.AddDays(-1);
        //                                objPagamento.PlanoParcela.PlanoParcelaSituacao = new PlanoParcelaSituacao((int)Enumeradores.PlanoParcelaSituacao.Pago);
        //                                objPagamento.PlanoParcela.Save(trans);
        //                            }

        //                            // se o PlanoParcela do objeto Pagamento for null, é um AdicionalPlano, que só é vendido para PJs
        //                            if (null != objPagamento.PlanoParcela &&
        //                                objPagamento.PlanoParcela.PlanoAdquirido.ParaPessoaFisica(trans))
        //                                QuemPagouPF(informacaoPagamento.PF, detalhe, trans);
        //                            else
        //                                QuemPagouPJ(informacaoPagamento.PJ, detalhe, trans);

        //                            #endregion
        //                        }
        //                        else
        //                        {
        //                            #region Liberação Pagamentos Sine

        //                            try
        //                            {
        //                                var dataDetalhe = ConverterData(detalhe.DataPagamento);
        //                                using (var objSine = new wsSine.AppClient())
        //                                {
        //                                    var objUsuarioLogado = UsuarioFilialPerfil.LoadObject(idUsuarioLogado);

        //                                    objUsuarioLogado.PessoaFisica.CompleteObject();

        //                                    var retorno = objSine.LiberarDestaqueVaga(detalhe.NumeroDocumento, objUsuarioLogado.PessoaFisica.NomePessoa, objUsuarioLogado.PessoaFisica.NumeroCPF, dataDetalhe);

        //                                    if (retorno != null)
        //                                        QuemPagouSine(informacaoPagamento.PJ, retorno.Nome, retorno.NotaFiscal, retorno.NumeroBoleto, retorno.NumeroDocumento, retorno.Parcela, retorno.Plano, retorno.ValorPlano);
        //                                }
        //                            }
        //                            catch (Exception ex)
        //                            {
        //                                EL.GerenciadorException.GravarExcecao(ex);
        //                            }

        //                            #endregion
        //                        }
        //                    }
        //                }
        //                trans.Commit();
        //            }
        //            catch (Exception ex)
        //            {
        //                EL.GerenciadorException.GravarExcecao(ex);
        //                trans.Rollback();
        //            }
        //        }
        //    }

        //    return informacaoPagamento;
        //}
        #endregion

        #region RecuperarIdPagamento

        public static int RecuperarIdPagamento(Detalhe objDetalhe)
        {

            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Numero_Boleto", SqlDbType = SqlDbType.VarChar, Size = 13, Value = objDetalhe.NumeroDocumento }
                };

            return Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, Spselectpornumeroboleto, parms));

        }

        #endregion

        //#region QuemPagouPF

        //public static void QuemPagouPF(List<Pagamento.InformacaoPagamento.InformacaoPagamentoBoleto> listaPagamentoBoleto, Detalhe objDetalhe, SqlTransaction trans)
        //{
        //    var parms = new List<SqlParameter>
        //        {
        //            new SqlParameter { ParameterName = "@Numero_Boleto", SqlDbType = SqlDbType.VarChar, Size = 13, Value = objDetalhe.NumeroDocumento }
        //        };

        //    using (IDataReader dr = DataAccessLayer.ExecuteReader(trans, CommandType.Text, Spselectquempagoupf, parms))
        //    {
        //        if (dr.Read())
        //        {
        //            //var dataRow = dt.NewRow();

        //            //dataRow["Plano"] = dr["Des_Funcao_Categoria"].ToString();
        //            //dataRow["Nome / Razão Social"] = dr["Nme_Pessoa"].ToString();
        //            //dataRow["CPF / CNPJ"] = dr["Num_CPF"].ToString();
        //            //dataRow["Cidade"] = dr["Nme_Cidade"].ToString() + '-' + dr["Sig_Estado"].ToString();
        //            //dataRow["Numero Boleto"] = dr["Des_Identificador"].ToString();
        //            //dataRow["Vlr Plano"] = dr["Vlr_Base"].ToString();
        //            //dataRow["Vlr Boleto"] = dr["Vlr_Pagamento"].ToString();
        //            //dataRow["Vlr Pago"] = dr["Vlr_Pagamento"].ToString();
        //            //if (Convert.ToInt32(dr["Parcela"]) == 0)
        //            //    dataRow["Parcela"] = "1 de " + dr["Qtd_Parcela"].ToString();
        //            //else
        //            //    dataRow["Parcela"] = dr["Parcela"].ToString() + " de " + dr["Qtd_Parcela"].ToString();
        //            //dataRow["Situacao"] = dr["Situacao"].ToString();
        //            //dataRow["Nota Fiscal"] = dr["NF"].ToString();
        //            //dataRow["Data Emissao"] = dr["Data Emissao"].ToString();
        //            //dataRow["Data Vencimento"] = dr["Data Vencimento"].ToString();
        //            //dataRow["Data Pagamento"] = dr["Data Pagamento"].ToString();
        //            //dataRow["Idf_Pagamento"] = Convert.ToInt32(dr["Idf_Pagamento"].ToString());
        //            //if (dr["Flg_Baixado"] == DBNull.Value || Convert.ToBoolean(dr["Flg_Baixado"].ToString()) == false)
        //            //    dataRow["Cr Baixado"] = "Não";
        //            //else
        //            //    dataRow["Cr Baixado"] = "Sim";

        //            //dt.Rows.Add(dataRow);

        //            var parcela = Convert.ToInt32(dr["Parcela"]);
        //            var quantidadeParcela = Convert.ToInt32(dr["Qtd_Parcela"]);

        //            var informacaoBoleto = new Pagamento.InformacaoPagamento.InformacaoPagamentoBoleto
        //            {
        //                Nome = dr["Nme_Pessoa"].ToString(),
        //                NotaFiscal = dr["NF"].ToString(),
        //                NumeroBoleto = dr["Des_Identificador"].ToString(),
        //                NumeroDocumento = Convert.ToDecimal(dr["Num_CPF"]),
        //                Parcela = parcela == 0 ? "1 de " + quantidadeParcela : parcela + " de " + quantidadeParcela,
        //                Plano = dr["Des_Funcao_Categoria"].ToString(),
        //                //TipoPagamento = ,
        //                ValorPlano = Convert.ToDecimal(dr["Vlr_Pagamento"])
        //            };
        //            listaPagamentoBoleto.Add(informacaoBoleto);

        //        }
        //    }
        //}

        //#endregion

        //#region QuemPagouPJ

        //public static void QuemPagouPJ(List<Pagamento.InformacaoPagamento.InformacaoPagamentoBoleto> listaPagamentoBoleto, Detalhe objDetalhe, SqlTransaction trans)
        //{

        //    var parms = new List<SqlParameter>
        //    {
        //        new SqlParameter { ParameterName = "@Numero_Boleto", SqlDbType = SqlDbType.VarChar, Size = 13, Value = objDetalhe.NumeroDocumento}
        //    };

        //    using (IDataReader dr = DataAccessLayer.ExecuteReader(trans, CommandType.Text, Spselectquempagoupj, parms))
        //    {
        //        if (dr.Read())
        //        {
        //            //var dataRow = dt.NewRow();

        //            //dataRow["Plano"] = dr["Des_Plano"].ToString();
        //            //dataRow["Nome / Razão Social"] = dr["Raz_Social"].ToString();
        //            //dataRow["CPF / CNPJ"] = dr["Num_CNPJ"].ToString();
        //            //dataRow["Cidade"] = dr["Nme_Cidade"].ToString() + '-' + dr["Sig_Estado"].ToString();
        //            //dataRow["Numero Boleto"] = dr["Des_Identificador"].ToString();
        //            //dataRow["Vlr Plano"] = dr["Vlr_Base"].ToString();
        //            //dataRow["Vlr Boleto"] = dr["Vlr_Pagamento"].ToString();
        //            //dataRow["Vlr Pago"] = dr["Vlr_Pagamento"].ToString();
        //            //if (Convert.ToInt32(dr["Parcela"]) == 0)
        //            //    dataRow["Parcela"] = "1 de " + dr["Qtd_Parcela"].ToString();
        //            //else
        //            //    dataRow["Parcela"] = dr["Parcela"].ToString() + " de " + dr["Qtd_Parcela"].ToString();
        //            //dataRow["Situacao"] = dr["Situacao"].ToString();
        //            //if (dr["NF"] != DBNull.Value)
        //            //    dataRow["Nota Fiscal"] = dr["NF"].ToString();
        //            //dataRow["Data Emissao"] = dr["Data Emissao"].ToString();
        //            //dataRow["Data Vencimento"] = dr["Data Vencimento"].ToString();
        //            //dataRow["Data Pagamento"] = dr["Data Pagamento"].ToString();
        //            //dataRow["Idf_Pagamento"] = Convert.ToInt32(dr["Idf_Pagamento"].ToString());
        //            //if (dr["Flg_Baixado"] == DBNull.Value || Convert.ToBoolean(dr["Flg_Baixado"].ToString()) == false)
        //            //    dataRow["Cr Baixado"] = "Não";
        //            //else
        //            //    dataRow["Cr Baixado"] = "Sim";

        //            //dt.Rows.Add(dataRow);

        //            var parcela = Convert.ToInt32(dr["Parcela"]);
        //            var quantidadeParcela = Convert.ToInt32(dr["Qtd_Parcela"]);

        //            var informacaoBoleto = new Pagamento.InformacaoPagamento.InformacaoPagamentoBoleto
        //            {
        //                Nome = dr["Raz_Social"].ToString(),
        //                NotaFiscal = dr["NF"].ToString(),
        //                NumeroBoleto = dr["Des_Identificador"].ToString(),
        //                NumeroDocumento = Convert.ToDecimal(dr["Num_CNPJ"]),
        //                Parcela = parcela == 0 ? "1 de " + quantidadeParcela : parcela + " de " + quantidadeParcela,
        //                Plano = dr["Des_Plano"].ToString(),
        //                //TipoPagamento = ,
        //                ValorPlano = Convert.ToDecimal(dr["Vlr_Pagamento"])
        //            };
        //            listaPagamentoBoleto.Add(informacaoBoleto);
        //        }
        //    }
        //}
        //#endregion

        //#region QuemPagouSine
        //public static void QuemPagouSine(List<Pagamento.InformacaoPagamento.InformacaoPagamentoBoleto> listaPagamentoBoleto, string nome, string notaFiscal, string numeroBoleto, decimal numeroDocumento, string parcela, string descricaoPlano, decimal valorPagamento)
        //{
        //    var informacaoBoleto = new Pagamento.InformacaoPagamento.InformacaoPagamentoBoleto
        //    {
        //        Nome = nome,
        //        NotaFiscal = notaFiscal,
        //        NumeroBoleto = numeroBoleto,
        //        NumeroDocumento = numeroDocumento,
        //        Parcela = parcela,
        //        Plano = descricaoPlano,
        //        ValorPlano = valorPagamento
        //    };
        //    listaPagamentoBoleto.Add(informacaoBoleto);
        //}
        //#endregion

        #endregion

    }
}


