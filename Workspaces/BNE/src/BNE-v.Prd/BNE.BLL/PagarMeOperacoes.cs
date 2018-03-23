using System;
using System.Configuration;
using PagarMe;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace BNE.BLL
{
    public class PagarMeOperacoes
    {
        private static readonly string DefaultApiKeyPagarMe = ConfigurationManager.AppSettings["DefaultApiKeyPagarMe"];
        private static readonly string DefaultEncryptionKeyPagarMe = ConfigurationManager.AppSettings["DefaultEncryptionKeyPagarMe"];

        public PagarMeOperacoes()
        {
            PagarMeService.DefaultApiKey = DefaultApiKeyPagarMe;
            PagarMeService.DefaultEncryptionKey = DefaultEncryptionKeyPagarMe;
        }

        public static Transaction Charge(Transacao transacao, string urlRetorno)
        {
            PagarMeService.DefaultApiKey = DefaultApiKeyPagarMe;
            PagarMeService.DefaultEncryptionKey = DefaultEncryptionKeyPagarMe;

            Transaction transaction = new Transaction();
            transaction = CriarTransacao(transacao, urlRetorno);
            return transaction;
        }

        public static Transaction CriarTransacao(Transacao transacao, string url)
        {
            Transaction transaction = new Transaction();

            transaction.Amount = Convert.ToInt32(transacao.ValorDocumento * 100);
            transaction.PaymentMethod = PaymentMethod.CreditCard;
            transaction.CardHash = GerarCardHash(transacao);
            //Recipient recipient = PagarMeService.GetDefaultService().Recipients.Find("re_cj16rfz7b083w8o5xpwsh34rq");
            //transaction.SplitRules = CreateSplitRule(recipient);

            return transaction;
        }


        private static string GerarCardHash(Transacao transacao)
        {

            var creditcard = new CardHash();

            //creditcard.CardHolderName = "Jose da Silva";
            //creditcard.CardNumber = "5433229077370451";
            //creditcard.CardExpirationDate = "1038";
            //creditcard.CardCvv = "018";

            creditcard.CardHolderName = transacao.NomeCartaoCredito;
            creditcard.CardNumber = transacao.NumeroCartaoCredito;
            creditcard.CardExpirationDate = transacao.NumeroMesValidadeCartaoCredito.PadLeft(2, '0') + transacao.NumeroAnoValidadeCartaoCredito;
            creditcard.CardCvv = transacao.NumeroCodigoVerificadorCartaoCredito;

            return creditcard.Generate();
        }


        public static Boolean CancelarTransacaoPagarMe(Pagamento pagamento)
        {
            PagarMeService.DefaultApiKey = DefaultApiKeyPagarMe;
            PagarMeService.DefaultEncryptionKey = DefaultEncryptionKeyPagarMe;

            var transaction = PagarMeService.GetDefaultService().Transactions.Find(pagamento.DescricaoIdentificador);

            transaction.Refund();

            return transaction.Status == TransactionStatus.Refunded;
        }

        public static Card RetornarCartao(string idCartao)
        {
            return PagarMeService.GetDefaultService().Cards.Find(idCartao);
        }

        public static DTO.DTOBoletoPagarMe GerarBoleto(Pagamento objPagamento, SqlTransaction trans = null, bool BoletoNovo = false, DateTime? DataVencimento = null)
        {
            PagarMeService.DefaultApiKey = DefaultApiKeyPagarMe;
            PagarMeService.DefaultEncryptionKey = DefaultEncryptionKeyPagarMe;
            DTO.DTOBoletoPagarMe objBoleto = new DTO.DTOBoletoPagarMe();

            Pagamento objPagamentoNovo = new Pagamento();

            try
            {
                Transaction transaction = new Transaction();

                BoletoBancario objBoletoBancario = BoletoBancario.CarregarBoletoDeParcela(objPagamento.IdPagamento);

                if (objBoletoBancario != null && !BoletoNovo)
                {//ja tem boleto
                    try
                    {
                        //transaction = PagarMeService.GetDefaultService().Transactions.Find(29542492); 
                        transaction = PagarMeService.GetDefaultService().Transactions.Find(objBoletoBancario.NumeroNossoNumero);
                        objBoleto.CodigoDeBarra = transaction.BoletoBarcode.Replace(" ", "").Replace(".", "");
                        objBoleto.UrlBoleto = transaction.BoletoUrl;
                        objBoleto.NossoNumero = transaction.Id;
                        return objBoleto;
                    }
                    catch (Exception)
                    {
                        // não tem o boleto no pagar me
                    }
               
                }
                #region [Novo pagamento]

                objPagamentoNovo.PagamentoSituacao = objPagamento.PagamentoSituacao;
                objPagamentoNovo.AdicionalPlano = objPagamento.AdicionalPlano;
                objPagamentoNovo.CodigoDesconto = objPagamento.CodigoDesconto;
                objPagamentoNovo.CodigoGuid = objPagamento.CodigoGuid;
                objPagamentoNovo.DataEmissao = DateTime.Now;
                objPagamentoNovo.DataVencimento = objPagamento.DataVencimento;
                objPagamentoNovo.DescricaoDescricao = objPagamento.DescricaoDescricao;
                objPagamentoNovo.DesOrdemDeCompra = objPagamento.DesOrdemDeCompra;
                objPagamentoNovo.Filial = objPagamento.Filial;
                objPagamentoNovo.FlagAvulso = objPagamento.FlagAvulso;
                objPagamentoNovo.FlagBaixado = objPagamento.FlagBaixado;
                objPagamentoNovo.FlagInativo = objPagamento.FlagInativo;
                objPagamentoNovo.FlagNotaEnviada = objPagamento.FlagNotaEnviada;
                objPagamentoNovo.NumeroNotaFiscal = objPagamento.NumeroNotaFiscal;
                objPagamentoNovo.Operadora = objPagamento.Operadora;
                objPagamentoNovo.PagamentoSituacao = new PagamentoSituacao((int)Enumeradores.PagamentoSituacao.EmAberto);
                objPagamentoNovo.PlanoParcela = objPagamento.PlanoParcela;
                objPagamentoNovo.TipoPagamento = objPagamento.TipoPagamento;
                objPagamentoNovo.UrlNotaFiscal = objPagamento.UrlNotaFiscal;
                objPagamentoNovo.UsuarioFilialPerfil = objPagamento.UsuarioFilialPerfil;
                objPagamentoNovo.UsuarioGerador = objPagamento.UsuarioGerador;
                objPagamentoNovo.ValorJuros = objPagamento.CalcularJuros();
                objPagamentoNovo.FlagJuros = objPagamento.FlagJuros;
                objPagamentoNovo.ValorPagamento = objPagamento.ValorPagamento;


                //inativar o antigo do boleto velho
                if (objPagamento.PagamentoSituacao.IdPagamentoSituacao.Equals((int)Enumeradores.PagamentoSituacao.Pago))
                    return objBoleto;//boleto antigo ja pago- aborta
                else
                    objPagamento.PagamentoSituacao = new PagamentoSituacao((int)Enumeradores.PagamentoSituacao.Cancelado);

                if (trans == null)
                    objPagamento.Save();
                else
                    objPagamento.Save(trans);

                #endregion

                if (DataVencimento.HasValue)
                    objPagamentoNovo.DataVencimento = DataVencimento;

                transaction.Amount = Convert.ToInt32((objPagamentoNovo.ValorPagamento+objPagamentoNovo.ValorJuros) * 100);
                transaction.PaymentMethod = PaymentMethod.Boleto;
                var dataVencimento = new DateTime(objPagamentoNovo.DataVencimento.Value.Year, objPagamentoNovo.DataVencimento.Value.Month, objPagamentoNovo.DataVencimento.Value.Day, 23, 59, 59);
                transaction.BoletoExpirationDate = dataVencimento;

                if (objPagamento.PlanoSine())//Planao do sine vai outras instruções no boleto
                    transaction.BoletoInstructions = Parametro.RecuperaValorParametro(Enumeradores.Parametro.InformacoesBoletoPlanoSine);
                else
                    transaction.BoletoInstructions = InformacoesBoleto(objPagamentoNovo.ValorPagamento);
                transaction.AcquirerName = "BNE - Banco Nacional de Empregos";

                //Recipient recipient = PagarMeService.GetDefaultService().Recipients.Find("re_cjckrpa6l07smhb5xpr60s5kd");

                //transaction.SplitRules = CreateSplitRule(recipient);

                if (objPagamentoNovo.Filial != null)
                {
                    objPagamentoNovo.Filial.CompleteObject();
                    objPagamentoNovo.Filial.Endereco.CompleteObject();
                    objPagamentoNovo.Filial.Endereco.Cidade.CompleteObject();
                    objPagamentoNovo.Filial.Endereco.Cidade.Estado.CompleteObject();
                }
                else
                {
                    objPagamentoNovo.UsuarioFilialPerfil.CompleteObject();
                    objPagamentoNovo.UsuarioFilialPerfil.PessoaFisica.CompleteObject();
                    objPagamentoNovo.UsuarioFilialPerfil.PessoaFisica.Endereco.CompleteObject();
                }


                transaction.Customer = new Customer()
                {

                    Name = objPagamentoNovo.Filial != null ? objPagamentoNovo.Filial.RazaoSocial : objPagamentoNovo.UsuarioFilialPerfil.PessoaFisica.NomeCompleto,
             
                    DocumentNumber = objPagamentoNovo.Filial != null ? objPagamentoNovo.Filial.NumeroCNPJ.ToString().PadLeft(14, '0') : objPagamentoNovo.UsuarioFilialPerfil.PessoaFisica.NumeroCPF.ToString().PadLeft(11, '0'),
                    //Phone = new Phone()
                    //{
                    //    Ddd = objPagamento.Filial.NumeroDDDComercial,
                    //    Number = objPagamento.Filial.NumeroComercial
                    //},
                    //Removi porque se instanciar o endereço e faltar algum dado vai dar erro no pagarme e como no testes deu erro porque uma empresa faltava o numero
                    // do endereço vou remover
                    //Address = new Address()
                    //{
                    //    Street = objPagamentoNovo.Filial.Endereco.DescricaoLogradouro,
                    //    StreetNumber = objPagamentoNovo.Filial.Endereco.NumeroEndereco,
                    //    Neighborhood = objPagamentoNovo.Filial.Endereco.DescricaoBairro,
                    //    City = BLL.Custom.Helper.FormatarCidade(objPagamentoNovo.Filial.Endereco.Cidade.NomeCidade, objPagamentoNovo.Filial.Endereco.Cidade.Estado.SiglaEstado),
                    //    Zipcode = objPagamentoNovo.Filial.Endereco.NumeroCEP
                    //},

                };
                if (objPagamentoNovo.Filial != null)
                    transaction.Customer.Email = objPagamentoNovo.UsuarioFilialPerfil.Email();
                else if (!string.IsNullOrEmpty(objPagamentoNovo.UsuarioFilialPerfil.PessoaFisica.EmailPessoa))
                    transaction.Customer.Email = objPagamentoNovo.UsuarioFilialPerfil.PessoaFisica.EmailPessoa;


                transaction.Save();


                objPagamentoNovo.DescricaoIdentificador = objBoleto.NossoNumero = transaction.Nsu.ToString();
                objBoleto.UrlBoleto = transaction.BoletoUrl;
                objPagamentoNovo.DescricaoDescricao = objBoleto.CodigoDeBarra = transaction.BoletoBarcode.Replace(" ", "").Replace(".", ""); ;

                if (trans == null)
                    objPagamentoNovo.Save();
                else
                    objPagamentoNovo.Save(trans);


                //Salvar boleto do pagar me
                objBoletoBancario = new BoletoBancario()
                {
                    Pagamento = new Pagamento(objPagamentoNovo.IdPagamento),
                    Banco = new Banco(Convert.ToInt32(Enumeradores.Banco.BRADESCO)),
                    DataEmissao = transaction.DateCreated,
                    DataVencimento = transaction.BoletoExpirationDate,

                    CedenteNumCNPJCPF = transaction.AcquirerName,
                    CedenteNome = transaction.Customer.Name,
                    //CedenteAgencia = transaction.,
                    //CedenteDVAgencia = string.Empty, // hoje não tem dv da agencia 
                    // CedenteNumeroConta = cedenteNumeroConta[0],
                    // CedenteDVConta = cedenteNumeroConta[1].ToString(),
                    // CedenteCodigo = cedenteCodigo,

                    SacadoNumCNPJCPF = transaction.Customer.DocumentNumber,
                    SacadoNome = objPagamentoNovo.Filial != null ? objPagamentoNovo.Filial.RazaoSocial : objPagamentoNovo.UsuarioFilialPerfil.PessoaFisica.NomeCompleto,
                    SacadoEnderecoLogradouro = objPagamentoNovo.Filial != null ?  objPagamentoNovo.Filial.Endereco.DescricaoLogradouro : objPagamentoNovo.UsuarioFilialPerfil.PessoaFisica.Endereco.DescricaoLogradouro,
                    SacadoEnderecoNumero = objPagamentoNovo.Filial != null ?  objPagamentoNovo.Filial.Endereco.NumeroEndereco : objPagamentoNovo.UsuarioFilialPerfil.PessoaFisica.Endereco.NumeroEndereco,
                    SacadoEnderecoComplemento = objPagamentoNovo.Filial != null ?  objPagamentoNovo.Filial.Endereco.DescricaoComplemento : objPagamentoNovo.UsuarioFilialPerfil.PessoaFisica.Endereco.DescricaoComplemento,
                    SacadoEnderecoBairro = objPagamentoNovo.Filial != null ?  objPagamentoNovo.Filial.Endereco.DescricaoBairro : objPagamentoNovo.UsuarioFilialPerfil.PessoaFisica.Endereco.DescricaoBairro,
                    SacadoEnderecoCidade = objPagamentoNovo.Filial != null ?  objPagamentoNovo.Filial.Endereco.Cidade : objPagamentoNovo.UsuarioFilialPerfil.PessoaFisica.Endereco.Cidade,
                    SacadoEnderecoCEP = objPagamentoNovo.Filial != null ? objPagamentoNovo.Filial.Endereco.NumeroCEP : objPagamentoNovo.UsuarioFilialPerfil.PessoaFisica.Endereco.NumeroCEP,

                   // InstrucaoDescricao = transaction.BoletoInstructions,
                    NumeroNossoNumero = transaction.Id,

                    ValorBoleto = objPagamentoNovo.ValorPagamento,
                    FlagEmpresa = true,
                    UrlBoleto = transaction.BoletoUrl

                };
                if (trans == null)
                    objBoletoBancario.Save();
                else
                    objBoletoBancario.Save(trans);

            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex, $"Erro ao gerar boleto pelo pagarme do pagamento: {objPagamento.IdPagamento}");
                throw;
            }

            return objBoleto;

        //}

        //public static SplitRule[] CreateSplitRule(Recipient recipient)
        //{
        //    List<SplitRule> splits = new List<SplitRule>();
            
        //    SplitRule split1 = new SplitRule()
        //    {
        //        Recipient = recipient,
        //        Percentage = 100
        //    };

        //    splits.Add(split1);

        //    return splits.ToArray();
        }


        #region [CarregarBoleto]
        public static DTO.DTOBoletoPagarMe CarregarBoletoPeloNossoNumero(string nossoNumero)
        {

            PagarMeService.DefaultApiKey = DefaultApiKeyPagarMe;
            PagarMeService.DefaultEncryptionKey = DefaultEncryptionKeyPagarMe;
            DTO.DTOBoletoPagarMe objBoleto = new DTO.DTOBoletoPagarMe();

            Transaction transaction = new Transaction();

            try
            {
                transaction = PagarMeService.GetDefaultService().Transactions.Find(nossoNumero);
                objBoleto.CodigoDeBarra = transaction.BoletoBarcode.Replace(" ", "").Replace(".", "");
                objBoleto.UrlBoleto = transaction.BoletoUrl;
                objBoleto.NossoNumero = transaction.Id;
                return objBoleto;
            }
            catch (Exception)
            {
                // não tem o boleto no pagar me
            }
            return objBoleto;
        }
        #endregion

        #region InformacoesBoleto]

        private static string InformacoesBoleto(decimal ValorBoleto)
        {
            var info = Parametro.RecuperaValorParametro(Enumeradores.Parametro.InformacoesBoleto);
            var multa = (ValorBoleto * 2)/ 100;
            var jurosDiario =  (ValorBoleto / 100) / 30;
            var jurosFormatado = Math.Truncate(jurosDiario * 100) / 100;
            if (jurosFormatado.Equals(0))
            {
                jurosFormatado = Math.Truncate(jurosDiario * 1000) / 1000;
            }
            info = info.Replace("{Multa}",(Math.Truncate(multa * 100)/100).ToString()).Replace("{Juros}", jurosFormatado.ToString());
            return info;
        }
        #endregion
    }
}
