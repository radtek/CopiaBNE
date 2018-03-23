using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using BNE.BLL;
using BNE.BLL.Custom;
using BNE.BLL.Custom.Email;
using BNE.BLL.Enumeradores;
using BNE.EL;
using BNE.Services.Base.EventLog;
using BNE.Services.Properties;
using Banco = BNE.BLL.Enumeradores.Banco;
using CartaEmail = BNE.BLL.CartaEmail;
using Parametro = BNE.BLL.Parametro;
using SituacaoFilial = BNE.BLL.Enumeradores.SituacaoFilial;
using TipoArquivo = BNE.BLL.Enumeradores.TipoArquivo;
using TipoPagamento = BNE.BLL.Enumeradores.TipoPagamento;
using System.Configuration;

namespace BNE.Services
{
    internal partial class ControleParcelas : BaseService
    {

        #region Construtor
        public ControleParcelas()
        {
            InitializeComponent();
        }
        #endregion

        #region Propriedades
        private Thread _objThread;
        private static DateTime _dataHoraUltimaExecucao;
        private static string HoraExecucao = Settings.Default.ControleParcelasHoraExecucao;
        private static int DelayExecucao = Settings.Default.ControleParcelasDelayMinutos;
        public static readonly string UtilizaCielo = ConfigurationManager.AppSettings["UtilizaCielo"];
        public static readonly string UtilizaPagarMe = ConfigurationManager.AppSettings["UtilizaPagarMe"];

        #endregion

        #region Consultas

        #region SpUpdateStatusEmpresa
        private const string SP_UPDATE_STATUS_EMPRESA =
            @"UPDATE TAB_FILIAL SET idf_Situacao_Filial = @IdSituacaoFilial WHERE idf_Filial = @IdFilial";
        #endregion

        #endregion

        #region Eventos

        #region OnStart
        protected override void OnStart(string[] args)
        {
            (_objThread = new Thread(IniciarControleParcelas)).Start();
        }
        #endregion

        #region OnStop
        protected override void OnStop()
        {
            if (_objThread != null)
                _objThread.Abort();
        }
        #endregion

        #endregion

        #region Services

        #region ControleDeEnvioNotaFiscalAntecipada
        private void ControleDeEnvioNotaFiscalAntecipada()
        {
            #region [E-mail Informativo]
            try
            {
#if !DEBUG
                EventLogWriter.LogEvent(String.Format("Iniciou agora o ControleDeEnvioNotaFiscalAntecipada {0}.", DateTime.Now), EventLogEntryType.Information, Event.InicioExecucao);
#endif
                using (var row = PlanoParcela.ListarParcelasEmissaoNotaFiscalAntecipada())
                {
                    var emailDestinatario = "financeiro@bne.com.br";
                    var assunto = "Envio de Notas Fiscais Antecipadas.";
                    string mensagem, mensagemComplemento = string.Empty;

                    var idUsuarioFilialNotaAntencipada =
                        Convert.ToInt32(
                            Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.IdUsuarioFilialNotaAntencipada));
                    if (string.IsNullOrWhiteSpace(emailDestinatario))
                        throw new Exception("ControleParcelasAVencer: Email vazio");

                    mensagem = @"<p>As notas fiscais emitidas pelo nosso sistema são:</p>
                                        <table>
                                            <tr><td>CNPJ da Empresa</td><td>Nome Empresa</td><td>Nota Emitida?</td></tr>";

                    while (row.Read())
                    {
                        try
                        {
                            int n;
                            if (!int.TryParse(row["Idf_Pagamento"].ToString(), out n))
                                throw new Exception("ControleParcelasAVencer: Pagamento inválido");


                            if (PlanoParcela.EmitirNF(Pagamento.LoadObject(n), idUsuarioFilialNotaAntencipada))
                                mensagemComplemento += @"<tr><td>" + row["Num_CNPJ"] + "</td><td>" + row["Nme_Fantasia"] +
                                                       "</td><td> SIM </td></tr>";
                            else
                                mensagemComplemento += @"<tr><td>" + row["Num_CNPJ"] + "</td><td>" + row["Nme_Fantasia"] +
                                                       "</td><td> NÃO </td></tr>";
                        }
                        catch (Exception ex)
                        {
                            string message;
                            var id = GerenciadorException.GravarExcecao(ex, out message);
                            message = string.Format("{0} - {1}", id, message);
#if !DEBUG
                            EventLogWriter.LogEvent(message, EventLogEntryType.Error, Event.ErroExecucao);
#endif
                        }
                    }

                    if (string.IsNullOrEmpty(mensagemComplemento))
                        mensagem = "Não existem mensagem na data de hoje dia " +
                                   DateTime.Today.Date.ToString("dd/MM/yyyy");
                    else
                        mensagem += mensagemComplemento + "</table>";
                    var objUsuarioFilialPerfil = UsuarioFilialPerfil.LoadObject(idUsuarioFilialNotaAntencipada);
                    MensagemCS.SalvarEmail(null, objUsuarioFilialPerfil, objUsuarioFilialPerfil, null, assunto, mensagem, null,
                        "rodrigobandini@bne.com.br", emailDestinatario, string.Empty, null, null);
                }

#if !DEBUG
                EventLogWriter.LogEvent(string.Format("Terminou agora o ControleDeEnvioNotaFiscalAntecipada {0}.", DateTime.Now), EventLogEntryType.Information, Event.FimExecucao);
#endif
            }
            catch (Exception ex)
            {
                string message;
                var id = GerenciadorException.GravarExcecao(ex, out message);
                message = string.Format("{0} - {1}", id, message);
#if !DEBUG
                EventLogWriter.LogEvent(message, EventLogEntryType.Error, Event.ErroExecucao);
#endif
            }
            #endregion
        }
        #endregion

