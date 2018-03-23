using BNE.BLL;
using BNE.BLL.Custom.Email;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web.UI;
using Enumeradores = BNE.BLL.Enumeradores;
using BNE.BLL.Custom;
using BNE.Web.Master;
using System.Web.Services;
using BNE.BLL.Integracoes.Pagamento;
using Parametros = BNE.BLL.Custom.BuscaCurriculo.Parametros;
using System.Web;
using Newtonsoft.Json;
using BNE.BLL.Integracoes.Facebook;

namespace BNE.Web.Payment
{


    public partial class Payment : BasePagePagamento
    {
        #region Properties

        public static string UrlErro { get { return String.Format("http://{0}/{1}", UIHelper.RecuperarURLAmbiente(), "Payment/PaymentMobileErro.aspx"); } }
        public static string UrlSuccess { get { return String.Format("http://{0}/{1}", UIHelper.RecuperarURLAmbiente(), "Payment/PaymentMobileSuccess.aspx"); } }
        public static string UrlRegistered { get { return String.Format("http://{0}/{1}", UIHelper.RecuperarURLAmbiente(), "Payment/PaymentMobileRegistered.aspx"); } }

        #endregion Properties

        #region PageLoad
        protected void Page_Load(object sender, EventArgs e)
        {
        }
        #endregion PageLoad

        #region Login

        #region CadastroMini
        [WebMethod]
        public static object CadastroMini()
        {
            return new ReturnPaymentMobile(false, null, null, String.Format("http://{0}/{1}", UIHelper.RecuperarURLAmbiente(), Rota.RecuperarURLRota(Enumeradores.RouteCollection.CadastroCurriculoMini)));
        }
        #endregion

        #region ValidarLogin
        [WebMethod]
        public static object ValidarLogin(string login, string dataNascimento)
        {
            try
            {
                if (!string.IsNullOrEmpty(login) && !string.IsNullOrEmpty(dataNascimento))
                {
                    var cpf = String.Join("", System.Text.RegularExpressions.Regex.Split(login, @"[^\d]"));

                    PessoaFisica objPessoaFisica;
                    if (PessoaFisica.CarregarPorCPF(cpf, out objPessoaFisica))
                    {
                        //Se a data informada for diferente do cadastro mostra mensagem para ir para o SOSRH.
                        if (!Convert.ToDateTime(dataNascimento).Equals(objPessoaFisica.DataNascimento))
                            return new ReturnPaymentMobile(true, null, "Os dados foram informados incorretamente!");
                        else
                        {
                            Curriculo objCurriculo = null;
                            Curriculo.CarregarPorPessoaFisica(objPessoaFisica.IdPessoaFisica, out objCurriculo);

                            BNE.Auth.BNEAutenticacao.LogarCandidato(objPessoaFisica.NomePessoa, objPessoaFisica.IdPessoaFisica, objPessoaFisica.CPF, objPessoaFisica.DataNascimento, objCurriculo.IdCurriculo, false);
                            if (BNE.Auth.BNEAutenticacao.User() != null)
                                return new ReturnPaymentMobile(false, null, null, String.Format("http://{0}/{1}", UIHelper.RecuperarURLAmbiente(), "entrar/usando?i=" + objCurriculo.IdCurriculo + "c=" + login + "&d=" + dataNascimento + "&ret=Payment/PaymentMobileFluxoVip.aspx"));
                            else
                                return new ReturnPaymentMobile(true, null, "Falha ao tentar efetuar o acesso!");
                        }
                    }
                    else //Se a pessoa fisica não existir no banco de dados
                    {
                        return new ReturnPaymentMobile(true, null, null, String.Format("http://{0}/{1}", UIHelper.RecuperarURLAmbiente(), Rota.RecuperarURLRota(Enumeradores.RouteCollection.CadastroCurriculoMini)));
                    }
                }
                return new ReturnPaymentMobile(true, null, "Os dados foram informados incorretamente!");
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
                return new ReturnPaymentMobile(true, null, null, UrlErro);
            }
        }

