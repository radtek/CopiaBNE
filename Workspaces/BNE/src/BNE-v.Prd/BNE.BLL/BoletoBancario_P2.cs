using BNE.BLL.Custom;
using BNE.BLL.Custom.Email;
using BNE.BLL.Enumeradores;
using BNE.EL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace BNE.BLL
{
    public partial class BoletoBancario
    {

        #region Consultas
        private const string SP_NOSSO_NUMERO = @"SELECT * FROM BNE.BNE_Boleto_Bancario WITH(NOLOCK)  WHERE Num_Nosso_Numero = @NossoNumero";



        #region [spCarregarBoletoDeParcela]
        private const string spCarregarBoletoDeParcela = @"SELECT * FROM BNE.BNE_Boleto_Bancario WITH(NOLOCK)  WHERE idf_pagamento = @Idf_Pagamento";
        #endregion
        private const string spRecuperarUltimioBoletoNaoPago = @"select top 1 p.idf_pagamento from bne.bne_plano_adquirido pa with(nolock)
                            join bne.BNE_Plano_Parcela pp with(nolock) on pp.Idf_Plano_Adquirido = pa.Idf_Plano_Adquirido
                            join bne.BNE_Pagamento p with(nolock) on p.Idf_Plano_Parcela = pp.Idf_Plano_Parcela
                            where pa.idf_filial = @Idf_Filial 
                            and pa.Idf_Plano_Situacao in(0, 1) --Aguardando Liberacao - Liberado
                            and p.idf_tipo_pagamento = 2 -- boleto bancario
                             and p.Idf_Pagamento_Situacao = 1 -- Em Aberto
                            and pp.idf_plano_parcela_situacao = 1--Em Aberto
                            order by pa.Dta_Cadastro desc, p.dta_vencimento asc";
        #endregion


        #region [spConsultarPagamentoBoletosPendente]

        private const string spConsultarPagamentoBoletosPendente = @"select bb.Idf_Boleto_Bancario,pag.Des_Identificador
                            from bne.bne_boleto_bancario bb with(nolock)
                                 join bne.bne_pagamento pag with(nolock) on pag.idf_pagamento = bb.idf_pagamento
                             where  pag.Idf_Pagamento_Situacao <> 2 -- pago
                                 and pag.Idf_Tipo_Pagamento = 2 --boleto
                                 and bb.url_boleto is not null 
								 and pag.dta_vencimento > getdate()-60 ";
        #endregion
        #region Metodos


        #region GerarBoletoPagamentoAdicionalNovo
        public static List<DTO.DTOBoletoPagarMe> GerarBoletoPagamentoAdicionalNovo(decimal pagamentoAdicionalValorTotal, int pagamentoAdicionalQuantidade, int idFilial, int idUsuarioFilialPerfilLogadoEmpresa, ref int pagamentoIdentificadorPagamento)
        {
            var objFilial = new Filial(idFilial);

            PlanoAdquirido objPlanoAdquirido;
            PlanoAdquirido.CarregarPlanoAdquiridoPorSituacao(objFilial, (int)Enumeradores.PlanoSituacao.Liberado, out objPlanoAdquirido);

            if (objPlanoAdquirido != null)
            {
                var objUsuarioFilialPerfil = UsuarioFilialPerfil.LoadObject(idUsuarioFilialPerfilLogadoEmpresa);

                BLL.Pagamento objPagamento;
                AdicionalPlano.CriarPagamentoEPlanoAdicionalSMS(objPlanoAdquirido, pagamentoAdicionalValorTotal, pagamentoAdicionalQuantidade, objUsuarioFilialPerfil, Enumeradores.TipoPagamento.BoletoBancario, DateTime.Now, DateTime.Today, out objPagamento);

                pagamentoIdentificadorPagamento = objPagamento.IdPagamento;
                var listaPagamento = new List<Pagamento> { objPagamento };

                return CriarBoletosPagarMe(listaPagamento);
            }
            return null;
        }
        #endregion


        #region CriarBoletos
        /// <summary>
        /// novo para criar pagamento e boleto novo para a parcela
        /// </summary>
        /// <param name="pagamentos"></param>
        /// <param name="novo"></param>
        /// <returns></returns>
        public static List<DTO.DTOBoletoPagarMe> CriarBoletosPagarMe(List<Pagamento> pagamentos, bool BoletoNovo = false, DateTime? DtaNova = null)
        {
            using (var conn = new SqlConnection(BLL.DataAccessLayer.CONN_STRING))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        List<DTO.DTOBoletoPagarMe> boletos = new List<DTO.DTOBoletoPagarMe>();

                        pagamentos.RemoveAll(p => p == null);

                        foreach (var objPagamento in pagamentos)
                            boletos.Add(BLL.PagarMeOperacoes.GerarBoleto(objPagamento, trans, BoletoNovo, DtaNova));

                        trans.Commit();

                        return boletos;
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        EL.GerenciadorException.GravarExcecao(ex);
                        throw ex;
                    }
                }
            }
        }
        #endregion

        #region ExisteBoletoParaParcela
        public static BoletoBancario CarregarBoletoDeParcela(string NumNossoNumero)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@NossoNumero", SqlDbType.VarChar, 20));
            parms[0].Value = NumNossoNumero;


            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SP_NOSSO_NUMERO, parms))
            {
                while (dr.Read())
                {
                    return BoletoBancario.LoadObject(Convert.ToInt32(dr["Idf_Boleto_Bancario"]));
                }

            }
            return null;
        }
        #endregion

        #region GerarFormatoArquivoDeBoleto
        private static string GerarFormatoArquivoDeBoleto(Enumeradores.FormatoBoleto formatoBoleto)
        {
            switch (formatoBoleto)
            {
                case Enumeradores.FormatoBoleto.IMAGEM:
                    return string.Format("boleto_{0}.jpg", DateTime.Now.Ticks);
                case Enumeradores.FormatoBoleto.PDF:
                    return string.Format("boleto_{0}.pdf", DateTime.Now.Ticks);
                default:
                    return string.Empty;
            }
        }
        #endregion

        #region GerarByteArquivoDeBoleto
        private static byte[] GerarByteArquivoDeBoleto(string html, Enumeradores.FormatoBoleto formatoBoleto)
        {
            switch (formatoBoleto)
            {
                case Enumeradores.FormatoBoleto.IMAGEM:
                    return (new NReco.ImageGenerator.HtmlToImageConverter()).GenerateImage(html, NReco.ImageGenerator.ImageFormat.Jpeg);
                case Enumeradores.FormatoBoleto.PDF:


                    var htmlToPdf = new NReco.PdfGenerator.HtmlToPdfConverter();
                    try
                    {
                        htmlToPdf.TempFilesPath = ConfigurationManager.AppSettings["ArquivosTemporarios"].ToString();

                    }
                    catch (Exception ex)
                    {
                        EL.GerenciadorException.GravarExcecao(ex, "erro pdf GerarByteArquivoDeBoleto");
                    }
                    return htmlToPdf.GeneratePdf(html);

                default:
                    return null;
            }
        }
        #endregion

        #region CriarAnexoParaEnviarPorEmail
        public static byte[] CriarAnexoParaEnviarPorEmail(string html, Enumeradores.FormatoBoleto formatoBoleto)
        {
            return GerarByteArquivoDeBoleto(html, formatoBoleto);
        }
        #endregion

        #region CriarConteudoParaEnviarPorEmail
        public static string CriarConteudoParaEnviarPorEmail(string conteudo, string html)
        {
            string mensagem = string.Empty;

            if (!string.IsNullOrEmpty(conteudo))
            {
                return conteudo + "</br></br></br></br></br></br></br></br></br></br>" + html;
            }
            else if (!string.IsNullOrEmpty(html))
            {
                return html;
            }
            else
            {
                return string.Empty;
            }
        }
        #endregion

        #region RetornarBoleto

        #region RetornarBoleto - string html
        public static string RetornarBoleto(string html, Enumeradores.FormatoBoleto formatoBoleto)
        {
            string strNomeArquivo = GerarFormatoArquivoDeBoleto(formatoBoleto);
            byte[] byteArray = MontarBoletoBytes(html, formatoBoleto);

            return GerarLinkDownload(byteArray, strNomeArquivo);
        }
        #endregion

        #region RetornarBoleto - BLL.Pagamento objPagamento
        public static string RetornarBoleto(BLL.Pagamento objPagamento, out string boletoImg, bool BoletoNovo = false, DateTime? DtaNovo = null)
        {
            List<Pagamento> pagamentos = new List<Pagamento>();
            pagamentos.Add(objPagamento);

            string strNomeArquivoPdf = GerarFormatoArquivoDeBoleto(Enumeradores.FormatoBoleto.PDF);
            string strNomeArquivoImg = GerarFormatoArquivoDeBoleto(Enumeradores.FormatoBoleto.IMAGEM);

            var htmlBoleto = BoletoBancario.GerarLayoutBoletoHTMLPagarMe(CriarBoletosPagarMe(pagamentos, BoletoNovo, DtaNovo));
            byte[] byteArrayPdf = GerarByteArquivoDeBoleto(htmlBoleto, Enumeradores.FormatoBoleto.PDF);
            byte[] byteArrayImg = GerarByteArquivoDeBoleto(htmlBoleto, Enumeradores.FormatoBoleto.IMAGEM);
            boletoImg = GerarLinkDownload(byteArrayImg, strNomeArquivoImg);

            return GerarLinkDownload(byteArrayPdf, strNomeArquivoPdf);
        }
        #endregion

        #region RetornarBoleto - List<BoletoNet.Boleto> boletos
        public static string RetornarBoleto(List<DTO.DTOBoletoPagarMe> boletos, Enumeradores.FormatoBoleto formatoBoleto)
        {
            string strNomeArquivo = GerarFormatoArquivoDeBoleto(formatoBoleto);
            byte[] byteArray = MontarBoletoBytes(GerarLayoutBoletoHTMLPagarMe(boletos), formatoBoleto);

            return GerarLinkDownload(byteArray, strNomeArquivo);
        }
        #endregion

        #region RetornarBoleto - string html, out byte[] pdfArray, out byte[] imgArray, out string urlPDF, out string urlIMG
        public static void RetornarBoleto(string html, out byte[] pdfArray, out byte[] imgArray)
        {
            pdfArray = MontarBoletoBytes(html, Enumeradores.FormatoBoleto.PDF);
            // urlPDF = GerarLinkDownload(pdfArray, GerarFormatoArquivoDeBoleto(Enumeradores.FormatoBoleto.PDF));

            imgArray = MontarBoletoBytes(html, Enumeradores.FormatoBoleto.IMAGEM);
            //urlIMG = GerarLinkDownload(imgArray, GerarFormatoArquivoDeBoleto(Enumeradores.FormatoBoleto.IMAGEM));
        }
        #endregion

        #endregion RetornarBoleto

        #region MontarBytes - Vários
        #region MontarBytes

        public static byte[] MontarBoletoBytes(string html, Enumeradores.FormatoBoleto formatoBoleto)
        {
            return GerarByteArquivoDeBoleto(html, formatoBoleto);
        }

        public static byte[] MontarBoletoBytes(List<Pagamento> pagamentos, Enumeradores.FormatoBoleto formatoBoleto, bool isCobranca = false)
        {
            string html = GerarLayoutBoletoHTMLPagarMe(CriarBoletosPagarMe(pagamentos, isCobranca));
            return GerarByteArquivoDeBoleto(html, formatoBoleto);
        }
        #endregion

        #region MontaBytes
        public static byte[] MontarBoletoBytes(int idPlanoAdquirido, Enumeradores.FormatoBoleto formatoBoleto, SqlTransaction trans = null)
        {
            PlanoAdquirido objPlanoAdquirido = PlanoAdquirido.LoadObject(idPlanoAdquirido);
            if (objPlanoAdquirido != null)
            {
                var parcelas = PlanoParcela.RecuperarParcelasPlanoAdquirido(objPlanoAdquirido);
                var pagamentos = new List<Pagamento>();

                foreach (var objPlanoParcela in parcelas)
                {
                    var listaPagamentoPorParcela = Pagamento.CarregaPagamentosPorPlanoParcela(objPlanoParcela.IdPlanoParcela).Where(p =>
                        p.TipoPagamento.IdTipoPagamento == (int)Enumeradores.TipoPagamento.BoletoBancario
                        && p.PagamentoSituacao.IdPagamentoSituacao == (int)Enumeradores.PagamentoSituacao.EmAberto
                        && p.FlagInativo == false)
                        .OrderByDescending(p => p.IdPagamento)
                        .ToList();
                    pagamentos.Add(listaPagamentoPorParcela.First()); // adiciona o último pagamento

                    if (listaPagamentoPorParcela.Count > 1)//Apenas para parcela com mais de um pagamento aberto
                    {
                        listaPagamentoPorParcela.Remove(listaPagamentoPorParcela.First()); //Remove da lista de pagamentos em aberto
                        Pagamento.CancelarOutrosPagamentosEmAbertoDePlanoParcela(listaPagamentoPorParcela, trans); // Cancela outros pagamentos
                    }
                }
                return MontarBoletoBytes(pagamentos, formatoBoleto);
            }
            return null;
        }
        #endregion
        #endregion

        #region GerarLinkDownload
        /// <summary>
        /// Passa a string para gerar o arquivo para a tela de geração de arquivo (DownloadArquivo.aspx)
        /// </summary>
        /// <param name="strFinal">String de dados que devem estar no arquivo</param>
        /// <param name="nomeArquivo">Nome do arquivo a ser gerado</param>
        private static string GerarLinkDownload(byte[] arrayFinal, string nomeArquivo)
        {
            string caminhoArquivo = GetFilePath(nomeArquivo);

            FileStream fs = null;
            try
            {
                fs = new FileStream(caminhoArquivo, FileMode.CreateNew, FileAccess.Write);
                fs.Write(arrayFinal, 0, arrayFinal.Length);
                fs.Close();
            }
            finally
            {
                if (fs != null)
                    fs.Dispose();
            }

            return GetURL(caminhoArquivo);
        }
        #endregion

        #region GetURL
        private static string GetURL(string filePath)
        {
            return Helper.GetVirtualPath(filePath);
        }
        #endregion

        #region GetFilePath
        private static string GetFilePath(string fileName)
        {
            return String.Format("{0}\\{1}", System.Web.HttpContext.Current.Server.MapPath("ArquivosTemporarios"), fileName);
        }
        #endregion

        #region Metodos Usados em Linha de Arquivo
        #region CarregarPeloNossoNumero
        /// <summary>
        /// Carrega o boleto através do "Nosso Número" do Boleto
        /// </summary>
        /// <param name="nossoNumero">String com o Nosso Número a ser considerado na busca</param>
        /// <param name="objBoletoBancario">Variável onde o objeto será instanciado</param>
        /// <returns>True se um boleto foi encontrado.</returns>
        public static bool CarregarPeloNossoNumero(string nossoNumero, out BoletoBancario objBoletoBancario)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@NossoNumero", SqlDbType.VarChar, 20));
            parms[0].Value = nossoNumero;
            objBoletoBancario = new BoletoBancario();

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SP_NOSSO_NUMERO, parms))
            {
                if (SetInstance(dr, objBoletoBancario))
                {
                    return true;
                }
            }
            return false;
        }
        #endregion
        #endregion

        #region Metodos Usados em Arquivo

        #region SetInstance_WithoutDispose
        /// <summary>
        /// Método auxiliar utilizado pelos métodos LoadObject e CompleteObject para percorrer um IDataReader e vincular as colunas com os atributos da classe.
        /// </summary>
        /// <param name="dr">Cursor de leitura do banco de dados.</param>
        /// <param name="objBoletoBancario">Instância a ser manipulada.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Francisco Ribas</remarks>
        private static bool SetInstance_WithoutDispose(IDataReader dr, BoletoBancario objBoletoBancario)
        {
            try
            {
                if (dr.Read())
                {
                    objBoletoBancario._idBoletoBancario = Convert.ToInt32(dr["Idf_Boleto_Bancario"]);
                    if (dr["Idf_Pagamento"] != DBNull.Value)
                        objBoletoBancario._pagamento = new Pagamento(Convert.ToInt32(dr["Idf_Pagamento"]));
                    if (dr["Idf_Banco"] != DBNull.Value)
                        objBoletoBancario._banco = new Banco(Convert.ToInt32(dr["Idf_Banco"]));
                    if (dr["Dta_Emissao"] != DBNull.Value)
                        objBoletoBancario._dataEmissao = Convert.ToDateTime(dr["Dta_Emissao"]);
                    if (dr["Dta_Vencimento"] != DBNull.Value)
                        objBoletoBancario._dataVencimento = Convert.ToDateTime(dr["Dta_Vencimento"]);
                    if (dr["Cedente_Numero_CNPJCPF"] != DBNull.Value)
                        objBoletoBancario._cedenteNumCNPJCPF = Convert.ToString(dr["Cedente_Numero_CNPJCPF"]);
                    if (dr["Cedente_Nome"] != DBNull.Value)
                        objBoletoBancario._cedenteNome = Convert.ToString(dr["Cedente_Nome"]);
                    if (dr["Cedente_Agencia"] != DBNull.Value)
                        objBoletoBancario._cedenteAgencia = Convert.ToString(dr["Cedente_Agencia"]);
                    if (dr["Cedente_DV_Agencia"] != DBNull.Value)
                        objBoletoBancario._cedenteDVAgencia = Convert.ToString(dr["Cedente_DV_Agencia"]);
                    if (dr["Cedente_Numero_Conta"] != DBNull.Value)
                        objBoletoBancario._cedenteNumeroConta = Convert.ToString(dr["Cedente_Numero_Conta"]);
                    if (dr["Cedente_DVConta"] != DBNull.Value)
                        objBoletoBancario._cedenteDVConta = Convert.ToString(dr["Cedente_DVConta"]);
                    if (dr["Cedente_Codigo"] != DBNull.Value)
                        objBoletoBancario._cedenteCodigo = Convert.ToString(dr["Cedente_Codigo"]);
                    if (dr["Sacado_Numero_CNPJCPF"] != DBNull.Value)
                        objBoletoBancario._sacadoNumCNPJCPF = Convert.ToString(dr["Sacado_Numero_CNPJCPF"]);
                    if (dr["Sacado_Nome"] != DBNull.Value)
                        objBoletoBancario._sacadoNome = Convert.ToString(dr["Sacado_Nome"]);
                    if (dr["Sacado_Endereco_Logradouro"] != DBNull.Value)
                        objBoletoBancario._sacadoEnderecoLogradouro = Convert.ToString(dr["Sacado_Endereco_Logradouro"]);
                    if (dr["Sacado_Endereco_Numero"] != DBNull.Value)
                        objBoletoBancario._sacadoEnderecoNumero = Convert.ToString(dr["Sacado_Endereco_Numero"]);
                    if (dr["Sacado_Endereco_Bairro"] != DBNull.Value)
                        objBoletoBancario._sacadoEnderecoBairro = Convert.ToString(dr["Sacado_Endereco_Bairro"]);
                    if (dr["Sacado_Endereco_Cidade"] != DBNull.Value)
                        objBoletoBancario._sacadoEnderecoCidade = new Cidade(Convert.ToInt32(dr["Sacado_Endereco_Cidade"]));
                    if (dr["Sacado_Endereco_CEP"] != DBNull.Value)
                        objBoletoBancario._sacadoEnderecoCEP = Convert.ToString(dr["Sacado_Endereco_CEP"]);
                    if (dr["Instrucao_Descricao"] != DBNull.Value)
                        objBoletoBancario._instrucaoDescricao = Convert.ToString(dr["Instrucao_Descricao"]);
                    if (dr["Num_Nosso_Numero"] != DBNull.Value)
                        objBoletoBancario._instrucaoDescricao = Convert.ToString(dr["Num_Nosso_Numero"]);
                    if (dr["Valor_Boleto"] != DBNull.Value)
                        objBoletoBancario._instrucaoDescricao = Convert.ToString(dr["Valor_Boleto"]);

                    objBoletoBancario._flagEmpresa = Convert.ToBoolean(dr["Flg_Empresa"]);

                    objBoletoBancario._persisted = true;
                    objBoletoBancario._modified = false;

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region Liquidar
        /// <summary>
        /// Faz a liquidação(pagamento) do boleto.
        /// </summary>
        /// <param name="trans"></param>
        /// <returns></returns>
        public bool Liquidar(SqlTransaction trans, DateTime dataPagamento, int idUsuarioLogado, decimal valorPagamento)
        {
            Pagamento objPagamento = null;

            if (Pagamento.CarregarPagamentoPorNossoNumeroBoleto(this.NumeroNossoNumero, out objPagamento))
            {
                //Caso o valor seja diferente informa o financeiro que o valor do pagamento estava incorreto
                if (!objPagamento.ValorPagamento.Equals(valorPagamento))
                {
                    InformaValorDiferentePagoEGerado(objPagamento, valorPagamento);
                    return false;
                }
                this.Pagamento = objPagamento;
            }
            else if (Pagamento.CarregarPagamentoEmAbertoOuPagoDePagamentoCanceladoPeloNossoNumero(this.NumeroNossoNumero, out objPagamento))
            { // CASO TENHA SIDO PAGO UM BOLETO CANCELADO

                if (!objPagamento.JaPago(trans)) //Caso 
                {
                    Pagamento objPagamentoCancelado = null;
                    Pagamento.CarregarPagamentoPorNossoNumeroBoleto(this.NumeroNossoNumero, out objPagamentoCancelado, Enumeradores.PagamentoSituacao.Cancelado);

                    if (objPagamento.ValorPagamento == valorPagamento && objPagamento.ValorPagamento == objPagamentoCancelado.ValorPagamento)
                    {
                        objPagamentoCancelado.PagamentoSituacao = new PagamentoSituacao((int)Enumeradores.PagamentoSituacao.EmAberto);
                        if (string.IsNullOrEmpty(objPagamentoCancelado.NumeroNotaFiscal))
                        {
                            objPagamentoCancelado.NumeroNotaFiscal = objPagamento.NumeroNotaFiscal;
                            objPagamentoCancelado.UrlNotaFiscal = objPagamento.UrlNotaFiscal;
                            objPagamentoCancelado.Save(trans);
                        }
                     

                        objPagamento.PagamentoSituacao = new PagamentoSituacao((int)Enumeradores.PagamentoSituacao.Cancelado);
                        objPagamento.Save(trans);

                        this.Pagamento = objPagamentoCancelado;
                    }
                    else
                    {
                        InformaValorDiferentePagoEGerado(objPagamento, valorPagamento);
                        return false;
                    }
                }
            }

            //Informações adicionais utilizadas na grid de liberação

            if (this.Pagamento != null)
            {

                #region Cancela demais boletos

                if (objPagamento.PlanoParcela != null)
                {
                    var listaPgMesmaParcela = Pagamento.RecuperarPagamentosMesmaParcela(this.Pagamento.PlanoParcela.IdPlanoParcela, this.Pagamento.IdPagamento, trans);

                    foreach (int pagamento in listaPgMesmaParcela)
                    {
                        var objPagamentoAux = BLL.Pagamento.LoadObject(pagamento);

                        objPagamentoAux.PagamentoSituacao = new PagamentoSituacao(Convert.ToInt32(Enumeradores.PagamentoSituacao.Cancelado));

                        objPagamentoAux.Save(trans);
                    }
                }
                #endregion

                //se o objeto Pagamento ja estiver marcado como pago, não reefetua o pagamento
                if (!this.Pagamento.JaPago(trans))
                {
                    this.Pagamento.TipoPagamento = new TipoPagamento((int)Enumeradores.TipoPagamento.BoletoBancario);

                    var primeiraParcela = PlanoParcela.CarregarPrimeiraParcelaPorPlanoAdquirido(this.Pagamento.PlanoParcela.PlanoAdquirido.IdPlanoAdquirido, trans);

                    this.Pagamento.Liberar(trans, dataPagamento);

                    #region [Liberar Saldo]

                    var objPlanoAdquirido = PlanoAdquirido.LoadObject(this.Pagamento.PlanoParcela.PlanoAdquirido.IdPlanoAdquirido, trans);
                    objPlanoAdquirido.Plano.CompleteObject(trans);
                    if (objPlanoAdquirido.Plano.FlagRecorrente || objPlanoAdquirido.Plano.FlagBoletoRecorrente)
                        PlanoAdquirido.SalvarNovaDataFimPlano(objPlanoAdquirido, trans);

                    #endregion


                    if ((objPlanoAdquirido != null && objPlanoAdquirido.FlagRecorrente) || (objPlanoAdquirido.Plano.CompleteObject() && objPlanoAdquirido.Plano.FlagRecorrente))
                    {
                        RegrasDataFinalPlano(objPlanoAdquirido, trans, primeiraParcela);

                        if (objPlanoAdquirido.Filial != null)
                        {
                            FilialObservacao.SalvarCRM("Plano Recorrente " + objPlanoAdquirido.IdPlanoAdquirido + " Pagamento feito via Boleto",
                            objPlanoAdquirido.Filial, "Pagamento Boleto Plano Recorrente.", trans);
                        }
                    }
                }
                else
                {
                    bool modified = false;
                    if (this.Pagamento.PlanoParcela != null)
                    {
                        if (this.Pagamento.PlanoParcela.DataPagamento == null)
                        {
                            this.Pagamento.PlanoParcela.DataPagamento = DateTime.Now.AddDays(-1);
                            modified = true;
                        }
                        this.Pagamento.PlanoParcela.CompleteObject();
                        if (this.Pagamento.PlanoParcela.PlanoParcelaSituacao.IdPlanoParcelaSituacao != (int)Enumeradores.PlanoParcelaSituacao.Pago)
                        {
                            this.Pagamento.PlanoParcela.PlanoParcelaSituacao = new PlanoParcelaSituacao((int)Enumeradores.PlanoParcelaSituacao.Pago);
                            modified = true;
                        }
                        if (modified)
                            this.Pagamento.PlanoParcela.Save();
                    }
                    else
                    {
                        this.Pagamento.AdicionalPlano.CompleteObject();
                        if (this.Pagamento.AdicionalPlano.AdicionalPlanoSituacao.IdAdicionalPlanoSituacao != (int)Enumeradores.AdicionalPlanoSituacao.Liberado)
                        {
                            this.Pagamento.AdicionalPlano.AdicionalPlanoSituacao = new AdicionalPlanoSituacao((int)Enumeradores.AdicionalPlanoSituacao.Liberado);
                            this.Pagamento.AdicionalPlano.Save();
                        }
                    }

                }
                //Salva o pagamento relacionado com o boleto
                //this.Pagamento = objPagamento;
                this.Save(trans);
            }
            else
            {
                try
                {
                    using (var objSine = new wsSine.AppClient())
                    {
                        UsuarioFilialPerfil objUsuario = UsuarioFilialPerfil.LoadObject(idUsuarioLogado);
                        objUsuario.PessoaFisica.CompleteObject();
                        objSine.LiberarDestaqueVaga(this.NumeroNossoNumero, objUsuario.PessoaFisica.NomeCompleto, objUsuario.PessoaFisica.CPF.ToString(), dataPagamento);
                    }
                }
                catch (Exception ex)
                {
                    EL.GerenciadorException.GravarExcecao(ex);
                    return false;
                }

            }

            return true;
        }
        #endregion

        public void RegrasDataFinalPlano(PlanoAdquirido planoAdquirido, SqlTransaction trans, PlanoParcela primeiraParcela)
        {

            var data = primeiraParcela.PlanoParcelaSituacao.IdPlanoParcelaSituacao ==
            (int)BLL.Enumeradores.PlanoParcelaSituacao.EmAberto ? DateTime.Today.AddMonths(1) : planoAdquirido.DataFimPlano.AddMonths(1);

            DateTime dataValida = data.AddDays(Feriado.RetornarDiaUtilVencimento(data));

            PlanoAdquirido.SalvarDataFimPlanoBoletoRecorrente(planoAdquirido.IdPlanoAdquirido, dataValida, trans);

        }

        #region InformaValorDiferentePagoEGerado
        private void InformaValorDiferentePagoEGerado(BLL.Pagamento objPagamento, decimal valorPagamento)
        {
            var carta = BLL.CartaEmail.RecuperarCarta(Enumeradores.CartaEmail.BoletoValorDiferente);
            objPagamento.Filial.CompleteObject();
            carta.Conteudo = carta.Conteudo.Replace("{CNPJ_EMPRESA}", objPagamento.Filial.CNPJ)
                    .Replace("{EMPRESA}", objPagamento.Filial.NomeFantasia)
                    .Replace("{Valor_Boleto}", objPagamento.ValorPagamento.ToString())
                    .Replace("{Valor_Pago}", valorPagamento.ToString());

            UsuarioFilialPerfil objUsuarioFilialPerfil = UsuarioFilialPerfil.LoadObject(Convert.ToInt32(Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.IdUsuarioFilialNotaAntencipada)));
            MensagemCS.SalvarEmail(null, objUsuarioFilialPerfil, objUsuarioFilialPerfil, null,
                carta.Assunto, carta.Conteudo, Enumeradores.CartaEmail.BoletoValorDiferente,
                "rodrigobandini@bne.com.br",
                Parametro.RecuperaValorParametro(Enumeradores.Parametro.EmailDestinoInformarValorDiferentePagoGerado), string.Empty, null, null);
        }
        #endregion

        #endregion


        #region RecuperarUltimioBoletoNaoPago
        /// <summary>
        /// Recupera ultimo boleto do plano que não foi pago.
        /// </summary>
        /// <param name="idf_Filial"></param>
        /// <returns></returns>
        public static string RecuperarUltimioBoletoNaoPago(int idf_Filial)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Filial", SqlDbType.Int));
            parms[0].Value = idf_Filial;
            List<Pagamento> objPagamento = new List<Pagamento>();
            byte[] boleto = null;
            //using (var conn = new SqlConnection(BLL.DataAccessLayer.CONN_STRING))
            //{
            //    conn.Open();
            //  using (SqlTransaction trans = conn.BeginTransaction())
            //  {
            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, spRecuperarUltimioBoletoNaoPago, parms))
            {
                if (dr.Read())
                {
                    objPagamento.Add(Pagamento.LoadObject(Convert.ToInt32(dr["Idf_Pagamento"])));
                }

            }


            string html = GerarLayoutBoletoHTMLPagarMe(CriarBoletosPagarMe(objPagamento, false));

            // }


            return html;
        }
        #endregion
        #region ExisteBoletoParaParcela
        /// <summary>
        /// Recupera o registro do boleto bancario atraves do Id Do Pagamengo
        /// </summary>
        /// <param name="idPagamento"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        public static BoletoBancario CarregarBoletoDeParcela(int idPagamento, SqlTransaction trans = null)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Pagamento", SqlDbType.Int));
            parms[0].Value = idPagamento;


            using (IDataReader dr = DataAccessLayer.ExecuteReader(trans, CommandType.Text, spCarregarBoletoDeParcela, parms))
            {
                while (dr.Read())
                {
                    return BoletoBancario.LoadObject(Convert.ToInt32(dr["Idf_Boleto_Bancario"]));
                }

            }
            return null;
        }
        #endregion

        #region GerarBoletoNovo
        public static List<DTO.DTOBoletoPagarMe> GerarBoletosNovoPagarme(PlanoAdquirido objPlanoAdquirido, SqlTransaction trans = null)
        {
            if (objPlanoAdquirido != null)
            {
                var parcelas = PlanoParcela.RecuperarParcelasPlanoAdquirido(objPlanoAdquirido);
                var listaPagamento = new List<Pagamento>();

                foreach (var objPlanoParcela in parcelas)
                {
                    var listaPagamentoPorParcela = Pagamento.CarregaPagamentosPorPlanoParcela(objPlanoParcela.IdPlanoParcela).Where(p =>
                        p.TipoPagamento.IdTipoPagamento == (int)Enumeradores.TipoPagamento.BoletoBancario
                        && p.PagamentoSituacao.IdPagamentoSituacao == (int)Enumeradores.PagamentoSituacao.EmAberto
                        && p.FlagInativo == false)
                        .OrderByDescending(p => p.IdPagamento)
                        .ToList();
                    if (listaPagamentoPorParcela.Count > 0)
                        listaPagamento.Add(listaPagamentoPorParcela.First()); // adiciona o último pagamento

                    if (listaPagamentoPorParcela.Count > 1)//Apenas para parcela com mais de um pagamento aberto
                    {
                        listaPagamentoPorParcela.Remove(listaPagamentoPorParcela.First()); //Remove da lista de pagamentos em aberto
                        Pagamento.CancelarOutrosPagamentosEmAbertoDePlanoParcela(listaPagamentoPorParcela, trans); // Cancela outros pagamentos
                    }

                }
                return CriarBoletosPagarMe(listaPagamento);
            }
            return null;
        }
        #endregion

        #region GerarLayoutBoletoHTML
        public static string GerarLayoutBoletoHTMLPagarMe(List<DTO.DTOBoletoPagarMe> boleto)
        {
            try
            {
                var html = string.Empty;
                WebResponse response = null;
                StreamReader reader = null;

                foreach (var item in boleto)
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(item.UrlBoleto);
                    request.Method = "GET";
                    response = request.GetResponse();
                    reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.UTF8);
                    html = html + reader.ReadToEnd() + "<div style=\"page-break-before: always;\" ></div>";
                }
                reader.Close();
                response.Close();

                html = html.Replace("18727053000174", "82344425000182");//colocar cnpj do bne.
                html = html.Replace("<input class=\"no-print\" type=\"button\" value=\"Clique aqui para imprimir\" onclick=\"window.print();return false;\" />", "");
                html = html.Replace("BNE | Pagar.me Pagamentos S/A", "BNE | Banco Nacional de Empregos");
                html = html.Replace("<meta name=\"format-detection\" content=\"telephone=no\"/>", "<meta name=\"format-detection\" content=\"telephone=no\"/><meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\"/>");


                return html;

            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
                throw ex;
            }

        }
        #endregion


        #region [ConsultarPagamentoBoletosPendente]

        public static DataTable ConsultarPagamentoBoletosPendente()
        {
            DataTable dt = new DataTable();

            using (var dr = DataAccessLayer.ExecuteReader(CommandType.Text, spConsultarPagamentoBoletosPendente, null))
            {

                dt.Load(dr);
                if (dt != null)
                    dt.Dispose();
            }

            return dt;
        }
        #endregion

        #region [GerarNovoPagamento]
        /// <summary>
        /// Gerar novo pagamento quando acaba o plano
        /// </summary>
        /// <param name="boleto"></param>
        /// <param name="trans"></param>
        public void GerarNovoPagamento(BoletoBancario boleto, SqlTransaction trans)
        {
            try
            {
                //criar uma nova parcela
                var objPlanoParcela = PlanoParcela.CriarParcelaRecorrenciaPeloPlanoAdquirido(boleto.Pagamento.PlanoParcela.PlanoAdquirido, null, trans);
                //criar um novo pagamento
                DateTime dataVencimento = boleto.Pagamento.DataVencimento.HasValue ? boleto.Pagamento.DataVencimento.Value.AddMonths(1) : DateTime.Today.AddDays(5);

                DateTime dataVenciamentoValida = dataVencimento.AddDays(Feriado.RetornarDiaUtilVencimento(dataVencimento));
                var pagamento = Pagamento.CriarPagamentoBoletoRecorrencia(objPlanoParcela,
                   boleto.Pagamento.PlanoParcela.PlanoAdquirido,
                   dataVenciamentoValida, boleto.Pagamento.PlanoParcela.ValorParcela,
                   trans);
                //criar Boleto para atualizar a tabela de pagamento
                PagarMeOperacoes.GerarBoleto(pagamento, trans);
            }
            catch (Exception ex)
            {
                String guid = EL.GerenciadorException.GravarExcecao(ex);

            }

        }
        #endregion


        #endregion
    }
}