        #region ControleDeEnvioDeBoletoComVencimentoParaXdias
        private void ControleDeEnvioDeBoletoComVencimentoParaXdias()
        {
            try
            {
#if !DEBUG
                EventLogWriter.LogEvent(String.Format("Iniciou agora o ControleDeEnvioDeBoletoComVencimentoParaXdias {0}.", DateTime.Now), EventLogEntryType.Information, Event.InicioExecucao);
#endif
                using (var dr = Pagamento.CarregarBoletosEmVencimentos())
                {
                    var emailFinanceiro = "financeiro@bne.com.br";
                    var assunto = "Envio de Boletos em Vencimento.";
                    string mensagem, mensagemComplemento = string.Empty;

                    var idUsuarioFilialNotaAntencipada =
                        Convert.ToInt32(
                            Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.IdUsuarioFilialNotaAntencipada));
                    if (string.IsNullOrWhiteSpace(emailFinanceiro))
                        throw new Exception("ControleParcelasAVencer: Email vazio");

                    mensagem = @"<p>Boletos em Vencimento em nosso sistema são:</p>
                                        <table>
                                            <tr><td>CNPJ da Empresa</td><td>Nome Empresa</td><td>Boleto de vencimento enviado?</td></tr>";

                    while (dr.Read())
                    {
                        try
                        {
                            var emailRemetente = String.Format("{0};{1}", Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.ControleParcelasRemetente), dr["EmailVendedor"].ToString());
                            var emailDestinatario = dr["EmailEnvio"].ToString();
                            var corpoEmail =
                                @"<table align=\""center\"" cellpadding=\""0\"" cellspacing=\""0\"" width=\""540\""> <tr> <td style=\""line-height: 150%;\""> <font color=\""#333333\"" face=\""Arial, Helvetica, sans-serif\"" size=\""3\""> <div style=\""margin-left:25px;\""> {NomeEmpresa}<br> <br> Lembramos que o vencimento da próxima parcela referente ao plano {NomePlano} no www.bne.com.br será {DataVencimento}. <br> Caso necessário, o boleto pode ser reimpresso através de sua conta no BNE, na opção Meu Plano, na sala selecionadora. <br><br> Atenciosamente <br><br> {NomeVendedor}<br> Email: {EmailVendedor}<br> Tel: {TelVendedor} </div> </font> </td> </tr> </table>";

                            corpoEmail = corpoEmail.Replace("{NomeEmpresa}", dr["NomeEnvio"].ToString())
                                .Replace("{NomePlano}", dr["NomePlano"].ToString())
                                .Replace("{DataVencimento}", dr["DataVencimento"].ToString())
                                .Replace("{NomeVendedor}", dr["NomeVendedor"].ToString())
                                .Replace("{EmailVendedor}", dr["EmailVendedor"].ToString())
                                .Replace("{TelVendedor}",
                                    string.IsNullOrEmpty(dr["TelVendedor"].ToString())
                                        ? "0800 41 2400"
                                        : string.Format("{0:(##) ####-####}", dr["TelVendedor"]));

                            if (MensagemCS.SalvarEmail(null, null,
                                UsuarioFilialPerfil.LoadObject(Convert.ToInt32(dr["Idf_Usuario_Filial_Perfil"])), null,
                                "Vencimento do Boleto", corpoEmail, null, emailRemetente, emailDestinatario, null, null, null))
                                mensagemComplemento += @"<tr><td>" + dr["Num_CNPJ"] + "</td><td>" + dr["NomeEnvio"] +
                                                       "</td><td> SIM </td></tr>";
                            else
                                mensagemComplemento += @"<tr><td>" + dr["Num_CNPJ"] + "</td><td>" + dr["NomeEnvio"] +
                                                       "</td><td> NÃO </td></tr>";
                        }
                        catch (Exception ex)
                        {
                            GerenciadorException.GravarExcecao(ex);
                        }
                    }
                    if (string.IsNullOrEmpty(mensagemComplemento))
                        mensagem = "Não existem Boletos com vencimento na data de hoje dia " +
                                   DateTime.Today.Date.ToString("dd/MM/yyyy");
                    else
                        mensagem += mensagemComplemento + "</table>";
                    var objUsuarioFilialPerfil = UsuarioFilialPerfil.LoadObject(idUsuarioFilialNotaAntencipada);
                    MensagemCS.SalvarEmail(null, objUsuarioFilialPerfil, objUsuarioFilialPerfil, null, assunto, mensagem, null,
                        "rodrigobandini@bne.com.br", "financeiro@bne.com.br", string.Empty, null, null);
                }
#if !DEBUG
                EventLogWriter.LogEvent(string.Format("Terminou agora o ControleDeEnvioDeBoletoComVencimentoParaXdias {0}.", DateTime.Now), EventLogEntryType.Information, Event.FimExecucao);
#endif
            }
            catch (Exception ex)
            {
                GerenciadorException.GravarExcecao(ex);
            }
        }
        #endregion

        #region ControleVencimentoParcelasRecorrencia
        public void ControleVencimentoParcelasRecorrencia()
        {

#if !DEBUG
            EventLogWriter.LogEvent(String.Format("Iniciou agora o ControleVencimentoParcelasRecorrencia {0}.", DateTime.Now), EventLogEntryType.Information, Event.InicioExecucao);
#endif

            var listPlanoAdquirido = PlanoAdquirido.CarregaListaPlanoAdquiridosComRecorrencia();

            var mensagem = @"<p>Empresas com Recorrência para ser Renovada:</p>";

            string msgPF = @"<h2>Pessoa Física</h2><table><tr><td>CPF</td><td>Nome</td><td>Plano</td><td>Forma de Pagamento</td><td>Sucesso?</td><td>Data Fim</td><td>Resultado</td></tr>";
            string msgPJ = @"<h2>Pessoa Jurídica</h2><table><tr><td>CNPJ</td><td>Nome</td><td>Plano</td><td>Forma de Pagamento</td><td>Sucesso?</td><td>Data Fim</td><td>Resultado</td></tr>";

            int contadorCielo = 0;
            int contadorPagarMe = 0;

            foreach (var objPlanoAdquirido in listPlanoAdquirido)
            {
                try
                {
                    //recuperar a última transacao paga
                    var objTransacaoUltima =
                        Transacao.CarregarUltimaTransacaoPagaPorPlanoAdquirido(objPlanoAdquirido.IdPlanoAdquirido);

                    if (UtilizaCielo == "True" && UtilizaPagarMe == "True")
                    {
                        if (string.IsNullOrEmpty(objTransacaoUltima.NomeCartaoCredito))
                        {
                            objTransacaoUltima.GerenciadoraTransacao = (Int32)BLL.Enumeradores.GerenciadoraTransacao.Cielo;
                        }
                        else
                        {
                            if (contadorPagarMe >= contadorCielo)
                            {
                                objTransacaoUltima.GerenciadoraTransacao =
                                    (Int32)BLL.Enumeradores.GerenciadoraTransacao.Cielo;
                                contadorCielo++;
                            }
                            else
                            {
                                objTransacaoUltima.GerenciadoraTransacao = (Int32)BLL.Enumeradores.GerenciadoraTransacao.PagarMe;
                                contadorPagarMe++;
                            }
                        }
                    }
                    else
                        objTransacaoUltima.GerenciadoraTransacao = UtilizaCielo == "True" ? (Int32)BLL.Enumeradores.GerenciadoraTransacao.Cielo : (Int32)BLL.Enumeradores.GerenciadoraTransacao.PagarMe;

                    objPlanoAdquirido.Plano.CompleteObject();
                    PlanoParcela objPlanoParcela;
                    Pagamento objPagamento;
                    var segundoMesGratis = PlanoParcela.ValidacoesSegundaGratis(objPlanoAdquirido);

                    PlanoDesconto objPlanoDesconto;
                    //Task 57029 - Verificar se tem desconto para a parcela do plano vip.
                    string usuarioInterno;
                    var DescontoParcela = PlanoDesconto.RecuperaDesconto(objPlanoAdquirido.IdPlanoAdquirido, out objPlanoDesconto, out usuarioInterno);


                    if (objPlanoAdquirido.Plano.IdPlano == 676 && segundoMesGratis)
                    {
                        objPlanoAdquirido.ValorBase = 0;
                        objPlanoParcela = PlanoParcela.CriarParcelaRecorrenciaPeloPlanoAdquiridoSegundoMesGratis(objPlanoAdquirido);
                        objPagamento = Pagamento.CriarPagamentoRecorrenciaSegundoMesGratis(objPlanoParcela, objPlanoAdquirido,
                            objTransacaoUltima);
                    }
                    else if (DescontoParcela)
                    {
                        objPlanoAdquirido.ValorBase = objPlanoDesconto.ValorDesconto.Value;
                        objPlanoParcela = PlanoParcela.CriarParcelaRecorrenciaPeloPlanoAdquirido(objPlanoAdquirido, objPlanoDesconto);
                      if(objPlanoParcela.ValorParcela.Equals(0))// segundo mes faz o mesmo que ia fazer com o valor da parcela 0
                            objPagamento = Pagamento.CriarPagamentoRecorrenciaSegundoMesGratis(objPlanoParcela, objPlanoAdquirido, objTransacaoUltima);
                        else
                            objPagamento = Pagamento.criarPagamentoRecorrencia(objPlanoParcela, objPlanoAdquirido, objTransacaoUltima);

                    }
                    else
                    {
                        //criar uma nova parcela
                        objPlanoParcela = PlanoParcela.CriarParcelaRecorrenciaPeloPlanoAdquirido(objPlanoAdquirido, null);
                        //criar um novo pagamento
                        objPagamento = Pagamento.criarPagamentoRecorrencia(objPlanoParcela, objPlanoAdquirido,
                         objTransacaoUltima);
                    }
                 

                    var erro = string.Empty;
                    var resultadoTransacaoResposta = string.Empty;

                    var retorno = false;

                    int tipoPagamento = objTransacaoUltima.TipoPagamento.IdTipoPagamento;

                    switch (tipoPagamento)
                    {
                        case (int)TipoPagamento.DebitoRecorrente:
                            Transacao.ValidarPagamentoDebito(ref objPagamento, objPlanoAdquirido,
                                objTransacaoUltima.DescricaoIPComprador, Banco.HSBC,
                                objTransacaoUltima.DescricaoAgenciaDebito,
                                objTransacaoUltima.DescricaoContaCorrenteDebito,
                                objTransacaoUltima.NumeroCPFTitularContaCorrenteDebito, objTransacaoUltima.NumeroCNPJTitularContaCorrenteDebito, out erro);
                            break;
                        case (int)TipoPagamento.CartaoCredito:
                            retorno = Transacao.ValidarPagamentoCartaoCredito(ref objPagamento,
                                objPlanoAdquirido.IdPlanoAdquirido, string.Empty, objTransacaoUltima.NumeroCartaoCredito,
                                Convert.ToInt32(objTransacaoUltima.NumeroMesValidadeCartaoCredito),
                                Convert.ToInt32(objTransacaoUltima.NumeroAnoValidadeCartaoCredito),
                                objTransacaoUltima.NumeroCodigoVerificadorCartaoCredito, objTransacaoUltima.NomeCartaoCredito, objTransacaoUltima.DesCodigoToken, (BLL.Enumeradores.GerenciadoraTransacao)objTransacaoUltima.GerenciadoraTransacao, out erro, out resultadoTransacaoResposta);
                            break;
                    }

                   if ((segundoMesGratis && objPlanoAdquirido.Plano.IdPlano == 676) || (DescontoParcela  && objPlanoParcela.ValorParcela.Equals(0)))
                    {
                        var transacaoRealizada = Transacao.CarregaTransacaoPorPagamento(objPagamento.IdPagamento);
                        transacaoRealizada.StatusTransacao = new BLL.StatusTransacao((int)BLL.Enumeradores.StatusTransacao.Capturada);
                        transacaoRealizada.Save();
                        var transacaoResposta = TransacaoResposta.AlterarRespostaPorIdTransacao(transacaoRealizada.IdTransacao);
                        transacaoResposta.FlagTransacaoAprovada = true;
                        transacaoResposta.DescricaoResultadoSolicitacaoAprovacao = "Segundo Mes Free";
                        transacaoResposta.Save();
                        retorno = true;

                    }

                    if (retorno)
                    {
                        //Marcar desconto como usado
                        if (DescontoParcela)
                        {
                            objPlanoDesconto.FlagInativo = true;
                            objPlanoDesconto.DataUtilizacao = DateTime.Now;
                            objPlanoDesconto.PlanoParcela = objPlanoParcela;
                            objPlanoDesconto.Save();
                        }
                        PlanoAdquirido.SalvarNovaDataFimPlano(objPlanoAdquirido);
                        if (objPlanoAdquirido.ParaPessoaFisica() && objPlanoAdquirido.PlanoSituacao.IdPlanoSituacao == (int)BNE.BLL.Enumeradores.PlanoSituacao.Bloqueado)
                            objPlanoAdquirido.DesbloquearPlano(objPlanoAdquirido.UsuarioFilialPerfil, "Regularização das Parcelas");
                    }


                    //Construindo o email de retorno


                    if (objPlanoAdquirido.ParaPessoaJuridica() && objPlanoAdquirido.Filial.CompleteObject())
                    {
                        objTransacaoUltima.TipoPagamento.CompleteObject();
                        objPlanoAdquirido.Plano.CompleteObject();
                        var resultado = objTransacaoUltima.TipoPagamento.IdTipoPagamento == 8
                            ? "ENVIADO PARA O HSBC"
                            : (retorno ? "SIM" : "NÃO");
                        msgPJ += @"<tr><td>" + objPlanoAdquirido.Filial.CNPJ + "</td><td>" +
                                    objPlanoAdquirido.Filial.RazaoSocial + "</td><td>" +
                                    objPlanoAdquirido.Plano.DescricaoPlano + "</td><td>" +
                                    objTransacaoUltima.TipoPagamento.DescricaoTipoPagamaneto + "</td><td>" + resultado +
                                    "</td><td>" + objPlanoAdquirido.DataFimPlano + "</td><td>" + erro + "</td></tr>";

                        if (!retorno && tipoPagamento == (int)TipoPagamento.CartaoCredito)
                        {
                            FilialObservacao.SalvarCRM(
                            "Plano Recorrente " + objPlanoAdquirido.IdPlanoAdquirido +
                            " Cancelado, via Sistema, motivo: " + erro, objPlanoAdquirido.Filial,
                            "ControleParcelas -> Renovacao Recorrencia");
                        }
                    }
                    else if (objPlanoAdquirido.ParaPessoaFisica() && objPlanoAdquirido.UsuarioFilialPerfil.CompleteObject() && objPlanoAdquirido.UsuarioFilialPerfil.PessoaFisica.CompleteObject())
                    {
                        objTransacaoUltima.TipoPagamento.CompleteObject();
                        objPlanoAdquirido.Plano.CompleteObject();
                        var resultado = objTransacaoUltima.TipoPagamento.IdTipoPagamento == 8
                            ? "ENVIADO PARA O HSBC"
                            : (retorno ? "SIM" : "NÃO");
                        msgPF += @"<tr><td>" + objPlanoAdquirido.UsuarioFilialPerfil.PessoaFisica.CPF + "</td><td>" +
                                    objPlanoAdquirido.UsuarioFilialPerfil.PessoaFisica.NomeCompleto + "</td><td>" +
                                    objPlanoAdquirido.Plano.DescricaoPlano + "</td><td>" +
                                    objTransacaoUltima.TipoPagamento.DescricaoTipoPagamaneto + "</td><td>" + resultado +
                                    "</td><td>" + erro + "</td></tr>";


                        Curriculo objCurriculo = null;
                        Curriculo.CarregarPorCpf(objPlanoAdquirido.UsuarioFilialPerfil.PessoaFisica.CPF, out objCurriculo);
                        //
                        if (!retorno && tipoPagamento == (int)TipoPagamento.CartaoCredito)
                        {
                            CurriculoObservacao.SalvarCRM(
                                "Plano Recorrente " + objPlanoAdquirido.IdPlanoAdquirido +
                                " Cancelado, via Sistema, motivo: " + erro, objCurriculo,
                                "ControleParcelas -> Renovacao Recorrencia");

                            //salvar no crm o motivo da compra com cartão não ser realizada
                            CurriculoObservacao.SalvarCRM("Pagamento Cartão do plano: " + objPlanoAdquirido.IdPlanoAdquirido +
                                 " não realizado, motivo: " + erro +", " + resultadoTransacaoResposta, objCurriculo,
                                 "ControleParcelas -> Renovacao Recorrencia");
                        }
                    }
                }
                catch (Exception ex)
                {
                    string message;
                    var id = GerenciadorException.GravarExcecao(ex, out message, "Erro ao executar o processo Recorrência");
                    message = string.Format("{0} - {1}", id, message);
                    EventLogWriter.LogEvent(message, EventLogEntryType.Error, Event.ErroExecucao);
                }
            }

            if (listPlanoAdquirido.Count > 0)
            {
                mensagem += msgPJ + "</table>";
                mensagem += msgPF + "</table>";

                var objUsuarioFilialPerfil =
                    UsuarioFilialPerfil.LoadObject(
                        Convert.ToInt32(
                            Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.IdUsuarioFilialNotaAntencipada)));
                MensagemCS.SalvarEmail(null, objUsuarioFilialPerfil, objUsuarioFilialPerfil, null,
                    "Renovação Planos Recorrentes", mensagem, null, "rodrigobandini@bne.com.br",
                    "financeiro@bne.com.br;adrianogoncalves@bne.com.br;rodrigobandini@bne.com.br;gaida@bne.com.br", string.Empty, null, null);
            }