        #endregion

        #region ValidarFacebook
        /// <summary>
        /// Validar id do facebook
        /// </summary>
        /// <param name="idFacebook"></param>
        /// <returns></returns>
        [WebMethod]
        public static object ValidarFacebook(string idFacebook)
        {
            try
            {
                PessoaFisicaRedeSocial objPessoaFisicaRedeSocial;
                if (PessoaFisicaRedeSocial.CarregarPorCodigoRedeSocial(idFacebook, BNE.BLL.Enumeradores.RedeSocial.FaceBook, out objPessoaFisicaRedeSocial))
                {
                    HttpContext.Current.Session["DadosFacebookPessoa"] = objPessoaFisicaRedeSocial.PessoaFisica.IdPessoaFisica;

                    Curriculo objCurriculo = null;
                    Curriculo.CarregarPorPessoaFisica(objPessoaFisicaRedeSocial.PessoaFisica.IdPessoaFisica, out objCurriculo);

                    BNE.Auth.BNEAutenticacao.LogarCandidato(objPessoaFisicaRedeSocial.PessoaFisica.NomePessoa, objPessoaFisicaRedeSocial.PessoaFisica.IdPessoaFisica, objPessoaFisicaRedeSocial.PessoaFisica.CPF, objPessoaFisicaRedeSocial.PessoaFisica.DataNascimento, objCurriculo.IdCurriculo, false);
                    if (BNE.Auth.BNEAutenticacao.User() != null)
                        return new ReturnPaymentMobile(false, null, null, String.Format("http://{0}/{1}", UIHelper.RecuperarURLAmbiente(), "entrar/usando?i=" + objCurriculo.IdCurriculo + "c=" + objPessoaFisicaRedeSocial.PessoaFisica.CPF + "&d=" + objPessoaFisicaRedeSocial.PessoaFisica.DataNascimento + "&ret=Payment/PaymentMobileFluxoVip.aspx"));
                    else
                        return new ReturnPaymentMobile(true, null, "Falha ao tentar efetuar o acesso!");

                }

                return new ReturnPaymentMobile(true, null, "Falha ao tentar efetuar o acesso!");
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex, "Falha ao recuperar dados do Facebook");
                throw;
            }
        }
        #endregion

