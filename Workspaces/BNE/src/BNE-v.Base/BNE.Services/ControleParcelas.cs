using BNE.BLL;
using BNE.BLL.Custom;
using BNE.Services.Code;
using BNE.Services.Properties;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.ServiceProcess;
using System.Threading;

namespace BNE.Services
{
    partial class ControleParcelas : ServiceBase
    {

        #region Propriedades
        private Thread _objThread;
        private static DateTime _dataHoraUltimaExecucao;
        private static string HoraExecucao = Settings.Default.ControleParcelasHoraExecucao;
        private static int DelayExecucao = Settings.Default.ControleParcelasDelayMinutos;
        private const string EventSourceName = "ControleParcelas";
        #endregion

        #region Consultas

        #region SpParcelaEmVencimento
        private const string SP_PARCELA_EM_VENCIMENTO = @"SELECT
	                                                        pg.Idf_Pagamento,
	                                                        pp.Idf_Plano_Adquirido,
                                                            pa.Idf_Filial,
	                                                        ultima_transacao.Num_Cartao_Credito,
	                                                        ultima_transacao.Num_Mes_Validade_Cartao_Credito,
	                                                        ultima_transacao.Num_Ano_Validade_Cartao_Credito,
	                                                        ultima_transacao.Num_Codigo_Verificador_Cartao_Credito,
	                                                        ISNULL(pad.Eml_Envio_Boleto, usuario_master.Eml_Comercial) AS 'Eml_Envio_Boleto', 
	                                                        f.Num_CNPJ, 
	                                                        f.Raz_Social, 
	                                                        CONVERT(VARCHAR, pg.Dta_Vencimento, 103) Dta_Vencimento, 
	                                                        pg.Vlr_Pagamento, 
	                                                        ISNULL(pf.Nme_Pessoa, usuario_master.Nme_Pessoa) AS 'Nme_Pessoa',
															DATEDIFF(DAY,CAST(pg.Dta_Vencimento AS DATE),CAST(GETDATE() AS DATE)) AS 'Dias_Atraso'
                                                        FROM [BNE].BNE_Pagamento pg WITH (NOLOCK)
	                                                        JOIN [BNE].BNE_Plano_Parcela pp WITH (NOLOCK)
		                                                        ON pg.Idf_Plano_Parcela = pp.Idf_Plano_Parcela
	                                                        JOIN [BNE].BNE_Plano_Adquirido pa WITH (NOLOCK) 
		                                                        ON pp.Idf_Plano_Adquirido = pa.Idf_Plano_Adquirido
															LEFT JOIN BNE.BNE_Plano AS p WITH(NOLOCK) 
																ON p.Idf_Plano = pa.Idf_Plano
	                                                        LEFT JOIN bne.BNE_Plano_Adquirido_Detalhes pad WITH(NOLOCK) 
		                                                        ON pa.Idf_Plano_Adquirido = pad.Idf_Plano_Adquirido
	                                                        LEFT JOIN BNE.TAB_Filial f WITH(NOLOCK) 
		                                                        ON pa.Idf_Filial = f.Idf_Filial
	                                                        LEFT JOIN BNE.TAB_Usuario_Filial_Perfil ufp WITH(NOLOCK) 
		                                                        ON pa.Idf_Usuario_Filial_Perfil = ufp.Idf_Usuario_Filial_Perfil
	                                                        LEFT JOIN BNE.TAB_Pessoa_Fisica pf WITH(NOLOCK) 
		                                                        ON ufp.Idf_Pessoa_Fisica = pf.Idf_Pessoa_Fisica
															