#if !DEBUG
            EventLogWriter.LogEvent(string.Format("Terminou agora o ControleVencimentoParcelasRecorrencia {0}.", DateTime.Now), EventLogEntryType.Information, Event.FimExecucao);
#endif
        }
        #endregion

       
        #region Controle de Visualizacoes de Planos
        private void ControleVisualizacoesPlanos()
        {
#if !DEBUG
            EventLogWriter.LogEvent(String.Format("Iniciou agora o ControleVisualizacoesPlanos {0}.", DateTime.Now), EventLogEntryType.Information, Event.InicioExecucao);
#endif
            using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();

                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        foreach (PlanoAdquirido objPlanoAdquirido in PlanoAdquirido.CarregaListaPlanoAdquiridosWebForPag50(trans))
                        {
                            Pagamento objPagamento = null;
                            Pagamento.CarregarPagamentoPrimeiraParcelaEmAbertoPorPlanoAdquirido(objPlanoAdquirido.IdPlanoAdquirido, out objPagamento);

                            if (objPagamento != null && objPagamento.DataVencimento <= DateTime.Today && objPagamento.Liberar(trans, DateTime.Today))
                                PlanoQuantidade.ReiniciarContagemSaldo(objPlanoAdquirido, trans);
                        }
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
#if !DEBUG
            EventLogWriter.LogEvent(string.Format("Terminou agora o ControleVisualizacoesPlanos {0}.", DateTime.Now), EventLogEntryType.Information, Event.FimExecucao);
#endif
        }

        #endregion

        #region VencimentoParcelasCartaoDeCredito
        private void VencimentoParcelasCartaoDeCredito()
        {
            try
            {
#if !DEBUG
                EventLogWriter.LogEvent(String.Format("Iniciou agora o VencimentoParcelasCartaoDeCredito {0}.", DateTime.Now), EventLogEntryType.Information, Event.InicioExecucao);
#endif

                var listPagamentos = Pagamento.RecuperarPagamentosCartaoDeCreditoAVencer();
                var mensagem = @"
                <p>Empresas com Pagamento Plano Via Cartão de Crédito:</p>
                    <table>
                        <tr><td>CNPJ da Empresa</td><td>Nome Empresa</td><td>Plano</td><td>Forma de Pagamento</td><td>Sucesso?</td><td>Resultado</td></tr>";

                int contadorPagarMe = 0;
                int contadorCielo = 0;    
                    

                foreach (var objPagamento in listPagamentos)
                {
                    bool retorno = false;
                    string motivo = string.Empty;
                    string resultadoTransacaoResposta = string.Empty;

                    PlanoAdquirido objPlanoAdquirido = PlanoAdquirido.CarregarPlanoAdquiridopDePagamento(objPagamento.IdPagamento);
                    
                    var objTransacaoUltima = Transacao.CarregarUltimaTransacaoPagaPorPlanoAdquirido(objPlanoAdquirido.IdPlanoAdquirido);

                    if (UtilizaCielo == "True" && UtilizaPagarMe == "True")
                    {
                        if (string.IsNullOrEmpty(objTransacaoUltima.NomeCartaoCredito))
                        {
                            objTransacaoUltima.GerenciadoraTransacao = (Int32)BLL.Enumeradores.GerenciadoraTransacao.Cielo;
                        }
                        else
                        {
                            if (contadorPagarMe >= contadorCielo)
                            {
                                objTransacaoUltima.GerenciadoraTransacao =
                                    (Int32)BLL.Enumeradores.GerenciadoraTransacao.Cielo;
                                contadorCielo++;
                            }
                            else
                            {
                                objTransacaoUltima.GerenciadoraTransacao = (Int32)BLL.Enumeradores.GerenciadoraTransacao.PagarMe;
                                contadorPagarMe++;
                            }
                        }
                    }
                    else
                        objTransacaoUltima.GerenciadoraTransacao = UtilizaCielo == "True" ? (Int32)BLL.Enumeradores.GerenciadoraTransacao.Cielo : (Int32)BLL.Enumeradores.GerenciadoraTransacao.PagarMe;

                    if (objPagamento.DataVencimento > DateTime.Today.AddDays(14))
                    {
                        motivo = "Não foi possível efetuar a cobrança via cartão de crédito, Plano Adquirido" + objPlanoAdquirido.IdPlanoAdquirido + " bloqueada";
                        //objPlanoAdquirido.BloquearPlano(null, motivo);
                    }
                    else
                    {

                        var objPagamentoClone = objPagamento.Clone() as Pagamento;
                        retorno = Transacao.ValidarPagamentoCartaoCredito(ref objPagamentoClone,
                                objPlanoAdquirido.IdPlanoAdquirido, string.Empty, objTransacaoUltima.NumeroCartaoCredito,
                                Convert.ToInt32(objTransacaoUltima.NumeroMesValidadeCartaoCredito),
                                Convert.ToInt32(objTransacaoUltima.NumeroAnoValidadeCartaoCredito),
                                objTransacaoUltima.NumeroCodigoVerificadorCartaoCredito, objTransacaoUltima.NomeCartaoCredito, objTransacaoUltima.DesCodigoToken,
                                (BLL.Enumeradores.GerenciadoraTransacao)objTransacaoUltima.GerenciadoraTransacao, out motivo,out resultadoTransacaoResposta);
                        if (!retorno)
                        {//salvar no crm o motivo da compra com cartão não ser realizada
                            Curriculo objCurriculo = null;
                            Curriculo.CarregarPorCpf(objPlanoAdquirido.UsuarioFilialPerfil.PessoaFisica.CPF, out objCurriculo);
                            CurriculoObservacao.SalvarCRM("Pagamento Cartão do plano: " + objPlanoAdquirido.IdPlanoAdquirido +
                                 " não realizado, motivo: " + motivo + " - " + resultadoTransacaoResposta, objCurriculo,
                                 "ControleParcelas -> Renovacao Recorrencia");
                        }
                    }
                    //Construindo o email de retorno
                    if (objPlanoAdquirido.Filial.CompleteObject())
                    {
                        objTransacaoUltima.TipoPagamento.CompleteObject();
                        objPlanoAdquirido.Plano.CompleteObject();
                        var resultado = objTransacaoUltima.TipoPagamento.IdTipoPagamento == 8
                            ? "ENVIADO PARA O HSBC"
                            : (retorno ? "SIM" : "NÃO");
                        mensagem += @"<tr><td>" + objPlanoAdquirido.Filial.CNPJ + "</td><td>" +
                                    objPlanoAdquirido.Filial.RazaoSocial + "</td><td>" +
                                    objPlanoAdquirido.Plano.DescricaoPlano + "</td><td>" +
                                    objTransacaoUltima.TipoPagamento.DescricaoTipoPagamaneto + "</td><td>" + resultado +
                                    "</td><td>" + motivo + "</td></tr>";
                    }
                }
                if (listPagamentos.Count > 0)
                {
                    mensagem += "</table>";
                    var objUsuarioFilialPerfil =
                        UsuarioFilialPerfil.LoadObject(
                            Convert.ToInt32(
                                Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.IdUsuarioFilialNotaAntencipada)));
                    MensagemCS.SalvarEmail(null, objUsuarioFilialPerfil, objUsuarioFilialPerfil, null,
                        "Renovação Planos Recorrentes", mensagem, null, "rodrigobandini@bne.com.br",
                        "financeiro@bne.com.br;adrianogoncalves@bne.com.br;rodrigobandini@bne.com.br", string.Empty, null, null);
                }
#if !DEBUG
                EventLogWriter.LogEvent(string.Format("Terminou agora o VencimentoParcelasCartaoDeCredito {0}.", DateTime.Now), EventLogEntryType.Information, Event.FimExecucao);
#endif
            }
            catch (Exception ex)
            {
                string message;
                var id = GerenciadorException.GravarExcecao(ex, out message);
                message = string.Format("{0} - {1}", id, message);
                EventLogWriter.LogEvent(message, EventLogEntryType.Error, Event.ErroExecucao);
            }
        }
        #endregion

        #endregion

        #region Metodos

        #region IniciarControleParcelas
        public void IniciarControleParcelas()
        {
            try
            {
                Settings.Default.Reload();
                HoraExecucao = Settings.Default.ControleParcelasHoraExecucao;
                DelayExecucao = Settings.Default.ControleParcelasDelayMinutos;

#if !DEBUG
                AjustarThread(DateTime.Now, true);
#endif

                while (true)
                {
                    _dataHoraUltimaExecucao = DateTime.Now;

#if !DEBUG
                    EventLogWriter.LogEvent(String.Format("Iniciou agora {0}.", DateTime.Now), EventLogEntryType.Information, Event.InicioExecucao);
#endif

                    Settings.Default.Reload();

                    #region Controle de Notas Fiscais Antecipadas
                    ControleDeEnvioNotaFiscalAntecipada();
                    #endregion

                    #region Controle De Envio De Boleto Com Vencimento Para X dias
                    ControleDeEnvioDeBoletoComVencimentoParaXdias();
                    #endregion

                    #region Controle de Vencimento  Parcelas Recorrentes
                    ControleVencimentoParcelasRecorrencia();
                    #endregion

                    #region Controle de Visualizacoes  Plano WebForPag50
                    ControleVisualizacoesPlanos();
                    #endregion

                    #region VencimentoParcelasCartaoDeCredito
                    VencimentoParcelasCartaoDeCredito();
                    #endregion

                    EventLogWriter.LogEvent(string.Format("Terminou agora {0}.", DateTime.Now),
                        EventLogEntryType.Information, Event.FimExecucao);
                    AjustarThread(DateTime.Now, false);
                }
            }
            catch (Exception ex)
            {
                string message;
                var id = GerenciadorException.GravarExcecao(ex, out message);
                if (string.IsNullOrEmpty(message))
                    return;
                message = string.Format("{0} - {1}", id, message);
                EventLogWriter.LogEvent(message, EventLogEntryType.Error, Event.ErroExecucao);
            }
        }
        #endregion

        #region EnviarArquivoRegistroDeBoletos
        private void EnviarArquivoRegistroDeBoletos()
        {
            var arquivo = Arquivo.GerarArquivo(TipoArquivo.RemessaRegistroBoletos);
            if (arquivo == null)
            {
                return;
            }

            var emailRemetente = Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.ContaPadraoEnvioEmail);
            var emailDestinatario =
                Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.EmailEnvioArquivoRegistroBoleto);
            const string corpoEmail =
                @"Segue anexo o arquivo de remessa para registro de boletos. Envie para o banco, através do Connect Bank, para efetuar o registro.";

            var bytes = arquivo.GetBytes();

            EmailSenderFactory.Create(TipoEnviadorEmail.Fila)
                .Enviar("[BOLETOS REGISTRADOS] Arquivo de Remessa", corpoEmail, null, emailRemetente, emailDestinatario,
                    arquivo.NomeArquivo, bytes);
        }
        #endregion

        #region EnviarArquivoRegistroDeDebitos
        private void EnviarArquivoRegistroDeDebitos()
        {
            var arquivo = Arquivo.GerarArquivo(TipoArquivo.RemessaDebitoHSBC);
            if (arquivo != null)
            {
                var emailRemetente = Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.ContaPadraoEnvioEmail);
                var emailDestinatario =
                    Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.EmailEnvioArquivoRegistroBoleto);
                var corpoEmail =
                    @"Segue anexo o arquivo de remessa para agendamento de débito no banco HSBC. Envie para o banco, através do Connect Bank, para efetuar o agendamento.";

                var bytes = arquivo.GetBytes();

