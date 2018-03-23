using BNE.BLL.Custom;
using BNE.BLL.Custom.Email;
using BNE.BLL.Enumeradores;
using BNE.EL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;

namespace BNE.BLL
{
    public partial class BoletoBancario
    {

        #region Consultas
        private const string SP_NOSSO_NUMERO = @"SELECT * FROM BNE.BNE_Boleto_Bancario WHERE Num_Nosso_Numero = @NossoNumero";

        #endregion

        #region Metodos

        #region GerarBoletoNovo
        public static List<BoletoNet.Boleto> GerarBoletosNovo(PlanoAdquirido objPlanoAdquirido, SqlTransaction trans = null)
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
                    listaPagamento.Add(listaPagamentoPorParcela.First()); // adiciona o último pagamento

                    if (listaPagamentoPorParcela.Count > 1)//Apenas para parcela com mais de um pagamento aberto
                    {
                        listaPagamentoPorParcela.Remove(listaPagamentoPorParcela.First()); //Remove da lista de pagamentos em aberto
                        Pagamento.CancelarOutrosPagamentosEmAbertoDePlanoParcela(listaPagamentoPorParcela, trans); // Cancela outros pagamentos
                    }
                }
                return CriarBoletos(listaPagamento);
            }
            return null;
        }
        #endregion

        #region GerarBoletoPagamentoAdicionalNovo
        public static List<BoletoNet.Boleto> GerarBoletoPagamentoAdicionalNovo(decimal pagamentoAdicionalValorTotal, int pagamentoAdicionalQuantidade, int idFilial, int idUsuarioFilialPerfilLogadoEmpresa, ref int pagamentoIdentificadorPagamento)
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

                return CriarBoletos(listaPagamento);
            }
            return null;
        }
        #endregion

        #region CriarBoletos
        public static List<BoletoNet.Boleto> CriarBoletos(List<Pagamento> pagamentos, bool isCobranca = false)
        {
            using (var conn = new SqlConnection(BLL.DataAccessLayer.CONN_STRING))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        List<BoletoNet.Boleto> boletos = new List<BoletoNet.Boleto>();

                        pagamentos.RemoveAll(p => p == null);

                        foreach (var objPagamento in pagamentos)
                            boletos.Add(ProcessarBoletoNovo(objPagamento, trans, isCobranca));
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

        #region ProcessarNovoBoleto
        public static BoletoNet.Boleto ProcessarBoletoNovo(BLL.Pagamento objPagamento, SqlTransaction trans, bool isCobranca = false)
        {
            List<Enumeradores.Parametro> parametros = new List<Enumeradores.Parametro>();
            parametros.Add(Enumeradores.Parametro.CobreBemXCodigoAgencia);
            parametros.Add(Enumeradores.Parametro.CobreBemXNumeroContaCorrente);
            parametros.Add(Enumeradores.Parametro.CobreBemXCodigoCedente);
            parametros.Add(Enumeradores.Parametro.CobreBemXCodBanco);

            //DADOS DO CEDENTE
            #region Dados do Cedente
            Dictionary<Enumeradores.Parametro, string> valoresParametros = Parametro.ListarParametros(parametros);

            String cedenteAgencia = valoresParametros[Enumeradores.Parametro.CobreBemXCodigoAgencia];
            String[] cedenteNumeroConta = valoresParametros[Enumeradores.Parametro.CobreBemXNumeroContaCorrente].Split('-');
            String cedenteCodigo = valoresParametros[Enumeradores.Parametro.CobreBemXCodigoCedente];
            String codigoBanco = valoresParametros[Enumeradores.Parametro.CobreBemXCodBanco];

            objPagamento.CompleteObject();

            String nossoNumero = string.Empty;

            if (string.IsNullOrEmpty(objPagamento.DescricaoIdentificador))
            {
                if (objPagamento.AdicionalPlano == null)
                    nossoNumero = new StringBuilder(Helper.NumericoParaTamanhoExato(objPagamento.PlanoParcela.IdPlanoParcela.ToString(), 12)).Insert(0, (int)BoletoAplicacao.BNE).ToString();
                else
                    nossoNumero = new StringBuilder(Helper.NumericoParaTamanhoExato(objPagamento.AdicionalPlano.IdAdicionalPlano.ToString(), 12)).Insert(0, (int)BoletoAplicacao.PLANOADICIONAL).ToString();
            }
            else
                nossoNumero = objPagamento.DescricaoIdentificador;


            String codigoBarras = String.Empty;
            String email = String.Empty;
            Cidade objCidade = null;
            var cedente = new BoletoNet.Cedente("82.344.425/0001-82", "Bne - Banco Nacional de Empregos Ltda", cedenteAgencia, "", cedenteNumeroConta[0], cedenteNumeroConta[1]);
            cedente.Codigo = cedenteCodigo;

            #endregion Dados do Cedente

            //CRIAÇÃO DO BOLETONET
            #region Criacao do BoletoNET
            var boleto = new BoletoNet.Boleto(objPagamento.DataVencimento.Value, objPagamento.ValorPagamento, "CNR", nossoNumero, cedente);
            boleto.Banco = new BoletoNet.Banco(Convert.ToInt32(codigoBanco));
            #endregion

            //DADOS DO SACADO
            #region Dados do Endereço e Sacado

            bool flagEmpresa = true;
            if (objPagamento.Filial != null)
            {
                flagEmpresa = true;
                objPagamento.Filial.CompleteObject(trans);
                boleto.Sacado = new BoletoNet.Sacado(objPagamento.Filial.NumeroCNPJ.Value.ToString().PadLeft(14, '0'), objPagamento.Filial.RazaoSocial);
                if (objPagamento.Filial.Endereco != null)
                {
                    objPagamento.Filial.Endereco.CompleteObject(trans);

                    boleto.Sacado.Endereco = new BoletoNet.Endereco();

                    if (objPagamento.Filial.Endereco.Cidade.CompleteObject())
                    {
                        boleto.Sacado.Endereco.Cidade = objPagamento.Filial.Endereco.Cidade.NomeCidade;
                        objCidade = objPagamento.Filial.Endereco.Cidade;
                    }
                    if (objPagamento.Filial.Endereco.Cidade.Estado.CompleteObject())
                        boleto.Sacado.Endereco.UF = objPagamento.Filial.Endereco.Cidade.Estado.SiglaEstado;
                    if (!String.IsNullOrEmpty(objPagamento.Filial.Endereco.DescricaoLogradouro))
                        boleto.Sacado.Endereco.Logradouro = objPagamento.Filial.Endereco.DescricaoLogradouro;
                    if (!String.IsNullOrEmpty(objPagamento.Filial.Endereco.NumeroEndereco))
                        boleto.Sacado.Endereco.Numero = objPagamento.Filial.Endereco.NumeroEndereco;
                    if (!String.IsNullOrEmpty(objPagamento.Filial.Endereco.DescricaoComplemento))
                        boleto.Sacado.Endereco.Complemento = objPagamento.Filial.Endereco.DescricaoComplemento;
                    if (!String.IsNullOrEmpty(objPagamento.Filial.Endereco.NumeroCEP))
                        boleto.Sacado.Endereco.CEP = objPagamento.Filial.Endereco.NumeroCEP;
                    if (!String.IsNullOrEmpty(objPagamento.Filial.Endereco.DescricaoBairro))
                        boleto.Sacado.Endereco.Bairro = objPagamento.Filial.Endereco.DescricaoBairro;
                }
            }
            else
            {
                flagEmpresa = false;
                objPagamento.UsuarioFilialPerfil.CompleteObject(trans);
                objPagamento.UsuarioFilialPerfil.PessoaFisica.CompleteObject(trans);

                boleto.Sacado = new BoletoNet.Sacado(objPagamento.UsuarioFilialPerfil.PessoaFisica.CPF.ToString().PadLeft(11, '0'), objPagamento.UsuarioFilialPerfil.PessoaFisica.NomePessoa);

                if (objPagamento.UsuarioFilialPerfil.PessoaFisica.Endereco != null)
                {
                    objPagamento.UsuarioFilialPerfil.PessoaFisica.Endereco.CompleteObject(trans);

                    boleto.Sacado.Endereco = new BoletoNet.Endereco();

                    if (objPagamento.UsuarioFilialPerfil.PessoaFisica.Endereco.Cidade.CompleteObject())
                    {
                        boleto.Sacado.Endereco.Cidade = objPagamento.UsuarioFilialPerfil.PessoaFisica.Endereco.Cidade.NomeCidade;
                        objCidade = objPagamento.UsuarioFilialPerfil.PessoaFisica.Endereco.Cidade;
                    }

                    if (objPagamento.UsuarioFilialPerfil.PessoaFisica.Endereco.Cidade.Estado.CompleteObject())
                        boleto.Sacado.Endereco.UF = objPagamento.UsuarioFilialPerfil.PessoaFisica.Endereco.Cidade.Estado.SiglaEstado;
                    if (!String.IsNullOrEmpty(objPagamento.UsuarioFilialPerfil.PessoaFisica.Endereco.DescricaoLogradouro))
                        boleto.Sacado.Endereco.Logradouro = objPagamento.UsuarioFilialPerfil.PessoaFisica.Endereco.DescricaoLogradouro;
                    if (!String.IsNullOrEmpty(objPagamento.UsuarioFilialPerfil.PessoaFisica.Endereco.NumeroEndereco))
                        boleto.Sacado.Endereco.Numero = objPagamento.UsuarioFilialPerfil.PessoaFisica.Endereco.NumeroEndereco;
                    if (!String.IsNullOrEmpty(objPagamento.UsuarioFilialPerfil.PessoaFisica.Endereco.DescricaoComplemento))
                        boleto.Sacado.Endereco.Complemento = objPagamento.UsuarioFilialPerfil.PessoaFisica.Endereco.DescricaoComplemento;
                    if (!String.IsNullOrEmpty(objPagamento.UsuarioFilialPerfil.PessoaFisica.Endereco.NumeroCEP))
                        boleto.Sacado.Endereco.CEP = objPagamento.UsuarioFilialPerfil.PessoaFisica.Endereco.NumeroCEP;
                    if (!String.IsNullOrEmpty(objPagamento.UsuarioFilialPerfil.PessoaFisica.Endereco.DescricaoBairro))
                        boleto.Sacado.Endereco.Bairro = objPagamento.UsuarioFilialPerfil.PessoaFisica.Endereco.DescricaoBairro;
                }
            }
            #endregion Dados do Endereço e Sacado

            #region Instruções e validação do Boleto
            BoletoNet.Instrucao_HSBC instrucao = new BoletoNet.Instrucao_HSBC();
            if (!isCobranca)
                instrucao.Descricao = "Todas as informações deste boleto são de exclusiva responsabilidade do cedente. Um dia útil após a confirmação do pagamento, você terá acesso ao serviço do BNE.";
            else
                instrucao.Descricao = "Protestar em 10 dias após o vencimento se não pago.";
            boleto.Instrucoes.Add(instrucao);

            boleto.Valida();
            #endregion Instruções e validação do Boleto

            objPagamento.DescricaoIdentificador = nossoNumero;
            objPagamento.DescricaoDescricao = String.Join("", System.Text.RegularExpressions.Regex.Split(boleto.CodigoBarra.LinhaDigitavel, @"[^\d]"));//Linha Digitavel

            if (trans != null)
                objPagamento.Save(trans);
            else
                objPagamento.Save();

            BoletoBancario objBoletoBancario = BoletoBancario.CarregarBoletoDeParcela(nossoNumero);

            if (objBoletoBancario == null)
            {
                objBoletoBancario = new BoletoBancario()
                {
                    Pagamento = new Pagamento(objPagamento.IdPagamento),
                    Banco = new Banco(Convert.ToInt32(codigoBanco)),
                    DataEmissao = boleto.DataDocumento,
                    DataVencimento = boleto.DataVencimento,

                    CedenteNumCNPJCPF = boleto.Cedente.CPFCNPJ,
                    CedenteNome = boleto.Cedente.Nome,
                    CedenteAgencia = cedenteAgencia,
                    CedenteDVAgencia = string.Empty, // hoje não tem dv da agencia 
                    CedenteNumeroConta = cedenteNumeroConta[0],
                    CedenteDVConta = cedenteNumeroConta[1].ToString(),
                    CedenteCodigo = cedenteCodigo,

                    SacadoNumCNPJCPF = boleto.Sacado.CPFCNPJ,
                    SacadoNome = boleto.Sacado.Nome,
                    SacadoEnderecoLogradouro = boleto.Sacado.Endereco.Logradouro,
                    SacadoEnderecoNumero = boleto.Sacado.Endereco.Numero,
                    SacadoEnderecoComplemento = boleto.Sacado.Endereco.Complemento,
                    SacadoEnderecoBairro = boleto.Sacado.Endereco.Bairro,
                    SacadoEnderecoCidade = objCidade,
                    SacadoEnderecoCEP = boleto.Sacado.Endereco.CEP,

                    InstrucaoDescricao = boleto.Instrucoes.First().Descricao,
                    NumeroNossoNumero = nossoNumero,

                    ValorBoleto = boleto.ValorBoleto,
                    FlagEmpresa = flagEmpresa



                };
                if (trans == null)
                    objBoletoBancario.Save();
                else
                    objBoletoBancario.Save(trans);
            }
            else
            {
                if (boleto.DataVencimento.Date != objBoletoBancario.DataVencimento.Value.Date || boleto.ValorBoleto != objBoletoBancario.ValorBoleto)
                {
                    objBoletoBancario.DataVencimento = boleto.DataVencimento.Date;
                    objBoletoBancario.ValorBoleto = boleto.ValorBoleto;

                    if (trans == null)
                        objBoletoBancario.Save();
                    else
                        objBoletoBancario.Save(trans);
                }
            }
            return boleto;
        }
        #endregion

        #region GerarLayoutBoletoHTML
        public static string GerarLayoutBoletoHTML(List<BoletoNet.Boleto> boletos)
        {
            try
            {
                BoletoNet.BoletoBancario boletoBancario;
                var html = new StringBuilder();

                if (boletos == null || boletos.Count == 0)
                    html.Append("Não Existem boletos!");
                foreach (var boleto in boletos)
                {
                    boletoBancario = new BoletoNet.BoletoBancario();
                    boletoBancario.CodigoBanco = (short)boleto.Banco.Codigo;
                    boletoBancario.Boleto = boleto;
                    html.Append("<div style=\"page-break-before: always;\"/>");
                    html.Append(boletoBancario.MontaHtmlEmbedded());
                }
                return html.ToString();

            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
                throw ex;
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
                    htmlToPdf.TempFilesPath = ConfigurationManager.AppSettings["ArquivosTemporarios"].ToString();
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
        public static string RetornarBoleto(BLL.Pagamento objPagamento, Enumeradores.FormatoBoleto formatoBoleto, bool isCobranca = false)
        {
            List<Pagamento> pagamentos = new List<Pagamento>();
            pagamentos.Add(objPagamento);

            string strNomeArquivo = GerarFormatoArquivoDeBoleto(formatoBoleto);
            byte[] byteArray = MontarBoletoBytes(pagamentos, formatoBoleto, isCobranca);

            return GerarLinkDownload(byteArray, strNomeArquivo);
        }
        #endregion

        #region RetornarBoleto - List<BoletoNet.Boleto> boletos
        public static string RetornarBoleto(List<BoletoNet.Boleto> boletos, Enumeradores.FormatoBoleto formatoBoleto)
        {
            string strNomeArquivo = GerarFormatoArquivoDeBoleto(formatoBoleto);
            byte[] byteArray = MontarBoletoBytes(GerarLayoutBoletoHTML(boletos), formatoBoleto);

            return GerarLinkDownload(byteArray, strNomeArquivo);
        }
        #endregion

        #region RetornarBoleto - string html, out byte[] pdfArray, out byte[] imgArray, out string urlPDF, out string urlIMG
        public static void RetornarBoleto(string html, out byte[] pdfArray, out byte[] imgArray, out string urlPDF, out string urlIMG)
        {
            pdfArray = MontarBoletoBytes(html, Enumeradores.FormatoBoleto.PDF);
            urlPDF = GerarLinkDownload(pdfArray, GerarFormatoArquivoDeBoleto(Enumeradores.FormatoBoleto.PDF));

            imgArray = MontarBoletoBytes(html, Enumeradores.FormatoBoleto.IMAGEM);
            urlIMG = GerarLinkDownload(imgArray, GerarFormatoArquivoDeBoleto(Enumeradores.FormatoBoleto.IMAGEM));
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
            string html = GerarLayoutBoletoHTML(CriarBoletos(pagamentos, isCobranca));
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
                        objPagamentoCancelado.Save(trans);

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

                    var objPlanoAdquirido = PlanoAdquirido.LoadObject(this.Pagamento.PlanoParcela.PlanoAdquirido.IdPlanoAdquirido);

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




        #endregion
    }
}