                                                        OUTER APPLY(
	                                                        SELECT TOP(1) 
                                                                t.Idf_Transacao,
                                                                t.Num_Cartao_Credito,
	                                                            t.Num_Mes_Validade_Cartao_Credito,
	                                                            t.Num_Ano_Validade_Cartao_Credito,
	                                                            t.Num_Codigo_Verificador_Cartao_Credito
                                                        FROM BNE.BNE_Pagamento pg2 WITH(NOLOCK) 
		                                                        JOIN BNE.BNE_Plano_Parcela pp2 WITH(NOLOCK) ON pg2.Idf_Plano_Parcela = pp2.Idf_Plano_Parcela
		                                                        JOIN BNE.BNE_Transacao t  WITH(NOLOCK) ON t.Idf_Pagamento = pg2.Idf_Pagamento
		                                                        WHERE pp2.Idf_Plano_Adquirido = pa.Idf_Plano_Adquirido
		                                                        AND pg2.Idf_Tipo_Pagamento = 1 --Cartão Credito
		                                                        AND pg2.Idf_Pagamento_Situacao = 2 --Pago
		                                                        ORDER BY pp2.Dta_Pagamento DESC
                                                        ) AS ultima_transacao
                                                        OUTER APPLY(
	                                                                SELECT  TOP(1) pf.Nme_Pessoa, UF.Eml_Comercial
	                                                                FROM    BNE.TAB_Usuario_Filial_Perfil UFP WITH (NOLOCK)
			                                                                JOIN BNE.BNE_Usuario_Filial UF WITH(NOLOCK) ON UFP.Idf_Usuario_Filial_Perfil = UF.Idf_Usuario_Filial_Perfil
			                                                                LEFT JOIN BNE.TAB_Pessoa_Fisica PF WITH(NOLOCK) ON UFP.Idf_Pessoa_Fisica = PF.Idf_Pessoa_Fisica
	                                                                WHERE   UFP.Idf_Filial = f.Idf_Filial 
			                                                                AND UFP.Idf_Perfil = 4 --Acesso Empresa Master
			                                                                AND UFP.Flg_Inativo = 0
                                                                            AND pad.Eml_Envio_Boleto IS NULL
                                                                ) usuario_master
                                                        WHERE pg.Idf_Tipo_Pagamento = 1 --Cartão Credito
	                                                        AND pg.Idf_Pagamento_Situacao = 1 --Em Aberto
	                                                        AND ultima_transacao.Idf_Transacao IS NOT NULL
	                                                        AND (CAST(pg.Dta_Vencimento AS DATE) BETWEEN CAST(DATEADD(DAY,-20, GETDATE()) AS DATE) AND CAST( GETDATE() AS DATE))
                                                            AND (f.Idf_Situacao_Filial <> 5 OR f.Idf_Situacao_Filial IS NULL)";
        #endregion

        #region SpUpdateStatusEmpresa
        private const string SP_UPDATE_STATUS_EMPRESA = @"UPDATE TAB_FILIAL SET idf_Situacao_Filial = @IdSituacaoFilial WHERE idf_Filial = @IdFilial";
        #endregion

        #endregion

        #region Construtor
        public ControleParcelas()
        {
            InitializeComponent();
        }
        #endregion