#if DEBUG
                emailDestinatario = "franciscoribas@bne.com.br";
#endif

                EmailSenderFactory.Create(TipoEnviadorEmail.Fila)
                    .Enviar("[DÉBITO HSBC] Arquivo de Remessa", corpoEmail, null, emailRemetente, emailDestinatario,
                        arquivo.NomeArquivo, bytes);
            }
        }
        #endregion

        #region IniciarControleParcelasCartaoDeCredito

        #region Constroi Email de Envio Financeiro
        //private bool ConstroiEmailParaFinanceiro(DataRow row, ref Pagamento objPagamento, int IdPlanoAdquirido,
        //    ref string info_email, out string erro, ref int count)
        //{
        //    var isValid = Transacao.ValidarPagamentoCartaoCredito(ref objPagamento,
        //        IdPlanoAdquirido,
        //        string.Empty,
        //        Convert.ToString(row["Num_Cartao_Credito"]),
        //        Convert.ToInt32(row["Num_Mes_Validade_Cartao_Credito"]),
        //        Convert.ToInt32(row["Num_Ano_Validade_Cartao_Credito"]),
        //        Convert.ToString(row["Num_Codigo_Verificador_Cartao_Credito"]),
        //        Convert.ToString(row["Des_Codigo_Token"]),
        //        out erro);

        //    if (!isValid) //Caso ocorra o problema um email será enviado ao Financeiro
        //    {
        //        info_email += string.Format(@"<b>CNPJ:</b> {0:00\.000\.000\/0000\-00}<br />
        //                                                <b>Razão Social:</b> {1}<br />
        //                                                <b>Data de vencimento do boleto:</b> {2}<br />
        //                                                <b>Nome do destinatário:</b> {3}<br />
        //                                                <b>E-mail do destinatário:</b> {4}<br />
        //                                                <b>Dias em Atraso:</b> {5}/{6} dias <br />
        //                                                <b>Mensagem:</b> {7}<br /><br />",
        //            Convert.ToDecimal(row["Num_CNPJ"]), row["Raz_Social"], row["Dta_Vencimento"], row["Nme_Pessoa"],
        //            row["Eml_Envio_Boleto"], row["Dias_Atraso"], Settings.Default.CartaoCreditoDiasAtraso, erro
        //            );
        //        count++;
        //    }
        //    return isValid;
        //}
        #endregion

        #region BloqueioDeEmpresasEmAtraso
        private void BloqueioDeEmpresasEmAtraso(int IdFilial)
        {
            var parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@IdSituacaoFilial", SqlDbType.Int));
            parms.Add(new SqlParameter("@IdFilial", SqlDbType.Int));

            parms[0].Value = SituacaoFilial.Bloqueado;
            parms[1].Value = IdFilial;

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, SP_UPDATE_STATUS_EMPRESA, parms);
        }
        #endregion

        #endregion

        #region AjustarThread
        private void AjustarThread(DateTime horaAtual, bool primeiraExecucao)
        {
            if (primeiraExecucao)
            {
                var horaMinuto = HoraExecucao.Split(':');

                var horaParaExecucao = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                    Convert.ToInt32(horaMinuto[0]), Convert.ToInt32(horaMinuto[1]), 0);

                if (horaParaExecucao.Subtract(horaAtual).TotalMilliseconds < 0)
                    horaParaExecucao = horaParaExecucao.AddDays(1);

                var tempoParaExecutar = horaParaExecucao - horaAtual;

                EventLogWriter.LogEvent(
                    string.Format("Está aguardando {0} para sua iniciar sua execução.", tempoParaExecutar),
                    EventLogEntryType.Information, Event.AjusteExecucao);
                Thread.Sleep((int)tempoParaExecutar.TotalMilliseconds);
            }
            else
            {
                var tempoTotalExecucao = horaAtual - _dataHoraUltimaExecucao;
                var delay = new TimeSpan(0, DelayExecucao, 0);
                if ((int)delay.TotalMilliseconds - (int)tempoTotalExecucao.TotalMilliseconds > 0)
                {
                    EventLogWriter.LogEvent(
                        string.Format(
                            "Está aguardando {0} para sua próxima execução, pois levou {1} para concluir a excução atual.",
                            delay - tempoTotalExecucao, tempoTotalExecucao), EventLogEntryType.Information,
                        Event.AjusteExecucao);
                    Thread.Sleep((int)delay.TotalMilliseconds - (int)tempoTotalExecucao.TotalMilliseconds);
                }
                else
                    EventLogWriter.LogEvent(
                        string.Format(
                            "Vai reiniciar agora {0} pois sua última execução foi às {1}, seu tempo de execução foi de {2} e seu delay é de {3}.",
                            DateTime.Now, _dataHoraUltimaExecucao, tempoTotalExecucao, (int)delay.TotalMilliseconds),
                        EventLogEntryType.Warning, Event.WarningAjusteExecucao);
            }
        }
        #endregion

        #endregion
    }
}