        #region ArmazenarDadosFacebook
        /// <summary>
        /// Validar id do facebook
        /// </summary>
        /// <param name="jsonFacebook">JSON com todos os dados do Facebook</param>
        /// <param name="jsonFriendsFacebook">JSON com todos os amigos do Facebook</param>
        /// <param name="jsonMePicture">JSON com a url da foto do candidato</param>
        /// <returns></returns>
        [WebMethod]
        public static object ArmazenarDadosFacebook(string jsonFacebook, string jsonFriendsFacebook, string jsonMePicture)
        {
            try
            {
                var dadosFacebook = JsonConvert.DeserializeObject<ProfileFacebook.DadosFacebook>(jsonFacebook);
                var dadosFoto = JsonConvert.DeserializeObject<ProfileFacebook.FotoFacebook>(jsonMePicture);

                HttpContext.Current.Session["DadosFacebook"] = dadosFacebook;
                HttpContext.Current.Session["DadosFotoFacebook"] = dadosFoto;

                return CadastroMini();
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex, "Falha ao recuperar dados do Facebook");
                throw;
            }
        }
        #endregion


        #endregion Login

        #region Plano

        #region Escolha de Plano

        [WebMethod]
        public static object PlanosDisponiveis(int? idCurriculo, bool stcUniversitario)
        {
            try
            {
                Plano objPlanoRecorrente = null;
                Plano objMensal = null;
                Plano objTrimestral = null;


                if (idCurriculo.HasValue)
                {
                    FuncaoCategoria objFuncaoCategoria =
                        FuncaoCategoria.RecuperarCategoriaPorCurriculo(new Curriculo(idCurriculo.Value));

                    List<Enumeradores.Parametro> listaParametros =
                            new List<Enumeradores.Parametro>()
                    {
                        Enumeradores.Parametro.PlanoVIPUniversitarioMensal,
                        Enumeradores.Parametro.PlanoVIPUniversitarioTrimestral,
                        Enumeradores.Parametro.PlanoRecorrenteVIP
                    };

                    Dictionary<Enumeradores.Parametro, string> dictionaryParams = Parametro.ListarParametros(listaParametros);

                    if (stcUniversitario && dictionaryParams.ContainsKey(Enumeradores.Parametro.PlanoVIPUniversitarioMensal) && dictionaryParams.ContainsKey(Enumeradores.Parametro.PlanoVIPUniversitarioTrimestral))
                    {
                        objMensal = Plano.LoadObject(Convert.ToInt32(dictionaryParams[Enumeradores.Parametro.PlanoVIPUniversitarioMensal]));
                        objTrimestral = Plano.LoadObject(Convert.ToInt32(dictionaryParams[Enumeradores.Parametro.PlanoVIPUniversitarioTrimestral]));
                    }
                    else
                    {
                        objMensal = Plano.LoadObject(Plano.RecuperarCodigoPlanoMensalPorFuncaoCategoria(objFuncaoCategoria));
                        objTrimestral = Plano.LoadObject(Plano.RecuperarCodigoPlanoTrimestralPorFuncaoCategoria(objFuncaoCategoria));
                    }

                    objPlanoRecorrente = Plano.LoadObject(Convert.ToInt32(dictionaryParams[Enumeradores.Parametro.PlanoRecorrenteVIP]));

                    var listaPlanos = new List<object>(){
                            new { TipoPlano = "Mensal",  Plano = objMensal.DescricaoPlano, Id = objMensal.IdPlano, Valor = objMensal.ValorBase.ToString("N2")  },
                            new { TipoPlano = "Trimestral", Plano = objTrimestral.DescricaoPlano, Id = objTrimestral.IdPlano, Valor = objTrimestral.ValorBase.ToString("N2")  },
                            new { TipoPlano = "Recorrente", Plano = objPlanoRecorrente.DescricaoPlano, Id = objPlanoRecorrente.IdPlano, Valor = objPlanoRecorrente.ValorBase.ToString("N2") }
                        };

                    return new ReturnPaymentMobile(false, listaPlanos);
                }
                else
                    return new ReturnPaymentMobile(true);
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
                return new ReturnPaymentMobile(true, null, null, UrlErro);
            }
        }
        #endregion

        #region Validar - Criacao Do Plano
        [WebMethod]
        public static object ValidarCriacaoDoPlano(int? idPlano, int? idUsuarioFilialPerfilLogadoCandidato, int? idPessoaFisica, int? idCupomDeDesconto)
        {
            if (!idPlano.HasValue || !idUsuarioFilialPerfilLogadoCandidato.HasValue || !idPessoaFisica.HasValue) return new ReturnPaymentMobile(true);

            Plano objPlano = null;
            PlanoAdquirido objPlanoAdquirido = null;

            //Tratamento especial para compra de plano do relatório do salario BR - plano adquirido é criado aqui
            objPlano = Plano.LoadObject(idPlano.Value);
            if (objPlano == null)
                return new ReturnPaymentMobile(true, null, null, UrlErro);

            if (idCupomDeDesconto.HasValue)
            {
                decimal valorPlano = objPlano.ValorBase;
                CodigoDesconto objCodigoDesconto = new CodigoDesconto(idCupomDeDesconto.Value);
                objCodigoDesconto.CalcularDesconto(ref valorPlano, objPlano);
                objPlano.ValorBase = valorPlano;

                if (objPlano.ValorBase == 0)
                {
                    Curriculo objCurriculo = null;
                    Curriculo.CarregarPorPessoaFisica(idPessoaFisica.Value, out objCurriculo);
                    string erro = string.Empty;
                    if (Pagamento.ConcederDescontoIntegral(objCurriculo, new UsuarioFilialPerfil(idUsuarioFilialPerfilLogadoCandidato.Value),
                        new Plano(idPlano.Value), new CodigoDesconto(idCupomDeDesconto.Value), out erro))
                        return new ReturnPaymentMobile(true, null, null, UrlSuccess);
                    else
                        return new ReturnPaymentMobile(true, null, null, UrlErro);
                }
            }

            //Caso Pessoa Jurídica redireciona
            if (objPlano.ParaPessoaJuridica())
                return new ReturnPaymentMobile(true, null, "", String.Format("http://{0}/{1}", UIHelper.RecuperarURLAmbiente(), Enumeradores.RouteCollection.EscolhaPlano.ToString()));

            objPlanoAdquirido = PlanoAdquirido.CriarPlanoAdquiridoPF(UsuarioFilialPerfil.LoadObject(idUsuarioFilialPerfilLogadoCandidato.Value), objPlano, 1);

            if (objPlanoAdquirido != null)
            {
                string nomePlano = string.Empty;
                //Grava os Objetos Na Sessao e nas Variaveis Hidden
                if (objPlano.PlanoTipo.IdPlanoTipo == (int)Enumeradores.PlanoTipo.PessoaFisica)
                {
                    if (objPlano.QuantidadeDiasValidade == 0)
                        nomePlano = string.Empty;
                    else if (objPlano.FlagRecorrente)
                        nomePlano = "Plano Assinatura";
                    else
                        nomePlano = "Plano " + objPlano.QuantidadeDiasValidade + " dias";
                }
                return new ReturnPaymentMobile(false, new { IdPlanoAdquirido = objPlanoAdquirido.IdPlanoAdquirido, NomePlano = nomePlano, NomeUsuario = PessoaFisica.LoadObject(idPessoaFisica.Value).PrimeiroNome, Recorrente = objPlano.FlagRecorrente });
            }
            return new ReturnPaymentMobile(true, null, null, UrlErro);
        }
        #endregion

        #endregion Plano

        #region Cupom de Desconto

        #region Validar - Validação Código De Desconto
        [WebMethod]
        public static object CalculaCupomDeDesconto(string cupom, int idPlanoMensal, int idPlanoTrimestral)
        {
            try
            {
                if (string.IsNullOrEmpty(cupom))
                    return new ReturnPaymentMobile(true, null, "Cupom Não Preenchido!");

                CodigoDesconto codigo;
                if (!BLL.CodigoDesconto.CarregarPorCodigo(cupom, out codigo))
                    return new ReturnPaymentMobile(true, null, "Código promocional inválido!");

                if (codigo.JaUtilizado())
                    return new ReturnPaymentMobile(true, null, "Código promocional já utilizado!");

                if (!codigo.DentroValidade())
                    return new ReturnPaymentMobile(true, null, "Código promocional fora da validade");

                TipoCodigoDesconto tipoCodigoDesconto;
                if (!codigo.TipoDescontoDefinido(out tipoCodigoDesconto))
                    return new ReturnPaymentMobile(true, null, "Código promocional inválido");

                List<Plano> planosVinculados;
                if (codigo.HaPlanosVinculados(out planosVinculados))
                {
                    decimal valorMensal = new Plano(idPlanoMensal).RecuperarValor();
                    decimal valorTrimestral = new Plano(idPlanoTrimestral).RecuperarValor();

                    List<object> retornoDescontos = new List<object>();

                    if (planosVinculados.Any(plano => plano.IdPlano == idPlanoMensal))
                    {
                        codigo.CalcularDesconto(ref valorMensal);
                        retornoDescontos.Add(new { TipoPlano = "Mensal", IdPlano = idPlanoMensal, Valor = valorMensal.ToString("N2"), IdCodigoDeDesconto = codigo.IdCodigoDesconto });
                    }

                    if (planosVinculados.Any(plano => plano.IdPlano == idPlanoTrimestral))
                    {
                        codigo.CalcularDesconto(ref valorTrimestral);
                        retornoDescontos.Add(new { TipoPlano = "Trimestral", IdPlano = idPlanoTrimestral, Valor = valorTrimestral.ToString("N2"), IdCodigoDeDesconto = codigo.IdCodigoDesconto });
                    }

                    if (retornoDescontos.Count > 0)
                        return new ReturnPaymentMobile(false, retornoDescontos);
                    else
                        return new ReturnPaymentMobile(true, null, "Cupom não é válido para esses planos!");
                }
                return new ReturnPaymentMobile(true, null, "Esse código promocional não serve para o plano escolhido! Favor escolher outro plano ou trocar o código promocional");
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
                return new ReturnPaymentMobile(true, null, null, UrlErro);
            }
        }
        #endregion

        #endregion Cupom de Desconto

        #region Pagamentos

        #region Eventos

        #region Evento Click - PayPal
        [WebMethod]
        public static object PagamentoPayPal(int idPlanoAdquirido, int idPessoaFisica)
        {
            try
            {
                PlanoAdquirido objPlanoAdquirido = PlanoAdquirido.LoadObject(idPlanoAdquirido);
                if (PagamentoMobilePF.ProcessoCompra(ref objPlanoAdquirido, Enumeradores.TipoPagamento.PayPal))
                {
                    Pagamento objPagamento = null;
                    if (Pagamento.CarregarPagamentoPrimeiraParcelaEmAbertoPorPlanoAdquirido(objPlanoAdquirido.IdPlanoAdquirido, out objPagamento))
                    {
                        string url = Transacao.CriarTransacaoPayPal(objPagamento, objPlanoAdquirido, idPessoaFisica, PageHelper.RecuperarIP(), UIHelper.GetAbsoluteUrl(string.Format("Confirmacao-de-Pagamento/{0}/{1}", IntermediadorPagamento.PayPal.ToString(), idPlanoAdquirido)));
                        return new ReturnPaymentMobile(false, null, null, url);
                    }
                }
                return new ReturnPaymentMobile(true, null, null, UrlErro);
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
                return new ReturnPaymentMobile(true, null, null, UrlErro);
            }
        }
        #endregion

        #region Evento Click - Cartao Credito
        [WebMethod]
        public static object PagamentoCartaoDeCredito(string numeroCartao, int mesValidade, int anoValidade, string codigoSeguranca, int idPlanoAdquirido, int idPessoaFisica)
        {
            try
            {
                string erro = string.Empty;

                PlanoAdquirido objPlanoAdquirido = PlanoAdquirido.LoadObject(idPlanoAdquirido);
                if (PagamentoMobilePF.ProcessoCompra(ref objPlanoAdquirido, Enumeradores.TipoPagamento.CartaoCredito))
                {
                    Pagamento objPagamento = null;
                    if (Pagamento.CarregarPagamentoPrimeiraParcelaEmAbertoPorPlanoAdquirido(objPlanoAdquirido.IdPlanoAdquirido, out objPagamento))
                    {

                        if (Transacao.ValidarPagamentoCartaoCredito(ref objPagamento, objPlanoAdquirido.IdPlanoAdquirido, PageHelper.RecuperarIP(), numeroCartao, mesValidade, anoValidade, codigoSeguranca, null, out erro))
                        {
                            return new ReturnPaymentMobile(false, null, null, UrlSuccess);
                        }
                        else
                        {
                            return new ReturnPaymentMobile(true, null, null, UrlErro);
                        }
                    }
                }
                return new ReturnPaymentMobile(true, null, erro);
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
                return new ReturnPaymentMobile(true, null, null, UrlErro);
            }
        }
        #endregion

        #region Evento Click - Pagamento Boleto
        [WebMethod]
        public static object PagamentoBoleto(int idPessoaFisica, int idPlanoAdquirido, int isEmail)
        {
            try
            {
                PlanoAdquirido objPlanoAdquirido = PlanoAdquirido.LoadObject(idPlanoAdquirido);
                if (PagamentoMobilePF.ProcessoCompra(ref objPlanoAdquirido, Enumeradores.TipoPagamento.BoletoBancario))
                {
                    if (isEmail == 1)
                    {
                        string templateAssunto;
                        string mensagem = CartaEmail.RetornarConteudoBNE(Enumeradores.CartaEmail.BoletosParaPagamento, out templateAssunto);
                        byte[] pdf = BoletoBancario.MontarBoletoBytes(idPlanoAdquirido, Enumeradores.FormatoBoleto.PDF);

                        PessoaFisica objPessoaFisica = PessoaFisica.LoadObject(idPessoaFisica);

                        EmailSenderFactory.Create(TipoEnviadorEmail.Fila)
                            .Enviar(templateAssunto, mensagem, null, "atendimento@bne.com.br", objPessoaFisica.EmailPessoa, "boletos.pdf", pdf);
                        return new ReturnPaymentMobile(false, null, "Seu email foi Encaminhado com Sucesso!");
                    }
                    else
                    {
                        Pagamento objPagamento = null;
                        BoletoNet.Boleto objBoleto = null;
                        if (!Pagamento.CarregarPagamentoPrimeiraParcelaEmAbertoPorPlanoAdquirido(idPlanoAdquirido, out objPagamento))
                            Pagamento.CarregarPagamentoPrimeiraParcelaEmAbertoPorPlanoAdquirido(objPlanoAdquirido.IdPlanoAdquirido, out objPagamento);
                        objBoleto = BoletoBancario.ProcessarBoletoNovo(objPagamento, null, false);

                        if (string.IsNullOrEmpty(objPagamento.DescricaoDescricao))
                            return new ReturnPaymentMobile(false, String.Join("", System.Text.RegularExpressions.Regex.Split(objBoleto.CodigoBarra.LinhaDigitavel, @"[^\d]")));
                        return new ReturnPaymentMobile(false, objPagamento.DescricaoDescricao);
                    }
                }
                return new ReturnPaymentMobile(true, null, null, UrlErro);
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
                return new ReturnPaymentMobile(true, null, null, UrlErro);
            }
        }
        #endregion

        #region Evento Click - Pagamento PagSeguro
        [WebMethod]
        public static object PagamentoPagSeguro(int idPlanoAdquirido, int idPessoaFisica)
        {
            try
            {
                PlanoAdquirido objPlanoAdquirido = PlanoAdquirido.LoadObject(idPlanoAdquirido);
                if (PagamentoMobilePF.ProcessoCompra(ref objPlanoAdquirido, Enumeradores.TipoPagamento.PagSeguro))
                {
                    Pagamento objPagamento = null;
                    if (Pagamento.CarregarPagamentoPrimeiraParcelaEmAbertoPorPlanoAdquirido(objPlanoAdquirido.IdPlanoAdquirido, out objPagamento))
                    {
                        string url = Transacao.CriarTransacaoPagSeguro(objPagamento, objPlanoAdquirido, idPessoaFisica, PageHelper.RecuperarIP(), UIHelper.GetAbsoluteUrl(string.Format("Confirmacao-de-Pagamento/{0}/{1}", IntermediadorPagamento.PagSeguro.ToString(), idPlanoAdquirido)));
                        return new ReturnPaymentMobile(false, null, null, url);
                    }
                }
                return new ReturnPaymentMobile(true, null, null, UrlErro);
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
                return new ReturnPaymentMobile(true, null, null, UrlErro);
            }
        }
        #endregion

        #region Evento Click - Debito Recorrente HSBC
        [WebMethod]
        public static object PagamentoDebitoRecorrenteHSBC(string cpfOuCnpj, string agencia, string conta, string digito, int idPlanoAdquirido, int idPessoaFisica)
        {
            try
            {
                PlanoAdquirido objPlanoAdquirido = PlanoAdquirido.LoadObject(idPlanoAdquirido);
                if (PagamentoMobilePF.ProcessoCompra(ref objPlanoAdquirido, Enumeradores.TipoPagamento.DebitoRecorrente))
                {
                    Pagamento objPagamento = null;
                    if (Pagamento.CarregarPagamentoPrimeiraParcelaEmAbertoPorPlanoAdquirido(objPlanoAdquirido.IdPlanoAdquirido, out objPagamento))
                    {
                        string cnpj = null;
                        string cpf = null;
                        string erro = string.Empty;
                        string numeros = String.Join("", System.Text.RegularExpressions.Regex.Split(cpfOuCnpj, @"[^\d]"));

                        string contaDigito = conta + digito;

                        if (numeros.Length == 14)
                            cnpj = numeros;
                        else
                            cpf = numeros;

                        if (Transacao.ValidarPagamentoDebito(ref objPagamento, objPlanoAdquirido, PageHelper.RecuperarIP(), Enumeradores.Banco.HSBC, agencia, contaDigito, Convert.ToDecimal(cpf), Convert.ToDecimal(cnpj), out erro))
                            return new ReturnPaymentMobile(false, null, null, UrlRegistered);
                    }
                }
                return new ReturnPaymentMobile(true, null, null, UrlErro);
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
                return new ReturnPaymentMobile(true, null, null, UrlErro);
            }
        }
        #endregion

        #region Evento Click - Debito Online Bradesco
        [WebMethod]
        public static object PagamentoBradescoOnline(int idPlanoAdquirido, int idPessoaFisica)
        {
            try
            {
                PlanoAdquirido objPlanoAdquirido = PlanoAdquirido.LoadObject(idPlanoAdquirido);
                if (PagamentoMobilePF.ProcessoCompra(ref objPlanoAdquirido, Enumeradores.TipoPagamento.DebitoOnline))
                {
                    Pagamento objPagamento = null;
                    if (Pagamento.CarregarPagamentoPrimeiraParcelaEmAbertoPorPlanoAdquirido(objPlanoAdquirido.IdPlanoAdquirido, out objPagamento))
                    {
                        string erro = string.Empty;
                        Transacao objTransacao = Transacao.ValidarPagamentoDebitoOnline(ref objPagamento, objPlanoAdquirido, PageHelper.RecuperarIP(), Enumeradores.Banco.BRADESCO, out erro);
                        objTransacao.PlanoAdquirido = objPlanoAdquirido;

                        if (objTransacao != null)
                            return new ReturnPaymentMobile(false, null, null, EnviaPostBradesco(objTransacao, objPagamento));
                    }
                }
                return new ReturnPaymentMobile(true, null, null, UrlErro);
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
                return new ReturnPaymentMobile(true, null, null, UrlErro);
            }
        }
        #endregion

        #region Evento Click - Debito Online Banco do Brasil
        [WebMethod]
        public static object PagamentoBancoDoBrasilOnline(int idPlanoAdquirido, int idPessoaFisica)
        {
            try
            {
                PlanoAdquirido objPlanoAdquirido = PlanoAdquirido.LoadObject(idPlanoAdquirido);
                if (PagamentoMobilePF.ProcessoCompra(ref objPlanoAdquirido, Enumeradores.TipoPagamento.DebitoOnline))
                {
                    Pagamento objPagamento = null;
                    if (Pagamento.CarregarPagamentoPrimeiraParcelaEmAbertoPorPlanoAdquirido(objPlanoAdquirido.IdPlanoAdquirido, out objPagamento))
                    {
                        //Se existe transações do cliente dentro de um intervalo, não o deixe criar uma nova
                        if (!Transacao.ExisteTransacaoDebitoOnlineNoIntervalo(idPessoaFisica, Parametro.RecuperaValorParametro(Enumeradores.Parametro.IntervaloTempoSondaDebitoOnlineBB)))
                        {
                            string erro = string.Empty;

                            Transacao objTransacao = Transacao.ValidarPagamentoDebitoOnline(ref objPagamento, objPlanoAdquirido, PageHelper.RecuperarIP(), Enumeradores.Banco.BANCODOBRASIL, out erro);

                            if (objTransacao != null)
                            {
                                return new ReturnPaymentMobile(false, null, null, EnviaPostBancoDoBrasil(objTransacao, objPagamento));
                            }
                        }
                    }
                }
                return new ReturnPaymentMobile(true, null, null, UrlErro);
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
                return new ReturnPaymentMobile(true, null, null, UrlErro);
            }
        }

        #endregion

        #endregion Eventos

        #region Falha Na Compra
        [WebMethod]
        public static object RedirectTelaPaymentMobileErro(int idPessoaFisica, int idPlanoAdquirido)
        {
            return new ReturnPaymentMobile(true, null, null, UrlErro);
        }
        #endregion Falha Na Compra

        #region Debito Online

        #region BRADESCO

        #region Debito Online - POST - Bradesco
        private static string EnviaPostBradesco(Transacao objTransacao, Pagamento objPagamento)
        {
            //Criação do POST de Envio para o BB
            string numConvenio = Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.ConvenioDebitoBradesco);
            string dadosDaRequisicao = string.Format(Parametro.RecuperaValorParametro(Enumeradores.Parametro.URLDebitoOnlineBradesco) + "&descritivo={3}&quantidade=1&unidade=UN&valor={4}"
                    , numConvenio
                    , numConvenio
                    , objTransacao.IdTransacao
                    , objTransacao.PlanoAdquirido.Plano.CompleteObject() ? objTransacao.PlanoAdquirido.Plano.DescricaoPlano : ""
                    , Convert.ToString(objPagamento.ValorPagamento).Replace(",", "").TrimStart(new Char[] { '0' })
                    );
            return dadosDaRequisicao;
        }
        #endregion

        #endregion

        #region BANCO DO BRASIL

        #region Debito Online - POST - Banco do Brasil
        private static string EnviaPostBancoDoBrasil(Transacao objTransacao, Pagamento objPagamento)
        {
            //Criação do POST de Envio para o BB
            string dadosDaRequisicao = string.Format("idConv={0}&refTran={1}&valor={2}&dtVenc={3}&tpPagamento={4}&urlRetorno={5}&urlInforma={6}",
                                        Parametro.RecuperaValorParametro(Enumeradores.Parametro.ConvenioDebitoBB),
                                        Convert.ToString(objTransacao.IdTransacao).PadLeft(17, '0'),
                                        Convert.ToString(objPagamento.ValorPagamento).Replace(",", "").TrimStart(new Char[] { '0' }),
                                        objPagamento.DataVencimento.Value.ToString("ddMMyyyy"),
                                        Parametro.RecuperaValorParametro(Enumeradores.Parametro.DebitoEmContaViaInternetPFePJ),
                                        Parametro.RecuperaValorParametro(Enumeradores.Parametro.URLRetornoDebitoOnline),
                                        Parametro.RecuperaValorParametro(Enumeradores.Parametro.URLInformaDebitoOnline)
                                        );

            return @"../PagamentoDebitoOnlineBB.aspx?" + dadosDaRequisicao;
        }
        #endregion


        #endregion

        #endregion Debito Online

        #endregion Pagamentos


    }
}