        #region Eventos
        #region OnStart
        protected override void OnStart(string[] args)
        {
            (_objThread = new Thread(new ThreadStart(IniciarControleParcelas))).Start();
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
                    EventLogWriter.LogEvent(EventSourceName, String.Format("Iniciou agora {0}.", DateTime.Now), EventLogEntryType.Information, (int)EventID.InicioExecucao);
#endif

                    Settings.Default.Reload();

                    #region Notas Antecipadas
                    EnvioNotaFiscalAntecipada();
                    #endregion

                    #region VencimentoParcelasCredito
                    VencimentoParcelasCredito();
                    #endregion

                   #region Enviar arquivo de Registro de Boletos
                    //EnviarArquivoRegistroDeBoletos();
                    #endregion

                    #region Enviar arquivo de Agendamento de Débito
                    //EnviarArquivoRegistroDeDebitos();
                    #endregion

                    #region VencimentoParcelasRecorrentesCartaoCredito
                    VencimentoParcelasCartaoDeCredito();
                    #endregion

#if !DEBUG
                    EventLogWriter.LogEvent(EventSourceName, String.Format("Terminou agora {0}.", DateTime.Now), EventLogEntryType.Information, (int)EventID.FimExecucao);
                    AjustarThread(DateTime.Now, false);
#endif
                }
            }
            catch (Exception ex)
            {
                string message;
                var id = EL.GerenciadorException.GravarExcecao(ex, out message);
                if (string.IsNullOrEmpty(message))
                    return;
                message = string.Format("{0} - {1}", id, message);
                EventLogWriter.LogEvent(EventSourceName, message, EventLogEntryType.Error, (int)EventID.ErroExecucao);
            }
        }
        #endregion

        #region EnvioNotaFiscalAntecipada
        private void EnvioNotaFiscalAntecipada()
        {

            try
            {
                DateTime considerarData;
                if (!DateTime.TryParse(Properties.Settings.Default.ControleParcelasConsiderarData, out considerarData))
                    considerarData = DateTime.Now;

                using (var dt = PlanoParcela.ListarParcelasEmissaoNotaFiscalAntecipada(considerarData))
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        try
                        {
                            int n;
                            string emailDestinatario = row["Eml_Envio_Boleto"].ToString();

#if DEBUG
                            emailDestinatario = "franciscoribas@bne.com.br";
#endif

                            if (string.IsNullOrWhiteSpace(emailDestinatario))
                                throw new Exception("ControleParcelasAVencer: Email vazio");
                            if (!int.TryParse(row["Idf_Pagamento"].ToString(), out n))
                                throw new Exception("ControleParcelasAVencer: Pagamento inválido");

                            var objPagamento = Pagamento.LoadObject(n);

                            int idUsuarioFilialPerfilLogado = Convert.ToInt32(BLL.Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.IdUsuarioFilialNotaAntencipada));

                            if (string.IsNullOrWhiteSpace(objPagamento.NumeroNotaFiscal))
                                BLL.PlanoParcela.EmitirNF(objPagamento, idUsuarioFilialPerfilLogado, null);
                        }
                        catch (Exception ex)
                        {
                            string message;
                            var id = EL.GerenciadorException.GravarExcecao(ex, out message);
                            message = string.Format("{0} - {1}", id, message);
#if !DEBUG
                            EventLogWriter.LogEvent(EventSourceName, message, EventLogEntryType.Error, (int)EventID.ErroExecucao);
#endif
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string message;
                var id = EL.GerenciadorException.GravarExcecao(ex, out message);
                message = string.Format("{0} - {1}", id, message);
#if !DEBUG
                EventLogWriter.LogEvent(EventSourceName, message, EventLogEntryType.Error, (int)EventID.ErroExecucao);
#endif
            }
        }
        #endregion

        #region VencimentoParcelasCredito
        private void VencimentoParcelasCredito()
        {
            var valores = new List<Tuple<int, int>>();
            try
            {
                valores = PlanoParcela.ListarParcelasVencimentoCredito();
            }
            catch (Exception ex)
            {
                string message;
                var id = EL.GerenciadorException.GravarExcecao(ex, out message);
                message = string.Format("{0} - {1}", id, message);
                EventLogWriter.LogEvent(EventSourceName, message, EventLogEntryType.Error, (int)EventID.ErroExecucao);
                return;
            }
            foreach (var item in valores)
            {
                try
                {
                    int IdPlanoAdquirido = item.Item1;
                    int IdPagamento = item.Item2;

                    var objPagamento = Pagamento.LoadObject(IdPagamento);
                    var objTransacao = Transacao.CarregarUltimaPorPlanoAdquirido(IdPlanoAdquirido);
                    String erro;
                    Transacao.ValidarPagamentoCartaoCredito(ref objPagamento,
                                                            IdPlanoAdquirido,
                                                            String.Empty,
                                                            objTransacao.NumeroCartaoCredito,
                                                            Convert.ToInt32(objTransacao.NumeroMesValidadeCartaoCredito),
                                                            Convert.ToInt32(objTransacao.NumeroAnoValidadeCartaoCredito),
                                                            objTransacao.NumeroCodigoVerificadorCartaoCredito,
                                                            out erro);
                }
                catch (Exception ex)
                {
                    string message;
                    var id = EL.GerenciadorException.GravarExcecao(ex, out message);
                    message = string.Format("{0} - {1}", id, message);
                    EventLogWriter.LogEvent(EventSourceName, message, EventLogEntryType.Error, (int)EventID.ErroExecucao);
                }
            }
        }
        #endregion

        #region EnviarArquivoRegistroDeBoletos
        private void EnviarArquivoRegistroDeBoletos()
        {
            Arquivo arquivo = Arquivo.GerarArquivo(BLL.Enumeradores.TipoArquivo.RemessaRegistroBoletos);
            if (arquivo == null)
            {
                return;
            }

            String emailRemetente = Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.ContaPadraoEnvioEmail);
            String emailDestinatario = Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.EmailEnvioArquivoRegistroBoleto);
            const string corpoEmail = @"Segue anexo o arquivo de remessa para registro de boletos. Envie para o banco, através do Connect Bank, para efetuar o registro.";

            byte[] bytes = arquivo.GetBytes();

            MailController.Send(emailDestinatario, emailRemetente, "[BOLETOS REGISTRADOS] Arquivo de Remessa", corpoEmail, new Dictionary<string, byte[]> { { arquivo.NomeArquivo, bytes } });
        }
        #endregion

        #region EnviarArquivoRegistroDeDebitos
        private void EnviarArquivoRegistroDeDebitos()
        {
            Arquivo arquivo = Arquivo.GerarArquivo(BLL.Enumeradores.TipoArquivo.RemessaDebitoHSBC);
            if (arquivo != null)
            {
                String emailRemetente = Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.ContaPadraoEnvioEmail);
                String emailDestinatario = Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.EmailEnvioArquivoRegistroBoleto);
                String corpoEmail = @"Segue anexo o arquivo de remessa para agendamento de débito no banco HSBC. Envie para o banco, através do Connect Bank, para efetuar o agendamento.";

                byte[] bytes = arquivo.GetBytes();

#if DEBUG
                emailDestinatario = "franciscoribas@bne.com.br";
#endif

                MailController.Send(emailDestinatario, emailRemetente, "[DÉBITO HSBC] Arquivo de Remessa", corpoEmail, new Dictionary<string, byte[]> { { arquivo.NomeArquivo, bytes } });
            }
        }

        #endregion

        #region IniciarControleParcelasCartaoDeCredito

        #region VencimentoParcelasCartaoDeCredito
        private void VencimentoParcelasCartaoDeCredito()
        {
            try
            {
                string emailDestinatario = Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.ControleParcelasRemetente);
#if DEBUG
                emailDestinatario = "ruas@bne.com.br";
#endif

                if (string.IsNullOrWhiteSpace(emailDestinatario))
                    throw new Exception("ControleParcelasAVencer: Email vazio");

                var listaDeBloqueio = new List<string>();
                string info_email = "";
                int count = 0;

                using (var valores = ListaInformacoesDeParcelasCartaoDeCreditoEmVencimento())
                {
                    foreach (DataRow row in valores.Rows)
                    {
                        int IdPlanoAdquirido = Convert.ToInt32(row["Idf_Plano_Adquirido"]);
                        int IdPagamento = Convert.ToInt32(row["Idf_Pagamento"]);
                        var objPagamento = Pagamento.LoadObject(IdPagamento);
                        String erro;

                        //Caso o tenha vencido o prazo definido ele bloqueia a empresa
                        //Se for o mesmo dia ele envia o boleto para pagamento e informa o mesmo a situação
                        if (Convert.ToInt16(row["Dias_Atraso"]) > Settings.Default.CartaoCreditoDiasAtraso)
                            BloqueioDeEmpresasEmAtraso(Convert.ToInt32(row["Idf_Filial"]));
                        //Os Cartões de Crédito que estiverem no periodo de atraso aceito, 
                        //todos serão efetuado o pagamento e após enviado um email com os que conterem erro ao Financeiro
                        else
                        {
                            var isvalid = ConstroiEmailParaFinanceiro(row, ref objPagamento, IdPlanoAdquirido, ref info_email, out erro, ref count);
                            //Melhoria para boleto no último dia do vencimento enviar para o Cliente o boleto
                            if ((!isvalid) && (Convert.ToUInt16(row["Dias_Atraso"]) == Settings.Default.CartaoCreditoDiasAtraso))
                            {
                                String retorno = CobrancaBoleto.GerarBoleto(objPagamento);
                                BLL.DTO.CartaEmail cartaEmail = CartaEmail.RecuperarCarta(BLL.Enumeradores.CartaEmail.BloqueioNaoPagamentoCartaoDeCredito);
                                string textoEmail = cartaEmail.Conteudo.Replace("{Nome}", row["Nme_Pessoa"].ToString()).Replace("{URLPagamento}", retorno).Replace("{erro}", erro);
                                MensagemCS.SalvarEmail(null, null, null, null, cartaEmail.Assunto, textoEmail, Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.ControleParcelasRemetente), row["Eml_Envio_Boleto"].ToString(), null, null, null);
                            }
                        }
                    }
                }
                if (count > 0)
                    MensagemCS.SalvarEmail(null, null, null, null, "Transações não autorizadas", info_email, null, emailDestinatario, null, null, null);
            }
            catch (Exception ex)
            {
                string message;
                var id = EL.GerenciadorException.GravarExcecao(ex, out message);
                message = string.Format("{0} - {1}", id, message);
                EventLogWriter.LogEvent(EventSourceName, message, EventLogEntryType.Error, (int)EventID.ErroExecucao);
            }
        }
        #endregion

        #region Constroi Email de Envio Financeiro
        private bool ConstroiEmailParaFinanceiro(DataRow row, ref Pagamento objPagamento, int IdPlanoAdquirido, ref string info_email, out string erro, ref int count)
        {
            bool isValid = Transacao.ValidarPagamentoCartaoCredito(ref objPagamento,
                                                                    IdPlanoAdquirido,
                                                                    String.Empty,
                                                                    Convert.ToString(row["Num_Cartao_Credito"]),
                                                                    Convert.ToInt32(row["Num_Mes_Validade_Cartao_Credito"]),
                                                                    Convert.ToInt32(row["Num_Ano_Validade_Cartao_Credito"]),
                                                                    Convert.ToString(row["Num_Codigo_Verificador_Cartao_Credito"]),
                                                                    out erro);

            if (!isValid) //Caso ocorra o problema um email será enviado ao Financeiro
            {
                info_email += string.Format(@"<b>CNPJ:</b> {0:00\.000\.000\/0000\-00}<br />
                                                        <b>Razão Social:</b> {1}<br />
                                                        <b>Data de vencimento do boleto:</b> {2}<br />
                                                        <b>Nome do destinatário:</b> {3}<br />
                                                        <b>E-mail do destinatário:</b> {4}<br />
                                                        <b>Dias em Atraso:</b> {5}/{6} dias <br />
                                                        <b>Mensagem:</b> {7}<br /><br />",
                                        Convert.ToDecimal(row["Num_CNPJ"]), row["Raz_Social"], row["Dta_Vencimento"], row["Nme_Pessoa"], row["Eml_Envio_Boleto"], row["Dias_Atraso"], Settings.Default.CartaoCreditoDiasAtraso, erro
                                        );
                count++;
            }
            return isValid;
        }
        #endregion

        #region ListaInformacoesDeParcelasCartaoDeCreditoEmVencimento
        public static DataTable ListaInformacoesDeParcelasCartaoDeCreditoEmVencimento()
        {
            DataTable dt = new DataTable();
            string SQL = SP_PARCELA_EM_VENCIMENTO.Replace("@DiasAtraso", Convert.ToString(Settings.Default.PeriodoListagemCartaoCreditoDiasAtraso * -1));
            SqlDataAdapter da = new SqlDataAdapter(SQL, DataAccessLayer.CONN_STRING);
            da.Fill(dt);
            return dt;
        }
        #endregion

        #region BloqueioDeEmpresasEmAtraso
        private void BloqueioDeEmpresasEmAtraso(int IdFilial)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@IdSituacaoFilial", SqlDbType.Int));
            parms.Add(new SqlParameter("@IdFilial", SqlDbType.Int));

            parms[0].Value = BLL.Enumeradores.SituacaoFilial.Bloqueado;
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
                string[] horaMinuto = HoraExecucao.Split(':');

                var horaParaExecucao = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Convert.ToInt32(horaMinuto[0]), Convert.ToInt32(horaMinuto[1]), 0);

                if (horaParaExecucao.Subtract(horaAtual).TotalMilliseconds < 0)
                    horaParaExecucao = horaParaExecucao.AddDays(1);

                var tempoParaExecutar = horaParaExecucao - horaAtual;

                EventLogWriter.LogEvent(EventSourceName, String.Format("Está aguardando {0} para sua iniciar sua execução.", tempoParaExecutar), EventLogEntryType.Information, (int)EventID.AjusteExecucao);
                Thread.Sleep((int)tempoParaExecutar.TotalMilliseconds);
            }
            else
            {
                var tempoTotalExecucao = horaAtual - _dataHoraUltimaExecucao;
                var delay = new TimeSpan(0, DelayExecucao, 0);
                if ((int)delay.TotalMilliseconds - (int)tempoTotalExecucao.TotalMilliseconds > 0)
                {
                    EventLogWriter.LogEvent(EventSourceName, String.Format("Está aguardando {0} para sua próxima execução, pois levou {1} para concluir a excução atual.", delay - tempoTotalExecucao, tempoTotalExecucao), EventLogEntryType.Information, (int)EventID.AjusteExecucao);
                    Thread.Sleep((int)delay.TotalMilliseconds - (int)tempoTotalExecucao.TotalMilliseconds);
                }
                else
                    EventLogWriter.LogEvent(EventSourceName, String.Format("Vai reiniciar agora {0} pois sua última execução foi às {1}, seu tempo de execução foi de {2} e seu delay é de {3}.", DateTime.Now, _dataHoraUltimaExecucao, tempoTotalExecucao, (int)delay.TotalMilliseconds), EventLogEntryType.Warning, (int)EventID.WarningAjusteExecucao);
            }
        }
        #endregion

        #endregion
    }
}
